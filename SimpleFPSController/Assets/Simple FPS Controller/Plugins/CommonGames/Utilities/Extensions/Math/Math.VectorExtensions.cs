using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using CommonGames.Utilities.CGTK;

using JetBrains.Annotations;

namespace CommonGames.Utilities.Extensions
{
	public static partial class Math
	{
		#region Vector3

		#region Distances

		//public static Vector3 MaxValue(this Vector3 vector) => new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);	
		
		[PublicAPI]
		public static Vector3 Closest(this IEnumerable<Vector3> vectors, in Vector3 comparer)
		{
			Vector3 __closestVector = Vector3.positiveInfinity;
			float __closestDistance = float.MaxValue;

			foreach (Vector3 __vector in vectors)
			{
				float __distance = __vector.DistanceTo(comparer);

				if (!(__distance <= __closestDistance)) continue;
				
				__closestDistance = __distance;
				__closestVector = __vector;
			}
			
			return __closestVector;
		}
		
		[PublicAPI]
		public static int IndexOfClosest(this IEnumerable<Vector3> vectors, in Vector3 comparer)
		{
			int __indexOfClosest = 0;
			float __closestDistance = float.MaxValue;

			int __indexCounter = 0;
			foreach (Vector3 __vector in vectors)
			{
				__indexCounter++;
				
				float __distance = __vector.DistanceTo(comparer);

				if (!(__distance <= __closestDistance)) continue;

				__indexOfClosest = (__indexCounter - 1);
				__closestDistance = __distance;
			}
			
			return __indexOfClosest;
		}
		
		[PublicAPI]
		public static (Vector3 closestPosition, int indexOfClosestPosition) ClosestWithIndex(this IEnumerable<Vector3> vectors, in Vector3 comparer)
		{
			int __indexOfClosest = 0;
			Vector3 __closestVector = Vector3.positiveInfinity;
			float __closestDistance = float.MaxValue;

			int __indexCounter = 0;
			foreach (Vector3 __vector in vectors)
			{
				__indexCounter++;
				
				float __distance = __vector.DistanceTo(comparer);

				if (!(__distance <= __closestDistance)) continue;

				__indexOfClosest = (__indexCounter - 1);
				__closestDistance = __distance;
				__closestVector = __vector;
			}
			
			return (__closestVector, __indexOfClosest);
		}

		[PublicAPI]
		public static Vector3 SecondClosest(this IEnumerable<Vector3> vectors, in Vector3 comparer)
		{
			Vector3 __closestVector = Vector3.positiveInfinity;
			Vector3 __secondClosestVector = Vector3.positiveInfinity;
			float __closestDistance = float.MaxValue;

			foreach (Vector3 __vector in vectors)
			{
				float __distance = __vector.DistanceTo(comparer);

				if (!(__distance <= __closestDistance)) continue;

				__secondClosestVector = __closestVector;
				
				__closestDistance = __distance;
				__closestVector = __vector;
			}
			
			return __secondClosestVector;
		}
		
		[PublicAPI]
		public static int IndexOfSecondClosest(this IEnumerable<Vector3> vectors, in Vector3 comparer)
		{
			int __indexOfClosest = 0;
			float __closestDistance = float.MaxValue;

			int __indexCounter = 0;
			foreach (Vector3 __vector in vectors)
			{
				__indexCounter++;
				
				float __distance = __vector.DistanceTo(comparer);

				if (!(__distance <= __closestDistance)) continue;

				__indexOfClosest = (__indexCounter - 1);
				__closestDistance = __distance;
			}
			
			return __indexOfClosest;
		}
		
		[PublicAPI]
		public static (Vector3 secondClosestPosition, int indexOfSecondClosestPosition) SecondClosestWithIndex(this IEnumerable<Vector3> vectors, in Vector3 comparer)
		{
			int __indexOfClosest = 0;
			int __indexOfSecondClosest = 0;
			Vector3 __closestVector = Vector3.positiveInfinity;
			Vector3 __secondClosestVector = Vector3.positiveInfinity;
			float __closestDistance = float.MaxValue;

			int __indexCounter = 0;
			foreach (Vector3 __vector in vectors)
			{
				__indexCounter++;
				
				float __distance = __vector.DistanceTo(comparer);

				if (!(__distance <= __closestDistance)) continue;

				__indexOfSecondClosest = __indexOfClosest;
				__secondClosestVector = __closestVector;

				__indexOfClosest = (__indexCounter - 1);
				__closestDistance = __distance;
				__closestVector = __vector;
			}
			
			return (__secondClosestVector, __indexOfSecondClosest);
		}
		
