using BombermanObjects.Drawable;
using BombermanObjects.Logical;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects
{
    public class GraphicalGameManager : GameManager
    {

        public DrawableWall background;

        public Dictionary<string, Texture2D> textures;

        public GraphicalGameManager(int players, Dictionary<string, Texture2D> textureMappings) : base(players)
        {
            textures = textureMappings;
        }

        public override void Initialize()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new DrawablePlayer(this, STARTS[i], textures["player"]);
            }
            background = new DrawableWall(this, textures["background"], new Rectangle(0, 0, BOX_WIDTH * GAME_SIZE, BOX_WIDTH * GAME_SIZE), null);

            for (int i = 0; i < GAME_SIZE; i++)
            {
                for (int j = 0; j < GAME_SIZE; j++)
                {
                    if (i == 0 || j == 0 || i == GAME_SIZE - 1 || j == GAME_SIZE - 1 || (i % 2 == 0 && j % 2 == 0))
                    {
                        AbstractGameObject wall = new DrawableWall(this, textures["wall"], new Rectangle(i * BOX_WIDTH, j * BOX_WIDTH, BOX_WIDTH, BOX_WIDTH), null);
                        statics.Add(wall);
                        collider.RegisterStatic(wall);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            background.Draw(spritebatch, gameTime);

            foreach (var item in statics)
            {
                (item as Drawable.IDrawable).Draw(spritebatch, gameTime);
            }
            foreach (var item in bombs)
            {
                (item as Drawable.IDrawable).Draw(spritebatch, gameTime);
            }
            foreach (var item in explosions)
            {
                (item as Drawable.IDrawable).Draw(spritebatch, gameTime);
            }
            foreach (var p in players)
            {
                (p as Drawable.IDrawable).Draw(spritebatch, gameTime);
            }
        }

        public override void UpdateBomb(GameTime gametime, Bomb b)
        {
            base.UpdateBomb(gametime, b);
        }

        public override void ExplodeBomb(GameTime gametime, Bomb b)
        {
            bombs.Remove(b);
            collider.UnRegisterStatic(b);
            b.placedBy.PlacedBombs--;

            int x = b.Position.Center.X / b.Position.Width;
            int y = b.Position.Center.Y / b.Position.Height;
            int p = b.placedBy.BombPower;
            int loX = x - p;
            int hiX = x + p;
            int loY = y - p;
            int hiY = y + p;
            // Negative X
            PlaceExplosion(x, y, gametime);
            for (int i = x - 1; i >= loX; i--)
            {
                if (!PlaceExplosion(i, y, gametime))
                    break;
            }
            // Positive X
            for (int i = x + 1; i <= hiX; i++)
            {
                if (!PlaceExplosion(i, y, gametime))
                    break;
            }
            // Negative Y
            for (int i = y - 1; i >= loY; i--)
            {
                if (!PlaceExplosion(x, i, gametime))
                    break;
            }
            // Positive Y
            for (int i = y + 1; i <= hiY; i++)
            {
                if (!PlaceExplosion(x, i, gametime))
                    break;
            }
        }

        private bool PlaceExplosion(int x, int y, GameTime gametime)
        {
            Point p = new Point(x, y);
            if (bombs.IsItemAtPoint(p))
            {
                Bomb b = bombs.GetAtPoint(p) as Bomb;
                ExplodeBomb(gametime, b);
            } else if (statics.IsItemAtPoint(p))
            {
                var item = statics.GetAtPoint(p);
                if (item is Box)
                {
                    // blow the box up
                }
                return false;
            }
            explosions.Add(new DrawableExplosion(this, x, y, BOX_WIDTH, gametime.TotalGameTime, textures["explosion"]));
            return true;
        }
    }
}
