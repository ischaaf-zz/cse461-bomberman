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

        private GameObjectCollection statics;
        private GameObjectCollection dynamics;

        public Collider(int width, int height, int box_size)
        {
            statics = new StaticObjectCollection(width, height, box_size);
            dynamics = new DynamicObjectCollection();
        }

        public int Collide(IGameObject obj, Vector2 maxMove)
        {
            if (maxMove.X != 0)
            {
                Rectangle collisionRect;
                int max = (int)maxMove.X;
                if (max < 0)
                {
                    collisionRect = new Rectangle(max + obj.Position.Left, obj.Position.Top, 
                        -1 * max, obj.Position.Height);
                    var items = statics.GetAllInRegion(collisionRect);
                    dynamics.GetAllInRegion(collisionRect, ref items);
                    int left = obj.Position.Left + max;
                    foreach (var item in items)
                    {
                        if (item.Position.Right < obj.Position.Left && item.Position.Left > left)
                            left = item.Position.Left;
                    }
                    return left - obj.Position.Left;
                } else
                {
                    collisionRect = new Rectangle(obj.Position.Right, obj.Position.Top, (int)maxMove.X, obj.Position.Height);
                    var items = statics.GetAllInRegion(collisionRect);
                    dynamics.GetAllInRegion(collisionRect, ref items);
                    int right = obj.Position.Right + max;
                    foreach (var item in items)
                    {
                        if (item.Position.Left > obj.Position.Right && item.Position.Left < right)
                            right = item.Position.Left;
                    }
                    return right - obj.Position.Right;
                }
            } else
            {
                Rectangle collisionRect;
                int max = (int)maxMove.Y;
                if (max < 0)
                {
                    collisionRect = new Rectangle(obj.Position.Left, obj.Position.Top + max, obj.Position.Width, -1 * max);
                    var items = statics.GetAllInRegion(collisionRect);
                    dynamics.GetAllInRegion(collisionRect, ref items);
                    int top = obj.Position.Top + max;
                    foreach (var item in items)
                    {
                        if (item.Position.Bottom < obj.Position.Top && item.Position.Bottom > top)
                            top = item.Position.Bottom;
                    }
                    return top - obj.Position.Top;
                } else
                {
                    collisionRect = new Rectangle(obj.Position.Left, obj.Position.Bottom, obj.Position.Width, max);
                    var items = statics.GetAllInRegion(collisionRect);
                    dynamics.GetAllInRegion(collisionRect, ref items);
                    int bot = obj.Position.Bottom + max;
                    foreach (var item in items)
                    {
                        if (item.Position.Top > obj.Position.Bottom && obj.Position.Top < bot)
                            bot = item.Position.Top;
                    }
                    return bot - obj.Position.Bottom;
                }
            }
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
