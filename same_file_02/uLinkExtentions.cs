using System;
using System.Runtime.CompilerServices;
using uLink;

public static class uLinkExtentions
{
	private const ushort bit8 = 128;

	private const ushort bit1234567 = 127;

	private const int kByte0 = 0;

	private const int kByte1 = 8;

	private const int kByte2 = 16;

	private const int kByte3 = 24;

	private const int kByte4 = 32;

	private const int kByte5 = 40;

	private const int kByte6 = 48;

	private const int kByte7 = 56;

	public static byte[] GetDataByteArray(this BitStream stream)
	{
		int num = stream._bitCount;
		int num1 = num / 8;
		if (num % 8 != 0)
		{
			num1++;
		}
		byte[] numArray = stream._data;
		if ((int)numArray.Length > num1)
		{
			Array.Resize<byte>(ref numArray, num1);
		}
		return numArray;
	}

	public static byte[] GetDataByteArrayShiftedRight(this BitStream stream, int right)
	{
		if (right == 0)
		{
			return stream.GetDataByteArray();
		}
		int num = stream._bitCount;
		int num1 = num / 8;
		if (num % 8 != 0)
		{
			num1++;
		}
		byte[] numArray = new byte[right + num1];
		byte[] numArray1 = stream._data;
		for (int i = 0; i < num1; i++)
		{
			int num2 = right;
			right = num2 + 1;
			numArray[num2] = numArray1[i];
		}
		return numArray;
	}

	public static void Read7BitEncodedSize(this BitStream stream, out ulong u)
	{
		byte num = stream.ReadByte();
		int num1 = 0;
		u = (ulong)(num & 127);
		while ((num & 128) == 128 && num1 <= 9)
		{
			num = stream.ReadByte();
			int num2 = num1 + 1;
			num1 = num2;
			u = (long)u | (ulong)((num & 127) << (num2 * 7 & 31 & 31));
		}
	}

	public static void Read7BitEncodedSize(this BitStream stream, out uint u)
	{
		byte num = stream.ReadByte();
		int num1 = 0;
		u = (uint)(num & 127);
		while ((num & 128) == 128 && num1 <= 4)
		{
			num = stream.ReadByte();
			int num2 = num1 + 1;
			num1 = num2;
			u = u | (num & 127) << (num2 * 7 & 31 & 31);
		}
	}

	public static void Read7BitEncodedSize(this BitStream stream, out ushort u)
	{
		byte num = stream.ReadByte();
		int num1 = 0;
		u = (ushort)(num & 127);
		while ((num & 128) == 128 && num1 <= 2)
		{
			num = stream.ReadByte();
			int num2 = num1 + 1;
			num1 = num2;
			u = (ushort)(u | (ushort)((num & 127) << (num2 * 7 & 31)));
		}
	}

	public static void Read7BitEncodedSize(this BitStream stream, out long u)
	{
		ulong num;
		stream.Read7BitEncodedSize(out num);
		if (num > 9223372036854775807L)
		{
			throw new InvalidOperationException("Wrong");
		}
		u = (long)num;
	}

	public static void Read7BitEncodedSize(this BitStream stream, out int u)
	{
		uint num;
		stream.Read7BitEncodedSize(out num);
		if (num > 2147483647)
		{
			throw new InvalidOperationException("Wrong");
		}
		u = (int)num;
	}

	public static void Read7BitEncodedSize(this BitStream stream, out short u)
	{
		ushort num;
		stream.Read7BitEncodedSize(out num);
		if (num > 32767)
		{
			throw new InvalidOperationException("Wrong");
		}
		u = (short)num;
	}

	public static void ReadByteArray_MinimalCalls(this BitStream stream, out byte[] array, out int length, params object[] codecOptions)
	{
		length = stream.Read<int>(codecOptions);
		if (length != 0)
		{
			array = new byte[length];
			int num = length;
			int num1 = length / 8;
			int num2 = length / 4;
			int num3 = length / 2;
			int num4 = length;
			num4 = num4 - num3 * 2;
			while (true)
			{
				int num5 = num4;
				num4 = num5 - 1;
				if (num5 <= 0)
				{
					break;
				}
				int num6 = num - 1;
				num = num6;
				array[num6] = stream.Read<byte>(codecOptions);
			}
			num3 = num3 - num2 * 2;
			while (true)
			{
				int num7 = num3;
				num3 = num7 - 1;
				if (num7 <= 0)
				{
					break;
				}
				ushort num8 = stream.Read<ushort>(codecOptions);
				int num9 = num - 1;
				num = num9;
				array[num9] = (byte)(num8 >> 8 & 255);
				int num10 = num - 1;
				num = num10;
				array[num10] = (byte)(num8 & 255);
			}
			num2 = num2 - num1 * 2;
			while (true)
			{
				int num11 = num2;
				num2 = num11 - 1;
				if (num11 <= 0)
				{
					break;
				}
				uint num12 = stream.Read<uint>(codecOptions);
				int num13 = num - 1;
				num = num13;
				array[num13] = (byte)(num12 >> 24 & 255);
				int num14 = num - 1;
				num = num14;
				array[num14] = (byte)(num12 >> 16 & 255);
				int num15 = num - 1;
				num = num15;
				array[num15] = (byte)(num12 >> 8 & 255);
				int num16 = num - 1;
				num = num16;
				array[num16] = (byte)(num12 & 255);
			}
			while (true)
			{
				int num17 = num1;
				num1 = num17 - 1;
				if (num17 <= 0)
				{
					break;
				}
				ulong num18 = stream.Read<ulong>(codecOptions);
				int num19 = num - 1;
				num = num19;
				array[num19] = (byte)(num18 >> 56 & (long)255);
				int num20 = num - 1;
				num = num20;
				array[num20] = (byte)(num18 >> 48 & (long)255);
				int num21 = num - 1;
				num = num21;
				array[num21] = (byte)(num18 >> 40 & (long)255);
				int num22 = num - 1;
				num = num22;
				array[num22] = (byte)(num18 >> 32 & (long)255);
				int num23 = num - 1;
				num = num23;
				array[num23] = (byte)(num18 >> 24 & (long)255);
				int num24 = num - 1;
				num = num24;
				array[num24] = (byte)(num18 >> 16 & (long)255);
				int num25 = num - 1;
				num = num25;
				array[num25] = (byte)(num18 >> 8 & (long)255);
				int num26 = num - 1;
				num = num26;
				array[num26] = (byte)(num18 & (long)255);
			}
		}
		else
		{
			array = null;
		}
	}

