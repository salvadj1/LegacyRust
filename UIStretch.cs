using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Stretch")]
[ExecuteInEditMode]
public class UIStretch : MonoBehaviour
{
	public Camera uiCamera;

	public UIStretch.Style style;

	public Vector2 relativeSize = Vector2.one;

	private Transform mTrans;

	private UIRoot mRoot;

	public UIStretch()
	{
	}

	private void OnEnable()
	{
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
	}

	private void Update()
	{
		if (this.uiCamera != null && this.style != UIStretch.Style.None)
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			Rect rect = this.uiCamera.pixelRect;
			float single = rect.width;
			float single1 = rect.height;
			if (this.mRoot != null && !this.mRoot.automatic && single1 > 1f)
			{
				float single2 = (float)this.mRoot.manualHeight / single1;
				single = single * single2;
				single1 = single1 * single2;
			}
			Vector3 vector3 = this.mTrans.localScale;
			if (this.style != UIStretch.Style.BasedOnHeight)
			{
				if (this.style == UIStretch.Style.Both || this.style == UIStretch.Style.Horizontal)
				{
					vector3.x = this.relativeSize.x * single;
				}
				if (this.style == UIStretch.Style.Both || this.style == UIStretch.Style.Vertical)
				{
					vector3.y = this.relativeSize.y * single1;
				}
			}
			else
			{
				vector3.x = this.relativeSize.x * single1;
				vector3.y = this.relativeSize.y * single1;
			}
			if (this.mTrans.localScale != vector3)
			{
				this.mTrans.localScale = vector3;
			}
		}
	}

	public enum Style
	{
		None,
		Horizontal,
		Vertical,
		Both,
		BasedOnHeight
	}
}