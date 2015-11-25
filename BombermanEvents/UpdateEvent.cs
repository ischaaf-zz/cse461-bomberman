using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    // Contains the entire game state for the server's periodic updates
    class UpdateEvent : GameEvent
    {
        public UpdateEvent()
        {
            Type = EventType.Update;
        }
    }
}