using System;
using UnityEngine;

public class QuickLight : MonoBehaviour
{
	public float range = 1f;

	public float duration = 0.25f;

	public QuickLight()
	{
	}

	public void Update()
	{
		Light light = base.light;
		light.range = light.range - Time.deltaTime / this.duration;
		if (base.light.range <= 0f)
		{
			base.light.range = 0f;
			base.light.intensity = 0f;
		}
	}
}