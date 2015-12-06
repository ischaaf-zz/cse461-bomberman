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

namespace BombermanClient
{
    class BombermanGame : Game
    {

        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spritebatch;
        protected GraphicalGameManager manager;

        NetClient client;
        private bool gameStarted;
        private bool gameDrawn;

        public BombermanGame(String hostIp, int port) : base()
        {
            gameStarted = false;
            gameDrawn = false;
            NetPeerConfiguration config = new NetPeerConfiguration("game");
            client = new NetClient(config);
            
            client.Start();

            NetOutgoingMessage outmsg = client.CreateMessage();
            outmsg.Write("Login message");
  
            NetConnection connection = client.Connect(hostIp, port, outmsg);

            if (connection.Status != NetConnectionStatus.Connected)
            {
                // Wait, try again, if still not connected, exit
            }
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
            manager = new GraphicalGameManager(1, textureMap);
            manager.Initialize();
        }

        private void WaitForStart()
        {
            bool startMessageRecieved = false;
            NetIncomingMessage inc;

            while(!startMessageRecieved)
            {
                if ((inc = client.ReadMessage()) != null)
                {
                    // If message is special message from server
                        // gameStarted = true;
                    // Else ignore
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!gameStarted && gameDrawn)
            {
                manager.Update(gameTime);
                base.Update(gameTime);
                WaitForStart();
            }
            // TODO Update game logic
            manager.Update(gameTime);
            base.Update(gameTime);
            gameDrawn = true;
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
