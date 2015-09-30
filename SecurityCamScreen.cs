using System;
using UnityEngine;

public class SecurityCamScreen : MonoBehaviour
{
	public Camera RenderCamera;

	public float renderInterval;

	private bool firstInit = true;

	public SecurityCamScreen()
	{
	}

	private void Awake()
	{
		base.Invoke("UpdateCam", this.renderInterval);
	}

	private void UpdateCam()
	{
		Controllable controllable;
		if (!this.RenderCamera)
		{
			return;
		}
		PlayerClient localPlayer = PlayerClient.GetLocalPlayer();
		if (!localPlayer)
		{
			controllable = null;
		}
		else
		{
			controllable = localPlayer.controllable;
		}
		Controllable controllable1 = controllable;
		if (controllable1)
		{
			if (this.firstInit)
			{
				this.RenderCamera.Render();
				this.firstInit = false;
			}
			if (Vector3.Distance(controllable1.transform.position, base.transform.position) < 15f)
			{
				this.RenderCamera.Render();
			}
		}
		base.Invoke("UpdateCam", this.renderInterval);
	}
}