using System;
using UnityEngine;

[ExecuteInEditMode]
public class TOD_Components : MonoBehaviour
{
	public GameObject Sun;

	public GameObject Moon;

	public GameObject Atmosphere;

	public GameObject Clear;

	public GameObject Clouds;

	public GameObject Space;

	public GameObject Light;

	public GameObject Projector;

	internal Transform DomeTransform;

	internal Transform SunTransform;

	internal Transform MoonTransform;

	internal Transform CameraTransform;

	internal Transform LightTransform;

	internal Renderer SpaceRenderer;

	internal Renderer AtmosphereRenderer;

	internal Renderer ClearRenderer;

	internal Renderer CloudRenderer;

	internal Renderer SunRenderer;

	internal Renderer MoonRenderer;

	internal MeshFilter SpaceMeshFilter;

	internal MeshFilter AtmosphereMeshFilter;

	internal MeshFilter ClearMeshFilter;

	internal MeshFilter CloudMeshFilter;

	internal MeshFilter SunMeshFilter;

	internal MeshFilter MoonMeshFilter;

	internal Material SpaceShader;

	internal Material AtmosphereShader;

	internal Material ClearShader;

	internal Material CloudShader;

	internal Material SunShader;

	internal Material MoonShader;

	internal Material ShadowShader;

	internal UnityEngine.Light LightSource;

	internal UnityEngine.Projector ShadowProjector;

	internal TOD_Sky Sky;

	internal TOD_Animation Animation;

	internal TOD_Time Time;

	internal TOD_Weather Weather;

	internal TOD_Resources Resources;

	internal TOD_SunShafts SunShafts;

	public TOD_Components()
	{
	}

	protected void OnEnable()
	{
		this.DomeTransform = base.transform;
		if (Camera.main == null)
		{
			Debug.LogWarning("Main camera does not exist or is not tagged 'MainCamera'.");
		}
		else
		{
			this.CameraTransform = Camera.main.transform;
		}
		this.Sky = base.GetComponent<TOD_Sky>();
		this.Animation = base.GetComponent<TOD_Animation>();
		this.Time = base.GetComponent<TOD_Time>();
		this.Weather = base.GetComponent<TOD_Weather>();
		this.Resources = base.GetComponent<TOD_Resources>();
		if (!this.Space)
		{
			Debug.LogError("Space reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.SpaceRenderer = this.Space.renderer;
		this.SpaceShader = this.SpaceRenderer.sharedMaterial;
		this.SpaceMeshFilter = this.Space.GetComponent<MeshFilter>();
		if (!this.Atmosphere)
		{
			Debug.LogError("Atmosphere reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.AtmosphereRenderer = this.Atmosphere.renderer;
		this.AtmosphereShader = this.AtmosphereRenderer.sharedMaterial;
		this.AtmosphereMeshFilter = this.Atmosphere.GetComponent<MeshFilter>();
		if (!this.Clear)
		{
			Debug.LogError("Clear reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.ClearRenderer = this.Clear.renderer;
		this.ClearShader = this.ClearRenderer.sharedMaterial;
		this.ClearMeshFilter = this.Clear.GetComponent<MeshFilter>();
		if (!this.Clouds)
		{
			Debug.LogError("Clouds reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.CloudRenderer = this.Clouds.renderer;
		this.CloudShader = this.CloudRenderer.sharedMaterial;
		this.CloudMeshFilter = this.Clouds.GetComponent<MeshFilter>();
		if (!this.Projector)
		{
			Debug.LogError("Projector reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.ShadowProjector = this.Projector.GetComponent<UnityEngine.Projector>();
		this.ShadowShader = this.ShadowProjector.material;
		if (!this.Light)
		{
			Debug.LogError("Light reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.LightTransform = this.Light.transform;
		this.LightSource = this.Light.light;
		if (!this.Sun)
		{
			Debug.LogError("Sun reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.SunTransform = this.Sun.transform;
		this.SunRenderer = this.Sun.renderer;
		this.SunShader = this.SunRenderer.sharedMaterial;
		this.SunMeshFilter = this.Sun.GetComponent<MeshFilter>();
		if (!this.Moon)
		{
			Debug.LogError("Moon reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.MoonTransform = this.Moon.transform;
		this.MoonRenderer = this.Moon.renderer;
		this.MoonShader = this.MoonRenderer.sharedMaterial;
		this.MoonMeshFilter = this.Moon.GetComponent<MeshFilter>();
	}
}