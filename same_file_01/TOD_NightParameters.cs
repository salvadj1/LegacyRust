using System;
using UnityEngine;

[Serializable]
public class TOD_NightParameters
{
	public Color AdditiveColor = Color.black;

	public Color MoonMeshColor = new Color32(255, 233, 200, 255);

	public Color MoonLightColor = new Color32(181, 204, 255, 255);

	public Color MoonHaloColor = new Color32(81, 104, 155, 255);

	public float MoonMeshSize = 1f;

	public float MoonLightIntensity = 0.1f;

	public float AmbientIntensity = 0.2f;

	public float ShadowStrength = 1f;

	public float SkyMultiplier = 0.1f;

	public float CloudMultiplier = 0.2f;

	public TOD_NightParameters()
	{
	}

	public void CheckRange()
	{
		this.MoonLightIntensity = Mathf.Max(0f, this.MoonLightIntensity);
		this.MoonMeshSize = Mathf.Max(0f, this.MoonMeshSize);
		this.AmbientIntensity = Mathf.Clamp01(this.AmbientIntensity);
		this.ShadowStrength = Mathf.Clamp01(this.ShadowStrength);
		this.SkyMultiplier = Mathf.Clamp01(this.SkyMultiplier);
		this.CloudMultiplier = Mathf.Clamp01(this.CloudMultiplier);
	}
}