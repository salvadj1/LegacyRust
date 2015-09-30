using System;
using UnityEngine;

public class CustomMeshMaker : ScriptableObject
{
	public Vector3[] vertices;

	public Vector3[] normals;

	public Vector4[] tangents;

	public Color[] colors;

	public Vector2[] uv1;

	public Vector2[] uv2;

	public int[] triangles;

	public Bounds bounds;

	public bool optimize;

	public bool autoBound;

	public bool autoNormals;

	public string output;

	public CustomMeshMaker()
	{
	}
}