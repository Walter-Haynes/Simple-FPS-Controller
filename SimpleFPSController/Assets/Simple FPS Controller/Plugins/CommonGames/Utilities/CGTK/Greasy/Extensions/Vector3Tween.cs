using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
    public static class Vector3Tweens
    {
        public static Coroutine To(this Vector3 from, Vector3 to, float duration, EaseType ease)
            => Greasy.To(from, to, duration, ease, setter:(x => from = x));
		
        public static Coroutine To(this Vector3 from, Vector3 to, float duration, EaseMethod ease)
            => Greasy.To(from, to, duration, ease, setter:(x => from = x));
    }
}