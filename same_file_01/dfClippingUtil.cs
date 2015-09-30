using System;
using System.Collections.Generic;
using UnityEngine;

public class dfClippingUtil
{
	private static int[] inside;

	private static dfClippingUtil.ClipTriangle[] clipSource;

	private static dfClippingUtil.ClipTriangle[] clipDest;

	static dfClippingUtil()
	{
		dfClippingUtil.inside = new int[3];
		dfClippingUtil.clipSource = dfClippingUtil.initClipBuffer(1024);
		dfClippingUtil.clipDest = dfClippingUtil.initClipBuffer(1024);
	}

	public dfClippingUtil()
	{
	}

	public static void Clip(IList<Plane> planes, dfRenderData source, dfRenderData dest)
	{
		dest.EnsureCapacity(dest.Vertices.Count + source.Vertices.Count);
		for (int i = 0; i < source.Triangles.Count; i = i + 3)
		{
			for (int j = 0; j < 3; j++)
			{
				int item = source.Triangles[i + j];
				Matrix4x4 transform = source.Transform;
				dfClippingUtil.clipSource[0].corner[j] = transform.MultiplyPoint(source.Vertices[item]);
				dfClippingUtil.clipSource[0].uv[j] = source.UV[item];
				dfClippingUtil.clipSource[0].color[j] = source.Colors[item];
			}
			int plane = 1;
			for (int k = 0; k < planes.Count; k++)
			{
				plane = dfClippingUtil.clipToPlane(planes[k], dfClippingUtil.clipSource, dfClippingUtil.clipDest, plane);
				dfClippingUtil.ClipTriangle[] clipTriangleArray = dfClippingUtil.clipSource;
				dfClippingUtil.clipSource = dfClippingUtil.clipDest;
				dfClippingUtil.clipDest = clipTriangleArray;
			}
			for (int l = 0; l < plane; l++)
			{
				dfClippingUtil.clipSource[l].CopyTo(dest);
			}
		}
	}

