using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Color")]
public class UIButtonColor : MonoBehaviour
{
	public GameObject tweenTarget;

	public Color hover = new Color(0.6f, 1f, 0.2f, 1f);

	public Color pressed = Color.grey;

	public float duration = 0.2f;

	protected Color mColor;

	protected bool mInitDone;

	protected bool mStarted;

	protected bool mHighlighted;

	public Color defaultColor
	{
		get
		{
			return this.mColor;
		}
		set
		{
			this.mColor = value;
		}
	}

	public UIButtonColor()
	{
	}

	protected void Init()
	{
		this.mInitDone = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
		}
		UIWidget component = this.tweenTarget.GetComponent<UIWidget>();
		if (component == null)
		{
			Renderer renderer = this.tweenTarget.renderer;
			if (renderer == null)
			{
				Light light = this.tweenTarget.light;
				if (light == null)
				{
					Debug.LogWarning(string.Concat(NGUITools.GetHierarchy(base.gameObject), " has nothing for UIButtonColor to color"), this);
					base.enabled = false;
				}
				else
				{
					this.mColor = light.color;
				}
			}
			else
			{
				this.mColor = renderer.material.color;
			}
		}
		else
		{
			this.mColor = component.color;
		}
	}

	private void OnDisable()
	{
		if (this.tweenTarget != null)
		{
			TweenColor component = this.tweenTarget.GetComponent<TweenColor>();
			if (component != null)
			{
				component.color = this.mColor;
				component.enabled = false;
			}
		}
	}

	protected virtual void OnEnable()
	{
		if (this.mStarted && this.mHighlighted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	protected virtual void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mInitDone)
			{
				this.Init();
			}
			TweenColor.Begin(this.tweenTarget, this.duration, (!isOver ? this.mColor : this.hover));
			this.mHighlighted = isOver;
		}
	}

	protected virtual void OnPress(bool isPressed)
	{
		Color color;
		if (!this.mInitDone)
		{
			this.Init();
		}
		if (base.enabled)
		{
			GameObject gameObject = this.tweenTarget;
			float single = this.duration;
			if (!isPressed)
			{
				color = (!UICamera.IsHighlighted(base.gameObject) ? this.mColor : this.hover);
			}
			else
			{
				color = this.pressed;
			}
			TweenColor.Begin(gameObject, single, color);
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (!this.mInitDone)
		{
			this.Init();
		}
	}
}