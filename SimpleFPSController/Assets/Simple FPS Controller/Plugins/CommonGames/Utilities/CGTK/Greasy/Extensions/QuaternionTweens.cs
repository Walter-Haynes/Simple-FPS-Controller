using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
	public static class QuaternionTweens
	{
		public static Coroutine To(this Quaternion from, Quaternion to, float duration, EaseType ease)
			=> Greasy.To(from, to, duration, ease, setter:(x => from = x));
		
		public static Coroutine To(this Quaternion from, Quaternion to, float duration, EaseMethod ease)
			=> Greasy.To(from, to, duration, ease, setter:(x => from = x));	
	}
}