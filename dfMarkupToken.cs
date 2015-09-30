using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

public class dfMarkupToken
{
	private static List<dfMarkupToken> pool;

	private static int poolIndex;

	private string @value;

	private int startAttributeIndex;

	public int AttributeCount
	{
		get;
		private set;
	}

	public int EndOffset
	{
		get;
		private set;
	}

	public int Height
	{
		get;
		set;
	}

	public char this[int index]
	{
		get
		{
			if (index < 0 || this.StartOffset + index > this.Source.Length - 1)
			{
				return '\0';
			}
			return this.Source[this.StartOffset + index];
		}
	}

	public int Length
	{
		get
		{
			return this.EndOffset - this.StartOffset + 1;
		}
	}

	public string Source
	{
		get;
		private set;
	}

	public int StartOffset
	{
		get;
		private set;
	}

	public dfMarkupTokenType TokenType
	{
		get;
		private set;
	}

	public string Value
	{
		get
		{
			if (this.@value == null)
			{
				int num = Math.Min(this.EndOffset - this.StartOffset + 1, this.Source.Length - this.StartOffset);
				this.@value = this.Source.Substring(this.StartOffset, num);
			}
			return this.@value;
		}
	}

	public int Width
	{
		get;
		internal set;
	}

	static dfMarkupToken()
	{
		dfMarkupToken.pool = new List<dfMarkupToken>();
		dfMarkupToken.poolIndex = 0;
	}

	protected dfMarkupToken()
	{
	}

	internal void AddAttribute(dfMarkupToken key, dfMarkupToken value)
	{
		dfMarkupTokenAttribute _dfMarkupTokenAttribute = dfMarkupTokenAttribute.Obtain(key, value);
		if (this.AttributeCount == 0)
		{
			this.startAttributeIndex = _dfMarkupTokenAttribute.Index;
		}
		dfMarkupToken attributeCount = this;
		attributeCount.AttributeCount = attributeCount.AttributeCount + 1;
	}

	public dfMarkupTokenAttribute GetAttribute(int index)
	{
		if (index >= this.AttributeCount)
		{
			return null;
		}
		return dfMarkupTokenAttribute.GetAttribute(this.startAttributeIndex + index);
	}

	public bool Matches(string text)
	{
		if (this.Length != text.Length)
		{
			return false;
		}
		int length = text.Length;
		for (int i = 0; i < length; i++)
		{
			if (char.ToLowerInvariant(text[i]) != char.ToLowerInvariant(this[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static dfMarkupToken Obtain(string source, dfMarkupTokenType type, int startIndex, int endIndex)
	{
		if (dfMarkupToken.poolIndex >= dfMarkupToken.pool.Count - 1)
		{
			dfMarkupToken.pool.Add(new dfMarkupToken());
		}
		List<dfMarkupToken> dfMarkupTokens = dfMarkupToken.pool;
		int num = dfMarkupToken.poolIndex;
		dfMarkupToken.poolIndex = num + 1;
		dfMarkupToken item = dfMarkupTokens[num];
		item.Source = source;
		item.TokenType = type;
		item.@value = null;
		item.StartOffset = startIndex;
		item.EndOffset = endIndex;
		item.AttributeCount = 0;
		item.startAttributeIndex = 0;
		item.Width = 0;
		item.Height = 0;
		return item;
	}

	public static void Reset()
	{
		dfMarkupToken.poolIndex = 0;
	}

	public override string ToString()
	{
		return this.ToString();
	}
}