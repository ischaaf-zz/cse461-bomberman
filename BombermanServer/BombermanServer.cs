using BombermanObjects;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BombermanServer
{
    public class BombermanServer
    {
        public static readonly string LOGIN_MSG = "Login message";
        private static readonly int TRIES = 10;

        public bool gameActive;
        ServerGameManager manager;
        NetPeerConfiguration config;
        public NetServer server;
        int totalPlayers;
        int playersConnected;
        NetConnection[] playerConnections;
        public PlayerInfo[] playerInfoArr;
        ServerGameRunner runner;
        Stack<int> unassignedPlayerIds; // 1 based

        public BombermanServer(int players, int port)
        {
            gameActive = false;
            config = new NetPeerConfiguration("game");
            config.Port = port;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            server = new NetServer(config);

            playerInfoArr = new PlayerInfo[players];

            manager = new ServerGameManager(server, playerInfoArr, players);
            manager.Initialize();
            runner = new ServerGameRunner(manager, this);
            totalPlayers = players;
            playersConnected = 0;
            playerConnections = new NetConnection[players];
            unassignedPlayerIds = new Stack<int>();
            for (int i = players; i >= 0; i--)
            {
                unassignedPlayerIds.Push(i + 1);
            }
            server.Configuration.ConnectionTimeout = 5;
        }
        
        public static void FillOutMsgHeader(NetOutgoingMessage outmsg, int sid, PacketTypeEnums.PacketType packetType)
        {
            outmsg.Write((byte)sid);
            outmsg.Write((byte)packetType);
        }

        public void Start()
        {
            server.Start();
            Console.WriteLine("waiting for client connections");
            while (!gameActive)
            {

                //server.GetConnection
                NetIncomingMessage message;
                while ((message = server.ReadMessage()) != null && !gameActive)
                {
                    for (int i = 0; i < totalPlayers; i++)
                    {
                        if (playerInfoArr[i] != null)
                        {
                            NetOutgoingMessage outmsg = server.CreateMessage();
                            FillOutMsgHeader(outmsg, 0, PacketTypeEnums.PacketType.CLIENT_IS_ALIVE);
                            server.SendMessage(outmsg, playerInfoArr[i].playerConnection, NetDeliveryMethod.ReliableSequenced, 0);
                            //Console.WriteLine($"player {i + 1} status: {playerInfoArr[i].playerConnection.Status}");
                        }
                    }
                    Thread.Sleep(100);
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            Console.WriteLine("approving incoming connection");
                            Console.WriteLine($"Message Type: {message.SenderConnection.Status}");
                            if (playersConnected < totalPlayers)
                            {
                                message.SenderConnection.Approve();
                            }
                            else
                            {
                                Console.WriteLine($"Already at max number of players, denying {message.SenderConnection}");
                                message.SenderConnection.Deny();
                                break;
                            }
                            var data = message.ReadString();
                            if (data.Equals(LOGIN_MSG))
                            {
                                playersConnected++;
                                int currPlayerId = unassignedPlayerIds.Pop();
                                manager.AddPlayer(currPlayerId);
                                NetConnection playerConnection = message.SenderConnection;
                                playerInfoArr[currPlayerId - 1] = new PlayerInfo(playerConnection, playersConnected, playerConnection.AverageRoundtripTime);

                                NetOutgoingMessage outmsg = server.CreateMessage();
                                // senderID, PacketType, ID
                                FillOutMsgHeader(outmsg, 0, PacketTypeEnums.PacketType.SEND_PLAYER_ID);
                                outmsg.Write((byte)currPlayerId);
                                Console.WriteLine("accepted Connection from: " + playerConnection);
                                Console.WriteLine("assigning playerID: " + currPlayerId);
                                Console.WriteLine($"broadcasting to player {currPlayerId} the following connected players:");
                                for (int i = 0; i < totalPlayers; i++)
                                {
                                    if (playerInfoArr[i] != null && currPlayerId - 1 != i)
                                    {
                                        outmsg.Write((byte)(i + 1));
                                        Console.Write($"player {i + 1}, ");
                                    }
                                }
                                Console.WriteLine();
                                outmsg.Write((byte)0xff);
                                // we don't want this to be lost, so set level to ReliableOrdered
                                Console.WriteLine($"Connection Status: {playerConnection.Status}");
                                Thread.Sleep(100);
                                server.SendMessage(outmsg, playerConnection, NetDeliveryMethod.ReliableOrdered, 0);
                                Thread.Sleep(100);
                                server.SendMessage(manager.GetFullGameState(), playerConnection, NetDeliveryMethod.ReliableOrdered, 0);
                                server.FlushSendQueue();
                                

                                
                                for (int i = 0; i < totalPlayers; i++)
                                {
                                    if (playerInfoArr[i] != null && i != currPlayerId - 1)
                                    {
                                        Console.WriteLine($"Broadcasting to connected player {i + 1} that new player {currPlayerId} has connected");
                                        NetOutgoingMessage newPlayerMsg = server.CreateMessage();
                                        FillOutMsgHeader(newPlayerMsg, 0, PacketTypeEnums.PacketType.NEW_PLAYER_ID);
                                        newPlayerMsg.Write((byte)currPlayerId);
                                        server.SendMessage(newPlayerMsg, playerInfoArr[i].playerConnection, NetDeliveryMethod.ReliableOrdered, 0);
                                    }   
                                }
                            }
                            break;

                        case NetIncomingMessageType.Data:
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            // handle connection status messages
                            Console.WriteLine($"status change: {message.ReadString()}");
                            for (int i = 0; i < totalPlayers; i++)
                            {
                                //Console.WriteLine($"hi{i}")

                                if (playerInfoArr[i] != null &&
                                    (playerInfoArr[i].playerConnection.Status != NetConnectionStatus.Connected &&
                                    playerInfoArr[i].playerConnection.Status != NetConnectionStatus.RespondedConnect &&
                                    playerInfoArr[i].playerConnection.Status != NetConnectionStatus.RespondedAwaitingApproval) &&
                                    !unassignedPlayerIds.Contains(i + 1))
                                {
                                    Console.WriteLine($"Player {i + 1} has disconnected, status: {playerInfoArr[i].playerConnection.Status}");
                                    playersConnected--;
                                    manager.DeletePlayer(i + 1);
                                    playerInfoArr[i] = null;
                                    unassignedPlayerIds.Push(i + 1);
                                    for (int j = 0; j < totalPlayers; j++)
                                    {
                                        if (playerInfoArr[j] != null && j != i)
                                        {
                                            Console.WriteLine($"broadcasting to player {j + 1} that player {i + 1} has disconnected");
                                            NetOutgoingMessage outmsg = server.CreateMessage();
                                            FillOutMsgHeader(outmsg, 0, PacketTypeEnums.PacketType.PLAYER_DISCONNECTION);
                                            outmsg.Write((byte)(i + 1));
                                            server.SendMessage(outmsg, playerInfoArr[j].playerConnection, NetDeliveryMethod.ReliableOrdered, 0);
                                        }
                                    }
                                }
                            }
                            break;

                        case NetIncomingMessageType.DebugMessage:
                            // handle debug messages
                            // (only received when compiled in DEBUG mode)
                            Console.WriteLine(message.ReadString());
                            break;

                        /* .. */
                        default:
                            Console.WriteLine("unhandled message with type: "
                                + message.MessageType);
                            Console.WriteLine(message.ReadString());
                            break;
                    }
                    
                    if (playersConnected >= totalPlayers)
                    {
                        gameActive = true;
                    }
                }
            }
            Console.WriteLine("Waiting to start...");
            Console.WriteLine("5");
            Thread.Sleep(1000);
            Console.WriteLine("4");
            Thread.Sleep(1000);
            Console.WriteLine("3");
            Thread.Sleep(1000);
            Console.WriteLine("2");
            Thread.Sleep(1000);
            Console.WriteLine("1");
            Thread.Sleep(1000);
            Console.WriteLine("Starting game...");

            //for (int i = 0; i < playersConnected; i++)
            //{
            //    Console.WriteLine($"Broadcasting to connected player {i + 1} that game has started");
            //    NetOutgoingMessage newPlayerMsg = server.CreateMessage();
            //    newPlayerMsg.Write((byte)0);
            //    newPlayerMsg.Write((byte)PacketTypeEnums.PacketType.GAME_START);
            //    server.SendMessage(newPlayerMsg, playerInfoArr[i].playerConnection, NetDeliveryMethod.ReliableOrdered, 0);
            //}
            runner.Run();
            // while connected clients less than players
            //  accept connection
            //  receive message and check if it an actual client, special message.
            //  if actual client, assign an id, and send that id to the client
            //      create the network interface between server and client with client network info
            //      store this info in an array

            //      manager.setInputEventSource(...)
            //      manager.setOutputEventSource(...)

            // after while loop, game manager is all setup
            // now create ServerBroadcastThread and pass in network interface info
            //  and also a reference to queue of events generated by the server that
            //  needs to be sent over to the clients. ServeBroadcastThread will poll
            //  events from the queue and broadcast to all clients.
            //
            // BROADCAST GAME START. (3 times 1 second intervals)
            //
            // while(true)
            //  for each client network interface
            //      if HasNext
            //          pollEvent (x events)
            //          verifyEventsAreLegal thru game manager
            //          update game manager state
            //          poll game manager for any special events(player death, player x wins, etc)
            //              if (win)
            //                  break????
            //          enqueue valid events into queues
            //          
        }

        public void Update(GameTime gameTime)
        {
            NetIncomingMessage inc;
            for (int i = 0; i < TRIES; i++)
            {
                if ((inc = server.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {


                        case NetIncomingMessageType.Data:
                            // handle custom messages
                            //Console.WriteLine("incoming data");
                            int senderID = inc.ReadByte();
                            if (senderID <= 0 || senderID > playersConnected)
                            {
                                Console.WriteLine("invalid sendID received, discarding packet");
                                break;
                            }
                            byte packetType = inc.ReadByte();
                            if (packetType == (byte)PacketTypeEnums.PacketType.EVENT)
                            {
                                HandleEvent(gameTime, inc, senderID);
                            }
                            else
                            {
                                Console.WriteLine("received unknown packet from client");
                            }
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            // handle connection status messages
                            Console.WriteLine("status change");
                            switch (inc.SenderConnection.Status)
                            {

                            }
                            break;

                        case NetIncomingMessageType.DebugMessage:
                            // handle debug messages
                            // (only received when compiled in DEBUG mode)
                            Console.WriteLine(inc.ReadString());
                            break;

                        /* .. */
                        default:
                            Console.WriteLine("unhandled message with type: "
                                + inc.MessageType);
                            break;
                    }
                }
            }
        }

        public void HandleEvent(GameTime gameTime, NetIncomingMessage inc, int playerID)
        {
            byte eventType = inc.ReadByte();
            if (eventType == (byte)PacketTypeEnums.EventType.EVENT_MOVE)
            {
                BombermanObjects.Logical.Player.Direction dir = (BombermanObjects.Logical.Player.Direction)inc.ReadByte();
                //Console.WriteLine($"moving player {playerID} {dir}");
                manager.MovePlayer(playerID - 1, dir);
            } else if (eventType == (byte)PacketTypeEnums.EventType.EVENT_BOMB_PLACEMENT)
            {
                //Console.WriteLine($"Player {playerID} has placed a bomb");
                manager.players[playerID - 1].placeBomb(gameTime);
            }
        }
    }
}
