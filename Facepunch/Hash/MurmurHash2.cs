using System;
using System.Text;

namespace Facepunch.Hash
{
	public static class MurmurHash2
	{
		public const uint m = 1540483477;

		public const int r = 24;

		public static int SINT(byte[] key, int len, uint seed)
		{
			return (int)MurmurHash2.UINT(key, len, seed);
		}

		public static int SINT(sbyte[] key, int len, uint seed)
		{
			return (int)MurmurHash2.UINT(key, len, seed);
		}

		public static int SINT(ushort[] key, int len, uint seed)
		{
			return (int)MurmurHash2.UINT(key, len, seed);
		}

		public static int SINT(short[] key, int len, uint seed)
		{
			return (int)MurmurHash2.UINT(key, len, seed);
		}

		public static int SINT(char[] key, int len, uint seed)
		{
			return (int)MurmurHash2.UINT(key, len, seed);
		}

		public static int SINT(string key, int len, uint seed)
		{
			return (int)MurmurHash2.UINT(key, len, seed);
		}

		public static int SINT(uint[] key, int len, uint seed)
		{
			return (int)MurmurHash2.UINT(key, len, seed);
		}

		public static int SINT(int[] key, int len, uint seed)
		{
			return (int)MurmurHash2.UINT(key, len, seed);
		}

		public static int SINT(ulong[] key, int len, uint seed)
		{
			return (int)MurmurHash2.UINT(key, len, seed);
		}

		public static int SINT(long[] key, int len, uint seed)
		{
			return (int)MurmurHash2.UINT(key, len, seed);
		}

