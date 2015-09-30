using AnimationOrTween;
using System;
using UnityEngine;

public abstract class UITweener : IgnoreTimeScale
{
	public UITweener.Method method;

	public UITweener.Style style;

	public float delay;

	public float duration = 1f;

	public bool steeperCurves;

	public int tweenGroup;

	public GameObject eventReceiver;

	public string callWhenFinished;

	private float mStartTime;

	private float mDuration;

	private float mAmountPerDelta = 1f;

	private float mFactor;

	public float amountPerDelta
	{
		get
		{
			if (this.mDuration != this.duration)
			{
				this.mDuration = this.duration;
				this.mAmountPerDelta = Mathf.Abs((this.duration <= 0f ? 1000f : 1f / this.duration));
			}
			return this.mAmountPerDelta;
		}
	}

	public Direction direction
	{
		get
		{
			return (this.mAmountPerDelta >= 0f ? Direction.Forward : Direction.Reverse);
		}
	}

	public float factor
	{
		get
		{
			return this.mFactor;
		}
	}

	protected UITweener()
	{
	}

	[Obsolete("Use Tweener.Play instead")]
	public void Animate(bool forward)
	{
		this.Play(forward);
	}

	public static T Begin<T>(GameObject go, float duration)
	where T : UITweener
	{
		T component = go.GetComponent<T>();
		if (component == null)
		{
			component = go.AddComponent<T>();
		}
		component.duration = duration;
		component.mFactor = 0f;
		component.style = UITweener.Style.Once;
		component.enabled = true;
		return component;
	}

	protected abstract void OnUpdate(float factor);

	public void Play(bool forward)
	{
		this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		if (!forward)
		{
			this.mAmountPerDelta = -this.mAmountPerDelta;
		}
		base.enabled = true;
	}

	public void Reset()
	{
		this.mFactor = (this.mAmountPerDelta >= 0f ? 0f : 1f);
	}

	private void Start()
	{
		this.mStartTime = Time.time + this.delay;
		this.Update();
	}

	public void Toggle()
	{
		if (this.mFactor <= 0f)
		{
			this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		}
		else
		{
			this.mAmountPerDelta = -this.amountPerDelta;
		}
		base.enabled = true;
	}

	private void Update()
	{
		if (Time.time < this.mStartTime)
		{
			return;
		}
		float single = base.UpdateRealTimeDelta();
		UITweener uITweener = this;
		uITweener.mFactor = uITweener.mFactor + this.amountPerDelta * single;
		if (this.style == UITweener.Style.Loop)
		{
			if (this.mFactor > 1f)
			{
				UITweener uITweener1 = this;
				uITweener1.mFactor = uITweener1.mFactor - Mathf.Floor(this.mFactor);
			}
		}
		else if (this.style == UITweener.Style.PingPong)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
			else if (this.mFactor < 0f)
			{
				this.mFactor = -this.mFactor;
				UITweener uITweener2 = this;
				uITweener2.mFactor = uITweener2.mFactor - Mathf.Floor(this.mFactor);
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
		}
		float single1 = Mathf.Clamp01(this.mFactor);
		if (this.method == UITweener.Method.EaseIn)
		{
			single1 = 1f - Mathf.Sin(1.57079637f * (1f - single1));
			if (this.steeperCurves)
			{
				single1 = single1 * single1;
			}
		}
		else if (this.method == UITweener.Method.EaseOut)
		{
			single1 = Mathf.Sin(1.57079637f * single1);
			if (this.steeperCurves)
			{
				single1 = 1f - single1;
				single1 = 1f - single1 * single1;
			}
		}
		else if (this.method == UITweener.Method.EaseInOut)
		{
			single1 = single1 - Mathf.Sin(single1 * 6.28318548f) / 6.28318548f;
			if (this.steeperCurves)
			{
				single1 = single1 * 2f - 1f;
				float single2 = Mathf.Sign(single1);
				single1 = 1f - Mathf.Abs(single1);
				single1 = 1f - single1 * single1;
				single1 = single2 * single1 * 0.5f + 0.5f;
			}
		}
		this.OnUpdate(single1);
		if (this.style == UITweener.Style.Once && (this.mFactor > 1f || this.mFactor < 0f))
		{
			this.mFactor = Mathf.Clamp01(this.mFactor);
			if (!string.IsNullOrEmpty(this.callWhenFinished))
			{
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				if (this.mFactor == 1f && this.mAmountPerDelta > 0f || this.mFactor == 0f && this.mAmountPerDelta < 0f)
				{
					base.enabled = false;
				}
			}
			else
			{
				base.enabled = false;
			}
		}
	}

	public enum Method
	{
		Linear,
		EaseIn,
		EaseOut,
		EaseInOut
	}

	public enum Style
	{
		Once,
		Loop,
		PingPong
	}
}