using Facepunch.Precision;
using System;
using UnityEngine;

public class MatrixHelper : MonoBehaviour
{
	public MatrixHelper()
	{
	}

	public static bool InvertMatrix(ref Matrix4x4 m, out Matrix4x4 o)
	{
		o = new Matrix4x4();
		Vector4 vector4 = new Vector4();
		Vector4 vector41 = new Vector4();
		Vector4 vector42 = new Vector4();
		Vector4 vector43 = new Vector4();
		Vector4 vector44;
		Vector4 vector45 = new Vector4();
		Vector4 vector46 = new Vector4();
		Vector4 vector47 = new Vector4();
		Vector4 vector48 = new Vector4();
		vector4.x = m.m00;
		vector4.y = m.m01;
		vector4.z = m.m02;
		vector4.w = m.m03;
		vector41.x = m.m10;
		vector41.y = m.m11;
		vector41.z = m.m12;
		vector41.w = m.m13;
		vector42.x = m.m20;
		vector42.y = m.m21;
		vector42.z = m.m22;
		vector42.w = m.m23;
		vector43.x = m.m30;
		vector43.y = m.m31;
		vector43.z = m.m32;
		vector43.w = m.m33;
		float single = 1f;
		float single1 = single;
		vector48.w = single;
		float single2 = single1;
		single1 = single2;
		vector47.z = single2;
		float single3 = single1;
		single1 = single3;
		vector46.y = single3;
		vector45.x = single1;
		float single4 = 0f;
		single1 = single4;
		vector48.z = single4;
		float single5 = single1;
		single1 = single5;
		vector48.y = single5;
		float single6 = single1;
		single1 = single6;
		vector48.x = single6;
		float single7 = single1;
		single1 = single7;
		vector47.w = single7;
		float single8 = single1;
		single1 = single8;
		vector47.y = single8;
		float single9 = single1;
		single1 = single9;
		vector47.x = single9;
		float single10 = single1;
		single1 = single10;
		vector46.w = single10;
		float single11 = single1;
		single1 = single11;
		vector46.z = single11;
		float single12 = single1;
		single1 = single12;
		vector46.x = single12;
		float single13 = single1;
		single1 = single13;
		vector45.w = single13;
		float single14 = single1;
		single1 = single14;
		vector45.z = single14;
		vector45.y = single1;
		if (vector43.x * vector43.x > vector42.x * vector42.x)
		{
			vector44 = vector43;
			vector43 = vector42;
			vector42 = vector44;
			vector44 = vector48;
			vector48 = vector47;
			vector47 = vector44;
		}
		if (vector42.x * vector42.x > vector41.x * vector41.x)
		{
			vector44 = vector41;
			vector41 = vector42;
			vector42 = vector44;
			vector44 = vector46;
			vector46 = vector47;
			vector47 = vector44;
		}
		if (vector41.x * vector42.x > vector4.x * vector4.x)
		{
			vector44 = vector41;
			vector41 = vector4;
			vector4 = vector44;
			vector44 = vector45;
			vector45 = vector46;
			vector46 = vector44;
		}
		if ((double)vector4.x == 0)
		{
			o = new Matrix4x4();
			return false;
		}
		float single15 = vector41.x / vector4.x;
		float single16 = vector42.x / vector4.x;
		float single17 = vector43.x / vector4.x;
		vector41.y = vector41.y - single15 * vector4.y;
		vector42.y = vector42.y - single16 * vector4.y;
		vector43.y = vector43.y - single17 * vector4.y;
		vector41.z = vector41.z - single15 * vector4.z;
		vector42.z = vector42.z - single16 * vector4.z;
		vector43.z = vector43.z - single17 * vector4.z;
		vector41.w = vector41.w - single15 * vector4.w;
		vector42.w = vector42.w - single16 * vector4.w;
		vector43.w = vector43.w - single17 * vector4.w;
		if ((double)vector45.x != 0)
		{
			vector46.x = vector46.x - single15 * vector45.x;
			vector47.x = vector47.x - single16 * vector45.x;
			vector48.x = vector48.x - single17 * vector45.x;
		}
		if ((double)vector45.y != 0)
		{
			vector46.y = vector46.y - single15 * vector45.y;
			vector47.y = vector47.y - single16 * vector45.y;
			vector48.y = vector48.y - single17 * vector45.y;
		}
		if ((double)vector45.z != 0)
		{
			vector46.z = vector46.z - single15 * vector45.z;
			vector47.z = vector47.z - single16 * vector45.z;
			vector48.z = vector48.z - single17 * vector45.z;
		}
		if ((double)vector45.w != 0)
		{
			vector46.w = vector46.w - single15 * vector45.w;
			vector47.w = vector47.w - single16 * vector45.w;
			vector48.w = vector48.w - single17 * vector45.w;
		}
		if (vector43.y * vector43.y > vector42.y * vector42.y)
		{
			vector44 = vector43;
			vector43 = vector42;
			vector42 = vector44;
			vector44 = vector48;
			vector48 = vector47;
			vector47 = vector44;
		}
		if (vector42.y * vector42.y > vector41.y * vector41.y)
		{
			vector44 = vector41;
			vector41 = vector42;
			vector42 = vector44;
			vector44 = vector46;
			vector46 = vector47;
			vector47 = vector44;
		}
		if ((double)vector41.y == 0)
		{
			o = new Matrix4x4();
			return false;
		}
		single16 = vector42.y / vector41.y;
		single17 = vector43.y / vector41.y;
		vector42.z = vector42.z - single16 * vector41.z;
		vector43.z = vector43.z - single17 * vector41.z;
		vector42.w = vector42.w - single16 * vector41.w;
		vector43.w = vector43.w - single17 * vector41.w;
		if ((double)vector46.x != 0)
		{
			vector47.x = vector47.x - single16 * vector46.x;
			vector48.x = vector48.x - single17 * vector46.x;
		}
		if ((double)vector46.y != 0)
		{
			vector47.y = vector47.y - single16 * vector46.y;
			vector48.y = vector48.y - single17 * vector46.y;
		}
		if ((double)vector46.z != 0)
		{
			vector47.z = vector47.z - single16 * vector46.z;
			vector48.z = vector48.z - single17 * vector46.z;
		}
		if ((double)vector46.w != 0)
		{
			vector47.w = vector47.w - single16 * vector46.w;
			vector48.w = vector48.w - single17 * vector46.w;
		}
		if (vector43.y * vector43.y > vector42.y * vector42.y)
		{
			vector44 = vector43;
			vector43 = vector42;
			vector42 = vector44;
			vector44 = vector48;
			vector48 = vector47;
			vector47 = vector44;
		}
		if ((double)vector42.z == 0)
		{
			o = new Matrix4x4();
			return false;
		}
		single17 = vector43.z / vector42.z;
		vector43.w = vector43.w - single17 * vector42.w;
		vector48.x = vector48.x - single17 * vector47.x;
		vector48.y = vector48.y - single17 * vector47.y;
		vector48.z = vector48.z - single17 * vector47.z;
		vector48.w = vector48.w - single17 * vector47.w;
		if ((double)vector43.w == 0)
		{
			o = new Matrix4x4();
			return false;
		}
		float single18 = 1f / vector43.w;
		vector48.x = vector48.x * single18;
		vector48.y = vector48.y * single18;
		vector48.z = vector48.z * single18;
		vector48.w = vector48.w * single18;
		single16 = vector42.w;
		single18 = 1f / vector42.z;
		vector47.x = single18 * (vector47.x - vector48.x * single16);
		vector47.y = single18 * (vector47.y - vector48.y * single16);
		vector47.z = single18 * (vector47.z - vector48.z * single16);
		vector47.w = single18 * (vector47.w - vector48.w * single16);
		single15 = vector41.w;
		vector46.x = vector46.x - vector48.x * single15;
		vector46.y = vector46.y - vector48.y * single15;
		vector46.z = vector46.z - vector48.z * single15;
		vector46.w = vector46.w - vector48.w * single15;
		float single19 = vector4.w;
		vector45.x = vector45.x - vector48.x * single19;
		vector45.y = vector45.y - vector48.y * single19;
		vector45.z = vector45.z - vector48.z * single19;
		vector45.w = vector45.w - vector48.w * single19;
		single15 = vector41.z;
		single18 = 1f / vector41.y;
		vector46.x = single18 * (vector46.x - vector47.x * single15);
		vector46.y = single18 * (vector46.y - vector47.y * single15);
		vector46.z = single18 * (vector46.z - vector47.z * single15);
		vector46.w = single18 * (vector46.w - vector47.w * single15);
		single19 = vector4.z;
		vector45.x = vector45.x - vector47.x * single19;
		vector45.y = vector45.y - vector47.y * single19;
		vector45.z = vector45.z - vector47.z * single19;
		vector45.w = vector45.w - vector47.w * single19;
		single19 = vector4.y;
		single18 = 1f / vector4.x;
		vector45.x = single18 * (vector45.x - vector46.x * single19);
		vector45.y = single18 * (vector45.y - vector46.y * single19);
		vector45.z = single18 * (vector45.z - vector46.z * single19);
		vector45.w = single18 * (vector45.w - vector46.w * single19);
		o.m00 = vector45.x;
		o.m01 = vector45.y;
		o.m02 = vector45.z;
		o.m03 = vector45.w;
		o.m10 = vector46.x;
		o.m11 = vector46.y;
		o.m12 = vector46.z;
		o.m13 = vector46.w;
		o.m20 = vector47.x;
		o.m21 = vector47.y;
		o.m22 = vector47.z;
		o.m23 = vector47.w;
		o.m30 = vector48.x;
		o.m31 = vector48.y;
		o.m32 = vector48.z;
		o.m33 = vector48.w;
		return true;
	}

