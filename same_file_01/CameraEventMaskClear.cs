using System;
using UnityEngine;

public sealed class CameraEventMaskClear : MonoBehaviour
{
	public CameraEventMaskClear()
	{
	}

	private void Awake()
	{
		base.camera.eventMask = 0;
	}
}