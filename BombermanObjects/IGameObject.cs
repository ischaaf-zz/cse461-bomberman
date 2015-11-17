using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

namespace BombermanObjects
{
    public interface IGameObject : INotifyPropertyChanged
    {
        Rectangle Position { get; set; }

        void Draw(SpriteBatch spritebatch);
    }
}
