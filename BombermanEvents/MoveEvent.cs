using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    public class MoveEvent : GameEvent
    {
        public Direction Direction { get; }
        public int Player { get; }

        public MoveEvent(int player, Direction direction)
        {
            Direction = direction;
            Player = player;
            Type = EventType.Move;
        }
    }

    public enum Direction
    {
        North, South, East, West
    }
}
