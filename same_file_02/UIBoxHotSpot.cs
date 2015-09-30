using System;
using UnityEngine;

public class UIBoxHotSpot : UIHotSpot
{
	private const UIHotSpot.Kind kKind = UIHotSpot.Kind.Box;

	public Vector3 center;

	public Vector3 size;

	public UIBoxHotSpot() : base(UIHotSpot.Kind.Box, true)
	{
	}

	internal Bounds? Internal_CalculateBounds(bool moved)
	{
		return new Bounds?(new Bounds(this.center, this.size));
	}

	internal bool Internal_RaycastRef(Ray ray, ref UIHotSpot.Hit hit)
	{
		Bounds bound = new Bounds(this.center, this.size);
		if (!bound.IntersectRay(ray, out hit.distance))
		{
			return false;
		}
		hit.point = ray.GetPoint(hit.distance);
		hit.normal = -ray.direction;
		return true;
	}

	private void OnDrawGizmos()
	{
		Gizmos.matrix = base.gizmoMatrix;
		Gizmos.color = base.gizmoColor;
		Gizmos.DrawWireCube(this.center, this.size);
	}
}