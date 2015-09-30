using System;

[Serializable]
public class CullGridSetup
{
	public int cellSquareDimension;

	public int cellsWide;

	public int cellsTall;

	public int groupBegin;

	public int gatheringCellsWide;

	public int gatheringCellsTall;

	public int gatheringCellsCenter;

	public int[] gatheringCellsBits;

	public CullGridSetup()
	{
		this.cellSquareDimension = 200;
		this.cellsWide = 80;
		this.cellsTall = 80;
		this.groupBegin = 100;
		this.gatheringCellsWide = 3;
		this.gatheringCellsTall = 3;
		this.gatheringCellsCenter = 4;
		this.gatheringCellsBits = new int[] { -8193, -101897 };
	}

	protected CullGridSetup(CullGridSetup copyFrom)
	{
		this.cellSquareDimension = copyFrom.cellSquareDimension;
		this.cellsWide = copyFrom.cellsWide;
		this.cellsTall = copyFrom.cellsTall;
		this.groupBegin = copyFrom.groupBegin;
		this.gatheringCellsWide = copyFrom.gatheringCellsWide;
		this.gatheringCellsTall = copyFrom.gatheringCellsTall;
		this.gatheringCellsCenter = copyFrom.gatheringCellsCenter;
		this.gatheringCellsBits = (int[])copyFrom.gatheringCellsBits.Clone();
	}

	public bool GetGatheringBit(int x, int y)
	{
		if (x >= this.gatheringCellsWide || x < 0)
		{
			throw new ArgumentOutOfRangeException("x", "must be < gatheringCellsWide && >= 0");
		}
		if (y >= this.gatheringCellsTall || y < 0)
		{
			throw new ArgumentOutOfRangeException("y", "must be < gatheringCellsTall && >= 0");
		}
		int num = y * this.gatheringCellsWide + x;
		int num1 = num / 32;
		int num2 = num % 32;
		if (this.gatheringCellsBits == null)
		{
			return true;
		}
		if ((int)this.gatheringCellsBits.Length <= num1)
		{
			return true;
		}
		return (this.gatheringCellsBits[num1] & 1 << (num2 & 31)) == 1 << (num2 & 31);
	}

	public void SetGatheringBit(int x, int y, bool v)
	{
		if (x >= this.gatheringCellsWide || x < 0)
		{
			throw new ArgumentOutOfRangeException("x", "must be < gatheringCellsWide && >= 0");
		}
		if (y >= this.gatheringCellsTall || y < 0)
		{
			throw new ArgumentOutOfRangeException("y", "must be < gatheringCellsTall && >= 0");
		}
		int num = y * this.gatheringCellsWide + x;
		int num1 = num / 32;
		int num2 = num % 32;
		if (this.gatheringCellsBits == null)
		{
			Array.Resize<int>(ref this.gatheringCellsBits, num1 + 1);
			for (int i = 0; i < num1; i++)
			{
				this.gatheringCellsBits[i] = -1;
			}
			if (v)
			{
				this.gatheringCellsBits[num1] = -1;
			}
			else
			{
				this.gatheringCellsBits[num1] = ~(1 << (num2 & 31));
			}
		}
		else if ((int)this.gatheringCellsBits.Length <= num1)
		{
			int length = (int)this.gatheringCellsBits.Length;
			Array.Resize<int>(ref this.gatheringCellsBits, num1 + 1);
			for (int j = length + 1; j <= num1; j++)
			{
				this.gatheringCellsBits[j] = -1;
			}
			if (!v)
			{
				this.gatheringCellsBits[num1] = this.gatheringCellsBits[num1] & ~(1 << (num2 & 31 & 31));
			}
		}
		else if (!v)
		{
			this.gatheringCellsBits[num1] = this.gatheringCellsBits[num1] & ~(1 << (num2 & 31 & 31));
		}
		else
		{
			this.gatheringCellsBits[num1] = this.gatheringCellsBits[num1] | 1 << (num2 & 31 & 31);
		}
	}

	public void SetGatheringDimensions(int gatheringCellsWide, int gatheringCellsTall)
	{
		if (this.gatheringCellsWide == gatheringCellsWide && this.gatheringCellsTall == gatheringCellsTall)
		{
			return;
		}
		this.gatheringCellsWide = gatheringCellsWide;
		this.gatheringCellsTall = gatheringCellsTall;
		this.gatheringCellsCenter = this.gatheringCellsWide / 2 + this.gatheringCellsTall / 2 * this.gatheringCellsWide;
	}

	public void ToggleGatheringBit(int x, int y)
	{
		if (x >= this.gatheringCellsWide || x < 0)
		{
			throw new ArgumentOutOfRangeException("x", "must be < gatheringCellsWide && >= 0");
		}
		if (y >= this.gatheringCellsTall || y < 0)
		{
			throw new ArgumentOutOfRangeException("y", "must be < gatheringCellsTall && >= 0");
		}
		int num = y * this.gatheringCellsWide + x;
		int num1 = num / 32;
		int num2 = num % 32;
		if (this.gatheringCellsBits == null)
		{
			Array.Resize<int>(ref this.gatheringCellsBits, num1 + 1);
			for (int i = 0; i < num1; i++)
			{
				this.gatheringCellsBits[i] = -1;
			}
			this.gatheringCellsBits[num1] = ~(1 << (num2 & 31));
		}
		else if ((int)this.gatheringCellsBits.Length > num1)
		{
			this.gatheringCellsBits[num1] = this.gatheringCellsBits[num1] ^ 1 << (num2 & 31 & 31);
		}
		else
		{
			int length = (int)this.gatheringCellsBits.Length;
			Array.Resize<int>(ref this.gatheringCellsBits, num1 + 1);
			for (int j = length + 1; j < num1; j++)
			{
				this.gatheringCellsBits[j] = -1;
			}
			this.gatheringCellsBits[num1] = ~(1 << (num2 & 31));
		}
	}
}