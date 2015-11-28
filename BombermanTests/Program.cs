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
            Collider c = new Collider(832, 832, 64);

            c.RegisterStatic(new TestGameObject(new Rectangle(0, 64, 64, 64), "Block"));
            TestGameObject player = new TestGameObject(new Rectangle(66, 64, 64, 64), "Player");
            Console.Read();
        }
    }
}
