using System;
using System.Collections;
using UnityEngine;
using CommonGames.Utilities.Extensions;

using JetBrains.Annotations;

namespace CommonGames.Utilities.CGTK.Greasy
{	
	public static partial class Greasy
	{
		public delegate void Setter<in T>(T value);

		private sealed class Greaser : EnsuredSingleton<Greaser>{}
		private static Greaser Tweener => Greaser.Instance;

		[PublicAPI]
		public static Coroutine CreateInterpolater(float duration, EaseType easeType, Action<float> onProgressChanged)
			=> CreateInterpolater(duration, Ease.GetEaseMethod(easeType), onProgressChanged);

		[PublicAPI]
		public static Coroutine CreateInterpolater(float duration, EaseMethod easeMethod, Action<float> onProgressChanged)
			=> Tweener.StartCoroutine(Lerper(duration, easeMethod, onProgressChanged));

		private static IEnumerator To(float duration, Action<float> onProgressChanged)
		{
			if(duration.Approximately(0f))
			{
				onProgressChanged(1f);
				yield break;
			}

			float __time = 0f;
			while(__time < duration)
			{
				onProgressChanged(__time / duration);
				__time += Time.deltaTime;
				yield return null;
			}

			onProgressChanged(1f);
		}

		private static IEnumerator Lerper(float duration, EaseMethod ease, Action<float> onProgressChanged)
			=> To(duration, time => onProgressChanged(ease(time)));
	}

	public static class Ease
	{
		public static EaseMethod GetEaseMethod(EaseType ease)
		{
			// I failed humanity
			//TODO: Un-Fail Humanity plz.
			
			switch(ease)
			{
				case EaseType.Custom:
					return null;
				case EaseType.Linear:
					return Linear;
				case EaseType.QuadIn:
					return QuadIn;
				case EaseType.QuadOut:
					return QuadOut;
				case EaseType.QuadInOut:
					return QuadInOut;
				case EaseType.CubicIn:
					return CubicIn;
				case EaseType.CubicOut:
					return CubicOut;
				case EaseType.CubicInOut:
					return CubicInOut;
				case EaseType.QuartIn:
					return QuartIn;
				case EaseType.QuartOut:
					return QuartOut;
				case EaseType.QuartInOut:
					return QuartInOut;
				case EaseType.QuintIn:
					return QuintIn;
				case EaseType.QuintOut:
					return QuintOut;
				case EaseType.QuintInOut:
					return QuintInOut;
				case EaseType.BounceIn:
					return BounceIn;
				case EaseType.BounceOut:
					return BounceOut;
				case EaseType.BounceInOut:
					return BounceInOut;
				case EaseType.ElasticIn:
					return ElasticIn;
				case EaseType.ElasticOut:
					return ElasticOut;
				case EaseType.ElasticInOut:
					return ElasticInOut;
				case EaseType.CircularIn:
					return CircularIn;
				case EaseType.CircularOut:
					return CircularOut;
				case EaseType.CircularInOut:
					return CircularInOut;
				case EaseType.SinusIn:
					return SinusIn;
				case EaseType.SinusOut:
					return SinusOut;
				case EaseType.SinusInOut:
					return SinusInOut;
				case EaseType.ExponentialIn:
					return ExponentialIn;
				case EaseType.ExponentialOut:
					return ExponentialOut;
				case EaseType.ExponentialInOut:
					return ExponentialInOut;
				default:
					return Linear;
			}
		}

		#region Default Easing Methods
		
		private static float Linear(float t) => t;
		private static float QuadIn(float t) => t * t;
		private static float QuadOut(float t) => t *(2f - t);
		private static float QuadInOut(float t) => t < 0.5f ? 2f * t * t : -1f +(4f - 2f * t) * t;
		private static float CubicIn(float t) => t * t * t;
		private static float CubicOut(float t) =>(t - 1f) * t * t + 1f;
		private static float CubicInOut(float t) => t < 0.5f ? 4f * t * t * t :(t - 1f) *(2f * t - 2f) *(2 * t - 2) + 1f;
		private static float QuartIn(float t) => t * t * t * t;
		private static float QuartOut(float t) => 1f -(t - 1f) * t * t * t;
		private static float QuartInOut(float t) => t < 0.5f ? 8f * t * t * t * t : 1f - 8f *(t - 1f) * t * t * t;
		private static float QuintIn(float t) => t * t * t * t * t;
		private static float QuintOut(float t) => 1f +(t - 1f) * t * t * t * t;
		private static float QuintInOut(float t) => t < 0.5f ? 16f * t * t * t * t * t : 1f + 16f *(t - 1f) * t * t * t * t;
		private static float BounceIn(float t) => 1f - BounceOut(1f - t);
		private static float BounceOut(float t) => t < 0.363636374f ? 7.5625f * t * t : t < 0.727272749f ? 7.5625f *(t -= 0.545454562f) * t + 0.75f : t < 0.909090936f ? 7.5625f *(t -= 0.8181818f) * t + 0.9375f : 7.5625f *(t -= 21f / 22f) * t + 63f / 64f;
		private static float BounceInOut(float t) => t < 0.5f ? BounceIn(t * 2f) * 0.5f : BounceOut(t * 2f - 1f) * 0.5f + 0.5f;
		private static float ElasticIn(float t) => -(Mathf.Pow(2, 10 *(t -= 1)) * Mathf.Sin((t -(0.3f / 4f)) *(2 * Mathf.PI) / 0.3f));
		private static float ElasticOut(float t) => t.Approximately(1f) ? 1f : 1f - ElasticIn(1f - t);
		private static float ElasticInOut(float t) =>(t *= 2f).Approximately(2f) ? 1f : t < 1f ? -0.5f *(Mathf.Pow(2f, 10f *(t -= 1)) * Mathf.Sin((t - 0.1125f) *(2f * Mathf.PI) / 0.45f)) :(Mathf.Pow(2f, -10f *(t -= 1f)) * Mathf.Sin((t - 0.1125f) *(2f * Mathf.PI) / 0.45f) * 0.5f + 1f);
		private static float CircularIn(float t) => -(Mathf.Sqrt(1 - t * t) - 1);
		private static float CircularOut(float t) => Mathf.Sqrt(1f -(t = t - 1f) * t);
		private static float CircularInOut(float t) =>(t *= 2f) < 1f ? -1f / 2f *(Mathf.Sqrt(1f - t * t) - 1f) : 0.5f *(Mathf.Sqrt(1 -(t -= 2) * t) + 1);
		private static float SinusIn(float t) => -Mathf.Cos(t *(Mathf.PI * 0.5f)) + 1f;
		private static float SinusOut(float t) => Mathf.Sin(t *(Mathf.PI * 0.5f));
		private static float SinusInOut(float t) => -0.5f *(Mathf.Cos(Mathf.PI * t) - 1f);
		private static float ExponentialIn(float t) => Mathf.Pow(2f, 10f *(t - 1f));
		private static float ExponentialOut(float t) => Mathf.Sin(t *(Mathf.PI * 0.5f));
		private static float ExponentialInOut(float t) => -0.5f *(Mathf.Cos(Mathf.PI * t) - 1f);
		
		#endregion
	}
	
}