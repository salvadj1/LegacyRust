using System;
using UnityEngine;

[Serializable]
public class TOD_LightParameters
{
	public float MinimumHeight;

	public float Falloff = 0.7f;

	public float Coloring = 0.7f;

	public float SkyColoring = 0.5f;

	public float CloudColoring = 0.9f;

	public float ShaftColoring = 0.9f;

	public TOD_LightParameters()
	{
	}

	public void CheckRange()
	{
		this.MinimumHeight = Mathf.Clamp(this.MinimumHeight, -1f, 1f);
		this.Falloff = Mathf.Clamp01(this.Falloff);
		this.Coloring = Mathf.Clamp01(this.Coloring);
		this.SkyColoring = Mathf.Clamp01(this.SkyColoring);
		this.CloudColoring = Mathf.Clamp01(this.CloudColoring);
		this.ShaftColoring = Mathf.Clamp01(this.ShaftColoring);
	}
}