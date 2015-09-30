using System;
using UnityEngine;

public static class VisGizmosUtility
{
	private const int numCircleVerts = 32;

	private const int lengthCircleVerts = 31;

	private const float degreePerCircleVert = 11.25f;

	private const float radPerCircleVert = 0.196349546f;

	private const int halveCircleIndex = 16;

	private static Matrix4x4[] matStack;

	private static int stackPos;

	private static Vector3[] circleVerts;

	private readonly static Matrix4x4 ninetyX;

	private readonly static Matrix4x4 ninetyY;

	private readonly static Matrix4x4 ninetyZ;

	static VisGizmosUtility()
	{
		VisGizmosUtility.matStack = new Matrix4x4[8];
		VisGizmosUtility.stackPos = 0;
		VisGizmosUtility.ninetyX = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90f, 0f, 0f), Vector3.one);
		VisGizmosUtility.ninetyY = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 90f, 0f), Vector3.one);
		VisGizmosUtility.ninetyZ = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 90f), Vector3.one);
		VisGizmosUtility.circleVerts = new Vector3[31];
		for (int i = 0; i < 31; i++)
		{
			float single = 0.196349546f * (float)i;
			VisGizmosUtility.circleVerts[i].x = Mathf.Cos(single);
			VisGizmosUtility.circleVerts[i].y = Mathf.Sin(single);
		}
	}

	public static void DrawAngle(Vector3 origin, Vector3 heading, Vector3 axis, float angle, float radius)
	{
		Vector3 vector3;
		VisGizmosUtility.PushMatrix();
		if (angle < 0f)
		{
			axis = -axis;
			angle = -angle;
		}
		Vector3 vector31 = Vector3.Cross(axis, heading);
		Gizmos.matrix = Matrix4x4.TRS(origin, Quaternion.LookRotation(axis, vector31), new Vector3(radius, radius, 1f)) * Gizmos.matrix;
		Vector3 vector32 = Vector3.zero;
		if (angle == 0f)
		{
			Gizmos.DrawLine(Vector3.zero, new Vector3(0f, 1f, 0f));
		}
		else if (angle < 360f)
		{
			int num = 0;
			float single = 0f;
			do
			{
				int num1 = num;
				num = num1 + 1;
				vector3 = VisGizmosUtility.circleVerts[num1];
				Gizmos.DrawLine(vector32, vector3);
				vector32 = vector3;
				single = single + 11.25f;
			}
			while (single < angle);
			if (single != angle)
			{
				vector3 = new Vector3(Mathf.Cos(angle * 0.0174532924f), Mathf.Sin(angle * 0.0174532924f));
				Gizmos.DrawLine(vector32, vector3);
				vector32 = vector3;
			}
			Gizmos.DrawLine(vector32, Vector3.zero);
		}
		VisGizmosUtility.PopMatrix();
	}

	public static void DrawCapsule(Vector3 capA, Vector3 capB, float radius)
	{
		if (radius != 0f)
		{
			float single = Vector3.Distance(capA, capB);
			if (single != 0f)
			{
				VisGizmosUtility.PushMatrix();
				Matrix4x4 matrix4x4 = Matrix4x4.TRS(capA, VisGizmosUtility.MagicFlat(capA, capB), new Vector3(radius, radius, radius)) * Gizmos.matrix;
				Gizmos.matrix = matrix4x4;
				float single1 = single / radius;
				VisGizmosUtility.DrawFlatCapsule(single1);
				Gizmos.matrix = VisGizmosUtility.ninetyZ * matrix4x4;
				VisGizmosUtility.DrawFlatCapsule(single1);
				Gizmos.matrix = VisGizmosUtility.ninetyY * Gizmos.matrix;
				VisGizmosUtility.DrawFlatCircle();
				Gizmos.matrix = VisGizmosUtility.ninetyY * matrix4x4;
				VisGizmosUtility.DrawFlatCircle();
				VisGizmosUtility.PopMatrix();
			}
			else
			{
				VisGizmosUtility.DrawSphere(capA, radius);
			}
		}
		else
		{
			Gizmos.DrawLine(capA, capB);
		}
	}

	public static void DrawCapsule(Vector3 center, float length, float radius, Vector3 heading)
	{
		length = Mathf.Max(length - radius * 2f, 0f);
		if (length == 0f)
		{
			VisGizmosUtility.DrawSphere(center, radius, heading);
		}
		heading.Normalize();
		length = length / 2f;
		VisGizmosUtility.DrawCapsule(center - (heading * length), center + (heading * length), radius);
	}

	public static void DrawCircle(Vector3 origin, Vector3 axis, float radius)
	{
		VisGizmosUtility.PushMatrix();
		Gizmos.matrix = Matrix4x4.TRS(origin, Quaternion.LookRotation(axis), new Vector3(radius, radius, 1f)) * Gizmos.matrix;
		VisGizmosUtility.DrawFlatCircle();
		VisGizmosUtility.PopMatrix();
	}

	public static void DrawDotArc(Vector3 position, Transform transform, float length, float arc, float back)
	{
		Vector3 vector3 = transform.forward;
		VisGizmosUtility.DrawDotCone(position, vector3, arc * length, arc, back);
		float single = Mathf.Acos(arc) * 57.29578f;
		Vector3 vector31 = transform.up;
		Vector3 vector32 = transform.right;
		VisGizmosUtility.DrawAngle(position, vector3, vector31, single, length);
		VisGizmosUtility.DrawAngle(position, vector3, vector31, -single, length);
		VisGizmosUtility.DrawAngle(position, vector3, vector32, single, length);
		VisGizmosUtility.DrawAngle(position, vector3, vector32, -single, length);
	}

	public static void DrawDotCone(Vector3 position, Vector3 forward, float length, float arc)
	{
		VisGizmosUtility.DrawDotCone(position, forward, length, arc, 0f);
	}

	public static void DrawDotCone(Vector3 position, Vector3 forward, float length, float arc, float back)
	{
		int num;
		float single;
		float single1;
		Matrix4x4 matrix4x4;
		if (arc != 1f)
		{
			float single2 = Mathf.Ceil(length);
			if (single2 != 0f)
			{
				float single3 = Mathf.Acos(arc);
				int num1 = Mathf.Abs((int)single2);
				float single4 = length / single2;
				float single5 = single4 * single3;
				if (back != 0f)
				{
					single2 = 0f;
					num = 0;
					single = single3 * back;
					single1 = single;
				}
				else
				{
					single2 = single4;
					num = 1;
					single = single5;
					single1 = 0f;
				}
				VisGizmosUtility.PushMatrixMul(Matrix4x4.TRS(position, Quaternion.LookRotation(forward), Vector3.one), out matrix4x4);
				Vector3 vector3 = new Vector3(single1, 0f, 0f);
				Vector3 vector31 = new Vector3(single1 + single3 * length, 0f, length);
				Gizmos.DrawLine(vector3, vector31);
				vector3.x = -vector3.x;
				vector31.x = -vector31.x;
				Gizmos.DrawLine(vector3, vector31);
				vector3.y = vector3.x;
				vector3.x = 0f;
				vector31.y = vector31.x;
				vector31.x = 0f;
				Gizmos.DrawLine(vector3, vector31);
				vector3.y = -vector3.y;
				vector31.y = -vector31.y;
				Gizmos.DrawLine(vector3, vector31);
				while (num <= num1)
				{
					Gizmos.matrix = matrix4x4 * Matrix4x4.TRS(new Vector3(0f, 0f, single2), Quaternion.identity, new Vector3(single, single, 1f));
					VisGizmosUtility.DrawFlatCircle();
					num++;
					single2 = single2 + single4;
					single = single + single5;
				}
				VisGizmosUtility.PopMatrix();
			}
		}
		else
		{
			Gizmos.DrawLine(position, position + (forward * length));
		}
	}

	public static void DrawFlatCapEnd()
	{
		int num = 30;
		int num1 = 0;
		do
		{
			Gizmos.DrawLine(VisGizmosUtility.circleVerts[num], VisGizmosUtility.circleVerts[num1]);
			int num2 = num1;
			num1 = num2 + 1;
			num = num2;
		}
		while (num1 < 16);
	}

	public static void DrawFlatCapStart()
	{
		int num = 0;
		int num1 = 30;
		do
		{
			Gizmos.DrawLine(VisGizmosUtility.circleVerts[num], VisGizmosUtility.circleVerts[num1]);
			int num2 = num1;
			num1 = num2 - 1;
			num = num2;
		}
		while (num1 >= 16);
	}

	public static void DrawFlatCapsule(float lengthOverRadius)
	{
		VisGizmosUtility.DrawFlatCapStart();
		Gizmos.DrawLine(VisGizmosUtility.circleVerts[16], VisGizmosUtility.circleVerts[16] + new Vector3(lengthOverRadius, 0f));
		VisGizmosUtility.PushMatrix();
		Gizmos.matrix = Gizmos.matrix * Matrix4x4.TRS(new Vector3(lengthOverRadius, 0f, 0f), Quaternion.identity, Vector3.one);
		VisGizmosUtility.DrawFlatCapEnd();
		Gizmos.DrawLine(VisGizmosUtility.circleVerts[0], VisGizmosUtility.circleVerts[0] - new Vector3(lengthOverRadius, 0f));
		VisGizmosUtility.PopMatrix();
	}

	public static void DrawFlatCircle()
	{
		int num = 30;
		int num1 = 0;
		do
		{
			Gizmos.DrawLine(VisGizmosUtility.circleVerts[num], VisGizmosUtility.circleVerts[num1]);
			int num2 = num1;
			num1 = num2 + 1;
			num = num2;
		}
		while (num1 < (int)VisGizmosUtility.circleVerts.Length);
	}

	public static void DrawFlatCircle(float radius)
	{
		VisGizmosUtility.PushMatrixMul(Matrix4x4.Scale(Vector3.one * radius));
		VisGizmosUtility.DrawFlatCircle();
		VisGizmosUtility.PopMatrix();
	}

	public static void DrawSphere(Vector3 center, float radius, Quaternion rotation)
	{
		VisGizmosUtility.PushMatrix();
		Matrix4x4 matrix4x4 = Matrix4x4.TRS(center, rotation, new Vector3(radius, radius, radius)) * Gizmos.matrix;
		Gizmos.matrix = matrix4x4;
		VisGizmosUtility.DrawFlatCircle();
		Gizmos.matrix = VisGizmosUtility.ninetyX * matrix4x4;
		VisGizmosUtility.DrawFlatCircle();
		Gizmos.matrix = VisGizmosUtility.ninetyY * matrix4x4;
		VisGizmosUtility.DrawFlatCircle();
		VisGizmosUtility.PopMatrix();
	}

	public static void DrawSphere(Vector3 center, float radius, Vector3 forward)
	{
		VisGizmosUtility.DrawSphere(center, radius, Quaternion.LookRotation(forward));
	}

	public static void DrawSphere(Vector3 center, float radius)
	{
		VisGizmosUtility.DrawSphere(center, radius, Quaternion.identity);
	}

	private static Quaternion MagicFlat(Vector3 a, Vector3 b)
	{
		Vector3 vector3;
		Vector3 vector31;
		VisGizmosUtility.MagicForward(a, b, out vector31, out vector3);
		return Quaternion.LookRotation(vector3, vector31);
	}

	private static void MagicForward(Vector3 a, Vector3 b, out Vector3 up, out Vector3 forward)
	{
		Vector3 vector3 = a - b;
		vector3.Normalize();
		if (vector3.y * vector3.y <= 0.8f)
		{
			forward = Vector3.Cross(vector3, Vector3.up);
			up = Vector3.Cross(vector3, forward);
		}
		else
		{
			up = Vector3.Cross(vector3, Vector3.forward);
			forward = Vector3.Cross(vector3, up);
		}
		up.Normalize();
		forward.Normalize();
	}

	public static void PopMatrix()
	{
		int num = VisGizmosUtility.stackPos - 1;
		VisGizmosUtility.stackPos = num;
		Gizmos.matrix = VisGizmosUtility.matStack[num];
	}

	public static void PopMatrix(out Matrix4x4 mat)
	{
		int num = VisGizmosUtility.stackPos - 1;
		VisGizmosUtility.stackPos = num;
		mat = VisGizmosUtility.matStack[num];
		Gizmos.matrix = mat;
	}

	public static void PushMatrix()
	{
		if (VisGizmosUtility.stackPos == (int)VisGizmosUtility.matStack.Length)
		{
			Array.Resize<Matrix4x4>(ref VisGizmosUtility.matStack, VisGizmosUtility.stackPos + 8);
		}
		Matrix4x4[] matrix4x4Array = VisGizmosUtility.matStack;
		int num = VisGizmosUtility.stackPos;
		VisGizmosUtility.stackPos = num + 1;
		matrix4x4Array[num] = Gizmos.matrix;
	}

	public static void PushMatrix(Matrix4x4 mat)
	{
		VisGizmosUtility.PushMatrix();
		Gizmos.matrix = mat;
	}

	public static void PushMatrixMul(Matrix4x4 mat)
	{
		VisGizmosUtility.PushMatrix();
		Gizmos.matrix = mat * VisGizmosUtility.matStack[VisGizmosUtility.stackPos - 1];
	}

	public static void PushMatrixMul(Matrix4x4 mat, out Matrix4x4 res)
	{
		VisGizmosUtility.PushMatrix();
		Matrix4x4 matrix4x4 = mat * VisGizmosUtility.matStack[VisGizmosUtility.stackPos - 1];
		Matrix4x4 matrix4x41 = matrix4x4;
		res = matrix4x4;
		Gizmos.matrix = matrix4x41;
	}

	public static void ResetMatrixStack()
	{
		VisGizmosUtility.stackPos = 0;
	}
}