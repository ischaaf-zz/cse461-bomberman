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
    public class DrawableExplosion : Explosion, IDrawable
    {
        private Texture2D texture;

        public DrawableExplosion(int x, int y, int dim, TimeSpan placedAt, Texture2D tex) : base(x, y, dim, placedAt)
        {
            texture = tex;
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            spritebatch.Draw(texture, Position, Color.White);
        }
    }
}
