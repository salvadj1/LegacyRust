using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ByteReader
{
	private byte[] mBuffer;

	private int mOffset;

	public bool canRead
	{
		get
		{
			return (this.mBuffer == null ? false : this.mOffset < (int)this.mBuffer.Length);
		}
	}

	public ByteReader(byte[] bytes)
	{
		this.mBuffer = bytes;
	}

	public ByteReader(TextAsset asset)
	{
		this.mBuffer = asset.bytes;
	}

	public Dictionary<string, string> ReadDictionary()
	{
		Dictionary<string, string> strs = new Dictionary<string, string>();
		char[] chrArray = new char[] { '=' };
		while (this.canRead)
		{
			string str = this.ReadLine();
			if (str != null)
			{
				string[] strArrays = str.Split(chrArray, 2, StringSplitOptions.RemoveEmptyEntries);
				if ((int)strArrays.Length != 2)
				{
					continue;
				}
				string str1 = strArrays[0].Trim();
				strs[str1] = strArrays[1].Trim();
			}
			else
			{
				break;
			}
		}
		return strs;
	}

	private static string ReadLine(byte[] buffer, int start, int count)
	{
		return Encoding.UTF8.GetString(buffer, start, count);
	}

	public string ReadLine()
	{
		int length = (int)this.mBuffer.Length;
		while (this.mOffset < length && this.mBuffer[this.mOffset] < 32)
		{
			ByteReader byteReader = this;
			byteReader.mOffset = byteReader.mOffset + 1;
		}
		int num = this.mOffset;
		if (num >= length)
		{
			this.mOffset = length;
			return null;
		}
		while (true)
		{
			if (num >= length)
			{
				num++;
				break;
			}
			else
			{
				int num1 = num;
				num = num1 + 1;
				int num2 = this.mBuffer[num1];
				if (num2 == 10 || num2 == 13)
				{
					break;
				}
			}
		}
		string str = ByteReader.ReadLine(this.mBuffer, this.mOffset, num - this.mOffset - 1);
		this.mOffset = num;
		return str;
	}
}