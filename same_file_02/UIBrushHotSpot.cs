using System;
using UnityEngine;

public class UIBrushHotSpot : UIHotSpot
{
	private const UIHotSpot.Kind kKind = UIHotSpot.Kind.Brush;

	public UIBrushHotSpot() : base(UIHotSpot.Kind.Brush, true)
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