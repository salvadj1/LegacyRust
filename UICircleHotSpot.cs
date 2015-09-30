using System;
using UnityEngine;

public class UICircleHotSpot : UIHotSpot
{
	private const UIHotSpot.Kind kKind = UIHotSpot.Kind.Circle;

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

	public UICircleHotSpot() : base(UIHotSpot.Kind.Circle, true)
	{
	}

	internal Bounds? Internal_CalculateBounds(bool moved)
	{
		float single = this.radius * 2f;
		return new Bounds?(new Bounds(this.center, new Vector3(single, single, 0f)));
	}

	internal bool Internal_RaycastRef(Ray ray, ref UIHotSpot.Hit hit)
	{
		float single;
		Vector2 vector2 = new Vector2();
		if (this.radius == 0f)
		{
			return false;
		}
		Plane plane = new Plane(UIHotSpot.forward, this.center);
		if (!plane.Raycast(ray, out single))
		{
			hit = new UIHotSpot.Hit();
			return false;
		}
		hit.point = ray.GetPoint(single);
		hit.normal = (!plane.GetSide(ray.origin) ? UIHotSpot.backward : UIHotSpot.forward);
		vector2.x = hit.point.x - this.center.x;
		vector2.y = hit.point.y - this.center.y;
		float single1 = vector2.x * vector2.x + vector2.y * vector2.y;
		if (single1 >= this.radius * this.radius)
		{
			return false;
		}
		hit.distance = Mathf.Sqrt(single1);
		return true;
	}

	private void OnDrawGizmos()
	{
		Gizmos.matrix = base.gizmoMatrix * Matrix4x4.TRS(this.center, Quaternion.identity, new Vector3(1f, 1f, 0.0001f));
		Gizmos.color = base.gizmoColor;
		Gizmos.DrawWireSphere(Vector3.zero, this.radius);
	}
}