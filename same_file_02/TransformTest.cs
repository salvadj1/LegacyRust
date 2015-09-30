using Facepunch.Precision;
using System;
using UnityEngine;

public class TransformTest : MonoBehaviour
{
	public TransformTest()
	{
	}

	private void OnDrawGizmos()
	{
		Matrix4x4G matrix4x4G;
		Vector3G vector3G;
		Vector3G vector3G1;
		Vector3G vector3G2;
		Vector3G vector3G3;
		Vector3G vector3G4 = new Vector3G();
		base.transform.ExtractLocalToWorld(out matrix4x4G);
		Matrix4x4 matrix4x4 = base.transform.localToWorldMatrix;
		Vector3 vector3 = matrix4x4.MultiplyPoint(Vector3.zero);
		Vector3 vector31 = matrix4x4.MultiplyPoint(Vector3.forward);
		Vector3 vector32 = matrix4x4.MultiplyPoint(Vector3.up);
		Vector3 vector33 = matrix4x4.MultiplyPoint(Vector3.right);
		vector3G4.x = 1;
		vector3G4.y = 0;
		vector3G4.z = 0;
		Matrix4x4G.Mult(ref vector3G4, ref matrix4x4G, out vector3G);
		vector3G4.x = 0;
		vector3G4.y = 1;
		vector3G4.z = 0;
		Matrix4x4G.Mult(ref vector3G4, ref matrix4x4G, out vector3G1);
		vector3G4.x = 0;
		vector3G4.y = 0;
		vector3G4.z = 1;
		Matrix4x4G.Mult(ref vector3G4, ref matrix4x4G, out vector3G2);
		vector3G4.x = 0;
		vector3G4.y = 0;
		vector3G4.z = 0;
		Matrix4x4G.Mult(ref vector3G4, ref matrix4x4G, out vector3G3);
		Gizmos.color = Color.red * new Color(1f, 1f, 1f, 0.5f);
		Gizmos.DrawLine(vector3, vector33);
		Gizmos.color = Color.green * new Color(1f, 1f, 1f, 0.5f);
		Gizmos.DrawLine(vector3, vector32);
		Gizmos.color = Color.blue * new Color(1f, 1f, 1f, 0.5f);
		Gizmos.DrawLine(vector3, vector31);
		Gizmos.color = Color.red * new Color(1f, 1f, 1f, 1f);
		Gizmos.DrawLine(vector3G3.f, vector3G.f);
		Gizmos.color = Color.green * new Color(1f, 1f, 1f, 1f);
		Gizmos.DrawLine(vector3G3.f, vector3G1.f);
		Gizmos.color = Color.blue * new Color(1f, 1f, 1f, 1f);
		Gizmos.DrawLine(vector3G3.f, vector3G2.f);
	}
}