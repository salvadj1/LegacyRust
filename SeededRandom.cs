using System;

public class SeededRandom
{
	private const int kBufferSize = 16;

	private const int kBufferBitSize = 128;

	private const int kBitsInByte = 8;

	private const byte kMaskBitPos = 7;

	private const int kShiftBitPos = 3;

	private const byte kMaskBytePos = 15;

	private const int kShiftBytePos = 4;

	private const int kMaxAllocPos = 33554431;

	private const int kMaxAllocCount = 33554432;

	private Random rand;

	private readonly byte[] byteBuffer;

	public readonly int Seed;

	private uint allocCount;

	private byte bytePos;

	private byte bitPos;

	public uint PositionData
	{
		get
		{
			return (uint)((((this.bytePos > 0 || this.bitPos > 0) && this.allocCount > 0 ? (int)(this.allocCount - 1) : (int)this.allocCount) << 4 | (byte)(this.bytePos & 15)) << 7 | (byte)(this.bitPos & 7));
		}
		set
		{
			byte num = (byte)(value & 7);
			UInt32 num1 = value >> 3;
			value = num1;
			byte num2 = (byte)(num1 & 15);
			UInt32 num3 = value >> 4;
			value = num3;
			uint num4 = num3;
			if (num > 0 || num2 > 0)
			{
				num4++;
			}
			if (num4 < this.allocCount)
			{
				this.allocCount = 0;
				this.rand = new Random(this.Seed);
			}
			while (this.allocCount < num4)
			{
				SeededRandom seededRandom = this;
				seededRandom.allocCount = seededRandom.allocCount + 1;
				this.rand.NextBytes(this.byteBuffer);
			}
			this.bitPos = num;
			this.bytePos = num2;
		}
	}

	public SeededRandom() : this(Environment.TickCount)
	{
	}

	public SeededRandom(int seed)
	{
		this.byteBuffer = new byte[16];
		this.Seed = seed;
		this.rand = new Random(seed);
	}

	public bool Boolean()
	{
		if (this.bytePos == 0 && this.bitPos == 0)
		{
			this.Fill();
			SeededRandom seededRandom = this;
			seededRandom.bitPos = (byte)(seededRandom.bitPos + 1);
			return (this.byteBuffer[0] & 1) == 1;
		}
		bool flag = (this.byteBuffer[this.bytePos] & 1 << (this.bitPos & 31)) == 1 << (this.bitPos & 31);
		SeededRandom seededRandom1 = this;
		byte num = (byte)(seededRandom1.bitPos + 1);
		byte num1 = num;
		seededRandom1.bitPos = num;
		if (num1 == 8)
		{
			this.bitPos = 0;
			SeededRandom seededRandom2 = this;
			byte num2 = (byte)(seededRandom2.bytePos + 1);
			num1 = num2;
			seededRandom2.bytePos = num2;
			if (num1 == 16)
			{
				this.bytePos = 0;
			}
		}
		return flag;
	}

	private void Fill()
	{
		SeededRandom seededRandom = this;
		UInt32 num = seededRandom.allocCount + 1;
		uint num1 = num;
		seededRandom.allocCount = num;
		if (num1 == 33554432)
		{
			this.rand = new Random(this.Seed);
			this.allocCount = 1;
		}
		this.rand.NextBytes(this.byteBuffer);
	}

	private static double LT1(double v)
	{
		return (v <= 9.88131291682493E-324 ? v : v - 4.94065645841247E-324);
	}

