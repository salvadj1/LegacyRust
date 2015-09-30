using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Color")]
public class TweenColor : UITweener
{
	public Color @from = Color.white;

	public Color to = Color.white;

	[NonSerialized]
	public bool isFullscreen;

	private Transform mTrans;

	private UIWidget mWidget;

	private Material mMat;

	private Light mLight;

	public Color color
	{
		get
		{
			if (this.mWidget != null)
			{
				return this.mWidget.color;
			}
			if (this.mLight != null)
			{
				return this.mLight.color;
			}
			if (this.mMat == null)
			{
				return Color.black;
			}
			return this.mMat.color;
		}
		set
		{
			if (this.mWidget != null)
			{
				this.mWidget.color = value;
			}
			if (this.mMat != null)
			{
				this.mMat.color = value;
			}
			if (this.mLight != null)
			{
				this.mLight.color = value;
				this.mLight.enabled = value.r + value.g + value.b > 0.01f;
			}
		}
	}

	public TweenColor()
	{
	}

	private void Awake()
	{
		this.mWidget = base.GetComponentInChildren<UIWidget>();
		Renderer renderer = base.renderer;
		if (renderer != null)
		{
			this.mMat = renderer.material;
		}
		this.mLight = base.light;
	}

	public static TweenColor Begin(GameObject go, float duration, Color color)
	{
		TweenColor tweenColor = UITweener.Begin<TweenColor>(go, duration);
		tweenColor.@from = tweenColor.color;
		tweenColor.to = color;
		return tweenColor;
	}

	protected override void OnUpdate(float factor)
	{
		Color color = (this.@from * (1f - factor)) + (this.to * factor);
		if (this.isFullscreen)
		{
			GameFullscreen instance = ImageEffectManager.GetInstance<GameFullscreen>();
			if (instance)
			{
				instance.autoFadeColor = color;
			}
			color.a = 0f;
		}
		this.color = color;
	}
}