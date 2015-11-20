using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BombermanObjects.Logical
{
    public class Wall : IGameObject
    {
        protected Rectangle position;

        public Wall(Rectangle position)
        {
            this.position = position;
        }

        public Rectangle Position
        {
            get
            {
                return position;
            }
        }
    }
}
