using BombermanObjects.Logical;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects.Drawable
{
    public class DrawableWall : Wall, IDrawable
    {
        private Texture2D texture;
        private Rectangle textureRect;

        public DrawableWall(Texture2D tex, Rectangle position, Rectangle? textureRect) : base(position)
        {
            texture = tex;
            this.textureRect = (textureRect.HasValue) ? textureRect.Value : new Rectangle(0, 0, position.Width, position.Height);
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            spritebatch.Draw(texture, position, textureRect, Color.White);
        }
    }
}
