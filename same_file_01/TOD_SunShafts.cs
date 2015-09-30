using System;
using UnityEngine;

[AddComponentMenu("Time of Day/Camera Sun Shafts")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
internal class TOD_SunShafts : TOD_PostEffectsBase
{
	private const int PASS_DEPTH = 2;

	private const int PASS_NODEPTH = 3;

	private const int PASS_RADIAL = 1;

	private const int PASS_SCREEN = 0;

	private const int PASS_ADD = 4;

	public TOD_Sky sky;

	public TOD_SunShafts.SunShaftsResolution Resolution = TOD_SunShafts.SunShaftsResolution.Normal;

	public TOD_SunShafts.SunShaftsBlendMode BlendMode;

	public int RadialBlurIterations = 2;

	public float SunShaftBlurRadius = 2f;

	public float SunShaftIntensity = 1f;

	public float MaxRadius = 1f;

	public bool UseDepthTexture = true;

	public Shader SunShaftsShader;

	public Shader ScreenClearShader;

	private Material sunShaftsMaterial;

	private Material screenClearMaterial;

	public TOD_SunShafts()
	{
	}

	protected override bool CheckResources()
	{
		base.CheckSupport(this.UseDepthTexture);
		this.sunShaftsMaterial = base.CheckShaderAndCreateMaterial(this.SunShaftsShader, this.sunShaftsMaterial);
		this.screenClearMaterial = base.CheckShaderAndCreateMaterial(this.ScreenClearShader, this.screenClearMaterial);
		if (!this.isSupported)
		{
			base.ReportAutoDisable();
		}
		return this.isSupported;
	}

	protected void OnDisable()
	{
		if (this.sunShaftsMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.sunShaftsMaterial);
		}
		if (this.screenClearMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.screenClearMaterial);
		}
	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		int num;
		int num1;
		Vector4 vector4;
		if (!this.CheckResources() || !this.sky)
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.sky.Components.SunShafts = this;
		if (this.UseDepthTexture)
		{
			Camera camera = base.camera;
			camera.depthTextureMode = camera.depthTextureMode | DepthTextureMode.Depth;
		}
		if (this.Resolution == TOD_SunShafts.SunShaftsResolution.High)
		{
			num = source.width;
			num1 = source.height;
		}
		else if (this.Resolution != TOD_SunShafts.SunShaftsResolution.Normal)
		{
			num = source.width / 4;
			num1 = source.height / 4;
		}
		else
		{
			num = source.width / 2;
			num1 = source.height / 2;
		}
		Vector3 viewportPoint = base.camera.WorldToViewportPoint(this.sky.Components.SunTransform.position);
		this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * this.SunShaftBlurRadius);
		this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(viewportPoint.x, viewportPoint.y, viewportPoint.z, this.MaxRadius));
		RenderTexture temporary = RenderTexture.GetTemporary(num, num1, 0);
		RenderTexture renderTexture = RenderTexture.GetTemporary(num, num1, 0);
		if (!this.UseDepthTexture)
		{
			Graphics.Blit(source, temporary, this.sunShaftsMaterial, 3);
		}
		else
		{
			Graphics.Blit(source, temporary, this.sunShaftsMaterial, 2);
		}
		base.DrawBorder(temporary, this.screenClearMaterial);
		float sunShaftBlurRadius = this.SunShaftBlurRadius * 0.00130208337f;
		this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(sunShaftBlurRadius, sunShaftBlurRadius, 0f, 0f));
		this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(viewportPoint.x, viewportPoint.y, viewportPoint.z, this.MaxRadius));
		for (int i = 0; i < this.RadialBlurIterations; i++)
		{
			Graphics.Blit(temporary, renderTexture, this.sunShaftsMaterial, 1);
			sunShaftBlurRadius = this.SunShaftBlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(sunShaftBlurRadius, sunShaftBlurRadius, 0f, 0f));
			Graphics.Blit(renderTexture, temporary, this.sunShaftsMaterial, 1);
			sunShaftBlurRadius = this.SunShaftBlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(sunShaftBlurRadius, sunShaftBlurRadius, 0f, 0f));
		}
		vector4 = ((double)viewportPoint.z < 0 ? Vector4.zero : (1f - this.sky.Atmosphere.Fogginess) * this.SunShaftIntensity * this.sky.SunShaftColor);
		this.sunShaftsMaterial.SetVector("_SunColor", vector4);
		this.sunShaftsMaterial.SetTexture("_ColorBuffer", temporary);
		if (this.BlendMode != TOD_SunShafts.SunShaftsBlendMode.Screen)
		{
			Graphics.Blit(source, destination, this.sunShaftsMaterial, 4);
		}
		else
		{
			Graphics.Blit(source, destination, this.sunShaftsMaterial, 0);
		}
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	public enum SunShaftsBlendMode
	{
		Screen,
		Add
	}

	public enum SunShaftsResolution
	{
		Low,
		Normal,
		High
	}
}