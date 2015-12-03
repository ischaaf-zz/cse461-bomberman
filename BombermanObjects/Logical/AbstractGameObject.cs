using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BombermanObjects.Logical
{
    public class AbstractGameObject : IGameObject
    {
        protected GameManager manager;

        public virtual Rectangle Position { get; set; }

        public Point CenterGrid
        {
            get
            {
                return new Point(Position.Center.X / GameManager.BOX_WIDTH, Position.Center.Y / GameManager.BOX_WIDTH);
            }
        }

        public AbstractGameObject(GameManager m)
        {
            manager = m;
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