	public static void MultiplyVector4(out Vector4 resultvector, ref Matrix4x4 matrix, ref Vector4 pvector)
	{
		resultvector = new Vector4();
		resultvector.x = matrix[0] * pvector[0] + matrix[4] * pvector[1] + matrix[8] * pvector[2] + matrix[12] * pvector[3];
		resultvector.y = matrix[1] * pvector[0] + matrix[5] * pvector[1] + matrix[9] * pvector[2] + matrix[13] * pvector[3];
		resultvector.z = matrix[2] * pvector[0] + matrix[6] * pvector[1] + matrix[10] * pvector[2] + matrix[14] * pvector[3];
		resultvector.w = matrix[3] * pvector[0] + matrix[7] * pvector[1] + matrix[11] * pvector[2] + matrix[15] * pvector[3];
	}

	public static bool Project(ref Vector3 obj, ref Matrix4x4 modelview, ref Matrix4x4 projection, ref Vector4 viewport, out Vector3 windowCoordinate)
	{
		windowCoordinate = new Vector3();
		Vector4 vector4 = new Vector4();
		Vector4 vector41 = new Vector4();
		vector4.x = modelview.m00 * obj.x + modelview.m10 * obj.y + modelview.m20 * obj.z + modelview.m30;
		vector4.y = modelview.m01 * obj.x + modelview.m11 * obj.y + modelview.m21 * obj.z + modelview.m31;
		vector4.z = modelview.m02 * obj.x + modelview.m12 * obj.y + modelview.m22 * obj.z + modelview.m32;
		vector4.w = modelview.m03 * obj.x + modelview.m13 * obj.y + modelview.m23 * obj.z + modelview.m33;
		vector41.x = projection.m00 * vector4.x + projection.m10 * vector4.y + projection.m20 * vector4.z + projection.m30 * vector4.w;
		vector41.y = projection.m01 * vector4.x + projection.m11 * vector4.y + projection.m21 * vector4.z + projection.m31 * vector4.w;
		vector41.z = projection.m02 * vector4.x + projection.m12 * vector4.y + projection.m22 * vector4.z + projection.m32 * vector4.w;
		vector41.w = -vector4.z;
		if ((double)vector41.w == 0)
		{
			windowCoordinate = new Vector3();
			return false;
		}
		vector41.w = 1f / vector41.w;
		vector41.x = vector41.x * vector41.w;
		vector41.y = vector41.y * vector41.w;
		vector41.z = vector41.z * vector41.w;
		windowCoordinate.x = (vector41.x * 0.5f + 0.5f) * viewport.z + viewport.x;
		windowCoordinate.y = (vector41.y * 0.5f + 0.5f) * viewport.w + viewport.y;
		windowCoordinate.z = 1f - vector41.z;
		return true;
	}

