using System;
using UnityEngine;

[ExecuteInEditMode]
public class PivotTest : MonoBehaviour
{
	public Transform child;

	public Vector3 pivot;

	public Vector3 pivotAngles;

	public Vector3 offsetTranslation;

	public Vector3 offsetRotation;

	public PivotTest()
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireSphere(this.pivot, 0.01f);
		Gizmos.color = new Color(0f, 0f, 0f, 0.5f);
		Gizmos.DrawLine(Vector3.zero, this.offsetTranslation);
		Quaternion quaternion = Quaternion.Euler(this.pivotAngles);
		Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		Gizmos.DrawLine(this.offsetTranslation, ((quaternion * -this.pivot) + this.pivot) + this.offsetTranslation);
		Gizmos.matrix = Gizmos.matrix * Matrix4x4.TRS(this.pivot + this.offsetTranslation, Quaternion.Euler(this.pivotAngles), Vector3.one);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(Vector3.zero, Vector3.right);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(Vector3.zero, Vector3.up);
		if (this.child)
		{
			Gizmos.matrix = this.child.localToWorldMatrix;
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.02f, 0.02f, 0.02f));
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(Vector3.zero, Vector3.forward);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(Vector3.zero, Vector3.right);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(Vector3.zero, Vector3.up);
		}
	}

	public void Update()
	{
		if (this.child)
		{
			Quaternion quaternion = Quaternion.Euler(this.pivotAngles);
			Vector3 vector3 = (quaternion * -this.pivot) + this.pivot;
			vector3 = vector3 + this.offsetTranslation;
			this.child.localRotation = quaternion;
			this.child.localPosition = vector3;
		}
	}
}