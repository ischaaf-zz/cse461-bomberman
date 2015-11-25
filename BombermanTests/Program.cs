using BombermanObjects.Collections;
using BombermanObjects.Collision;
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
            Collider c = new Collider(100, 100, 10);

            c.RegisterStatic(new TestGameObject(new Rectangle(40, 40, 20, 20), "Block"));
            TestGameObject player = new TestGameObject(new Rectangle(45, 70, 10, 10), "Player");

            
            int shortMove = c.Collide(player, new Vector2(0, -5));
            Console.WriteLine($"Short Move (5 expected): {shortMove}");

            int blockedMove = c.Collide(player, new Vector2(0, -50));
            Console.WriteLine($"Blocked Move (20 expected): {blockedMove}");

            Console.Read();
        }
    }
}
