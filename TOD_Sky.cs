using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class TOD_Sky : MonoBehaviour
{
	private const float pi = 3.14159274f;

	private const float pi2 = 9.869605f;

	private const float pi3 = 31.006279f;

	private const float pi4 = 97.4091f;

	private Vector2 opticalDepth;

	private Vector3 oneOverBeta;

	private Vector3 betaRayleigh;

	private Vector3 betaRayleighTheta;

	private Vector3 betaMie;

	private Vector3 betaMieTheta;

	private Vector2 betaMiePhase;

	private Vector3 betaNight;

	public TOD_Sky.ColorSpaceDetection UnityColorSpace;

	public TOD_Sky.CloudQualityType CloudQuality = TOD_Sky.CloudQualityType.Bumped;

	public TOD_Sky.MeshQualityType MeshQuality = TOD_Sky.MeshQualityType.High;

	public TOD_CycleParameters Cycle;

	public TOD_AtmosphereParameters Atmosphere;

	public TOD_DayParameters Day;

	public TOD_NightParameters Night;

	public TOD_LightParameters Light;

	public TOD_StarParameters Stars;

	public TOD_CloudParameters Clouds;

	public TOD_WorldParameters World;

	internal Color AdditiveColor
	{
		get;
		private set;
	}

	internal Color AmbientColor
	{
		get;
		private set;
	}

	internal Color CloudColor
	{
		get;
		private set;
	}

	internal TOD_Components Components
	{
		get;
		private set;
	}

	internal Color FogColor
	{
		get
		{
			return (!this.World.SetFogColor ? this.SampleFogColor() : RenderSettings.fogColor);
		}
	}

	internal float Gamma
	{
		get
		{
			return ((this.UnityColorSpace != TOD_Sky.ColorSpaceDetection.Auto || QualitySettings.activeColorSpace != ColorSpace.Linear) && this.UnityColorSpace != TOD_Sky.ColorSpaceDetection.Linear ? 2.2f : 1f);
		}
	}

	internal bool IsDay
	{
		get
		{
			return this.LerpValue > 0f;
		}
	}

	internal bool IsNight
	{
		get
		{
			return this.LerpValue == 0f;
		}
	}

	internal float LerpValue
	{
		get;
		private set;
	}

	internal Color LightColor
	{
		get
		{
			return this.Components.LightSource.color;
		}
	}

	internal Vector3 LightDirection
	{
		get
		{
			return Vector3.Lerp(this.MoonDirection, this.SunDirection, this.LerpValue * this.LerpValue);
		}
	}

	internal float LightIntensity
	{
		get
		{
			return this.Components.LightSource.intensity;
		}
	}

	internal float LightZenith
	{
		get
		{
			return Mathf.Min(this.SunZenith, this.MoonZenith);
		}
	}

	internal Color MoonColor
	{
		get;
		private set;
	}

	internal Vector3 MoonDirection
	{
		get
		{
			return this.Components.MoonTransform.forward;
		}
	}

	internal Color MoonHaloColor
	{
		get;
		private set;
	}

	internal float MoonZenith
	{
		get;
		private set;
	}

	internal float OneOverGamma
	{
		get
		{
			return ((this.UnityColorSpace != TOD_Sky.ColorSpaceDetection.Auto || QualitySettings.activeColorSpace != ColorSpace.Linear) && this.UnityColorSpace != TOD_Sky.ColorSpaceDetection.Linear ? 0.454545438f : 1f);
		}
	}

	internal float Radius
	{
		get
		{
			return this.Components.DomeTransform.localScale.x;
		}
	}

	internal Color SunColor
	{
		get;
		private set;
	}

	internal Vector3 SunDirection
	{
		get
		{
			return this.Components.SunTransform.forward;
		}
	}

	internal Color SunShaftColor
	{
		get;
		private set;
	}

	internal float SunZenith
	{
		get;
		private set;
	}

	public TOD_Sky()
	{
	}

	private Vector3 Inverse(Vector3 v)
	{
		return new Vector3(1f / v.x, 1f / v.y, 1f / v.z);
	}

	private float Max3(float a, float b, float c)
	{
		float single;
		if (a < b || a < c)
		{
			single = (b < c ? c : b);
		}
		else
		{
			single = a;
		}
		return single;
	}

	protected void OnEnable()
	{
		this.Components = base.GetComponent<TOD_Components>();
		if (this.Components)
		{
			return;
		}
		Debug.LogError("TOD_Components not found. Disabling script.");
		base.enabled = false;
	}

	internal Vector3 OrbitalToLocal(float theta, float phi)
	{
		Vector3 vector3 = new Vector3();
		float single = Mathf.Sin(theta);
		float single1 = Mathf.Cos(theta);
		float single2 = Mathf.Sin(phi);
		vector3.z = single * Mathf.Cos(phi);
		vector3.y = single1;
		vector3.x = single * single2;
		return vector3;
	}

	internal Vector3 OrbitalToUnity(float radius, float theta, float phi)
	{
		Vector3 vector3 = new Vector3();
		float single = Mathf.Sin(theta);
		float single1 = Mathf.Cos(theta);
		float single2 = Mathf.Sin(phi);
		float single3 = Mathf.Cos(phi);
		vector3.z = radius * single * single3;
		vector3.y = radius * single1;
		vector3.x = radius * single * single2;
		return vector3;
	}

	private Color PowRGB(Color c, float p)
	{
		return new Color(Mathf.Pow(c.r, p), Mathf.Pow(c.g, p), Mathf.Pow(c.b, p), c.a);
	}

	private Color PowRGBA(Color c, float p)
	{
		return new Color(Mathf.Pow(c.r, p), Mathf.Pow(c.g, p), Mathf.Pow(c.b, p), Mathf.Pow(c.a, p));
	}

	internal Color SampleAtmosphere(Vector3 direction, bool clampAlpha = true)
	{
		direction = this.Components.DomeTransform.InverseTransformDirection(direction);
		float horizonOffset = this.World.HorizonOffset;
		float contrast = this.Atmosphere.Contrast * 0.454545438f;
		float haziness = this.Atmosphere.Haziness;
		float fogginess = this.Atmosphere.Fogginess;
		Color sunColor = this.SunColor;
		Color moonColor = this.MoonColor;
		Color moonHaloColor = this.MoonHaloColor;
		Color cloudColor = this.CloudColor;
		Color additiveColor = this.AdditiveColor;
		Vector3 vector3 = this.Components.DomeTransform.InverseTransformDirection(this.SunDirection);
		Vector3 vector31 = this.Components.DomeTransform.InverseTransformDirection(this.MoonDirection);
		Vector3 vector32 = this.opticalDepth;
		Vector3 vector33 = this.oneOverBeta;
		Vector3 vector34 = this.betaRayleigh;
		Vector3 vector35 = this.betaRayleighTheta;
		Vector3 vector36 = this.betaMie;
		Vector3 vector37 = this.betaMieTheta;
		Vector3 vector38 = this.betaMiePhase;
		Vector3 vector39 = this.betaNight;
		Color color = Color.black;
		float single = Mathf.Max(0f, Vector3.Dot(-direction, vector3));
		float single1 = Mathf.Clamp(direction.y + horizonOffset, 0.001f, 1f);
		float single2 = Mathf.Pow(single1, haziness);
		float single3 = (1f - single2) * 190000f;
		float single4 = single3 + single2 * (vector32.x - single3);
		float single5 = single3 + single2 * (vector32.y - single3);
		float single6 = 1f + single * single;
		Vector3 vector310 = (vector34 * single4) + (vector36 * single5);
		Vector3 vector311 = vector35 + (vector37 / Mathf.Pow(vector38.x - vector38.y * single, 1.5f));
		float single7 = sunColor.r;
		float single8 = sunColor.g;
		float single9 = sunColor.b;
		float single10 = moonColor.r;
		float single11 = moonColor.g;
		float single12 = moonColor.b;
		float single13 = Mathf.Exp(-vector310.x);
		float single14 = Mathf.Exp(-vector310.y);
		float single15 = Mathf.Exp(-vector310.z);
		float single16 = single6 * vector311.x * vector33.x;
		float single17 = single6 * vector311.y * vector33.y;
		float single18 = single6 * vector311.z * vector33.z;
		float single19 = vector39.x;
		float single20 = vector39.y;
		float single21 = vector39.z;
		color.r = (1f - single13) * (single7 * single16 + single10 * single19);
		color.g = (1f - single14) * (single8 * single17 + single11 * single20);
		color.b = (1f - single15) * (single9 * single18 + single12 * single21);
		color.a = 10f * this.Max3(color.r, color.g, color.b);
		color = color + (moonHaloColor * Mathf.Pow(Mathf.Max(0f, Vector3.Dot(vector31, -direction)), 10f));
		color = color + additiveColor;
		color.r = Mathf.Lerp(color.r, cloudColor.r, fogginess);
		color.g = Mathf.Lerp(color.g, cloudColor.g, fogginess);
		color.b = Mathf.Lerp(color.b, cloudColor.b, fogginess);
		color.a = color.a + fogginess;
		if (clampAlpha)
		{
			color.a = Mathf.Clamp01(color.a);
		}
		color = this.PowRGBA(color, contrast);
		return color;
	}

	private Color SampleFogColor()
	{
		Vector3 vector3 = (this.Components.CameraTransform == null ? Vector3.forward : this.Components.CameraTransform.forward);
		Vector3 vector31 = Vector3.Lerp(new Vector3(vector3.x, 0f, vector3.z), Vector3.up, this.World.FogColorBias);
		Color color = this.SampleAtmosphere(vector31, true);
		return new Color(color.a * color.r, color.a * color.g, color.a * color.b, 1f);
	}

	private void SetupLightSource(float theta, float phi)
	{
		float single;
		float shadowStrength;
		Vector3 local;
		float single1 = Mathf.Cos(Mathf.Pow(theta / 6.28318548f, 2f - this.Light.Falloff) * 2f * 3.14159274f);
		float single2 = Mathf.Sqrt(501264f * single1 * single1 + 1416f + 1f) - 708f * single1;
		float sunLightColor = this.Day.SunLightColor.r;
		float sunLightColor1 = this.Day.SunLightColor.g;
		float sunLightColor2 = this.Day.SunLightColor.b;
		float lightSource = this.Components.LightSource.intensity / Mathf.Max(this.Day.SunLightIntensity, this.Night.MoonLightIntensity);
		sunLightColor = sunLightColor * Mathf.Exp(-0.008735f * Mathf.Pow(0.68f, -4.08f * single2));
		sunLightColor1 = sunLightColor1 * Mathf.Exp(-0.008735f * Mathf.Pow(0.55f, -4.08f * single2));
		sunLightColor2 = sunLightColor2 * Mathf.Exp(-0.008735f * Mathf.Pow(0.44f, -4.08f * single2));
		this.LerpValue = Mathf.Clamp01(1.1f * this.Max3(sunLightColor, sunLightColor1, sunLightColor2));
		float moonLightColor = this.Night.MoonLightColor.r;
		float moonLightColor1 = this.Night.MoonLightColor.g;
		float moonLightColor2 = this.Night.MoonLightColor.b;
		float sunLightColor3 = this.Day.SunLightColor.r * Mathf.Lerp(1f, sunLightColor, this.Light.Coloring);
		float single3 = this.Day.SunLightColor.g * Mathf.Lerp(1f, sunLightColor1, this.Light.Coloring);
		float sunLightColor4 = this.Day.SunLightColor.b * Mathf.Lerp(1f, sunLightColor2, this.Light.Coloring);
		float sunShaftColor = this.Day.SunShaftColor.r * Mathf.Lerp(1f, sunLightColor, this.Light.ShaftColoring);
		float sunShaftColor1 = this.Day.SunShaftColor.g * Mathf.Lerp(1f, sunLightColor1, this.Light.ShaftColoring);
		float sunShaftColor2 = this.Day.SunShaftColor.b * Mathf.Lerp(1f, sunLightColor2, this.Light.ShaftColoring);
		Color color = new Color(moonLightColor, moonLightColor1, moonLightColor2, lightSource);
		Color color1 = new Color(sunLightColor3, single3, sunLightColor4, lightSource);
		Color color2 = Color.Lerp(color, color1, this.Max3(color1.r, color1.g, color1.b));
		this.SunShaftColor = new Color(sunShaftColor, sunShaftColor1, sunShaftColor2, lightSource);
		float single4 = Mathf.Lerp(this.Night.AmbientIntensity, this.Day.AmbientIntensity, this.LerpValue);
		this.AmbientColor = new Color(color2.r * single4, color2.g * single4, color2.b * single4, 1f);
		this.SunColor = this.Atmosphere.Brightness * this.Day.SkyMultiplier * Mathf.Lerp(1f, 0.1f, Mathf.Sqrt(this.SunZenith / 90f) - 0.25f) * Color.Lerp(this.Day.SunLightColor * this.LerpValue, new Color(sunLightColor, sunLightColor1, sunLightColor2, lightSource), this.Light.SkyColoring);
		float sunColor = this.SunColor.r;
		float sunColor1 = this.SunColor.g;
		Color sunColor2 = this.SunColor;
		this.SunColor = new Color(sunColor, sunColor1, sunColor2.b, this.LerpValue);
		this.MoonColor = (1f - this.LerpValue) * 0.5f * this.Atmosphere.Brightness * this.Night.SkyMultiplier * this.Night.MoonLightColor;
		float moonColor = this.MoonColor.r;
		float moonColor1 = this.MoonColor.g;
		Color moonColor2 = this.MoonColor;
		this.MoonColor = new Color(moonColor, moonColor1, moonColor2.b, 1f - this.LerpValue);
		Color lerpValue = (1f - this.LerpValue) * (1f - Mathf.Abs(this.Cycle.MoonPhase)) * this.Atmosphere.Brightness * this.Night.MoonHaloColor;
		lerpValue.r = lerpValue.r * lerpValue.a;
		lerpValue.g = lerpValue.g * lerpValue.a;
		lerpValue.b = lerpValue.b * lerpValue.a;
		lerpValue.a = this.Max3(lerpValue.r, lerpValue.g, lerpValue.b);
		this.MoonHaloColor = lerpValue;
		Color color3 = Color.Lerp(this.MoonColor, this.SunColor, this.LerpValue);
		float single5 = Mathf.Lerp(this.Night.CloudMultiplier, this.Day.CloudMultiplier, this.LerpValue);
		float single6 = (color3.r + color3.g + color3.b) / 3f;
		this.CloudColor = single5 * 1.25f * this.Clouds.Brightness * Color.Lerp(new Color(single6, single6, single6), color3, this.Light.CloudColoring);
		float cloudColor = this.CloudColor.r;
		float cloudColor1 = this.CloudColor.g;
		Color cloudColor2 = this.CloudColor;
		this.CloudColor = new Color(cloudColor, cloudColor1, cloudColor2.b, single5);
		Color color4 = Color.Lerp(this.Night.AdditiveColor, this.Day.AdditiveColor, this.LerpValue);
		color4.r = color4.r * color4.a;
		color4.g = color4.g * color4.a;
		color4.b = color4.b * color4.a;
		color4.a = this.Max3(color4.r, color4.g, color4.b);
		this.AdditiveColor = color4;
		if (this.LerpValue <= 0.2f)
		{
			float lerpValue1 = (0.2f - this.LerpValue) / 0.2f;
			float single7 = 1f - Mathf.Abs(this.Cycle.MoonPhase);
			single = Mathf.Lerp(0f, this.Night.MoonLightIntensity * single7, lerpValue1);
			shadowStrength = this.Night.ShadowStrength;
			local = this.OrbitalToLocal(Mathf.Max(theta + 3.14159274f, (1f + this.Light.MinimumHeight) * 3.14159274f / 2f + 3.14159274f), phi);
			this.Components.LightSource.color = color;
		}
		else
		{
			float lerpValue2 = (this.LerpValue - 0.2f) / 0.8f;
			single = Mathf.Lerp(0f, this.Day.SunLightIntensity, lerpValue2);
			shadowStrength = this.Day.ShadowStrength;
			local = this.OrbitalToLocal(Mathf.Min(theta, (1f - this.Light.MinimumHeight) * 3.14159274f / 2f), phi);
			this.Components.LightSource.color = color1;
		}
		LightShadows lightShadow = (this.Components.LightSource.shadowStrength != 0f ? LightShadows.Soft : LightShadows.None);
		this.Components.LightSource.intensity = single;
		this.Components.LightSource.shadowStrength = shadowStrength;
		this.Components.LightTransform.localPosition = local;
		this.Components.LightTransform.LookAt(this.Components.DomeTransform.position);
		this.Components.LightSource.shadows = lightShadow;
	}

	private void SetupQualitySettings()
	{
		Material spaceMaterial;
		TOD_Resources resources = this.Components.Resources;
		Material cloudMaterialFastest = null;
		Material shadowMaterialFastest = null;
		switch (this.CloudQuality)
		{
			case TOD_Sky.CloudQualityType.Fastest:
			{
				cloudMaterialFastest = resources.CloudMaterialFastest;
				shadowMaterialFastest = resources.ShadowMaterialFastest;
				break;
			}
			case TOD_Sky.CloudQualityType.Density:
			{
				cloudMaterialFastest = resources.CloudMaterialDensity;
				shadowMaterialFastest = resources.ShadowMaterialDensity;
				break;
			}
			case TOD_Sky.CloudQualityType.Bumped:
			{
				cloudMaterialFastest = resources.CloudMaterialBumped;
				shadowMaterialFastest = resources.ShadowMaterialBumped;
				break;
			}
			default:
			{
				Debug.LogError("Unknown cloud quality.");
				break;
			}
		}
		Mesh icosphereLow = null;
		Mesh icosphereMedium = null;
		Mesh mesh = null;
		Mesh halfIcosphereLow = null;
		Mesh quad = null;
		Mesh sphereLow = null;
		switch (this.MeshQuality)
		{
			case TOD_Sky.MeshQualityType.Low:
			{
				icosphereLow = resources.IcosphereLow;
				icosphereMedium = resources.IcosphereLow;
				mesh = resources.IcosphereLow;
				halfIcosphereLow = resources.HalfIcosphereLow;
				quad = resources.Quad;
				sphereLow = resources.SphereLow;
				break;
			}
			case TOD_Sky.MeshQualityType.Medium:
			{
				icosphereLow = resources.IcosphereMedium;
				icosphereMedium = resources.IcosphereMedium;
				mesh = resources.IcosphereLow;
				halfIcosphereLow = resources.HalfIcosphereMedium;
				quad = resources.Quad;
				sphereLow = resources.SphereMedium;
				break;
			}
			case TOD_Sky.MeshQualityType.High:
			{
				icosphereLow = resources.IcosphereHigh;
				icosphereMedium = resources.IcosphereHigh;
				mesh = resources.IcosphereLow;
				halfIcosphereLow = resources.HalfIcosphereHigh;
				quad = resources.Quad;
				sphereLow = resources.SphereHigh;
				break;
			}
			default:
			{
				Debug.LogError("Unknown mesh quality.");
				break;
			}
		}
		if (!this.Components.SpaceShader || this.Components.SpaceShader.name != resources.SpaceMaterial.name)
		{
			TOD_Components components = this.Components;
			spaceMaterial = resources.SpaceMaterial;
			this.Components.SpaceRenderer.sharedMaterial = spaceMaterial;
			components.SpaceShader = spaceMaterial;
		}
		if (!this.Components.AtmosphereShader || this.Components.AtmosphereShader.name != resources.AtmosphereMaterial.name)
		{
			TOD_Components tODComponent = this.Components;
			spaceMaterial = resources.AtmosphereMaterial;
			this.Components.AtmosphereRenderer.sharedMaterial = spaceMaterial;
			tODComponent.AtmosphereShader = spaceMaterial;
		}
		if (!this.Components.ClearShader || this.Components.ClearShader.name != resources.ClearMaterial.name)
		{
			TOD_Components components1 = this.Components;
			spaceMaterial = resources.ClearMaterial;
			this.Components.ClearRenderer.sharedMaterial = spaceMaterial;
			components1.ClearShader = spaceMaterial;
		}
		if (!this.Components.CloudShader || this.Components.CloudShader.name != cloudMaterialFastest.name)
		{
			TOD_Components tODComponent1 = this.Components;
			spaceMaterial = cloudMaterialFastest;
			this.Components.CloudRenderer.sharedMaterial = spaceMaterial;
			tODComponent1.CloudShader = spaceMaterial;
		}
		if (!this.Components.ShadowShader || this.Components.ShadowShader.name != shadowMaterialFastest.name)
		{
			TOD_Components components2 = this.Components;
			spaceMaterial = shadowMaterialFastest;
			this.Components.ShadowProjector.material = spaceMaterial;
			components2.ShadowShader = spaceMaterial;
		}
		if (!this.Components.SunShader || this.Components.SunShader.name != resources.SunMaterial.name)
		{
			TOD_Components tODComponent2 = this.Components;
			spaceMaterial = resources.SunMaterial;
			this.Components.SunRenderer.sharedMaterial = spaceMaterial;
			tODComponent2.SunShader = spaceMaterial;
		}
		if (!this.Components.MoonShader || this.Components.MoonShader.name != resources.MoonMaterial.name)
		{
			TOD_Components components3 = this.Components;
			spaceMaterial = resources.MoonMaterial;
			this.Components.MoonRenderer.sharedMaterial = spaceMaterial;
			components3.MoonShader = spaceMaterial;
		}
		if (this.Components.SpaceMeshFilter.sharedMesh != icosphereLow)
		{
			this.Components.SpaceMeshFilter.mesh = icosphereLow;
		}
		if (this.Components.AtmosphereMeshFilter.sharedMesh != icosphereMedium)
		{
			this.Components.AtmosphereMeshFilter.mesh = icosphereMedium;
		}
		if (this.Components.ClearMeshFilter.sharedMesh != mesh)
		{
			this.Components.ClearMeshFilter.mesh = mesh;
		}
		if (this.Components.CloudMeshFilter.sharedMesh != halfIcosphereLow)
		{
			this.Components.CloudMeshFilter.mesh = halfIcosphereLow;
		}
		if (this.Components.SunMeshFilter.sharedMesh != quad)
		{
			this.Components.SunMeshFilter.mesh = quad;
		}
		if (this.Components.MoonMeshFilter.sharedMesh != sphereLow)
		{
			this.Components.MoonMeshFilter.mesh = sphereLow;
		}
	}

	private void SetupScattering()
	{
		float rayleighMultiplier = 0.001f + this.Atmosphere.RayleighMultiplier * this.Atmosphere.ScatteringColor.r;
		float single = 0.001f + this.Atmosphere.RayleighMultiplier * this.Atmosphere.ScatteringColor.g;
		float rayleighMultiplier1 = 0.001f + this.Atmosphere.RayleighMultiplier * this.Atmosphere.ScatteringColor.b;
		this.betaRayleigh.x = 5.8E-06f * rayleighMultiplier;
		this.betaRayleigh.y = 1.35E-05f * single;
		this.betaRayleigh.z = 3.31E-05f * rayleighMultiplier1;
		this.betaRayleighTheta.x = 0.000116f * rayleighMultiplier * 0.0596831031f;
		this.betaRayleighTheta.y = 0.00027f * single * 0.0596831031f;
		this.betaRayleighTheta.z = 0.000662000035f * rayleighMultiplier1 * 0.0596831031f;
		this.opticalDepth.x = 8000f * Mathf.Exp(-this.World.ViewerHeight * 50000f / 8000f);
		float mieMultiplier = 0.001f + this.Atmosphere.MieMultiplier * this.Atmosphere.ScatteringColor.r;
		float mieMultiplier1 = 0.001f + this.Atmosphere.MieMultiplier * this.Atmosphere.ScatteringColor.g;
		float single1 = 0.001f + this.Atmosphere.MieMultiplier * this.Atmosphere.ScatteringColor.b;
		float directionality = this.Atmosphere.Directionality;
		float single2 = 0.238732412f * (1f - directionality * directionality) / (2f + directionality * directionality);
		this.betaMie.x = 2E-06f * mieMultiplier;
		this.betaMie.y = 2E-06f * mieMultiplier1;
		this.betaMie.z = 2E-06f * single1;
		this.betaMieTheta.x = 4E-05f * mieMultiplier * single2;
		this.betaMieTheta.y = 4E-05f * mieMultiplier1 * single2;
		this.betaMieTheta.z = 4E-05f * single1 * single2;
		this.betaMiePhase.x = 1f + directionality * directionality;
		this.betaMiePhase.y = 2f * directionality;
		this.opticalDepth.y = 1200f * Mathf.Exp(-this.World.ViewerHeight * 50000f / 1200f);
		this.oneOverBeta = this.Inverse(this.betaMie + this.betaRayleigh);
		this.betaNight = Vector3.Scale(this.betaRayleighTheta + (this.betaMieTheta / Mathf.Pow(this.betaMiePhase.x, 1.5f)), this.oneOverBeta);
	}

	private void SetupSunAndMoon()
	{
		float latitude = 0.0174532924f * this.Cycle.Latitude;
		float single = Mathf.Sin(latitude);
		float single1 = Mathf.Cos(latitude);
		float longitude = this.Cycle.Longitude;
		float year = (float)(367 * this.Cycle.Year - 7 * (this.Cycle.Year + (this.Cycle.Month + 9) / 12) / 4 + 275 * this.Cycle.Month / 9 + this.Cycle.Day - 730530);
		float hour = this.Cycle.Hour - this.Cycle.UTC;
		float single2 = 0.0174532924f * (23.4393f - 3.563E-07f * year);
		float single3 = Mathf.Sin(single2);
		float single4 = Mathf.Cos(single2);
		float single5 = 282.9404f + 4.70935E-05f * year;
		float single6 = 0.016709f - 1.151E-09f * year;
		float single7 = 356.047f + 0.985600233f * year;
		float single8 = 0.0174532924f * single7;
		float single9 = Mathf.Sin(single8);
		float single10 = Mathf.Cos(single8);
		float single11 = single7 + single6 * 57.29578f * single9 * (1f + single6 * single10);
		float single12 = 0.0174532924f * single11;
		float single13 = Mathf.Sin(single12);
		float single14 = Mathf.Cos(single12) - single6;
		float single15 = single13 * Mathf.Sqrt(1f - single6 * single6);
		float single16 = 57.29578f * Mathf.Atan2(single15, single14);
		float single17 = Mathf.Sqrt(single14 * single14 + single15 * single15);
		float single18 = 0.0174532924f * (single16 + single5);
		float single19 = Mathf.Sin(single18);
		float single20 = single17 * Mathf.Cos(single18);
		float single21 = single17 * single19;
		float single22 = single20;
		float single23 = single21 * single4;
		float single24 = single21 * single3;
		float single25 = 57.29578f * Mathf.Atan2(single23, single22);
		float single26 = Mathf.Atan2(single24, Mathf.Sqrt(single22 * single22 + single23 * single23));
		float single27 = Mathf.Sin(single26);
		float single28 = Mathf.Cos(single26);
		float single29 = single16 + single5 + 180f;
		float single30 = single29 + hour * 15f;
		float single31 = 0.0174532924f * (single30 + longitude - single25);
		float single32 = Mathf.Sin(single31);
		float single33 = Mathf.Cos(single31) * single28;
		float single34 = single32 * single28;
		float single35 = single27;
		float single36 = single33 * single - single35 * single1;
		float single37 = single34;
		float single38 = single33 * single1 + single35 * single;
		float single39 = Mathf.Atan2(single37, single36) + 3.14159274f;
		float single40 = Mathf.Atan2(single38, Mathf.Sqrt(single36 * single36 + single37 * single37));
		float single41 = 1.57079637f - single40;
		float single42 = single39;
		Vector3 local = this.OrbitalToLocal(single41, single42);
		this.Components.SunTransform.localPosition = local;
		this.Components.SunTransform.LookAt(this.Components.DomeTransform.position, this.Components.SunTransform.up);
		if (this.Components.CameraTransform != null)
		{
			Vector3 cameraTransform = this.Components.CameraTransform.rotation.eulerAngles;
			Vector3 sunTransform = this.Components.SunTransform.localEulerAngles;
			sunTransform.z = 2f * Time.time + Mathf.Abs(cameraTransform.x) + Mathf.Abs(cameraTransform.y) + Mathf.Abs(cameraTransform.z);
			this.Components.SunTransform.localEulerAngles = sunTransform;
		}
		Vector3 vector3 = this.OrbitalToLocal(single41 + 3.14159274f, single42);
		this.Components.MoonTransform.localPosition = vector3;
		this.Components.MoonTransform.LookAt(this.Components.DomeTransform.position, this.Components.MoonTransform.up);
		float single43 = 4f * Mathf.Tan(0.008726646f * this.Day.SunMeshSize);
		float single44 = 2f * single43;
		Vector3 vector31 = new Vector3(single44, single44, single44);
		this.Components.SunTransform.localScale = vector31;
		float single45 = 2f * Mathf.Tan(0.008726646f * this.Night.MoonMeshSize);
		float single46 = 2f * single45;
		Vector3 vector32 = new Vector3(single46, single46, single46);
		this.Components.MoonTransform.localScale = vector32;
		this.SunZenith = 57.29578f * single41;
		this.MoonZenith = Mathf.PingPong(this.SunZenith + 180f, 180f);
		bool flag = this.Components.SunTransform.localPosition.y > -0.5f;
		bool moonTransform = this.Components.MoonTransform.localPosition.y > -0.1f;
		Color color = this.SampleAtmosphere(Vector3.up, false);
		bool flag1 = color.a < 1.1f;
		bool density = this.Clouds.Density > 0f;
		this.Components.SunRenderer.enabled = flag;
		this.Components.MoonRenderer.enabled = moonTransform;
		this.Components.SpaceRenderer.enabled = flag1;
		this.Components.CloudRenderer.enabled = density;
		this.SetupLightSource(single41, single42);
	}

	protected void Update()
	{
		if (this.Components.SunShafts != null && this.Components.SunShafts.enabled)
		{
			if (!this.Components.ClearRenderer.enabled)
			{
				this.Components.ClearRenderer.enabled = true;
			}
		}
		else if (this.Components.ClearRenderer.enabled)
		{
			this.Components.ClearRenderer.enabled = false;
		}
		this.Cycle.CheckRange();
		this.SetupQualitySettings();
		this.SetupSunAndMoon();
		this.SetupScattering();
		if (this.World.SetFogColor)
		{
			RenderSettings.fogColor = this.SampleFogColor();
		}
		if (this.World.SetAmbientLight)
		{
			RenderSettings.ambientLight = this.AmbientColor;
		}
		Vector4 cloudUV = this.Components.Animation.CloudUV + this.Components.Animation.OffsetUV;
		Shader.SetGlobalFloat("TOD_Gamma", this.Gamma);
		Shader.SetGlobalFloat("TOD_OneOverGamma", this.OneOverGamma);
		Shader.SetGlobalColor("TOD_LightColor", this.LightColor);
		Shader.SetGlobalColor("TOD_CloudColor", this.CloudColor);
		Shader.SetGlobalColor("TOD_SunColor", this.SunColor);
		Shader.SetGlobalColor("TOD_MoonColor", this.MoonColor);
		Shader.SetGlobalColor("TOD_AdditiveColor", this.AdditiveColor);
		Shader.SetGlobalColor("TOD_MoonHaloColor", this.MoonHaloColor);
		Shader.SetGlobalVector("TOD_SunDirection", this.SunDirection);
		Shader.SetGlobalVector("TOD_MoonDirection", this.MoonDirection);
		Shader.SetGlobalVector("TOD_LightDirection", this.LightDirection);
		Shader.SetGlobalVector("TOD_LocalSunDirection", this.Components.DomeTransform.InverseTransformDirection(this.SunDirection));
		Shader.SetGlobalVector("TOD_LocalMoonDirection", this.Components.DomeTransform.InverseTransformDirection(this.MoonDirection));
		Shader.SetGlobalVector("TOD_LocalLightDirection", this.Components.DomeTransform.InverseTransformDirection(this.LightDirection));
		if (this.Components.AtmosphereShader != null)
		{
			this.Components.AtmosphereShader.SetFloat("_Contrast", this.Atmosphere.Contrast * this.OneOverGamma);
			this.Components.AtmosphereShader.SetFloat("_Haziness", this.Atmosphere.Haziness);
			this.Components.AtmosphereShader.SetFloat("_Fogginess", this.Atmosphere.Fogginess);
			this.Components.AtmosphereShader.SetFloat("_Horizon", this.World.HorizonOffset);
			this.Components.AtmosphereShader.SetVector("_OpticalDepth", this.opticalDepth);
			this.Components.AtmosphereShader.SetVector("_OneOverBeta", this.oneOverBeta);
			this.Components.AtmosphereShader.SetVector("_BetaRayleigh", this.betaRayleigh);
			this.Components.AtmosphereShader.SetVector("_BetaRayleighTheta", this.betaRayleighTheta);
			this.Components.AtmosphereShader.SetVector("_BetaMie", this.betaMie);
			this.Components.AtmosphereShader.SetVector("_BetaMieTheta", this.betaMieTheta);
			this.Components.AtmosphereShader.SetVector("_BetaMiePhase", this.betaMiePhase);
			this.Components.AtmosphereShader.SetVector("_BetaNight", this.betaNight);
		}
		if (this.Components.CloudShader != null)
		{
			float fogginess = (1f - this.Atmosphere.Fogginess) * this.LerpValue;
			float single = (1f - this.Atmosphere.Fogginess) * 0.6f * (1f - Mathf.Abs(this.Cycle.MoonPhase));
			this.Components.CloudShader.SetFloat("_SunGlow", fogginess);
			this.Components.CloudShader.SetFloat("_MoonGlow", single);
			this.Components.CloudShader.SetFloat("_CloudDensity", this.Clouds.Density);
			this.Components.CloudShader.SetFloat("_CloudSharpness", this.Clouds.Sharpness);
			this.Components.CloudShader.SetVector("_CloudScale1", this.Clouds.Scale1);
			this.Components.CloudShader.SetVector("_CloudScale2", this.Clouds.Scale2);
			this.Components.CloudShader.SetVector("_CloudUV", cloudUV);
		}
		if (this.Components.SpaceShader != null)
		{
			Vector2 vector2 = new Vector2(this.Stars.Tiling, this.Stars.Tiling);
			this.Components.SpaceShader.mainTextureScale = vector2;
			this.Components.SpaceShader.SetFloat("_Subtract", 1f - Mathf.Pow(this.Stars.Density, 0.1f));
		}
		if (this.Components.SunShader != null)
		{
			this.Components.SunShader.SetColor("_Color", (this.Day.SunMeshColor * this.LerpValue) * (1f - this.Atmosphere.Fogginess));
		}
		if (this.Components.MoonShader != null)
		{
			this.Components.MoonShader.SetColor("_Color", this.Night.MoonMeshColor);
			this.Components.MoonShader.SetFloat("_Phase", this.Cycle.MoonPhase);
		}
		if (this.Components.ShadowShader != null)
		{
			float shadowStrength = this.Clouds.ShadowStrength * Mathf.Clamp01(1f - this.LightZenith / 90f);
			this.Components.ShadowShader.SetFloat("_Alpha", shadowStrength);
			this.Components.ShadowShader.SetFloat("_CloudDensity", this.Clouds.Density);
			this.Components.ShadowShader.SetFloat("_CloudSharpness", this.Clouds.Sharpness);
			this.Components.ShadowShader.SetVector("_CloudScale1", this.Clouds.Scale1);
			this.Components.ShadowShader.SetVector("_CloudScale2", this.Clouds.Scale2);
			this.Components.ShadowShader.SetVector("_CloudUV", cloudUV);
		}
		if (this.Components.ShadowProjector != null)
		{
			bool flag = (this.Clouds.ShadowStrength == 0f ? false : this.Components.ShadowShader != null);
			float radius = this.Radius * 2f;
			float radius1 = this.Radius;
			this.Components.ShadowProjector.enabled = flag;
			this.Components.ShadowProjector.farClipPlane = radius;
			this.Components.ShadowProjector.orthographicSize = radius1;
		}
	}

	public enum CloudQualityType
	{
		Fastest,
		Density,
		Bumped
	}

	public enum ColorSpaceDetection
	{
		Auto,
		Linear,
		Gamma
	}

	public enum MeshQualityType
	{
		Low,
		Medium,
		High
	}
}