	public static bool UnProject(ref Vector3 win, ref Matrix4x4 modelview, ref Matrix4x4 projection, ref Vector4 viewport, out Vector3 objectCoordinate)
	{
		objectCoordinate = new Vector3();
		Matrix4x4 matrix4x4;
		Vector4 vector4 = new Vector4();
		Vector4 vector41;
		Matrix4x4 matrix4x41 = projection * modelview;
		if (!MatrixHelper.InvertMatrix(ref matrix4x41, out matrix4x4))
		{
			objectCoordinate = new Vector3();
			return false;
		}
		vector4.x = (win.x - viewport.x) / viewport.z * 2f - 1f;
		vector4.y = (win.y - viewport.y) / viewport.w * 2f - 1f;
		vector4.z = 1f - win.z;
		vector4.w = 1f;
		MatrixHelper.MultiplyVector4(out vector41, ref matrix4x4, ref vector4);
		if ((double)vector41.w == 0)
		{
			objectCoordinate = new Vector3();
			return false;
		}
		vector41.w = 1f / vector41.w;
		objectCoordinate.x = vector41.x * vector41.w;
		objectCoordinate.y = vector41.y * vector41.w;
		objectCoordinate.z = vector41.z * vector41.w;
		return true;
	}

	public struct ProjectHelper
	{
		public Matrix4x4 modelview;

