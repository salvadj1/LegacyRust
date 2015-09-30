using System;

public enum RelativeControl : sbyte
{
	Assigned = -2,
	IsOverriding = -1,
	Incompatible = 0,
	OverriddenBy = 2
}