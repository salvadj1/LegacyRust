using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class SoundPoolFilter : MonoBehaviour
{
	private static SoundPoolFilter instance;

	private bool awake;

	private bool quitting;

	public SoundPoolFilter()
	{
	}

	private void Awake()
	{
		if (!SoundPoolFilter.instance || !(SoundPoolFilter.instance != this))
		{
			SoundPoolFilter.instance = this;
			this.awake = true;
			SoundPool.enabled = base.enabled;
		}
		else
		{
			Debug.LogError("ONLY HAVE ONE PLEASE", this);
		}
	}

	private void OnApplicationQuit()
	{
		SoundPool.quitting = true;
		this.quitting = true;
	}

	private void OnDestroy()
	{
		if (SoundPoolFilter.instance == this)
		{
			this.awake = false;
			SoundPoolFilter.instance = null;
			SoundPool.enabled = false;
			if (this.quitting)
			{
				SoundPool.Drain();
			}
		}
	}

	private void OnDisable()
	{
		if (this.awake)
		{
			SoundPool.enabled = false;
		}
	}

	private void OnEnable()
	{
		if (this.awake)
		{
			SoundPool.enabled = true;
		}
	}

	private void OnPreCull()
	{
		SoundPool.Pump();
	}
}