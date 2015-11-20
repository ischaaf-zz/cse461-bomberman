using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BombermanObjects.Logical;

namespace BombermanObjects.Collections
{
    public class DynamicObjectCollection : GameObjectCollection
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

        public override void GetAllAtPoint(Vector2 position, ref HashSet<IGameObject> ret)
        {
            foreach (var item in items)
            {
                if (item.Position.Contains(position))
                    ret.Add(item);
            }
        }

        public override void GetAllInRegion(Rectangle box, ref HashSet<IGameObject> ret)
        {
            foreach (var item in items)
            {
                if (item.Position.Intersects(box))
                    ret.Add(item);
            }
        }

        #endregion

        #region Mutators

        public override void Add(IGameObject obj)
        {
            items.Add(obj);
        }

        public override bool Remove(IGameObject obj)
        {
            return items.Remove(obj);
        }

        #endregion

        #endregion
    }
}
