using System;
using UnityEngine;

[ExecuteInEditMode]
public class WaterColor : MonoBehaviour
{
	public Color colorMain = Color.green;

	public float colorLerp = 0.5f;

	private TOD_Sky sky;

	private WaterBase water;

	public WaterColor()
	{
	}

	private void Start()
	{
		this.sky = (TOD_Sky)UnityEngine.Object.FindObjectOfType(typeof(TOD_Sky));
		this.water = base.GetComponent<WaterBase>();
	}

	private void Update()
	{
		if (!this.sky || !this.water)
		{
			return;
		}
		Color color = Color.Lerp(this.sky.FogColor, this.sky.AmbientColor, 0.5f);
		color.a = 1f;
		Color color1 = Color.Lerp(color, this.colorMain, this.colorLerp) * 0.8f;
		color1.a = 0.1f;
		Color color2 = color1 * 0.8f;
		color2.a = 0.75f;
		this.water.sharedMaterial.SetColor("_ReflectionColor", color1);
		this.water.sharedMaterial.SetColor("_BaseColor", color2);
	}
}