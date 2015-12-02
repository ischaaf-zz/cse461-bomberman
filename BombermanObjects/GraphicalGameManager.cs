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

        public Dictionary<string, Texture2D> textures;

        public GraphicalGameManager(int players, Dictionary<string, Texture2D> textureMappings) : base(players)
        {
            textures = textureMappings;
        }

        public override void Initialize()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new DrawablePlayer(STARTS[i], textures["player"]);
                players[i].Manager = this;
            }
            IGameObject background = new DrawableWall(textures["background"], new Rectangle(0, 0, BOX_WIDTH * GAME_SIZE, BOX_WIDTH * GAME_SIZE), null);
            statics.Add(background);

            for (int i = 0; i < GAME_SIZE; i++)
            {
                for (int j = 0; j < GAME_SIZE; j++)
                {
                    if (i == 0 || j == 0 || i == GAME_SIZE - 1 || j == GAME_SIZE - 1 || (i % 2 == 0 && j % 2 == 0))
                    {
                        IGameObject wall = new DrawableWall(textures["wall"], new Rectangle(i * BOX_WIDTH, j * BOX_WIDTH, BOX_WIDTH, BOX_WIDTH), null);
                        statics.Add(wall);
                        collider.RegisterStatic(wall);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            var backgrounds = statics.GetAllInRegion(new Rectangle(0, 0, GAME_SIZE * BOX_WIDTH, GAME_SIZE * BOX_WIDTH));
            foreach (var item in backgrounds)
            {
                (item as Drawable.IDrawable).Draw(spritebatch, gameTime);
            }
            var bombs = base.bombs.GetAllInRegion(new Rectangle(0, 0, GAME_SIZE * BOX_WIDTH, GAME_SIZE * BOX_WIDTH));
            foreach (var item in bombs)
            {
                (item as Drawable.IDrawable).Draw(spritebatch, gameTime);
            }
            foreach (var item in explosions.GetAllInRegion(new Rectangle(0, 0, GAME_SIZE * BOX_WIDTH, GAME_SIZE * BOX_WIDTH)))
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
            var expBounds = collider.MaxFill(new Point(x, y), p);
            int loX = x - p;
            int hiX = x + p;
            int loY = y - p;
            int hiY = y + p;
            if (expBounds[0] != null)
            {
                loX = expBounds[0].Position.Center.X / BOX_WIDTH + 1;
            }
            if (expBounds[1] != null)
            {
                hiX = expBounds[1].Position.Center.X / BOX_WIDTH - 1;
            }
            if (expBounds[2] != null)
            {
                loY = expBounds[2].Position.Center.Y / BOX_WIDTH + 1;
            }
            if (expBounds[3] != null)
            {
                hiY = expBounds[3].Position.Center.Y / BOX_WIDTH - 1;
            }
            for (int i = loX; i <= hiX; i++)
            {
                if (i == x)
                {
                    for (int j = loY; j <= hiY; j++)
                    {
                        explosions.Add(new DrawableExplosion(x, j, b.Position.Width, gametime.TotalGameTime, textures["explosion"]));
                    }
                }
                else
                {
                    explosions.Add(new DrawableExplosion(i, y, b.Position.Width, gametime.TotalGameTime, textures["explosion"]));
                }
            }
        }
    }
}
