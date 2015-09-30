using System;
using System.Security.Cryptography;
using System.Text;

public sealed class CRC32 : HashAlgorithm
{
	public const uint kDefaultPolynomial = 3988292384;

	public const uint kDefaultSeed = 4294967295;

	public const uint kTableSize = 256;

	private const uint I = 256;

	private const uint J = 8;

	private uint hash;

	private readonly uint seed;

	private readonly uint[] table;

	public sealed override int HashSize
	{
		get
		{
			return 32;
		}
	}

	public CRC32()
	{
		this.table = CRC32.Default.Table;
		this.seed = -1;
		this.Initialize();
	}

	public CRC32(uint polynomial, uint seed)
	{
		this.table = (polynomial != -306674912 ? CRC32.ProcessHashTable(polynomial) : CRC32.Default.Table);
		this.seed = seed;
		this.Initialize();
	}

	private static uint BufferHash(uint[] table, uint seed, byte[] buffer, int start, int size)
	{
		unsafe
		{
			while (true)
			{
				int num = size;
				size = num - 1;
				if (num <= 0)
				{
					break;
				}
				int num1 = start;
				start = num1 + 1;
				seed = seed >> 8 ^ table[buffer[num1] ^ seed & 255];
			}
			return seed;
		}
	}

	private void BufferHash(byte[] buffer, int start, int size)
	{
		unsafe
		{
			while (true)
			{
				int num = size;
				size = num - 1;
				if (num <= 0)
				{
					break;
				}
				uint[] numArray = this.table;
				int num1 = start;
				start = num1 + 1;
				this.hash = this.hash >> 8 ^ numArray[buffer[num1] ^ this.hash & 255];
			}
		}
	}

	protected sealed override void HashCore(byte[] buffer, int start, int length)
	{
		this.BufferHash(buffer, start, length);
	}

	protected sealed override byte[] HashFinal()
	{
		uint num = ~this.hash;
		byte[] numArray = new byte[] { (byte)(num >> 24 & 255), (byte)(num >> 16 & 255), (byte)(num >> 8 & 255), (byte)(num & 255) };
		this.HashValue = numArray;
		return numArray;
	}

	public override void Initialize()
	{
		this.hash = this.seed;
	}

	private static uint[] ProcessHashTable(uint p)
	{
		unsafe
		{
			uint[] numArray = new uint[256];
			for (ushort i = 0; i < 256; i = (ushort)(i + 1))
			{
				numArray[i] = i;
				for (uint j = 0; j < 8; j++)
				{
					numArray[i] = ((numArray[i] & 1) != 1 ? numArray[i] >> 1 : numArray[i] >> 1 ^ p);
				}
			}
			return numArray;
		}
	}

	public static uint Quick(byte[] buffer)
	{
		return ~CRC32.BufferHash(CRC32.Default.Table, -1, buffer, 0, (int)buffer.Length);
	}

	public static uint Quick(uint seed, byte[] buffer)
	{
		return ~CRC32.BufferHash(CRC32.Default.Table, seed, buffer, 0, (int)buffer.Length);
	}

	public static uint Quick(uint polynomial, uint seed, byte[] buffer)
	{
		return ~CRC32.BufferHash(CRC32.Default.Table, seed, buffer, 0, (int)buffer.Length);
	}

	public static uint String(string str)
	{
		return CRC32.Quick(Encoding.ASCII.GetBytes(str));
	}

	private static class Default
	{
		public readonly static uint[] Table;

		static Default()
		{
			unsafe
			{
				CRC32.Default.Table = new uint[256];
				for (uint i = 0; i < 256; i++)
				{
					CRC32.Default.Table[i] = i;
					for (uint j = 0; j < 8; j++)
					{
						CRC32.Default.Table[i] = ((CRC32.Default.Table[i] & 1) != 1 ? CRC32.Default.Table[i] >> 1 : CRC32.Default.Table[i] >> 1 ^ -306674912);
					}
				}
			}
		}
	}
}