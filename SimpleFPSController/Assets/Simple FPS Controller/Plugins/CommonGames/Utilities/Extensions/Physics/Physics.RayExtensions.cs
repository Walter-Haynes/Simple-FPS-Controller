using UnityEngine;

namespace Utilities.Extensions
{
    public static partial class Physics
    {
        /// <summary>
        /// Returns the closest point on a ray to "position".
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 ClosestPoint(this Ray ray, Vector3 position)
        {
            Vector3 point = position - ray.origin;
            Vector3 projection = Vector3.Project(point, ray.direction);
            return point - projection;
        }
    }
}