using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
    public static class FloatTweens
    {
        public static Coroutine To(this float from, float to, float duration, EaseType ease)
            => Greasy.To(from, to, duration, ease, setter:(x => from = x));
		
        public static Coroutine To(this float from, float to, float duration, EaseMethod ease)
            => Greasy.To(from, to, duration, ease, setter:(x => from = x));
    }
}