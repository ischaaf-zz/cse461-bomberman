using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BombermanObjects.Collections
{
    public class DynamicObjectCollection : IObjectCollection
    {
        #region Members

        private HashSet<IGameObject> items;

        #endregion

        #region Constructors
        
        public DynamicObjectCollection()
        {
            items = new HashSet<IGameObject>();
        }

        #endregion

        #region Public Member Functions

        #region Queries

        public List<IGameObject> GetAllAtPoint(Vector2 position)
        {
            List<IGameObject> ret = new List<IGameObject>();
            foreach (var item in items)
            {
                if (item.Position.Contains(position))
                    ret.Add(item);
            }
            return ret;
        }

        public List<IGameObject> GetAllInRegion(Rectangle box)
        {
            List<IGameObject> ret = new List<IGameObject>();
            foreach (var item in items)
            {
                if (item.Position.Intersects(box))
                    ret.Add(item);
            }
            return ret;
        }

        #endregion

        #region Mutators

        public void Add(IGameObject obj)
        {
            items.Add(obj);
        }

        public bool Remove(IGameObject obj)
        {
            return items.Remove(obj);
        }

        #endregion

        #endregion
    }
}
