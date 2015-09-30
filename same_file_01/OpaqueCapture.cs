using System;
using UnityEngine;

public class OpaqueCapture : PostEffectsBase
{
	private RenderTexture captureRT;

	private int w = -1;

	private int h = -1;

	private int d = -1;

	private RenderTextureFormat fmt;

	public OpaqueCapture()
	{
	}

	public override bool CheckResources()
	{
		this.CheckSupport(false);
		if (!this.isSupported)
		{
			this.ReportAutoDisable();
		}
		return this.isSupported;
	}

	private void CleanupCaptureRT()
	{
		if (this.captureRT)
		{
			UnityEngine.Object.DestroyImmediate(this.captureRT);
		}
		this.w = -1;
		this.h = -1;
		this.d = -1;
	}

	protected void OnDisable()
	{
		this.CleanupCaptureRT();
	}

	[ImageEffectOpaque]
	protected void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (!this.CheckResources())
		{
			Graphics.Blit(src, dst);
			return;
		}
		int num = src.width;
		int num1 = src.height;
		int num2 = src.depth;
		RenderTextureFormat renderTextureFormat = src.format;
		if (num != this.w || num1 != this.h || num2 != this.d || renderTextureFormat != this.fmt)
		{
			this.CleanupCaptureRT();
			RenderTexture renderTexture = new RenderTexture(num, num1, num2, renderTextureFormat)
			{
				hideFlags = HideFlags.DontSave
			};
			this.captureRT = renderTexture;
			if (!this.captureRT.Create() && !this.captureRT.IsCreated())
			{
				Graphics.Blit(src, dst);
				return;
			}
			this.captureRT.SetGlobalShaderProperty("_OpaqueFrame");
			this.w = num;
			this.h = num1;
			this.d = num2;
			this.fmt = renderTextureFormat;
		}
		Graphics.Blit(src, this.captureRT);
		Graphics.Blit(src, dst);
	}
}