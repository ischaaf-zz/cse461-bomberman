using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    class StopEvent : GameEvent
    {
        int Player { get; }
        int X { get; }
        int Y { get; }

        public StopEvent(int player, int x, int y)
        {
            Player = player;
            X = x;
            Y = y;
            Type = EventType.Stop;
        }
    }
}
