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
            North, South, East, West, Center
        }

        public static readonly Dictionary<Keys, Direction> BINDINGS = new Dictionary<Keys, Direction>()
        {
            {Keys.Up, Direction.North },
            {Keys.Right, Direction.East },
            {Keys.Left, Direction.West },
            {Keys.Down, Direction.South }
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
            int moved = 0;
            Direction first = Direction.Center;
            foreach (var key in keys)
            {
                if (BINDINGS.ContainsKey(key))
                {
                    moved += move(BINDINGS[key], Speed);
                    if (first == Direction.Center)
                        first = BINDINGS[key];
                }
                if (moved >= Speed)
                    break;
            }
            if (moved < Speed)
            {
                move(first, Speed - moved);
            }
        }

        private int move(Direction dir, int dist)
        {
            int moved = 0;
            Point move = new Point();
            Point res = new Point();
            switch (dir)
            {
                case Direction.North:
                    move.Y = -dist;
                    move.X = 0;
                    res = Manager.collider.Move(this, move);
                    moved += Math.Abs(res.Y);
                    position.Y += res.Y;
                    break;
                case Direction.South:
                    move.Y = dist;
                    move.X = 0;
                    res = Manager.collider.Move(this, move);
                    moved += Math.Abs(res.Y);
                    position.Y += res.Y;
                    break;
                case Direction.West:
                    move.Y = 0;
                    move.X = -dist;
                    res = Manager.collider.Move(this, move);
                    moved += Math.Abs(res.X);
                    position.X += res.X;
                    break;
                case Direction.East:
                    move.Y = 0;
                    move.X = dist;
                    res = Manager.collider.Move(this, move);
                    moved += Math.Abs(res.X);
                    position.X += res.X;
                    break;
            }
            return moved;
        }
    }
}
