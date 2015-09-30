using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Orthographic Camera")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class UIOrthoCamera : MonoBehaviour
{
	private Camera mCam;

	private Transform mTrans;

	public UIOrthoCamera()
	{
	}

	private void Start()
	{
		this.mCam = base.camera;
		this.mTrans = base.transform;
		this.mCam.orthographic = true;
	}

	private void Update()
	{
		Rect rect = this.mCam.rect;
		float single = rect.yMin * (float)Screen.height;
		Rect rect1 = this.mCam.rect;
		float single1 = rect1.yMax * (float)Screen.height;
		Vector3 vector3 = this.mTrans.lossyScale;
		float single2 = (single1 - single) * 0.5f * vector3.y;
		if (!Mathf.Approximately(this.mCam.orthographicSize, single2))
		{
			this.mCam.orthographicSize = single2;
		}
	}
}