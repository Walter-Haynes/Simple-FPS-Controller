namespace CommonGames.Utilities.CGTK
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Utilities.Extensions;
    using static System.Math;

    public static partial class CGMath
    {
        public static Quaternion Eulernion(float roll, float pitch, float yaw)
        {
            roll *= Mathf.Deg2Rad / 2f;
            pitch *= Mathf.Deg2Rad / 2f;
            yaw *= Mathf.Deg2Rad / 2f;

            //TODO: Replace this.
            Vector3 forward = Vector3.forward;
            Vector3 right = Vector3.right;
            Vector3 up = Vector3.up;

            float sin = (float)Sin(roll);
            float cos = (float)Cos(roll);
            Quaternion q1 = new Quaternion(0f, 0f, (forward.z * sin), cos);

            sin = (float)Sin(pitch);
            cos = (float)Cos(pitch);
            Quaternion q2 = new Quaternion((right.x * sin), 0f, 0f, cos);

            sin = (float)Sin(yaw);
            cos = (float)Cos(yaw);
            Quaternion q3 = new Quaternion(0f, (up.y * sin), 0f, cos);

            return MultiplyQuaternions(MultiplyQuaternions(q1, q2), q3);
        }

        public static Quaternion MultiplyQuaternions(Quaternion q1, Quaternion q2)
        {
            float x = q1.x * q2.w + q1.y * q2.z - q1.z * q2.y + q1.w * q2.x;
            float y = -q1.x * q2.z + q1.y * q2.w + q1.z * q2.x + q1.w * q2.y;
            float z = q1.x * q2.y - q1.y * q2.x + q1.z * q2.w + q1.w * q2.z;
            float w = -q1.x * q2.x - q1.y * q2.y - q1.z * q2.z + q1.w * q2.w;

            return new Quaternion(x, y, z, w);
        }

        public static Quaternion AverageQuaternion(List<Quaternion> quaternions)
        {
            Vector3 forward = Vector3.zero;
            Vector3 upwards = Vector3.zero;

            foreach (Quaternion quaternion in quaternions)
            {
                forward += quaternion * Vector3.forward;
                upwards += quaternion * Vector3.up;
            }

            forward /= quaternions.Count;
            upwards /= quaternions.Count;

            return Quaternion.LookRotation(forward, upwards);
        }

        public static Quaternion AverageQuaternion(params Quaternion[] quaternions)
        {
            Vector3 forward = Vector3.zero;
            Vector3 upwards = Vector3.zero;

            foreach (Quaternion quaternion in quaternions)
            {
                forward += quaternion * Vector3.forward;
                upwards += quaternion * Vector3.up;
            }

            forward /= quaternions.Length;
            upwards /= quaternions.Length;

            return Quaternion.LookRotation(forward, upwards);
        }

        public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
        {
            // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.html
            Quaternion q = new Quaternion
            {
                w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2,
                x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2,
                y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2,
                z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2
            };

            q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
            q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
            q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));

            return q.normalized;
        }

        public static bool AreQuaternionsClose(Quaternion q1, Quaternion q2)
            => (Quaternion.Dot(q1, q2) < 0.0f) ? false : true;
    }
}