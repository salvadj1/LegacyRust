using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PathSlopScene : MonoBehaviour
{
	public Vector4[] sloppymess;

	public float initialWidth = 1f;

	public float areaGrid = 8f;

	public float pushup = 0.05f;

	public LayerMask layerMask = 1;

	public static PathSlopScene current
	{
		get
		{
			return null;
		}
	}

	public MeshFilter filter
	{
		get
		{
			return base.GetComponent<MeshFilter>();
		}
	}

	public new MeshRenderer renderer
	{
		get
		{
			return (MeshRenderer)base.renderer;
		}
	}

	public PathSlopScene()
	{
	}
}