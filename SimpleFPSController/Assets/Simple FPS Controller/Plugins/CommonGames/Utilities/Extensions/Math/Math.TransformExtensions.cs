using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Math
    {
        public static Matrix4x4 LocalMatrix(this Transform transform)
            => Matrix4x4.TRS(pos: transform.localPosition, q: transform.localRotation, s: transform.localScale);

        public static Matrix4x4 WorldMatrix(this Transform transform)
            => Matrix4x4.TRS(pos: transform.position, q: transform.rotation, s: transform.lossyScale);
        
        #region Convert Methods

        public static Matrix4x4 GetMatrix(this Transform transform)
        {
            return transform.localToWorldMatrix;
        }

        public static Matrix4x4 RelativeMatrix(this Transform transform, in Transform relativeTo)
        {
            Matrix4x4 __myWorldMatrix = transform.localToWorldMatrix;
            return relativeTo.worldToLocalMatrix * __myWorldMatrix;
        }
        
        public static Vector3 GetRelativePosition(this Transform transform, Transform relativeTo)
        {
            return relativeTo.InverseTransformPoint(position: transform.position);
        }

        #endregion

        #region Matrix Methods

        /*
        public static Vector3 GetTranslation(this Matrix4x4 m)
        {
            Vector4 __col = m.GetColumn(3);
            return new Vector3(x: __col.x, y: __col.y, z: __col.z);
        }

        public static Quaternion GetRotation(this Matrix4x4 m)
        {
            // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
            Quaternion __q = new Quaternion
            {
                w = Mathf.Sqrt(f: Mathf.Max(0, b: 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2,
                x = Mathf.Sqrt(f: Mathf.Max(0, b: 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2,
                y = Mathf.Sqrt(f: Mathf.Max(0, b: 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2,
                z = Mathf.Sqrt(f: Mathf.Max(0, b: 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2
            };
            __q.x *= Mathf.Sign(f: __q.x * (m[2, 1] - m[1, 2]));
            __q.y *= Mathf.Sign(f: __q.y * (m[0, 2] - m[2, 0]));
            __q.z *= Mathf.Sign(f: __q.z * (m[1, 0] - m[0, 1]));
            return __q;
        }

        public static Vector3 GetScale(this Matrix4x4 m)
        {
            //var xs = m.GetColumn(0);
            //var ys = m.GetColumn(1);
            //var zs = m.GetColumn(2);

            //var sc = new Vector3();
            //sc.x = Vector3.Magnitude(new Vector3(xs.x, xs.y, xs.z));
            //sc.y = Vector3.Magnitude(new Vector3(ys.x, ys.y, ys.z));
            //sc.z = Vector3.Magnitude(new Vector3(zs.x, zs.y, zs.z));

            //return sc;

            return new Vector3(x: m.GetColumn(0).magnitude, y: m.GetColumn(1).magnitude, z: m.GetColumn(2).magnitude);
        }
        */

        #endregion

        #region Parent

        public static Vector3 ParentTransformPoint(this Transform t, Vector3 pnt)
        {
            Transform __parent = t.parent;
            return __parent == null ? pnt : __parent.TransformPoint(position: pnt);
        }

        public static Vector3 ParentInverseTransformPoint(this Transform t, Vector3 pnt)
        {
            Transform __parent = t.parent;
            return __parent == null ? pnt : __parent.InverseTransformPoint(position: pnt);
        }

        #endregion
    }
}