	private static int clipToPlane(Plane plane, dfClippingUtil.ClipTriangle[] source, dfClippingUtil.ClipTriangle[] dest, int count)
	{
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			num = num + dfClippingUtil.clipToPlane(plane, source[i], dest, num);
		}
		return num;
	}

	private static int clipToPlane(Plane plane, dfClippingUtil.ClipTriangle triangle, dfClippingUtil.ClipTriangle[] dest, int destIndex)
	{
		Vector3[] vector3Array = triangle.corner;
		int num = 0;
		int num1 = 0;
		Vector3 vector3 = plane.normal;
		float single = plane.distance;
		for (int i = 0; i < 3; i++)
		{
			if (Vector3.Dot(vector3, vector3Array[i]) + single <= 0f)
			{
				num1 = i;
			}
			else
			{
				int num2 = num;
				num = num2 + 1;
				dfClippingUtil.inside[num2] = i;
			}
		}
		if (num == 3)
		{
			triangle.CopyTo(dest[destIndex]);
			return 1;
		}
		if (num == 0)
		{
			return 0;
		}
		if (num == 1)
		{
			int num3 = dfClippingUtil.inside[0];
			int num4 = (num3 + 1) % 3;
			int num5 = (num3 + 2) % 3;
			Vector3 vector31 = vector3Array[num3];
			Vector3 vector32 = vector3Array[num4];
			Vector3 vector33 = vector3Array[num5];
			Vector2 vector2 = triangle.uv[num3];
			Vector2 vector21 = triangle.uv[num4];
			Vector2 vector22 = triangle.uv[num5];
			Color32 color32 = triangle.color[num3];
			Color32 color321 = triangle.color[num4];
			Color32 color322 = triangle.color[num5];
			float single1 = 0f;
			Vector3 vector34 = vector32 - vector31;
			Ray ray = new Ray(vector31, vector34.normalized);
			plane.Raycast(ray, out single1);
			float single2 = single1 / vector34.magnitude;
			Vector3 vector35 = ray.origin + (ray.direction * single1);
			Vector2 vector23 = Vector2.Lerp(vector2, vector21, single2);
			Color color = Color.Lerp(color32, color321, single2);
			vector34 = vector33 - vector31;
			ray = new Ray(vector31, vector34.normalized);
			plane.Raycast(ray, out single1);
			single2 = single1 / vector34.magnitude;
			Vector3 vector36 = ray.origin + (ray.direction * single1);
			Vector2 vector24 = Vector2.Lerp(vector2, vector22, single2);
			Color color1 = Color.Lerp(color32, color322, single2);
			dest[destIndex].corner[0] = vector31;
			dest[destIndex].corner[1] = vector35;
			dest[destIndex].corner[2] = vector36;
			dest[destIndex].uv[0] = vector2;
			dest[destIndex].uv[1] = vector23;
			dest[destIndex].uv[2] = vector24;
			dest[destIndex].color[0] = color32;
			dest[destIndex].color[1] = color;
			dest[destIndex].color[2] = color1;
			return 1;
		}
		int num6 = num1;
		int num7 = (num6 + 1) % 3;
		int num8 = (num6 + 2) % 3;
		Vector3 vector37 = vector3Array[num6];
		Vector3 vector38 = vector3Array[num7];
		Vector3 vector39 = vector3Array[num8];
		Vector2 vector25 = triangle.uv[num6];
		Vector2 vector26 = triangle.uv[num7];
		Vector2 vector27 = triangle.uv[num8];
		Color32 color323 = triangle.color[num6];
		Color32 color324 = triangle.color[num7];
		Color32 color325 = triangle.color[num8];
		Vector3 vector310 = vector38 - vector37;
		Ray ray1 = new Ray(vector37, vector310.normalized);
		float single3 = 0f;
		plane.Raycast(ray1, out single3);
		float single4 = single3 / vector310.magnitude;
		Vector3 vector311 = ray1.origin + (ray1.direction * single3);
		Vector2 vector28 = Vector2.Lerp(vector25, vector26, single4);
		Color color2 = Color.Lerp(color323, color324, single4);
		vector310 = vector39 - vector37;
		ray1 = new Ray(vector37, vector310.normalized);
		plane.Raycast(ray1, out single3);
		single4 = single3 / vector310.magnitude;
		Vector3 vector312 = ray1.origin + (ray1.direction * single3);
		Vector2 vector29 = Vector2.Lerp(vector25, vector27, single4);
		Color color3 = Color.Lerp(color323, color325, single4);
		dest[destIndex].corner[0] = vector311;
		dest[destIndex].corner[1] = vector38;
		dest[destIndex].corner[2] = vector312;
		dest[destIndex].uv[0] = vector28;
		dest[destIndex].uv[1] = vector26;
		dest[destIndex].uv[2] = vector29;
		dest[destIndex].color[0] = color2;
		dest[destIndex].color[1] = color324;
		dest[destIndex].color[2] = color3;
		destIndex++;
		dest[destIndex].corner[0] = vector312;
		dest[destIndex].corner[1] = vector38;
		dest[destIndex].corner[2] = vector39;
		dest[destIndex].uv[0] = vector29;
		dest[destIndex].uv[1] = vector26;
		dest[destIndex].uv[2] = vector27;
		dest[destIndex].color[0] = color3;
		dest[destIndex].color[1] = color324;
		dest[destIndex].color[2] = color325;
		return 2;
	}

	private static dfClippingUtil.ClipTriangle[] initClipBuffer(int size)
	{
		dfClippingUtil.ClipTriangle[] clipTriangleArray = new dfClippingUtil.ClipTriangle[size];
		for (int i = 0; i < size; i++)
		{
			clipTriangleArray[i].corner = new Vector3[3];
			clipTriangleArray[i].uv = new Vector2[3];
			clipTriangleArray[i].color = new Color32[3];
		}
		return clipTriangleArray;
	}

	protected struct ClipTriangle
	{
		public Vector3[] corner;

		public Vector2[] uv;

		public Color32[] color;

		public void CopyTo(dfClippingUtil.ClipTriangle target)
		{
			Array.Copy(this.corner, target.corner, 3);
			Array.Copy(this.uv, target.uv, 3);
			Array.Copy(this.color, target.color, 3);
		}

		public void CopyTo(dfRenderData buffer)
		{
			int count = buffer.Vertices.Count;
			buffer.Vertices.AddRange(this.corner);
			buffer.UV.AddRange(this.uv);
			buffer.Colors.AddRange(this.color);
			buffer.Triangles.Add(count);
			buffer.Triangles.Add(count + 1);
			buffer.Triangles.Add(count + 2);
		}
	}
}