		public Matrix4x4 projection;

		public Vector2 offset;

		public Vector2 size;

		public bool Project(ref Vector3 obj, out Vector3 windowCoordinate)
		{
			windowCoordinate = new Vector3();
			Vector4 vector4 = new Vector4();
			Vector4 vector41 = new Vector4();
			vector4.x = this.modelview.m00 * obj.x + this.modelview.m01 * obj.y + this.modelview.m02 * obj.z + this.modelview.m03;
			vector4.y = this.modelview.m10 * obj.x + this.modelview.m11 * obj.y + this.modelview.m12 * obj.z + this.modelview.m13;
			vector4.z = this.modelview.m20 * obj.x + this.modelview.m21 * obj.y + this.modelview.m22 * obj.z + this.modelview.m23;
			vector4.w = this.modelview.m30 * obj.x + this.modelview.m31 * obj.y + this.modelview.m32 * obj.z + this.modelview.m33;
			vector41.x = this.projection.m00 * vector4.x + this.projection.m01 * vector4.y + this.projection.m02 * vector4.z + this.projection.m03 * vector4.w;
			vector41.y = this.projection.m10 * vector4.x + this.projection.m11 * vector4.y + this.projection.m12 * vector4.z + this.projection.m13 * vector4.w;
			vector41.z = this.projection.m20 * vector4.x + this.projection.m21 * vector4.y + this.projection.m22 * vector4.z + this.projection.m23 * vector4.w;
			vector41.w = -vector4.z;
			if ((double)vector41.w == 0)
			{
				windowCoordinate = new Vector3();
				return false;
			}
			vector41.w = 1f / vector41.w;
			vector41.x = vector41.x * vector41.w;
			vector41.y = vector41.y * vector41.w;
			windowCoordinate.x = (vector41.x * 0.5f + 0.5f) * this.size.x + this.offset.x;
			windowCoordinate.y = (vector41.y * 0.5f + 0.5f) * this.size.y + this.offset.y;
			windowCoordinate.z = vector41.z;
			return true;
		}

