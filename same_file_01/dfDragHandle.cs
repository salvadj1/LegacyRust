using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Drag Handle")]
[ExecuteInEditMode]
[Serializable]
public class dfDragHandle : dfControl
{
	private Vector3 lastPosition;

	public dfDragHandle()
	{
	}

	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		base.GetRootContainer().BringToFront();
		args.Use();
		Plane plane = new Plane(this.parent.transform.TransformDirection(Vector3.back), this.parent.transform.position);
		Ray ray = args.Ray;
		float single = 0f;
		plane.Raycast(args.Ray, out single);
		this.lastPosition = ray.origin + (ray.direction * single);
		base.OnMouseDown(args);
	}

	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		args.Use();
		if (args.Buttons.IsSet(dfMouseButtons.Left))
		{
			Ray ray = args.Ray;
			float single = 0f;
			Vector3 vector3 = base.GetCamera().transform.TransformDirection(Vector3.back);
			Plane plane = new Plane(vector3, this.lastPosition);
			plane.Raycast(ray, out single);
			Vector3 vector31 = (ray.origin + (ray.direction * single)).Quantize(this.parent.PixelsToUnits());
			Vector3 vector32 = vector31 - this.lastPosition;
			Vector3 vector33 = (this.parent.transform.position + vector32).Quantize(this.parent.PixelsToUnits());
			this.parent.transform.position = vector33;
			this.lastPosition = vector31;
		}
		base.OnMouseMove(args);
	}

	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		base.OnMouseUp(args);
		base.Parent.MakePixelPerfect(true);
	}

	public override void Start()
	{
		base.Start();
		if (base.Size.magnitude <= 1.401298E-45f)
		{
			if (base.Parent == null)
			{
				base.Size = new Vector2(200f, 25f);
			}
			else
			{
				base.Size = new Vector2(base.Parent.Width, 30f);
				base.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Left | dfAnchorStyle.Right;
				base.RelativePosition = Vector2.zero;
			}
		}
	}
}