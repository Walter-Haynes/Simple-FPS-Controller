using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
    public static partial class Greasy
    {
        public static Coroutine To(Quaternion from, Quaternion to, float duration, EaseType ease, Setter<Quaternion> setter)
            => CreateInterpolater(duration, ease, t => setter(Quaternion.SlerpUnclamped(from, to, t)));

        public static Coroutine To(Quaternion from, Quaternion to, float duration, EaseMethod ease, Setter<Quaternion> setter)
            => CreateInterpolater(duration, ease, t => setter(Quaternion.SlerpUnclamped(from, to, t)));
		
    }
}