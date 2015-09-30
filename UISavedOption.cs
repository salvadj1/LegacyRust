using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
	public string keyName;

	private string key
	{
		get
		{
			return (!string.IsNullOrEmpty(this.keyName) ? this.keyName : string.Concat("NGUI State: ", base.name));
		}
	}

	public UISavedOption()
	{
	}

	private void OnDisable()
	{
		this.Save(null);
	}

	private void OnEnable()
	{
		string str = PlayerPrefs.GetString(this.key);
		if (!string.IsNullOrEmpty(str))
		{
			UICheckbox component = base.GetComponent<UICheckbox>();
			if (component == null)
			{
				UICheckbox[] componentsInChildren = base.GetComponentsInChildren<UICheckbox>();
				int num = 0;
				int length = (int)componentsInChildren.Length;
				while (num < length)
				{
					UICheckbox uICheckbox = componentsInChildren[num];
					UIEventListener.Get(uICheckbox.gameObject).onClick -= new UIEventListener.VoidDelegate(this.Save);
					uICheckbox.isChecked = uICheckbox.name == str;
					Debug.Log(str);
					UIEventListener.Get(uICheckbox.gameObject).onClick += new UIEventListener.VoidDelegate(this.Save);
					num++;
				}
			}
			else
			{
				component.isChecked = str == "true";
			}
		}
	}

	private void Save(GameObject go)
	{
		UICheckbox component = base.GetComponent<UICheckbox>();
		if (component == null)
		{
			UICheckbox[] componentsInChildren = base.GetComponentsInChildren<UICheckbox>();
			int num = 0;
			int length = (int)componentsInChildren.Length;
			while (num < length)
			{
				UICheckbox uICheckbox = componentsInChildren[num];
				if (!uICheckbox.isChecked)
				{
					num++;
				}
				else
				{
					PlayerPrefs.SetString(this.key, uICheckbox.name);
					break;
				}
			}
		}
		else
		{
			PlayerPrefs.SetString(this.key, (!component.isChecked ? "false" : "true"));
		}
	}
}