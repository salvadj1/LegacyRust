using Facepunch;
using System;
using UnityEngine;

public sealed class LocalRadiationEffect : IDLocalCharacterAddon
{
	[NonSerialized]
	private Radiation radiation;

	private static AudioClip geiger0;

	private static AudioClip geiger1;

	private static AudioClip geiger2;

	private static AudioClip geiger3;

	private static GameObject geigerSoundPlayer;

	static LocalRadiationEffect()
	{
	}

	public LocalRadiationEffect() : base(IDLocalCharacterAddon.AddonFlags.PrerequisitCheck)
	{
	}

	protected override bool CheckPrerequesits()
	{
		this.radiation = base.GetComponent<Radiation>();
		return this.radiation;
	}

	private void OnDestroy()
	{
		if (LocalRadiationEffect.geigerSoundPlayer)
		{
			UnityEngine.Object.Destroy(LocalRadiationEffect.geigerSoundPlayer);
		}
	}

	private void Update()
	{
		float radExposureScalar;
		float single;
		float single1;
		float single2;
		if (base.dead)
		{
			return;
		}
		if (!this.radiation)
		{
			radExposureScalar = 0f;
			single = 0f;
			single2 = 0f;
			single1 = 0f;
		}
		else
		{
			single2 = this.radiation.CalculateExposure(true);
			single1 = this.radiation.CalculateExposure(false);
			radExposureScalar = this.radiation.GetRadExposureScalar(single2);
			single = this.radiation.GetRadExposureScalar(single1);
		}
		ImageEffectManager.GetInstance<NoiseAndGrain>().intensityMultiplier = 10f * radExposureScalar;
		if (LocalRadiationEffect.geiger0 == null)
		{
			Bundling.Load<AudioClip>("content/item/sfx/geiger_low", out LocalRadiationEffect.geiger0);
			Bundling.Load<AudioClip>("content/item/sfx/geiger_medium", out LocalRadiationEffect.geiger1);
			Bundling.Load<AudioClip>("content/item/sfx/geiger_high", out LocalRadiationEffect.geiger2);
			Bundling.Load<AudioClip>("content/item/sfx/geiger_ultra", out LocalRadiationEffect.geiger3);
		}
		if (single1 >= 0.02f)
		{
			if (!LocalRadiationEffect.geigerSoundPlayer)
			{
				LocalRadiationEffect.geigerSoundPlayer = new GameObject("GEIGER SOUNDS", new Type[] { typeof(AudioSource) });
				LocalRadiationEffect.geigerSoundPlayer.transform.position = base.transform.position;
				LocalRadiationEffect.geigerSoundPlayer.transform.parent = base.transform;
				LocalRadiationEffect.geigerSoundPlayer.audio.loop = true;
			}
			AudioClip audioClip = null;
			if (single <= 0.25f)
			{
				audioClip = LocalRadiationEffect.geiger0;
			}
			else if (single > 0.5f)
			{
				audioClip = (single > 0.75f ? LocalRadiationEffect.geiger3 : LocalRadiationEffect.geiger2);
			}
			else
			{
				audioClip = LocalRadiationEffect.geiger1;
			}
			if (audioClip != LocalRadiationEffect.geigerSoundPlayer.audio.clip)
			{
				LocalRadiationEffect.geigerSoundPlayer.audio.Stop();
				LocalRadiationEffect.geigerSoundPlayer.audio.clip = audioClip;
				LocalRadiationEffect.geigerSoundPlayer.audio.Play();
			}
		}
		else if (LocalRadiationEffect.geigerSoundPlayer)
		{
			LocalRadiationEffect.geigerSoundPlayer.audio.Stop();
			UnityEngine.Object.Destroy(LocalRadiationEffect.geigerSoundPlayer);
			LocalRadiationEffect.geigerSoundPlayer = null;
		}
	}
}