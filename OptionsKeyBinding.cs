using System;
using UnityEngine;

public class OptionsKeyBinding : MonoBehaviour
{
	public dfRichTextLabel keyOne;

	public dfRichTextLabel keyTwo;

	public dfRichTextLabel labelName;

	protected static dfRichTextLabel doingKeyListen;

	protected static string strPreviousValue;

	static OptionsKeyBinding()
	{
		OptionsKeyBinding.strPreviousValue = string.Empty;
	}

	public OptionsKeyBinding()
	{
	}

	private KeyCode FetchKey()
	{
		for (int i = 0; i < 429; i++)
		{
			if (Input.GetKey((KeyCode)i))
			{
				return (KeyCode)i;
			}
		}
		return KeyCode.None;
	}

	public void OnClickOne()
	{
		this.StartKeyListen(this.keyOne);
	}

	public void OnClickTwo()
	{
		this.StartKeyListen(this.keyTwo);
	}

	public void Setup(GameInput.GameButton button)
	{
		this.labelName.Text = button.Name;
		this.keyOne.Text = button.bindingOne.ToString();
		this.keyTwo.Text = button.bindingTwo.ToString();
		if (this.keyOne.Text == "None")
		{
			this.keyOne.Text = " ";
		}
		if (this.keyTwo.Text == "None")
		{
			this.keyTwo.Text = " ";
		}
	}

	private void StartKeyListen(dfRichTextLabel key)
	{
		if (OptionsKeyBinding.doingKeyListen != null)
		{
			return;
		}
		OptionsKeyBinding.strPreviousValue = key.Text;
		key.Text = "...";
		OptionsKeyBinding.doingKeyListen = key;
	}

	private void Update()
	{
		if (OptionsKeyBinding.doingKeyListen != this.keyOne && OptionsKeyBinding.doingKeyListen != this.keyTwo)
		{
			return;
		}
		if (!Input.anyKeyDown)
		{
			return;
		}
		KeyCode keyCode = this.FetchKey();
		if (keyCode == KeyCode.None)
		{
			return;
		}
		if (keyCode != KeyCode.Escape)
		{
			OptionsKeyBinding.doingKeyListen.Text = keyCode.ToString();
		}
		else
		{
			OptionsKeyBinding.doingKeyListen.Text = " ";
		}
		OptionsKeyBinding.doingKeyListen = null;
	}

	public void UpdateConVars()
	{
		string text = "None";
		string str = "None";
		if (this.keyOne.Text.Length > 0 && this.keyOne.Text != " ")
		{
			text = this.keyOne.Text;
		}
		if (this.keyTwo.Text.Length > 0 && this.keyTwo.Text != " ")
		{
			str = this.keyTwo.Text;
		}
		ConsoleSystem.Run(string.Concat(new string[] { "input.bind ", this.labelName.Text, " ", text, " ", str, string.Empty }), false);
	}
}