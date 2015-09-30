using System;
using UnityEngine;

public class Camera_FollowTarget : MonoBehaviour
{
	public GameObject goTarget;

	public float flDistanceFromPlayer = 4f;

	public float flCameraYawOffset = 45f;

	private Quaternion quatCameraAngles;

	public bool bDropCamera;

	private Vector3 vecLastCameraPosition;

	public Camera_FollowTarget()
	{
	}

	private void Start()
	{
		this.quatCameraAngles = Quaternion.identity;
	}

	private void Update()
	{
		if (this.bDropCamera)
		{
			base.transform.position = this.vecLastCameraPosition;
		}
		else
		{
			this.UpdateCameraPosition();
		}
		base.transform.rotation = Quaternion.LookRotation((this.goTarget.transform.position + Vector3.up) - base.transform.position);
	}

	private void UpdateCameraPosition()
	{
		Vector3 vector3 = this.goTarget.transform.TransformDirection(Vector3.forward);
		Quaternion quaternion = Quaternion.AngleAxis(this.flCameraYawOffset, Vector3.up);
		base.transform.position = (this.goTarget.transform.position + Vector3.up) + ((quaternion * vector3) * this.flDistanceFromPlayer);
		this.vecLastCameraPosition = base.transform.position;
	}
}