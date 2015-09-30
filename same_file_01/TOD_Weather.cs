using System;
using UnityEngine;

public class TOD_Weather : MonoBehaviour
{
	public float FadeTime = 10f;

	public TOD_Weather.CloudType Clouds;

	public TOD_Weather.WeatherType Weather;

	private float cloudBrightnessDefault;

	private float cloudDensityDefault;

	private float atmosphereFogDefault;

	private float cloudBrightness;

	private float cloudDensity;

	private float atmosphereFog;

	private float cloudSharpness;

	private TOD_Sky sky;

	public TOD_Weather()
	{
	}

	protected void Start()
	{
		this.sky = base.GetComponent<TOD_Sky>();
		float brightness = this.sky.Clouds.Brightness;
		float single = brightness;
		this.cloudBrightnessDefault = brightness;
		this.cloudBrightness = single;
		float density = this.sky.Clouds.Density;
		single = density;
		this.cloudDensityDefault = density;
		this.cloudDensity = single;
		float fogginess = this.sky.Atmosphere.Fogginess;
		single = fogginess;
		this.atmosphereFogDefault = fogginess;
		this.atmosphereFog = single;
		this.cloudSharpness = this.sky.Clouds.Sharpness;
	}

	protected void Update()
	{
		if (this.Clouds == TOD_Weather.CloudType.Custom && this.Weather == TOD_Weather.WeatherType.Custom)
		{
			return;
		}
		switch (this.Clouds)
		{
			case TOD_Weather.CloudType.Custom:
			{
				this.cloudDensity = this.sky.Clouds.Density;
				this.cloudSharpness = this.sky.Clouds.Sharpness;
				break;
			}
			case TOD_Weather.CloudType.None:
			{
				this.cloudDensity = 0f;
				this.cloudSharpness = 1f;
				break;
			}
			case TOD_Weather.CloudType.Few:
			{
				this.cloudDensity = this.cloudDensityDefault;
				this.cloudSharpness = 6f;
				break;
			}
			case TOD_Weather.CloudType.Scattered:
			{
				this.cloudDensity = this.cloudDensityDefault;
				this.cloudSharpness = 3f;
				break;
			}
			case TOD_Weather.CloudType.Broken:
			{
				this.cloudDensity = this.cloudDensityDefault;
				this.cloudSharpness = 1f;
				break;
			}
			case TOD_Weather.CloudType.Overcast:
			{
				this.cloudDensity = this.cloudDensityDefault;
				this.cloudSharpness = 0.1f;
				break;
			}
		}
		switch (this.Weather)
		{
			case TOD_Weather.WeatherType.Custom:
			{
				this.cloudBrightness = this.sky.Clouds.Brightness;
				this.atmosphereFog = this.sky.Atmosphere.Fogginess;
				break;
			}
			case TOD_Weather.WeatherType.Clear:
			{
				this.cloudBrightness = this.cloudBrightnessDefault;
				this.atmosphereFog = this.atmosphereFogDefault;
				break;
			}
			case TOD_Weather.WeatherType.Storm:
			{
				this.cloudBrightness = 0.3f;
				this.atmosphereFog = 1f;
				break;
			}
			case TOD_Weather.WeatherType.Dust:
			{
				this.cloudBrightness = this.cloudBrightnessDefault;
				this.atmosphereFog = 0.5f;
				break;
			}
			case TOD_Weather.WeatherType.Fog:
			{
				this.cloudBrightness = this.cloudBrightnessDefault;
				this.atmosphereFog = 1f;
				break;
			}
		}
		float fadeTime = Time.deltaTime / this.FadeTime;
		this.sky.Clouds.Brightness = Mathf.Lerp(this.sky.Clouds.Brightness, this.cloudBrightness, fadeTime);
		this.sky.Clouds.Density = Mathf.Lerp(this.sky.Clouds.Density, this.cloudDensity, fadeTime);
		this.sky.Clouds.Sharpness = Mathf.Lerp(this.sky.Clouds.Sharpness, this.cloudSharpness, fadeTime);
		this.sky.Atmosphere.Fogginess = Mathf.Lerp(this.sky.Atmosphere.Fogginess, this.atmosphereFog, fadeTime);
	}

	public enum CloudType
	{
		Custom,
		None,
		Few,
		Scattered,
		Broken,
		Overcast
	}

	public enum WeatherType
	{
		Custom,
		Clear,
		Storm,
		Dust,
		Fog
	}
}