using System;
using UnityEngine;

public class Orbit : MonoBehaviour
{
	public Vector3 orbitPosition;

	public Vector3 orbitEulerSpeed;

	public bool orbitCenter;

	public Orbit()
	{
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawLine(this.orbitPosition, Vector3.zero);
		Gizmos.DrawSphere(this.orbitPosition, 0.01f);
	}

	private void Update()
	{
		Vector3 vector3 = new Vector3();
		float single = Time.deltaTime;
		if (single != 0f)
		{
			vector3.x = this.orbitEulerSpeed.x * single;
			vector3.y = this.orbitEulerSpeed.y * single;
			vector3.z = this.orbitEulerSpeed.z * single;
			if (vector3.x != 0f || vector3.y != 0f || vector3.z != 0f)
			{
				Quaternion quaternion = Quaternion.Euler(vector3);
				Quaternion quaternion1 = base.transform.localRotation * quaternion;
				if (!this.orbitCenter)
				{
					base.transform.localPosition = (quaternion1 * -this.orbitPosition) + this.orbitPosition;
				}
				else
				{
					base.transform.localPosition = quaternion1 * this.orbitPosition;
				}
				base.transform.localRotation = quaternion1;
			}
		}
	}
}