using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects.Logical
{
    public class Explosion : AbstractGameObject
    {
        public TimeSpan RemoveAt { get; }

        public Explosion(GameManager m, int x, int y, int dim, TimeSpan startedAt) : base(m)
        {
            Position = new Rectangle(x * dim, y * dim, dim, dim);
            RemoveAt = startedAt.Add(new TimeSpan(0, 0, 1));
        }
    }
}
