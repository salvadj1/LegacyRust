using System;
using UnityEngine;

[Serializable]
public class TOD_DayParameters
{
	public Color AdditiveColor = Color.black;

	public Color SunMeshColor = new Color32(255, 233, 180, 255);

	public Color SunLightColor = new Color32(255, 243, 234, 255);

	public Color SunShaftColor = new Color32(255, 243, 234, 255);

	public float SunMeshSize = 1f;

	public float SunLightIntensity = 0.75f;

	public float AmbientIntensity = 0.75f;

	public float ShadowStrength = 1f;

	public float SkyMultiplier = 1f;

	public float CloudMultiplier = 1f;

	public TOD_DayParameters()
	{
	}

	public void CheckRange()
	{
		this.SunLightIntensity = Mathf.Max(0f, this.SunLightIntensity);
		this.SunMeshSize = Mathf.Max(0f, this.SunMeshSize);
		this.AmbientIntensity = Mathf.Clamp01(this.AmbientIntensity);
		this.ShadowStrength = Mathf.Clamp01(this.ShadowStrength);
		this.SkyMultiplier = Mathf.Clamp01(this.SkyMultiplier);
		this.CloudMultiplier = Mathf.Clamp01(this.CloudMultiplier);
	}
}