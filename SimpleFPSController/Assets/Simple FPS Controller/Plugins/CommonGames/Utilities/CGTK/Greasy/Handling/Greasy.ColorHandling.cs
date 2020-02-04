using UnityEngine;

using JetBrains.Annotations;

namespace CommonGames.Utilities.CGTK.Greasy
{
    [PublicAPI]
    public static partial class Greasy
    {
        public static Coroutine To(Color from, Color to, in float duration, in EaseType ease, Setter<Color> setter)
            => CreateInterpolater(duration, ease, t => setter (Color.LerpUnclamped(from, to, t)));
        
        public static Coroutine To(Color from, Color to, in float duration, in EaseMethod ease, Setter<Color> setter)
            => CreateInterpolater(duration, ease, t => setter (Color.LerpUnclamped(from, to, t)));

    }
}