using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    class BombDetEvent : GameEvent
    {
        int Player { get; }
        int X { get; }
        int Y { get; }
        int Power { get; }

        public BombDetEvent(int player, int x, int y, int power)
        {
            Player = player;
            X = x;
            Y = y;
            Power = power;
            Type = EventType.BombDet;
        }
    }
}
