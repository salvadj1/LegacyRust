using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SurveillanceCamera : MonoBehaviour
{
	public const int kWidth = 512;

	public const int kHeight = 512;

	public const int kDepth = 24;

	public const RenderTextureFormat kFormat = RenderTextureFormat.RGB565;

	public const float kAspect = 1f;

	private const int kRetireFrameCount = 3;

	public Camera camera;

	private int lastFrameRendered;

	private RenderTexture boundTarget;

	public SurveillanceCamera()
	{
	}

	private void Awake()
	{
		this.camera = base.camera;
		this.camera.enabled = false;
		base.enabled = false;
	}

	private void LateUpdate()
	{
		if (Mathf.Abs(this.lastFrameRendered - Time.frameCount) > 3)
		{
			this.camera.targetTexture = null;
			RenderTexture.ReleaseTemporary(this.boundTarget);
			this.boundTarget = null;
			base.enabled = false;
		}
	}

	private void OnDestroy()
	{
		if (this.boundTarget)
		{
			if (this.camera)
			{
				this.camera.targetTexture = null;
			}
			RenderTexture.ReleaseTemporary(this.boundTarget);
			this.boundTarget = null;
		}
	}

	public RenderTexture Render()
	{
		int num = Time.frameCount;
		if (this.lastFrameRendered == num)
		{
			return this.boundTarget;
		}
		bool flag = this.lastFrameRendered != num - 1;
		this.lastFrameRendered = Time.frameCount;
		if (flag && !this.boundTarget)
		{
			this.boundTarget = RenderTexture.GetTemporary(512, 512, 24, RenderTextureFormat.RGB565);
			base.enabled = true;
			this.camera.targetTexture = this.boundTarget;
			this.camera.ResetAspect();
		}
		this.camera.Render();
		return this.boundTarget;
	}
}