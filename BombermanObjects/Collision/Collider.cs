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

        public Point Move(IGameObject obj, Point maxMove)
        {
            int xMove = maxMove.X;
            int yMove = maxMove.Y;
            var rect = obj.Position;

            int xdistToBound;
            int ydistToBound;

            if (yMove != 0)
            {
                if (yMove < 0)
                {
                    maxNegativeY(rect, ref yMove, out ydistToBound);
                    if (ydistToBound < 0 && ydistToBound > yMove)
                        yMove = ydistToBound;
                }
                else
                {
                    maxPositiveY(rect, ref yMove, out ydistToBound);
                    if (ydistToBound > 0 && ydistToBound < yMove)
                        yMove = ydistToBound;
                }
            } else
            {
                ydistToBound = 0;
            }
            if (xMove != 0)
            {
                if (xMove < 0)
                {
                    maxNegativeX(rect, ref xMove, out xdistToBound);
                    if (xdistToBound < 0 && xdistToBound > xMove)
                        xMove = xdistToBound;
                }
                else
                {
                    maxPositiveX(rect, ref xMove, out xdistToBound);
                    if (xdistToBound > 0 && xdistToBound < xMove)
                        xMove = xdistToBound;
                }
            }
            else
            {
                xdistToBound = 0;
            }
            return new Point(xMove, yMove);
        }

        private void maxPositiveX(Rectangle rect, ref int max, out int distToBound)
        {
            distToBound = (rect.Center.X / statics.Dimension + 1) * statics.Dimension - rect.Right;
            bool ly = statics.IsItemAtPoint(new Point(rect.Right + max, rect.Top));
            bool ry = statics.IsItemAtPoint(new Point(rect.Right + max, rect.Bottom - 1));
            max = (ly || ry) ? distToBound : max;
        }

        private void maxNegativeX(Rectangle rect, ref int max, out int distToBound)
        {
            distToBound = -(rect.Left % statics.Dimension);
            bool ly = statics.IsItemAtPoint(new Point(rect.Left + max, rect.Top));
            bool ry = statics.IsItemAtPoint(new Point(rect.Left + max, rect.Bottom - 1));
            max = (ly || ry) ? distToBound : max;
        }

        private void maxPositiveY(Rectangle rect, ref int max, out int distToBound)
        {
            distToBound = (rect.Center.Y / statics.Dimension + 1) * statics.Dimension - rect.Bottom;
            bool lx = statics.IsItemAtPoint(new Point(rect.Left, rect.Bottom + max));
            bool rx = statics.IsItemAtPoint(new Point(rect.Right - 1, rect.Bottom + max));
            max = (lx || rx) ? distToBound : max;
        }

        private void maxNegativeY(Rectangle rect, ref int max, out int distToBound)
        {
            distToBound = -(rect.Top % statics.Dimension);
            bool lx = statics.IsItemAtPoint(new Point(rect.Left, rect.Top + max));
            bool rx = statics.IsItemAtPoint(new Point(rect.Right - 1, rect.Top + max));
            max = (lx || rx) ? distToBound : max;
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
