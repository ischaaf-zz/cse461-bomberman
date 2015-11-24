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
        protected GameObjectCollection statics;
        protected GameObjectCollection dynamics;
        protected Player[] players;
        protected LocalInput input;


        public GameManager(int players)
        {
            int dim = GAME_SIZE * BOX_WIDTH;
            collider = new Collider(dim, dim, BOX_WIDTH);
            statics = new StaticObjectCollection(dim, dim, BOX_WIDTH);
            dynamics = new DynamicObjectCollection();
            this.players = new Player[players];
            input = new LocalInput();
        }

        public virtual void Initialize()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player(STARTS[i]);
                players[i].Manager = this;
            }
            IGameObject background = new Wall(new Rectangle(0, 0, BOX_WIDTH * GAME_SIZE, BOX_WIDTH * GAME_SIZE));
            statics.Add(background);

            for (int i = 0; i < GAME_SIZE; i++)
            {
                for (int j = 0; j < GAME_SIZE; j++)
                {
                    if (i == 0 || j == 0 || i == GAME_SIZE - 1 || j == GAME_SIZE - 1 || (i % 2 == 0 && j % 2 == 0))
                    {
                        IGameObject wall = new Wall(new Rectangle(i * BOX_WIDTH, j * BOX_WIDTH, BOX_WIDTH, BOX_WIDTH));
                        statics.Add(wall);
                        collider.RegisterStatic(wall);
                    }
                }
            }
        }

        public virtual void Update(GameTime gametime)
        {
            input.Update(gametime);

            foreach (var p in players)
            {
                p.Update(gametime, input.Keys);
            }
        }
    }
}
