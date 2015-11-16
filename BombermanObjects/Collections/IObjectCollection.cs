using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanObjects.Collections
{
    public interface IObjectCollection
    {
        /// <summary>
        /// Get all the objects in the collection that overlap the given point
        /// </summary>
        /// <param name="position">The point to check for</param>
        /// <returns>a <see cref="System.Collections.Generic.List{T}"/> such that all elements in the list overlap the given point</returns>
        List<IGameObject> GetAllAtPoint(Vector2 position);

        /// <summary>
        /// Get all the objects in the collection that overlap the given rectange
        /// the object overlaps the rectangle iff there exists at least one point that is in both
        /// </summary>
        /// <param name="position">The point to check for</param>
        /// <returns>a <see cref="System.Collections.Generic.List{T}"/> such that all elements in the list overlap the given rectangle</returns>
        List<IGameObject> GetAllInRegion(Rectangle box);

        /// <summary>
        /// Adds the given item to the Collection
        /// </summary>
        /// <param name="obj">the GameObject to add</param>
        void Add(IGameObject obj);

        /// <summary>
        /// Removes the given item from the collection
        /// </summary>
        /// <param name="obj">the item to remove</param>
        /// <returns></returns>
        bool Remove(IGameObject obj);
    }
}
