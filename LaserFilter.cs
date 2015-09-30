using System;
using UnityEngine;

public sealed class LaserFilter : MonoBehaviour
{
	[NonSerialized]
	private bool _gotCam;

	[NonSerialized]
	private Camera _camera;

	public new Camera camera
	{
		get
		{
			if (!this._gotCam)
			{
				this._gotCam = true;
				this._camera = base.camera;
			}
			return this._camera;
		}
	}

	public LaserFilter()
	{
	}

	private void OnPreCull()
	{
		if (base.enabled)
		{
			LaserGraphics.RenderLasersOnCamera(this.camera);
		}
	}
}