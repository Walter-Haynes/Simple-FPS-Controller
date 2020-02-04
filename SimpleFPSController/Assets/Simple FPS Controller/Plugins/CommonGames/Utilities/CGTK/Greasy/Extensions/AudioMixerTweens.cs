using UnityEngine;
using UnityEngine.Audio;

namespace CommonGames.Utilities.CGTK.Greasy
{
	public static class AudioMixerTweens
	{
		public static Coroutine FloatTo (this AudioMixer audioMixer, string name, float to, float duration, EaseType ease)
		{
			return FloatTo (audioMixer, name, to, duration, Ease.GetEaseMethod (ease));
		}
		public static Coroutine FloatTo (this AudioMixer audioMixer, string name, float to, float duration, EaseMethod ease)
		{
			if (!audioMixer.GetFloat(name, out float from))
			{
				throw new System.Exception(	$"Audio Mixer doesn't have a float called '{name} or is currently being edited.'");
			}

			return Greasy.To (from, to, duration, ease, x => audioMixer.SetFloat (name, x));
		}
	}
}