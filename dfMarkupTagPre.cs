using System;
using UnityEngine;

[dfMarkupTagInfo("pre")]
public class dfMarkupTagPre : dfMarkupTag
{
	public dfMarkupTagPre() : base("pre")
	{
	}

	public dfMarkupTagPre(dfMarkupTag original) : base(original)
	{
	}

	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		style = base.applyTextStyleAttributes(style);
		style.PreserveWhitespace = true;
		style.Preformatted = true;
		if (style.Align == dfMarkupTextAlign.Justify)
		{
			style.Align = dfMarkupTextAlign.Left;
		}
		dfMarkupBox _dfMarkupBox = null;
		if (style.BackgroundColor.a <= 0.1f)
		{
			_dfMarkupBox = new dfMarkupBox(this, dfMarkupDisplayType.block, style);
		}
		else
		{
			dfMarkupBoxSprite _dfMarkupBoxSprite = new dfMarkupBoxSprite(this, dfMarkupDisplayType.block, style);
			_dfMarkupBoxSprite.LoadImage(base.Owner.Atlas, base.Owner.BlankTextureSprite);
			_dfMarkupBoxSprite.Style.Color = style.BackgroundColor;
			_dfMarkupBox = _dfMarkupBoxSprite;
		}
		dfMarkupAttribute _dfMarkupAttribute = base.findAttribute(new string[] { "margin" });
		if (_dfMarkupAttribute != null)
		{
			_dfMarkupBox.Margins = dfMarkupBorders.Parse(_dfMarkupAttribute.Value);
		}
		dfMarkupAttribute _dfMarkupAttribute1 = base.findAttribute(new string[] { "padding" });
		if (_dfMarkupAttribute1 != null)
		{
			_dfMarkupBox.Padding = dfMarkupBorders.Parse(_dfMarkupAttribute1.Value);
		}
		container.AddChild(_dfMarkupBox);
		base._PerformLayoutImpl(_dfMarkupBox, style);
		_dfMarkupBox.FitToContents(false);
	}
}