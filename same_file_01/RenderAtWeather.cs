using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RenderAtWeather : MonoBehaviour
{
	public TOD_Sky sky;

	public TOD_Weather.WeatherType type;

	private Renderer rendererComponent;

	public RenderAtWeather()
	{
	}

	protected void OnEnable()
	{
		if (!this.sky)
		{
			Debug.LogError("Sky instance reference not set. Disabling script.");
			base.enabled = false;
		}
		this.rendererComponent = base.renderer;
	}

	protected void Update()
	{
		this.rendererComponent.enabled = this.sky.Components.Weather.Weather == this.type;
	}
}