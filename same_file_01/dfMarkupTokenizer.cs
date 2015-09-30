using System;
using System.Collections.Generic;

public class dfMarkupTokenizer
{
	private static dfMarkupTokenizer singleton;

	private static List<string> validTags;

	private List<dfMarkupToken> tokens = new List<dfMarkupToken>();

	private string source;

	private int index;

	static dfMarkupTokenizer()
	{
		dfMarkupTokenizer.validTags = new List<string>()
		{
			"color",
			"sprite"
		};
	}

	public dfMarkupTokenizer()
	{
	}

	private char Advance(int amount = 1)
	{
		dfMarkupTokenizer _dfMarkupTokenizer = this;
		_dfMarkupTokenizer.index = _dfMarkupTokenizer.index + amount;
		return this.Peek(0);
	}

	private bool AtTagPosition()
	{
		if (this.Peek(0) != '[')
		{
			return false;
		}
		char chr = this.Peek(1);
		if (chr != '/')
		{
			if (!char.IsLetter(chr))
			{
				return false;
			}
			return this.isValidTag(this.index + 1, false);
		}
		if (!char.IsLetter(this.Peek(2)))
		{
			return false;
		}
		return this.isValidTag(this.index + 2, true);
	}

	private bool isValidTag(int index, bool endTag)
	{
		for (int i = 0; i < dfMarkupTokenizer.validTags.Count; i++)
		{
			string item = dfMarkupTokenizer.validTags[i];
			bool flag = true;
			int num = 0;
			while (num < item.Length - 1 && num + index < this.source.Length - 1)
			{
				if (!endTag && this.source[num + index] == ' ')
				{
					break;
				}
				else if (this.source[num + index] == ']')
				{
					break;
				}
				else if (char.ToLowerInvariant(item[num]) == char.ToLowerInvariant(this.source[num + index]))
				{
					num++;
				}
				else
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	private dfMarkupToken parseAttributeValue()
	{
		int num = this.index;
		int num1 = this.index;
		while (this.index < this.source.Length)
		{
			char chr = this.Advance(1);
			if (chr == ']' || char.IsWhiteSpace(chr))
			{
				break;
			}
			else
			{
				num1++;
			}
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Text, num, num1);
	}

	private dfMarkupToken parseEndTag()
	{
		this.Advance(2);
		int num = this.index;
		int num1 = this.index;
		while (this.index < this.source.Length && char.IsLetterOrDigit(this.Advance(1)))
		{
			num1++;
		}
		if (this.Peek(0) == ']')
		{
			this.Advance(1);
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.EndTag, num, num1);
	}

	private dfMarkupToken parseNonWhitespace()
	{
		int num = this.index;
		int num1 = this.index;
		while (this.index < this.source.Length)
		{
			if (char.IsWhiteSpace(this.Advance(1)) || this.AtTagPosition())
			{
				break;
			}
			else
			{
				num1++;
			}
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Text, num, num1);
	}

	private dfMarkupToken parseQuotedString()
	{
		char chr = this.Peek(0);
		if (chr != '\"' && chr != '\'')
		{
			return null;
		}
		this.Advance(1);
		int num = this.index;
		int num1 = this.index;
		while (this.index < this.source.Length && this.Advance(1) != chr)
		{
			num1++;
		}
		if (this.Peek(0) == chr)
		{
			this.Advance(1);
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Text, num, num1);
	}

	private dfMarkupToken parseTag()
	{
		if (this.Peek(0) != '[')
		{
			return null;
		}
		if (this.Peek(1) == '/')
		{
			return this.parseEndTag();
		}
		this.Advance(1);
		if (!char.IsLetterOrDigit(this.Peek(0)))
		{
			return null;
		}
		int num = this.index;
		int num1 = this.index;
		while (this.index < this.source.Length && char.IsLetterOrDigit(this.Advance(1)))
		{
			num1++;
		}
		dfMarkupToken _dfMarkupToken = dfMarkupToken.Obtain(this.source, dfMarkupTokenType.StartTag, num, num1);
		if (this.index < this.source.Length && this.Peek(0) != ']')
		{
			if (char.IsWhiteSpace(this.Peek(0)))
			{
				this.parseWhitespace();
			}
			int num2 = this.index;
			int num3 = this.index;
			if (this.Peek(0) != '\"')
			{
				while (this.index < this.source.Length && this.Advance(1) != ']')
				{
					num3++;
				}
				dfMarkupToken _dfMarkupToken1 = dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Text, num2, num3);
				_dfMarkupToken.AddAttribute(_dfMarkupToken1, _dfMarkupToken1);
			}
			else
			{
				dfMarkupToken _dfMarkupToken2 = this.parseQuotedString();
				_dfMarkupToken.AddAttribute(_dfMarkupToken2, _dfMarkupToken2);
			}
		}
		if (this.Peek(0) == ']')
		{
			this.Advance(1);
		}
		return _dfMarkupToken;
	}

	private dfMarkupToken parseWhitespace()
	{
		int num = this.index;
		int num1 = this.index;
		if (this.Peek(0) == '\n')
		{
			this.Advance(1);
			return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Newline, num, num);
		}
		while (this.index < this.source.Length)
		{
			char chr = this.Advance(1);
			if (chr == '\n' || chr == '\r' || !char.IsWhiteSpace(chr))
			{
				break;
			}
			else
			{
				num1++;
			}
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Whitespace, num, num1);
	}

	private dfMarkupToken parseWord()
	{
		if (!char.IsLetter(this.Peek(0)))
		{
			return null;
		}
		int num = this.index;
		int num1 = this.index;
		while (this.index < this.source.Length && char.IsLetter(this.Advance(1)))
		{
			num1++;
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Text, num, num1);
	}

	private char Peek(int offset = 0)
	{
		if (this.index + offset > this.source.Length - 1)
		{
			return '\0';
		}
		return this.source[this.index + offset];
	}

	private void reset()
	{
		dfMarkupToken.Reset();
		dfMarkupTokenAttribute.Reset();
		this.tokens.Clear();
	}

	private List<dfMarkupToken> tokenize(string source)
	{
		this.reset();
		this.source = source;
		this.index = 0;
		while (this.index < source.Length)
		{
			char chr = this.Peek(0);
			if (!this.AtTagPosition())
			{
				dfMarkupToken _dfMarkupToken = null;
				if (!char.IsWhiteSpace(chr))
				{
					_dfMarkupToken = this.parseNonWhitespace();
				}
				else if (chr != '\r')
				{
					_dfMarkupToken = this.parseWhitespace();
				}
				if (_dfMarkupToken != null)
				{
					this.tokens.Add(_dfMarkupToken);
				}
				else
				{
					this.Advance(1);
				}
			}
			else
			{
				dfMarkupToken _dfMarkupToken1 = this.parseTag();
				if (_dfMarkupToken1 != null)
				{
					this.tokens.Add(_dfMarkupToken1);
				}
			}
		}
		return this.tokens;
	}

	public static List<dfMarkupToken> Tokenize(string source)
	{
		if (dfMarkupTokenizer.singleton == null)
		{
			dfMarkupTokenizer.singleton = new dfMarkupTokenizer();
		}
		return dfMarkupTokenizer.singleton.tokenize(source);
	}
}