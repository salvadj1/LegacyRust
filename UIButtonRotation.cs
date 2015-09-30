using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = Vector3.zero;

	public Vector3 pressed = Vector3.zero;

	public float duration = 0.2f;

	private Quaternion mRot;

	private bool mInitDone;

	private bool mStarted;

	private bool mHighlighted;

	public UIButtonRotation()
	{
	}

	private void Init()
	{
		this.mInitDone = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.transform;
		}
		this.mRot = this.tweenTarget.localRotation;
	}

	private void OnDisable()
	{
		if (this.tweenTarget != null)
		{
			TweenRotation component = this.tweenTarget.GetComponent<TweenRotation>();
			if (component != null)
			{
				component.rotation = this.mRot;
				component.enabled = false;
			}
		}
	}

	private void OnEnable()
	{
		if (this.mStarted && this.mHighlighted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mInitDone)
			{
				this.Init();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, (!isOver ? this.mRot : this.mRot * Quaternion.Euler(this.hover))).method = UITweener.Method.EaseInOut;
			this.mHighlighted = isOver;
		}
	}

	private void OnPress(bool isPressed)
	{
		Quaternion quaternion;
		if (base.enabled)
		{
			if (!this.mInitDone)
			{
				this.Init();
			}
			GameObject gameObject = this.tweenTarget.gameObject;
			float single = this.duration;
			if (!isPressed)
			{
				quaternion = (!UICamera.IsHighlighted(base.gameObject) ? this.mRot : this.mRot * Quaternion.Euler(this.hover));
			}
			else
			{
				quaternion = this.mRot * Quaternion.Euler(this.pressed);
			}
			TweenRotation.Begin(gameObject, single, quaternion).method = UITweener.Method.EaseInOut;
		}
	}

	private void Start()
	{
		this.mStarted = true;
	}
}