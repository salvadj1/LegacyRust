using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BMFont
{
	[HideInInspector]
	[SerializeField]
	private BMGlyph[] mGlyphs;

	[HideInInspector]
	[SerializeField]
	private int mSize;

	[HideInInspector]
	[SerializeField]
	private int mBase;

	[HideInInspector]
	[SerializeField]
	private int mWidth;

	[HideInInspector]
	[SerializeField]
	private int mHeight;

	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	[HideInInspector]
	[SerializeField]
	private List<BMGlyph> mSaved = new List<BMGlyph>();

	[HideInInspector]
	[SerializeField]
	private List<BMSymbol> mSymbols = new List<BMSymbol>();

	[NonSerialized]
	private Dictionary<int, BMGlyph> mDict;

	[NonSerialized]
	private bool mDictMade;

	[NonSerialized]
	private bool mDictAny;

	public int baseOffset
	{
		get
		{
			return this.mBase;
		}
		set
		{
			this.mBase = value;
		}
	}

	public int charSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			this.mSize = value;
		}
	}

	public int glyphCount
	{
		get
		{
			return (!this.isValid ? 0 : this.mSaved.Count);
		}
	}

	public bool isValid
	{
		get
		{
			return (this.mSaved.Count > 0 ? true : this.LegacyCheck());
		}
	}

	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			this.mSpriteName = value;
		}
	}

	public List<BMSymbol> symbols
	{
		get
		{
			return this.mSymbols;
		}
	}

	public int texHeight
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			this.mHeight = value;
		}
	}

	public int texWidth
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			this.mWidth = value;
		}
	}

	public BMFont()
	{
	}

	public void Clear()
	{
		this.mGlyphs = null;
		this.mDict = null;
		int num = 0;
		bool flag = (bool)num;
		this.mDictMade = (bool)num;
		this.mDictAny = flag;
		this.mSaved.Clear();
	}

	public bool ContainsGlyph(int index)
	{
		if (!this.mDictMade)
		{
			this.mDictMade = true;
			int count = this.mSaved.Count;
			if (count == 0 && this.LegacyCheck())
			{
				count = this.mSaved.Count;
			}
			this.mDictAny = count > 0;
			if (this.mDictAny)
			{
				this.mDict = BMFont.CreateGlyphDictionary(count);
				for (int i = count - 1; i >= 0; i--)
				{
					BMGlyph item = this.mSaved[i];
					this.mDict.Add(item.index, item);
					if (item.index == index)
					{
						while (true)
						{
							int num = i - 1;
							i = num;
							if (num < 0)
							{
								break;
							}
							item = this.mSaved[i];
							this.mDict.Add(item.index, item);
						}
						return true;
					}
				}
			}
		}
		else if (this.mDictAny && this.mDict.ContainsKey(index))
		{
			return true;
		}
		return false;
	}

	private static Dictionary<int, BMGlyph> CreateGlyphDictionary()
	{
		return new Dictionary<int, BMGlyph>();
	}

	private static Dictionary<int, BMGlyph> CreateGlyphDictionary(int cap)
	{
		return new Dictionary<int, BMGlyph>(cap);
	}

	private int GetArraySize(int index)
	{
		if (index < 256)
		{
			return 256;
		}
		if (index < 65536)
		{
			return 65536;
		}
		if (index < 262144)
		{
			return 262144;
		}
		return 0;
	}

	public bool GetGlyph(int index, out BMGlyph glyph)
	{
		if (!this.mDictMade)
		{
			this.mDictMade = true;
			int count = this.mSaved.Count;
			if (count == 0 && this.LegacyCheck())
			{
				count = this.mSaved.Count;
			}
			this.mDictAny = count > 0;
			if (this.mDictAny)
			{
				this.mDict = BMFont.CreateGlyphDictionary(count);
				for (int i = count - 1; i >= 0; i--)
				{
					BMGlyph item = this.mSaved[i];
					this.mDict.Add(item.index, item);
					if (item.index == index)
					{
						glyph = item;
						while (true)
						{
							int num = i - 1;
							i = num;
							if (num < 0)
							{
								break;
							}
							item = this.mSaved[i];
							this.mDict.Add(item.index, item);
						}
						return true;
					}
				}
			}
		}
		else if (this.mDictAny)
		{
			return this.mDict.TryGetValue(index, out glyph);
		}
		glyph = null;
		return false;
	}

	public BMFont.GetOrCreateGlyphResult GetOrCreateGlyph(int index, out BMGlyph glyph)
	{
		if (!this.mDictMade)
		{
			this.mDictMade = true;
			this.mDictAny = true;
			int count = this.mSaved.Count;
			if (count == 0 && this.LegacyCheck())
			{
				count = this.mSaved.Count;
			}
			if (count <= 0)
			{
				this.mDict = BMFont.CreateGlyphDictionary();
			}
			else
			{
				this.mDict = BMFont.CreateGlyphDictionary(count + 1);
				for (int i = count - 1; i >= 0; i--)
				{
					BMGlyph item = this.mSaved[i];
					this.mDict.Add(item.index, item);
					if (item.index == index)
					{
						glyph = item;
						while (true)
						{
							int num = i - 1;
							i = num;
							if (num < 0)
							{
								break;
							}
							item = this.mSaved[i];
							this.mDict.Add(item.index, item);
						}
						return BMFont.GetOrCreateGlyphResult.Found | BMFont.GetOrCreateGlyphResult.Created;
					}
				}
			}
		}
		else if (!this.mDictAny)
		{
			this.mDict = BMFont.CreateGlyphDictionary();
			this.mDictAny = true;
		}
		else if (this.mDict.TryGetValue(index, out glyph))
		{
			return BMFont.GetOrCreateGlyphResult.Found | BMFont.GetOrCreateGlyphResult.Created;
		}
		glyph = new BMGlyph()
		{
			index = index
		};
		this.mDict.Add(index, glyph);
		return BMFont.GetOrCreateGlyphResult.Created;
	}

	public BMSymbol GetSymbol(string sequence, bool createIfMissing)
	{
		int num = 0;
		int count = this.mSymbols.Count;
		while (num < count)
		{
			BMSymbol item = this.mSymbols[num];
			if (item.sequence == sequence)
			{
				return item;
			}
			num++;
		}
		if (!createIfMissing)
		{
			return null;
		}
		BMSymbol bMSymbol = new BMSymbol()
		{
			sequence = sequence
		};
		this.mSymbols.Add(bMSymbol);
		return bMSymbol;
	}

	public bool LegacyCheck()
	{
		if (this.mGlyphs == null || (int)this.mGlyphs.Length <= 0)
		{
			return false;
		}
		int num = 0;
		int length = (int)this.mGlyphs.Length;
		while (num < length)
		{
			BMGlyph bMGlyph = this.mGlyphs[num];
			if (bMGlyph != null)
			{
				bMGlyph.index = num;
				this.mSaved.Add(bMGlyph);
				while (true)
				{
					int num1 = num + 1;
					num = num1;
					if (num1 >= length)
					{
						break;
					}
					if (bMGlyph != null)
					{
						bMGlyph.index = num;
						this.mSaved.Add(bMGlyph);
					}
				}
				this.mGlyphs = null;
				return true;
			}
			num++;
		}
		this.mGlyphs = null;
		return false;
	}

	public bool MatchSymbol(string text, int offset, int textLength, out BMSymbol symbol)
	{
		int num;
		int count = this.mSymbols.Count;
		if (count > 0)
		{
			textLength = textLength - offset;
			if (textLength > 0)
			{
				for (int i = 0; i < count; i++)
				{
					BMSymbol item = this.mSymbols[i];
					int length = item.sequence.Length;
					if (length != 0 && textLength >= length)
					{
						if (string.Compare(item.sequence, 0, text, offset, length) == 0)
						{
							symbol = item;
							if (length < textLength)
							{
								int num1 = i + 1;
								i = num1;
								if (num1 < count)
								{
									int num2 = length;
									do
									{
										item = this.mSymbols[i];
										length = item.sequence.Length;
										if (textLength >= length && length > num2)
										{
											if (string.Compare(item.sequence, 0, text, offset, length) == 0)
											{
												num2 = length;
												symbol = item;
											}
										}
										num = i + 1;
										i = num;
									}
									while (num < count);
								}
							}
							return true;
						}
					}
				}
			}
		}
		symbol = null;
		return false;
	}

	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		if (this.isValid)
		{
			int num = 0;
			int count = this.mSaved.Count;
			while (num < count)
			{
				BMGlyph item = this.mSaved[num];
				if (item != null)
				{
					item.Trim(xMin, yMin, xMax, yMax);
				}
				num++;
			}
		}
	}

	public enum GetOrCreateGlyphResult : sbyte
	{
		Found = -1,
		Failed = 0,
		Created = 1
	}
}