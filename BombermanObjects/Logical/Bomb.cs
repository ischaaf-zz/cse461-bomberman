using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BombermanObjects.Logical
{
    public class Bomb : IGameObject
    {

        public int x;
        public int y;
        public int dim;
        public TimeSpan placedAt;
        public TimeSpan detonateAt;
        public Player placedBy;

        public Bomb(int x, int y, TimeSpan placed, int ttd, Player placedBy, int dim)
        {
            this.x = x;
            this.y = y;
            this.dim = dim;
            this.placedAt = placed;
            detonateAt = placedAt.Add(new TimeSpan(0, 0, ttd));
            this.placedBy = placedBy;
        }

        public TimeSpan DetonateTime
        {
            get
            {
                return detonateAt;
            }
        }
        public Rectangle Position
        {
            get
            {
                return new Rectangle(x*dim, y*dim, dim, dim);
            }
        }
    }
}