		[PublicAPI]
		public static (Vector3 closestVector, Vector3 secondClosestVector) TwoClosest(this IEnumerable<Vector3> vectors, in Vector3 comparer)
		{
			Vector3 __closestVector = Vector3.positiveInfinity;
			Vector3 __secondClosestVector = __closestVector;
			float __closestDistance = float.MaxValue;

			foreach (Vector3 __vector in vectors)
			{
				float __distance = __vector.DistanceTo(comparer);

				if (!(__distance <= __closestDistance)) continue;

				__secondClosestVector = __closestVector;
				
				__closestDistance = __distance;
				__closestVector = __vector;
			}
			
			return(__closestVector, __secondClosestVector);
		}
		
		[PublicAPI]
		public static (Vector3 closestPosition, int closestIndex, Vector3 secondClosestPosition, int secondClosestIndex) 
			TwoClosestWithIndexes(this IEnumerable<Vector3> vectors, in Vector3 comparer)
		{
			int __indexOfClosest = 0;
			int __indexOfSecondClosest = 0;
			Vector3 __closestVector = Vector3.positiveInfinity;
			Vector3 __secondClosestVector = Vector3.positiveInfinity;
			float __closestDistance = float.MaxValue;

			int __indexCounter = 0;
			foreach (Vector3 __vector in vectors)
			{
				__indexCounter++;
				
				float __distance = __vector.DistanceTo(comparer);

				if (!(__distance <= __closestDistance)) continue;

				__indexOfSecondClosest = __indexOfClosest;
				__secondClosestVector = __closestVector;

				__indexOfClosest = (__indexCounter - 1);
				__closestDistance = __distance;
				__closestVector = __vector;
			}

			return (__closestVector, __indexOfClosest, __secondClosestVector, __indexOfSecondClosest);
		}
		
		/*
		/// <summary>
		/// Calculates the combined Distance of this List/Array of Vector3's.
		/// </summary>
		/// <param name="vectors"></param>
		/// <returns></returns>
		[PublicAPI]
		public static float CombinedDistanceInOrder(this IEnumerable<Vector3> vectors)
		{
			float __combinedDistance = 0f;

			if(vectors == null) goto RETURN;
			
			Vector3? __lastVector = null;
			foreach(Vector3 __vector in vectors) //Normal for loop wasn't possible because we use an IEnumerable instead of an array or list.
			{
				if(__lastVector == null)
				{
					__lastVector = __vector;
					continue;
				}

				__combinedDistance += __vector.DistanceTo((Vector3)__lastVector);

				__lastVector = __vector;
			}
			
			RETURN:
			return __combinedDistance;
		}
		*/
		
		/// <summary>
		/// Calculates the combined Distance of this List/Array of Vector3's.
		/// </summary>
		/// <param name="positions"></param>
		[PublicAPI]
		public static float CombinedDistance(this Vector3[] positions)
			=> CombinedDistanceInOrder(positions: positions);

		/// <summary>
		/// Calculates the combined Distance of this List/Array of Vector3's.
		/// </summary>
		/// <param name="positions"></param>
		[PublicAPI]
		public static float CombinedDistance(this IEnumerable<Vector3> positions)
			=> CombinedDistanceInOrder(positions: positions);
		
		/// <summary>
		/// Calculates the combined Distance of this List/Array of Vector3's.
		/// </summary>
		/// <param name="positions"></param>
		[PublicAPI]
		public static float CombinedDistanceInOrder(in IEnumerable<Vector3> positions)
		{
			float __combinedDistance = 0f;

			if(positions == null) goto RETURN;
			
			Vector3 __lastPosition = positions.First();
			foreach(Vector3 position in positions) //Normal for loop wasn't possible because we use an IEnumerable instead of an array or list.
			{
				__combinedDistance += position.DistanceTo(to: __lastPosition);

				__lastPosition = position;
			}

			RETURN:
			return __combinedDistance;
		}
		
		//public static Vector3 GetClosest(this List<Vector3> vectors, Vector3 comparer) => vectors.OrderBy(vector => Math.Abs(vector - comparer)).First();
		
