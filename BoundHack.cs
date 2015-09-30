using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class BoundHack : MonoBehaviour
{
	private static HashSet<BoundHack> renders;

	private SkinnedMeshRenderer renderer;

	public Transform rootbone;

	public BoundHack()
	{
	}

	public static void Achieve(Vector3 centroid)
	{
		if (BoundHack.renders != null)
		{
			foreach (BoundHack render in BoundHack.renders)
			{
				render.renderer.localBounds = new Bounds(((!render.rootbone ? render.transform : render.rootbone)).InverseTransformPoint(centroid), new Vector3(100f, 100f, 100f));
			}
		}
	}

	private void Awake()
	{
		this.renderer = base.renderer as SkinnedMeshRenderer;
		if (BoundHack.renders == null)
		{
			BoundHack.renders = new HashSet<BoundHack>();
		}
		BoundHack.renders.Add(this);
	}

	private void OnDestroy()
	{
		if (BoundHack.renders != null)
		{
			BoundHack.renders.Remove(this);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (base.renderer)
		{
			Bounds bound = base.renderer.bounds;
			Gizmos.DrawWireCube(bound.center, base.renderer.bounds.size);
		}
		if (this.rootbone && this.renderer)
		{
			Gizmos.color = new Color(0.8f, 0.8f, 1f, 0.1f);
			Gizmos.matrix = this.rootbone.localToWorldMatrix;
			Gizmos.DrawCube(this.renderer.localBounds.center, this.renderer.localBounds.size);
		}
	}
}