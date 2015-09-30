using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class RectExtensions
{
	public static RectOffset ConstrainPadding(this RectOffset borders)
	{
		if (borders == null)
		{
			return new RectOffset();
		}
		borders.left = Mathf.Max(0, borders.left);
		borders.right = Mathf.Max(0, borders.right);
		borders.top = Mathf.Max(0, borders.top);
		borders.bottom = Mathf.Max(0, borders.bottom);
		return borders;
	}

	public static bool Contains(this Rect rect, Rect other)
	{
		bool flag = rect.x <= other.x;
		bool flag1 = rect.x + rect.width >= other.x + other.width;
		bool flag2 = rect.yMin <= other.yMin;
		bool flag3 = rect.y + rect.height >= other.y + other.height;
		return (!flag || !flag1 || !flag2 ? false : flag3);
	}

	public static string Debug(this Rect rect)
	{
		return string.Format("[{0},{1},{2},{3}]", new object[] { rect.xMin, rect.yMin, rect.xMax, rect.yMax });
	}

	public static Rect Intersection(this Rect a, Rect b)
	{
		if (!a.Intersects(b))
		{
			return new Rect();
		}
		float single = Mathf.Max(a.xMin, b.xMin);
		float single1 = Mathf.Min(a.xMax, b.xMax);
		float single2 = Mathf.Max(a.yMin, b.yMin);
		float single3 = Mathf.Min(a.yMax, b.yMax);
		return Rect.MinMaxRect(single, single3, single1, single2);
	}

	public static bool Intersects(this Rect rect, Rect other)
	{
		return (rect.xMax < other.xMin || rect.yMax < other.xMin || rect.xMin > other.xMax ? false : rect.yMin <= other.yMax);
	}

	public static bool IsEmpty(this Rect rect)
	{
		return (rect.xMin == rect.xMax ? true : rect.yMin == rect.yMax);
	}

	public static Rect RoundToInt(this Rect rect)
	{
		return new Rect((float)Mathf.RoundToInt(rect.x), (float)Mathf.RoundToInt(rect.y), (float)Mathf.RoundToInt(rect.width), (float)Mathf.RoundToInt(rect.height));
	}

	public static Rect Union(this Rect a, Rect b)
	{
		float single = Mathf.Min(a.xMin, b.xMin);
		float single1 = Mathf.Max(a.xMax, b.xMax);
		float single2 = Mathf.Min(a.yMin, b.yMin);
		float single3 = Mathf.Max(a.yMax, b.yMax);
		return Rect.MinMaxRect(single, single2, single1, single3);
	}
}