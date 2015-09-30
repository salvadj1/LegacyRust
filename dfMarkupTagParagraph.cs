using System;
using System.Collections.Generic;
using UnityEngine;

[dfMarkupTagInfo("p")]
public class dfMarkupTagParagraph : dfMarkupTag
{
	public dfMarkupTagParagraph() : base("p")
	{
	}

	public dfMarkupTagParagraph(dfMarkupTag original) : base(original)
	{
	}

	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (base.ChildNodes.Count == 0)
		{
			return;
		}
		style = base.applyTextStyleAttributes(style);
		int num = (container.Children.Count != 0 ? style.LineHeight : 0);
		dfMarkupBox _dfMarkupBox = null;
		if (style.BackgroundColor.a <= 0.005f)
		{
			_dfMarkupBox = new dfMarkupBox(this, dfMarkupDisplayType.block, style);
		}
		else
		{
			dfMarkupBoxSprite _dfMarkupBoxSprite = new dfMarkupBoxSprite(this, dfMarkupDisplayType.block, style)
			{
				Atlas = base.Owner.Atlas,
				Source = base.Owner.BlankTextureSprite
			};
			_dfMarkupBoxSprite.Style.Color = style.BackgroundColor;
			_dfMarkupBox = _dfMarkupBoxSprite;
		}
		_dfMarkupBox.Margins = new dfMarkupBorders(0, 0, num, style.LineHeight);
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
		if (_dfMarkupBox.Children.Count > 0)
		{
			_dfMarkupBox.Children[_dfMarkupBox.Children.Count - 1].IsNewline = true;
		}
		_dfMarkupBox.FitToContents(true);
	}
}