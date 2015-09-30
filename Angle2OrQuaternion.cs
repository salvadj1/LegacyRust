using System;
using UnityEngine;

public struct Angle2OrQuaternion
{
	internal Quaternion quat;

	public static implicit operator Angle2OrQuaternion(Angle2 v)
	{
		Angle2OrQuaternion angle2OrQuaternion = new Angle2OrQuaternion();
		angle2OrQuaternion.quat = v.quat;
		return angle2OrQuaternion;
	}

	public static implicit operator Angle2OrQuaternion(Quaternion v)
	{
		Angle2OrQuaternion angle2OrQuaternion = new Angle2OrQuaternion();
		angle2OrQuaternion.quat = v;
		return angle2OrQuaternion;
	}
}