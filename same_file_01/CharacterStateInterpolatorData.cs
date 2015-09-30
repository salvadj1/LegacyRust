using System;
using UnityEngine;

public struct CharacterStateInterpolatorData
{
	public Vector3 origin;

	public Angle2 eyesAngles;

	public CharacterStateFlags state;

	public static void Lerp(ref CharacterStateInterpolatorData a, ref CharacterStateInterpolatorData b, float t, out CharacterStateInterpolatorData result)
	{
		result = new CharacterStateInterpolatorData();
		if (t == 0f)
		{
			result = a;
		}
		else if (t != 1f)
		{
			float single = 1f - t;
			result.origin.x = a.origin.x * single + b.origin.x * t;
			result.origin.y = a.origin.y * single + b.origin.y * t;
			result.origin.z = a.origin.z * single + b.origin.z * t;
			result.eyesAngles = new Angle2()
			{
				yaw = a.eyesAngles.yaw + Mathf.DeltaAngle(a.eyesAngles.yaw, b.eyesAngles.yaw) * t,
				pitch = Mathf.DeltaAngle(0f, a.eyesAngles.pitch + Mathf.DeltaAngle(a.eyesAngles.pitch, b.eyesAngles.pitch) * t)
			};
			if (t > 1f)
			{
				result.state = b.state;
			}
			else if (t >= 0f)
			{
				result.state = a.state;
				result.state.flags = (ushort)(result.state.flags | (byte)(b.state.flags & 67));
				if (result.state.grounded != b.state.grounded)
				{
					result.state.grounded = false;
				}
			}
			else
			{
				result.state = a.state;
			}
		}
		else
		{
			result = b;
		}
	}
}