using System;
using UnityEngine;

[ExecuteInEditMode]
public class ClearFirstFrame : MonoBehaviour
{
	public ClearFirstFrame()
	{
	}

	private void Disable()
	{
		base.enabled = false;
	}

	protected void OnPreRender()
	{
		GL.Clear(true, true, Color.black);
		this.Disable();
	}

	protected void Update()
	{
		if (base.camera.clearFlags != CameraClearFlags.Depth)
		{
			this.Disable();
		}
	}
}