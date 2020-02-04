using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
	public static class CameraTweens
	{
		public static Coroutine BackgroundColorTo(this Camera camera, Color to, float duration, EaseType ease = EaseType.Linear)
		{
			return Greasy.To(camera.backgroundColor, to, duration, ease, x => camera.backgroundColor = x);
		}
		public static Coroutine BackgroundColorTo(this Camera camera, Color to, float duration, EaseMethod ease)
		{
			return Greasy.To(camera.backgroundColor, to, duration, ease, x => camera.backgroundColor = x);
		}

		public static Coroutine FieldOfViewTo(this Camera camera, float to, float duration, EaseType ease = EaseType.Linear)
		{
			return Greasy.To(camera.fieldOfView, to, duration, ease, x => camera.fieldOfView = x);
		}
		public static Coroutine FieldOfViewTo(this Camera camera, float to, float duration, EaseMethod ease)
		{
			return Greasy.To(camera.fieldOfView, to, duration, ease, x => camera.fieldOfView = x);
		}
	}
}