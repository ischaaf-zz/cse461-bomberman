using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BombermanEvents
{
    public class LocalEventSource : IEventSource
    {

        private ConcurrentQueue<GameEvent> events;
        private Task runner; 

        public LocalEventSource()
        {
            events = new ConcurrentQueue<GameEvent>();
            runner = new Task(pollEvents);
        }

        public void Start()

        {
            runner.Start();
        }

        private void pollEvents()
        {
            
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void Send(GameEvent gEvent, int id)
        {
            throw new NotImplementedException();
        }

        public bool HasEvent
        {
            get
            {
                return !events.IsEmpty;
            }
        }

        public GameEvent Next
        {
            get
            {
                GameEvent e;
                bool s = events.TryDequeue(out e);
                if (s)
                    return e;
                else
                    throw new ArgumentException("no events in queue");
            }
        }
    }
}
