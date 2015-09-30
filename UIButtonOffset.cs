using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = Vector3.zero;

	public Vector3 pressed = new Vector3(2f, -2f);

	public float duration = 0.2f;

	private Vector3 mPos;

	private bool mInitDone;

	private bool mStarted;

	private bool mHighlighted;

	public UIButtonOffset()
	{
	}

	private void Init()
	{
		this.mInitDone = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.transform;
		}
		this.mPos = this.tweenTarget.localPosition;
	}

	private void OnDisable()
	{
		if (this.tweenTarget != null)
		{
			TweenPosition component = this.tweenTarget.GetComponent<TweenPosition>();
			if (component != null)
			{
				component.position = this.mPos;
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
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, (!isOver ? this.mPos : this.mPos + this.hover)).method = UITweener.Method.EaseInOut;
			this.mHighlighted = isOver;
		}
	}

	private void OnPress(bool isPressed)
	{
		Vector3 vector3;
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
				vector3 = (!UICamera.IsHighlighted(base.gameObject) ? this.mPos : this.mPos + this.hover);
			}
			else
			{
				vector3 = this.mPos + this.pressed;
			}
			TweenPosition.Begin(gameObject, single, vector3).method = UITweener.Method.EaseInOut;
		}
	}

	private void Start()
	{
		this.mStarted = true;
	}
}