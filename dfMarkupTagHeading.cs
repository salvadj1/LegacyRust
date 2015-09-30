using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[dfMarkupTagInfo("h1")]
[dfMarkupTagInfo("h2")]
[dfMarkupTagInfo("h3")]
[dfMarkupTagInfo("h4")]
[dfMarkupTagInfo("h5")]
[dfMarkupTagInfo("h6")]
public class dfMarkupTagHeading : dfMarkupTag
{
	public dfMarkupTagHeading() : base("h1")
	{
	}

	public dfMarkupTagHeading(dfMarkupTag original) : base(original)
	{
	}

	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		dfMarkupBorders dfMarkupBorder = new dfMarkupBorders();
		dfMarkupStyle _dfMarkupStyle = this.applyDefaultStyles(style, ref dfMarkupBorder);
		_dfMarkupStyle = base.applyTextStyleAttributes(_dfMarkupStyle);
		dfMarkupAttribute _dfMarkupAttribute = base.findAttribute(new string[] { "margin" });
		if (_dfMarkupAttribute != null)
		{
			dfMarkupBorder = dfMarkupBorders.Parse(_dfMarkupAttribute.Value);
		}
		dfMarkupBox _dfMarkupBox = new dfMarkupBox(this, dfMarkupDisplayType.block, _dfMarkupStyle)
		{
			Margins = dfMarkupBorder
		};
		container.AddChild(_dfMarkupBox);
		base._PerformLayoutImpl(_dfMarkupBox, _dfMarkupStyle);
		_dfMarkupBox.FitToContents(false);
	}

	private dfMarkupStyle applyDefaultStyles(dfMarkupStyle style, ref dfMarkupBorders margins)
	{
		int num;
		float fontSize = 1f;
		float single = 1f;
		string tagName = base.TagName;
		if (tagName != null)
		{
			if (dfMarkupTagHeading.<>f__switch$mapE == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(6)
				{
					{ "h1", 0 },
					{ "h2", 1 },
					{ "h3", 2 },
					{ "h4", 3 },
					{ "h5", 4 },
					{ "h6", 5 }
				};
				dfMarkupTagHeading.<>f__switch$mapE = strs;
			}
			if (dfMarkupTagHeading.<>f__switch$mapE.TryGetValue(tagName, out num))
			{
				switch (num)
				{
					case 0:
					{
						single = 2f;
						fontSize = 0.65f;
						break;
					}
					case 1:
					{
						single = 1.5f;
						fontSize = 0.75f;
						break;
					}
					case 2:
					{
						single = 1.35f;
						fontSize = 0.85f;
						break;
					}
					case 3:
					{
						single = 1.15f;
						fontSize = 0f;
						break;
					}
					case 4:
					{
						single = 0.85f;
						fontSize = 1.5f;
						break;
					}
					case 5:
					{
						single = 0.75f;
						fontSize = 1.75f;
						break;
					}
				}
			}
		}
		style.FontSize = (int)((float)style.FontSize * single);
		style.FontStyle = FontStyle.Bold;
		style.Align = dfMarkupTextAlign.Left;
		fontSize = fontSize * (float)style.FontSize;
		int num1 = (int)fontSize;
		num = num1;
		margins.bottom = num1;
		margins.top = num;
		return style;
	}
}