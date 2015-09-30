using System;
using UnityEngine;

public class EffectSoundPlayer : MonoBehaviour
{
	public AudioClipArray sounds;

	public EffectSoundPlayer()
	{
	}

	private void Start()
	{
		AudioClip item = this.sounds[UnityEngine.Random.Range(0, this.sounds.Length)];
		item.Play(base.transform.position, 1f, 1f, 10f);
	}
}