		[PublicAPI]
		public static float DistanceTo(in this Vector3 from, in Vector3 to)
			=> Vector3.Distance(from, to);
		
		
		#endregion
		
		#region Rounding
		
		public static void Round(this Vector3 v) => v.Rounded();
		
		public static void Round(this Vector3 v, CGMath.RoundingMode roundingMode) => v.Rounded(roundingMode);

		public static void Floor(this Vector3 v) => v.Floored();

		public static void Ceil(this Vector3 v) => v.Ceiled();
		
		public static Vector3 Rounded(this Vector3 v) 
			=> new Vector3 {x = v.x.Round(), y = v.y.Round(), z = v.z.Round()};
		
		public static Vector3 Rounded(this Vector3 v, CGMath.RoundingMode roundingMode) 
			=> new Vector3 {x = v.x.Round(roundingMode), y = v.y.Round(roundingMode), z = v.z.Round(roundingMode)};
		
		public static Vector3Int RoundedToVector3Int(this Vector3 v)
			=> new Vector3Int {x = v.x.RoundToInt(), y = v.y.RoundToInt(), z = v.z.RoundToInt()};
		
		public static Vector3Int RoundedToVector3Int(this Vector3 v, CGMath.RoundingMode roundingMode)
			=> new Vector3Int {x = v.x.RoundToInt(), y = v.y.RoundToInt(), z = v.z.RoundToInt()};

		public static Vector3 Floored(this Vector3 v)
			=> new Vector3 {x = v.x.Floor(), y = v.y.Floor(), z = v.z.Floor()};
		
		public static Vector3Int FlooredToVector3Int(this Vector3 v)
			=> new Vector3Int {x = v.x.FloorToInt(), y = v.y.FloorToInt(), z = v.z.FloorToInt()};
		
		public static Vector3 Ceiled(this Vector3 v)
			=> new Vector3 {x = v.x.Ceil(), y = v.y.Ceil(), z = v.z.Ceil()};
		
		public static Vector3Int CeiledToVector3Int(this Vector3 v)
			=> new Vector3Int {x = v.x.CeilToInt(), y = v.y.CeilToInt(), z = v.z.CeilToInt()};
		
		#endregion

		#region Relativity
		
		[PublicAPI]
		public static Vector3 GetRelativePositionFrom(in this Vector3 position, in Matrix4x4 from)
			=> from.MultiplyPoint(position);

		[PublicAPI]
		public static Vector3 GetRelativePositionTo(in this Vector3 position, in Matrix4x4 to)
			=> to.inverse.MultiplyPoint(position);

		[PublicAPI]
		public static Vector3 GetRelativeDirectionFrom(in this Vector3 direction, in Matrix4x4 from)
			=> from.MultiplyVector(direction);

		[PublicAPI]
		public static Vector3 GetRelativeDirectionTo(in this Vector3 direction, in Matrix4x4 to)
			=> to.inverse.MultiplyVector(direction);
		
		#endregion
		
		#region Math Operations
		
		/// <summary> Create vector of direction "vector" with length "size" </summary>
		[PublicAPI]
		public static Vector3 SetVectorLength(this Vector3 vector, float size)
		{
			//Normalize
			Vector3 __vectorNormalized = Vector3.Normalize(vector);
 
			//Scale
			return __vectorNormalized *= size;
		}
		
		/// <summary>
		/// Mirrors a Vector in desired  Axis
		/// </summary>
		/// <returns></returns>
		[PublicAPI]
		public static Vector3 Mirror(this Vector3 vector, in Vector3 axis) //TODO: Edit to *actually* use an axis.
		{
			if (axis == Vector3.right) { vector.x *= -1f; }

			if (axis == Vector3.up) { vector.y *= -1f; }

			if (axis == Vector3.forward) { vector.z *= -1f; }

			return vector;
		}

		[PublicAPI]
		public static Vector3 ToAbs(ref this Vector3 vector)
		{
			vector.x.ToAbs();
			vector.y.ToAbs();
			vector.z.ToAbs();
			
			return vector;
		}
		
		/*
		public static Vector3 Abs(this Vector3 vector) 
			=> new Vector3(vector.x.ToAbs(), vector.y.ToAbs(), vector.z.ToAbs());
		*/
		
