using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PerspectiveFit : MonoBehaviour
{
	[PrefetchComponent]
	public Camera camera;

	public float targetDistance = 2.2f;

	public Vector2 targetSize = new Vector2(2.4f, 1.1f);

	public PerspectiveFit()
	{
	}

	private void OnPreCull()
	{
		float single;
		if (base.enabled && this.camera && this.camera.enabled)
		{
			float single1 = this.camera.aspect;
			float single2 = this.targetSize.x / this.targetSize.y;
			float single3 = Vector2.Angle(new Vector2(this.targetSize.x / single1 * 0.5f, this.targetDistance), new Vector2(0f, this.targetDistance)) * 2f;
			float single4 = Vector2.Angle(new Vector2(this.targetSize.y * 0.5f, this.targetDistance), new Vector2(0f, this.targetDistance)) * 2f;
			single = (single2 >= single1 ? single3 : single4);
			this.camera.fieldOfView = single;
		}
	}

	private void Reset()
	{
		if (!this.camera)
		{
			this.camera = base.camera;
		}
	}
}