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
        private static readonly Point[] NORTH_SPRITES = { new Point(0, 5), new Point(1, 5), new Point(2, 5), new Point(3, 5), new Point(4, 5), new Point(5, 5) };

        private static readonly Point[] SOUTH_SPRITES = { new Point(6, 5), new Point(7, 5), new Point(8, 5), new Point(9, 5), new Point(10, 5), new Point(11, 5) };

        private static readonly Point[] EAST_SPRITES = { new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0), new Point(5, 0) };

        private static readonly Point[] WEST_SPRITES = { new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1), new Point(5, 1) };

        private Texture2D texture;
        private Direction lastDirection;
        private int spriteIndex;
        private TimeSpan lastTime;

        public DrawablePlayer(Rectangle pos, Texture2D tex) : base(pos)
        {
            texture = tex;
            lastDirection = Direction.Center;
            lastTime = new TimeSpan();
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            Point m = new Point();
            if (MoveDirection != Direction.Center)
            {
                if (MoveDirection == lastDirection)
                {
                    if (lastTime.Milliseconds > 50)
                    {
                        spriteIndex = (spriteIndex + 1) % 6;
                        lastTime = new TimeSpan();
                    }
                    lastTime = lastTime.Add(gameTime.ElapsedGameTime);
                } else
                {
                    spriteIndex = 0;
                    lastTime = new TimeSpan();
                }
                switch (MoveDirection)
                {
                    case Direction.North:
                        m = NORTH_SPRITES[spriteIndex];
                        break;
                    case Direction.South:
                        m = SOUTH_SPRITES[spriteIndex];
                        break;
                    case Direction.East:
                        m = EAST_SPRITES[spriteIndex];
                        break;
                    case Direction.West:
                        m = WEST_SPRITES[spriteIndex];
                        break;
                }
            }
            lastDirection = MoveDirection;
            Rectangle textureRect = new Rectangle(m.X * 80 + 8, m.Y * 80 + 8, 64, 64);

            spritebatch.Draw(texture, Position, textureRect, Color.White);
        }
    }
}
