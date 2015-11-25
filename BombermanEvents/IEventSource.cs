using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanEvents
{
    public interface IEventSource
    {
        bool HasEvent { get; }

        GameEvent Next { get; }
    }
}
