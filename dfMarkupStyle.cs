using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public struct dfMarkupStyle
{
	private static Dictionary<string, UnityEngine.Color> namedColors;

	public dfRichTextLabel Host;

	public dfAtlas Atlas;

	public dfDynamicFont Font;

	public int FontSize;

	public UnityEngine.FontStyle FontStyle;

	public dfMarkupTextDecoration TextDecoration;

	public dfMarkupTextAlign Align;

	public dfMarkupVerticalAlign VerticalAlign;

	public UnityEngine.Color Color;

	public UnityEngine.Color BackgroundColor;

	public float Opacity;

	public bool PreserveWhitespace;

	public bool Preformatted;

	public int WordSpacing;

	public int CharacterSpacing;

	private int lineHeight;

	public int LineHeight
	{
		get
		{
			if (this.lineHeight == 0)
			{
				return Mathf.CeilToInt((float)this.FontSize);
			}
			return Mathf.Max(this.FontSize, this.lineHeight);
		}
		set
		{
			this.lineHeight = value;
		}
	}

	static dfMarkupStyle()
	{
		Dictionary<string, UnityEngine.Color> strs = new Dictionary<string, UnityEngine.Color>()
		{
			{ "aqua", dfMarkupStyle.UIntToColor(65535) },
			{ "black", UnityEngine.Color.black },
			{ "blue", UnityEngine.Color.blue },
			{ "cyan", UnityEngine.Color.cyan },
			{ "fuchsia", dfMarkupStyle.UIntToColor(16711935) },
			{ "gray", UnityEngine.Color.gray },
			{ "green", UnityEngine.Color.green },
			{ "lime", dfMarkupStyle.UIntToColor(65280) },
			{ "magenta", UnityEngine.Color.magenta },
			{ "maroon", dfMarkupStyle.UIntToColor(8388608) },
			{ "navy", dfMarkupStyle.UIntToColor(128) },
			{ "olive", dfMarkupStyle.UIntToColor(8421376) },
			{ "orange", dfMarkupStyle.UIntToColor(16753920) },
			{ "purple", dfMarkupStyle.UIntToColor(8388736) },
			{ "red", UnityEngine.Color.red },
			{ "silver", dfMarkupStyle.UIntToColor(12632256) },
			{ "teal", dfMarkupStyle.UIntToColor(32896) },
			{ "white", UnityEngine.Color.white },
			{ "yellow", UnityEngine.Color.yellow }
		};
		dfMarkupStyle.namedColors = strs;
	}

	public dfMarkupStyle(dfDynamicFont Font, int FontSize, UnityEngine.FontStyle FontStyle)
	{
		this.Host = null;
		this.Atlas = null;
		this.Font = Font;
		this.FontSize = FontSize;
		this.FontStyle = FontStyle;
		this.Align = dfMarkupTextAlign.Left;
		this.VerticalAlign = dfMarkupVerticalAlign.Baseline;
		this.Color = UnityEngine.Color.white;
		this.BackgroundColor = UnityEngine.Color.clear;
		this.TextDecoration = dfMarkupTextDecoration.None;
		this.PreserveWhitespace = false;
		this.Preformatted = false;
		this.WordSpacing = 0;
		this.CharacterSpacing = 0;
		this.lineHeight = 0;
		this.Opacity = 1f;
	}

	public static UnityEngine.Color ParseColor(string color, UnityEngine.Color defaultColor)
	{
		UnityEngine.Color color1;
		UnityEngine.Color color2 = defaultColor;
		if (color.StartsWith("#"))
		{
			uint num = 0;
			if (!uint.TryParse(color.Substring(1), NumberStyles.HexNumber, null, out num))
			{
				color2 = UnityEngine.Color.red;
			}
			else
			{
				color2 = dfMarkupStyle.UIntToColor(num);
			}
		}
		else if (dfMarkupStyle.namedColors.TryGetValue(color.ToLowerInvariant(), out color1))
		{
			color2 = color1;
		}
		return color2;
	}

	public static UnityEngine.FontStyle ParseFontStyle(string value, UnityEngine.FontStyle baseStyle)
	{
		if (value == "normal")
		{
			return UnityEngine.FontStyle.Normal;
		}
		if (value == "bold")
		{
			if (baseStyle == UnityEngine.FontStyle.Normal)
			{
				return UnityEngine.FontStyle.Bold;
			}
			if (baseStyle == UnityEngine.FontStyle.Italic)
			{
				return UnityEngine.FontStyle.BoldAndItalic;
			}
		}
		else if (value == "italic")
		{
			if (baseStyle == UnityEngine.FontStyle.Normal)
			{
				return UnityEngine.FontStyle.Italic;
			}
			if (baseStyle == UnityEngine.FontStyle.Bold)
			{
				return UnityEngine.FontStyle.BoldAndItalic;
			}
		}
		return baseStyle;
	}

	public static int ParseSize(string value, int baseValue)
	{
		int num;
		int num1;
		if (value.Length > 1 && value.EndsWith("%"))
		{
			if (int.TryParse(value.TrimEnd(new char[] { '%' }), out num))
			{
				return (int)((float)baseValue * ((float)num / 100f));
			}
		}
		if (value.EndsWith("px"))
		{
			value = value.Substring(0, value.Length - 2);
		}
		if (int.TryParse(value, out num1))
		{
			return num1;
		}
		return baseValue;
	}

	public static dfMarkupTextAlign ParseTextAlignment(string value)
	{
		if (value == "right")
		{
			return dfMarkupTextAlign.Right;
		}
		if (value == "center")
		{
			return dfMarkupTextAlign.Center;
		}
		if (value == "justify")
		{
			return dfMarkupTextAlign.Justify;
		}
		return dfMarkupTextAlign.Left;
	}

	public static dfMarkupTextDecoration ParseTextDecoration(string value)
	{
		if (value == "underline")
		{
			return dfMarkupTextDecoration.Underline;
		}
		if (value == "overline")
		{
			return dfMarkupTextDecoration.Overline;
		}
		if (value == "line-through")
		{
			return dfMarkupTextDecoration.LineThrough;
		}
		return dfMarkupTextDecoration.None;
	}

	public static dfMarkupVerticalAlign ParseVerticalAlignment(string value)
	{
		if (value == "top")
		{
			return dfMarkupVerticalAlign.Top;
		}
		if (value == "center" || value == "middle")
		{
			return dfMarkupVerticalAlign.Middle;
		}
		if (value == "bottom")
		{
			return dfMarkupVerticalAlign.Bottom;
		}
		return dfMarkupVerticalAlign.Baseline;
	}

	private static Color32 UIntToColor(uint color)
	{
		byte num = (byte)(color >> 16);
		byte num1 = (byte)(color >> 8);
		return new Color32(num, num1, (byte)color, 255);
	}
}