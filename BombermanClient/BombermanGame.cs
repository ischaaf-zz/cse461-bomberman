﻿using BombermanObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;
using BombermanObjects.Logical;

namespace BombermanClient
{
    class BombermanGame : Game
    {

        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spritebatch;
        protected GraphicalGameManager manager;
        protected int playerId;
        protected int totalPlayers;
        LocalInput input = new LocalInput();
        NetConnection serverConnection;

        NetClient client;
        private bool gameStarted;
        private string hostIP { get; set; }
        private int port { get; set; }

        public BombermanGame(string hostIp, int port) : base()
        {
            hostIP = hostIp;
            this.port = port;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GameManager.BOX_WIDTH * GameManager.GAME_SIZE;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = GameManager.BOX_WIDTH * GameManager.GAME_SIZE;   // set this value to the desired height of your window
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.Window.AllowUserResizing = false;

            gameStarted = false;
            NetPeerConfiguration config = new NetPeerConfiguration("game");
            client = new NetClient(config);

            client.Start();

            NetOutgoingMessage outmsg = client.CreateMessage();
            outmsg.Write("Login message");


            serverConnection = client.Connect(hostIP, port, outmsg);
            Thread.Sleep(2000);
            bool awaitingAssignment = true;
            while (awaitingAssignment)
            {
                NetIncomingMessage inc;
                if ((inc = client.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            int sid = inc.ReadByte();
                            PacketTypeEnums.PacketType type = (PacketTypeEnums.PacketType)(inc.ReadByte());
                            Console.WriteLine($"Packet Type: {inc.MessageType} SID: {sid} type: {type}");
                            if (type == PacketTypeEnums.PacketType.SEND_PLAYER_ID)
                            {
                                playerId = inc.ReadByte();
                                for (int i = 1; i <= playerId; i++)
                                {
                                    manager.AddPlayer(i);
                                }
                                Console.WriteLine($"Assigned player: {playerId}");
                                totalPlayers = playerId;
                            }
                            awaitingAssignment = false;
                            break;
                        case NetIncomingMessageType.StatusChanged:

                            break;
                        default:
                            Console.WriteLine($"Unknown Message: Type: {inc.MessageType} with data: {inc.ReadString()}");
                            break;
                    }
                }
            }

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spritebatch = new SpriteBatch(GraphicsDevice);
            Dictionary<string, Texture2D> textureMap = new Dictionary<string, Texture2D>();
            textureMap["background"] = Content.Load<Texture2D>("background");
            textureMap["wall"] = Content.Load<Texture2D>("wall");
            textureMap["player"] = Content.Load<Texture2D>("player");
            textureMap["bomb"] = Content.Load<Texture2D>("bomb");
            textureMap["explosion"] = Content.Load<Texture2D>("explosion");
            textureMap["box"] = Content.Load<Texture2D>("box");
            textureMap["powerups"] = Content.Load<Texture2D>("powerups");
            manager = new GraphicalGameManager(4, textureMap);
            manager.InitializeBare();
        }

        protected override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (gameStarted)
            {
                input.Update(gameTime);
                var current = input.CurrentInput;
                var curM = current.Move.Length > 0 ? current.Move[0] : Player.Direction.Center;
                if (manager.players[playerId - 1].MoveDirection != curM)
                {
                    // send Move;
                    NetOutgoingMessage moveMsg = client.CreateMessage();
                    moveMsg.Write((byte)playerId);
                    moveMsg.Write((byte)PacketTypeEnums.PacketType.EVENT);
                    moveMsg.Write((byte)PacketTypeEnums.EventType.EVENT_MOVE);
                    moveMsg.Write((byte)curM);
                    client.SendMessage(moveMsg, serverConnection, NetDeliveryMethod.Unreliable, 0);
                }
                if (current.PlaceBomb)
                {
                    // send Bomb
                    NetOutgoingMessage bombMsg = client.CreateMessage();
                    bombMsg.Write((byte)playerId);
                    bombMsg.Write((byte)PacketTypeEnums.PacketType.EVENT);
                    bombMsg.Write((byte)PacketTypeEnums.EventType.EVENT_BOMB_PLACEMENT);
                    client.SendMessage(bombMsg, serverConnection, NetDeliveryMethod.Unreliable, 0);
                }
            }

