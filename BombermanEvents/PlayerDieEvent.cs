using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    class PlayerDieEvent : GameEvent
    {
        int Victim { get; }
        int pKiller { get; }
        int X { get; }
        int Y { get; }
  

        public PlayerDieEvent(int victim, int killer, int x, int y)
        {
            Victim = victim;
            pKiller = killer;
            X = x;
            Y = y;
            Type = EventType.PlayerDie;
        }
    }
}
