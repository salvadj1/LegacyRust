using System;
using System.Text.RegularExpressions;

public struct dfMarkupBorders
{
	public int left;

	public int top;

	public int right;

	public int bottom;

	public int horizontal
	{
		get
		{
			return this.left + this.right;
		}
	}

	public int vertical
	{
		get
		{
			return this.top + this.bottom;
		}
	}

	public dfMarkupBorders(int left, int right, int top, int bottom)
	{
		this.left = left;
		this.top = top;
		this.right = right;
		this.bottom = bottom;
	}

	public static dfMarkupBorders Parse(string value)
	{
		int num;
		dfMarkupBorders dfMarkupBorder = new dfMarkupBorders();
		value = Regex.Replace(value, "\\s+", " ");
		string[] strArrays = value.Split(new char[] { ' ' });
		if ((int)strArrays.Length == 1)
		{
			int num1 = dfMarkupStyle.ParseSize(value, 0);
			int num2 = num1;
			num = num2;
			dfMarkupBorder.right = num2;
			dfMarkupBorder.left = num;
			int num3 = num1;
			num = num3;
			dfMarkupBorder.bottom = num3;
			dfMarkupBorder.top = num;
		}
		else if ((int)strArrays.Length == 2)
		{
			int num4 = dfMarkupStyle.ParseSize(strArrays[0], 0);
			num = num4;
			dfMarkupBorder.bottom = num4;
			dfMarkupBorder.top = num;
			int num5 = dfMarkupStyle.ParseSize(strArrays[1], 0);
			num = num5;
			dfMarkupBorder.right = num5;
			dfMarkupBorder.left = num;
		}
		else if ((int)strArrays.Length == 3)
		{
			int num6 = dfMarkupStyle.ParseSize(strArrays[0], 0);
			dfMarkupBorder.top = num6;
			int num7 = dfMarkupStyle.ParseSize(strArrays[1], 0);
			num = num7;
			dfMarkupBorder.right = num7;
			dfMarkupBorder.left = num;
			int num8 = dfMarkupStyle.ParseSize(strArrays[2], 0);
			dfMarkupBorder.bottom = num8;
		}
		else if ((int)strArrays.Length == 4)
		{
			int num9 = dfMarkupStyle.ParseSize(strArrays[0], 0);
			dfMarkupBorder.top = num9;
			int num10 = dfMarkupStyle.ParseSize(strArrays[1], 0);
			dfMarkupBorder.right = num10;
			int num11 = dfMarkupStyle.ParseSize(strArrays[2], 0);
			dfMarkupBorder.bottom = num11;
			int num12 = dfMarkupStyle.ParseSize(strArrays[3], 0);
			dfMarkupBorder.left = num12;
		}
		return dfMarkupBorder;
	}

	public override string ToString()
	{
		return string.Format("[T:{0},R:{1},L:{2},B:{3}]", new object[] { this.top, this.right, this.left, this.bottom });
	}
}