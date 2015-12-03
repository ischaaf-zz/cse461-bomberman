﻿using BombermanObjects.Logical;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BombermanObjects.Collections
{
    public class GridObjectCollection
    {
        private List<AbstractGameObject>[][] items;

        public int Dimension { get; }
        public int Width { get; }
        public int Height { get; }

        public GridObjectCollection(int dim, int w, int h)
        {
            Dimension = dim;
            Width = w;
            Height = h;
            items = new List<AbstractGameObject>[w][];
            for (int i = 0; i < w; i++)
            {
                items[i] = new List<AbstractGameObject>[h];
                for (int j = 0; j < h; j++)
                {
                    items[i][j] = new List<AbstractGameObject>();
                }
            }
        }

        public void Add(AbstractGameObject obj)
        {
            Point loc = obj.CenterGrid;
            items[loc.X][loc.Y].Add(obj);
        }

        public List<AbstractGameObject> GetAtPoint(Point position)
        {
            return items[position.X][position.Y];
        }

        public bool IsItemAtPoint(Point p)
        {
            return items[p.X][p.Y].Count != 0;
        }

        public bool Remove(AbstractGameObject obj)
        {
            var loc = obj.CenterGrid;
            var list = items[loc.X][loc.Y];
            foreach (var item in list)
            {
                if (item == obj)
                {
                    list.Remove(item);
                    return true;
                }
            }
            return false;
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
                    foreach (var arr2 in arr1)
                    {
                        foreach (var item in arr2)
                        {
                            if (item != null)
                                yield return item;
                        }
                    }
                }
            }
        }
    }
}
