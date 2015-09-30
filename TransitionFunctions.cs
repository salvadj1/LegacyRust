using Facepunch.Precision;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class TransitionFunctions
{
	public static double Acos(double v)
	{
		return Math.Acos(v);
	}

	public static float Acos(float v)
	{
		return Mathf.Acos(v);
	}

	public static float AngleDegrees(Vector3 a, Vector3 b)
	{
		float single = TransitionFunctions.DotNormal(a, b);
		if (single >= 1f)
		{
			return 0f;
		}
		if (single <= -1f)
		{
			return 180f;
		}
		if (single == 0f)
		{
			return 90f;
		}
		return TransitionFunctions.RadiansToDegrees(TransitionFunctions.Acos(single));
	}

	public static double AngleDegrees(Vector3G a, Vector3G b)
	{
		double num = TransitionFunctions.DotNormal(a, b);
		if (num >= 1)
		{
			return 0;
		}
		if (num <= -1)
		{
			return 180;
		}
		if (num == 0)
		{
			return 90;
		}
		return TransitionFunctions.RadiansToDegrees(TransitionFunctions.Acos(num));
	}

	public static float AngleRadians(Vector3 a, Vector3 b)
	{
		float single = TransitionFunctions.DotNormal(a, b);
		if (single >= 1f)
		{
			return 0f;
		}
		if (single <= -1f)
		{
			return 3.14159274f;
		}
		if (single == 0f)
		{
			return 1.57079637f;
		}
		return TransitionFunctions.Acos(single);
	}

	public static double AngleRadians(Vector3G a, Vector3G b)
	{
		double num = TransitionFunctions.DotNormal(a, b);
		if (num >= 1)
		{
			return 0;
		}
		if (num <= -1)
		{
			return 3.14159265358979;
		}
		if (num == 0)
		{
			return 1.5707963267949;
		}
		return TransitionFunctions.Acos(num);
	}

	public static double Atan2(double y, double x)
	{
		return Math.Atan2(y, x);
	}

	public static float Atan2(float y, float x)
	{
		return Mathf.Atan2(y, x);
	}

	public static Color Ceil(float t, Color a, Color b)
	{
		return (t <= 0f ? a : b);
	}

	public static Matrix4x4 Ceil(float t, Matrix4x4 a, Matrix4x4 b)
	{
		return (t <= 0f ? a : b);
	}

	public static Matrix4x4G Ceil(double t, Matrix4x4G a, Matrix4x4G b)
	{
		return (t <= 0 ? a : b);
	}

	public static Quaternion Ceil(float t, Quaternion a, Quaternion b)
	{
		return (t <= 0f ? a : b);
	}

	public static QuaternionG Ceil(double t, QuaternionG a, QuaternionG b)
	{
		return (t <= 0 ? a : b);
	}

	public static Vector2 Ceil(float t, Vector2 a, Vector2 b)
	{
		return (t <= 0f ? a : b);
	}

	public static Vector3 Ceil(float t, Vector3 a, Vector3 b)
	{
		return (t <= 0f ? a : b);
	}

	public static Vector3G Ceil(double t, Vector3G a, Vector3G b)
	{
		return (t <= 0 ? a : b);
	}

	public static Vector4 Ceil(float t, Vector4 a, Vector4 b)
	{
		return (t <= 0f ? a : b);
	}

	public static double Ceil(double t, double a, double b)
	{
		return (t <= 0 ? a : b);
	}

	public static double Ceil(float t, double a, double b)
	{
		return (t <= 0f ? a : b);
	}

	public static float Ceil(float t, float a, float b)
	{
		return (t <= 0f ? a : b);
	}

	public static double Cos(double v)
	{
		return Math.Cos(v);
	}

	public static float Cos(float v)
	{
		return Mathf.Cos(v);
	}

	public static Vector3 Cross(Vector3 a, Vector3 b)
	{
		Vector3 vector3 = new Vector3();
		vector3.x = a.y * b.z - a.z * b.y;
		vector3.y = a.z * b.x - a.x * b.z;
		vector3.z = a.x * b.y - a.y * b.x;
		return vector3;
	}

	public static Vector3G Cross(Vector3G a, Vector3G b)
	{
		Vector3G vector3G = new Vector3G();
		vector3G.x = a.y * b.z - a.z * b.y;
		vector3G.y = a.z * b.x - a.x * b.z;
		vector3G.z = a.x * b.y - a.y * b.x;
		return vector3G;
	}

	public static float CrossDot(Vector3 a, Vector3 b, Vector3 dotB)
	{
		return (a.y * b.z - a.z * b.y) * dotB.x + (a.z * b.x - a.x * b.z) * dotB.y + (a.x * b.y - a.y * b.x) * dotB.z;
	}

	public static double CrossDot(Vector3G a, Vector3G b, Vector3G dotB)
	{
		return (a.y * b.z - a.z * b.y) * dotB.x + (a.z * b.x - a.x * b.z) * dotB.y + (a.x * b.y - a.y * b.x) * dotB.z;
	}

	public static double DegreesToRadians(double rads)
	{
		return 0.0174532925199433 * rads;
	}

	public static float DegreesToRadians(float rads)
	{
		return 0.0174532924f * rads;
	}

	private static Vector3 DIR_X(Matrix4x4 a)
	{
		return TransitionFunctions.GET_X0(a);
	}

	private static void DIR_X(ref Matrix4x4 a, Vector3 v)
	{
		TransitionFunctions.SET_X0(ref a, v);
	}

	private static Vector3G DIR_X(Matrix4x4G a)
	{
		return TransitionFunctions.GET_X0(a);
	}

	private static void DIR_X(ref Matrix4x4G a, Vector3G v)
	{
		TransitionFunctions.SET_X0(ref a, v);
	}

	private static Vector3 DIR_Y(Matrix4x4 a)
	{
		return TransitionFunctions.GET_X1(a);
	}

	private static void DIR_Y(ref Matrix4x4 a, Vector3 v)
	{
		TransitionFunctions.SET_X1(ref a, v);
	}

	private static Vector3G DIR_Y(Matrix4x4G a)
	{
		return TransitionFunctions.GET_X1(a);
	}

	private static void DIR_Y(ref Matrix4x4G a, Vector3G v)
	{
		TransitionFunctions.SET_X1(ref a, v);
	}

	private static Vector3 DIR_Z(Matrix4x4 a)
	{
		return TransitionFunctions.GET_X2(a);
	}

	private static void DIR_Z(ref Matrix4x4 a, Vector3 v)
	{
		TransitionFunctions.SET_X2(ref a, v);
	}

	private static Vector3G DIR_Z(Matrix4x4G a)
	{
		return TransitionFunctions.GET_X2(a);
	}

	private static void DIR_Z(ref Matrix4x4G a, Vector3G v)
	{
		TransitionFunctions.SET_X2(ref a, v);
	}

	public static double Distance(double a, double b)
	{
		return (b <= a ? a - b : b - a);
	}

	public static float Distance(float a, float b)
	{
		return (b <= a ? a - b : b - a);
	}

	public static float Dot(Vector3 a, Vector3 b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z;
	}

	public static double Dot(Vector3G a, Vector3G b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z;
	}

	public static float DotNormal(Vector3 a, Vector3 b)
	{
		return TransitionFunctions.Dot(TransitionFunctions.Normalize(a), TransitionFunctions.Normalize(b));
	}

	public static double DotNormal(Vector3G a, Vector3G b)
	{
		return TransitionFunctions.Dot(TransitionFunctions.Normalize(a), TransitionFunctions.Normalize(b));
	}

	public static Color Evaluate(this TransitionFunction f, float t, Color a, Color b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static Color Evaluate(this TransitionFunction<Color> v, float t)
	{
		return v.f.Evaluate(t, (Color)v.a, (Color)v.b);
	}

	public static Matrix4x4 Evaluate(this TransitionFunction f, float t, Matrix4x4 a, Matrix4x4 b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static Matrix4x4 Evaluate(this TransitionFunction<Matrix4x4> v, float t)
	{
		return v.f.Evaluate(t, (Matrix4x4)v.a, (Matrix4x4)v.b);
	}

	public static Matrix4x4G Evaluate(this TransitionFunction f, double t, Matrix4x4G a, Matrix4x4G b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static Matrix4x4G Evaluate(this TransitionFunction<Matrix4x4G> v, double t)
	{
		return v.f.Evaluate(t, (Matrix4x4G)v.a, (Matrix4x4G)v.b);
	}

	public static Quaternion Evaluate(this TransitionFunction f, float t, Quaternion a, Quaternion b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static Quaternion Evaluate(this TransitionFunction<Quaternion> v, float t)
	{
		return v.f.Evaluate(t, (Quaternion)v.a, (Quaternion)v.b);
	}

	public static QuaternionG Evaluate(this TransitionFunction f, double t, QuaternionG a, QuaternionG b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static QuaternionG Evaluate(this TransitionFunction<QuaternionG> v, double t)
	{
		return v.f.Evaluate(t, (QuaternionG)v.a, (QuaternionG)v.b);
	}

	public static Vector2 Evaluate(this TransitionFunction f, float t, Vector2 a, Vector2 b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static Vector2 Evaluate(this TransitionFunction<Vector2> v, float t)
	{
		return v.f.Evaluate(t, (Vector2)v.a, (Vector2)v.b);
	}

	public static Vector3 Evaluate(this TransitionFunction f, float t, Vector3 a, Vector3 b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static Vector3 Evaluate(this TransitionFunction<Vector3> v, float t)
	{
		return v.f.Evaluate(t, (Vector3)v.a, (Vector3)v.b);
	}

	public static Vector3G Evaluate(this TransitionFunction f, double t, Vector3G a, Vector3G b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static Vector3G Evaluate(this TransitionFunction<Vector3G> v, double t)
	{
		return v.f.Evaluate(t, (Vector3G)v.a, (Vector3G)v.b);
	}

	public static Vector4 Evaluate(this TransitionFunction f, float t, Vector4 a, Vector4 b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static Vector4 Evaluate(this TransitionFunction<Vector4> v, float t)
	{
		return v.f.Evaluate(t, (Vector4)v.a, (Vector4)v.b);
	}

	public static double Evaluate(this TransitionFunction f, double t)
	{
		return f.Evaluate(t, 0, 1);
	}

	public static double Evaluate(this TransitionFunction f, double t, double a, double b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static double Evaluate(this TransitionFunction<double> v, double t)
	{
		return v.f.Evaluate(t, (double)v.a, (double)v.b);
	}

	public static double Evaluate(this TransitionFunction f, float t, double a, double b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static double Evaluate(this TransitionFunction<double> v, float t)
	{
		return v.f.Evaluate(t, (double)v.a, (double)v.b);
	}

	public static float Evaluate(this TransitionFunction f, float t)
	{
		return f.Evaluate(t, 0f, 1f);
	}

	public static float Evaluate(this TransitionFunction f, float t, float a, float b)
	{
		switch (f)
		{
			case TransitionFunction.Linear:
			{
				return TransitionFunctions.Linear(t, a, b);
			}
			case TransitionFunction.Round:
			{
				return TransitionFunctions.Round(t, a, b);
			}
			case TransitionFunction.Floor:
			{
				return TransitionFunctions.Floor(t, a, b);
			}
			case TransitionFunction.Ceil:
			{
				return TransitionFunctions.Ceil(t, a, b);
			}
			case TransitionFunction.Spline:
			{
				return TransitionFunctions.Spline(t, a, b);
			}
		}
		throw new ArgumentOutOfRangeException("v", "Attempted use of unrecognized TransitionFunction enum value");
	}

	public static float Evaluate(this TransitionFunction<float> v, float t)
	{
		return v.f.Evaluate(t, (float)v.a, (float)v.b);
	}

	public static Color Floor(float t, Color a, Color b)
	{
		return (t >= 1f ? b : a);
	}

	public static Matrix4x4 Floor(float t, Matrix4x4 a, Matrix4x4 b)
	{
		return (t >= 1f ? b : a);
	}

	public static Matrix4x4G Floor(double t, Matrix4x4G a, Matrix4x4G b)
	{
		return (t >= 1 ? b : a);
	}

	public static Quaternion Floor(float t, Quaternion a, Quaternion b)
	{
		return (t >= 1f ? b : a);
	}

	public static QuaternionG Floor(double t, QuaternionG a, QuaternionG b)
	{
		return (t >= 1 ? b : a);
	}

	public static Vector2 Floor(float t, Vector2 a, Vector2 b)
	{
		return (t >= 1f ? b : a);
	}

	public static Vector3 Floor(float t, Vector3 a, Vector3 b)
	{
		return (t >= 1f ? b : a);
	}

	public static Vector3G Floor(double t, Vector3G a, Vector3G b)
	{
		return (t >= 1 ? b : a);
	}

	public static Vector4 Floor(float t, Vector4 a, Vector4 b)
	{
		return (t >= 1f ? b : a);
	}

	public static double Floor(double t, double a, double b)
	{
		return (t >= 1 ? b : a);
	}

	public static double Floor(float t, double a, double b)
	{
		return (t >= 1f ? b : a);
	}

	public static float Floor(float t, float a, float b)
	{
		return (t >= 1f ? b : a);
	}

	private static Vector3 GET_0X(Matrix4x4 a)
	{
		return TransitionFunctions.Vect(a.m00, a.m01, a.m02);
	}

	private static Vector3G GET_0X(Matrix4x4G a)
	{
		return TransitionFunctions.VECT3F(a.m00, a.m01, a.m02);
	}

	private static Vector3 GET_1X(Matrix4x4 a)
	{
		return TransitionFunctions.Vect(a.m10, a.m11, a.m12);
	}

	private static Vector3G GET_1X(Matrix4x4G a)
	{
		return TransitionFunctions.VECT3F(a.m10, a.m11, a.m12);
	}

	private static Vector3 GET_2X(Matrix4x4 a)
	{
		return TransitionFunctions.Vect(a.m20, a.m21, a.m22);
	}

	private static Vector3G GET_2X(Matrix4x4G a)
	{
		return TransitionFunctions.VECT3F(a.m20, a.m21, a.m22);
	}

	private static Vector3 GET_3X(Matrix4x4 a)
	{
		return TransitionFunctions.Vect(a.m30, a.m31, a.m32);
	}

	private static Vector3G GET_3X(Matrix4x4G a)
	{
		return TransitionFunctions.VECT3F(a.m30, a.m31, a.m32);
	}

	private static Vector3 GET_X0(Matrix4x4 a)
	{
		return TransitionFunctions.Vect(a.m00, a.m10, a.m20);
	}

	private static Vector3G GET_X0(Matrix4x4G a)
	{
		return TransitionFunctions.VECT3F(a.m00, a.m10, a.m20);
	}

	private static Vector3 GET_X1(Matrix4x4 a)
	{
		return TransitionFunctions.Vect(a.m01, a.m11, a.m21);
	}

	private static Vector3G GET_X1(Matrix4x4G a)
	{
		return TransitionFunctions.VECT3F(a.m01, a.m11, a.m21);
	}

	private static Vector3 GET_X2(Matrix4x4 a)
	{
		return TransitionFunctions.Vect(a.m02, a.m12, a.m22);
	}

	private static Vector3G GET_X2(Matrix4x4G a)
	{
		return TransitionFunctions.VECT3F(a.m02, a.m12, a.m22);
	}

	private static Vector3 GET_X3(Matrix4x4 a)
	{
		return TransitionFunctions.Vect(a.m03, a.m13, a.m23);
	}

	private static Vector3G GET_X3(Matrix4x4G a)
	{
		return TransitionFunctions.VECT3F(a.m03, a.m13, a.m23);
	}

	public static Matrix4x4 Inverse(Matrix4x4 v)
	{
		return Matrix4x4.Inverse(v);
	}

	public static Matrix4x4G Inverse(Matrix4x4G v)
	{
		Matrix4x4G matrix4x4G;
		Matrix4x4G.Inverse(ref v, out matrix4x4G);
		return matrix4x4G;
	}

	public static float Length(Vector3 a)
	{
		return TransitionFunctions.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
	}

	public static double Length(Vector3G a)
	{
		return TransitionFunctions.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
	}

	public static Color Linear(float t, Color a, Color b)
	{
		return TransitionFunctions.Sum(TransitionFunctions.Mul(a, 1f - t), TransitionFunctions.Mul(b, t));
	}

	public static Matrix4x4 Linear(float t, Matrix4x4 a, Matrix4x4 b)
	{
		return TransitionFunctions.Sum(TransitionFunctions.Mul(a, 1f - t), TransitionFunctions.Mul(b, t));
	}

	public static Matrix4x4G Linear(double t, Matrix4x4G a, Matrix4x4G b)
	{
		return TransitionFunctions.Sum(TransitionFunctions.Mul(a, 1 - t), TransitionFunctions.Mul(b, t));
	}

	public static Quaternion Linear(float t, Quaternion a, Quaternion b)
	{
		return TransitionFunctions.Slerp(t, a, b);
	}

	public static QuaternionG Linear(double t, QuaternionG a, QuaternionG b)
	{
		return TransitionFunctions.Slerp(t, a, b);
	}

	public static Vector2 Linear(float t, Vector2 a, Vector2 b)
	{
		return TransitionFunctions.Sum(TransitionFunctions.Mul(a, 1f - t), TransitionFunctions.Mul(b, t));
	}

	public static Vector3 Linear(float t, Vector3 a, Vector3 b)
	{
		return TransitionFunctions.Sum(TransitionFunctions.Mul(a, 1f - t), TransitionFunctions.Mul(b, t));
	}

	public static Vector3G Linear(double t, Vector3G a, Vector3G b)
	{
		return TransitionFunctions.Sum(TransitionFunctions.Mul(a, 1 - t), TransitionFunctions.Mul(b, t));
	}

	public static Vector4 Linear(float t, Vector4 a, Vector4 b)
	{
		return TransitionFunctions.Sum(TransitionFunctions.Mul(a, 1f - t), TransitionFunctions.Mul(b, t));
	}

	public static double Linear(double t, double a, double b)
	{
		return TransitionFunctions.Sum(TransitionFunctions.Mul(a, 1 - t), TransitionFunctions.Mul(b, t));
	}

	public static double Linear(float t, double a, double b)
	{
		return TransitionFunctions.Sum(TransitionFunctions.Mul(a, (double)(1f - t)), TransitionFunctions.Mul(b, (double)t));
	}

	public static float Linear(float t, float a, float b)
	{
		return TransitionFunctions.Sum(TransitionFunctions.Mul(a, 1f - t), TransitionFunctions.Mul(b, t));
	}

	private static Vector3 LLERP(float t, Vector3 a, Vector3 b)
	{
		return TransitionFunctions.Linear(t, a, b);
	}

	private static Vector3G LLERP(double t, Vector3G a, Vector3G b)
	{
		return TransitionFunctions.Linear(t, a, b);
	}

	public static Quaternion LookRotation(Vector3 forward, Vector3 up)
	{
		return Quaternion.LookRotation(forward, up);
	}

	public static QuaternionG LookRotation(Vector3G forward, Vector3G up)
	{
		QuaternionG quaternionG;
		QuaternionG.LookRotation(ref forward, ref up, out quaternionG);
		return quaternionG;
	}

	public static double Max(double a, double b)
	{
		return (b <= a ? a : b);
	}

	public static float Max(float a, float b)
	{
		return (b <= a ? a : b);
	}

	public static double Min(double a, double b)
	{
		return (b >= a ? a : b);
	}

	public static float Min(float a, float b)
	{
		return (b >= a ? a : b);
	}

	public static Color Mul(Color a, float b)
	{
		Color color = new Color();
		color.r = a.r * b;
		color.g = a.g * b;
		color.b = a.b * b;
		color.a = a.a * b;
		return color;
	}

	public static Matrix4x4 Mul(Matrix4x4 a, float b)
	{
		Matrix4x4 matrix4x4 = new Matrix4x4();
		matrix4x4.m00 = a.m00 * b;
		matrix4x4.m10 = a.m10 * b;
		matrix4x4.m20 = a.m20 * b;
		matrix4x4.m30 = a.m30 * b;
		matrix4x4.m01 = a.m01 * b;
		matrix4x4.m11 = a.m11 * b;
		matrix4x4.m21 = a.m21 * b;
		matrix4x4.m31 = a.m31 * b;
		matrix4x4.m02 = a.m02 * b;
		matrix4x4.m12 = a.m12 * b;
		matrix4x4.m22 = a.m22 * b;
		matrix4x4.m32 = a.m32 * b;
		matrix4x4.m03 = a.m03 * b;
		matrix4x4.m13 = a.m13 * b;
		matrix4x4.m23 = a.m23 * b;
		matrix4x4.m33 = a.m33 * b;
		return matrix4x4;
	}

	public static Matrix4x4G Mul(Matrix4x4G a, double b)
	{
		Matrix4x4G matrix4x4G = new Matrix4x4G();
		matrix4x4G.m00 = a.m00 * b;
		matrix4x4G.m10 = a.m10 * b;
		matrix4x4G.m20 = a.m20 * b;
		matrix4x4G.m30 = a.m30 * b;
		matrix4x4G.m01 = a.m01 * b;
		matrix4x4G.m11 = a.m11 * b;
		matrix4x4G.m21 = a.m21 * b;
		matrix4x4G.m31 = a.m31 * b;
		matrix4x4G.m02 = a.m02 * b;
		matrix4x4G.m12 = a.m12 * b;
		matrix4x4G.m22 = a.m22 * b;
		matrix4x4G.m32 = a.m32 * b;
		matrix4x4G.m03 = a.m03 * b;
		matrix4x4G.m13 = a.m13 * b;
		matrix4x4G.m23 = a.m23 * b;
		matrix4x4G.m33 = a.m33 * b;
		return matrix4x4G;
	}

	public static Quaternion Mul(Quaternion a, float b)
	{
		Quaternion quaternion = new Quaternion();
		quaternion.x = a.x * b;
		quaternion.y = a.y * b;
		quaternion.z = a.z * b;
		quaternion.w = a.w * b;
		return quaternion;
	}

	public static QuaternionG Mul(QuaternionG a, double b)
	{
		QuaternionG quaternionG = new QuaternionG();
		quaternionG.x = a.x * b;
		quaternionG.y = a.y * b;
		quaternionG.z = a.z * b;
		quaternionG.w = a.w * b;
		return quaternionG;
	}

	public static Vector2 Mul(Vector2 a, float b)
	{
		Vector2 vector2 = new Vector2();
		vector2.x = a.x * b;
		vector2.y = a.y * b;
		return vector2;
	}

	public static Vector3 Mul(Vector3 a, float b)
	{
		Vector3 vector3 = new Vector3();
		vector3.x = a.x * b;
		vector3.y = a.y * b;
		vector3.z = a.z * b;
		return vector3;
	}

	public static Vector3G Mul(Vector3G a, double b)
	{
		Vector3G vector3G = new Vector3G();
		vector3G.x = a.x * b;
		vector3G.y = a.y * b;
		vector3G.z = a.z * b;
		return vector3G;
	}

	public static Vector4 Mul(Vector4 a, float b)
	{
		Vector4 vector4 = new Vector4();
		vector4.x = a.x * b;
		vector4.y = a.y * b;
		vector4.z = a.z * b;
		vector4.w = a.w * b;
		return vector4;
	}

	public static double Mul(double a, double b)
	{
		return a * b;
	}

	public static float Mul(float a, float b)
	{
		return a * b;
	}

	public static Vector3 Normalize(Vector3 v)
	{
		Vector3 vector3 = new Vector3();
		float single = v.x * v.x + v.y * v.y + v.z * v.z;
		if (single == 0f || single == 1f)
		{
			return v;
		}
		single = TransitionFunctions.Sqrt(single);
		vector3.x = v.x / single;
		vector3.y = v.y / single;
		vector3.z = v.z / single;
		return vector3;
	}

	public static Vector3G Normalize(Vector3G v)
	{
		Vector3G vector3G = new Vector3G();
		double num = v.x * v.x + v.y * v.y + v.z * v.z;
		if (num == 0 || num == 1)
		{
			return v;
		}
		num = TransitionFunctions.Sqrt(num);
		vector3G.x = v.x / num;
		vector3G.y = v.y / num;
		vector3G.z = v.z / num;
		return vector3G;
	}

	public static double RadiansToDegrees(double degs)
	{
		return 57.2957795130823 * degs;
	}

	public static float RadiansToDegrees(float degs)
	{
		return 57.29578f * degs;
	}

	public static Vector3 Rotate(Quaternion rotation, Vector3 vector)
	{
		return rotation * vector;
	}

	public static Vector3G Rotate(QuaternionG rotation, Vector3G vector)
	{
		Vector3G vector3G;
		QuaternionG.Mult(ref rotation, ref vector, out vector3G);
		return vector3G;
	}

	public static Color Round(float t, Color a, Color b)
	{
		return (t >= 0.5f ? b : a);
	}

	public static Matrix4x4 Round(float t, Matrix4x4 a, Matrix4x4 b)
	{
		return (t >= 0.5f ? b : a);
	}

	public static Matrix4x4G Round(double t, Matrix4x4G a, Matrix4x4G b)
	{
		return (t >= 0.5 ? b : a);
	}

	public static Quaternion Round(float t, Quaternion a, Quaternion b)
	{
		return (t >= 0.5f ? b : a);
	}

	public static QuaternionG Round(double t, QuaternionG a, QuaternionG b)
	{
		return (t >= 0.5 ? b : a);
	}

	public static Vector2 Round(float t, Vector2 a, Vector2 b)
	{
		return (t >= 0.5f ? b : a);
	}

	public static Vector3 Round(float t, Vector3 a, Vector3 b)
	{
		return (t >= 0.5f ? b : a);
	}

	public static Vector3G Round(double t, Vector3G a, Vector3G b)
	{
		return (t >= 0.5 ? b : a);
	}

	public static Vector4 Round(float t, Vector4 a, Vector4 b)
	{
		return (t >= 0.5f ? b : a);
	}

	public static double Round(double t, double a, double b)
	{
		return (t >= 0.5 ? b : a);
	}

	public static double Round(float t, double a, double b)
	{
		return (t >= 0.5f ? b : a);
	}

	public static float Round(float t, float a, float b)
	{
		return (t >= 0.5f ? b : a);
	}

	private static Vector3 SCALE(Matrix4x4 a)
	{
		return TransitionFunctions.GET_3X(a);
	}

	private static void SCALE(ref Matrix4x4 a, Vector3 v)
	{
		TransitionFunctions.SET_3X(ref a, v);
	}

	private static Vector3G SCALE(Matrix4x4G a)
	{
		return TransitionFunctions.GET_3X(a);
	}

	private static void SCALE(ref Matrix4x4G a, Vector3G v)
	{
		TransitionFunctions.SET_3X(ref a, v);
	}

	public static Vector3 Scale3(float xyz)
	{
		Vector3 vector3 = new Vector3();
		float single = xyz;
		float single1 = single;
		vector3.z = single;
		float single2 = single1;
		single1 = single2;
		vector3.y = single2;
		vector3.x = single1;
		return vector3;
	}

	public static Vector3G Scale3(double xyz)
	{
		Vector3G vector3G = new Vector3G();
		double num = xyz;
		double num1 = num;
		vector3G.z = num;
		double num2 = num1;
		num1 = num2;
		vector3G.y = num2;
		vector3G.x = num1;
		return vector3G;
	}

	private static void SET_0X(ref Matrix4x4 m, Vector3 v)
	{
		m.m00 = v.x;
		m.m01 = v.y;
		m.m02 = v.z;
	}

	private static void SET_0X(ref Matrix4x4G m, Vector3G v)
	{
		m.m00 = v.x;
		m.m01 = v.y;
		m.m02 = v.z;
	}

	private static void SET_1X(ref Matrix4x4 m, Vector3 v)
	{
		m.m10 = v.x;
		m.m11 = v.y;
		m.m12 = v.z;
	}

	private static void SET_1X(ref Matrix4x4G m, Vector3G v)
	{
		m.m10 = v.x;
		m.m11 = v.y;
		m.m12 = v.z;
	}

	private static void SET_2X(ref Matrix4x4 m, Vector3 v)
	{
		m.m20 = v.x;
		m.m21 = v.y;
		m.m22 = v.z;
	}

	private static void SET_2X(ref Matrix4x4G m, Vector3G v)
	{
		m.m20 = v.x;
		m.m21 = v.y;
		m.m22 = v.z;
	}

	private static void SET_3X(ref Matrix4x4 m, Vector3 v)
	{
		m.m30 = v.x;
		m.m31 = v.y;
		m.m32 = v.z;
	}

	private static void SET_3X(ref Matrix4x4G m, Vector3G v)
	{
		m.m30 = v.x;
		m.m31 = v.y;
		m.m32 = v.z;
	}

	private static void SET_X0(ref Matrix4x4 m, Vector3 v)
	{
		m.m00 = v.x;
		m.m10 = v.y;
		m.m20 = v.z;
	}

	private static void SET_X0(ref Matrix4x4G m, Vector3G v)
	{
		m.m00 = v.x;
		m.m10 = v.y;
		m.m20 = v.z;
	}

	private static void SET_X1(ref Matrix4x4 m, Vector3 v)
	{
		m.m01 = v.x;
		m.m11 = v.y;
		m.m21 = v.z;
	}

	private static void SET_X1(ref Matrix4x4G m, Vector3G v)
	{
		m.m01 = v.x;
		m.m11 = v.y;
		m.m21 = v.z;
	}

	private static void SET_X2(ref Matrix4x4 m, Vector3 v)
	{
		m.m02 = v.x;
		m.m12 = v.y;
		m.m22 = v.z;
	}

	private static void SET_X2(ref Matrix4x4G m, Vector3G v)
	{
		m.m02 = v.x;
		m.m12 = v.y;
		m.m22 = v.z;
	}

	private static void SET_X3(ref Matrix4x4 m, Vector3 v)
	{
		m.m03 = v.x;
		m.m13 = v.y;
		m.m23 = v.z;
	}

	private static void SET_X3(ref Matrix4x4G m, Vector3G v)
	{
		m.m03 = v.x;
		m.m13 = v.y;
		m.m23 = v.z;
	}

	private static double SimpleSpline(double v01)
	{
		return 3 * (v01 * v01) - 2 * (v01 * v01) * v01;
	}

	private static float SimpleSpline(float v01)
	{
		return 3f * (v01 * v01) - 2f * (v01 * v01) * v01;
	}

	public static double Sin(double v)
	{
		return Math.Sin(v);
	}

	public static float Sin(float v)
	{
		return Mathf.Sin(v);
	}

	public static Matrix4x4 Slerp(float t, Matrix4x4 a, Matrix4x4 b)
	{
		Matrix4x4 matrix4x4 = Matrix4x4.identity;
		Vector3 vector3 = TransitionFunctions.Slerp(t, TransitionFunctions.DIR_X(a), TransitionFunctions.DIR_X(b));
		Vector3 vector31 = TransitionFunctions.Slerp(t, TransitionFunctions.DIR_Y(a), TransitionFunctions.DIR_Y(b));
		Vector3 vector32 = TransitionFunctions.Slerp(t, TransitionFunctions.DIR_Z(a), TransitionFunctions.DIR_Z(b));
		Quaternion quaternion = TransitionFunctions.LookRotation(vector32, vector31);
		vector31 = TransitionFunctions.Rotate(quaternion, TransitionFunctions.Y3(TransitionFunctions.Length(vector31)));
		vector3 = (TransitionFunctions.CrossDot(vector32, vector31, vector3) <= 0f ? TransitionFunctions.Rotate(quaternion, TransitionFunctions.X3(TransitionFunctions.Length(vector3))) : TransitionFunctions.Rotate(quaternion, TransitionFunctions.X3(-TransitionFunctions.Length(vector3))));
		TransitionFunctions.DIR_X(ref matrix4x4, vector3);
		TransitionFunctions.DIR_Y(ref matrix4x4, vector31);
		TransitionFunctions.DIR_Z(ref matrix4x4, vector32);
		TransitionFunctions.SCALE(ref matrix4x4, TransitionFunctions.Linear(t, TransitionFunctions.SCALE(a), TransitionFunctions.SCALE(b)));
		TransitionFunctions.TRANS(ref matrix4x4, TransitionFunctions.Linear(t, TransitionFunctions.TRANS(a), TransitionFunctions.TRANS(b)));
		matrix4x4.m33 = TransitionFunctions.Linear(t, a.m33, b.m33);
		return matrix4x4;
	}

	public static Matrix4x4G Slerp(double t, Matrix4x4G a, Matrix4x4G b)
	{
		Matrix4x4G matrix4x4G = Matrix4x4G.identity;
		Vector3G vector3G = TransitionFunctions.Slerp(t, TransitionFunctions.DIR_X(a), TransitionFunctions.DIR_X(b));
		Vector3G vector3G1 = TransitionFunctions.Slerp(t, TransitionFunctions.DIR_Y(a), TransitionFunctions.DIR_Y(b));
		Vector3G vector3G2 = TransitionFunctions.Slerp(t, TransitionFunctions.DIR_Z(a), TransitionFunctions.DIR_Z(b));
		QuaternionG quaternionG = TransitionFunctions.LookRotation(vector3G2, vector3G1);
		vector3G1 = TransitionFunctions.Rotate(quaternionG, TransitionFunctions.Y3(TransitionFunctions.Length(vector3G1)));
		vector3G = (TransitionFunctions.CrossDot(vector3G2, vector3G1, vector3G) <= 0 ? TransitionFunctions.Rotate(quaternionG, TransitionFunctions.X3(TransitionFunctions.Length(vector3G))) : TransitionFunctions.Rotate(quaternionG, TransitionFunctions.X3(-TransitionFunctions.Length(vector3G))));
		TransitionFunctions.DIR_X(ref matrix4x4G, vector3G);
		TransitionFunctions.DIR_Y(ref matrix4x4G, vector3G1);
		TransitionFunctions.DIR_Z(ref matrix4x4G, vector3G2);
		TransitionFunctions.SCALE(ref matrix4x4G, TransitionFunctions.Linear(t, TransitionFunctions.SCALE(a), TransitionFunctions.SCALE(b)));
		TransitionFunctions.TRANS(ref matrix4x4G, TransitionFunctions.Linear(t, TransitionFunctions.TRANS(a), TransitionFunctions.TRANS(b)));
		matrix4x4G.m33 = TransitionFunctions.Linear(t, a.m33, b.m33);
		return matrix4x4G;
	}

	public static Quaternion Slerp(float t, Quaternion a, Quaternion b)
	{
		Quaternion quaternion = new Quaternion();
		float single;
		float single1;
		float single2;
		if (t == 0f)
		{
			quaternion = a;
		}
		else if (t != 1f)
		{
			float single3 = (float)a.x * b.x + (float)a.y * b.y + (float)a.z * b.z + (float)a.w * b.w;
			if (single3 == 1f)
			{
				quaternion = a;
			}
			else if (single3 >= 0f)
			{
				single3 = TransitionFunctions.Acos(single3);
				single2 = TransitionFunctions.Sin(single3);
				if (single2 != 0f)
				{
					single1 = TransitionFunctions.Sin(single3 * t);
					single = TransitionFunctions.Sin(single3 * (1f - t));
					quaternion.x = (float)((a.x * single + b.x * single1) / single2);
					quaternion.y = (float)((a.y * single + b.y * single1) / single2);
					quaternion.z = (float)((a.z * single + b.z * single1) / single2);
					quaternion.w = (float)((a.w * single + b.w * single1) / single2);
				}
				else
				{
					single = 1f - t;
					quaternion.x = (float)(a.x * single + b.x * t);
					quaternion.y = (float)(a.y * single + b.y * t);
					quaternion.z = (float)(a.z * single + b.z * t);
					quaternion.w = (float)(a.w * single + b.w * t);
				}
			}
			else
			{
				single3 = TransitionFunctions.Acos(-single3);
				single2 = TransitionFunctions.Sin(single3);
				if (single2 != 0f)
				{
					single1 = TransitionFunctions.Sin(single3 * t);
					single = TransitionFunctions.Sin(single3 * (1f - t));
					quaternion.x = (float)((a.x * single - b.x * single1) / single2);
					quaternion.y = (float)((a.y * single - b.y * single1) / single2);
					quaternion.z = (float)((a.z * single - b.z * single1) / single2);
					quaternion.w = (float)((a.w * single - b.w * single1) / single2);
				}
				else
				{
					single = 1f - t;
					quaternion.x = (float)(a.x * single + b.x * t);
					quaternion.y = (float)(a.y * single + b.y * t);
					quaternion.z = (float)(a.z * single + b.z * t);
					quaternion.w = (float)(a.w * single + b.w * t);
				}
			}
		}
		else
		{
			quaternion = b;
		}
		return quaternion;
	}

	public static QuaternionG Slerp(double t, QuaternionG a, QuaternionG b)
	{
		QuaternionG quaternionG = new QuaternionG();
		double num;
		double num1;
		double num2;
		if (t == 0)
		{
			quaternionG = a;
		}
		else if (t != 1)
		{
			double num3 = (double)a.x * b.x + (double)a.y * b.y + (double)a.z * b.z + (double)a.w * b.w;
			if (num3 == 1)
			{
				quaternionG = a;
			}
			else if (num3 >= 0)
			{
				num3 = TransitionFunctions.Acos(num3);
				num2 = TransitionFunctions.Sin(num3);
				if (num2 != 0)
				{
					num1 = TransitionFunctions.Sin(num3 * t);
					num = TransitionFunctions.Sin(num3 * (1 - t));
					quaternionG.x = (double)((a.x * num + b.x * num1) / num2);
					quaternionG.y = (double)((a.y * num + b.y * num1) / num2);
					quaternionG.z = (double)((a.z * num + b.z * num1) / num2);
					quaternionG.w = (double)((a.w * num + b.w * num1) / num2);
				}
				else
				{
					num = 1 - t;
					quaternionG.x = (double)(a.x * num + b.x * t);
					quaternionG.y = (double)(a.y * num + b.y * t);
					quaternionG.z = (double)(a.z * num + b.z * t);
					quaternionG.w = (double)(a.w * num + b.w * t);
				}
			}
			else
			{
				num3 = TransitionFunctions.Acos(-num3);
				num2 = TransitionFunctions.Sin(num3);
				if (num2 != 0)
				{
					num1 = TransitionFunctions.Sin(num3 * t);
					num = TransitionFunctions.Sin(num3 * (1 - t));
					quaternionG.x = (double)((a.x * num - b.x * num1) / num2);
					quaternionG.y = (double)((a.y * num - b.y * num1) / num2);
					quaternionG.z = (double)((a.z * num - b.z * num1) / num2);
					quaternionG.w = (double)((a.w * num - b.w * num1) / num2);
				}
				else
				{
					num = 1 - t;
					quaternionG.x = (double)(a.x * num + b.x * t);
					quaternionG.y = (double)(a.y * num + b.y * t);
					quaternionG.z = (double)(a.z * num + b.z * t);
					quaternionG.w = (double)(a.w * num + b.w * t);
				}
			}
		}
		else
		{
			quaternionG = b;
		}
		return quaternionG;
	}

	public static Vector2 Slerp(float t, Vector2 a, Vector2 b)
	{
		float radians = TransitionFunctions.DegreesToRadians((float)Vector2.Angle(a, b));
		if (radians != 0f)
		{
			float single = TransitionFunctions.Sin((float)radians);
			float single1 = single;
			if (single != 0f)
			{
				float single2 = TransitionFunctions.Sin((1f - t) * radians) / single1;
				float single3 = TransitionFunctions.Sin(t * radians) / single1;
				return TransitionFunctions.Sum(TransitionFunctions.Mul(a, single2), TransitionFunctions.Mul(b, single3));
			}
		}
		return TransitionFunctions.Linear(t, a, b);
	}

	public static Vector3 Slerp(float t, Vector3 a, Vector3 b)
	{
		float single = TransitionFunctions.AngleRadians(a, b);
		if (single != 0f)
		{
			float single1 = TransitionFunctions.Sin((float)single);
			float single2 = single1;
			if (single1 != 0f)
			{
				float single3 = TransitionFunctions.Sin((1f - t) * single) / single2;
				float single4 = TransitionFunctions.Sin(t * single) / single2;
				return TransitionFunctions.Sum(TransitionFunctions.Mul(a, single3), TransitionFunctions.Mul(b, single4));
			}
		}
		return TransitionFunctions.Linear(t, a, b);
	}

	public static Vector3G Slerp(double t, Vector3G a, Vector3G b)
	{
		double num = TransitionFunctions.AngleRadians(a, b);
		if (num != 0)
		{
			double num1 = TransitionFunctions.Sin((double)num);
			double num2 = num1;
			if (num1 != 0)
			{
				double num3 = TransitionFunctions.Sin((1 - t) * num) / num2;
				double num4 = TransitionFunctions.Sin(t * num) / num2;
				return TransitionFunctions.Sum(TransitionFunctions.Mul(a, num3), TransitionFunctions.Mul(b, num4));
			}
		}
		return TransitionFunctions.Linear(t, a, b);
	}

	private static Vector3 SLERP(float t, Vector3 a, Vector3 b)
	{
		return TransitionFunctions.Slerp(t, a, b);
	}

	private static Vector3G SLERP(double t, Vector3G a, Vector3G b)
	{
		return TransitionFunctions.Slerp(t, a, b);
	}

	public static Matrix4x4 SlerpWorldToCamera(float t, Matrix4x4 a, Matrix4x4 b)
	{
		Matrix4x4 matrix4x4 = TransitionFunctions.Slerp(t, a.inverse, b.inverse);
		return matrix4x4.inverse;
	}

	public static Matrix4x4G SlerpWorldToCamera(double t, Matrix4x4G a, Matrix4x4G b)
	{
		return TransitionFunctions.Inverse(TransitionFunctions.Slerp(t, TransitionFunctions.Inverse(a), TransitionFunctions.Inverse(b)));
	}

	public static Color Spline(float t, Color a, Color b)
	{
		Color color;
		if (t > 0f)
		{
			color = (t < 1f ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			color = a;
		}
		return color;
	}

	public static Matrix4x4 Spline(float t, Matrix4x4 a, Matrix4x4 b)
	{
		Matrix4x4 matrix4x4;
		if (t > 0f)
		{
			matrix4x4 = (t < 1f ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			matrix4x4 = a;
		}
		return matrix4x4;
	}

	public static Matrix4x4G Spline(double t, Matrix4x4G a, Matrix4x4G b)
	{
		Matrix4x4G matrix4x4G;
		if (t > 0)
		{
			matrix4x4G = (t < 1 ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			matrix4x4G = a;
		}
		return matrix4x4G;
	}

	public static Quaternion Spline(float t, Quaternion a, Quaternion b)
	{
		Quaternion quaternion;
		if (t > 0f)
		{
			quaternion = (t < 1f ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			quaternion = a;
		}
		return quaternion;
	}

	public static QuaternionG Spline(double t, QuaternionG a, QuaternionG b)
	{
		QuaternionG quaternionG;
		if (t > 0)
		{
			quaternionG = (t < 1 ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			quaternionG = a;
		}
		return quaternionG;
	}

	public static Vector2 Spline(float t, Vector2 a, Vector2 b)
	{
		Vector2 vector2;
		if (t > 0f)
		{
			vector2 = (t < 1f ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			vector2 = a;
		}
		return vector2;
	}

	public static Vector3 Spline(float t, Vector3 a, Vector3 b)
	{
		Vector3 vector3;
		if (t > 0f)
		{
			vector3 = (t < 1f ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			vector3 = a;
		}
		return vector3;
	}

	public static Vector3G Spline(double t, Vector3G a, Vector3G b)
	{
		Vector3G vector3G;
		if (t > 0)
		{
			vector3G = (t < 1 ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			vector3G = a;
		}
		return vector3G;
	}

	public static Vector4 Spline(float t, Vector4 a, Vector4 b)
	{
		Vector4 vector4;
		if (t > 0f)
		{
			vector4 = (t < 1f ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			vector4 = a;
		}
		return vector4;
	}

	public static double Spline(double t, double a, double b)
	{
		double num;
		if (t > 0)
		{
			num = (t < 1 ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			num = a;
		}
		return num;
	}

	public static double Spline(float t, double a, double b)
	{
		double num;
		if (t > 0f)
		{
			num = (t < 1f ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			num = a;
		}
		return num;
	}

	public static float Spline(float t, float a, float b)
	{
		float single;
		if (t > 0f)
		{
			single = (t < 1f ? TransitionFunctions.Linear(TransitionFunctions.SimpleSpline(t), a, b) : b);
		}
		else
		{
			single = a;
		}
		return single;
	}

	public static double Sqrt(double v)
	{
		return Math.Sqrt(v);
	}

	public static float Sqrt(float v)
	{
		return Mathf.Sqrt(v);
	}

	public static Color Sum(Color a, Color b)
	{
		Color color = new Color();
		color.r = a.r + b.r;
		color.g = a.g + b.g;
		color.b = a.b + b.b;
		color.a = a.a * b.a;
		return color;
	}

	public static Matrix4x4 Sum(Matrix4x4 a, Matrix4x4 b)
	{
		Matrix4x4 matrix4x4 = new Matrix4x4();
		matrix4x4.m00 = a.m00 + b.m00;
		matrix4x4.m10 = a.m10 + b.m10;
		matrix4x4.m20 = a.m20 + b.m20;
		matrix4x4.m30 = a.m30 + b.m30;
		matrix4x4.m01 = a.m01 + b.m01;
		matrix4x4.m11 = a.m11 + b.m11;
		matrix4x4.m21 = a.m21 + b.m21;
		matrix4x4.m31 = a.m31 + b.m31;
		matrix4x4.m02 = a.m02 + b.m02;
		matrix4x4.m12 = a.m12 + b.m12;
		matrix4x4.m22 = a.m22 + b.m22;
		matrix4x4.m32 = a.m32 + b.m32;
		matrix4x4.m03 = a.m03 + b.m03;
		matrix4x4.m13 = a.m13 + b.m13;
		matrix4x4.m23 = a.m23 + b.m23;
		matrix4x4.m33 = a.m33 + b.m33;
		return matrix4x4;
	}

	public static Matrix4x4G Sum(Matrix4x4G a, Matrix4x4G b)
	{
		Matrix4x4G matrix4x4G = new Matrix4x4G();
		matrix4x4G.m00 = a.m00 + b.m00;
		matrix4x4G.m10 = a.m10 + b.m10;
		matrix4x4G.m20 = a.m20 + b.m20;
		matrix4x4G.m30 = a.m30 + b.m30;
		matrix4x4G.m01 = a.m01 + b.m01;
		matrix4x4G.m11 = a.m11 + b.m11;
		matrix4x4G.m21 = a.m21 + b.m21;
		matrix4x4G.m31 = a.m31 + b.m31;
		matrix4x4G.m02 = a.m02 + b.m02;
		matrix4x4G.m12 = a.m12 + b.m12;
		matrix4x4G.m22 = a.m22 + b.m22;
		matrix4x4G.m32 = a.m32 + b.m32;
		matrix4x4G.m03 = a.m03 + b.m03;
		matrix4x4G.m13 = a.m13 + b.m13;
		matrix4x4G.m23 = a.m23 + b.m23;
		matrix4x4G.m33 = a.m33 + b.m33;
		return matrix4x4G;
	}

	public static Quaternion Sum(Quaternion a, Quaternion b)
	{
		Quaternion quaternion = new Quaternion();
		quaternion.x = a.x + b.x;
		quaternion.y = a.y + b.y;
		quaternion.z = a.z + b.z;
		quaternion.w = a.w * b.w;
		return quaternion;
	}

	public static QuaternionG Sum(QuaternionG a, QuaternionG b)
	{
		QuaternionG quaternionG = new QuaternionG();
		quaternionG.x = a.x + b.x;
		quaternionG.y = a.y + b.y;
		quaternionG.z = a.z + b.z;
		quaternionG.w = a.w * b.w;
		return quaternionG;
	}

	public static Vector2 Sum(Vector2 a, Vector2 b)
	{
		Vector2 vector2 = new Vector2();
		vector2.x = a.x + b.x;
		vector2.y = a.y + b.y;
		return vector2;
	}

	public static Vector3 Sum(Vector3 a, Vector3 b)
	{
		Vector3 vector3 = new Vector3();
		vector3.x = a.x + b.x;
		vector3.y = a.y + b.y;
		vector3.z = a.z + b.z;
		return vector3;
	}

	public static Vector3G Sum(Vector3G a, Vector3G b)
	{
		Vector3G vector3G = new Vector3G();
		vector3G.x = a.x + b.x;
		vector3G.y = a.y + b.y;
		vector3G.z = a.z + b.z;
		return vector3G;
	}

	public static Vector4 Sum(Vector4 a, Vector4 b)
	{
		Vector4 vector4 = new Vector4();
		vector4.x = a.x + b.x;
		vector4.y = a.y + b.y;
		vector4.z = a.z + b.z;
		vector4.w = a.w * b.w;
		return vector4;
	}

	public static double Sum(double a, double b)
	{
		return a + b;
	}

	public static float Sum(float a, float b)
	{
		return a + b;
	}

	private static Vector3 TRANS(Matrix4x4 a)
	{
		return TransitionFunctions.GET_X3(a);
	}

	private static void TRANS(ref Matrix4x4 a, Vector3 v)
	{
		TransitionFunctions.SET_X3(ref a, v);
	}

	private static Vector3G TRANS(Matrix4x4G a)
	{
		return TransitionFunctions.GET_X3(a);
	}

	private static void TRANS(ref Matrix4x4G a, Vector3G v)
	{
		TransitionFunctions.SET_X3(ref a, v);
	}

	public static Matrix4x4 Transpose(Matrix4x4 v)
	{
		return Matrix4x4.Transpose(v);
	}

	public static Matrix4x4G Transpose(Matrix4x4G v)
	{
		Matrix4x4G matrix4x4G;
		Matrix4x4G.Transpose(ref v, out matrix4x4G);
		return matrix4x4G;
	}

	public static Vector3 Vect(float x, float y, float z)
	{
		Vector3 vector3 = new Vector3();
		vector3.x = x;
		vector3.y = y;
		vector3.z = z;
		return vector3;
	}

	public static Vector3G Vect(double x, double y, double z)
	{
		Vector3G vector3G = new Vector3G();
		vector3G.x = x;
		vector3G.y = y;
		vector3G.z = z;
		return vector3G;
	}

	private static Vector3G VECT3F(double x, double y, double z)
	{
		Vector3G vector3G = new Vector3G();
		vector3G.x = x;
		vector3G.y = y;
		vector3G.z = z;
		return vector3G;
	}

	public static Vector3 X3(float x)
	{
		Vector3 vector3 = new Vector3();
		float single = 0f;
		float single1 = single;
		vector3.z = single;
		vector3.y = single1;
		vector3.x = x;
		return vector3;
	}

	public static Vector3G X3(double x)
	{
		Vector3G vector3G = new Vector3G();
		double num = 0;
		double num1 = num;
		vector3G.z = num;
		vector3G.y = num1;
		vector3G.x = x;
		return vector3G;
	}

	public static Vector3 Y3(float y)
	{
		Vector3 vector3 = new Vector3();
		float single = 0f;
		float single1 = single;
		vector3.z = single;
		vector3.x = single1;
		vector3.y = y;
		return vector3;
	}

	public static Vector3G Y3(double y)
	{
		Vector3G vector3G = new Vector3G();
		double num = 0;
		double num1 = num;
		vector3G.z = num;
		vector3G.x = num1;
		vector3G.y = y;
		return vector3G;
	}

	public static Vector3 Z3(float z)
	{
		Vector3 vector3 = new Vector3();
		float single = 0f;
		float single1 = single;
		vector3.y = single;
		vector3.x = single1;
		vector3.z = z;
		return vector3;
	}

	public static Vector3G Z3(double z)
	{
		Vector3G vector3G = new Vector3G();
		double num = 0;
		double num1 = num;
		vector3G.y = num;
		vector3G.x = num1;
		vector3G.z = z;
		return vector3G;
	}
}