		[PublicAPI]
		public static Vector3 Abs(in this Vector3 vector) 
			=> new Vector3(vector.x.Abs(), vector.y.Abs(), vector.z.Abs());

		[PublicAPI]
		public static bool Approximately(in this Vector3 position, in Vector3 comparer)
			=> position.x.Approximately(comparer.x) 
			   && position.y.Approximately(comparer.y) 
			   && position.z.Approximately(comparer.z);

		/// <summary>
		/// Returns an angle in degrees [0, 360] between from and to
		/// </summary>
		/// <param name="from">The angle extends round from this vector</param>
		/// <param name="to">The angle extends round to this vector</param>
		/// <param name="normal">Up direction of the clockwise axis</param>
		public static float Angle360(this Vector3 from, Vector3 to, Vector3 normal)
		{
			float __angle = Vector3.SignedAngle(from, to, normal);
			
			while (__angle < 0)
			{
				__angle += 360;
			}
			return __angle;
		}
		
		/*
		public static Vector3 Random(this Vector3 target, Vector3 minRange, Vector3 maxRange)
		{
			minRange = -minRange.Abs();
			minRange = maxRange.Abs();
			
			return new Vector3(CGRandom.RandomRange(minRange.x, maxRange.x), CGRandom.RandomRange(minRange.y, maxRange.y),CGRandom.RandomRange(minRange.z, maxRange.z));
		}
		*/
		
		#endregion

		#region Transforming

		//public static Vector3 Shake

		[PublicAPI]
		public static Vector3 InverseLerp(in Vector3 a, in Vector3 b, in Vector3 value)
		{
			return new Vector3(
				Mathf.InverseLerp(a.x, b.x, value.x),
				Mathf.InverseLerp(a.y, b.y, value.y),
				Mathf.InverseLerp(a.z, b.z, value.z));
		}
		
		#endregion
		
		#endregion
		
		#region Vector2
		
		#region Rounding
		
		[PublicAPI]
		public static void Round(in this Vector2 v) => v.Rounded();
		
		[PublicAPI]
		public static void Round(in this Vector2 v, in CGMath.RoundingMode roundingMode) => v.Rounded(roundingMode);

		[PublicAPI]
		public static void Floor(in this Vector2 v) => v.Floored();

		[PublicAPI]
		public static void Ceil(in this Vector2 v) => v.Ceiled();
		
		[PublicAPI]
		public static Vector2 Rounded(in this Vector2 v) 
			=> new Vector2 {x = v.x.Round(), y = v.y.Round()};
		
		[PublicAPI]
		public static Vector2 Rounded(in this Vector2 v, CGMath.RoundingMode roundingMode) 
			=> new Vector3 {x = v.x.Round(roundingMode), y = v.y.Round(roundingMode)};
		
		[PublicAPI]
		public static Vector2Int RoundedToVector3Int(in this Vector2 v) 
			=> new Vector2Int {x = v.x.RoundToInt(), y = v.y.RoundToInt()};
		
		[PublicAPI]
		public static Vector2Int RoundedToVector3Int(in this Vector2 v, CGMath.RoundingMode roundingMode) 
			=> new Vector2Int {x = v.x.RoundToInt(), y = v.y.RoundToInt()};

		[PublicAPI]
		public static Vector2 Floored(in this Vector2 v) 
			=> new Vector2 {x = v.x.Floor(), y = v.y.Floor()};
		
		[PublicAPI]
		public static Vector2Int FlooredToVector2Int(in this Vector2 v) 
			=> new Vector2Int {x = v.x.FloorToInt(), y = v.y.FloorToInt()};
		
		[PublicAPI]
		public static Vector2 Ceiled(in this Vector2 v) 
			=> new Vector2 {x = v.x.Ceil(), y = v.y.Ceil()};
	
		[PublicAPI]
		public static Vector2Int CeiledToVector2Int(in this Vector2 v) 
			=> new Vector2Int {x = v.x.CeilToInt(), y = v.y.CeilToInt()};
		
		#endregion

		#region Relativity
		
		[PublicAPI]
		public static Vector2 GetRelativePositionFrom(this Vector2 position, Matrix4x4 from) 
			=> from.MultiplyPoint(position);

		[PublicAPI]
		public static Vector2 GetRelativePositionTo(this Vector2 position, Matrix4x4 to) 
			=> to.inverse.MultiplyPoint(position);

