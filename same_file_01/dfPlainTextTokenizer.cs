using System;
using System.Collections.Generic;

public class dfPlainTextTokenizer
{
	private static dfPlainTextTokenizer singleton;

	private List<dfMarkupToken> tokens = new List<dfMarkupToken>();

	public dfPlainTextTokenizer()
	{
	}

	private List<dfMarkupToken> tokenize(string source)
	{
		dfMarkupToken.Reset();
		dfMarkupTokenAttribute.Reset();
		this.tokens.Clear();
		int num = 0;
		int num1 = 0;
		int length = source.Length;
		while (num < length)
		{
			if (source[num] != '\r')
			{
				while (num < length && !char.IsWhiteSpace(source[num]))
				{
					num++;
				}
				if (num > num1)
				{
					this.tokens.Add(dfMarkupToken.Obtain(source, dfMarkupTokenType.Text, num1, num - 1));
					num1 = num;
				}
				if (num < length && source[num] == '\n')
				{
					this.tokens.Add(dfMarkupToken.Obtain(source, dfMarkupTokenType.Newline, num, num));
					num++;
					num1 = num;
				}
				while (num < length && source[num] != '\n' && source[num] != '\r' && char.IsWhiteSpace(source[num]))
				{
					num++;
				}
				if (num <= num1)
				{
					continue;
				}
				this.tokens.Add(dfMarkupToken.Obtain(source, dfMarkupTokenType.Whitespace, num1, num - 1));
				num1 = num;
			}
			else
			{
				num++;
				num1 = num;
			}
		}
		return this.tokens;
	}

	public static List<dfMarkupToken> Tokenize(string source)
	{
		if (dfPlainTextTokenizer.singleton == null)
		{
			dfPlainTextTokenizer.singleton = new dfPlainTextTokenizer();
		}
		return dfPlainTextTokenizer.singleton.tokenize(source);
	}
}