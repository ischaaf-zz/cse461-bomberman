using BombermanObjects.Collections;
using BombermanObjects.Collision;
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
    public class GameManager
    {
        public static readonly Color[] COLORS = { Color.Blue, Color.LightBlue, Color.LightSalmon, Color.White };
        public static readonly int GAME_SIZE = 13;
        public static readonly int BOX_WIDTH = 64;
        public static readonly Rectangle[] STARTS = {
            new Rectangle(64, 64, 64, 64),
            new Rectangle(11*64, 11*64, 64, 64),
            new Rectangle(11*64, 64, 64, 64),
            new Rectangle(64, 11*64, 64, 64)
        };

        public ICollider collider;
        public SingleGridObjectCollection statics;
        public SingleGridObjectCollection bombs;
        public GridObjectCollection explosions;
        public SingleGridObjectCollection powerUps;
        // changed players to public for server use from protected
        public Player[] players;
        protected LocalInput input;

        public Wall background;

        public int TotalBombCap { get; set; }
        public int TotalBombPow { get; set; }
        public int TotalSpeed { get; set; }

        public List<Box> DestroyedBoxes { get; set; }

        public GameManager(int players)
        {
            int dim = GAME_SIZE * BOX_WIDTH;
            collider = new Collider(GAME_SIZE, GAME_SIZE, BOX_WIDTH);
            statics = new SingleGridObjectCollection(BOX_WIDTH, GAME_SIZE, BOX_WIDTH);
            explosions = new GridObjectCollection(BOX_WIDTH, GAME_SIZE, BOX_WIDTH);
            bombs = new SingleGridObjectCollection(BOX_WIDTH, GAME_SIZE, GAME_SIZE);
            powerUps = new SingleGridObjectCollection(BOX_WIDTH, GAME_SIZE, GAME_SIZE);
            this.players = new Player[players];
            input = new LocalInput();
            TotalBombCap = 15;
            TotalBombPow = 20;
            TotalSpeed = 10;
        }

        public Player AddPlayer(int number)
        {
            if (players[number - 1] == null)
            {
                players[number - 1] = new Player(this, STARTS[number - 1]);
                return players[number - 1];
            } else
            {
                return null;
            }
        }

        public virtual void Initialize()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = CreatePlayer(STARTS[i], COLORS[i]);
            }
            background = CreateBackground();

            for (int i = 0; i < GAME_SIZE; i++)
            {
                for (int j = 0; j < GAME_SIZE; j++)
                {
                    if (i == 0 || j == 0 || i == GAME_SIZE - 1 || j == GAME_SIZE - 1 || (i % 2 == 0 && j % 2 == 0))
                    {
                        AbstractGameObject wall = CreateWall(i * BOX_WIDTH, j * BOX_WIDTH, BOX_WIDTH, null);
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
                    Box box = CreateBox(i, j, null);
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
                boxes[index].PowerUp = CreatePowerUp(PowerUp.PowerUpType.BombCap, boxes[index].CenterGrid.X, boxes[index].CenterGrid.Y); ;
                index++;
            }
            for (int i = 0; i < TotalBombPow; i++)
            {
                boxes[index].PowerUp = CreatePowerUp(PowerUp.PowerUpType.BombPower, boxes[index].CenterGrid.X, boxes[index].CenterGrid.Y); ;
                index++;
            }
            for (int i = 0; i < TotalSpeed; i++)
            {
                boxes[index].PowerUp = CreatePowerUp(PowerUp.PowerUpType.Speed, boxes[index].CenterGrid.X, boxes[index].CenterGrid.Y); ;
                index++;
            }
        }

        #region Create Objects

        public virtual Wall CreateBackground()
        {
            return null;
        }

        public virtual Wall CreateWall(int x, int y, int dim, Rectangle? textureRect)
        {
            return new Wall(this, new Rectangle(x, y, dim, dim));
        }

        public virtual Player CreatePlayer(Rectangle pos, Color? c)
        {
            return new Player(this, pos);
        }

        public virtual Box CreateBox(int x, int y, PowerUp p)
        {
            return new Box(this, x, y, p);
        }

        public virtual PowerUp CreatePowerUp(PowerUp.PowerUpType type, int x, int y)
        {
            return new PowerUp(this, type, x, y);
        }

        public virtual Explosion CreateExplosion(int x, int y, int dim, TimeSpan startedAt)
        {
            return new Explosion(this, x, y, dim, startedAt);
        }

        public virtual Bomb CreateBomb(int x, int y, TimeSpan placed, int ttd, Player placedBy, int dim)
        {
            return new Bomb(this, x, y, placed, ttd, placedBy, dim);
        }

        #endregion

        public virtual void Update(GameTime gametime)
        {
            input.Update(gametime);

            HashSet<AbstractGameObject> toRemove = new HashSet<AbstractGameObject>();

            foreach (var e in explosions)
            {
                if (gametime.TotalGameTime >= (e as Explosion).RemoveAt)
                {
                    toRemove.Add(e);
                }
            }
            foreach (var e in toRemove)
            {
                explosions.Remove(e);
                Explosion exp = e as Explosion;
                var item = statics.GetAtPoint(exp.CenterGrid) as Box;
                if (item != null)
                {
                    item.Destroy();
                    DestroyedBoxes.Add(item);
                }
            }

            foreach (var p in players)
            {
                if (p != null)
                {
                    p.Update(gametime, input.CurrentInput);
                }
            }

            foreach (var b in bombs)
            {
                UpdateBomb(gametime, b as Bomb);
            }
            
        }

        public virtual void UpdateBomb(GameTime gametime, Bomb b)
        {
            if (gametime.TotalGameTime >= b.DetonateTime)
            {
                ExplodeBomb(gametime, b);
            }
        }

        public virtual void ExplodeBomb(GameTime gametime, Bomb b)
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

        public virtual bool PlaceExplosion(int x, int y, GameTime gametime)
        {
            Point p = new Point(x, y);
            if (bombs.IsItemAtPoint(p))
            {
                Bomb b = bombs.GetAtPoint(p) as Bomb;
                ExplodeBomb(gametime, b);
            }
            else if (statics.IsItemAtPoint(p))
            {
                var item = statics.GetAtPoint(p);
                if (item is Box)
                {
                    explosions.Add(new Explosion(this, x, y, BOX_WIDTH, gametime.TotalGameTime));
                }
                return false;
            }
            explosions.Add(new Explosion(this, x, y, BOX_WIDTH, gametime.TotalGameTime));
            return true;
        }
    }
}
