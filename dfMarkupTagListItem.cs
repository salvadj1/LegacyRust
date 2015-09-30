using System;
using System.Collections.Generic;
using UnityEngine;

[dfMarkupTagInfo("li")]
public class dfMarkupTagListItem : dfMarkupTag
{
	public dfMarkupTagListItem() : base("li")
	{
	}

	public dfMarkupTagListItem(dfMarkupTag original) : base(original)
	{
	}

	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (base.ChildNodes.Count == 0)
		{
			return;
		}
		float size = container.Size.x;
		dfMarkupBox _dfMarkupBox = new dfMarkupBox(this, dfMarkupDisplayType.listItem, style);
		_dfMarkupBox.Margins.top = 10;
		container.AddChild(_dfMarkupBox);
		dfMarkupTagList parent = base.Parent as dfMarkupTagList;
		if (parent == null)
		{
			base._PerformLayoutImpl(container, style);
			return;
		}
		style.VerticalAlign = dfMarkupVerticalAlign.Baseline;
		string str = "•";
		if (parent.TagName == "ol")
		{
			str = string.Concat(container.Children.Count, ".");
		}
		dfMarkupStyle _dfMarkupStyle = style;
		_dfMarkupStyle.VerticalAlign = dfMarkupVerticalAlign.Baseline;
		_dfMarkupStyle.Align = dfMarkupTextAlign.Right;
		dfMarkupBoxText bulletWidth = dfMarkupBoxText.Obtain(this, dfMarkupDisplayType.inlineBlock, _dfMarkupStyle);
		bulletWidth.SetText(str);
		bulletWidth.Width = parent.BulletWidth;
		bulletWidth.Margins.left = style.FontSize * 2;
		_dfMarkupBox.AddChild(bulletWidth);
		dfMarkupBox vector2 = new dfMarkupBox(this, dfMarkupDisplayType.inlineBlock, style);
		int fontSize = style.FontSize;
		float single = size - bulletWidth.Size.x - (float)bulletWidth.Margins.left - (float)fontSize;
		vector2.Size = new Vector2(single, (float)fontSize);
		vector2.Margins.left = (int)((float)style.FontSize * 0.5f);
		_dfMarkupBox.AddChild(vector2);
		for (int i = 0; i < base.ChildNodes.Count; i++)
		{
			base.ChildNodes[i].PerformLayout(vector2, style);
		}
		vector2.FitToContents(false);
		vector2.Parent.FitToContents(false);
		_dfMarkupBox.FitToContents(false);
	}
}