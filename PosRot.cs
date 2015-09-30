using System;
using UnityEngine;

public struct PosRot
{
	public Vector3 position;

	public Quaternion rotation;

	public static void Lerp(ref PosRot a, ref PosRot b, float t, out PosRot v)
	{
		v = new PosRot();
		v.position = Vector3.Lerp(a.position, b.position, t);
		v.rotation = Quaternion.Slerp(a.rotation, b.rotation, t);
	}

	public static void Lerp(ref PosRot a, ref PosRot b, double t, out PosRot v)
	{
		PosRot.Lerp(ref a, ref b, (float)t, out v);
	}
}