using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
	public static partial class Greasy
	{
		public static Coroutine To(int from, int to, float duration, EaseType ease, Setter<int> setter)
			=> CreateInterpolater (duration, ease, t => setter ((int)(t * Mathf.Abs (to - from) + from)));
		
		public static Coroutine To(int from, int to, float duration, EaseMethod ease, Setter<int> setter)
			=> CreateInterpolater (duration, ease, t => setter ((int)(t * Mathf.Abs (to - from) + from)));
	}
}
