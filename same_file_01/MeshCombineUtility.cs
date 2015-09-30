using System;
using UnityEngine;

public class MeshCombineUtility
{
	public MeshCombineUtility()
	{
	}

	public static Mesh Combine(MeshCombineUtility.MeshInstance[] combines, bool generateStrips)
	{
		int num = 0;
		int length = 0;
		int num1 = 0;
		MeshCombineUtility.MeshInstance[] meshInstanceArray = combines;
		for (int i = 0; i < (int)meshInstanceArray.Length; i++)
		{
			MeshCombineUtility.MeshInstance meshInstance = meshInstanceArray[i];
			if (meshInstance.mesh)
			{
				num = num + meshInstance.mesh.vertexCount;
				if (generateStrips)
				{
					int length1 = (int)meshInstance.mesh.GetTriangleStrip(meshInstance.subMeshIndex).Length;
					if (length1 == 0)
					{
						generateStrips = false;
					}
					else
					{
						if (num1 != 0)
						{
							num1 = ((num1 & 1) != 1 ? num1 + 2 : num1 + 3);
						}
						num1 = num1 + length1;
					}
				}
			}
		}
		if (!generateStrips)
		{
			MeshCombineUtility.MeshInstance[] meshInstanceArray1 = combines;
			for (int j = 0; j < (int)meshInstanceArray1.Length; j++)
			{
				MeshCombineUtility.MeshInstance meshInstance1 = meshInstanceArray1[j];
				if (meshInstance1.mesh)
				{
					length = length + (int)meshInstance1.mesh.GetTriangles(meshInstance1.subMeshIndex).Length;
				}
			}
		}
		Vector3[] vector3Array = new Vector3[num];
		Vector3[] vector3Array1 = new Vector3[num];
		Vector4[] vector4Array = new Vector4[num];
		Vector2[] vector2Array = new Vector2[num];
		Vector2[] vector2Array1 = new Vector2[num];
		Color[] colorArray = new Color[num];
		int[] numArray = new int[length];
		int[] numArray1 = new int[num1];
		int num2 = 0;
		MeshCombineUtility.MeshInstance[] meshInstanceArray2 = combines;
		for (int k = 0; k < (int)meshInstanceArray2.Length; k++)
		{
			MeshCombineUtility.MeshInstance meshInstance2 = meshInstanceArray2[k];
			if (meshInstance2.mesh)
			{
				MeshCombineUtility.Copy(meshInstance2.mesh.vertexCount, meshInstance2.mesh.vertices, vector3Array, ref num2, meshInstance2.transform);
			}
		}
		num2 = 0;
		MeshCombineUtility.MeshInstance[] meshInstanceArray3 = combines;
		for (int l = 0; l < (int)meshInstanceArray3.Length; l++)
		{
			MeshCombineUtility.MeshInstance meshInstance3 = meshInstanceArray3[l];
			if (meshInstance3.mesh)
			{
				Matrix4x4 matrix4x4 = meshInstance3.transform.inverse.transpose;
				MeshCombineUtility.CopyNormal(meshInstance3.mesh.vertexCount, meshInstance3.mesh.normals, vector3Array1, ref num2, matrix4x4);
			}
		}
		num2 = 0;
		MeshCombineUtility.MeshInstance[] meshInstanceArray4 = combines;
		for (int m = 0; m < (int)meshInstanceArray4.Length; m++)
		{
			MeshCombineUtility.MeshInstance meshInstance4 = meshInstanceArray4[m];
			if (meshInstance4.mesh)
			{
				Matrix4x4 matrix4x41 = meshInstance4.transform.inverse.transpose;
				MeshCombineUtility.CopyTangents(meshInstance4.mesh.vertexCount, meshInstance4.mesh.tangents, vector4Array, ref num2, matrix4x41);
			}
		}
		num2 = 0;
		MeshCombineUtility.MeshInstance[] meshInstanceArray5 = combines;
		for (int n = 0; n < (int)meshInstanceArray5.Length; n++)
		{
			MeshCombineUtility.MeshInstance meshInstance5 = meshInstanceArray5[n];
			if (meshInstance5.mesh)
			{
				MeshCombineUtility.Copy(meshInstance5.mesh.vertexCount, meshInstance5.mesh.uv, vector2Array, ref num2);
			}
		}
		num2 = 0;
		MeshCombineUtility.MeshInstance[] meshInstanceArray6 = combines;
		for (int o = 0; o < (int)meshInstanceArray6.Length; o++)
		{
			MeshCombineUtility.MeshInstance meshInstance6 = meshInstanceArray6[o];
			if (meshInstance6.mesh)
			{
				MeshCombineUtility.Copy(meshInstance6.mesh.vertexCount, meshInstance6.mesh.uv1, vector2Array1, ref num2);
			}
		}
		num2 = 0;
		MeshCombineUtility.MeshInstance[] meshInstanceArray7 = combines;
		for (int p = 0; p < (int)meshInstanceArray7.Length; p++)
		{
			MeshCombineUtility.MeshInstance meshInstance7 = meshInstanceArray7[p];
			if (meshInstance7.mesh)
			{
				MeshCombineUtility.CopyColors(meshInstance7.mesh.vertexCount, meshInstance7.mesh.colors, colorArray, ref num2);
			}
		}
		int length2 = 0;
		int length3 = 0;
		int num3 = 0;
		MeshCombineUtility.MeshInstance[] meshInstanceArray8 = combines;
		for (int q = 0; q < (int)meshInstanceArray8.Length; q++)
		{
			MeshCombineUtility.MeshInstance meshInstance8 = meshInstanceArray8[q];
			if (meshInstance8.mesh)
			{
				if (!generateStrips)
				{
					int[] triangles = meshInstance8.mesh.GetTriangles(meshInstance8.subMeshIndex);
					for (int r = 0; r < (int)triangles.Length; r++)
					{
						numArray[r + length2] = triangles[r] + num3;
					}
					length2 = length2 + (int)triangles.Length;
				}
				else
				{
					int[] triangleStrip = meshInstance8.mesh.GetTriangleStrip(meshInstance8.subMeshIndex);
					if (length3 != 0)
					{
						if ((length3 & 1) != 1)
						{
							numArray1[length3] = numArray1[length3 - 1];
							numArray1[length3 + 1] = triangleStrip[0] + num3;
							length3 = length3 + 2;
						}
						else
						{
							numArray1[length3] = numArray1[length3 - 1];
							numArray1[length3 + 1] = triangleStrip[0] + num3;
							numArray1[length3 + 2] = triangleStrip[0] + num3;
							length3 = length3 + 3;
						}
					}
					for (int s = 0; s < (int)triangleStrip.Length; s++)
					{
						numArray1[s + length3] = triangleStrip[s] + num3;
					}
					length3 = length3 + (int)triangleStrip.Length;
				}
				num3 = num3 + meshInstance8.mesh.vertexCount;
			}
		}
		Mesh mesh = new Mesh()
		{
			name = "Combined Mesh",
			vertices = vector3Array,
			normals = vector3Array1,
			colors = colorArray,
			uv = vector2Array,
			uv1 = vector2Array1,
			tangents = vector4Array
		};
		if (!generateStrips)
		{
			mesh.triangles = numArray;
		}
		else
		{
			mesh.SetTriangleStrip(numArray1, 0);
		}
		return mesh;
	}

