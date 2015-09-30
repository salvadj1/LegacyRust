using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class dfPivotExtensions
{
	public static Vector3 TransformToCenter(this dfPivotPoint pivot, Vector2 size)
	{
		switch (pivot)
		{
			case dfPivotPoint.TopLeft:
			{
				return new Vector2(0.5f * size.x, 0.5f * -size.y);
			}
			case dfPivotPoint.TopCenter:
			{
				return new Vector2(0f, 0.5f * -size.y);
			}
			case dfPivotPoint.TopRight:
			{
				return new Vector2(0.5f * -size.x, 0.5f * -size.y);
			}
			case dfPivotPoint.MiddleLeft:
			{
				return new Vector2(0.5f * size.x, 0f);
			}
			case dfPivotPoint.MiddleCenter:
			{
				return new Vector2(0f, 0f);
			}
			case dfPivotPoint.MiddleRight:
			{
				return new Vector2(0.5f * -size.x, 0f);
			}
			case dfPivotPoint.BottomLeft:
			{
				return new Vector2(0.5f * size.x, 0.5f * size.y);
			}
			case dfPivotPoint.BottomCenter:
			{
				return new Vector2(0f, 0.5f * size.y);
			}
			case dfPivotPoint.BottomRight:
			{
				return new Vector2(0.5f * -size.x, 0.5f * size.y);
			}
		}
		throw new Exception(string.Concat(new object[] { "Unhandled ", pivot.GetType().Name, " value: ", pivot }));
	}

	public static Vector3 TransformToUpperLeft(this dfPivotPoint pivot, Vector2 size)
	{
		switch (pivot)
		{
			case dfPivotPoint.TopLeft:
			{
				return new Vector2(0f, 0f);
			}
			case dfPivotPoint.TopCenter:
			{
				return new Vector2(0.5f * -size.x, 0f);
			}
			case dfPivotPoint.TopRight:
			{
				return new Vector2(-size.x, 0f);
			}
			case dfPivotPoint.MiddleLeft:
			{
				return new Vector2(0f, 0.5f * size.y);
			}
			case dfPivotPoint.MiddleCenter:
			{
				return new Vector2(0.5f * -size.x, 0.5f * size.y);
			}
			case dfPivotPoint.MiddleRight:
			{
				return new Vector2(-size.x, 0.5f * size.y);
			}
			case dfPivotPoint.BottomLeft:
			{
				return new Vector2(0f, size.y);
			}
			case dfPivotPoint.BottomCenter:
			{
				return new Vector2(0.5f * -size.x, size.y);
			}
			case dfPivotPoint.BottomRight:
			{
				return new Vector2(-size.x, size.y);
			}
		}
		throw new Exception(string.Concat(new object[] { "Unhandled ", pivot.GetType().Name, " value: ", pivot }));
	}

	public static Vector3 UpperLeftToTransform(this dfPivotPoint pivot, Vector2 size)
	{
		return pivot.TransformToUpperLeft(size).Scale(-1f, -1f, 1f);
	}
}