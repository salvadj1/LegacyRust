using System;
using UnityEngine;

public class GeometryMeshing
{
	public GeometryMeshing()
	{
	}

	public static GeometryMeshing.Mesh Capsule(GeometryMeshing.CapsuleInfo capsule)
	{
		GeometryMeshing.SphereInfo sphereInfo = new GeometryMeshing.SphereInfo();
		if (capsule.height <= capsule.radius * 2f)
		{
			sphereInfo.offset = capsule.offset;
			sphereInfo.radius = capsule.radius;
			sphereInfo.capSplit = capsule.capSplit;
			sphereInfo.sides = capsule.sides;
			return GeometryMeshing.Sphere(sphereInfo);
		}
		bool flag = capsule.capSplit == 0;
		int num = (!flag ? capsule.capSplit - 1 : 0);
		int num1 = (!flag ? num * capsule.sides + 1 : 0);
		Vector3[] vector3 = new Vector3[capsule.sides * 2 + (!flag ? 2 + num * capsule.sides * 2 : 0)];
		float single = capsule.offset.y - capsule.height / 2f;
		float single1 = single + capsule.radius;
		float single2 = capsule.offset.y + capsule.height / 2f;
		float single3 = single2 - capsule.radius;
		for (int i = 0; i < capsule.sides; i++)
		{
			float single4 = (float)i / ((float)capsule.sides / 2f) * 3.14159274f;
			int num2 = i + num1;
			int num3 = num2 + capsule.sides;
			float single5 = capsule.offset.x + Mathf.Cos(single4) * capsule.radius;
			float single6 = single5;
			vector3[num3].x = single5;
			vector3[num2].x = single6;
			float single7 = capsule.offset.z + Mathf.Sin(single4) * capsule.radius;
			single6 = single7;
			vector3[num3].z = single7;
			vector3[num2].z = single6;
			vector3[num2].y = single1;
			vector3[num3].y = single3;
		}
		if (!flag)
		{
			vector3[0] = new Vector3(capsule.offset.x, single, capsule.offset.z);
			vector3[(int)vector3.Length - 1] = new Vector3(capsule.offset.x, single2, capsule.offset.z);
		}
		int[] length = new int[3 * ((!flag ? capsule.sides : capsule.sides - 1) * 2 + capsule.sides * 2)];
		int num4 = 0;
		if (!flag)
		{
			for (int j = 0; j < capsule.sides; j++)
			{
				int num5 = num4;
				num4 = num5 + 1;
				length[num5] = j + num1;
				int num6 = num4;
				num4 = num6 + 1;
				length[num6] = (j + 1) % capsule.sides + num1;
				int num7 = num4;
				num4 = num7 + 1;
				length[num7] = 0;
			}
			for (int k = 0; k < capsule.sides; k++)
			{
				int num8 = num4;
				num4 = num8 + 1;
				length[num8] = k + num1 + capsule.sides;
				int num9 = num4;
				num4 = num9 + 1;
				length[num9] = (int)vector3.Length - 1;
				int num10 = num4;
				num4 = num10 + 1;
				length[num10] = (k + 1) % capsule.sides + num1 + capsule.sides;
			}
		}
		else
		{
			for (int l = 1; l < capsule.sides; l++)
			{
				int num11 = num4;
				num4 = num11 + 1;
				length[num11] = l + num1;
				int num12 = num4;
				num4 = num12 + 1;
				length[num12] = (l + 1) % capsule.sides + num1;
				int num13 = num4;
				num4 = num13 + 1;
				length[num13] = 0;
			}
			for (int m = 0; m < capsule.sides - 1; m++)
			{
				int num14 = num4;
				num4 = num14 + 1;
				length[num14] = m + num1 + capsule.sides;
				int num15 = num4;
				num4 = num15 + 1;
				length[num15] = (m + 1) % capsule.sides + num1 + capsule.sides;
				int num16 = num4;
				num4 = num16 + 1;
				length[num16] = (int)vector3.Length - 1;
			}
		}
		for (int n = 0; n < capsule.sides; n++)
		{
			int num17 = num4;
			num4 = num17 + 1;
			length[num17] = n + num1;
			int num18 = num4;
			num4 = num18 + 1;
			length[num18] = n + num1 + capsule.sides;
			int num19 = num4;
			num4 = num19 + 1;
			length[num19] = (n + 1) % capsule.sides + num1;
			int num20 = num4;
			num4 = num20 + 1;
			length[num20] = n + num1 + capsule.sides;
			int num21 = num4;
			num4 = num21 + 1;
			length[num21] = (n + 1) % capsule.sides + num1 + capsule.sides;
			int num22 = num4;
			num4 = num22 + 1;
			length[num22] = (n + 1) % capsule.sides + num1;
		}
		return new GeometryMeshing.Mesh(vector3, length, GeometryMeshing.IndexKind.Triangles);
	}

	public static GeometryMeshing.Mesh Sphere(GeometryMeshing.SphereInfo sphere)
	{
		Debug.Log("TODO");
		return new GeometryMeshing.Mesh();
	}

	public struct CapsuleInfo
	{
		public Vector3 offset;

		public float height;

		public float radius;

		public int sides;

		public int capSplit;
	}

	public enum IndexKind : sbyte
	{
		Invalid,
		Triangles,
		TriangleStrip
	}

	public struct Mesh
	{
		public readonly Vector3[] vertices;

		public readonly int[] indices;

		public readonly uint indexCount;

		public readonly ushort vertexCount;

		public readonly GeometryMeshing.IndexKind indexKind;

		public bool valid
		{
			get
			{
				return (int)this.indexKind != 0;
			}
		}

		internal Mesh(Vector3[] vertices, int[] indices, GeometryMeshing.IndexKind kind)
		{
			this.vertices = vertices;
			this.indices = indices;
			this.vertexCount = (ushort)((int)this.vertices.Length);
			this.indexCount = (uint)this.indices.Length;
			this.indexKind = kind;
		}
	}

	public struct SphereInfo
	{
		public Vector3 offset;

		public float radius;

		public int sides;

		public int capSplit;
	}
}