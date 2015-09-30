using System;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
	public Light myLight;

	private float initialIntensity;

	private float startTime;

	public MuzzleFlash()
	{
	}

	private void Start()
	{
		this.startTime = Time.time;
		this.initialIntensity = this.myLight.intensity;
	}

	private void Update()
	{
		float single = Mathf.Clamp(1f - (Time.time - this.startTime) / 0.1f, 0f, 1f);
		this.myLight.intensity = this.initialIntensity * single;
	}
}