using BombermanObjects.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BombermanObjects.Drawable
{
    public class DrawablePlayer : Player, IDrawable
    {

        private Texture2D texture;

        public DrawablePlayer(Rectangle pos, Texture2D tex) : base(pos)
        {
            texture = tex;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, Position, Color.White);
        }
    }
}
