using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Input (Saved)")]
public class UIInputSaved : UIInput
{
	public string playerPrefsField;

	public UIInputSaved()
	{
	}

	private void OnApplicationQuit()
	{
		if (!string.IsNullOrEmpty(this.playerPrefsField))
		{
			PlayerPrefs.SetString(this.playerPrefsField, base.text);
		}
	}

	private void Start()
	{
		base.Init();
		if (!string.IsNullOrEmpty(this.playerPrefsField) && PlayerPrefs.HasKey(this.playerPrefsField))
		{
			base.text = PlayerPrefs.GetString(this.playerPrefsField);
		}
	}
}