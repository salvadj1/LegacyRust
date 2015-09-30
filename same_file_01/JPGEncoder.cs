using System;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;

public class JPGEncoder
{
	private int[] ZigZag = new int[] { 0, 1, 5, 6, 14, 15, 27, 28, 2, 4, 7, 13, 16, 26, 29, 42, 3, 8, 12, 17, 25, 30, 41, 43, 9, 11, 18, 24, 31, 40, 44, 53, 10, 19, 23, 32, 39, 45, 52, 54, 20, 22, 33, 38, 46, 51, 55, 60, 21, 34, 37, 47, 50, 56, 59, 61, 35, 36, 48, 49, 57, 58, 62, 63 };

	private int[] YTable = new int[64];

	private int[] UVTable = new int[64];

	private float[] fdtbl_Y = new float[64];

	private float[] fdtbl_UV = new float[64];

	private JPGEncoder.BitString[] YDC_HT;

	private JPGEncoder.BitString[] UVDC_HT;

	private JPGEncoder.BitString[] YAC_HT;

	private JPGEncoder.BitString[] UVAC_HT;

	private byte[] std_dc_luminance_nrcodes = new byte[] { typeof(<PrivateImplementationDetails>).GetField("$$field-12").FieldHandle };

	private byte[] std_dc_luminance_values = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

	private byte[] std_ac_luminance_nrcodes = new byte[] { typeof(<PrivateImplementationDetails>).GetField("$$field-14").FieldHandle };

	private byte[] std_ac_luminance_values = new byte[] { typeof(<PrivateImplementationDetails>).GetField("$$field-15").FieldHandle };

	private byte[] std_dc_chrominance_nrcodes = new byte[] { typeof(<PrivateImplementationDetails>).GetField("$$field-16").FieldHandle };

	private byte[] std_dc_chrominance_values = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

	private byte[] std_ac_chrominance_nrcodes = new byte[] { typeof(<PrivateImplementationDetails>).GetField("$$field-18").FieldHandle };

	private byte[] std_ac_chrominance_values = new byte[] { typeof(<PrivateImplementationDetails>).GetField("$$field-19").FieldHandle };

	private JPGEncoder.BitString[] bitcode = new JPGEncoder.BitString[65535];

	private int[] category = new int[65535];

	private uint bytenew;

	private int bytepos = 7;

	private JPGEncoder.ByteArray byteout = new JPGEncoder.ByteArray();

	private int[] DU = new int[64];

	private float[] YDU = new float[64];

	private float[] UDU = new float[64];

	private float[] VDU = new float[64];

	public bool isDone;

	private JPGEncoder.BitmapData image;

	private int sf;

	private string path;

	private int cores;

	public JPGEncoder(Texture2D texture, float quality) : this(texture, quality, string.Empty, false)
	{
	}

	public JPGEncoder(Texture2D texture, float quality, bool blocking) : this(texture, quality, string.Empty, blocking)
	{
	}

	public JPGEncoder(Texture2D texture, float quality, string path) : this(texture, quality, path, false)
	{
	}

	public JPGEncoder(Texture2D texture, float quality, string path, bool blocking)
	{
		this.path = path;
		this.image = new JPGEncoder.BitmapData(texture);
		quality = Mathf.Clamp(quality, 1f, 100f);
		this.sf = (quality >= 50f ? (int)(200f - quality * 2f) : (int)(5000f / quality));
		this.cores = SystemInfo.processorCount;
		Thread thread = new Thread(new ThreadStart(this.DoEncoding));
		thread.Start();
		if (blocking)
		{
			thread.Join();
		}
	}

	private JPGEncoder.BitString[] ComputeHuffmanTbl(byte[] nrcodes, byte[] std_table)
	{
		int num = 0;
		int num1 = 0;
		JPGEncoder.BitString[] bitStringArray = new JPGEncoder.BitString[256];
		for (int i = 1; i <= 16; i++)
		{
			for (int j = 1; j <= nrcodes[i]; j++)
			{
				JPGEncoder.BitString bitString = new JPGEncoder.BitString();
				bitStringArray[std_table[num1]] = bitString;
				bitStringArray[std_table[num1]].@value = num;
				bitStringArray[std_table[num1]].length = i;
				num1++;
				num++;
			}
			num = num * 2;
		}
		return bitStringArray;
	}

