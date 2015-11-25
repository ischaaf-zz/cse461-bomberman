using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    class BombMoveEvent : GameEvent
    {
        Direction Direction { get; }
        int X { get; }
        int Y { get; }

        public BombMoveEvent(Direction direction, int x, int y)
        {
            Direction = direction;
            X = x;
            Y = y;
            Type = EventType.BombMove;
        }
    }
}
