using System;
using UnityEngine;

public class UIActionProgress : MonoBehaviour
{
	[SerializeField]
	private UILabel _label;

	[SerializeField]
	private UISlider _slider;

	private UISprite[] sliderSprites;

	public UILabel label
	{
		get
		{
			return this._label;
		}
	}

	public float progress
	{
		get
		{
			return this.slider.sliderValue;
		}
		set
		{
			this.slider.sliderValue = value;
		}
	}

	public UISlider slider
	{
		get
		{
			return this._slider;
		}
	}

	public string text
	{
		get
		{
			return this.label.text;
		}
		set
		{
			this.label.text = value;
		}
	}

	public UIActionProgress()
	{
	}

	private void Awake()
	{
		this.sliderSprites = this._slider.GetComponentsInChildren<UISprite>();
	}

	private void OnDisable()
	{
		this.SetEnabled(false);
	}

	private void OnEnable()
	{
		this.SetEnabled(true);
	}

	private void SetEnabled(bool yes)
	{
		if (this._slider)
		{
			this._slider.enabled = yes;
		}
		if (this._label)
		{
			this._label.enabled = yes;
		}
		if (this.sliderSprites != null)
		{
			UISprite[] uISpriteArray = this.sliderSprites;
			for (int i = 0; i < (int)uISpriteArray.Length; i++)
			{
				UISprite uISprite = uISpriteArray[i];
				if (uISprite)
				{
					uISprite.enabled = yes;
				}
			}
		}
	}
}