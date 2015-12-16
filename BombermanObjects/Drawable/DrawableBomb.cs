using BombermanObjects.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BombermanObjects.Drawable
{
    public class DrawableBomb : Bomb, IDrawable
    {

        private Texture2D texture;

        public DrawableBomb(GameManager m, int x, int y, TimeSpan placed, int ttd, Player placedBy, int dim, Texture2D tex) 
            : base(m, x, y, placed, ttd, placedBy, dim)
        {
            texture = tex;
        }

        public DrawableBomb(GameManager m, int x, int y, TimeSpan placed, int ttd, Player placedBy, int dim, Texture2D tex, bool super)
            : base(m, x, y, placed, ttd, placedBy, dim, super)
        {
            texture = tex;
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            spritebatch.Draw(texture, Position, super ? Color.Black : Color.White);
        }
    }
}
