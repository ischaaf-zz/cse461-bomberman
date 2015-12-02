using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects.Logical
{
    public class Explosion : IGameObject
    {
        public Rectangle Position { get; set; }
        public TimeSpan RemoveAt { get; }

        public Explosion(int x, int y, int dim, TimeSpan startedAt)
        {
            Position = new Rectangle(x * dim, y * dim, dim, dim);
            RemoveAt = startedAt.Add(new TimeSpan(0, 0, 1));
        }
    }
}
