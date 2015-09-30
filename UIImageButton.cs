using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Image Button")]
[ExecuteInEditMode]
public class UIImageButton : MonoBehaviour
{
	public UISprite target;

	public string normalSprite;

	public string hoverSprite;

	public string pressedSprite;

	public UIImageButton()
	{
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled && this.target != null)
		{
			this.target.spriteName = (!isOver ? this.normalSprite : this.hoverSprite);
			this.target.MakePixelPerfect();
		}
	}

	private void OnPress(bool pressed)
	{
		if (base.enabled && this.target != null)
		{
			this.target.spriteName = (!pressed ? this.normalSprite : this.pressedSprite);
			this.target.MakePixelPerfect();
		}
	}

	private void Start()
	{
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<UISprite>();
		}
	}
}