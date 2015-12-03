using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

namespace BombermanObjects.Logical
{
    public interface IGameObject
    {
        Rectangle Position { get; }

        void Update(GameTime gameTime);
    }
}
