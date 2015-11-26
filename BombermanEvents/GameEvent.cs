using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    public abstract class GameEvent
    {
        public EventType Type { get; set; }
    }
}
