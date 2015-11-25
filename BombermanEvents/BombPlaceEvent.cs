using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    class BombPlaceEvent : GameEvent
    {
        int Player { get; }
        int X { get; }
        int Y { get; }
        int Power { get; }
        double Time { get; }

        public BombPlaceEvent(int player, int x, int y, int power, double time)
        {
            Player = player;
            X = x;
            Y = y;
            Power = power;
            Time = time;
            Type = EventType.BombPlace;
        }
    }
}
