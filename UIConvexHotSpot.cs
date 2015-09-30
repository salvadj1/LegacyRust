using System;
using UnityEngine;

public class UIConvexHotSpot : UIHotSpot
{
	private const UIHotSpot.Kind kKind = UIHotSpot.Kind.Convex;

	public UIConvexHotSpot() : base(UIHotSpot.Kind.Convex, true)
	{
	}

	internal Bounds? Internal_CalculateBounds(bool moved)
	{
		throw new NotImplementedException();
	}

	internal bool Internal_RaycastRef(Ray ray, ref UIHotSpot.Hit hit)
	{
		throw new NotImplementedException();
	}
}