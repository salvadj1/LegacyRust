using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFXPost : MonoBehaviour
{
	private static int lastRenderFrame;

	private static bool didPostRender;

	public static CameraFX cameraFX;

	public static MountedCamera mountedCamera;

	public bool allowPostRenderCalls;

	static CameraFXPost()
	{
		CameraFXPost.lastRenderFrame = -100;
	}

	public CameraFXPost()
	{
	}

	private void OnPostRender()
	{
		if (this.allowPostRenderCalls)
		{
			if (Time.renderedFrameCount != CameraFXPost.lastRenderFrame || CameraFXPost.didPostRender)
			{
				return;
			}
			if (CameraFXPost.cameraFX)
			{
				CameraFXPost.cameraFX.PostPostRender();
			}
			CameraFXPost.didPostRender = true;
		}
	}

	private void OnPreCull()
	{
		if (CameraFXPost.lastRenderFrame == Time.renderedFrameCount)
		{
			return;
		}
		CameraFXPost.lastRenderFrame = Time.renderedFrameCount;
		CameraFXPost.didPostRender = false;
		if (CameraFXPost.cameraFX)
		{
			CameraFXPost.cameraFX.PostPreCull();
			if (CameraFXPost.mountedCamera)
			{
				CameraFXPost.mountedCamera.PreCullEnd(true);
			}
		}
		else if (CameraFXPost.mountedCamera)
		{
			CameraFXPost.mountedCamera.PreCullEnd(false);
		}
	}
}