using System;
using System.Text;
using UnityEngine;

public class TextEntryRestrictions : MonoBehaviour
{
	public string allowedChars = "0123456789";

	public TextEntryRestrictions()
	{
	}

	public void OnKeyDown(dfControl control, dfKeyEventArgs keyEvent)
	{
		if (char.IsControl(keyEvent.Character))
		{
			return;
		}
		if (this.allowedChars.IndexOf(keyEvent.Character) == -1)
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
		if (this.allowedChars.IndexOf(keyEvent.Character) == -1)
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
			if (this.allowedChars.IndexOf(value[i]) == -1)
			{
				cursorIndex = Mathf.Max(0, cursorIndex + 1);
			}
			else
			{
				stringBuilder.Append(value[i]);
			}
		}
		control.Text = stringBuilder.ToString();
		control.CursorIndex = cursorIndex;
	}
}