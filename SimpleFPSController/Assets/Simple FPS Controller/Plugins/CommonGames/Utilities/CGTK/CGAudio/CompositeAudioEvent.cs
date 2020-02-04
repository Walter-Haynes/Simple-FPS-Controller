using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio Events/Composite")]
public class CompositeAudioEvent : AudioEvent
{
	[Serializable]
	public struct CompositeEntry
	{
		public AudioEvent audioEvent;
		public float weight;
	}

	public CompositeEntry[] entries;

	public override void Play(AudioSource source)
	{
		float __totalWeight = 0;
		for (int __i = 0; __i < entries.Length; ++__i)
		{
			__totalWeight += entries[__i].weight;
		}

		float __pick = Random.Range(0, __totalWeight);
		for (int __i = 0; __i < entries.Length; ++__i)
		{
			if (__pick > entries[__i].weight)
			{
				__pick -= entries[__i].weight;
				continue;
			}

			entries[__i].audioEvent.Play(source);
			return;
		}
	}
}