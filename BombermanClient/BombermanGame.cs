﻿using BombermanObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanClient
{
    class BombermanGame : Game
    {

        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spritebatch;
        protected GraphicalGameManager manager;

        public BombermanGame() : base()
        {
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
            manager = new GraphicalGameManager(0, textureMap);
            manager.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO Update game logic

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO Update graphics
            spritebatch.Begin();
            manager.Draw(spritebatch);
            spritebatch.End();

            base.Draw(gameTime);
        }
    }
}
