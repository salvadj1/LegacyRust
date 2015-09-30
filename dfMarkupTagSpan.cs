using System;
using System.Collections.Generic;

[dfMarkupTagInfo("span")]
public class dfMarkupTagSpan : dfMarkupTag
{
	private static Queue<dfMarkupTagSpan> objectPool;

	static dfMarkupTagSpan()
	{
		dfMarkupTagSpan.objectPool = new Queue<dfMarkupTagSpan>();
	}

	public dfMarkupTagSpan() : base("span")
	{
	}

	public dfMarkupTagSpan(dfMarkupTag original) : base(original)
	{
	}

	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		style = base.applyTextStyleAttributes(style);
		for (int i = 0; i < base.ChildNodes.Count; i++)
		{
			dfMarkupElement item = base.ChildNodes[i];
			if (!(item is dfMarkupString) || !((item as dfMarkupString).Text == "\n"))
			{
				item.PerformLayout(container, style);
			}
			else if (style.PreserveWhitespace)
			{
				container.AddLineBreak();
			}
		}
	}

	internal static dfMarkupTagSpan Obtain()
	{
		if (dfMarkupTagSpan.objectPool.Count <= 0)
		{
			return new dfMarkupTagSpan();
		}
		return dfMarkupTagSpan.objectPool.Dequeue();
	}

	internal override void Release()
	{
		base.Release();
		dfMarkupTagSpan.objectPool.Enqueue(this);
	}
}