using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);

	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);

	public float duration = 0.2f;

	private Vector3 mScale;

	private bool mInitDone;

	private bool mStarted;

	private bool mHighlighted;

	public UIButtonScale()
	{
	}

	private void Init()
	{
		this.mInitDone = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.transform;
		}
		this.mScale = this.tweenTarget.localScale;
	}

	private void OnDisable()
	{
		if (this.tweenTarget != null)
		{
			TweenScale component = this.tweenTarget.GetComponent<TweenScale>();
			if (component != null)
			{
				component.scale = this.mScale;
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
			TweenScale.Begin(this.tweenTarget.gameObject, this.duration, (!isOver ? this.mScale : Vector3.Scale(this.mScale, this.hover))).method = UITweener.Method.EaseInOut;
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
				vector3 = (!UICamera.IsHighlighted(base.gameObject) ? this.mScale : Vector3.Scale(this.mScale, this.hover));
			}
			else
			{
				vector3 = Vector3.Scale(this.mScale, this.pressed);
			}
			TweenScale.Begin(gameObject, single, vector3).method = UITweener.Method.EaseInOut;
		}
	}

	private void Start()
	{
		this.mStarted = true;
	}
}