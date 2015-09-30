using System;
using UnityEngine;

[Serializable]
public class TOD_WorldParameters
{
	public bool SetAmbientLight;

	public bool SetFogColor;

	public float FogColorBias;

	public float ViewerHeight;

	public float HorizonOffset;

	public TOD_WorldParameters()
	{
	}

	public void CheckRange()
	{
		this.FogColorBias = Mathf.Clamp01(this.FogColorBias);
		this.ViewerHeight = Mathf.Clamp01(this.ViewerHeight);
		this.HorizonOffset = Mathf.Clamp01(this.HorizonOffset);
	}
}