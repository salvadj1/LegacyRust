using System;
using UnityEngine;

public class ConvarBinding : MonoBehaviour
{
	public string convarName;

	public bool useValuesNotNumbers;

	public ConvarBinding()
	{
	}

	public bool GetStringValueFromControl(out string value)
	{
		dfSlider component = base.GetComponent<dfSlider>();
		if (component != null)
		{
			value = component.Value.ToString();
			return true;
		}
		dfDropdown _dfDropdown = base.GetComponent<dfDropdown>();
		if (!_dfDropdown)
		{
			dfCheckbox _dfCheckbox = base.GetComponent<dfCheckbox>();
			if (!_dfCheckbox)
			{
				value = string.Empty;
				return false;
			}
			value = (!_dfCheckbox.IsChecked ? bool.FalseString : bool.TrueString);
			return true;
		}
		int selectedIndex = _dfDropdown.SelectedIndex;
		if (selectedIndex == -1)
		{
			value = string.Empty;
			return false;
		}
		if (!this.useValuesNotNumbers)
		{
			value = selectedIndex.ToString();
		}
		else
		{
			value = _dfDropdown.SelectedValue;
		}
		return true;
	}

	private void Start()
	{
		this.UpdateFromConVar();
	}

	public void UpdateConVars()
	{
		string str;
		if (!this.GetStringValueFromControl(out str))
		{
			return;
		}
		ConsoleSystem.Run(string.Concat(this.convarName, " \"", str, "\""), false);
	}

	public void UpdateFromConVar()
	{
		dfSlider component = base.GetComponent<dfSlider>();
		if (component != null)
		{
			component.Value = ConVar.GetFloat(this.convarName, component.Value);
		}
		dfDropdown _dfDropdown = base.GetComponent<dfDropdown>();
		if (_dfDropdown != null)
		{
			if (!this.useValuesNotNumbers)
			{
				int num = ConVar.GetInt(this.convarName, -1f);
				if (num != -1)
				{
					_dfDropdown.SelectedIndex = num;
				}
			}
			else
			{
				string str = ConVar.GetString(this.convarName, string.Empty);
				if (!string.IsNullOrEmpty(str))
				{
					int selectedIndex = _dfDropdown.SelectedIndex;
					_dfDropdown.SelectedValue = str;
					if (_dfDropdown.SelectedIndex == -1)
					{
						_dfDropdown.SelectedIndex = selectedIndex;
					}
				}
			}
		}
		dfCheckbox flag = base.GetComponent<dfCheckbox>();
		if (flag != null)
		{
			flag.IsChecked = ConVar.GetBool(this.convarName, flag.IsChecked);
		}
	}
}