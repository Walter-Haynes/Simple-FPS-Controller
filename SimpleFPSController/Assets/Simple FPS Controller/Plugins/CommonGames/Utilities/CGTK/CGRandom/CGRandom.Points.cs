using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace CommonGames.Utilities.CGTK
{
	 public static partial class CGRandom
	 {
		 /*
		 public static Vector3[] PointsOnSphere(int n)
		 {
			 List<Vector3> randomPoints = new List<Vector3>(n);
			 
			 float increment = Mathf.PI * (3 - Mathf.Sqrt(5));
			 float offset = 2f / n;
	 
			 for (int i = 0; i < n; i++)
			 {
				 float y = i * offset - 1 + (offset /2);
				 float r = Mathf.Sqrt(1 - y * y);
				 float phi = i * increment;
				 float x = Mathf.Cos(phi) * r;
				 float z = Mathf.Sin(phi) * r;
					
				 randomPoints.Add(new Vector3(x, y, z));
			 }
			 Vector3[] pts = randomPoints.ToArray();
			 return pts;
		 }
		 */

		 #region Sphere
		 
		 public static Vector3[] PointsOnUnitSphere(int n, float radius)
		 {
			 List<Vector3> randomPoints = new List<Vector3>(n);
	 
			 for (int i = 0; i < n; i++)
			 {
				 randomPoints.Add(Random.onUnitSphere * radius);
			 }
	 
			 return randomPoints.ToArray();
		 }
		 
		 #endregion

		 #region Circle
		 
		 //TODO: Add Axis
		 public static Vector3[] PointsOnUnitCircle(int n, float radius)
		 {
			 List<Vector3> randomPoints = new List<Vector3>(n);
			 
			 for (int i = 0; i < n; i++)
			 {
				 Vector2 randomPoint2D = Random.insideUnitCircle.normalized * radius; 
				 
				 randomPoints.Add(new Vector3(randomPoint2D.x, 0,randomPoint2D.y));
			 }
	 
			 return randomPoints.ToArray();
		 }
		 
		 #endregion
	 
		 #region HemiSphere
		 
		 /// <summary> Gets random points on a hemisphere. </summary>
		 public static Vector3[] PointsOnHemiSphere(int n, float radius)
			 => PointsOnHemiSphere(n, radius, Vector3.up);
		 
		 /// <summary> Gets random points on a hemisphere with direction axis.</summary>
		 public static Vector3[] PointsOnHemiSphere(int n, float radius, Vector3 axis)
		 {
			 Vector3[] randomPoints = new Vector3[n];
	 
			 for (int i = 0; i < n; i++)
			 {
				 randomPoints[i] = PointOnHemiSphere(radius: radius, axis: axis);
			 }
	 
			 return randomPoints;
		 }
	 
		 
		 /// <summary> Gets a random point on a hemisphere.</summary>
		 public static Vector3 PointOnHemiSphere(float radius)
			 => PointOnHemiSphere(radius: radius, axis: Vector3.up);
		 
		 /// <summary> Gets a random point on a hemisphere with direction axis. </summary>
		 public static Vector3 PointOnHemiSphere(float radius, Vector3 axis)
		 {
			 Vector3 randomPoint = Random.onUnitSphere* radius;
			 randomPoint.y = Mathf.Abs(randomPoint.y);
			 
			 Quaternion rotation = Quaternion.LookRotation(axis.normalized, Vector3.forward);
			 
			 Matrix4x4 matrix = Matrix4x4.Rotate(rotation);
			 randomPoint = matrix.MultiplyPoint3x4(randomPoint);
	 
			 return randomPoint;
		 }
		 
		 #endregion
	 
	}
}