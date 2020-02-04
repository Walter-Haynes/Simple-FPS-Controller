using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
    public static class IntTweens
    {
        public static Coroutine To(this int from, int to, float duration, EaseType ease)
            => Greasy.To(from, to, duration, ease, setter:(x => from = x));
		
        public static Coroutine To(this int from, int to, float duration, EaseMethod ease)
            => Greasy.To(from, to, duration, ease, setter:(x => from = x));
    }
}