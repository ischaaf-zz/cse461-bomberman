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
        public List<IGameObject> GetAllAtPoint(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public List<IGameObject> GetAllInRegion(Rectangle box)
        {
            throw new NotImplementedException();
        }
    }
}
