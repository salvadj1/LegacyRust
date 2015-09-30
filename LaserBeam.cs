using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public sealed class LaserBeam : MonoBehaviour
{
	public float beamMaxDistance = 100f;

	public Vector4 beamColor = Color.red;

	public float beamOutput = 1f;

	public float beamWidthStart = 0.1f;

	public float beamWidthEnd = 0.2f;

	public float dotRadiusStart = 0.15f;

	public float dotRadiusEnd = 0.25f;

	public bool isViewModel;

	public Vector4 dotColor = Color.red;

	public Material beamMaterial;

	public Material dotMaterial;

	public LayerMask beamLayers = 1;

	public LayerMask cullLayers = 1;

	public LaserBeam.FrameData frame;

	public LaserBeam()
	{
	}

	public static List<LaserBeam> Collect()
	{
		LaserBeam.g.currentRendering.Clear();
		LaserBeam.g.currentRendering.AddRange(LaserBeam.g.allActiveBeams);
		return LaserBeam.g.currentRendering;
	}

	private void OnDisable()
	{
		LaserBeam.g.allActiveBeams.Remove(this);
	}

	private void OnEnable()
	{
		LaserBeam.g.allActiveBeams.Add(this);
		LaserGraphics.EnsureGraphicsExist();
	}

	public struct FrameData
	{
		public MaterialPropertyBlock block;

		public Bounds bounds;

		public bool hit;

		public Vector3 hitPoint;

		public Vector3 hitNormal;

		public LaserBeam.Quad<Vector3> beamVertices;

		public LaserBeam.Quad<Vector3> beamNormals;

		public LaserBeam.Quad<Vector2> beamUVs;

		public LaserBeam.Quad<Vector3> dotVertices1;

		public LaserBeam.Quad<Vector3> dotVertices2;

		public LaserBeam.Quad<Color> beamColor;

		public LaserBeam.Quad<Color> dotColor1;

		public LaserBeam.Quad<Color> dotColor2;

		public Vector3 direction;

		public Vector3 origin;

		public Vector3 point;

		public float distance;

		public float distanceFraction;

		public float pointWidth;

		public float originWidth;

		public float dotRadius;

		public bool didHit;

		public bool drawDot;

		public int beamsLayer;

		internal LaserGraphics.MeshBuffer bufBeam;

		internal LaserGraphics.MeshBuffer bufDot;
	}

	private static class g
	{
		public static HashSet<LaserBeam> allActiveBeams;

		public static List<LaserBeam> currentRendering;

		static g()
		{
			LaserBeam.g.allActiveBeams = new HashSet<LaserBeam>();
			LaserBeam.g.currentRendering = new List<LaserBeam>();
		}
	}

	public struct Quad<T>
	{
		public T m0;

		public T m1;

		public T m2;

		public T m3;
	}
}