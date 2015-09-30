using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Tweens/Rotation")]
public class dfTweenRotation : dfTweenComponent<Quaternion>
{
	public dfTweenRotation()
	{
	}

	public override Quaternion evaluate(Quaternion startValue, Quaternion endValue, float time)
	{
		Vector3 vector3 = startValue.eulerAngles;
		Vector3 vector31 = endValue.eulerAngles;
		return Quaternion.Euler(dfTweenRotation.LerpEuler(vector3, vector31, time));
	}

	private static float LerpAngle(float startValue, float endValue, float time)
	{
		float single = Mathf.Repeat(endValue - startValue, 360f);
		if (single > 180f)
		{
			single = single - 360f;
		}
		return startValue + single * time;
	}

	private static Vector3 LerpEuler(Vector3 startValue, Vector3 endValue, float time)
	{
		return new Vector3(dfTweenRotation.LerpAngle(startValue.x, endValue.x, time), dfTweenRotation.LerpAngle(startValue.y, endValue.y, time), dfTweenRotation.LerpAngle(startValue.z, endValue.z, time));
	}

	public override Quaternion offset(Quaternion lhs, Quaternion rhs)
	{
		return lhs * rhs;
	}
}