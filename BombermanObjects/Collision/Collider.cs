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

        private SingleGridObjectCollection blocks;
        private int dim;
        private int width;
        private int height;

        public Collider(int width, int height, int box_size)
        {
            blocks = new SingleGridObjectCollection(box_size, width, height);
            dim = box_size;
            this.width = width;
            this.height = height;
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
            int top = rect.Top % blocks.Dimension;

            int bot = (rect.Center.Y / blocks.Dimension + 1) * blocks.Dimension - rect.Bottom;
            if (bot < 0)
                bot += blocks.Dimension;

            int left = rect.Left % blocks.Dimension;

            int right = (rect.Center.X / blocks.Dimension + 1) * blocks.Dimension - rect.Right;
            if (right < 0)
                right += blocks.Dimension;

            switch (maxMove.Direction)
            {
                case Player.Direction.North:
                    d1 = Player.Direction.North;
                    d2 = Player.Direction.North;
                    canMove = !blocks.IsItemAtPoint(new Point(rect.Center.X / dim, (rect.Top - maxMove.Move) / dim));
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
                    canMove = !blocks.IsItemAtPoint(new Point(rect.Center.X / dim, (rect.Bottom + maxMove.Move) / dim));
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
                    canMove = !blocks.IsItemAtPoint(new Point((rect.Right + maxMove.Move) / dim, rect.Center.Y / dim));
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
                    canMove = !blocks.IsItemAtPoint(new Point((rect.Left - maxMove.Move) / dim, rect.Center.Y / dim));
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
            m1 = Math.Min(m1, maxMove.Move);
            m2 = Math.Min(remaining - m1, maxMove.Move);
            if (m1 == 0 && d2 == Player.Direction.Center)
                return new Movement[0];
            else if (m2 == 0)
                return new Movement[1] { new Movement(m1, d1) };
            else
                return new Movement[2] { new Movement(m1, d1), new Movement(m2, d2) };
        }

        AbstractGameObject[] ICollider.MaxFill(Point start, int power)
        {
            AbstractGameObject[] res = new AbstractGameObject[4];
            for (int i = start.X; i >= Math.Max(start.X - power, 0); i--)
            {
                var p = new Point(i, start.Y);
                if (blocks.IsItemAtPoint(p))
                {
                    res[0] = blocks.GetAtPoint(p);
                }
            }
            for (int i = start.X; i <= Math.Min(start.X + power, width - 1); i++)
            {
                var p = new Point(i, start.Y);
                if (blocks.IsItemAtPoint(p))
                {
                    res[1] = blocks.GetAtPoint(p);
                }
            }
            for (int i = start.Y; i >= Math.Max(start.Y - power, 0); i--)
            {
                var p = new Point(start.X, i);
                if (blocks.IsItemAtPoint(p))
                {
                    res[2] = blocks.GetAtPoint(p);
                }
            }
            for (int i = start.Y; i <= Math.Min(start.Y + power, height - 1); i++)
            {
                var p = new Point(start.X, i);
                if (blocks.IsItemAtPoint(p))
                {
                    res[3] = blocks.GetAtPoint(p);
                }
            }
            return res;
        }

        public bool RegisterStatic(AbstractGameObject obj)
        {
            blocks.Add(obj);
            return true;
        }

        public bool UnRegisterStatic(AbstractGameObject obj)
        {
            return blocks.Remove(obj);
        }
    }
}
