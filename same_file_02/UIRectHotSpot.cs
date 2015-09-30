using System;
using UnityEngine;

public class UIRectHotSpot : UIHotSpot
{
	private const UIHotSpot.Kind kKind = UIHotSpot.Kind.Rect;

	private const float kEpsilon = 2.802597E-45f;

	public Vector3 center;

	public Vector2 size = Vector2.one;

	public UIRectHotSpot() : base(UIHotSpot.Kind.Rect, true)
	{
	}

	internal Bounds? Internal_CalculateBounds(bool moved)
	{
		return new Bounds?(new Bounds(this.center, this.size));
	}

	internal bool Internal_RaycastRef(Ray ray, ref UIHotSpot.Hit hit)
	{
		float single;
		Vector2 vector2 = new Vector2();
		if (this.size.x < 2.802597E-45f || this.size.y < 2.802597E-45f)
		{
			return false;
		}
		hit.normal = UIHotSpot.forward;
		Plane plane = new Plane(UIHotSpot.forward, this.center);
		if (!plane.Raycast(ray, out single))
		{
			hit = new UIHotSpot.Hit();
			return false;
		}
		hit.point = ray.GetPoint(single);
		vector2.x = (hit.point.x >= this.center.x ? hit.point.x - this.center.x : this.center.x - hit.point.x);
		vector2.y = (hit.point.y >= this.center.y ? hit.point.y - this.center.y : this.center.y - hit.point.y);
		if (vector2.x * 2f > this.size.x || vector2.y * 2f > this.size.y)
		{
			return false;
		}
		hit.distance = Mathf.Sqrt(vector2.x * vector2.x + vector2.y * vector2.y);
		return true;
	}

	private void OnDrawGizmos()
	{
		Gizmos.matrix = base.gizmoMatrix;
		Gizmos.color = base.gizmoColor;
		Gizmos.DrawWireCube(this.center, this.size);
	}
}