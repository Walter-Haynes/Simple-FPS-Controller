using UnityEngine;
using System.Collections;

using Random = UnityEngine.Random;

using CommonGames.Utilities.CustomTypes;

[CreateAssetMenu(menuName="Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
	public AudioClip[] clips;

	public RangedFloat volume = new RangedFloat(minValue: 0.9f, maxValue: 1.0f);

	[MinMaxRange(0, 2)]
	public RangedFloat pitch = new RangedFloat(minValue: 0.9f, maxValue: 1.1f);

	public override void Play(AudioSource source)
	{
		if (clips.Length == 0) return;

		source.clip = clips[Random.Range(0, clips.Length)];
		source.volume = Random.Range(volume.MinValue, volume.MaxValue);
		source.pitch = Random.Range(pitch.MinValue, pitch.MaxValue);
		source.Play();
	}
}