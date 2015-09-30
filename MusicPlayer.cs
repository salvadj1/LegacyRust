using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	public float timeBetweenTracks = 600f;

	public float targetVolume = 0.2f;

	public float startDelay;

	public AudioClip[] tracks;

	private float nextMusicTime = 5f;

	private bool wasMuted = true;

	private static int savedFrameCount;

	static MusicPlayer()
	{
		MusicPlayer.savedFrameCount = -1;
	}

	public MusicPlayer()
	{
	}

	private void Start()
	{
		this.wasMuted = MusicPlayer.settings.mute;
		this.nextMusicTime = Time.time + 3f + this.startDelay;
		base.audio.volume = 0f;
		if (this.tracks == null || (int)this.tracks.Length == 0)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		int num = Time.frameCount;
		if (num != MusicPlayer.savedFrameCount)
		{
			MusicPlayer.savedFrameCount = num;
			if (Input.GetKeyDown(KeyCode.PageUp))
			{
				MusicPlayer.settings.mute = !MusicPlayer.settings.mute;
			}
		}
		if (this.wasMuted != MusicPlayer.settings.mute)
		{
			if (!this.wasMuted || Time.time <= this.nextMusicTime)
			{
				base.audio.Stop();
				this.nextMusicTime = Time.time;
			}
			else
			{
				this.nextMusicTime = Time.time;
			}
			this.wasMuted = !this.wasMuted;
		}
		if (!this.wasMuted)
		{
			if (Time.time > this.nextMusicTime)
			{
				if ((int)this.tracks.Length == 0)
				{
					return;
				}
				AudioClip audioClip = this.tracks[UnityEngine.Random.Range(0, (int)this.tracks.Length)];
				base.audio.Stop();
				base.audio.clip = audioClip;
				this.nextMusicTime = Time.time + audioClip.length + this.timeBetweenTracks * UnityEngine.Random.RandomRange(0.75f, 1.25f);
				base.audio.Play();
			}
			float single = this.targetVolume * sound.music;
			if (base.audio.volume < single)
			{
				AudioSource audioSource = base.audio;
				audioSource.volume = audioSource.volume + Time.deltaTime / 3f * single;
			}
			if (base.audio.volume > single)
			{
				base.audio.volume = single;
			}
		}
	}

	private static class settings
	{
		private static bool _mute;

		public static bool mute
		{
			get
			{
				return MusicPlayer.settings._mute;
			}
			set
			{
				if (value != MusicPlayer.settings._mute)
				{
					if (!value)
					{
						PlayerPrefs.DeleteKey("MUSIC_MUTE");
					}
					else
					{
						PlayerPrefs.SetInt("MUSIC_MUTE", 1);
					}
					MusicPlayer.settings._mute = value;
				}
			}
		}

		static settings()
		{
			MusicPlayer.settings._mute = PlayerPrefs.GetInt("MUSIC_MUTE", 0) != 0;
		}
	}
}