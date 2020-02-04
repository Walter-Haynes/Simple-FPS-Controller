using UnityEngine;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Math
    {
        public static Vector3 Position(this Matrix4x4 m)
            => new Vector3(m[0, 3], m[1, 3], m[2, 3]);

        public static Vector3 Scale(this Matrix4x4 m)
            => new Vector3 (m.GetColumn(0).magnitude, m.GetColumn(1).magnitude, m.GetColumn(2).magnitude);

        public static Quaternion Rotation(this Matrix4x4 m)
        {
            Vector3 s = Scale(m);

            // Normalize Scale from Matrix4x4
            float m00 = m[0, 0] / s.x;
            float m01 = m[0, 1] / s.y;
            float m02 = m[0, 2] / s.z;
            float m10 = m[1, 0] / s.x;
            float m11 = m[1, 1] / s.y;
            float m12 = m[1, 2] / s.z;
            float m20 = m[2, 0] / s.x;
            float m21 = m[2, 1] / s.y;
            float m22 = m[2, 2] / s.z;

            Quaternion q = new Quaternion
            {
                w = Mathf.Sqrt(Mathf.Max(0, 1 + m00 + m11 + m22)) / 2,
                x = Mathf.Sqrt(Mathf.Max(0, 1 + m00 - m11 - m22)) / 2,
                y = Mathf.Sqrt(Mathf.Max(0, 1 - m00 + m11 - m22)) / 2,
                z = Mathf.Sqrt(Mathf.Max(0, 1 - m00 - m11 + m22)) / 2
            };

            q.x *= Mathf.Sign(q.x * (m21 - m12));
            q.y *= Mathf.Sign(q.y * (m02 - m20));
            q.z *= Mathf.Sign(q.z * (m10 - m01));

            // q.Normalize()
            float qMagnitude = Mathf.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z);
            q.w /= qMagnitude;
            q.x /= qMagnitude;
            q.y /= qMagnitude;
            q.z /= qMagnitude;

            return q;
        }
    }
}