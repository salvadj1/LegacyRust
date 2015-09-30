using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Anchor (Bordered)")]
[ExecuteInEditMode]
public class UIBorderedAnchor : UIAnchor
{
	public RectOffset screenPixelBorder;

	public UIBorderedAnchor()
	{
	}

	protected new void Update()
	{
		if (this.uiCamera)
		{
			Vector3 vector3 = UIAnchor.WorldOrigin(this.uiCamera, this.side, this.screenPixelBorder, this.depthOffset, this.relativeOffset.x, this.relativeOffset.y, this.halfPixelOffset);
			base.SetPosition(ref vector3);
		}
	}
}