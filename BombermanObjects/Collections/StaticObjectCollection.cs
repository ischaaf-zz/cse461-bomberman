using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BombermanObjects.Collections
{
    public class StaticObjectCollection : IObjectCollection
    {
        #region Properties

        public int Width { get; }
        public int Height { get; }
        public int XBoxes { get; }
        public int YBoxes { get; }

        #endregion

        #region Members

        private DynamicObjectCollection[][] items;

        private delegate bool Fn(IGameObject obj, DynamicObjectCollection col);

        #endregion

        #region Constructers

        public StaticObjectCollection(int width, int height, int dim) : this(width, height, dim, dim) { }

        public StaticObjectCollection(int width, int height, int xDim, int yDim)
        {
            Width = width;
            Height = height;
            XBoxes = xDim;
            YBoxes = yDim;
            items = new DynamicObjectCollection[xDim][];
            for (int i = 0; i < xDim; i++)
            {
                items[i] = new DynamicObjectCollection[yDim];
            }
        }

        #endregion

        #region Public Member Functions

        #region Queries

        public List<IGameObject> GetAllAtPoint(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public List<IGameObject> GetAllInRegion(Rectangle box)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Mutators

        public void Add(IGameObject obj)
        {
            opHelper(obj, insert);
        }

        public bool Remove(IGameObject obj)
        {
            return opHelper(obj, remove);
        }

        #endregion

        #endregion

        #region Private Member Functions

        private bool opHelper(IGameObject obj, Fn op)
        {
            var rect = obj.Position;
            if (rect == null)
                throw new ArgumentNullException("obj cannot be null");
            if (rect.Left < 0 || rect.Top < 0 || rect.Right >= Width || rect.Bottom >= Height)
                throw new ArgumentException("obj must be completely within the bounds of the collection");
            int startX = rect.Left / XBoxes;
            int startY = rect.Top / YBoxes;
            bool success = true;
            while (startX * (Width / XBoxes) < rect.Right)
            {
                while (startY * (Height / YBoxes) < rect.Bottom)
                {
                    success &= op(obj, items[startX][startY]);
                    ++startY;
                }
                ++startX;
            }
            return success;
        }

        private bool insert(IGameObject obj, DynamicObjectCollection col)
        {
            col.Add(obj);
            return true;
        }

        private bool remove(IGameObject obj, DynamicObjectCollection col)
        {
            return col.Remove(obj);
        }

        #endregion
    }
}