		public static int SINT(byte[] key, uint seed)
		{
			return (int)MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static int SINT(sbyte[] key, uint seed)
		{
			return (int)MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static int SINT(ushort[] key, uint seed)
		{
			return (int)MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static int SINT(short[] key, uint seed)
		{
			return (int)MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static int SINT(char[] key, uint seed)
		{
			return (int)MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static int SINT(string key, uint seed)
		{
			return (int)MurmurHash2.UINT(key, key.Length, seed);
		}

		public static int SINT(uint[] key, uint seed)
		{
			return (int)MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static int SINT(int[] key, uint seed)
		{
			return (int)MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static int SINT(ulong[] key, uint seed)
		{
			return (int)MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static int SINT(long[] key, uint seed)
		{
			return (int)MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static int SINT(string key, Encoding encoding, uint seed)
		{
			return (int)MurmurHash2.UINT(key, encoding, seed);
		}

		public static int SINT_BLOCK(Array key, int len, uint seed)
		{
			return (int)MurmurHash2.UINT_BLOCK(key, len, seed);
		}

		public static int SINT_BLOCK(Array key, uint seed)
		{
			return (int)MurmurHash2.UINT_BLOCK(key, Buffer.ByteLength(key), seed);
		}

		public static uint UINT(byte[] key, int len, uint seed)
		{
			uint num = (uint)((ulong)seed ^ (long)(len * 1));
			int num1 = 0;
			while (len >= 4)
			{
				int num2 = num1;
				num1 = num2 + 1;
				int num3 = num1;
				num1 = num3 + 1;
				int num4 = num1;
				num1 = num4 + 1;
				int num5 = num1;
				num1 = num5 + 1;
				uint num6 = (uint)(key[num2] | key[num3] << 8 | key[num4] << 16 | key[num5] << 24);
				num6 = num6 * 1540483477;
				num6 = num6 ^ num6 >> 24;
				num6 = num6 * 1540483477;
				num = num * 1540483477;
				num = num ^ num6;
				len = len - 4;
			}
			switch (len)
			{
				case 1:
				{
					num = num ^ key[num1];
					num = num * 1540483477;
					break;
				}
				case 2:
				{
					num = num ^ key[num1 + 1] << 8;
					num = num ^ key[num1];
					num = num * 1540483477;
					break;
				}
				case 3:
				{
					num = num ^ key[num1 + 2] << 16;
					num = num ^ key[num1 + 1] << 8;
					num = num ^ key[num1];
					num = num * 1540483477;
					break;
				}
			}
			num = num ^ num >> 13;
			num = num * 1540483477;
			num = num ^ num >> 15;
			return num;
		}

		public static uint UINT(byte[] key, uint seed)
		{
			return MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static uint UINT(sbyte[] key, int len, uint seed)
		{
			uint num = (uint)((ulong)seed ^ (long)(len * 1));
			int num1 = 0;
			while (len >= 4)
			{
				int num2 = num1;
				num1 = num2 + 1;
				int num3 = num1;
				num1 = num3 + 1;
				int num4 = num1;
				num1 = num4 + 1;
				int num5 = num1;
				num1 = num5 + 1;
				uint num6 = (uint)((byte)key[num2] | (byte)key[num3] << 8 | (byte)key[num4] << 16 | (byte)key[num5] << 24);
				num6 = num6 * 1540483477;
				num6 = num6 ^ num6 >> 24;
				num6 = num6 * 1540483477;
				num = num * 1540483477;
				num = num ^ num6;
				len = len - 4;
			}
			switch (len)
			{
				case 1:
				{
					num = num ^ (uint)key[num1];
					num = num * 1540483477;
					break;
				}
				case 2:
				{
					num = num ^ (uint)key[num1 + 1] << 8;
					num = num ^ (uint)key[num1];
					num = num * 1540483477;
					break;
				}
				case 3:
				{
					num = num ^ (uint)key[num1 + 2] << 16;
					num = num ^ (uint)key[num1 + 1] << 8;
					num = num ^ (uint)key[num1];
					num = num * 1540483477;
					break;
				}
			}
			num = num ^ num >> 13;
			num = num * 1540483477;
			num = num ^ num >> 15;
			return num;
		}

		public static uint UINT(sbyte[] key, uint seed)
		{
			return MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static uint UINT(ushort[] key, int len, uint seed)
		{
			uint num = (uint)((ulong)seed ^ (long)(len * 2));
			int num1 = 0;
			while (len >= 2)
			{
				int num2 = num1;
				num1 = num2 + 1;
				int num3 = num1;
				num1 = num3 + 1;
				uint num4 = (uint)(key[num2] | key[num3] << 16);
				num4 = num4 * 1540483477;
				num4 = num4 ^ num4 >> 24;
				num4 = num4 * 1540483477;
				num = num * 1540483477;
				num = num ^ num4;
				len = len - 2;
			}
			if (len == 1)
			{
				num = num ^ key[num1] & 65280;
				num = num ^ key[num1] & 255;
				num = num * 1540483477;
			}
			num = num ^ num >> 13;
			num = num * 1540483477;
			num = num ^ num >> 15;
			return num;
		}

		public static uint UINT(ushort[] key, uint seed)
		{
			return MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static uint UINT(short[] key, int len, uint seed)
		{
			uint num = (uint)((ulong)seed ^ (long)(len * 2));
			int num1 = 0;
			while (len >= 2)
			{
				int num2 = num1;
				num1 = num2 + 1;
				int num3 = num1;
				num1 = num3 + 1;
				uint num4 = (uint)((ushort)key[num2] | (ushort)key[num3] << 16);
				num4 = num4 * 1540483477;
				num4 = num4 ^ num4 >> 24;
				num4 = num4 * 1540483477;
				num = num * 1540483477;
				num = num ^ num4;
				len = len - 2;
			}
			if (len == 1)
			{
				num = num ^ key[num1] & 65280;
				num = num ^ key[num1] & 255;
				num = num * 1540483477;
			}
			num = num ^ num >> 13;
			num = num * 1540483477;
			num = num ^ num >> 15;
			return num;
		}

		public static uint UINT(short[] key, uint seed)
		{
			return MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static uint UINT(char[] key, int len, uint seed)
		{
			uint num = (uint)((ulong)seed ^ (long)(len * 2));
			int num1 = 0;
			while (len >= 2)
			{
				int num2 = num1;
				num1 = num2 + 1;
				int num3 = num1;
				num1 = num3 + 1;
				uint num4 = (uint)(key[num2] | (char)(key[num3] << '\u0010'));
				num4 = num4 * 1540483477;
				num4 = num4 ^ num4 >> 24;
				num4 = num4 * 1540483477;
				num = num * 1540483477;
				num = num ^ num4;
				len = len - 2;
			}
			if (len == 1)
			{
				num = num ^ key[num1] & '\uFF00';
				num = num ^ key[num1] & 'ÿ';
				num = num * 1540483477;
			}
			num = num ^ num >> 13;
			num = num * 1540483477;
			num = num ^ num >> 15;
			return num;
		}

		public static uint UINT(char[] key, uint seed)
		{
			return MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static uint UINT(string key, int len, uint seed)
		{
			uint num = (uint)((ulong)seed ^ (long)(len * 2));
			int num1 = 0;
			while (len >= 2)
			{
				int num2 = num1;
				num1 = num2 + 1;
				int num3 = num1;
				num1 = num3 + 1;
				uint num4 = (uint)(key[num2] | (char)(key[num3] << '\u0010'));
				num4 = num4 * 1540483477;
				num4 = num4 ^ num4 >> 24;
				num4 = num4 * 1540483477;
				num = num * 1540483477;
				num = num ^ num4;
				len = len - 2;
			}
			if (len == 1)
			{
				num = num ^ key[num1] & '\uFF00';
				num = num ^ key[num1] & 'ÿ';
				num = num * 1540483477;
			}
			num = num ^ num >> 13;
			num = num * 1540483477;
			num = num ^ num >> 15;
			return num;
		}

		public static uint UINT(string key, uint seed)
		{
			return MurmurHash2.UINT(key, key.Length, seed);
		}

		public static uint UINT(uint[] key, int len, uint seed)
		{
			uint num = (uint)((ulong)seed ^ (long)(len * 4));
			int num1 = 0;
			while (len > 0)
			{
				int num2 = num1;
				num1 = num2 + 1;
				uint num3 = key[num2];
				num3 = num3 * 1540483477;
				num3 = num3 ^ num3 >> 24;
				num3 = num3 * 1540483477;
				num = num * 1540483477;
				num = num ^ num3;
				len--;
			}
			num = num ^ num >> 13;
			num = num * 1540483477;
			num = num ^ num >> 15;
			return num;
		}

		public static uint UINT(uint[] key, uint seed)
		{
			return MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static uint UINT(int[] key, int len, uint seed)
		{
			uint num = (uint)((ulong)seed ^ (long)(len * 4));
			int num1 = 0;
			while (len > 0)
			{
				int num2 = num1;
				num1 = num2 + 1;
				uint num3 = (uint)key[num2];
				num3 = num3 * 1540483477;
				num3 = num3 ^ num3 >> 24;
				num3 = num3 * 1540483477;
				num = num * 1540483477;
				num = num ^ num3;
				len--;
			}
			num = num ^ num >> 13;
			num = num * 1540483477;
			num = num ^ num >> 15;
			return num;
		}

		public static uint UINT(int[] key, uint seed)
		{
			return MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static uint UINT(ulong[] key, int len, uint seed)
		{
			uint num = (uint)((ulong)seed ^ (long)(len * 8));
			int num1 = 0;
			while (len > 0)
			{
				uint num2 = (uint)(key[num1] & (ulong)-1);
				num2 = num2 * 1540483477;
				num2 = num2 ^ num2 >> 24;
				num2 = num2 * 1540483477;
				num = num * 1540483477;
				num = num ^ num2;
				uint num3 = (uint)(key[num1] >> 32 & (ulong)-1);
				num3 = num3 * 1540483477;
				num3 = num3 ^ num3 >> 24;
				num3 = num3 * 1540483477;
				num = num * 1540483477;
				num = num ^ num3;
				len--;
			}
			num = num ^ num >> 13;
			num = num * 1540483477;
			num = num ^ num >> 15;
			return num;
		}

		public static uint UINT(ulong[] key, uint seed)
		{
			return MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static uint UINT(long[] key, int len, uint seed)
		{
			uint num = (uint)((ulong)seed ^ (long)(len * 8));
			int num1 = 0;
			while (len > 0)
			{
				uint num2 = (uint)(key[num1] & (ulong)-1);
				num2 = num2 * 1540483477;
				num2 = num2 ^ num2 >> 24;
				num2 = num2 * 1540483477;
				num = num * 1540483477;
				num = num ^ num2;
				uint num3 = (uint)(key[num1] >> 32 & (ulong)-1);
				num3 = num3 * 1540483477;
				num3 = num3 ^ num3 >> 24;
				num3 = num3 * 1540483477;
				num = num * 1540483477;
				num = num ^ num3;
				len--;
			}
			num = num ^ num >> 13;
			num = num * 1540483477;
			num = num ^ num >> 15;
			return num;
		}

		public static uint UINT(long[] key, uint seed)
		{
			return MurmurHash2.UINT(key, (int)key.Length, seed);
		}

		public static uint UINT(string key, Encoding encoding, uint seed)
		{
			return MurmurHash2.UINT(encoding.GetBytes(key), seed);
		}

		public static uint UINT_BLOCK(Array key, int len, uint seed)
		{
			uint num = (uint)((ulong)seed ^ (long)(len * 1));
			int num1 = 0;
			while (len >= 4)
			{
				int num2 = num1;
				num1 = num2 + 1;
				int num3 = num1;
				num1 = num3 + 1;
				int num4 = num1;
				num1 = num4 + 1;
				int num5 = num1;
				num1 = num5 + 1;
				uint num6 = (uint)(Buffer.GetByte(key, num2) | Buffer.GetByte(key, num3) << 8 | Buffer.GetByte(key, num4) << 16 | Buffer.GetByte(key, num5) << 24);
				num6 = num6 * 1540483477;
				num6 = num6 ^ num6 >> 24;
				num6 = num6 * 1540483477;
				num = num * 1540483477;
				num = num ^ num6;
				len = len - 4;
			}
			switch (len)
			{
				case 1:
				{
					num = num ^ Buffer.GetByte(key, num1);
					num = num * 1540483477;
					break;
				}
				case 2:
				{
					num = num ^ Buffer.GetByte(key, num1 + 1) << 8;
					num = num ^ Buffer.GetByte(key, num1);
					num = num * 1540483477;
					break;
				}
				case 3:
				{
					num = num ^ Buffer.GetByte(key, num1 + 2) << 16;
					num = num ^ Buffer.GetByte(key, num1 + 1) << 8;
					num = num ^ Buffer.GetByte(key, num1);
					num = num * 1540483477;
					break;
				}
			}
			num = num ^ num >> 13;
			num = num * 1540483477;
			num = num ^ num >> 15;
			return num;
		}

		public static uint UINT_BLOCK(Array key, uint seed)
		{
			return MurmurHash2.UINT_BLOCK(key, Buffer.ByteLength(key), seed);
		}
	}
}