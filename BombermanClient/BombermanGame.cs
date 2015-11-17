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

        public BombermanGame() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spritebatch = new SpriteBatch(GraphicsDevice);
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

            base.Draw(gameTime);
        }
    }
}
