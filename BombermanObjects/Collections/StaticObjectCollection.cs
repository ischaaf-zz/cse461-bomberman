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
        public int XBoxes { get; }
        public int YBoxes { get; }

        #endregion

        #region Members

        private DynamicObjectCollection[][] items;

        private delegate bool Fn(IGameObject obj, DynamicObjectCollection col);

        private int xWidth;
        private int yWidth;

        #endregion

        #region Constructers

        public StaticObjectCollection(int width, int height, int dim) : this(width, height, dim, dim) { }

        public StaticObjectCollection(int width, int height, int xDim, int yDim)
        {
            Width = width;
            Height = height;
            XBoxes = xDim;
            YBoxes = yDim;

            xWidth = Width / xDim;
            yWidth = Height / yDim;

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
            int x = (int)(position.X / xWidth);
            int y = (int)(position.Y / yWidth);

            items[x][y].GetAllAtPoint(position, ref current);
        }

        public override void GetAllInRegion(Rectangle box, ref HashSet<IGameObject> current)
        {
            int xLo = box.Left / XBoxes;
            xLo = xLo >= 0 ? xLo : 0;
            int xHi = box.Right / XBoxes;
            xHi = xHi < xWidth ? xHi : (xWidth - 1);
            int yLo = box.Top / YBoxes;
            yLo = yLo >= 0 ? yLo : 0;
            int yHi = box.Bottom / YBoxes;
            yHi = yHi < yWidth ? yHi : (yWidth - 1);

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
            int startX = rect.Left / XBoxes;
            int startY = rect.Top / YBoxes;
            bool success = true;
            while (startX * XBoxes < rect.Right)
            {
                while (startY * YBoxes < rect.Bottom)
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
