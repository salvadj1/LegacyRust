using System;
using UnityEngine;

public class UISphereHotSpot : UIHotSpot
{
	private const UIHotSpot.Kind kKind = UIHotSpot.Kind.Sphere;

	public Vector3 center;

	public float radius = 0.5f;

	public new float size
	{
		get
		{
			return this.radius * 2f;
		}
		set
		{
			this.radius = value / 2f;
		}
	}

	public UISphereHotSpot() : base(UIHotSpot.Kind.Sphere, true)
	{
	}

	internal Bounds? Internal_CalculateBounds(bool moved)
	{
		float single = this.radius * 2f;
		return new Bounds?(new Bounds(this.center, new Vector3(single, single, single)));
	}

	internal bool Internal_RaycastRef(Ray ray, ref UIHotSpot.Hit hit)
	{
		float single;
		float single1;
		IntersectHint intersectHint = Intersect3D.RayCircleInfiniteForward(ray, this.center, this.radius, out single, out single1);
		switch (intersectHint)
		{
			case IntersectHint.Touching:
			case IntersectHint.Thru:
			case IntersectHint.Entry:
			{
				hit.distance = Mathf.Min(single, single1);
				if (hit.distance < 0f)
				{
					float single2 = Mathf.Max(single, single1);
					float single3 = single2;
					hit.distance = single2;
					if (single3 < 0f)
					{
						return false;
					}
				}
				hit.point = ray.GetPoint(hit.distance);
				hit.normal = Vector3.Normalize(hit.point - this.center);
				return true;
			}
		}
		Debug.Log(intersectHint, this);
		return false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.matrix = base.gizmoMatrix;
		Gizmos.color = base.gizmoColor;
		Gizmos.DrawWireSphere(this.center, this.radius);
	}
}