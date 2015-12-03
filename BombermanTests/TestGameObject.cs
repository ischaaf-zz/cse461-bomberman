using Microsoft.Xna.Framework;
using BombermanObjects.Logical;
using BombermanObjects;

namespace BombermanTests
{
    class TestGameObject : AbstractGameObject
    {
        public string Name { get; set; }

        public TestGameObject(GameManager m, Rectangle position, string name) : base(m)
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
