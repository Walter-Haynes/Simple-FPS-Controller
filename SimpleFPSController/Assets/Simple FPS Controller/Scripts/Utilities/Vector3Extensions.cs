using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 Multiply(this Vector3 a, in Vector3 b)
        => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    
    public static float DistanceSquared(this Vector3 a, in Vector3 b)
        => (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
}
