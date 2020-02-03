using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 Multiply(this Vector3 a, in Vector3 b)
        => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
}
