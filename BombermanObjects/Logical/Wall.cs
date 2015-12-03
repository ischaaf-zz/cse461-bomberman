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
    public class Wall : AbstractGameObject
    {
        protected Rectangle position;

        public Wall(GameManager m, Rectangle position) : base(m)
        {
            this.position = position;
        }

        public override Rectangle Position
        {
            get
            {
                return position;
            }
        }
    }
}
