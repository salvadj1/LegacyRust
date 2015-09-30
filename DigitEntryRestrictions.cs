using System;
using System.Text;
using UnityEngine;

public class DigitEntryRestrictions : MonoBehaviour
{
	public DigitEntryRestrictions()
	{
	}

	public void OnKeyDown(dfControl control, dfKeyEventArgs keyEvent)
	{
		if (char.IsControl(keyEvent.Character))
		{
			return;
		}
		if (!char.IsDigit(keyEvent.Character))
		{
			keyEvent.Use();
		}
	}

	public void OnKeyPress(dfControl control, dfKeyEventArgs keyEvent)
	{
		if (char.IsControl(keyEvent.Character))
		{
			return;
		}
		if (!char.IsDigit(keyEvent.Character))
		{
			keyEvent.Use();
		}
	}

	public void OnTextChanged(dfTextbox control, string value)
	{
		int cursorIndex = control.CursorIndex;
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < value.Length; i++)
		{
			if (char.IsDigit(value[i]))
			{
				stringBuilder.Append(value[i]);
			}
		}
		control.Text = stringBuilder.ToString();
	}
}