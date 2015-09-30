using System;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Spring Panel")]
[RequireComponent(typeof(UIPanel))]
public class SpringPanel : IgnoreTimeScale
{
	public Vector3 target = Vector3.zero;

	public float strength = 10f;

	private UIPanel mPanel;

	private Transform mTrans;

	private float mThreshold;

	private UIDraggablePanel mDrag;

	public SpringPanel()
	{
	}

	public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPanel component = go.GetComponent<SpringPanel>();
		if (component == null)
		{
			component = go.AddComponent<SpringPanel>();
		}
		component.target = pos;
		component.strength = strength;
		if (!component.enabled)
		{
			component.mThreshold = 0f;
			component.enabled = true;
		}
		return component;
	}

	private void Start()
	{
		this.mPanel = base.GetComponent<UIPanel>();
		this.mDrag = base.GetComponent<UIDraggablePanel>();
		this.mTrans = base.transform;
	}

	private void Update()
	{
		float single = base.UpdateRealTimeDelta();
		if (this.mThreshold == 0f)
		{
			Vector3 vector3 = this.target - this.mTrans.localPosition;
			this.mThreshold = vector3.magnitude * 0.005f;
		}
		Vector3 vector31 = this.mTrans.localPosition;
		this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, single);
		Vector3 vector32 = this.mTrans.localPosition - vector31;
		Vector4 vector4 = this.mPanel.clipRange;
		vector4.x = vector4.x - vector32.x;
		vector4.y = vector4.y - vector32.y;
		this.mPanel.clipRange = vector4;
		if (this.mDrag != null)
		{
			this.mDrag.UpdateScrollbars(false);
		}
		if (this.mThreshold >= (this.target - this.mTrans.localPosition).magnitude)
		{
			base.enabled = false;
		}
	}
}