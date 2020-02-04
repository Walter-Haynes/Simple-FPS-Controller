using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
    public static partial class Greasy
    {
        public static Coroutine To(float from, float to, in float duration, in EaseType ease, Setter<float> setter)
            => CreateInterpolater(duration, ease, t => setter (Mathf.LerpUnclamped(from, to, t)));
        
        public static Coroutine To(float from, float to, in float duration, in EaseMethod ease, Setter<float> setter)
            => CreateInterpolater(duration, ease, t => setter (Mathf.LerpUnclamped(from, to, t)));

    }
}
