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
        private double maxTicks = new TimeSpan(0, 0, 3).TotalMilliseconds;

        public DrawableExplosion(GameManager m, int x, int y, int dim, TimeSpan placedAt, Texture2D tex) : base(m, x, y, dim, placedAt)
        {
            texture = tex;
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            double diff = (RemoveAt.TotalMilliseconds - gameTime.TotalGameTime.TotalMilliseconds) / maxTicks;
            diff = Math.Max(diff, 0);
             
            spritebatch.Draw(texture, Position, new Color(Color.White, (int)(diff * 255)));
        }
    }
}
