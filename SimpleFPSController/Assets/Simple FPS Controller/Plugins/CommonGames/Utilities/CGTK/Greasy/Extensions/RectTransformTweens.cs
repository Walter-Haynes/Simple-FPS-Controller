using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
	public static class RectTransformTweens
	{
		
		#region PositionTo
		
		public static Coroutine PositionTo(this RectTransform transform, Vector3 to, float duration, EaseType ease)
			=> Greasy.To(transform.position, to, duration, ease, setter:(x => transform.position = x));
		public static Coroutine PositionTo(this RectTransform transform, Vector3 to, float duration, EaseMethod ease)
			=> Greasy.To(transform.position, to, duration, ease, setter:(x => transform.position = x));
		
		#endregion

		#region LocalPositionTo
		
		public static Coroutine LocalPositionTo(this RectTransform transform, Vector3 to, float duration, EaseType ease)
			=> Greasy.To(transform.localPosition, to, duration, ease, setter:(x => transform.localPosition = x));
		
		public static Coroutine LocalPositionTo(this RectTransform transform, Vector3 to, float duration, EaseMethod ease)
			=> Greasy.To(transform.localPosition, to, duration, ease, setter:(x => transform.localPosition = x));
		
		#endregion

		#region RotationTo
		
		public static Coroutine RotationTo(this RectTransform transform, Quaternion to, float duration, EaseType ease)
		{
			return Greasy.To(transform.rotation, to, duration, ease, setter:(x => transform.rotation = x));
		}
		public static Coroutine RotationTo(this RectTransform transform, Quaternion to, float duration, EaseMethod ease)
		{
			return Greasy.To(transform.rotation, to, duration, ease, setter:(x => transform.rotation = x));
		}
		
		#endregion

		#region LocalRotationTo
		
		public static Coroutine LocalRotationTo(this RectTransform transform, Quaternion to, float duration, EaseType ease)
		{
			return Greasy.To(transform.localRotation, to, duration, ease, x => transform.localRotation = x);
		}
		public static Coroutine LocalRotationTo(this RectTransform transform, Quaternion to, float duration, EaseMethod ease)
		{
			return Greasy.To(transform.localRotation, to, duration, ease, x => transform.localRotation = x);
		}
		
		#endregion

		#region LookTo
		
		public static Coroutine LookTo(this RectTransform transform, Vector3 to, Vector3 up, float duration, EaseType ease)
		{
			return Greasy.To(transform.rotation, Quaternion.LookRotation(to - transform.position, up), duration, ease, x => transform.localRotation = x);
		}
		public static Coroutine LookTo(this RectTransform transform, Vector3 to, Vector3 up, float duration, EaseMethod ease)
		{
			return Greasy.To(transform.rotation, Quaternion.LookRotation(to - transform.position, up), duration, ease, x => transform.localRotation = x);
		}
		
		#endregion

		#region ScaleTo
		
		public static Coroutine ScaleTo(this RectTransform transform, Vector3 to, float duration, EaseType ease)
		{
			return Greasy.To(transform.localScale, to, duration, ease, x => transform.localScale = x);
		}
		public static Coroutine ScaleTo(this RectTransform transform, Vector3 to, float duration, EaseMethod ease)
		{
			return Greasy.To(transform.localScale, to, duration, ease, x => transform.localScale = x);
		}
		
		#endregion
		
	}
}