using System;
using UnityEngine;

[Serializable]
public class TOD_CloudParameters
{
	public float Density = 3f;

	public float Sharpness = 3f;

	public float Brightness = 1f;

	public float ShadowStrength;

	public Vector2 Scale1 = new Vector2(3f, 3f);

	public Vector2 Scale2 = new Vector2(7f, 7f);

	public TOD_CloudParameters()
	{
	}

	public void CheckRange()
	{
		this.Scale1 = new Vector2(Mathf.Max(1f, this.Scale1.x), Mathf.Max(1f, this.Scale1.y));
		this.Scale2 = new Vector2(Mathf.Max(1f, this.Scale2.x), Mathf.Max(1f, this.Scale2.y));
		this.Density = Mathf.Max(0f, this.Density);
		this.Sharpness = Mathf.Max(0f, this.Sharpness);
		this.Brightness = Mathf.Max(0f, this.Brightness);
		this.ShadowStrength = Mathf.Clamp01(this.ShadowStrength);
	}
}