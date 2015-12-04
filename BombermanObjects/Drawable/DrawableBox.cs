using BombermanObjects.Logical;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BombermanObjects.Drawable
{
    public class DrawableBox : Box, IDrawable
    {

        private Texture2D texture;

        public DrawableBox(GameManager m, int x, int y, PowerUp p, Texture2D tex) : base(m, x, y, p)
        {
            texture = tex;
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            spritebatch.Draw(texture, Position, Color.White);
        }
    }
}
