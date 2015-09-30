using System;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
	public Light myLight;

	public float initialLightIntensity = 2f;

	public float startTime;

	public ExplosionEffect()
	{
	}

	public virtual void Start()
	{
		this.startTime = Time.time;
		UnityEngine.Object.Destroy(base.gameObject, 3f);
		base.audio.pitch = UnityEngine.Random.Range(0.9f, 1f);
		base.audio.Play();
	}

	public virtual void Update()
	{
		float single = Time.time - this.startTime;
		if (this.myLight)
		{
			this.myLight.intensity = Mathf.Clamp(this.initialLightIntensity * (1f - single / 0.25f), 0f, this.initialLightIntensity);
			if (this.myLight.intensity <= 0f)
			{
				UnityEngine.Object.Destroy(this.myLight.gameObject);
				this.myLight = null;
			}
		}
	}
}