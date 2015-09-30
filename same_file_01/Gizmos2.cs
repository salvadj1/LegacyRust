using System;
using UnityEngine;

public static class Gizmos2
{
	public static Color color
	{
		get
		{
			return Gizmos.color;
		}
		set
		{
			Gizmos.color = value;
		}
	}

	public static Matrix4x4 matrix
	{
		get
		{
			return Gizmos.matrix;
		}
		set
		{
			Gizmos.matrix = value;
		}
	}

	public static void DrawCube(Vector3 center, Vector3 size)
	{
		Gizmos.DrawCube(center, size);
	}

	public static void DrawFrustum(Vector3 center, float fov, float maxRange, float minRange, float aspect)
	{
		Gizmos.DrawFrustum(center, fov, maxRange, minRange, aspect);
	}

	public static void DrawGUITexture(Rect screenRect, Texture texture)
	{
		Gizmos.DrawGUITexture(screenRect, texture);
	}

	public static void DrawGUITexture(Rect screenRect, Texture texture, Material mat)
	{
		Gizmos.DrawGUITexture(screenRect, texture, mat);
	}

	public static void DrawGUITexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat)
	{
		Gizmos2.DrawGUITexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat);
	}

	public static void DrawGUITexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
	{
		Gizmos2.DrawGUITexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder);
	}

	public static void DrawIcon(Vector3 center, string name, bool allowScaling)
	{
		Gizmos.DrawIcon(center, name, allowScaling);
	}

	public static void DrawIcon(Vector3 center, string name)
	{
		Gizmos.DrawIcon(center, name);
	}

	public static void DrawLine(Vector3 from, Vector3 to)
	{
		Gizmos.DrawLine(from, to);
	}

	public static void DrawRay(Ray r)
	{
		Gizmos.DrawRay(r);
	}

	public static void DrawRay(Vector3 from, Vector3 direction)
	{
		Gizmos.DrawRay(from, direction);
	}

	public static void DrawSphere(Vector3 center, float radius)
	{
		Gizmos.DrawSphere(center, radius);
	}

	public static void DrawWireCapsule(Vector3 center, float radius, float height, int axis)
	{
		Vector3 vector3;
		Vector3 vector31;
		Vector3 vector32;
		switch (axis % 3)
		{
			case -2:
			case 1:
			{
				vector3 = Vector3.up;
				vector31 = Vector3.forward;
				vector32 = Vector3.right;
				break;
			}
			case -1:
			case 2:
			{
				vector3 = Vector3.forward;
				vector31 = Vector3.right;
				vector32 = Vector3.up;
				break;
			}
			case 0:
			{
				vector3 = Vector3.right;
				vector31 = Vector3.up;
				vector32 = Vector3.forward;
				break;
			}
			default:
			{
				return;
			}
		}
		Vector3 vector33 = Vector3.one - (vector31 * 2f);
		Vector3 vector34 = Vector3.one - (vector32 * 2f);
		if (radius * 2f < height)
		{
			Vector3 vector35 = center + (vector3 * ((height - radius * 2f) / 2f));
			Vector3 vector36 = center - (vector3 * ((height - radius * 2f) / 2f));
			Gizmos.DrawLine(vector35 + (vector31 * radius), vector36 + (vector31 * radius));
			Gizmos.DrawLine(vector35 + (vector32 * radius), vector36 + (vector32 * radius));
			Gizmos.DrawLine(vector35 - (vector31 * radius), vector36 - (vector31 * radius));
			Gizmos.DrawLine(vector35 - (vector32 * radius), vector36 - (vector32 * radius));
			for (int i = 0; i < 6; i++)
			{
				float single = (float)i / 12f * 3.14159274f;
				float single1 = ((float)i + 1f) / 12f * 3.14159274f;
				float single2 = Mathf.Cos(single) * radius;
				float single3 = Mathf.Sin(single) * radius;
				float single4 = Mathf.Cos(single1) * radius;
				float single5 = Mathf.Sin(single1) * radius;
				Vector3 vector37 = (vector3 * single3) + (vector31 * single2);
				Vector3 vector38 = (vector3 * single5) + (vector31 * single4);
				Vector3 vector39 = (vector3 * single3) + (vector32 * single2);
				Vector3 vector310 = (vector3 * single5) + (vector32 * single4);
				Vector3 vector311 = (vector31 * single3) + (vector32 * single2);
				Vector3 vector312 = (vector31 * single5) + (vector32 * single4);
				Gizmos.DrawLine(vector35 + vector37, vector35 + vector38);
				Gizmos.DrawLine(vector35 + vector39, vector35 + vector310);
				Gizmos.DrawLine(vector36 - vector37, vector36 - vector38);
				Gizmos.DrawLine(vector36 - vector39, vector36 - vector310);
				Gizmos.DrawLine(vector35 + vector311, vector35 + vector312);
				Gizmos.DrawLine(vector35 - vector311, vector35 - vector312);
				Gizmos.DrawLine(vector36 + vector311, vector36 + vector312);
				Gizmos.DrawLine(vector36 - vector311, vector36 - vector312);
				vector37 = Vector3.Scale(vector37, vector33);
				vector38 = Vector3.Scale(vector38, vector33);
				vector39 = Vector3.Scale(vector39, vector34);
				vector310 = Vector3.Scale(vector310, vector34);
				vector311 = Vector3.Scale(vector311, vector33);
				vector312 = Vector3.Scale(vector312, vector33);
				Gizmos.DrawLine(vector35 + vector37, vector35 + vector38);
				Gizmos.DrawLine(vector35 + vector39, vector35 + vector310);
				Gizmos.DrawLine(vector36 - vector37, vector36 - vector38);
				Gizmos.DrawLine(vector36 - vector39, vector36 - vector310);
				Gizmos.DrawLine(vector35 + vector311, vector35 + vector312);
				Gizmos.DrawLine(vector35 - vector311, vector35 - vector312);
				Gizmos.DrawLine(vector36 + vector311, vector36 + vector312);
				Gizmos.DrawLine(vector36 - vector311, vector36 - vector312);
			}
		}
		else
		{
			Gizmos.DrawWireSphere(center, radius);
		}
	}

	public static void DrawWireCube(Vector3 center, Vector3 size)
	{
		Gizmos.DrawWireCube(center, size);
	}

	public static void DrawWireSphere(Vector3 center, float radius)
	{
		Gizmos.DrawWireSphere(center, radius);
	}
}