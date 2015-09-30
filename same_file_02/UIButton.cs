using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	public Color disabledColor = Color.grey;

	public bool isEnabled
	{
		get
		{
			return NGUITools.GetAllowClick(this);
		}
		set
		{
			bool flag;
			bool allowClick = NGUITools.GetAllowClick(this, out flag);
			if (!flag)
			{
				return;
			}
			if (allowClick != value)
			{
				NGUITools.SetAllowClick(this, value);
				this.UpdateColor(value, false);
			}
		}
	}

	public UIButton()
	{
	}

	protected override void OnEnable()
	{
		if (!this.isEnabled)
		{
			this.UpdateColor(false, true);
		}
		else
		{
			base.OnEnable();
		}
	}

	protected override void OnHover(bool isOver)
	{
		if (this.isEnabled)
		{
			base.OnHover(isOver);
		}
	}

	protected override void OnPress(bool isPressed)
	{
		if (this.isEnabled)
		{
			base.OnPress(isPressed);
		}
	}

	public void UpdateColor(bool shouldBeEnabled, bool immediate)
	{
		if (this.tweenTarget)
		{
			if (!this.mInitDone)
			{
				base.Init();
			}
			Color color = (!shouldBeEnabled ? this.disabledColor : base.defaultColor);
			TweenColor tweenColor = TweenColor.Begin(this.tweenTarget, 0.15f, color);
			if (immediate)
			{
				tweenColor.color = color;
				tweenColor.enabled = false;
			}
		}
	}
}