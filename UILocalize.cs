using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Localize")]
[RequireComponent(typeof(UIWidget))]
public class UILocalize : MonoBehaviour
{
	public string key;

	private string mLanguage;

	private bool mStarted;

	public UILocalize()
	{
	}

	private void OnEnable()
	{
		if (this.mStarted && Localization.instance != null)
		{
			this.OnLocalize(Localization.instance);
		}
	}

	private void OnLocalize(Localization loc)
	{
		if (this.mLanguage != loc.currentLanguage)
		{
			UIWidget component = base.GetComponent<UIWidget>();
			UILabel uILabel = component as UILabel;
			UISprite uISprite = component as UISprite;
			if (string.IsNullOrEmpty(this.mLanguage) && string.IsNullOrEmpty(this.key) && uILabel != null)
			{
				this.key = uILabel.text;
			}
			string str = (!string.IsNullOrEmpty(this.key) ? loc.Get(this.key) : loc.Get(component.name));
			if (uILabel != null)
			{
				uILabel.text = str;
			}
			else if (uISprite != null)
			{
				uISprite.spriteName = str;
				uISprite.MakePixelPerfect();
			}
			this.mLanguage = loc.currentLanguage;
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (Localization.instance != null)
		{
			this.OnLocalize(Localization.instance);
		}
	}
}