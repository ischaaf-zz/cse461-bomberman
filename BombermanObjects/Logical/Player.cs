using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BombermanObjects.Logical
{
    public class Player : IGameObject
    {
        public enum Direction
        {
            North, South, East, West
        }

        public static readonly Dictionary<Keys, Direction> BINDINGS = new Dictionary<Keys, Direction>()
        {
            {Keys.W, Direction.North },
            {Keys.D, Direction.East },
            {Keys.A, Direction.West },
            {Keys.S, Direction.South }
        };

        public GameManager Manager { get; set; }

        public int Speed { get; private set; }

        public int Lives { get; private set; }

        public int MaxBombs { get; private set; }

        public int PlacedBombs { get; private set; }

        public bool CanKick { get; private set; }


        private Rectangle position;

        public Player(Rectangle pos)
        {
            position = pos;
            Speed = 5;
            Lives = 2;
            MaxBombs = 1;
            PlacedBombs = 0;
            CanKick = false;
        }

        public Rectangle Position
        {
            get
            {
                return position;
            }
        }

        public void Update(GameTime gametime, LinkedList<Keys> keys)
        {
            foreach (var key in keys)
            {
                if (BINDINGS.ContainsKey(key))
                {
                    Vector2 move = new Vector2();
                    int res;
                    bool moved = false;
                    switch (BINDINGS[key]) {
                        case Direction.North:
                            move.Y = -Speed;
                            res = Manager.collider.Collide(this, move);
                            if (res != 0)
                            {
                                moved = true;
                                position.Y += res;
                            }
                            break;
                        case Direction.South:
                            move.Y = Speed;
                            res = Manager.collider.Collide(this, move);
                            if (res != 0)
                            {
                                moved = true;
                                position.Y += res;
                            }
                            break;
                        case Direction.West:
                            move.X = -Speed;
                            res = Manager.collider.Collide(this, move);
                            if (res != 0)
                            {
                                moved = true;
                                position.X += res;
                            }
                            break;
                        case Direction.East:
                            move.X = Speed;
                            res = Manager.collider.Collide(this, move);
                            if (res != 0)
                            {
                                moved = true;
                                position.X += res;
                            }
                            break;
                    }
                    if (moved)
                        break;
                }
            }
        }
    }
}
