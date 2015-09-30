using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[dfMarkupTagInfo("ol")]
[dfMarkupTagInfo("ul")]
public class dfMarkupTagList : dfMarkupTag
{
	internal int BulletWidth
	{
		get;
		private set;
	}

	public dfMarkupTagList() : base("ul")
	{
	}

	public dfMarkupTagList(dfMarkupTag original) : base(original)
	{
	}

	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (base.ChildNodes.Count == 0)
		{
			return;
		}
		style.Align = dfMarkupTextAlign.Left;
		dfMarkupBox _dfMarkupBox = new dfMarkupBox(this, dfMarkupDisplayType.block, style);
		container.AddChild(_dfMarkupBox);
		this.calculateBulletWidth(style);
		for (int i = 0; i < base.ChildNodes.Count; i++)
		{
			dfMarkupTag item = base.ChildNodes[i] as dfMarkupTag;
			if (item != null && !(item.TagName != "li"))
			{
				item.PerformLayout(_dfMarkupBox, style);
			}
		}
		_dfMarkupBox.FitToContents(false);
	}

	private void calculateBulletWidth(dfMarkupStyle style)
	{
		if (base.TagName == "ul")
		{
			Vector2 vector2 = style.Font.MeasureText("•", style.FontSize, style.FontStyle);
			this.BulletWidth = Mathf.CeilToInt(vector2.x);
			return;
		}
		int num = 0;
		for (int i = 0; i < base.ChildNodes.Count; i++)
		{
			dfMarkupTag item = base.ChildNodes[i] as dfMarkupTag;
			if (item != null && item.TagName == "li")
			{
				num++;
			}
		}
		string str = string.Concat(new string('X', num.ToString().Length), ".");
		Vector2 vector21 = style.Font.MeasureText(str, style.FontSize, style.FontStyle);
		this.BulletWidth = Mathf.CeilToInt(vector21.x);
	}
}