using BombermanObjects.Collections;
using BombermanObjects.Collision;
using BombermanObjects.Logical;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects
{
    public class GameManager
    {
        public static readonly int GAME_SIZE = 13;
        public static readonly int BOX_WIDTH = 64;
        public static readonly Rectangle[] STARTS = {
            new Rectangle(64, 64, 64, 64),
            new Rectangle(64, 64, 64, 64),
            new Rectangle(64, 64, 64, 64),
            new Rectangle(64, 64, 64, 64)
        };

        public ICollider collider;
        public SingleGridObjectCollection statics;
        public SingleGridObjectCollection bombs;
        public GridObjectCollection explosions;
        protected Player[] players;
        protected LocalInput input;


        public GameManager(int players)
        {
            int dim = GAME_SIZE * BOX_WIDTH;
            collider = new Collider(GAME_SIZE, GAME_SIZE, BOX_WIDTH);
            statics = new SingleGridObjectCollection(BOX_WIDTH, GAME_SIZE, BOX_WIDTH);
            explosions = new GridObjectCollection(BOX_WIDTH, GAME_SIZE, BOX_WIDTH);
            bombs = new SingleGridObjectCollection(BOX_WIDTH, GAME_SIZE, GAME_SIZE);
            this.players = new Player[players];
            input = new LocalInput();
        }

        public virtual void Initialize()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player(this, STARTS[i]);
            }
            AbstractGameObject background = new Wall(this, new Rectangle(0, 0, BOX_WIDTH * GAME_SIZE, BOX_WIDTH * GAME_SIZE));
            statics.Add(background);

            for (int i = 0; i < GAME_SIZE; i++)
            {
                for (int j = 0; j < GAME_SIZE; j++)
                {
                    if (i == 0 || j == 0 || i == GAME_SIZE - 1 || j == GAME_SIZE - 1 || (i % 2 == 0 && j % 2 == 0))
                    {
                        AbstractGameObject wall = new Wall(this, new Rectangle(i * BOX_WIDTH, j * BOX_WIDTH, BOX_WIDTH, BOX_WIDTH));
                        statics.Add(wall);
                        collider.RegisterStatic(wall);
                    }
                }
            }
        }

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
            }

            foreach (var p in players)
            {
                p.Update(gametime, input.CurrentInput);
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
                        explosions.Add(new Explosion(this, x, j, b.Position.Width, gametime.TotalGameTime));
                    }
                } else
                {
                    explosions.Add(new Explosion(this, i, y, b.Position.Width, gametime.TotalGameTime));
                }
            }
        }
    }
}
