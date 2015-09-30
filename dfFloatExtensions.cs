using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class dfFloatExtensions
{
	public static float Quantize(this float value, float stepSize)
	{
		if (stepSize <= 0f)
		{
			return value;
		}
		return Mathf.Floor(value / stepSize) * stepSize;
	}

	public static float RoundToNearest(this float value, float stepSize)
	{
		if (stepSize <= 0f)
		{
			return value;
		}
		return (float)Mathf.RoundToInt(value / stepSize) * stepSize;
	}
}