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
    public class Player : AbstractGameObject
    {
        public enum Direction : byte
        {
            North, South, East, West, Center
        }

        public int Speed { get; set; }

        public int Lives { get; set; }

        public int MaxBombs { get; set; }

        public int PlacedBombs { get; set; }

        public int BombPower { get; set; }

        public TimeSpan ImmuneTill { get; set; }

        public bool CanKick { get; set; }

        public bool AutoBomb { get; set; }

        public bool BombPass { get; set; }

        public bool Pierce { get; set; }

        public Direction MoveDirection { get; set; }

        // added for server communication.
        public ExternalOutput externalOutput { get; set; }

        public override Rectangle Position
        {
            get
            {
                return position;
            }
        }

        public Rectangle position;

        public Player(GameManager m, Rectangle pos) : base(m)
        {
            position = pos;
            // start: 3, end: 7_
            Speed = 2;
            Lives = 2;
            MaxBombs = 1;
            BombPower = 2;
            PlacedBombs = 0;
            CanKick = false;
            MoveDirection = Direction.Center;
        }

        public void Update(GameTime gametime, PlayerInput input)
        {
            // check if dead
            if (Lives <= 0)
            {
                return;
            }

            move(MoveDirection, Speed);

            Rectangle iRect = new Rectangle(position.X + 10, position.Y + 10, 44, 44);
            int cY = iRect.Center.Y / GameManager.BOX_WIDTH;
            int hY = iRect.Bottom / GameManager.BOX_WIDTH;
            int lY = iRect.Top / GameManager.BOX_WIDTH;
            int cX = iRect.Center.X / GameManager.BOX_WIDTH;
            int hX = iRect.Right / GameManager.BOX_WIDTH;
            int lX = iRect.Left / GameManager.BOX_WIDTH;

            Point[] ps = { new Point(lX, cY), new Point(hX, cY), new Point(cX, hY), new Point(cX, lY) };
            // check for explosions
            // check left
            if (gametime.TotalGameTime > ImmuneTill && 
                (manager.explosions.GetAtPoint(ps[0])?.Count > 0 ||
                manager.explosions.GetAtPoint(ps[1])?.Count > 0 ||
                manager.explosions.GetAtPoint(ps[2])?.Count > 0 ||
                manager.explosions.GetAtPoint(ps[3])?.Count > 0))
            {
                Lives--;
                Console.WriteLine("Player lost a life");
                ImmuneTill = gametime.TotalGameTime.Add(new TimeSpan(0, 0, 1));
            }
            // check for powerups
            for (int i = 0; i < 4; i++)
            {
                PowerUp p = manager.powerUps.GetAtPoint(ps[i]) as PowerUp;
                if (p != null)
                {
                    p.Apply(this);
                    manager.powerUps.Remove(p);
                }
            }
        }

        // changed to public from private for server use.
        public bool move(Direction dir, int dist)
        {
            bool moved = false;
            Movement m = new Movement(dist, dir);
            var res = manager.collider.Move(this, m);
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

        // changed to public from protected for server use.
        public virtual void placeBomb(GameTime gameTime)
        {
            // added for external communication
            if (externalOutput != null)
            {
                externalOutput.sendBombPlacement(gameTime);
            }
            int x = position.Center.X / position.Width;
            int y = position.Center.Y / position.Height;
            if (PlacedBombs < MaxBombs && !manager.bombs.IsItemAtPoint(new Point(x, y)) && !manager.explosions.IsItemAtPoint(new Point(x, y)))
            {
                Bomb b = new Bomb(
                    manager,
                    x,
                    y,
                    gameTime.TotalGameTime,
                    3,
                    this,
                    position.Width
                );
                manager.collider.RegisterStatic(b);
                manager.bombs.Add(b);
                PlacedBombs++;
            }
        }
    }
}