	private void DoEncoding()
	{
		this.isDone = false;
		this.InitHuffmanTbl();
		this.InitCategoryfloat();
		this.InitQuantTables(this.sf);
		this.Encode();
		if (!string.IsNullOrEmpty(this.path))
		{
			File.WriteAllBytes(this.path, this.GetBytes());
		}
		this.isDone = true;
	}

	private void Encode()
	{
		this.byteout = new JPGEncoder.ByteArray();
		this.bytenew = 0;
		this.bytepos = 7;
		this.WriteWord(65496);
		this.WriteAPP0();
		this.WriteDQT();
		this.WriteSOF0(this.image.width, this.image.height);
		this.WriteDHT();
		this.writeSOS();
		float single = 0f;
		float single1 = 0f;
		float single2 = 0f;
		this.bytenew = 0;
		this.bytepos = 7;
		for (int i = 0; i < this.image.height; i = i + 8)
		{
			for (int j = 0; j < this.image.width; j = j + 8)
			{
				this.RGB2YUV(this.image, j, i);
				single = this.ProcessDU(this.YDU, this.fdtbl_Y, single, this.YDC_HT, this.YAC_HT);
				single1 = this.ProcessDU(this.UDU, this.fdtbl_UV, single1, this.UVDC_HT, this.UVAC_HT);
				single2 = this.ProcessDU(this.VDU, this.fdtbl_UV, single2, this.UVDC_HT, this.UVAC_HT);
				if (this.cores == 1)
				{
					Thread.Sleep(0);
				}
			}
		}
		if (this.bytepos >= 0)
		{
			JPGEncoder.BitString bitString = new JPGEncoder.BitString()
			{
				length = this.bytepos + 1,
				@value = (1 << (this.bytepos + 1 & 31)) - 1
			};
			this.WriteBits(bitString);
		}
		this.WriteWord(65497);
		this.isDone = true;
	}

	private float[] FDCTQuant(float[] data, float[] fdtbl)
	{
		float single;
		float single1;
		float single2;
		float single3;
		float single4;
		float single5;
		float single6;
		float single7;
		float single8;
		float single9;
		float single10;
		float single11;
		float single12;
		float single13;
		float single14;
		float single15;
		float single16;
		float single17;
		float single18;
		int i;
		int num = 0;
		for (i = 0; i < 8; i++)
		{
			single = data[num] + data[num + 7];
			single7 = data[num] - data[num + 7];
			single1 = data[num + 1] + data[num + 6];
			single6 = data[num + 1] - data[num + 6];
			single2 = data[num + 2] + data[num + 5];
			single5 = data[num + 2] - data[num + 5];
			single3 = data[num + 3] + data[num + 4];
			single4 = data[num + 3] - data[num + 4];
			single8 = single + single3;
			single11 = single - single3;
			single9 = single1 + single2;
			single10 = single1 - single2;
			data[num] = single8 + single9;
			data[num + 4] = single8 - single9;
			single12 = (single10 + single11) * 0.707106769f;
			data[num + 2] = single11 + single12;
			data[num + 6] = single11 - single12;
			single8 = single4 + single5;
			single9 = single5 + single6;
			single10 = single6 + single7;
			single16 = (single8 - single10) * 0.382683426f;
			single13 = 0.5411961f * single8 + single16;
			single15 = 1.306563f * single10 + single16;
			single14 = single9 * 0.707106769f;
			single17 = single7 + single14;
			single18 = single7 - single14;
			data[num + 5] = single18 + single13;
			data[num + 3] = single18 - single13;
			data[num + 1] = single17 + single15;
			data[num + 7] = single17 - single15;
			num = num + 8;
		}
		num = 0;
		for (i = 0; i < 8; i++)
		{
			single = data[num] + data[num + 56];
			single7 = data[num] - data[num + 56];
			single1 = data[num + 8] + data[num + 48];
			single6 = data[num + 8] - data[num + 48];
			single2 = data[num + 16] + data[num + 40];
			single5 = data[num + 16] - data[num + 40];
			single3 = data[num + 24] + data[num + 32];
			single4 = data[num + 24] - data[num + 32];
			single8 = single + single3;
			single11 = single - single3;
			single9 = single1 + single2;
			single10 = single1 - single2;
			data[num] = single8 + single9;
			data[num + 32] = single8 - single9;
			single12 = (single10 + single11) * 0.707106769f;
			data[num + 16] = single11 + single12;
			data[num + 48] = single11 - single12;
			single8 = single4 + single5;
			single9 = single5 + single6;
			single10 = single6 + single7;
			single16 = (single8 - single10) * 0.382683426f;
			single13 = 0.5411961f * single8 + single16;
			single15 = 1.306563f * single10 + single16;
			single14 = single9 * 0.707106769f;
			single17 = single7 + single14;
			single18 = single7 - single14;
			data[num + 40] = single18 + single13;
			data[num + 24] = single18 - single13;
			data[num + 8] = single17 + single15;
			data[num + 56] = single17 - single15;
			num++;
		}
		for (i = 0; i < 64; i++)
		{
			data[i] = Mathf.Round(data[i] * fdtbl[i]);
		}
		return data;
	}

