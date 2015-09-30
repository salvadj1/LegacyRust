using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Language Selection")]
[RequireComponent(typeof(UIPopupList))]
public class LanguageSelection : MonoBehaviour
{
	private UIPopupList mList;

	public LanguageSelection()
	{
	}

	private void OnLanguageSelection(string language)
	{
		if (Localization.instance != null)
		{
			Localization.instance.currentLanguage = language;
		}
	}

	private void Start()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.UpdateList();
		this.mList.eventReceiver = base.gameObject;
		this.mList.functionName = "OnLanguageSelection";
	}

	private void UpdateList()
	{
		if (Localization.instance != null && Localization.instance.languages != null)
		{
			this.mList.items.Clear();
			int num = 0;
			int length = (int)Localization.instance.languages.Length;
			while (num < length)
			{
				TextAsset textAsset = Localization.instance.languages[num];
				if (textAsset != null)
				{
					this.mList.items.Add(textAsset.name);
				}
				num++;
			}
			this.mList.selection = Localization.instance.currentLanguage;
		}
	}
}