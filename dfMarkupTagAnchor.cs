using System;
using System.Collections.Generic;

[dfMarkupTagInfo("a")]
public class dfMarkupTagAnchor : dfMarkupTag
{
	public string HRef
	{
		get
		{
			dfMarkupAttribute _dfMarkupAttribute = base.findAttribute(new string[] { "href" });
			return (_dfMarkupAttribute == null ? string.Empty : _dfMarkupAttribute.Value);
		}
	}

	public dfMarkupTagAnchor() : base("a")
	{
	}

	public dfMarkupTagAnchor(dfMarkupTag original) : base(original)
	{
	}

	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		style.TextDecoration = dfMarkupTextDecoration.Underline;
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
}