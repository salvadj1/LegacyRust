using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Viewport Camera")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class UIViewport : MonoBehaviour
{
	public Camera sourceCamera;

	public Transform topLeft;

	public Transform bottomRight;

	public float fullSize = 1f;

	private Camera mCam;

	public UIViewport()
	{
	}

	private void LateUpdate()
	{
		if (this.topLeft != null && this.bottomRight != null)
		{
			Vector3 screenPoint = this.sourceCamera.WorldToScreenPoint(this.topLeft.position);
			Vector3 vector3 = this.sourceCamera.WorldToScreenPoint(this.bottomRight.position);
			Rect rect = new Rect(screenPoint.x / (float)Screen.width, vector3.y / (float)Screen.height, (vector3.x - screenPoint.x) / (float)Screen.width, (screenPoint.y - vector3.y) / (float)Screen.height);
			float single = this.fullSize * rect.height;
			if (rect != this.mCam.rect)
			{
				this.mCam.rect = rect;
			}
			if (this.mCam.orthographicSize != single)
			{
				this.mCam.orthographicSize = single;
			}
		}
	}

	private void Start()
	{
		this.mCam = base.camera;
		if (this.sourceCamera == null)
		{
			this.sourceCamera = Camera.main;
		}
	}
}