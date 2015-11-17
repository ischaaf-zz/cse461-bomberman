using BombermanObjects.Collections;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanTests
{
    class Program
    {
        static void Main(string[] args)
        {
            StaticObjectCollection sta = new StaticObjectCollection(100, 100, 5);

            TestGameObject obj1 = new TestGameObject(new Rectangle(10, 10, 20, 15), "obj1");
            sta.Add(obj1);

            TestGameObject obj2 = new TestGameObject(new Rectangle(10, 10, 3, 3), "obj2");
            sta.Add(obj2);

            Vector2 point = new Vector2(13, 12);
            Rectangle box = new Rectangle(10, 10, 1, 1);

            Console.WriteLine($"Objects instersecting ({point.X}, {point.Y}):");
            foreach (var obj in sta.GetAllAtPoint(point))
            {
                Console.WriteLine(obj.ToString());
            }

            Console.WriteLine($"Objects in box ({box.Left}, {box.Top}, {box.Width}, {box.Height}):");
            foreach (var obj in sta.GetAllInRegion(box))
            {
                Console.WriteLine(obj.ToString());
            }

            Console.Read();
        }
    }
}
