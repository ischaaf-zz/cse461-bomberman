using BombermanObjects.Logical;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects.Collision
{
    public interface ICollider
    {
        bool RegisterStatic(IGameObject obj);

        bool UnRegisterStatic(IGameObject obj);

        bool RegisterDynamic(IGameObject obj);

        bool UnRegisterDynamic(IGameObject obj);

        /// <summary>
        /// return the max value that the object can move bounded by the given max move vector
        /// maxMove should contain one non-zero value and one zero value (move on one axis only)
        /// </summary>
        /// <param name="obj">the object to collide</param>
        /// <param name="maxMove">the max x or y movement to be made</param>
        /// <returns>the max distance the object can move bounded by maxMove</returns>
        Point Move(IGameObject obj, Point maxMove);
    }
}
