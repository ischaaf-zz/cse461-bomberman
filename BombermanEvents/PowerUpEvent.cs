using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    class PowerUpEvent : GameEvent
    {
        int Player { get; }
        PowerUp Power { get; }

        public PowerUpEvent(int player, PowerUp power)
        {
            Player = player;
            Power = power;
            Type = EventType.PowerUp;
        }
    }

    public enum PowerUp
    {
        BombCapacity, BombStrength, RunSpeed
    }
}
