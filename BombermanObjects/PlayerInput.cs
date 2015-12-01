using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects
{
    public struct PlayerInput
    {
        public bool PlaceBomb;
        public Logical.Player.Direction[] Move;

        public PlayerInput(bool place, Logical.Player.Direction[] m)
        {
            PlaceBomb = place;
            Move = m;
        }
    }
}