	public T Pick<T>(T[] array)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		return array[this.RandomIndex((int)array.Length)];
	}

	public bool Pick<T>(T[] array, out T value)
	{
		if (array == null || (int)array.Length == 0)
		{
			value = default(T);
			return false;
		}
		value = array[this.RandomIndex((int)array.Length)];
		return true;
	}

	public int RandomBits(int bitCount)
	{
		if (bitCount < 0 || bitCount > 32)
		{
			throw new ArgumentOutOfRangeException("bitCount");
		}
		int num = 0;
		int num1 = 0;
		while (true)
		{
			int num2 = bitCount;
			bitCount = num2 - 1;
			if (num2 <= 0)
			{
				break;
			}
			if (this.Boolean())
			{
				num = num | 1 << (num1 & 31 & 31);
			}
			num1++;
		}
		return num;
	}

	public double RandomFraction16()
	{
		uint num = 0;
		for (int i = 0; i < 16; i++)
		{
			if (this.Boolean())
			{
				num = num | 1 << (i & 31 & 31);
			}
		}
		return (double)((float)num) / 65535;
	}

	public double RandomFraction32()
	{
		uint num = 0;
		for (int i = 0; i < 32; i++)
		{
			if (this.Boolean())
			{
				num = num | 1 << (i & 31 & 31);
			}
		}
		return (double)((float)num) / 4294967295;
	}

	public double RandomFraction8()
	{
		uint num = 0;
		for (int i = 0; i < 8; i++)
		{
			if (this.Boolean())
			{
				num = num | 1 << (i & 31 & 31);
			}
		}
		return (double)((float)num) / 255;
	}

	private double RandomFractionBitDepth(int bitDepth, int bitMask)
	{
		if (bitDepth < 1 || bitDepth > 32)
		{
			throw new ArgumentOutOfRangeException("bitDepth", "!( bitDepth > 0 && bitDepth <= 32 )");
		}
		if (bitDepth == 32)
		{
			return this.RandomFraction32();
		}
		if (bitMask <= 0)
		{
			throw new ArgumentException("bitMask", "!(bitMask > 0)");
		}
		int num = 0;
		for (int i = 0; i < bitDepth; i++)
		{
			if (this.Boolean())
			{
				num = num | 1 << (i & 31 & 31);
			}
		}
		return (double)num / (double)bitMask;
	}

	public double RandomFractionBitDepth(int bitDepth)
	{
		if (bitDepth < 1 || bitDepth > 32)
		{
			throw new ArgumentOutOfRangeException("bitDepth", "!( bitDepth > 0 && bitDepth <= 32 )");
		}
		if (bitDepth == 32)
		{
			return this.RandomFraction32();
		}
		int num = 0;
		int num1 = 0;
		for (int i = 0; i < bitDepth; i++)
		{
			int num2 = 1 << (i & 31);
			if (this.Boolean())
			{
				num = num | num2;
			}
			num1 = num1 | num2;
		}
		return (double)num / (double)num1;
	}

	private double RandomFractionBitDepthLT1(int bitDepth, int bitMask)
	{
		return SeededRandom.LT1(this.RandomFractionBitDepth(bitDepth, bitMask));
	}

	public double RandomFractionBitDepthLT1(int bitDepth)
	{
		return SeededRandom.LT1(this.RandomFractionBitDepth(bitDepth));
	}

	public int RandomIndex(int length)
	{
		UInt32 num;
		if (length == 0 || (length & -2147483648) == -2147483648)
		{
			throw new ArgumentOutOfRangeException("length", "!(length <= 0)");
		}
		uint num1 = (uint)length;
		UInt32 num2 = num1 >> 1;
		num1 = num2;
		if (num2 == 0)
		{
			return 0;
		}
		int num3 = 1;
		byte num4 = 1;
		UInt32 num5 = num1 >> 1;
		num1 = num5;
		if (num5 == 0)
		{
			return (!this.Boolean() ? 0 : 1);
		}
		do
		{
			num4 = (byte)(num4 + 1);
			num3 = num3 << 1 | 1;
			num = num1 >> 1;
			num1 = num;
		}
		while (num != 0);
		return (int)Math.Floor(this.RandomFractionBitDepthLT1((int)num4, num3) * (double)length);
	}

	public double Range(double minInclusive, double maxInclusive, int bitDepth)
	{
		return (minInclusive != maxInclusive ? this.RandomFractionBitDepth(bitDepth) * (maxInclusive - minInclusive) + minInclusive : minInclusive);
	}

	public double Range(double minInclusive, double maxInclusive)
	{
		return this.Range(minInclusive, maxInclusive, 16);
	}

	public float Range(float minInclusive, float maxInclusive, int bitDepth)
	{
		return (float)this.Range((double)minInclusive, (double)maxInclusive, bitDepth);
	}

	public float Range(float minInclusive, float maxInclusive)
	{
		return (float)this.Range((double)minInclusive, (double)maxInclusive);
	}

	public int Range(int minInclusive, int maxInclusive)
	{
		if (minInclusive > maxInclusive)
		{
			int num = maxInclusive;
			maxInclusive = minInclusive;
			minInclusive = num;
		}
		else if (maxInclusive == minInclusive)
		{
			return minInclusive;
		}
		ulong num1 = (ulong)((long)maxInclusive - (long)minInclusive);
		if (num1 > (long)2147483647)
		{
			return (int)((long)minInclusive + (long)Math.Round((double)((float)num1) * this.RandomFraction32()));
		}
		int num2 = 0;
		int num3 = 0;
		uint num4 = (uint)num1;
		while (true)
		{
			UInt32 num5 = num4 >> 1;
			num4 = num5;
			if (num5 == 0)
			{
				break;
			}
			num2++;
			num3 = num3 << 1 | 1;
		}
		return minInclusive + (int)Math.Round((double)this.RandomBits(num2) / (double)num3 * (double)((float)num1));
	}

	public bool Reset()
	{
		if (this.allocCount <= 0)
		{
			return false;
		}
		this.rand = new Random(this.Seed);
		this.allocCount = 0;
		return true;
	}
}