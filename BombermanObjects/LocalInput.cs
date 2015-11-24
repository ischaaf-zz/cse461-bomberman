using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects
{
    public class LocalInput
    {
        public LinkedList<Keys> Keys { get; private set; }
        private KeyboardState last;

        public LocalInput()
        {
            Keys = new LinkedList<Keys>();
            last = new KeyboardState();
        }

        public void Update(GameTime gametime)
        {
            // for each element in the list, if the key is not down, remove it
            // for each key that is down add it to the front if it is not in the list
            var current = Keyboard.GetState();
            var node = Keys.First;
            while (node != null)
            {
                var next = node.Next;
                if (!current.IsKeyDown(node.Value))
                {
                    Keys.Remove(node);
                }
                node = next;
            }

            foreach (var k in current.GetPressedKeys())
            {
                if (!last.IsKeyDown(k))
                {
                    Keys.AddFirst(k);
                }
            }
            last = current;
        }
    }
}
