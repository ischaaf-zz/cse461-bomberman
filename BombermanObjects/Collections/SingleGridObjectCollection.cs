using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BombermanObjects.Logical;
using Microsoft.Xna.Framework;

namespace BombermanObjects.Collections
{
    public class SingleGridObjectCollection
    {

        private AbstractGameObject[][] items;

        public int Dimension { get; }
        public int Width { get; }
        public int Height { get; }

        public SingleGridObjectCollection(int dim, int w, int h)
        {
            Dimension = dim;
            Width = w;
            Height = h;
            items = new AbstractGameObject[w][];
            for (int i = 0; i < w; i++)
            {
                items[i] = new AbstractGameObject[h];
            }
        }

        public void Add(AbstractGameObject obj)
        {
            Point loc = obj.CenterGrid;
            items[loc.X][loc.Y] = obj;
        }

        public AbstractGameObject GetAtPoint(Point position)
        {
            return items[position.X][position.Y];
        }

        public bool IsItemAtPoint(Point p)
        {
            return items[p.X][p.Y] != null;
        }

        public bool Remove(AbstractGameObject obj)
        {
            var loc = obj.CenterGrid;
            if (items[loc.X][loc.Y] == obj)
            {
                items[loc.X][loc.Y] = null;
                return true;
            } else
            {
                return false;
            }
        }

        public IEnumerator<AbstractGameObject> GetEnumerator()
        {
            return Next;
        }

        public IEnumerator<AbstractGameObject> Next
        {
            get
            {
                foreach (var arr1 in items)
                {
                    foreach (var item in arr1)
                    {
                        if (item != null)
                            yield return item;
                    }
                }
            }
        }
    }
}
