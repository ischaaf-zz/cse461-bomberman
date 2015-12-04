using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BombermanObjects.Logical
{
    public class Box : AbstractGameObject
    {
        public PowerUp PowerUp { get; set; }
        protected Rectangle position;

        public override Rectangle Position
        {
            get
            {
                return position;
            }
        }

        public Box(GameManager m, int x, int y, PowerUp p) : base(m)
        {
            position = new Rectangle(x * GameManager.BOX_WIDTH, y * GameManager.BOX_WIDTH, GameManager.BOX_WIDTH, GameManager.BOX_WIDTH);
            PowerUp = p;
        }

        public void Destroy()
        {
            manager.statics.Remove(this);
            manager.collider.UnRegisterStatic(this);
            if (PowerUp != null)
            {
                manager.powerUps.Add(PowerUp);
            }
        }
    }
}
