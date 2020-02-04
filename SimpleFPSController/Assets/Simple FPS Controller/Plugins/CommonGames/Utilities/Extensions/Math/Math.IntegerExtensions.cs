using UnityEngine;

namespace CommonGames.Utilities.Extensions
{
	public static partial class Math
	{
		public static int Abs(this int value)
		{
			return Mathf.Abs(value);
		}
		
		public static int ToAbs(this int value)
		{
			value = Mathf.Abs(value);
			return value;
		}

		public static int ClosestPowerOfTwo(this int value)
		{
			return Mathf.ClosestPowerOfTwo(value);
		}
	}
}
