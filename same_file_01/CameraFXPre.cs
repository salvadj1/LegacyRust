using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFXPre : MonoBehaviour
{
	private static int lastRenderFrame;

	private static bool didPostRender;

	public static CameraFX cameraFX;

	public static MountedCamera mountedCamera;

	public bool allowPostRenderCalls;

	static CameraFXPre()
	{
		CameraFXPre.lastRenderFrame = -100;
	}

	public CameraFXPre()
	{
	}

	private void OnPostRender()
	{
		if (this.allowPostRenderCalls)
		{
			if (Time.renderedFrameCount != CameraFXPre.lastRenderFrame || CameraFXPre.didPostRender)
			{
				return;
			}
			if (CameraFXPre.cameraFX)
			{
				CameraFXPre.cameraFX.PrePostRender();
			}
			CameraFXPre.didPostRender = true;
		}
	}

	private void OnPreCull()
	{
		if (CameraFXPre.lastRenderFrame == Time.renderedFrameCount)
		{
			return;
		}
		CameraFXPre.lastRenderFrame = Time.renderedFrameCount;
		CameraFXPre.didPostRender = false;
		if (CameraFXPre.mountedCamera)
		{
			CameraFXPre.mountedCamera.PreCullBegin();
		}
		if (CameraFXPre.cameraFX)
		{
			CameraFXPre.cameraFX.PrePreCull();
		}
	}
}