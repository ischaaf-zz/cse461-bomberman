using BombermanObjects.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects
{
    public struct Movement
    {
        public int Move;
        public Player.Direction Direction;

        public Movement(int m, Player.Direction d)
        {
            Move = m;
            Direction = d;
        }
    }
}
