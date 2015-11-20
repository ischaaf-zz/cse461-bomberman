using Microsoft.Xna.Framework;
using BombermanObjects.Logical;

namespace BombermanTests
{
    class TestGameObject : IGameObject
    {

        public Rectangle Position { get; set; }

        public string Name { get; set; }

        public TestGameObject(Rectangle position, string name)
        {
            Name = name;
            Position = position;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
