using BombermanObjects;
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


            NetConnection connection = client.Connect(hostIP, port, outmsg);
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
                            int sid = inc.ReadVariableInt32();
                            PacketTypeEnums.PacketType type = (PacketTypeEnums.PacketType)(inc.ReadByte());
                            Console.WriteLine($"Packet Type: {inc.MessageType} SID: {sid} type: {type}");
                            if (type == PacketTypeEnums.PacketType.SEND_PLAYER_ID)
                            {
                                playerId = inc.ReadVariableInt32();
                                for (int i = 1; i <= playerId; i++)
                                {
                                    manager.AddPlayer(i);
                                }
                                Console.WriteLine($"Assigned player: {playerId}");
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

            NetIncomingMessage inc;
            if ((inc = client.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        int sid = inc.ReadVariableInt32();
                        PacketTypeEnums.PacketType type = (PacketTypeEnums.PacketType)(inc.ReadByte());
                        Console.WriteLine($"Packet Type: {inc.MessageType} SID: {sid} type: {type}");

                        switch (type)
                        {
                            case PacketTypeEnums.PacketType.SEND_PLAYER_ID:
                                Console.WriteLine("ID has already been assigned");
                                break;
                            case PacketTypeEnums.PacketType.NEW_PLAYER_ID:
                                manager.AddPlayer(inc.ReadVariableInt32());
                                break;
                            case PacketTypeEnums.PacketType.EVENT:
                                break;
                            case PacketTypeEnums.PacketType.GAME_STATE:
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

        protected override void Draw(GameTime gameTime)
        {
            // TODO Update graphics
            spritebatch.Begin();
            manager.Draw(spritebatch, gameTime);
            spritebatch.End();

            base.Draw(gameTime);
        }
    }
}
