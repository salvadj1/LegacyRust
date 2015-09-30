using System;
using UnityEngine;

[AddComponentMenu("Time of Day/Camera Main Script")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class TOD_Camera : MonoBehaviour
{
	public TOD_Sky sky;

	public bool DomePosToCamera = true;

	public bool DomeScaleToFarClip;

	public float DomeScaleFactor = 0.95f;

	public TOD_Camera()
	{
	}

	protected void OnPreCull()
	{
		if (!this.sky)
		{
			return;
		}
		if (this.DomeScaleToFarClip)
		{
			float domeScaleFactor = this.DomeScaleFactor * base.camera.farClipPlane;
			Vector3 vector3 = new Vector3(domeScaleFactor, domeScaleFactor, domeScaleFactor);
			this.sky.transform.localScale = vector3;
		}
		if (this.DomePosToCamera)
		{
			Vector3 vector31 = base.transform.position;
			this.sky.transform.position = vector31;
		}
	}
}