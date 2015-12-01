using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BombermanObjects.Logical;

namespace BombermanObjects
{
    public class LocalInput
    {
        private static readonly Dictionary<Keys, Player.Direction> BINDINGS = new Dictionary<Keys, Player.Direction>()
        {
            {Keys.Up, Player.Direction.North },
            {Keys.Right, Player.Direction.East },
            {Keys.Left, Player.Direction.West },
            {Keys.Down, Player.Direction.South }
        };
        private static readonly Keys PLACE_BOMB_KEY = Keys.Space;

        public LinkedList<Keys> KeysDown { get; private set; }
        public bool BombPlace { get; private set; }

        public PlayerInput CurrentInput
        {
            get
            {
                var m = new Player.Direction[KeysDown.Count];
                int i = 0;
                foreach (var d in KeysDown)
                {
                    m[i] = BINDINGS[d];
                    i++;
                }
                return new PlayerInput(BombPlace, m);
            }
        }

        private KeyboardState last;

        public LocalInput()
        {
            KeysDown = new LinkedList<Keys>();
            last = new KeyboardState();
        }

        public void Update(GameTime gametime)
        {
            // for each element in the list, if the key is not down, remove it
            // for each key that is down add it to the front if it is not in the list
            var current = Keyboard.GetState();
            var node = KeysDown.First;
            while (node != null)
            {
                var next = node.Next;
                if (!current.IsKeyDown(node.Value))
                {
                    KeysDown.Remove(node);
                }
                node = next;
            }

            foreach (var k in current.GetPressedKeys())
            {
                if (BINDINGS.ContainsKey(k) && !last.IsKeyDown(k))
                {
                    KeysDown.AddFirst(k);
                }
            }
            BombPlace = !last.IsKeyDown(PLACE_BOMB_KEY) && current.IsKeyDown(PLACE_BOMB_KEY);
            last = current;
        }
    }
}
