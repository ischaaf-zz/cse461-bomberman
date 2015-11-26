using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    public abstract class IEventSource
    {
        bool HasEvent { get; }

        GameEvent Next { get; }

        public abstract void Send(GameEvent gEvent, int id);

    }
}