	public byte[] GetBytes()
	{
		if (!this.isDone)
		{
			Debug.LogError("JPEGEncoder not complete, cannot get bytes!");
			return null;
		}
		return this.byteout.GetAllBytes();
	}

	private void InitCategoryfloat()
	{
		int j;
		JPGEncoder.BitString bitString;
		int num = 1;
		int num1 = 2;
		for (int i = 1; i <= 15; i++)
		{
			for (j = num; j < num1; j++)
			{
				this.category[32767 + j] = i;
				bitString = new JPGEncoder.BitString()
				{
					length = i,
					@value = j
				};
				this.bitcode[32767 + j] = bitString;
			}
			for (j = -(num1 - 1); j <= -num; j++)
			{
				this.category[32767 + j] = i;
				bitString = new JPGEncoder.BitString()
				{
					length = i,
					@value = num1 - 1 + j
				};
				this.bitcode[32767 + j] = bitString;
			}
			num = num << 1;
			num1 = num1 << 1;
		}
	}

	private void InitHuffmanTbl()
	{
		this.YDC_HT = this.ComputeHuffmanTbl(this.std_dc_luminance_nrcodes, this.std_dc_luminance_values);
		this.UVDC_HT = this.ComputeHuffmanTbl(this.std_dc_chrominance_nrcodes, this.std_dc_chrominance_values);
		this.YAC_HT = this.ComputeHuffmanTbl(this.std_ac_luminance_nrcodes, this.std_ac_luminance_values);
		this.UVAC_HT = this.ComputeHuffmanTbl(this.std_ac_chrominance_nrcodes, this.std_ac_chrominance_values);
	}

	private void InitQuantTables(int sf)
	{
		int i;
		float single;
		int[] numArray = new int[] { 16, 11, 10, 16, 24, 40, 51, 61, 12, 12, 14, 19, 26, 58, 60, 55, 14, 13, 16, 24, 40, 57, 69, 56, 14, 17, 22, 29, 51, 87, 80, 62, 18, 22, 37, 56, 68, 109, 103, 77, 24, 35, 55, 64, 81, 104, 113, 92, 49, 64, 78, 87, 103, 121, 120, 101, 72, 92, 95, 98, 112, 100, 103, 99 };
		for (i = 0; i < 64; i++)
		{
			single = Mathf.Floor((float)((numArray[i] * sf + 50) / 100));
			single = Mathf.Clamp(single, 1f, 255f);
			this.YTable[this.ZigZag[i]] = Mathf.RoundToInt(single);
		}
		int[] numArray1 = new int[] { 17, 18, 24, 47, 99, 99, 99, 99, 18, 21, 26, 66, 99, 99, 99, 99, 24, 26, 56, 99, 99, 99, 99, 99, 47, 66, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99 };
		for (i = 0; i < 64; i++)
		{
			single = Mathf.Floor((float)((numArray1[i] * sf + 50) / 100));
			single = Mathf.Clamp(single, 1f, 255f);
			this.UVTable[this.ZigZag[i]] = (int)single;
		}
		float[] singleArray = new float[] { 1f, 1.3870399f, 1.306563f, 1.17587554f, 1f, 0.785694957f, 0.5411961f, 0.27589938f };
		i = 0;
		for (int j = 0; j < 8; j++)
		{
			for (int k = 0; k < 8; k++)
			{
				this.fdtbl_Y[i] = 1f / ((float)this.YTable[this.ZigZag[i]] * singleArray[j] * singleArray[k] * 8f);
				this.fdtbl_UV[i] = 1f / ((float)this.UVTable[this.ZigZag[i]] * singleArray[j] * singleArray[k] * 8f);
				i++;
			}
		}
	}

