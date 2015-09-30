using AnimationOrTween;
using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Tween")]
public class UIButtonTween : MonoBehaviour
{
	public GameObject tweenTarget;

	public int tweenGroup;

	public Trigger trigger;

	public Direction playDirection = Direction.Forward;

	public bool resetOnPlay;

	public EnableCondition ifDisabledOnPlay;

	public DisableCondition disableWhenFinished;

	public bool includeChildren;

	public GameObject eventReceiver;

	public string callWhenFinished;

	private UITweener[] mTweens;

	private bool mStarted;

	private bool mHighlighted;

	public UIButtonTween()
	{
	}

	private void OnActivate(bool isActive)
	{
		if (base.enabled && (this.trigger == Trigger.OnActivate || this.trigger == Trigger.OnActivateTrue && isActive || this.trigger == Trigger.OnActivateFalse && !isActive))
		{
			this.Play(isActive);
		}
	}

	private void OnClick()
	{
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true);
		}
	}

	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true);
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
			if (this.trigger == Trigger.OnHover || this.trigger == Trigger.OnHoverTrue && isOver || this.trigger == Trigger.OnHoverFalse && !isOver)
			{
				this.Play(isOver);
			}
			this.mHighlighted = isOver;
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && (this.trigger == Trigger.OnPress || this.trigger == Trigger.OnPressTrue && isPressed || this.trigger == Trigger.OnPressFalse && !isPressed))
		{
			this.Play(isPressed);
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (this.trigger == Trigger.OnSelect || this.trigger == Trigger.OnSelectTrue && isSelected || this.trigger == Trigger.OnSelectFalse && !isSelected))
		{
			this.Play(true);
		}
	}

	public void Play(bool forward)
	{
		GameObject gameObject = (this.tweenTarget != null ? this.tweenTarget : base.gameObject);
		if (!gameObject.activeInHierarchy)
		{
			if (this.ifDisabledOnPlay != EnableCondition.EnableThenPlay)
			{
				return;
			}
			NGUITools.SetActive(gameObject, true);
		}
		this.mTweens = (!this.includeChildren ? gameObject.GetComponents<UITweener>() : gameObject.GetComponentsInChildren<UITweener>());
		if ((int)this.mTweens.Length != 0)
		{
			bool flag = false;
			if (this.playDirection == Direction.Reverse)
			{
				forward = !forward;
			}
			int num = 0;
			int length = (int)this.mTweens.Length;
			while (num < length)
			{
				UITweener uITweener = this.mTweens[num];
				if (uITweener.tweenGroup == this.tweenGroup)
				{
					if (!flag && !gameObject.activeInHierarchy)
					{
						flag = true;
						NGUITools.SetActive(gameObject, true);
					}
					if (this.playDirection != Direction.Toggle)
					{
						uITweener.Play(forward);
					}
					else
					{
						uITweener.Toggle();
					}
					if (this.resetOnPlay)
					{
						uITweener.Reset();
					}
					if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
					{
						uITweener.eventReceiver = this.eventReceiver;
						uITweener.callWhenFinished = this.callWhenFinished;
					}
				}
				num++;
			}
		}
		else if (this.disableWhenFinished != DisableCondition.DoNotDisable)
		{
			NGUITools.SetActive(this.tweenTarget, false);
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
		}
	}

	private void Update()
	{
		if (this.disableWhenFinished != DisableCondition.DoNotDisable && this.mTweens != null)
		{
			bool flag = true;
			bool flag1 = true;
			int num = 0;
			int length = (int)this.mTweens.Length;
			while (num < length)
			{
				UITweener uITweener = this.mTweens[num];
				if (!uITweener.enabled)
				{
					if ((int)uITweener.direction != (int)this.disableWhenFinished)
					{
						flag1 = false;
					}
					num++;
				}
				else
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				if (flag1)
				{
					NGUITools.SetActive(this.tweenTarget, false);
				}
				this.mTweens = null;
			}
		}
	}
}