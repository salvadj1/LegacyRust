using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class CurveUtility
{
	public static float EvaluateClampedTime(this AnimationCurve curve, ref float time, float advance)
	{
		int num = curve.length;
		if (curve.length == 0)
		{
			return 1f;
		}
		if (curve.length == 1)
		{
			return curve.Evaluate(0f);
		}
		if (advance > 0f)
		{
			float item = curve[num - 1].time;
			if (time < item)
			{
				time = time + advance;
				if (time > item)
				{
					time = item;
				}
			}
		}
		else if (advance < 0f)
		{
			float single = curve[0].time;
			if (time > single)
			{
				time = time + advance;
				if (time < single)
				{
					time = single;
				}
			}
		}
		return curve.Evaluate(time);
	}

	public static float GetEndTime(this AnimationCurve curve)
	{
		if (curve.length == 0)
		{
			return 0f;
		}
		return curve[curve.length - 1].time;
	}

	public static float GetStartTime(this AnimationCurve curve)
	{
		if (curve.length == 0)
		{
			return 0f;
		}
		return curve[0].time;
	}
}