using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAtNight : MonoBehaviour
{
	public TOD_Sky sky;

	public float fadeTime = 1f;

	private float lerpTime;

	private AudioSource audioComponent;

	private float audioVolume;

	public AudioAtNight()
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
		int num = (!this.sky.IsNight ? -1 : 1);
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.audioComponent.volume = Mathf.Lerp(0f, this.audioVolume, this.lerpTime);
	}
}