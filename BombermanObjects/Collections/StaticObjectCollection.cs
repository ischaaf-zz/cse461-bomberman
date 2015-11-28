using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BombermanObjects.Logical;

namespace BombermanObjects.Collections
{
    public class StaticObjectCollection : GameObjectCollection
    {
        #region Properties

        public int Width { get; }
        public int Height { get; }
        public int XBoxSize { get; }
        public int YBoxSize { get; }

        #endregion

        #region Members

        private DynamicObjectCollection[][] items;

        private delegate bool Fn(IGameObject obj, DynamicObjectCollection col);

        private int xBoxes;
        private int yBoxes;

        #endregion

        #region Constructers

        public StaticObjectCollection(int width, int height, int dim) : this(width, height, dim, dim) { }

        public StaticObjectCollection(int width, int height, int xDim, int yDim)
        {
            Width = width;
            Height = height;
            XBoxSize = xDim;
            YBoxSize = yDim;

            xBoxes = Width / xDim;
            yBoxes = Height / yDim;

            items = new DynamicObjectCollection[xDim][];
            for (int i = 0; i < xDim; i++)
            {
                items[i] = new DynamicObjectCollection[yDim];
                for (int j = 0; j < yDim; j++)
                {
                    items[i][j] = new DynamicObjectCollection();
                }
            }
        }

        #endregion

        #region Public Member Functions

        #region Queries

        public override void GetAllAtPoint(Vector2 position, ref HashSet<IGameObject> current)
        {
            int x = (int)(position.X / XBoxSize);
            int y = (int)(position.Y / YBoxSize);

            items[x][y].GetAllAtPoint(position, ref current);
        }

        public override void GetAllInRegion(Rectangle box, ref HashSet<IGameObject> current)
        {
            int xLo = box.Left / XBoxSize;
            xLo = xLo >= 0 ? xLo : 0;
            int xHi = box.Right / XBoxSize;
            xHi = xHi < xBoxes ? xHi : (xBoxes - 1);
            int yLo = box.Top / YBoxSize;
            yLo = yLo >= 0 ? yLo : 0;
            int yHi = box.Bottom / YBoxSize;
            yHi = yHi < yBoxes ? yHi : (yBoxes - 1);

            for (int i = xLo; i <= xHi; ++i)
            {
                for (int j = yLo; j <= yHi; ++j)
                {
                    items[i][j].GetAllInRegion(box, ref current);
                }
            }

        }

        #endregion

        #region Mutators

        public override void Add(IGameObject obj)
        {
            opHelper(obj, insert);
        }

        public override bool Remove(IGameObject obj)
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
            if (rect.Left < 0 || rect.Top < 0 || rect.Right > Width || rect.Bottom > Height)
                throw new ArgumentException("obj must be completely within the bounds of the collection");
            int startX = rect.Left / XBoxSize;
            int startY = rect.Top / YBoxSize;
            bool success = true;
            while (startX * XBoxSize < rect.Right)
            {
                while (startY * YBoxSize < rect.Bottom)
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
