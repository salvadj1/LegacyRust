using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class dfMarkupTag : dfMarkupElement
{
	private static int ELEMENTID;

	public List<dfMarkupAttribute> Attributes;

	private dfRichTextLabel owner;

	private string id;

	public string ID
	{
		get
		{
			return this.id;
		}
	}

	public virtual bool IsClosedTag
	{
		get;
		set;
	}

	public virtual bool IsEndTag
	{
		get;
		set;
	}

	public virtual bool IsInline
	{
		get;
		set;
	}

	public dfRichTextLabel Owner
	{
		get
		{
			return this.owner;
		}
		set
		{
			this.owner = value;
			for (int i = 0; i < base.ChildNodes.Count; i++)
			{
				dfMarkupTag item = base.ChildNodes[i] as dfMarkupTag;
				if (item != null)
				{
					item.Owner = value;
				}
			}
		}
	}

	public string TagName
	{
		get;
		set;
	}

	static dfMarkupTag()
	{
	}

	public dfMarkupTag(string tagName)
	{
		this.Attributes = new List<dfMarkupAttribute>();
		this.TagName = tagName;
		int eLEMENTID = dfMarkupTag.ELEMENTID;
		dfMarkupTag.ELEMENTID = eLEMENTID + 1;
		int num = eLEMENTID;
		this.id = string.Concat(tagName, num.ToString("X"));
	}

	public dfMarkupTag(dfMarkupTag original)
	{
		this.TagName = original.TagName;
		this.Attributes = original.Attributes;
		this.IsEndTag = original.IsEndTag;
		this.IsClosedTag = original.IsClosedTag;
		this.IsInline = original.IsInline;
		this.id = original.id;
		List<dfMarkupElement> childNodes = original.ChildNodes;
		for (int i = 0; i < childNodes.Count; i++)
		{
			base.AddChildNode(childNodes[i]);
		}
	}

	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (this.IsEndTag)
		{
			return;
		}
		for (int i = 0; i < base.ChildNodes.Count; i++)
		{
			base.ChildNodes[i].PerformLayout(container, style);
		}
	}

	protected dfMarkupStyle applyTextStyleAttributes(dfMarkupStyle style)
	{
		dfMarkupAttribute _dfMarkupAttribute = this.findAttribute(new string[] { "font", "font-family" });
		if (_dfMarkupAttribute != null)
		{
			style.Font = dfDynamicFont.FindByName(_dfMarkupAttribute.Value);
		}
		dfMarkupAttribute _dfMarkupAttribute1 = this.findAttribute(new string[] { "style", "font-style" });
		if (_dfMarkupAttribute1 != null)
		{
			style.FontStyle = dfMarkupStyle.ParseFontStyle(_dfMarkupAttribute1.Value, style.FontStyle);
		}
		dfMarkupAttribute _dfMarkupAttribute2 = this.findAttribute(new string[] { "size", "font-size" });
		if (_dfMarkupAttribute2 != null)
		{
			style.FontSize = dfMarkupStyle.ParseSize(_dfMarkupAttribute2.Value, style.FontSize);
		}
		dfMarkupAttribute _dfMarkupAttribute3 = this.findAttribute(new string[] { "color" });
		if (_dfMarkupAttribute3 != null)
		{
			Color opacity = dfMarkupStyle.ParseColor(_dfMarkupAttribute3.Value, style.Color);
			opacity.a = style.Opacity;
			style.Color = opacity;
		}
		dfMarkupAttribute _dfMarkupAttribute4 = this.findAttribute(new string[] { "align", "text-align" });
		if (_dfMarkupAttribute4 != null)
		{
			style.Align = dfMarkupStyle.ParseTextAlignment(_dfMarkupAttribute4.Value);
		}
		dfMarkupAttribute _dfMarkupAttribute5 = this.findAttribute(new string[] { "valign", "vertical-align" });
		if (_dfMarkupAttribute5 != null)
		{
			style.VerticalAlign = dfMarkupStyle.ParseVerticalAlignment(_dfMarkupAttribute5.Value);
		}
		dfMarkupAttribute _dfMarkupAttribute6 = this.findAttribute(new string[] { "line-height" });
		if (_dfMarkupAttribute6 != null)
		{
			style.LineHeight = dfMarkupStyle.ParseSize(_dfMarkupAttribute6.Value, style.LineHeight);
		}
		dfMarkupAttribute _dfMarkupAttribute7 = this.findAttribute(new string[] { "text-decoration" });
		if (_dfMarkupAttribute7 != null)
		{
			style.TextDecoration = dfMarkupStyle.ParseTextDecoration(_dfMarkupAttribute7.Value);
		}
		dfMarkupAttribute _dfMarkupAttribute8 = this.findAttribute(new string[] { "background", "background-color" });
		if (_dfMarkupAttribute8 != null)
		{
			style.BackgroundColor = dfMarkupStyle.ParseColor(_dfMarkupAttribute8.Value, Color.clear);
			style.BackgroundColor.a = style.Opacity;
		}
		return style;
	}

	protected dfMarkupAttribute findAttribute(params string[] names)
	{
		for (int i = 0; i < this.Attributes.Count; i++)
		{
			for (int j = 0; j < (int)names.Length; j++)
			{
				if (this.Attributes[i].Name == names[j])
				{
					return this.Attributes[i];
				}
			}
		}
		return null;
	}

	internal override void Release()
	{
		base.Release();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[");
		if (this.IsEndTag)
		{
			stringBuilder.Append("/");
		}
		stringBuilder.Append(this.TagName);
		for (int i = 0; i < this.Attributes.Count; i++)
		{
			stringBuilder.Append(" ");
			stringBuilder.Append(this.Attributes[i].ToString());
		}
		if (this.IsClosedTag)
		{
			stringBuilder.Append("/");
		}
		stringBuilder.Append("]");
		if (!this.IsClosedTag)
		{
			for (int j = 0; j < base.ChildNodes.Count; j++)
			{
				stringBuilder.Append(base.ChildNodes[j].ToString());
			}
			stringBuilder.Append("[/");
			stringBuilder.Append(this.TagName);
			stringBuilder.Append("]");
		}
		return stringBuilder.ToString();
	}
}