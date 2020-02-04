using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
	public static partial class Greasy
	{
		public static Coroutine To(Vector2 from, Vector2 to, in float duration, in EaseType ease, Setter<Vector2> setter)
			=> CreateInterpolater (duration, ease, t => setter (Vector2.LerpUnclamped (from, to, t)));
		
		public static Coroutine To(Vector2 from, Vector2 to, in float duration, in EaseMethod ease, Setter<Vector2> setter)
			=> CreateInterpolater (duration, ease, t => setter (Vector2.LerpUnclamped (from, to, t)));
		
	}
}