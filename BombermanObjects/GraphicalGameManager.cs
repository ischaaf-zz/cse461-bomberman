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
            List<Box> boxes = new List<Box>();
            HashSet<Point> avoid = new HashSet<Point>() {
                new Point(1, 1), new Point(1, 2), new Point(2, 1), new Point(11, 1), new Point(10, 1), new Point(11, 2),
                new Point(1, 11), new Point(1, 10), new Point(2, 11), new Point(11, 11), new Point(10, 11), new Point(11, 10)
            };
            for (int i = 1; i < GAME_SIZE - 1; i++)
            {
                for (int j = 1; j < GAME_SIZE - 1; j++)
                {
                    if (statics.IsItemAtPoint(new Point(i, j)))
                        continue;
                    if (avoid.Contains(new Point(i, j)))
                        continue;
                    DrawableBox box = new DrawableBox(this, i, j, null, textures["box"]);
                    statics.Add(box);
                    collider.RegisterStatic(box);
                    boxes.Add(box);
                }
            }
            Random rand = new Random();
            boxes.Shuffle();
            int index = 0;
            for (int i = 0; i < TotalBombCap; i++)
            {
                boxes[index].PowerUp = new DrawablePowerUp(this, PowerUp.PowerUpType.BombCap, boxes[index].CenterGrid.X, boxes[index].CenterGrid.Y, textures["powerups"]); ;
                index++;
            }
            for (int i = 0; i < TotalBombPow; i++)
            {
                boxes[index].PowerUp = new DrawablePowerUp(this, PowerUp.PowerUpType.BombPower, boxes[index].CenterGrid.X, boxes[index].CenterGrid.Y, textures["powerups"]); ;
                index++;
            }
            for (int i = 0; i < TotalSpeed; i++)
            {
                boxes[index].PowerUp = new DrawablePowerUp(this, PowerUp.PowerUpType.Speed, boxes[index].CenterGrid.X, boxes[index].CenterGrid.Y, textures["powerups"]); ;
                index++;
            }
            //foreach (var b in boxes)
            //{
            //    b.Destroy();
            //}
        }

        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            background.Draw(spritebatch, gameTime);

            foreach (var item in statics)
            {
                (item as Drawable.IDrawable).Draw(spritebatch, gameTime);
            }
            foreach (var item in powerUps)
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

        public override bool PlaceExplosion(int x, int y, GameTime gametime)
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
                    explosions.Add(new DrawableExplosion(this, x, y, BOX_WIDTH, gametime.TotalGameTime, textures["explosion"]));
                }
                return false;
            }
            explosions.Add(new DrawableExplosion(this, x, y, BOX_WIDTH, gametime.TotalGameTime, textures["explosion"]));
            return true;
        }
    }
}
