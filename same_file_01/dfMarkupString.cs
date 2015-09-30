using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

public class dfMarkupString : dfMarkupElement
{
	private static StringBuilder buffer;

	private static Regex whitespacePattern;

	private static Queue<dfMarkupString> objectPool;

	private bool isWhitespace;

	public bool IsWhitespace
	{
		get
		{
			return this.isWhitespace;
		}
	}

	public string Text
	{
		get;
		private set;
	}

	static dfMarkupString()
	{
		dfMarkupString.buffer = new StringBuilder();
		dfMarkupString.whitespacePattern = new Regex("\\s+");
		dfMarkupString.objectPool = new Queue<dfMarkupString>();
	}

	public dfMarkupString(string text)
	{
		this.Text = this.processWhitespace(dfMarkupEntity.Replace(text));
		this.isWhitespace = dfMarkupString.whitespacePattern.IsMatch(this.Text);
	}

	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (style.Font == null)
		{
			return;
		}
		string str = (style.PreserveWhitespace || !this.isWhitespace ? this.Text : " ");
		dfMarkupBoxText _dfMarkupBoxText = dfMarkupBoxText.Obtain(this, dfMarkupDisplayType.inline, style);
		_dfMarkupBoxText.SetText(str);
		container.AddChild(_dfMarkupBoxText);
	}

	internal static dfMarkupString Obtain(string text)
	{
		if (dfMarkupString.objectPool.Count <= 0)
		{
			return new dfMarkupString(text);
		}
		dfMarkupString _dfMarkupString = dfMarkupString.objectPool.Dequeue();
		_dfMarkupString.Text = dfMarkupEntity.Replace(text);
		_dfMarkupString.isWhitespace = dfMarkupString.whitespacePattern.IsMatch(_dfMarkupString.Text);
		return _dfMarkupString;
	}

	private string processWhitespace(string text)
	{
		dfMarkupString.buffer.Length = 0;
		dfMarkupString.buffer.Append(text);
		dfMarkupString.buffer.Replace("\r\n", "\n");
		dfMarkupString.buffer.Replace("\r", "\n");
		dfMarkupString.buffer.Replace("\t", "    ");
		return dfMarkupString.buffer.ToString();
	}

	internal override void Release()
	{
		base.Release();
		dfMarkupString.objectPool.Enqueue(this);
	}

	internal dfMarkupElement SplitWords()
	{
		dfMarkupTagSpan _dfMarkupTagSpan = dfMarkupTagSpan.Obtain();
		int num = 0;
		int num1 = 0;
		int length = this.Text.Length;
		while (num < length)
		{
			while (num < length && !char.IsWhiteSpace(this.Text[num]))
			{
				num++;
			}
			if (num > num1)
			{
				_dfMarkupTagSpan.AddChildNode(dfMarkupString.Obtain(this.Text.Substring(num1, num - num1)));
				num1 = num;
			}
			while (num < length && this.Text[num] != '\n' && char.IsWhiteSpace(this.Text[num]))
			{
				num++;
			}
			if (num > num1)
			{
				_dfMarkupTagSpan.AddChildNode(dfMarkupString.Obtain(this.Text.Substring(num1, num - num1)));
				num1 = num;
			}
			if (num >= length || this.Text[num] != '\n')
			{
				continue;
			}
			_dfMarkupTagSpan.AddChildNode(dfMarkupString.Obtain("\n"));
			int num2 = num + 1;
			num = num2;
			num1 = num2;
		}
		return _dfMarkupTagSpan;
	}

	public override string ToString()
	{
		return this.Text;
	}
}