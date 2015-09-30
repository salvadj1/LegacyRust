using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAtDay : MonoBehaviour
{
	public TOD_Sky sky;

	public float fadeTime = 1f;

	private float lerpTime;

	private AudioSource audioComponent;

	private float audioVolume;

	public AudioAtDay()
	{
	}

	protected void OnEnable()
	{
		if (!this.sky)
		{
			Debug.LogError("Sky instance reference not set. Disabling script.");
			base.enabled = false;
		}
		this.audioComponent = base.audio;
		this.audioVolume = this.audioComponent.volume;
	}

	protected void Update()
	{
		int num = (!this.sky.IsDay ? -1 : 1);
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.audioComponent.volume = Mathf.Lerp(0f, this.audioVolume, this.lerpTime);
	}
}