		[PublicAPI]
		public static Vector2 GetRelativeDirectionFrom(this Vector2 direction, Matrix4x4 from) 
			=> from.MultiplyVector(direction);

		[PublicAPI]
		public static Vector2 GetRelativeDirectionTo(this Vector2 direction, Matrix4x4 to) 
			=> to.inverse.MultiplyVector(direction);
		
		#endregion
		
		#region Math Operations
		
		[PublicAPI]
		public static Vector2 Perpendicular(in this Vector2 vector)
		{
			return new Vector2(-vector.y, vector.x);
		}
		
		[PublicAPI]
		public static Vector2 ToPerpendicular(ref this Vector2 vector)
		{ //Test if have to return void or not?
			vector.x = -vector.y;
			vector.y = vector.x;

			return vector;
		}
		
		/// <summary>
		/// Returns a perp dot product of vectors
		/// </summary>
		/// <remarks>
		/// Hill, F. S. Jr. "The Pleasures of 'Perp Dot' Products."
		/// Ch. II.5 in Graphics Gems IV (Ed. P. S. Heckbert). San Diego: Academic Press, pp. 138-148, 1994
		/// </remarks>
		public static float PerpDot(Vector2 a, Vector2 b)
		{
			return a.x*b.y - a.y*b.x;
		}
		
		/// <summary>
		/// Returns a clockwise angle in degrees [0, 360] between from and to
		/// </summary>
		/// <param name="from">The angle extends round from this vector</param>
		/// <param name="to">The angle extends round to this vector</param>
		public static float Angle360(Vector2 from, Vector2 to)
		{
			float __angle = SignedAngle(from, to);
			while (__angle < 0)
			{
				__angle += 360;
			}
			return __angle;
		}
		
		/// <summary>
		/// Returns a signed clockwise angle in degrees [-180, 180] between from and to
		/// </summary>
		/// <param name="from">The angle extends round from this vector</param>
		/// <param name="to">The angle extends round to this vector</param>
		public static float SignedAngle(this Vector2 from, Vector2 to)
		{
			return Mathf.Atan2(
					   PerpDot(to, from), 
					   Vector2.Dot(to, from)) * Mathf.Rad2Deg;
		}
		
		#endregion
		
		#region Transforming

		//public static Vector3 Shake

		[PublicAPI]
		public static Vector2 InverseLerp(in Vector2 a, in Vector2 b, in Vector2 value)
		{
			return new Vector2(
				Mathf.InverseLerp(a.x, b.x, value.x),
				Mathf.InverseLerp(a.y, b.y, value.y));
		}
		
		/// <summary> Returns a new vector rotated clockwise by the specified angle. </summary>
		[PublicAPI]
		public static Vector2 RotateCW(this Vector2 vector, float degrees)
		{
			float radians = degrees*Mathf.Deg2Rad;
			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);
			return new Vector2(
				vector.x*cos + vector.y*sin,
				vector.y*cos - vector.x*sin);
		}

		/// <summary> Returns a new vector rotated counterclockwise by the specified angle. </summary>
		[PublicAPI]
		public static Vector2 RotateCCW(this Vector2 vector, float degrees)
		{
			float radians = degrees*Mathf.Deg2Rad;
			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);
			return new Vector2(
				vector.x*cos - vector.y*sin,
				vector.y*cos + vector.x*sin);
		}
		
		#endregion
		
		#endregion

		#region Vector2Int

		[PublicAPI]
		public static Vector2Int Perpendicular(in this Vector2Int vector)
		{
			return new Vector2Int(-vector.y, vector.x);
		}
		
		[PublicAPI]
		public static Vector2Int ToPerpendicular(ref this Vector2Int vector)
		{
			vector.x = -vector.y;
			vector.y = vector.x;

			return vector;
		}

		#endregion

		#region Vector4

		[PublicAPI]
		public static Vector4 InverseLerp(in Vector4 a, in Vector4 b, in Vector4 value)
		{
			return new Vector4(
				Mathf.InverseLerp(a.x, b.x, value.x),
				Mathf.InverseLerp(a.y, b.y, value.y),
				Mathf.InverseLerp(a.z, b.z, value.z),
				Mathf.InverseLerp(a.w, b.w, value.w));
		}

		#endregion
	}
}