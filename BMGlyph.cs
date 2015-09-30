using System;
using System.Collections.Generic;

[Serializable]
public class BMGlyph
{
	public int index;

	public int x;

	public int y;

	public int width;

	public int height;

	public int offsetX;

	public int offsetY;

	public int advance;

	public int channel;

	public List<BMGlyph.Kerning> kerning;

	public BMGlyph()
	{
	}

	public int GetKerning(int previousChar)
	{
		if (this.kerning != null)
		{
			int num = 0;
			int count = this.kerning.Count;
			while (num < count)
			{
				BMGlyph.Kerning item = this.kerning[num];
				if (item.previousChar == previousChar)
				{
					return item.amount;
				}
				num++;
			}
		}
		return 0;
	}

	public void SetKerning(int previousChar, int amount)
	{
		if (this.kerning == null)
		{
			this.kerning = new List<BMGlyph.Kerning>();
		}
		for (int i = 0; i < this.kerning.Count; i++)
		{
			if (this.kerning[i].previousChar == previousChar)
			{
				BMGlyph.Kerning item = this.kerning[i];
				item.amount = amount;
				this.kerning[i] = item;
				return;
			}
		}
		BMGlyph.Kerning kerning = new BMGlyph.Kerning()
		{
			previousChar = previousChar,
			amount = amount
		};
		this.kerning.Add(kerning);
	}

	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		int num = this.x + this.width;
		int num1 = this.y + this.height;
		if (this.x < xMin)
		{
			int num2 = xMin - this.x;
			BMGlyph bMGlyph = this;
			bMGlyph.x = bMGlyph.x + num2;
			BMGlyph bMGlyph1 = this;
			bMGlyph1.width = bMGlyph1.width - num2;
			BMGlyph bMGlyph2 = this;
			bMGlyph2.offsetX = bMGlyph2.offsetX + num2;
		}
		if (this.y < yMin)
		{
			int num3 = yMin - this.y;
			BMGlyph bMGlyph3 = this;
			bMGlyph3.y = bMGlyph3.y + num3;
			BMGlyph bMGlyph4 = this;
			bMGlyph4.height = bMGlyph4.height - num3;
			BMGlyph bMGlyph5 = this;
			bMGlyph5.offsetY = bMGlyph5.offsetY + num3;
		}
		if (num > xMax)
		{
			BMGlyph bMGlyph6 = this;
			bMGlyph6.width = bMGlyph6.width - (num - xMax);
		}
		if (num1 > yMax)
		{
			BMGlyph bMGlyph7 = this;
			bMGlyph7.height = bMGlyph7.height - (num1 - yMax);
		}
	}

	public struct Kerning
	{
		public int previousChar;

		public int amount;
	}
}