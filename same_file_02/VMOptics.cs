using System;
using UnityEngine;

public class VMOptics : MonoBehaviour
{
	public Socket.CameraSpace sightOverride;

	public VMOptics()
	{
	}

	private void OnDrawGizmosSelected()
	{
		this.sightOverride.DrawGizmos("sights");
	}
}