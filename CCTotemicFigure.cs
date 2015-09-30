using System;
using UnityEngine;

[AddComponentMenu("")]
[RequireComponent(typeof(CCDesc))]
public sealed class CCTotemicFigure : CCTotem<CCTotem.TotemicFigure, CCTotemicFigure>
{
	public CCTotemicFigure()
	{
	}

	private void OnDrawGizmos()
	{
		if (this.totemicObject != null)
		{
			float bottomUpIndex = 3.14159274f * Time.time + 0.7853982f * (float)this.totemicObject.BottomUpIndex;
			Matrix4x4 matrix4x4 = Camera.current.cameraToWorldMatrix;
			Vector3 vector3 = matrix4x4.MultiplyVector(new Vector3(Mathf.Sin(bottomUpIndex) * 0.25f + 0.75f, 0f, 0f));
			Vector3 vector31 = -vector3;
			Gizmos.color = Color.green;
			Gizmos.DrawLine(this.totemicObject.TopOrigin, this.totemicObject.TopOrigin + vector3);
			Gizmos.DrawLine(this.totemicObject.SlideTopOrigin + vector3, this.totemicObject.TopOrigin + vector3);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(this.totemicObject.BottomOrigin, this.totemicObject.BottomOrigin + vector31);
			Gizmos.DrawLine(this.totemicObject.BottomOrigin + vector31, this.totemicObject.SlideBottomOrigin + vector31);
			Gizmos.color = ((this.totemicObject.BottomUpIndex & 1) != 1 ? Color.red : new Color(1f, 0.4f, 0.4f, 1f));
			Gizmos.DrawLine(this.totemicObject.SlideBottomOrigin + vector3, this.totemicObject.SlideBottomOrigin + vector31);
			Gizmos.DrawLine(this.totemicObject.SlideTopOrigin + vector3, this.totemicObject.SlideTopOrigin + vector31);
			Gizmos.DrawLine(this.totemicObject.SlideBottomOrigin + vector31, this.totemicObject.SlideTopOrigin + vector31);
			Gizmos.DrawLine(this.totemicObject.SlideBottomOrigin + vector3, this.totemicObject.SlideTopOrigin + vector3);
		}
	}
}