	private static void Copy(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < (int)src.Length; i++)
		{
			dst[i + offset] = transform.MultiplyPoint(src[i]);
		}
		offset = offset + vertexcount;
	}

	private static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
	{
		for (int i = 0; i < (int)src.Length; i++)
		{
			dst[i + offset] = src[i];
		}
		offset = offset + vertexcount;
	}

	private static void CopyColors(int vertexcount, Color[] src, Color[] dst, ref int offset)
	{
		for (int i = 0; i < (int)src.Length; i++)
		{
			dst[i + offset] = src[i];
		}
		offset = offset + vertexcount;
	}

	private static void CopyNormal(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < (int)src.Length; i++)
		{
			Vector3 vector3 = transform.MultiplyVector(src[i]);
			dst[i + offset] = vector3.normalized;
		}
		offset = offset + vertexcount;
	}

	private static void CopyTangents(int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < (int)src.Length; i++)
		{
			Vector4 vector4 = src[i];
			Vector3 vector3 = new Vector3(vector4.x, vector4.y, vector4.z);
			vector3 = transform.MultiplyVector(vector3).normalized;
			dst[i + offset] = new Vector4(vector3.x, vector3.y, vector3.z, vector4.w);
		}
		offset = offset + vertexcount;
	}

	public struct MeshInstance
	{
		public Mesh mesh;

		public int subMeshIndex;

		public Matrix4x4 transform;
	}
}