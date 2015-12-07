using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using BombermanObjects;
using Microsoft.Xna.Framework;

namespace BombermanServer
{
    public class BombermanServer
    {
        public static readonly String LOGIN_MSG = "Login message";
        public static readonly int SERVER_PORT = 12346;
        public Boolean gameActive;
        GameManager manager;
        NetPeerConfiguration config;
        NetServer server;
        int totalPlayers;
        int playersConnected;
        NetConnection[] playerConnections;
        PlayerInfo[] playerInfoArr;


        public BombermanServer(int players)
        {
            gameActive = false;
            config = new NetPeerConfiguration("game");
            config.Port = SERVER_PORT;

            server = new NetServer(config);

            manager = new GameManager(players);
            totalPlayers = players;
            playersConnected = 0;
            playerConnections = new NetConnection[players];
            playerInfoArr = new PlayerInfo[players];
        }
        
        public void Start()
        {
            server.Start();
            while (!gameActive)
            {
                //server.GetConnection
                NetIncomingMessage message;
                while ((message = server.ReadMessage()) != null && !gameActive)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            // handle custom messages
                            var data = message.ReadString();
                            if (data.Equals(LOGIN_MSG))
                            {
                                playersConnected++;
                                NetConnection playerConnection = message.SenderConnection;
                                playerInfoArr[playersConnected - 1] = new PlayerInfo(playerConnection, playersConnected, playerConnection.AverageRoundtripTime);

                                NetOutgoingMessage outmsg = server.CreateMessage();
                                outmsg.Write((byte)PacketTypeEnums.PacketType.SEND_PLAYER_ID);
                                outmsg.WriteVariableInt32(playersConnected);
                                // we don't want this to be lost, so set level to ReliableOrdered
                                server.SendMessage(outmsg, playerConnection, NetDeliveryMethod.ReliableOrdered, 0);
                                Console.WriteLine("accepted Connection from: " + playerConnection);
                                Console.WriteLine("assigning playerID: " + playersConnected);
                            }
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            // handle connection status messages
                            switch (message.SenderConnection.Status)
                            {
                                case NetConnectionStatus.RespondedConnect:
                                    message.SenderConnection.Approve();
                                    break;
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
                            break;
                    }
                    if (playersConnected >= totalPlayers)
                    {
                        gameActive = true;
                    }
                }
            }
            NetIncomingMessage inc;
            while (true)
            {
                if ((inc = server.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            // handle custom messages
                            int senderID = inc.ReadByte();
                            if (senderID <= 0 || senderID > playersConnected)
                            {
                                Console.WriteLine("invalid sendID received, discarding packet");
                                break;
                            }
                            byte packetType = inc.ReadByte();
                            if (packetType == (byte) PacketTypeEnums.PacketType.EVENT)
                            {
                                HandleEvent(inc, senderID);
                            } else
                            {
                                Console.WriteLine("received unknown packet from client");                             
                            }
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            // handle connection status messages
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
        public void HandleEvent(NetIncomingMessage inc, int playerID)
        {
            byte eventType = inc.ReadByte();
            if (eventType == (byte)PacketTypeEnums.EventType.EVENT_MOVE)
            {
                BombermanObjects.Logical.Player.Direction dir = (BombermanObjects.Logical.Player.Direction)inc.ReadInt32();
                int dist = inc.ReadInt32();
                manager.players[playerID - 1].move(dir, dist);

            } else if (eventType == (byte)PacketTypeEnums.EventType.EVENT_BOMB_PLACEMENT)
            {
                GameTime gameTime = new GameTime();
                inc.ReadAllProperties(gameTime);
                manager.players[playerID - 1].placeBomb(gameTime);

            }
        }
    }
}
