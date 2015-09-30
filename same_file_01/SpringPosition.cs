using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : IgnoreTimeScale
{
	public Vector3 target = Vector3.zero;

	public float strength = 10f;

	public bool worldSpace;

	public bool ignoreTimeScale;

	private Transform mTrans;

	private float mThreshold;

	public SpringPosition()
	{
	}

	public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPosition component = go.GetComponent<SpringPosition>();
		if (component == null)
		{
			component = go.AddComponent<SpringPosition>();
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
		this.mTrans = base.transform;
	}

	private void Update()
	{
		float single = (!this.ignoreTimeScale ? Time.deltaTime : base.UpdateRealTimeDelta());
		if (!this.worldSpace)
		{
			if (this.mThreshold == 0f)
			{
				Vector3 vector3 = this.target - this.mTrans.localPosition;
				this.mThreshold = vector3.magnitude * 0.001f;
			}
			this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, single);
			if (this.mThreshold >= (this.target - this.mTrans.localPosition).magnitude)
			{
				this.mTrans.localPosition = this.target;
				base.enabled = false;
			}
		}
		else
		{
			if (this.mThreshold == 0f)
			{
				Vector3 vector31 = this.target - this.mTrans.position;
				this.mThreshold = vector31.magnitude * 0.001f;
			}
			this.mTrans.position = NGUIMath.SpringLerp(this.mTrans.position, this.target, this.strength, single);
			if (this.mThreshold >= (this.target - this.mTrans.position).magnitude)
			{
				this.mTrans.position = this.target;
				base.enabled = false;
			}
		}
	}
}