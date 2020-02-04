using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGames.Utilities.Extensions;
using static System.Math;

namespace CommonGames.Utilities.CGTK
{
	public static partial class CGMath
	{
		#region Int

		public static int Lerp(int from, int to, float duration)
			=> Mathf.Lerp(from, to, duration).RoundToInt();
		
		public static int LerpUnclamped(int from, int to, float duration)
			=> Mathf.LerpUnclamped(from, to, duration).RoundToInt();
		
		public static int InverseLerp(int from, int to, float duration)
			=> Mathf.InverseLerp(from, to, duration).RoundToInt();

		#endregion
		
		#region Float
		
		public static float Lerp(float from, float to, float duration = 0f)
			=> Mathf.Lerp(from, to, duration);

		public static float LerpUnclamped(float from, float to, float duration = 0f)
			=> Mathf.LerpUnclamped(from, to, duration);
		
		public static int InverseLerp(float from, float to, float duration)
			=> Mathf.InverseLerp(from, to, duration).RoundToInt();
		
		#endregion

		#region Vector3

		public static Vector3 Lerp(Vector3 from, Vector3 to, float duration = 0f)
			=> Vector3.Lerp(from, to, duration);
		
		public static Vector3 LerpUnclamped(Vector3 from, Vector3 to, float duration = 0f)
			=> Vector3.LerpUnclamped(from, to, duration);
		
		public static Vector3 InverseLerp(Vector3 from, Vector3 to, float duration = 0f)
		{
			return new Vector3
			(
				x: InverseLerp(from.x, to.x, duration),
				y: InverseLerp(from.y, to.y, duration),
				z: InverseLerp(from.z, to.z, duration)
			);
		}
		
		public static Vector3 Slerp(Vector3 from, Vector3 to, float duration = 0f)
			=> Vector3.Slerp(from, to, duration);

		public static Vector3 SlerpUnclamped(Vector3 from, Vector3 to, float duration = 0f)
			=> Vector3.SlerpUnclamped(from, to, duration);
		
		#endregion

		#region Quaternion

		public static Quaternion Slerp(Quaternion p, Quaternion q, float t)
		{
			 Quaternion ret;
 
			 float fCos = Quaternion.Dot(p, q);
 
			 if ((1.0f + fCos) > Mathf.Epsilon)
			 {
					float fCoeff0, fCoeff1;
 
					if ((1.0f - fCos) > Mathf.Epsilon)
					{
						 float omega = Mathf.Acos(fCos);
						 float invSin = 1.0f / Mathf.Sin(omega);
						 fCoeff0 = Mathf.Sin((1.0f - t) * omega) * invSin;
						 fCoeff1 = Mathf.Sin(t * omega) * invSin;
					}
					else
					{
						 fCoeff0 = 1.0f - t;
						 fCoeff1 = t;
					}
 
					ret.x = fCoeff0 * p.x + fCoeff1 * q.x;
					ret.y = fCoeff0 * p.y + fCoeff1 * q.y;
					ret.z = fCoeff0 * p.z + fCoeff1 * q.z;
					ret.w = fCoeff0 * p.w + fCoeff1 * q.w;
			  }
			  else
			  {
					float fCoeff0 = Mathf.Sin((1.0f - t) * Mathf.PI * 0.5f);
					float fCoeff1 = Mathf.Sin(t * Mathf.PI * 0.5f);
 
					ret.x = fCoeff0 * p.x - fCoeff1 * p.y;
					ret.y = fCoeff0 * p.y + fCoeff1 * p.x;
					ret.z = fCoeff0 * p.z - fCoeff1 * p.w;
					ret.w = p.z;
			  }
 
			  return ret;
		 }

		#endregion	
	}
}