	public static void Write7BitEncodedSize(this BitStream stream, ulong u)
	{
		while (u >= (long)128)
		{
			stream.WriteByte((byte)(u & (long)127 | (long)128));
			u = u >> 7;
		}
		stream.WriteByte((byte)u);
	}

	public static void Write7BitEncodedSize(this BitStream stream, uint u)
	{
		while (u >= 128)
		{
			stream.WriteByte((byte)(u & 127 | 128));
			u = u >> 7;
		}
		stream.WriteByte((byte)u);
	}

	public static void Write7BitEncodedSize(this BitStream stream, ushort u)
	{
		while (u >= 128)
		{
			stream.WriteByte((byte)(u & 127 | 128));
			u = (ushort)(u >> 7);
		}
		stream.WriteByte((byte)u);
	}

	public static void Write7BitEncodedSize(this BitStream stream, long u)
	{
		if (u < (long)0)
		{
			throw new ArgumentOutOfRangeException("u", "u<0");
		}
		stream.Write7BitEncodedSize((ulong)u);
	}

	public static void Write7BitEncodedSize(this BitStream stream, int u)
	{
		if (u < 0)
		{
			throw new ArgumentOutOfRangeException("u", "u<0");
		}
		stream.Write7BitEncodedSize((uint)u);
	}

	public static void Write7BitEncodedSize(this BitStream stream, short u)
	{
		if (u < 0)
		{
			throw new ArgumentOutOfRangeException("u", "u<0");
		}
		stream.Write7BitEncodedSize((ushort)u);
	}

	public static void WriteByteArray_MinimumCalls(this BitStream stream, byte[] array, int offset, int length, params object[] codecOptions)
	{
		stream.Write<int>(length, codecOptions);
		int num = offset + length;
		int num1 = length / 8;
		int num2 = length / 4;
		int num3 = length / 2;
		int num4 = length;
		num4 = num4 - num3 * 2;
		while (true)
		{
			int num5 = num4;
			num4 = num5 - 1;
			if (num5 <= 0)
			{
				break;
			}
			int num6 = num - 1;
			num = num6;
			stream.Write<byte>(array[num6], codecOptions);
		}
		num3 = num3 - num2 * 2;
		while (true)
		{
			int num7 = num3;
			num3 = num7 - 1;
			if (num7 <= 0)
			{
				break;
			}
			int num8 = num - 1;
			num = num8;
			ushort num9 = (ushort)(array[num8] << 8);
			int num10 = num - 1;
			num = num10;
			stream.Write<ushort>((ushort)(num9 | array[num10]), codecOptions);
		}
		num2 = num2 - num1 * 2;
		while (true)
		{
			int num11 = num2;
			num2 = num11 - 1;
			if (num11 <= 0)
			{
				break;
			}
			int num12 = num - 1;
			num = num12;
			uint num13 = (uint)(array[num12] << 24);
			int num14 = num - 1;
			num = num14;
			num13 = num13 | array[num14] << 16;
			int num15 = num - 1;
			num = num15;
			num13 = num13 | array[num15] << 8;
			int num16 = num - 1;
			num = num16;
			stream.Write<uint>(num13 | array[num16], codecOptions);
		}
		while (true)
		{
			int num17 = num1;
			num1 = num17 - 1;
			if (num17 <= 0)
			{
				break;
			}
			int num18 = num - 1;
			num = num18;
			ulong num19 = (ulong)array[num18] << 56;
			int num20 = num - 1;
			num = num20;
			num19 = num19 | (ulong)array[num20] << 48;
			int num21 = num - 1;
			num = num21;
			num19 = num19 | (ulong)array[num21] << 40;
			int num22 = num - 1;
			num = num22;
			num19 = num19 | (ulong)array[num22] << 32;
			int num23 = num - 1;
			num = num23;
			num19 = num19 | (ulong)array[num23] << 24;
			int num24 = num - 1;
			num = num24;
			num19 = num19 | (ulong)array[num24] << 16;
			int num25 = num - 1;
			num = num25;
			num19 = num19 | (ulong)array[num25] << 8;
			int num26 = num - 1;
			num = num26;
			stream.Write<ulong>(num19 | (ulong)array[num26], codecOptions);
		}
	}
}