using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using CommonGames.Utilities.CGTK;

using static CommonGames.Utilities.CGTK.CGDebug;
using static CommonGames.Utilities.Extensions.GeneralExtensions;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Physics
    {
        /*
        /// <summary>
        /// Returns a whether the given bounds are completely within this one.
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="otherBounds">The bounds to compare with this one</param>
        public static bool ContainsCompletely(this Bounds bounds, Bounds otherBounds)
            => (bounds.min.MinComponent() <= otherBounds.min.MinComponent()) 
               && (bounds.max.MinComponent() >= otherBounds.max.MinComponent());
        */
        
        public static bool GetBoundsWithChildren(this Bounds bounds, in Transform transform, ref bool encapsulate)
        {
            bool __looped = false;

            Renderer __renderer = transform.gameObject.GetComponent<Renderer>();
        
            if (__renderer != null)
            {
                Bounds __bounds = __renderer.bounds;
                if (encapsulate)
                {
                    bounds.Encapsulate(__bounds.min);
                    bounds.Encapsulate(__bounds.max);
                }
                else
                {
                    bounds.min = __bounds.min;
                    bounds.max = __bounds.max;
                    encapsulate = true;
                }
 
                __looped = true;
            }
        
            foreach (Transform __child in transform)
            {
                if (GetBoundsWithChildren(bounds, __child, ref encapsulate))
                {
                    __looped = true;
                }
            }
 
            return __looped;
        }

        #region Debugs

        public static void DebugCorners(this Bounds bounds, in float radius = 1f)
        {
            DrawSphere(bounds.TopLeftBack(), 1f).Color(Color.red);
            DrawSphere(bounds.TopRightBack(), 1f).Color(Color.blue);
            DrawSphere(bounds.BottomLeftBack(), 1f).Color(Color.blue);
            DrawSphere(bounds.BottomRightBack(), 1f).Color(Color.red);
        
            DrawSphere(bounds.TopLeftFront(), 1f).Color(Color.blue);
            DrawSphere(bounds.TopRightFront(), 1f).Color(Color.red);
            DrawSphere(bounds.BottomLeftFront(), 1f).Color(Color.red);
            DrawSphere(bounds.BottomRightFront(), 1f).Color(Color.blue);
        }
        
        public static void DebugCenter(this Bounds bounds, in float radius = 1f)
        {
            DrawCube(bounds.TopLeftBack(), size: Vector3.one * radius);
            //Gizmos.DrawCube(__bounds.center, size: Vector3.one * (__bounds.max.magnitude / 20f));
        }


        #endregion

        #region Back

        public static Vector3 TopLeftBack(this Bounds bounds)
        {
            Vector3 __corner = bounds.center;
            
            __corner.x += bounds.extents.x;
            __corner.y += bounds.extents.y;
            __corner.z += bounds.extents.z;
            
            return __corner;
        }
        
        public static Vector3 TopRightBack(this Bounds bounds)
        {
            Vector3 __corner = bounds.center;
            
            __corner.x -= bounds.extents.x;
            __corner.y += bounds.extents.y;
            __corner.z += bounds.extents.z;
            
            return __corner;
        }
        
        public static Vector3 BottomLeftBack(this Bounds bounds)
        {
            Vector3 __corner = bounds.center;
            
            __corner.x += bounds.extents.x;
            __corner.y -= bounds.extents.y;
            __corner.z += bounds.extents.z;
            
            return __corner;
        }
        
        public static Vector3 BottomRightBack(this Bounds bounds)
        {
            Vector3 __corner = bounds.center;
            
            __corner.x -= bounds.extents.x;
            __corner.y -= bounds.extents.y;
            __corner.z += bounds.extents.z;
            
            return __corner;
        }

        #endregion
        
        #region Front

        public static Vector3 TopLeftFront(this Bounds bounds)
        {
            Vector3 __corner = bounds.center;
            
            __corner.x += bounds.extents.x;
            __corner.y += bounds.extents.y;
            __corner.z -= bounds.extents.z;
            
            return __corner;
        }
        
        public static Vector3 TopRightFront(this Bounds bounds)
        {
            Vector3 __corner = bounds.center;
            
            __corner.x -= bounds.extents.x;
            __corner.y += bounds.extents.y;
            __corner.z -= bounds.extents.z;
            
            return __corner;
        }
        
        public static Vector3 BottomLeftFront(this Bounds bounds)
        {
            Vector3 __corner = bounds.center;
            
            __corner.x += bounds.extents.x;
            __corner.y -= bounds.extents.y;
            __corner.z -= bounds.extents.z;
            
            return __corner;
        }
        
        public static Vector3 BottomRightFront(this Bounds bounds)
        {
            Vector3 __corner = bounds.center;
            
            __corner.x -= bounds.extents.x;
            __corner.y -= bounds.extents.y;
            __corner.z -= bounds.extents.z;
            
            return __corner;
        }

        #endregion
        
    }
}