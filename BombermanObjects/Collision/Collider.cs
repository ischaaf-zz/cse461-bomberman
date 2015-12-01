using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BombermanObjects.Logical;
using Microsoft.Xna.Framework;
using BombermanObjects.Collections;

namespace BombermanObjects.Collision
{
    public class Collider : ICollider
    {

        private GridObjectCollection statics;
        private GameObjectCollection dynamics;

        public Collider(int width, int height, int box_size)
        {
            statics = new GridObjectCollection(box_size, width, height);
            dynamics = new DynamicObjectCollection();
        }

        Movement[] ICollider.Move(IGameObject obj, Movement maxMove)
        {

            var rect = obj.Position;
            int remaining = maxMove.Move;
            int m1 = 0;
            int m2 = 0;
            Player.Direction d1 = Player.Direction.Center;
            Player.Direction d2 = Player.Direction.Center;
            bool canMove = false;
            int top = rect.Top % statics.Dimension;

            int bot = (rect.Center.Y / statics.Dimension + 1) * statics.Dimension - rect.Bottom;
            if (bot < 0)
                bot += statics.Dimension;

            int left = rect.Left % statics.Dimension;

            int right = (rect.Center.X / statics.Dimension + 1) * statics.Dimension - rect.Right;
            if (right < 0)
                right += statics.Dimension;

            switch (maxMove.Direction)
            {
                case Player.Direction.North:
                    d1 = Player.Direction.North;
                    d2 = Player.Direction.North;
                    canMove = !statics.IsItemAtPoint(new Point(rect.Center.X, rect.Top - maxMove.Move));
                    if (canMove)
                    { 
                        if (left < right)
                        {
                            m1 = Math.Min(remaining, left);
                            d1 = Player.Direction.West;
                        } else
                        {
                            m1 = Math.Min(remaining, right);
                            d1 = Player.Direction.East;
                        }
                    } else
                    {
                        m1 = top;
                        d2 = Player.Direction.Center;
                    }
                    break;
                case Player.Direction.South:
                    d1 = Player.Direction.South;
                    d2 = Player.Direction.South;
                    canMove = !statics.IsItemAtPoint(new Point(rect.Center.X, rect.Bottom + maxMove.Move));
                    if (canMove)
                    {
                        if (left < right)
                        {
                            m1 = Math.Min(remaining, left);
                            d1 = Player.Direction.West;
                        }
                        else
                        {
                            m1 = Math.Min(remaining, right);
                            d1 = Player.Direction.East;
                        }
                    }
                    else
                    {
                        m1 = bot;
                        d2 = Player.Direction.Center;
                    }
                    break;
                case Player.Direction.East:
                    d1 = Player.Direction.East;
                    d2 = Player.Direction.East;
                    canMove = !statics.IsItemAtPoint(new Point(rect.Right + maxMove.Move, rect.Center.Y));
                    if (canMove)
                    {
                        if (top < bot)
                        {
                            m1 = Math.Min(remaining, top);
                            d1 = Player.Direction.North;
                        }
                        else
                        {
                            m1 = Math.Min(remaining, bot);
                            d1 = Player.Direction.South;
                        }
                    }
                    else
                    {
                        m1 = right;
                        d2 = Player.Direction.Center;
                    }
                    break;
                case Player.Direction.West:
                    d1 = Player.Direction.West;
                    d2 = Player.Direction.West;
                    canMove = !statics.IsItemAtPoint(new Point(rect.Left - maxMove.Move, rect.Center.Y));
                    if (canMove)
                    {
                        if (top < bot)
                        {
                            m1 = Math.Min(remaining, top);
                            d1 = Player.Direction.North;
                        }
                        else
                        {
                            m1 = Math.Min(remaining, bot);
                            d1 = Player.Direction.South;
                        }
                    }
                    else
                    {
                        m1 = left;
                        d2 = Player.Direction.Center;
                    }
                    break;
            }
            m2 = remaining - m1;
            if (m1 == 0 && d2 == Player.Direction.Center)
                return new Movement[0];
            else if (m2 == 0)
                return new Movement[1] { new Movement(m1, d1) };
            else
                return new Movement[2] { new Movement(m1, d1), new Movement(m2, d2) };
        }

        public bool RegisterDynamic(IGameObject obj)
        {
            dynamics.Add(obj);
            return true;
        }

        public bool RegisterStatic(IGameObject obj)
        {
            statics.Add(obj);
            return true;
        }

        public bool UnRegisterDynamic(IGameObject obj)
        {
            return dynamics.Remove(obj);
        }

        public bool UnRegisterStatic(IGameObject obj)
        {
            return statics.Remove(obj);
        }
    }
}