		public bool UnProject(ref Vector3 win, out Vector3 objectCoordinate)
		{
			objectCoordinate = new Vector3();
			Matrix4x4 matrix4x4;
			Vector4 vector4 = new Vector4();
			Vector4 vector41;
			Matrix4x4 matrix4x41 = this.projection * this.modelview;
			if (!MatrixHelper.InvertMatrix(ref matrix4x41, out matrix4x4))
			{
				objectCoordinate = new Vector3();
				return false;
			}
			vector4.x = (win.x - this.offset.x) / this.size.x * 2f - 1f;
			vector4.y = (win.y - this.offset.y) / this.size.y * 2f - 1f;
			vector4.z = -win.z;
			vector4.w = 1f;
			MatrixHelper.MultiplyVector4(out vector41, ref matrix4x4, ref vector4);
			if ((double)vector41.w == 0)
			{
				objectCoordinate = new Vector3();
				return false;
			}
			vector41.w = 1f / vector41.w;
			objectCoordinate.x = vector41.x * vector41.w;
			objectCoordinate.y = vector41.y * vector41.w;
			objectCoordinate.z = vector41.z * vector41.w;
			return true;
		}
	}

	public struct ProjectHelperG
	{
		public Matrix4x4G modelview;

		public Matrix4x4G projection;

		public Vector2G offset;

		public Vector2G size;

		public bool Project(ref Vector3G obj, out Vector3G windowCoordinate)
		{
			windowCoordinate = new Vector3G();
			Vector4G vector4G = new Vector4G();
			Vector4G vector4G1 = new Vector4G();
			vector4G.x = this.modelview.m00 * obj.x + this.modelview.m01 * obj.y + this.modelview.m02 * obj.z + this.modelview.m03;
			vector4G.y = this.modelview.m10 * obj.x + this.modelview.m11 * obj.y + this.modelview.m12 * obj.z + this.modelview.m13;
			vector4G.z = this.modelview.m20 * obj.x + this.modelview.m21 * obj.y + this.modelview.m22 * obj.z + this.modelview.m23;
			vector4G.w = this.modelview.m30 * obj.x + this.modelview.m31 * obj.y + this.modelview.m32 * obj.z + this.modelview.m33;
			vector4G1.x = this.projection.m00 * vector4G.x + this.projection.m01 * vector4G.y + this.projection.m02 * vector4G.z + this.projection.m03 * vector4G.w;
			vector4G1.y = this.projection.m10 * vector4G.x + this.projection.m11 * vector4G.y + this.projection.m12 * vector4G.z + this.projection.m13 * vector4G.w;
			vector4G1.z = this.projection.m20 * vector4G.x + this.projection.m21 * vector4G.y + this.projection.m22 * vector4G.z + this.projection.m23 * vector4G.w;
			vector4G1.w = -vector4G.z;
			if (vector4G1.w == 0)
			{
				windowCoordinate = new Vector3G();
				return false;
			}
			vector4G1.w = 1 / vector4G1.w;
			vector4G1.x = vector4G1.x * vector4G1.w;
			vector4G1.y = vector4G1.y * vector4G1.w;
			windowCoordinate.x = (vector4G1.x * 0.5 + 0.5) * this.size.x + this.offset.x;
			windowCoordinate.y = (vector4G1.y * 0.5 + 0.5) * this.size.y + this.offset.y;
			windowCoordinate.z = vector4G1.z;
			return true;
		}

		public bool UnProject(ref Vector3G win, out Vector3G objectCoordinate)
		{
			objectCoordinate = new Vector3G();
			Matrix4x4G matrix4x4G;
			Matrix4x4G matrix4x4G1;
			Vector4G vector4G = new Vector4G();
			Vector4G vector4G1;
			Matrix4x4G.Mult(ref this.projection, ref this.modelview, out matrix4x4G);
			if (!Matrix4x4G.Inverse(ref matrix4x4G, out matrix4x4G1))
			{
				objectCoordinate = new Vector3G();
				return false;
			}
			vector4G.x = (win.x - this.offset.x) / this.size.x * 2 - 1;
			vector4G.y = (win.y - this.offset.y) / this.size.y * 2 - 1;
			vector4G.z = -win.z;
			vector4G.w = 1;
			Matrix4x4G.Mult(ref vector4G, ref matrix4x4G1, out vector4G1);
			if (vector4G1.w == 0)
			{
				objectCoordinate = new Vector3G();
				return false;
			}
			vector4G1.w = 1 / vector4G1.w;
			objectCoordinate.x = vector4G1.x * vector4G1.w;
			objectCoordinate.y = vector4G1.y * vector4G1.w;
			objectCoordinate.z = vector4G1.z * vector4G1.w;
			return true;
		}
	}
}