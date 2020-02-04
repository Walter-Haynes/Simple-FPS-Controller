using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGames.Utilities.CGTK;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Math
    {
        public static Quaternion AverageQuaternion(this List<Quaternion> quaternions) => CGMath.AverageQuaternion(quaternions: quaternions);

        public static Quaternion AverageQuaternion(this Quaternion[] quaternions) => CGMath.AverageQuaternion(quaternions: quaternions);

        public static Quaternion AverageQuaternion(this Quaternion q1, Quaternion q2) => CGMath.AverageQuaternion(q1, q2);

        public static Quaternion QuaternionFromMatrix(this Matrix4x4 m) => CGMath.QuaternionFromMatrix(m: m);

        /*
        public static Quaternion NormalizedQuaternion(this Quaternion q)
        {
            float invMag = 1.0f / Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
            q.x *= invMag;
            q.y *= invMag;
            q.z *= invMag;
            q.w *= invMag;

            return q;
        }
        */

    }
}
