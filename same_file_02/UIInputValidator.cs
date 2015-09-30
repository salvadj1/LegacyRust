using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Input Validator")]
[RequireComponent(typeof(UIInput))]
public class UIInputValidator : MonoBehaviour
{
	public UIInputValidator.Validation logic;

	public UIInputValidator()
	{
	}

	private void Start()
	{
		base.GetComponent<UIInput>().validator = new UIInput.Validator(this.Validate);
	}

	private char Validate(string text, char ch)
	{
		if (this.logic == UIInputValidator.Validation.None || !base.enabled)
		{
			return ch;
		}
		if (this.logic == UIInputValidator.Validation.Integer || this.logic == UIInputValidator.Validation.IntegerPositive)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (this.logic != UIInputValidator.Validation.IntegerPositive && ch == '-' && text.Length == 0)
			{
				return ch;
			}
		}
		else if (this.logic == UIInputValidator.Validation.Float)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch == '-' && text.Length == 0)
			{
				return ch;
			}
			if (ch == '.' && !text.Contains("."))
			{
				return ch;
			}
		}
		else if (this.logic == UIInputValidator.Validation.Alphanumeric)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return ch;
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		else if (this.logic == UIInputValidator.Validation.Username)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return (char)(ch - 65 + 97);
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		else if (this.logic == UIInputValidator.Validation.Name)
		{
			char chr = (text.Length <= 0 ? ' ' : text[text.Length - 1]);
			if (ch >= 'a' && ch <= 'z')
			{
				if (chr != ' ')
				{
					return ch;
				}
				return (char)(ch - 97 + 65);
			}
			if (ch >= 'A' && ch <= 'Z')
			{
				if (chr == ' ' || chr == '\'')
				{
					return ch;
				}
				return (char)(ch - 65 + 97);
			}
			if (ch == '\'')
			{
				if (chr != ' ' && chr != '\'' && !text.Contains("'"))
				{
					return ch;
				}
			}
			else if (ch == ' ' && chr != ' ' && chr != '\'')
			{
				return ch;
			}
		}
		return '\0';
	}

	public enum Validation
	{
		None,
		Integer,
		Float,
		Alphanumeric,
		Username,
		Name,
		IntegerPositive
	}
}