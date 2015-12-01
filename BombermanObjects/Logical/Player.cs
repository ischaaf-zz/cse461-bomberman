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

        public GameManager Manager { get; set; }

        public int Speed { get; set; }

        public int Lives { get; set; }

        public int MaxBombs { get; set; }

        public int PlacedBombs { get; set; }

        public int BombPower { get; set; }

        public bool CanKick { get; set; }

        public Direction MoveDirection { get; set; }


        protected Rectangle position;

        public Player(Rectangle pos)
        {
            position = pos;
            // start: 3, end: 7_
            Speed = 3;
            Lives = 2;
            MaxBombs = 1;
            BombPower = 2;
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

        public void Update(GameTime gametime, PlayerInput input)
        {
            bool moved = false;
            foreach (var dir in input.Move)
            {
                moved = move(dir, Speed);
                if (moved)
                    break;
            }
            if (!moved)
                MoveDirection = Direction.Center;
            if (input.PlaceBomb)
            {
                placeBomb(gametime);
            }
        }

        private bool move(Direction dir, int dist)
        {
            bool moved = false;
            Movement m = new Movement(dist, dir);
            var res = Manager.collider.Move(this, m);
            foreach (var move in res)
            {
                switch (move.Direction)
                {
                    case Direction.North:
                        position.Y -= move.Move;
                        break;
                    case Direction.South:
                        position.Y += move.Move;
                        break;
                    case Direction.East:
                        position.X += move.Move;
                        break;
                    case Direction.West:
                        position.X -= move.Move;
                        break;
                }
                moved = true;
                MoveDirection = move.Direction;
            }
            if (!moved)
                MoveDirection = Direction.Center;
            return moved;
        }

        protected virtual void placeBomb(GameTime gameTime)
        {
            int x = position.Center.X / position.Width;
            int y = position.Center.Y / position.Height;
            if (PlacedBombs < MaxBombs && Manager.bombs.GetAllAtPoint(new Vector2(position.Center.X, position.Center.Y)).Count == 0)
            {
                Bomb b = new Bomb(
                    x,
                    y,
                    gameTime.TotalGameTime,
                    3,
                    this,
                    position.Width
                );
                Manager.collider.RegisterStatic(b);
                Manager.bombs.Add(b);
                PlacedBombs++;
            }
        }
    }
}
