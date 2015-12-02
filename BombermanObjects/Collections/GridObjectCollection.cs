using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BombermanObjects.Logical;
using Microsoft.Xna.Framework;

namespace BombermanObjects.Collections
{
    public class GridObjectCollection : GameObjectCollection
    {

        private List<List<IGameObject>> items;

        public int Dimension { get; }
        public int Width { get; }
        public int Height { get; }

        public GridObjectCollection(int dim, int w, int h)
        {
            Dimension = dim;
            Width = w;
            Height = h;
            items = new List<List<IGameObject>>(w);
            for (int i = 0; i < w; i++)
            {
                var l = new List<IGameObject>(h);
                for (int j = 0; j < h; j++)
                {
                    l.Add(null);
                }
                items.Add(l);
            }
        }

        public override void Add(IGameObject obj)
        {
            var rect = obj.Position;
            if (rect.Left % Dimension != 0 || rect.Width != Dimension || rect.Height != Dimension)
                throw new ArgumentException($"Item not {Dimension}-aligned");
            int w = rect.Center.X / Dimension;
            int h = rect.Center.Y / Dimension;
            items[w][h] = obj;
        }

        public override void GetAllAtPoint(Vector2 position, ref HashSet<IGameObject> current)
        {
            int w = (int)position.X / Dimension;
            int h = (int)position.Y / Dimension;
            if (items[w][h] != null)
                current.Add(items[w][h]);
        }

        public override void GetAllInRegion(Rectangle box, ref HashSet<IGameObject> current)
        {
            throw new NotImplementedException();
        }

        public bool IsItemAtPoint(Point p)
        {
            return items[p.X / Dimension][p.Y / Dimension] != null;
        }

        public override bool Remove(IGameObject obj)
        {
            var rect = obj.Position;
            if (rect.Left % Dimension != 0 || rect.Width != Dimension || rect.Height != Dimension)
                throw new ArgumentException($"Item not {Dimension}-aligned");
            int w = rect.Center.X / Dimension;
            int h = rect.Center.Y / Dimension;
            if (items[w][h] == obj)
            {
                items[w][h] = null;
                return true;
            } else
            {
                return false;
            }
        }
    }
}
