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
    public class DrawablePowerUp : PowerUp, IDrawable
    {
        private Texture2D texture;
        private Rectangle textureRect;

        public DrawablePowerUp(GameManager m, PowerUpType type, int x, int y, Texture2D tex) : base(m, type, x, y)
        {
            texture = tex;
            int off = 0;
            switch (type) {
                case PowerUpType.BombCap:
                    off = 2;
                    break;
                case PowerUpType.BombPower:
                    off = 0;
                    break;
                case PowerUpType.Speed:
                    off = 1;
                    break;
            }
            textureRect = new Rectangle(off * GameManager.BOX_WIDTH, 0, GameManager.BOX_WIDTH, GameManager.BOX_WIDTH);
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            spritebatch.Draw(texture, Position, textureRect, Color.White);
        }
    }
}
