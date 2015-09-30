using System;
using System.Collections.Generic;

public class dfRichTextTokenizer
{
	private static dfRichTextTokenizer singleton;

	private List<dfMarkupToken> tokens = new List<dfMarkupToken>();

	private string source;

	private int index;

	public dfRichTextTokenizer()
	{
	}

	private char Advance(int amount = 1)
	{
		dfRichTextTokenizer _dfRichTextTokenizer = this;
		_dfRichTextTokenizer.index = _dfRichTextTokenizer.index + amount;
		return this.Peek(0);
	}

	private bool AtTagPosition()
	{
		if (this.Peek(0) != '<')
		{
			return false;
		}
		char chr = this.Peek(1);
		if (chr != '/')
		{
			if (char.IsLetter(chr))
			{
				return true;
			}
			return false;
		}
		if (char.IsLetter(this.Peek(2)))
		{
			return true;
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
			if (chr == '>' || char.IsWhiteSpace(chr))
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
		if (this.Peek(0) == '>')
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
		if (this.Peek(0) != '<')
		{
			return null;
		}
		char chr = this.Peek(1);
		if (chr == '/')
		{
			return this.parseEndTag();
		}
		this.Advance(1);
		chr = this.Peek(0);
		if (!char.IsLetterOrDigit(chr))
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
		while (this.index < this.source.Length && this.Peek(0) != '>')
		{
			chr = this.Peek(0);
			if (!char.IsWhiteSpace(chr))
			{
				dfMarkupToken _dfMarkupToken1 = this.parseWord();
				if (_dfMarkupToken1 != null)
				{
					chr = this.Peek(0);
					if (chr == '=')
					{
						chr = this.Advance(1);
						dfMarkupToken _dfMarkupToken2 = null;
						_dfMarkupToken2 = (chr == '\"' || chr == '\'' ? this.parseQuotedString() : this.parseAttributeValue());
						_dfMarkupToken.AddAttribute(_dfMarkupToken1, _dfMarkupToken2 ?? _dfMarkupToken1);
					}
					else
					{
						_dfMarkupToken.AddAttribute(_dfMarkupToken1, _dfMarkupToken1);
					}
				}
				else
				{
					this.Advance(1);
				}
			}
			else
			{
				this.parseWhitespace();
			}
		}
		if (this.Peek(0) == '>')
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

	private List<dfMarkupToken> tokenize(string source)
	{
		dfMarkupToken.Reset();
		dfMarkupTokenAttribute.Reset();
		this.tokens.Clear();
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
		if (dfRichTextTokenizer.singleton == null)
		{
			dfRichTextTokenizer.singleton = new dfRichTextTokenizer();
		}
		return dfRichTextTokenizer.singleton.tokenize(source);
	}
}