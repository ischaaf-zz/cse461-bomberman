using BombermanObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace BombermanTests
{
    class TestGameObject : IGameObject
    {

        public Rectangle Position { get; set; }

        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TestGameObject(Rectangle position, string name)
        {
            Name = name;
            Position = position;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritebatch)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
