using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class dfRenderData : IDisposable
{
	private static Queue<dfRenderData> pool;

	public uint Checksum
	{
		get;
		set;
	}

	public dfList<Color32> Colors
	{
		get;
		set;
	}

	public dfIntersectionType Intersection
	{
		get;
		set;
	}

	public UnityEngine.Material Material
	{
		get;
		set;
	}

	public dfList<Vector3> Normals
	{
		get;
		set;
	}

	public UnityEngine.Shader Shader
	{
		get;
		set;
	}

	public dfList<Vector4> Tangents
	{
		get;
		set;
	}

	public Matrix4x4 Transform
	{
		get;
		set;
	}

	public dfList<int> Triangles
	{
		get;
		set;
	}

	public dfList<Vector2> UV
	{
		get;
		set;
	}

	public dfList<Vector3> Vertices
	{
		get;
		set;
	}

	static dfRenderData()
	{
		dfRenderData.pool = new Queue<dfRenderData>();
	}

	internal dfRenderData(int capacity = 32)
	{
		this.Vertices = new dfList<Vector3>(capacity);
		this.Triangles = new dfList<int>(capacity);
		this.Normals = new dfList<Vector3>(capacity);
		this.Tangents = new dfList<Vector4>(capacity);
		this.UV = new dfList<Vector2>(capacity);
		this.Colors = new dfList<Color32>(capacity);
		this.Transform = Matrix4x4.identity;
	}

	internal void ApplyTransform(Matrix4x4 transform)
	{
		for (int i = 0; i < this.Vertices.Count; i++)
		{
			this.Vertices[i] = transform.MultiplyPoint(this.Vertices[i]);
		}
		if (this.Normals.Count > 0)
		{
			for (int j = 0; j < this.Vertices.Count; j++)
			{
				this.Normals[j] = transform.MultiplyVector(this.Normals[j]);
			}
		}
	}

	public void Clear()
	{
		this.Material = null;
		this.Shader = null;
		this.Transform = Matrix4x4.identity;
		this.Checksum = 0;
		this.Intersection = dfIntersectionType.None;
		this.Vertices.Clear();
		this.UV.Clear();
		this.Triangles.Clear();
		this.Colors.Clear();
		this.Normals.Clear();
		this.Tangents.Clear();
	}

	public void Dispose()
	{
		this.Release();
	}

	public void EnsureCapacity(int capacity)
	{
		this.Vertices.EnsureCapacity(capacity);
		this.Triangles.EnsureCapacity(capacity);
		this.UV.EnsureCapacity(capacity);
		this.Colors.EnsureCapacity(capacity);
	}

	public static void FlushObjectPool()
	{
		while (dfRenderData.pool.Count > 0)
		{
			dfRenderData dfRenderDatum = dfRenderData.pool.Dequeue();
			dfRenderDatum.Vertices.TrimExcess();
			dfRenderDatum.Triangles.TrimExcess();
			dfRenderDatum.UV.TrimExcess();
			dfRenderDatum.Colors.TrimExcess();
		}
	}

	public bool IsValid()
	{
		int count = this.Vertices.Count;
		return (count <= 0 || count > 65000 || this.UV.Count != count ? false : this.Colors.Count == count);
	}

	public void Merge(dfRenderData buffer, bool transformVertices = true)
	{
		int count = this.Vertices.Count;
		this.Vertices.EnsureCapacity(this.Vertices.Count + buffer.Vertices.Count);
		if (!transformVertices)
		{
			this.Vertices.AddRange(buffer.Vertices);
		}
		else
		{
			for (int i = 0; i < buffer.Vertices.Count; i++)
			{
				dfList<Vector3> vertices = this.Vertices;
				Matrix4x4 transform = buffer.Transform;
				vertices.Add(transform.MultiplyPoint(buffer.Vertices[i]));
			}
		}
		this.UV.AddRange(buffer.UV);
		this.Colors.AddRange(buffer.Colors);
		this.Normals.AddRange(buffer.Normals);
		this.Tangents.AddRange(buffer.Tangents);
		this.Triangles.EnsureCapacity(this.Triangles.Count + buffer.Triangles.Count);
		for (int j = 0; j < buffer.Triangles.Count; j++)
		{
			this.Triangles.Add(buffer.Triangles[j] + count);
		}
	}

	public static dfRenderData Obtain()
	{
		return (dfRenderData.pool.Count <= 0 ? new dfRenderData(32) : dfRenderData.pool.Dequeue());
	}

	public void Release()
	{
		this.Clear();
		dfRenderData.pool.Enqueue(this);
	}

	public override string ToString()
	{
		return string.Format("V:{0} T:{1} U:{2} C:{3}", new object[] { this.Vertices.Count, this.Triangles.Count, this.UV.Count, this.Colors.Count });
	}
}