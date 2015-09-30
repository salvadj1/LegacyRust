using AnimationOrTween;
using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Active Animation")]
[RequireComponent(typeof(Animation))]
public class ActiveAnimation : IgnoreTimeScale
{
	public GameObject eventReceiver;

	public string callWhenFinished;

	private Animation mAnim;

	private Direction mLastDirection;

	private Direction mDisableDirection;

	private bool mNotify;

	public ActiveAnimation()
	{
	}

	private void Play(string clipName, Direction playDirection)
	{
		if (this.mAnim != null)
		{
			this.mAnim.enabled = false;
			if (playDirection == Direction.Toggle)
			{
				playDirection = (this.mLastDirection == Direction.Forward ? Direction.Reverse : Direction.Forward);
			}
			if (string.IsNullOrEmpty(clipName))
			{
				if (!this.mAnim.isPlaying)
				{
					this.mAnim.Play();
				}
			}
			else if (!this.mAnim.IsPlaying(clipName))
			{
				this.mAnim.Play(clipName);
			}
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AnimationState current = (AnimationState)enumerator.Current;
					if (!string.IsNullOrEmpty(clipName) && !(current.name == clipName))
					{
						continue;
					}
					float single = Mathf.Abs(current.speed);
					current.speed = single * (float)playDirection;
					if (playDirection != Direction.Reverse || current.time != 0f)
					{
						if (playDirection != Direction.Forward || current.time != current.length)
						{
							continue;
						}
						current.time = 0f;
					}
					else
					{
						current.time = current.length;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			this.mLastDirection = playDirection;
			this.mNotify = true;
		}
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (!anim.gameObject.activeInHierarchy)
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(anim.gameObject, true);
		}
		ActiveAnimation component = anim.GetComponent<ActiveAnimation>();
		if (component == null)
		{
			component = anim.gameObject.AddComponent<ActiveAnimation>();
		}
		else
		{
			component.enabled = true;
		}
		component.mAnim = anim;
		component.mDisableDirection = (Direction)disableCondition;
		component.Play(clipName, playDirection);
		return component;
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	public static ActiveAnimation Play(Animation anim, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	public void Reset()
	{
		if (this.mAnim != null)
		{
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AnimationState current = (AnimationState)enumerator.Current;
					if (this.mLastDirection != Direction.Reverse)
					{
						if (this.mLastDirection != Direction.Forward)
						{
							continue;
						}
						current.time = 0f;
					}
					else
					{
						current.time = current.length;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
		}
	}

	private void Update()
	{
		float single = base.UpdateRealTimeDelta();
		if (single == 0f)
		{
			return;
		}
		if (this.mAnim != null)
		{
			bool flag = false;
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AnimationState current = (AnimationState)enumerator.Current;
					float single1 = current.speed * single;
					AnimationState animationState = current;
					animationState.time = animationState.time + single1;
					if (single1 < 0f)
					{
						if (current.time <= 0f)
						{
							current.time = 0f;
						}
						else
						{
							flag = true;
						}
					}
					else if (current.time >= current.length)
					{
						current.time = current.length;
					}
					else
					{
						flag = true;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			this.mAnim.enabled = true;
			this.mAnim.Sample();
			this.mAnim.enabled = false;
			if (flag)
			{
				return;
			}
			if (this.mNotify)
			{
				this.mNotify = false;
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				if (this.mDisableDirection != Direction.Toggle && this.mLastDirection == this.mDisableDirection)
				{
					NGUITools.SetActive(base.gameObject, false);
				}
			}
		}
		base.enabled = false;
	}
}