	private float ProcessDU(float[] CDU, float[] fdtbl, float DC, JPGEncoder.BitString[] HTDC, JPGEncoder.BitString[] HTAC)
	{
		int i;
		JPGEncoder.BitString hTAC = HTAC[0];
		JPGEncoder.BitString bitString = HTAC[240];
		float[] singleArray = this.FDCTQuant(CDU, fdtbl);
		for (i = 0; i < 64; i++)
		{
			this.DU[this.ZigZag[i]] = (int)singleArray[i];
		}
		int dU = (int)((float)this.DU[0] - DC);
		DC = (float)this.DU[0];
		if (dU != 0)
		{
			this.WriteBits(HTDC[this.category[32767 + dU]]);
			this.WriteBits(this.bitcode[32767 + dU]);
		}
		else
		{
			this.WriteBits(HTDC[0]);
		}
		int num = 63;
		while (num > 0 && this.DU[num] == 0)
		{
			num--;
		}
		if (num == 0)
		{
			this.WriteBits(hTAC);
			return DC;
		}
		for (i = 1; i <= num; i++)
		{
			int num1 = i;
			while (this.DU[i] == 0 && i <= num)
			{
				i++;
			}
			int num2 = i - num1;
			if (num2 >= 16)
			{
				for (int j = 1; j <= num2 / 16; j++)
				{
					this.WriteBits(bitString);
				}
				num2 = num2 & 15;
			}
			this.WriteBits(HTAC[num2 * 16 + this.category[32767 + this.DU[i]]]);
			this.WriteBits(this.bitcode[32767 + this.DU[i]]);
		}
		if (num != 63)
		{
			this.WriteBits(hTAC);
		}
		return DC;
	}