            NetIncomingMessage inc;
            if ((inc = client.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        int sid = inc.ReadByte();
                        PacketTypeEnums.PacketType type = (PacketTypeEnums.PacketType)(inc.ReadByte());
                        Console.WriteLine($"Packet Type: {inc.MessageType} SID: {sid} type: {type}");

                        switch (type)
                        {
                            case PacketTypeEnums.PacketType.SEND_PLAYER_ID:
                                Console.WriteLine("ID has already been assigned");
                                break;
                            case PacketTypeEnums.PacketType.NEW_PLAYER_ID:
                                totalPlayers++;
                                Console.WriteLine($"New player added, {totalPlayers} players total");
                                manager.AddPlayer(inc.ReadByte());
                                break;
                            case PacketTypeEnums.PacketType.EVENT:
                                Console.WriteLine("Event received");
                                break;
                            case PacketTypeEnums.PacketType.GAME_START:
                                gameStarted = true;
                                Console.WriteLine("Game starting...");
                                break;
                            case PacketTypeEnums.PacketType.GAME_STATE:
                                Console.WriteLine("Game state received");
                                UpdateGameState(inc);
                                break;
                            case PacketTypeEnums.PacketType.GAME_STATE_FULL:
                                while (inc.PeekByte() != (byte)0xff)
                                {
                                    byte x = inc.ReadByte();
                                    byte y = inc.ReadByte();
                                    PowerUp.PowerUpType powerup = (PowerUp.PowerUpType)inc.ReadByte();
                                    manager.PlaceBox(x, y, powerup);
                                }
                                
                                
                                break;
                            default:
                                break;
                        }

                        //Console.WriteLine($"Unknown Message: Type: {inc.MessageType} with data: {inc.ReadString()}");
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
            }


            if (gameStarted)
            {
                manager.Update(gameTime);
            }
            // TODO Update game logic
            base.Update(gameTime);
        }

        private void UpdateGameState(NetIncomingMessage inc)
        {
            // read player info
            for (int i = 0; i < totalPlayers; i++)
            {
                int speed = inc.ReadByte();
                int lives = inc.ReadByte();
                int maxBombs = inc.ReadByte();
                int placedBombs = inc.ReadByte();
                int bombPower = inc.ReadByte();
                long immune = inc.ReadVariableInt64();
                Player.Direction dir = (Player.Direction)inc.ReadByte();
                int x = inc.ReadVariableInt32();
                int y = inc.ReadVariableInt32();
                manager.OverridePlayer(i, lives, speed, maxBombs, bombPower, placedBombs, immune, dir, new Rectangle(x, y, 64, 64));
            }

            // read bomb info
            while (inc.PeekByte() != 0xff)
            {
                int player = inc.ReadByte();
                int x = inc.ReadByte();
                int y = inc.ReadByte();
                long detTime = inc.ReadVariableInt64();
                manager.PlaceBombOrUpdate(player, x, y, detTime);
            }
            inc.ReadByte();

            // read box info
            while (inc.PeekByte() != 0xff)
            {
                int x = inc.ReadByte();
                int y = inc.ReadByte();
                if (manager.statics.IsItemAtPoint(new Point(x, y)))
                {
                    manager.DestroyBox(x, y);
                }
            }
            inc.ReadByte();
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO Update graphics
            spritebatch.Begin();
            manager.Draw(spritebatch, gameTime);

            if (!gameStarted)
            {
                
            }

            spritebatch.End();

            base.Draw(gameTime);
        }
    }
}
