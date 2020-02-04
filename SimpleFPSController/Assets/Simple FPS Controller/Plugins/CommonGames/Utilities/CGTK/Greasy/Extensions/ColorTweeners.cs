using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
    public static class ColorTweens
    {
        public static Coroutine To(this Color from, Color to, float duration, EaseType ease)
            => Greasy.To(from, to, duration, ease, setter:(x => from = x));
		
        public static Coroutine To(this Color from, Color to, float duration, EaseMethod ease)
            => Greasy.To(from, to, duration, ease, setter:(x => from = x));
    }
}