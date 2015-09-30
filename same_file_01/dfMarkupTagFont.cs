using System;
using UnityEngine;

[dfMarkupTagInfo("font")]
public class dfMarkupTagFont : dfMarkupTag
{
	public dfMarkupTagFont() : base("font")
	{
	}

	public dfMarkupTagFont(dfMarkupTag original) : base(original)
	{
	}

	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		dfMarkupAttribute _dfMarkupAttribute = base.findAttribute(new string[] { "name", "face" });
		if (_dfMarkupAttribute != null)
		{
			style.Font = dfDynamicFont.FindByName(_dfMarkupAttribute.Value) ?? style.Font;
		}
		dfMarkupAttribute _dfMarkupAttribute1 = base.findAttribute(new string[] { "size", "font-size" });
		if (_dfMarkupAttribute1 != null)
		{
			style.FontSize = dfMarkupStyle.ParseSize(_dfMarkupAttribute1.Value, style.FontSize);
		}
		dfMarkupAttribute _dfMarkupAttribute2 = base.findAttribute(new string[] { "color" });
		if (_dfMarkupAttribute2 != null)
		{
			style.Color = dfMarkupStyle.ParseColor(_dfMarkupAttribute2.Value, Color.red);
			style.Color.a = style.Opacity;
		}
		dfMarkupAttribute _dfMarkupAttribute3 = base.findAttribute(new string[] { "style" });
		if (_dfMarkupAttribute3 != null)
		{
			style.FontStyle = dfMarkupStyle.ParseFontStyle(_dfMarkupAttribute3.Value, style.FontStyle);
		}
		base._PerformLayoutImpl(container, style);
	}
}