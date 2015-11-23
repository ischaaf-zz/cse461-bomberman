using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    public class MoveEvent : GameEvent
    {
        public int Player { get; }
        public Direction Direction { get; }

        public MoveEvent(int player, Direction direction)
        {
            Player = player;
            Direction = direction;
        }
    }

    public enum Direction
    {
        North, South, East, West
    }
}
