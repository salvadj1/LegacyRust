using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public abstract class TOD_PostEffectsBase : MonoBehaviour
{
	protected bool isSupported = true;

	protected TOD_PostEffectsBase()
	{
	}

	protected abstract bool CheckResources();

	protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
	{
		Material material1;
		if (!shader)
		{
			Debug.Log(string.Concat("Missing shader in ", this.ToString()));
			base.enabled = false;
			return null;
		}
		if (shader.isSupported && material && material.shader == shader)
		{
			return material;
		}
		if (shader.isSupported)
		{
			material = new Material(shader)
			{
				hideFlags = HideFlags.DontSave
			};
			if (!material)
			{
				material1 = null;
			}
			else
			{
				material1 = material;
			}
			return material1;
		}
		this.NotSupported();
		Debug.LogError(string.Concat(new string[] { "The shader ", shader.ToString(), " on effect ", this.ToString(), " is not supported on this platform!" }));
		return null;
	}

	protected bool CheckSupport(bool needDepth)
	{
		this.isSupported = true;
		if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
		{
			this.NotSupported();
			return false;
		}
		if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			this.NotSupported();
			return false;
		}
		if (needDepth)
		{
			Camera camera = base.camera;
			camera.depthTextureMode = camera.depthTextureMode | DepthTextureMode.Depth;
		}
		return true;
	}

	protected bool CheckSupport(bool needDepth, bool needHdr)
	{
		if (!this.CheckSupport(needDepth))
		{
			return false;
		}
		if (!needHdr || SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
		{
			return true;
		}
		this.NotSupported();
		return false;
	}

	protected Material CreateMaterial(Shader shader, Material material)
	{
		Material material1;
		if (!shader)
		{
			Debug.Log(string.Concat("Missing shader in ", this.ToString()));
			return null;
		}
		if (material && material.shader == shader && shader.isSupported)
		{
			return material;
		}
		if (!shader.isSupported)
		{
			return null;
		}
		material = new Material(shader)
		{
			hideFlags = HideFlags.DontSave
		};
		if (!material)
		{
			material1 = null;
		}
		else
		{
			material1 = material;
		}
		return material1;
	}

	protected void DrawBorder(RenderTexture dest, Material material)
	{
		float single;
		float single1;
		RenderTexture.active = dest;
		bool flag = true;
		GL.PushMatrix();
		GL.LoadOrtho();
		for (int i = 0; i < material.passCount; i++)
		{
			material.SetPass(i);
			if (!flag)
			{
				single = 0f;
				single1 = 1f;
			}
			else
			{
				single = 1f;
				single1 = 0f;
			}
			float single2 = 0f;
			float single3 = 0f + 1f / ((float)dest.width * 1f);
			float single4 = 0f;
			float single5 = 1f;
			GL.Begin(7);
			GL.TexCoord2(0f, single);
			GL.Vertex3(single2, single4, 0.1f);
			GL.TexCoord2(1f, single);
			GL.Vertex3(single3, single4, 0.1f);
			GL.TexCoord2(1f, single1);
			GL.Vertex3(single3, single5, 0.1f);
			GL.TexCoord2(0f, single1);
			GL.Vertex3(single2, single5, 0.1f);
			single2 = 1f - 1f / ((float)dest.width * 1f);
			single3 = 1f;
			single4 = 0f;
			single5 = 1f;
			GL.TexCoord2(0f, single);
			GL.Vertex3(single2, single4, 0.1f);
			GL.TexCoord2(1f, single);
			GL.Vertex3(single3, single4, 0.1f);
			GL.TexCoord2(1f, single1);
			GL.Vertex3(single3, single5, 0.1f);
			GL.TexCoord2(0f, single1);
			GL.Vertex3(single2, single5, 0.1f);
			single2 = 0f;
			single3 = 1f;
			single4 = 0f;
			single5 = 0f + 1f / ((float)dest.height * 1f);
			GL.TexCoord2(0f, single);
			GL.Vertex3(single2, single4, 0.1f);
			GL.TexCoord2(1f, single);
			GL.Vertex3(single3, single4, 0.1f);
			GL.TexCoord2(1f, single1);
			GL.Vertex3(single3, single5, 0.1f);
			GL.TexCoord2(0f, single1);
			GL.Vertex3(single2, single5, 0.1f);
			single2 = 0f;
			single3 = 1f;
			single4 = 1f - 1f / ((float)dest.height * 1f);
			single5 = 1f;
			GL.TexCoord2(0f, single);
			GL.Vertex3(single2, single4, 0.1f);
			GL.TexCoord2(1f, single);
			GL.Vertex3(single3, single4, 0.1f);
			GL.TexCoord2(1f, single1);
			GL.Vertex3(single3, single5, 0.1f);
			GL.TexCoord2(0f, single1);
			GL.Vertex3(single2, single5, 0.1f);
			GL.End();
		}
		GL.PopMatrix();
	}

	protected void NotSupported()
	{
		base.enabled = false;
		this.isSupported = false;
	}

	protected void OnEnable()
	{
		this.isSupported = true;
	}

	protected void ReportAutoDisable()
	{
		Debug.LogWarning(string.Concat("The image effect ", this.ToString(), " has been disabled as it's not supported on the current platform."));
	}

	protected void Start()
	{
		this.CheckResources();
	}
}