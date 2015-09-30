using System;
using UnityEngine;

public class NearFarAdjustment : MonoBehaviour
{
	public NearFarAdjustment()
	{
	}

	private void Update()
	{
		if (!Physics.Raycast(new Ray(base.transform.position, base.transform.forward), 1.2f))
		{
			base.camera.nearClipPlane = 0.8f;
		}
		else
		{
			base.camera.nearClipPlane = 0.21f;
		}
	}
}