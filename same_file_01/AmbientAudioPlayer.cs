using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class AmbientAudioPlayer : MonoBehaviour
{
	public AudioClip daySound;

	public AudioClip nightSound;

	public AudioSource daySource;

	public AudioSource nightSource;

	public AmbientAudioPlayer()
	{
	}

	private void Awake()
	{
		base.InvokeRepeating("CheckTimeChange", 0f, 5f);
		this.daySource.clip = this.daySound;
		this.nightSource.clip = this.nightSound;
		this.daySource.volume = 0f;
		this.nightSource.volume = 0f;
		this.daySource.Stop();
		this.nightSource.Stop();
		this.daySource.enabled = false;
		this.nightSource.enabled = false;
	}

	private void CheckTimeChange()
	{
		if (this.NeedsAudioUpdate())
		{
			base.Invoke("UpdateVolume", 0f);
		}
	}

	public bool NeedsAudioUpdate()
	{
		if (!EnvironmentControlCenter.Singleton)
		{
			return true;
		}
		if (EnvironmentControlCenter.Singleton && !EnvironmentControlCenter.Singleton.IsNight() && (this.daySource.volume < 1f || this.nightSource.volume > 0f))
		{
			return true;
		}
		if (EnvironmentControlCenter.Singleton && EnvironmentControlCenter.Singleton.IsNight() && (this.nightSource.volume < 1f || this.daySource.volume > 0f))
		{
			return true;
		}
		return false;
	}

	private void UpdateVolume()
	{
		if (!this.NeedsAudioUpdate())
		{
			return;
		}
		base.Invoke("UpdateVolume", Time.deltaTime);
		bool flag = (!EnvironmentControlCenter.Singleton ? true : !EnvironmentControlCenter.Singleton.IsNight());
		AudioSource audioSource = (!flag ? this.nightSource : this.daySource);
		AudioSource audioSource1 = (!flag ? this.daySource : this.nightSource);
		if (!audioSource.isPlaying)
		{
			audioSource.enabled = true;
			audioSource.Play();
		}
		AudioSource audioSource2 = audioSource;
		audioSource2.volume = audioSource2.volume + 0.2f * Time.deltaTime;
		AudioSource audioSource3 = audioSource1;
		audioSource3.volume = audioSource3.volume - 0.2f * Time.deltaTime;
		if (audioSource.volume > 1f)
		{
			audioSource.volume = 1f;
		}
		if (audioSource1.volume < 0f)
		{
			audioSource1.volume = 0f;
			audioSource1.Stop();
			audioSource1.enabled = false;
		}
	}
}