	private void RGB2YUV(JPGEncoder.BitmapData image, int xpos, int ypos)
	{
		int num = 0;
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				Color32 pixelColor = image.GetPixelColor(xpos + j, image.height - (ypos + i));
				this.YDU[num] = 0.299f * (float)pixelColor.r + 0.587f * (float)pixelColor.g + 0.114f * (float)pixelColor.b - 128f;
				this.UDU[num] = -0.16874f * (float)pixelColor.r + -0.33126f * (float)pixelColor.g + 0.5f * (float)pixelColor.b;
				this.VDU[num] = 0.5f * (float)pixelColor.r + -0.41869f * (float)pixelColor.g + -0.08131f * (float)pixelColor.b;
				num++;
			}
		}
	}

	private void WriteAPP0()
	{
		this.WriteWord(65504);
		this.WriteWord(16);
		this.WriteByte(74);
		this.WriteByte(70);
		this.WriteByte(73);
		this.WriteByte(70);
		this.WriteByte(0);
		this.WriteByte(1);
		this.WriteByte(1);
		this.WriteByte(0);
		this.WriteWord(1);
		this.WriteWord(1);
		this.WriteByte(0);
		this.WriteByte(0);
	}

	private void WriteBits(JPGEncoder.BitString bs)
	{
		int num = bs.@value;
		int num1 = bs.length - 1;
		while (num1 >= 0)
		{
			if (((long)num & (ulong)Convert.ToUInt32(1 << (num1 & 31))) != 0)
			{
				JPGEncoder jPGEncoder = this;
				jPGEncoder.bytenew = jPGEncoder.bytenew | Convert.ToUInt32(1 << (this.bytepos & 31));
			}
			num1--;
			JPGEncoder jPGEncoder1 = this;
			jPGEncoder1.bytepos = jPGEncoder1.bytepos - 1;
			if (this.bytepos >= 0)
			{
				continue;
			}
			if (this.bytenew != 255)
			{
				this.WriteByte((byte)this.bytenew);
			}
			else
			{
				this.WriteByte(255);
				this.WriteByte(0);
			}
			this.bytepos = 7;
			this.bytenew = 0;
		}
	}

	private void WriteByte(byte value)
	{
		this.byteout.WriteByte(value);
	}

	private void WriteDHT()
	{
		int i;
		this.WriteWord(65476);
		this.WriteWord(418);
		this.WriteByte(0);
		for (i = 0; i < 16; i++)
		{
			this.WriteByte(this.std_dc_luminance_nrcodes[i + 1]);
		}
		for (i = 0; i <= 11; i++)
		{
			this.WriteByte(this.std_dc_luminance_values[i]);
		}
		this.WriteByte(16);
		for (i = 0; i < 16; i++)
		{
			this.WriteByte(this.std_ac_luminance_nrcodes[i + 1]);
		}
		for (i = 0; i <= 161; i++)
		{
			this.WriteByte(this.std_ac_luminance_values[i]);
		}
		this.WriteByte(1);
		for (i = 0; i < 16; i++)
		{
			this.WriteByte(this.std_dc_chrominance_nrcodes[i + 1]);
		}
		for (i = 0; i <= 11; i++)
		{
			this.WriteByte(this.std_dc_chrominance_values[i]);
		}
		this.WriteByte(17);
		for (i = 0; i < 16; i++)
		{
			this.WriteByte(this.std_ac_chrominance_nrcodes[i + 1]);
		}
		for (i = 0; i <= 161; i++)
		{
			this.WriteByte(this.std_ac_chrominance_values[i]);
		}
	}

	private void WriteDQT()
	{
		int i;
		this.WriteWord(65499);
		this.WriteWord(132);
		this.WriteByte(0);
		for (i = 0; i < 64; i++)
		{
			this.WriteByte((byte)this.YTable[i]);
		}
		this.WriteByte(1);
		for (i = 0; i < 64; i++)
		{
			this.WriteByte((byte)this.UVTable[i]);
		}
	}

	private void WriteSOF0(int width, int height)
	{
		this.WriteWord(65472);
		this.WriteWord(17);
		this.WriteByte(8);
		this.WriteWord(height);
		this.WriteWord(width);
		this.WriteByte(3);
		this.WriteByte(1);
		this.WriteByte(17);
		this.WriteByte(0);
		this.WriteByte(2);
		this.WriteByte(17);
		this.WriteByte(1);
		this.WriteByte(3);
		this.WriteByte(17);
		this.WriteByte(1);
	}

	private void writeSOS()
	{
		this.WriteWord(65498);
		this.WriteWord(12);
		this.WriteByte(3);
		this.WriteByte(1);
		this.WriteByte(0);
		this.WriteByte(2);
		this.WriteByte(17);
		this.WriteByte(3);
		this.WriteByte(17);
		this.WriteByte(0);
		this.WriteByte(63);
		this.WriteByte(0);
	}

	private void WriteWord(int value)
	{
		this.WriteByte((byte)(value >> 8 & 255));
		this.WriteByte((byte)(value & 255));
	}

	private class BitmapData
	{
		public int height;

		public int width;

		private Color32[] pixels;

		public BitmapData(Texture2D texture)
		{
			this.height = texture.height;
			this.width = texture.width;
			this.pixels = texture.GetPixels32();
		}

		public Color32 GetPixelColor(int x, int y)
		{
			x = Mathf.Clamp(x, 0, this.width - 1);
			y = Mathf.Clamp(y, 0, this.height - 1);
			return this.pixels[y * this.width + x];
		}
	}

	private struct BitString
	{
		public int length;

		public int @value;
	}

	private class ByteArray
	{
		private MemoryStream stream;

		private BinaryWriter writer;

		public ByteArray()
		{
			this.stream = new MemoryStream();
			this.writer = new BinaryWriter(this.stream);
		}

		public byte[] GetAllBytes()
		{
			byte[] numArray = new byte[checked((IntPtr)this.stream.Length)];
			this.stream.Position = (long)0;
			this.stream.Read(numArray, 0, (int)numArray.Length);
			return numArray;
		}

		public void WriteByte(byte value)
		{
			this.writer.Write(value);
		}
	}
}