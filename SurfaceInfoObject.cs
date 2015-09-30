using Facepunch;
using System;
using UnityEngine;

public class SurfaceInfoObject : ScriptableObject
{
	public static SurfaceInfoObject _default;

	public GameObject[] bulletEffects;

	public GameObject[] meleeEffects;

	public AudioClipArray bipedFootsteps;

	public AudioClipArray animalFootsteps;

	public SurfaceInfoObject()
	{
	}

	public static SurfaceInfoObject GetDefault()
	{
		if (SurfaceInfoObject._default == null)
		{
			Bundling.Load<SurfaceInfoObject>("rust/effects/impact/default", out SurfaceInfoObject._default);
			if (SurfaceInfoObject._default == null)
			{
				Debug.Log("COULD NOT GET DEFAULT!");
			}
		}
		return SurfaceInfoObject._default;
	}

	public AudioClip GetFootstepAnimal()
	{
		return this.animalFootsteps[UnityEngine.Random.Range(0, this.animalFootsteps.Length)];
	}

	public AudioClip GetFootstepBiped(AudioClip last)
	{
		int num = UnityEngine.Random.Range(0, this.bipedFootsteps.Length);
		AudioClip item = this.bipedFootsteps[num];
		if (last && item == last)
		{
			if (num < this.bipedFootsteps.Length - 1)
			{
				num++;
			}
			else if (num >= 1)
			{
				num--;
			}
			item = this.bipedFootsteps[num];
		}
		return this.bipedFootsteps[num];
	}

	public AudioClip GetFootstepBiped()
	{
		return this.bipedFootsteps[UnityEngine.Random.Range(0, this.bipedFootsteps.Length)];
	}

	public GameObject GetImpactEffect(SurfaceInfoObject.ImpactType type)
	{
		if (type == SurfaceInfoObject.ImpactType.Bullet)
		{
			return this.bulletEffects[UnityEngine.Random.Range(0, (int)this.bulletEffects.Length)];
		}
		if (type != SurfaceInfoObject.ImpactType.Melee)
		{
			return null;
		}
		return this.meleeEffects[UnityEngine.Random.Range(0, (int)this.meleeEffects.Length)];
	}

	public enum ImpactType
	{
		Melee,
		Bullet
	}
}