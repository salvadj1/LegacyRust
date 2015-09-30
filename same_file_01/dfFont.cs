using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Font Definition")]
[Serializable]
public class dfFont : dfFontBase
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected string sprite;

	[SerializeField]
	protected string face = string.Empty;

	[SerializeField]
	protected int size;

	[SerializeField]
	protected bool bold;

	[SerializeField]
	protected bool italic;

	[SerializeField]
	protected string charset;

	[SerializeField]
	protected int stretchH;

	[SerializeField]
	protected bool smooth;

	[SerializeField]
	protected int aa;

	[SerializeField]
	protected int[] padding;

	[SerializeField]
	protected int[] spacing;

	[SerializeField]
	protected int outline;

	[SerializeField]
	protected int lineHeight;

	[SerializeField]
	private List<dfFont.GlyphDefinition> glyphs = new List<dfFont.GlyphDefinition>();

	[SerializeField]
	protected List<dfFont.GlyphKerning> kerning = new List<dfFont.GlyphKerning>();

	private Dictionary<int, dfFont.GlyphDefinition> glyphMap;

	private Dictionary<int, dfFont.GlyphKerningList> kerningMap;

	public dfAtlas Atlas
	{
		get
		{
			return this.atlas;
		}
		set
		{
			if (value != this.atlas)
			{
				this.atlas = value;
				this.glyphMap = null;
			}
		}
	}

	public bool Bold
	{
		get
		{
			return this.bold;
		}
	}

	public int Count
	{
		get
		{
			return this.glyphs.Count;
		}
	}

	public string FontFace
	{
		get
		{
			return this.face;
		}
	}

	public override int FontSize
	{
		get
		{
			return this.size;
		}
		set
		{
			throw new InvalidOperationException();
		}
	}

	public List<dfFont.GlyphDefinition> Glyphs
	{
		get
		{
			return this.glyphs;
		}
	}

	public override bool IsValid
	{
		get
		{
			if (!(this.Atlas == null) && !(this.Atlas[this.Sprite] == null))
			{
				return true;
			}
			return false;
		}
	}

	public bool Italic
	{
		get
		{
			return this.italic;
		}
	}

	public List<dfFont.GlyphKerning> KerningInfo
	{
		get
		{
			return this.kerning;
		}
	}

	public override int LineHeight
	{
		get
		{
			return this.lineHeight;
		}
		set
		{
			throw new InvalidOperationException();
		}
	}

	public override UnityEngine.Material Material
	{
		get
		{
			return this.Atlas.Material;
		}
		set
		{
			throw new InvalidOperationException();
		}
	}

	public int Outline
	{
		get
		{
			return this.outline;
		}
	}

	public int[] Padding
	{
		get
		{
			return this.padding;
		}
	}

	public int[] Spacing
	{
		get
		{
			return this.spacing;
		}
	}

	public string Sprite
	{
		get
		{
			return this.sprite;
		}
		set
		{
			if (value != this.sprite)
			{
				this.sprite = value;
				this.glyphMap = null;
			}
		}
	}

	public override UnityEngine.Texture Texture
	{
		get
		{
			return this.Atlas.Texture;
		}
	}

	public dfFont()
	{
	}

	public void AddKerning(int first, int second, int amount)
	{
		List<dfFont.GlyphKerning> glyphKernings = this.kerning;
		dfFont.GlyphKerning glyphKerning = new dfFont.GlyphKerning()
		{
			first = first,
			second = second,
			amount = amount
		};
		glyphKernings.Add(glyphKerning);
	}

	private void buildKerningMap()
	{
		Dictionary<int, dfFont.GlyphKerningList> nums = new Dictionary<int, dfFont.GlyphKerningList>();
		Dictionary<int, dfFont.GlyphKerningList> nums1 = nums;
		this.kerningMap = nums;
		Dictionary<int, dfFont.GlyphKerningList> glyphKerningList = nums1;
		for (int i = 0; i < this.kerning.Count; i++)
		{
			dfFont.GlyphKerning item = this.kerning[i];
			if (!glyphKerningList.ContainsKey(item.first))
			{
				glyphKerningList[item.first] = new dfFont.GlyphKerningList();
			}
			glyphKerningList[item.first].Add(item);
		}
	}

	public dfFont.GlyphDefinition GetGlyph(char id)
	{
		if (this.glyphMap == null)
		{
			this.glyphMap = new Dictionary<int, dfFont.GlyphDefinition>();
			for (int i = 0; i < this.glyphs.Count; i++)
			{
				dfFont.GlyphDefinition item = this.glyphs[i];
				this.glyphMap[item.id] = item;
			}
		}
		dfFont.GlyphDefinition glyphDefinition = null;
		this.glyphMap.TryGetValue(id, out glyphDefinition);
		return glyphDefinition;
	}

	public int GetKerning(char previousChar, char currentChar)
	{
		int num;
		try
		{
			if (this.kerningMap == null)
			{
				this.buildKerningMap();
			}
			dfFont.GlyphKerningList glyphKerningList = null;
			num = (this.kerningMap.TryGetValue(previousChar, out glyphKerningList) ? glyphKerningList.GetKerning(previousChar, currentChar) : 0);
		}
		finally
		{
		}
		return num;
	}

	public override dfFontRendererBase ObtainRenderer()
	{
		return dfFont.BitmappedFontRenderer.Obtain(this);
	}

	public void OnEnable()
	{
		this.glyphMap = null;
	}

	public class BitmappedFontRenderer : dfFontRendererBase
	{
		private static Queue<dfFont.BitmappedFontRenderer> objectPool;

		private static Vector2[] OUTLINE_OFFSETS;

		private static int[] TRIANGLE_INDICES;

		private static Stack<Color32> textColors;

		private dfList<dfFont.LineRenderInfo> lines;

		private List<dfMarkupToken> tokens;

		public int LineCount
		{
			get
			{
				return this.lines.Count;
			}
		}

		static BitmappedFontRenderer()
		{
			dfFont.BitmappedFontRenderer.objectPool = new Queue<dfFont.BitmappedFontRenderer>();
			dfFont.BitmappedFontRenderer.OUTLINE_OFFSETS = new Vector2[] { new Vector2(-1f, -1f), new Vector2(-1f, 1f), new Vector2(1f, -1f), new Vector2(1f, 1f) };
			dfFont.BitmappedFontRenderer.TRIANGLE_INDICES = new int[] { 0, 1, 3, 3, 1, 2 };
			dfFont.BitmappedFontRenderer.textColors = new Stack<Color32>();
		}

		internal BitmappedFontRenderer()
		{
		}

		private static void addTriangleIndices(dfList<Vector3> verts, dfList<int> triangles)
		{
			int count = verts.Count;
			int[] tRIANGLEINDICES = dfFont.BitmappedFontRenderer.TRIANGLE_INDICES;
			for (int i = 0; i < (int)tRIANGLEINDICES.Length; i++)
			{
				triangles.Add(count + tRIANGLEINDICES[i]);
			}
		}

		private Color32 applyOpacity(Color32 color)
		{
			color.a = (byte)(base.Opacity * 255f);
			return color;
		}

		private int calculateLineAlignment(dfFont.LineRenderInfo line)
		{
			float single = line.lineWidth;
			if (base.TextAlign == TextAlignment.Left || single == 0f)
			{
				return 0;
			}
			int num = 0;
			if (base.TextAlign != TextAlignment.Right)
			{
				Vector2 maxSize = base.MaxSize;
				num = Mathf.FloorToInt((maxSize.x / base.TextScale - single) * 0.5f);
			}
			else
			{
				Vector2 vector2 = base.MaxSize;
				num = Mathf.FloorToInt(vector2.x / base.TextScale - single);
			}
			return Mathf.Max(0, num);
		}

		private dfList<dfFont.LineRenderInfo> calculateLinebreaks()
		{
			dfList<dfFont.LineRenderInfo> lineRenderInfos;
			bool flag;
			try
			{
				if (this.lines == null)
				{
					this.lines = dfList<dfFont.LineRenderInfo>.Obtain();
					int num = 0;
					int num1 = 0;
					int num2 = 0;
					int num3 = 0;
					float lineHeight = (float)base.Font.LineHeight * base.TextScale;
					while (num2 < this.tokens.Count)
					{
						Vector2 maxSize = base.MaxSize;
						if ((float)this.lines.Count * lineHeight >= maxSize.y)
						{
							break;
						}
						dfMarkupToken item = this.tokens[num2];
						dfMarkupTokenType tokenType = item.TokenType;
						if (tokenType != dfMarkupTokenType.Newline)
						{
							int num4 = Mathf.CeilToInt((float)item.Width * base.TextScale);
							if (!base.WordWrap || num <= num1)
							{
								flag = false;
							}
							else if (tokenType == dfMarkupTokenType.Text)
							{
								flag = true;
							}
							else
							{
								flag = (tokenType != dfMarkupTokenType.StartTag ? false : item.Matches("sprite"));
							}
							if (!flag || (float)(num3 + num4) < base.MaxSize.x)
							{
								if (tokenType == dfMarkupTokenType.Whitespace)
								{
									num = num2;
								}
								num3 = num3 + num4;
								num2++;
							}
							else if (num <= num1)
							{
								this.lines.Add(dfFont.LineRenderInfo.Obtain(num1, num - 1));
								int num5 = num2 + 1;
								num2 = num5;
								num = num5;
								num1 = num5;
								num3 = 0;
							}
							else
							{
								this.lines.Add(dfFont.LineRenderInfo.Obtain(num1, num - 1));
								int num6 = num + 1;
								num = num6;
								num2 = num6;
								num1 = num6;
								num3 = 0;
							}
						}
						else
						{
							this.lines.Add(dfFont.LineRenderInfo.Obtain(num1, num2));
							int num7 = num2 + 1;
							num2 = num7;
							num = num7;
							num1 = num7;
							num3 = 0;
						}
					}
					if (num1 < this.tokens.Count)
					{
						this.lines.Add(dfFont.LineRenderInfo.Obtain(num1, this.tokens.Count - 1));
					}
					for (int i = 0; i < this.lines.Count; i++)
					{
						this.calculateLineSize(this.lines[i]);
					}
					lineRenderInfos = this.lines;
				}
				else
				{
					lineRenderInfos = this.lines;
				}
			}
			finally
			{
			}
			return lineRenderInfos;
		}

		private void calculateLineSize(dfFont.LineRenderInfo line)
		{
			line.lineHeight = (float)base.Font.LineHeight;
			int width = 0;
			for (int i = line.startOffset; i <= line.endOffset; i++)
			{
				width = width + this.tokens[i].Width;
			}
			line.lineWidth = (float)width;
		}

		private void calculateTokenRenderSize(dfMarkupToken token)
		{
			try
			{
				dfFont font = (dfFont)base.Font;
				int kerning = 0;
				char chr = '\0';
				char item = '\0';
				if ((token.TokenType == dfMarkupTokenType.Whitespace ? true : token.TokenType == dfMarkupTokenType.Text))
				{
					int num = 0;
					while (num < token.Length)
					{
						item = token[num];
						if (item != '\t')
						{
							dfFont.GlyphDefinition glyph = font.GetGlyph(item);
							if (glyph != null)
							{
								if (num > 0)
								{
									kerning = kerning + font.GetKerning(chr, item);
									kerning = kerning + base.CharacterSpacing;
								}
								kerning = kerning + glyph.xadvance;
							}
						}
						else
						{
							kerning = kerning + base.TabSize;
						}
						num++;
						chr = item;
					}
				}
				else if (token.TokenType == dfMarkupTokenType.StartTag && token.Matches("sprite"))
				{
					if (token.AttributeCount < 1)
					{
						throw new Exception("Missing sprite name in markup");
					}
					UnityEngine.Texture texture = font.Texture;
					int lineHeight = font.LineHeight;
					string value = token.GetAttribute(0).Value.Value;
					dfAtlas.ItemInfo itemInfo = font.atlas[value];
					if (itemInfo != null)
					{
						float single = itemInfo.region.width * (float)texture.width / (itemInfo.region.height * (float)texture.height);
						kerning = Mathf.CeilToInt((float)lineHeight * single);
					}
				}
				token.Height = base.Font.LineHeight;
				token.Width = kerning;
			}
			finally
			{
			}
		}

		private void clipBottom(dfRenderData destination, int startIndex)
		{
			float vectorOffset = base.VectorOffset.y;
			Vector2 maxSize = base.MaxSize;
			float pixelRatio = vectorOffset - maxSize.y * base.PixelRatio;
			dfList<Vector3> vertices = destination.Vertices;
			dfList<Vector2> uV = destination.UV;
			dfList<Color32> colors = destination.Colors;
			for (int i = startIndex; i < vertices.Count; i = i + 4)
			{
				Vector3 item = vertices[i];
				Vector3 vector3 = vertices[i + 1];
				Vector3 item1 = vertices[i + 2];
				Vector3 vector31 = vertices[i + 3];
				float single = item.y - vector31.y;
				if (vector31.y <= pixelRatio)
				{
					float single1 = 1f - Mathf.Abs(-pixelRatio + item.y) / single;
					item = new Vector3(item.x, Mathf.Max(item.y, pixelRatio), vector3.z);
					vertices[i] = item;
					vector3 = new Vector3(vector3.x, Mathf.Max(vector3.y, pixelRatio), vector3.z);
					vertices[i + 1] = vector3;
					item1 = new Vector3(item1.x, Mathf.Max(item1.y, pixelRatio), item1.z);
					vertices[i + 2] = item1;
					vector31 = new Vector3(vector31.x, Mathf.Max(vector31.y, pixelRatio), vector31.z);
					vertices[i + 3] = vector31;
					float item2 = uV[i + 3].y;
					Vector2 vector2 = uV[i];
					float single2 = Mathf.Lerp(item2, vector2.y, single1);
					Vector2 vector21 = uV[i + 3];
					uV[i + 3] = new Vector2(vector21.x, single2);
					Vector2 vector22 = uV[i + 2];
					uV[i + 2] = new Vector2(vector22.x, single2);
					Color color = Color.Lerp(colors[i + 3], colors[i], single1);
					colors[i + 3] = color;
					colors[i + 2] = color;
				}
			}
		}

		private void clipRight(dfRenderData destination, int startIndex)
		{
			float vectorOffset = base.VectorOffset.x;
			Vector2 maxSize = base.MaxSize;
			float pixelRatio = vectorOffset + maxSize.x * base.PixelRatio;
			dfList<Vector3> vertices = destination.Vertices;
			dfList<Vector2> uV = destination.UV;
			for (int i = startIndex; i < vertices.Count; i = i + 4)
			{
				Vector3 item = vertices[i];
				Vector3 vector3 = vertices[i + 1];
				Vector3 item1 = vertices[i + 2];
				Vector3 vector31 = vertices[i + 3];
				float single = vector3.x - item.x;
				if (vector3.x > pixelRatio)
				{
					float single1 = 1f - (pixelRatio - vector3.x + single) / single;
					item = new Vector3(Mathf.Min(item.x, pixelRatio), item.y, item.z);
					vertices[i] = item;
					vector3 = new Vector3(Mathf.Min(vector3.x, pixelRatio), vector3.y, vector3.z);
					vertices[i + 1] = vector3;
					item1 = new Vector3(Mathf.Min(item1.x, pixelRatio), item1.y, item1.z);
					vertices[i + 2] = item1;
					vector31 = new Vector3(Mathf.Min(vector31.x, pixelRatio), vector31.y, vector31.z);
					vertices[i + 3] = vector31;
					float item2 = uV[i + 1].x;
					Vector2 vector2 = uV[i];
					float single2 = Mathf.Lerp(item2, vector2.x, single1);
					Vector2 vector21 = uV[i + 1];
					uV[i + 1] = new Vector2(single2, vector21.y);
					Vector2 vector22 = uV[i + 2];
					uV[i + 2] = new Vector2(single2, vector22.y);
					single = vector3.x - item.x;
				}
			}
		}

		private dfFont.LineRenderInfo fitSingleLine()
		{
			return dfFont.LineRenderInfo.Obtain(0, 0);
		}

		public override float[] GetCharacterWidths(string text)
		{
			float single = 0f;
			return this.GetCharacterWidths(text, 0, text.Length - 1, out single);
		}

		public float[] GetCharacterWidths(string text, int startIndex, int endIndex, out float totalWidth)
		{
			totalWidth = 0f;
			dfFont font = (dfFont)base.Font;
			float[] singleArray = new float[text.Length];
			float textScale = base.TextScale * base.PixelRatio;
			float characterSpacing = (float)base.CharacterSpacing * textScale;
			for (int i = startIndex; i <= endIndex; i++)
			{
				dfFont.GlyphDefinition glyph = font.GetGlyph(text[i]);
				if (glyph != null)
				{
					if (i > 0)
					{
						singleArray[i - 1] = singleArray[i - 1] + characterSpacing;
						totalWidth = totalWidth + characterSpacing;
					}
					float single = (float)glyph.xadvance * textScale;
					singleArray[i] = single;
					totalWidth = totalWidth + single;
				}
			}
			return singleArray;
		}

		private float getTabStop(float position)
		{
			float pixelRatio = base.PixelRatio * base.TextScale;
			if (base.TabStops != null && base.TabStops.Count > 0)
			{
				for (int i = 0; i < base.TabStops.Count; i++)
				{
					if ((float)base.TabStops[i] * pixelRatio > position)
					{
						return (float)base.TabStops[i] * pixelRatio;
					}
				}
			}
			if (base.TabSize > 0)
			{
				return position + (float)base.TabSize * pixelRatio;
			}
			return position + (float)(base.Font.FontSize * 4) * pixelRatio;
		}

		public override Vector2 MeasureString(string text)
		{
			this.tokenize(text);
			dfList<dfFont.LineRenderInfo> lineRenderInfos = this.calculateLinebreaks();
			int num = 0;
			int item = 0;
			for (int i = 0; i < lineRenderInfos.Count; i++)
			{
				num = Mathf.Max((int)lineRenderInfos[i].lineWidth, num);
				item = item + (int)lineRenderInfos[i].lineHeight;
			}
			return new Vector2((float)num, (float)item) * base.TextScale;
		}

		private Color multiplyColors(Color lhs, Color rhs)
		{
			return new Color(lhs.r * rhs.r, lhs.g * rhs.g, lhs.b * rhs.b, lhs.a * rhs.a);
		}

		public static dfFontRendererBase Obtain(dfFont font)
		{
			dfFont.BitmappedFontRenderer bitmappedFontRenderer = (dfFont.BitmappedFontRenderer.objectPool.Count <= 0 ? new dfFont.BitmappedFontRenderer() : dfFont.BitmappedFontRenderer.objectPool.Dequeue());
			bitmappedFontRenderer.Reset();
			bitmappedFontRenderer.Font = font;
			return bitmappedFontRenderer;
		}

		private Color32 parseColor(dfMarkupToken token)
		{
			Color color = Color.white;
			if (token.AttributeCount == 1)
			{
				string value = token.GetAttribute(0).Value.Value;
				if (value.Length != 7 || value[0] != '#')
				{
					color = dfMarkupStyle.ParseColor(value, base.DefaultColor);
				}
				else
				{
					uint num = 0;
					uint.TryParse(value.Substring(1), NumberStyles.HexNumber, null, out num);
					color = this.UIntToColor(num | -16777216);
				}
			}
			return this.applyOpacity(color);
		}

		public override void Release()
		{
			this.Reset();
			this.tokens = null;
			if (this.lines != null)
			{
				this.lines.Release();
				this.lines = null;
			}
			dfFont.LineRenderInfo.ResetPool();
			base.BottomColor = null;
			dfFont.BitmappedFontRenderer.objectPool.Enqueue(this);
		}

		public override void Render(string text, dfRenderData destination)
		{
			dfFont.BitmappedFontRenderer.textColors.Clear();
			dfFont.BitmappedFontRenderer.textColors.Push(Color.white);
			this.tokenize(text);
			dfList<dfFont.LineRenderInfo> lineRenderInfos = this.calculateLinebreaks();
			int num = 0;
			int num1 = 0;
			Vector3 vectorOffset = base.VectorOffset;
			float textScale = base.TextScale * base.PixelRatio;
			for (int i = 0; i < lineRenderInfos.Count; i++)
			{
				dfFont.LineRenderInfo item = lineRenderInfos[i];
				int count = destination.Vertices.Count;
				this.renderLine(lineRenderInfos[i], dfFont.BitmappedFontRenderer.textColors, vectorOffset, destination);
				vectorOffset.y = vectorOffset.y - (float)base.Font.LineHeight * textScale;
				num = Mathf.Max((int)item.lineWidth, num);
				num1 = num1 + (int)item.lineHeight;
				if (item.lineWidth * base.TextScale > base.MaxSize.x)
				{
					this.clipRight(destination, count);
				}
				if ((float)num1 * base.TextScale > base.MaxSize.y)
				{
					this.clipBottom(destination, count);
				}
			}
			Vector2 maxSize = base.MaxSize;
			float single = Mathf.Min(maxSize.x, (float)num);
			Vector2 vector2 = base.MaxSize;
			base.RenderedSize = new Vector2(single, Mathf.Min(vector2.y, (float)num1)) * base.TextScale;
		}

		private void renderLine(dfFont.LineRenderInfo line, Stack<Color32> colors, Vector3 position, dfRenderData destination)
		{
			float textScale = base.TextScale * base.PixelRatio;
			position.x = position.x + (float)this.calculateLineAlignment(line) * textScale;
			for (int i = line.startOffset; i <= line.endOffset; i++)
			{
				dfMarkupToken item = this.tokens[i];
				dfMarkupTokenType tokenType = item.TokenType;
				if (tokenType == dfMarkupTokenType.Text)
				{
					this.renderText(item, colors.Peek(), position, destination);
				}
				else if (tokenType != dfMarkupTokenType.StartTag)
				{
					if (tokenType == dfMarkupTokenType.EndTag && item.Matches("color") && colors.Count > 1)
					{
						colors.Pop();
					}
				}
				else if (item.Matches("sprite"))
				{
					this.renderSprite(item, colors.Peek(), position, destination);
				}
				else if (item.Matches("color"))
				{
					colors.Push(this.parseColor(item));
				}
				position.x = position.x + (float)item.Width * textScale;
			}
		}

		private void renderSprite(dfMarkupToken token, Color32 color, Vector3 position, dfRenderData destination)
		{
			try
			{
				dfList<Vector3> vertices = destination.Vertices;
				dfList<int> triangles = destination.Triangles;
				dfList<Color32> colors = destination.Colors;
				dfList<Vector2> uV = destination.UV;
				dfFont font = (dfFont)base.Font;
				string value = token.GetAttribute(0).Value.Value;
				dfAtlas.ItemInfo item = font.Atlas[value];
				if (item != null)
				{
					float height = (float)token.Height * base.TextScale * base.PixelRatio;
					float width = (float)token.Width * base.TextScale * base.PixelRatio;
					float single = position.x;
					float single1 = position.y;
					int count = vertices.Count;
					vertices.Add(new Vector3(single, single1));
					vertices.Add(new Vector3(single + width, single1));
					vertices.Add(new Vector3(single + width, single1 - height));
					vertices.Add(new Vector3(single, single1 - height));
					triangles.Add(count);
					triangles.Add(count + 1);
					triangles.Add(count + 3);
					triangles.Add(count + 3);
					triangles.Add(count + 1);
					triangles.Add(count + 2);
					Color32 color32 = (!base.ColorizeSymbols ? this.applyOpacity(base.DefaultColor) : this.applyOpacity(color));
					colors.Add(color32);
					colors.Add(color32);
					colors.Add(color32);
					colors.Add(color32);
					Rect rect = item.region;
					uV.Add(new Vector2(rect.x, rect.yMax));
					uV.Add(new Vector2(rect.xMax, rect.yMax));
					uV.Add(new Vector2(rect.xMax, rect.y));
					uV.Add(new Vector2(rect.x, rect.y));
				}
			}
			finally
			{
			}
		}

		private void renderText(dfMarkupToken token, Color32 color, Vector3 position, dfRenderData destination)
		{
			try
			{
				dfList<Vector3> vertices = destination.Vertices;
				dfList<int> triangles = destination.Triangles;
				dfList<Color32> colors = destination.Colors;
				dfList<Vector2> uV = destination.UV;
				dfFont font = (dfFont)base.Font;
				dfAtlas.ItemInfo item = font.Atlas[font.sprite];
				UnityEngine.Texture texture = font.Texture;
				float single = 1f / (float)texture.width;
				float single1 = 1f / (float)texture.height;
				float single2 = single * 0.125f;
				float single3 = single1 * 0.125f;
				float textScale = base.TextScale * base.PixelRatio;
				char chr = '\0';
				char item1 = '\0';
				Color32 color32 = this.applyOpacity(this.multiplyColors(color, base.DefaultColor));
				Color32 color321 = color32;
				if (base.BottomColor.HasValue)
				{
					Color color1 = color;
					Color32? bottomColor = base.BottomColor;
					color321 = this.applyOpacity(this.multiplyColors(color1, bottomColor.Value));
				}
				int num = 0;
				while (num < token.Length)
				{
					item1 = token[num];
					if (item1 != 0)
					{
						dfFont.GlyphDefinition glyph = font.GetGlyph(item1);
						if (glyph != null)
						{
							int kerning = font.GetKerning(chr, item1);
							float single4 = position.x + (float)(glyph.xoffset + kerning) * textScale;
							float single5 = position.y - (float)glyph.yoffset * textScale;
							float single6 = (float)glyph.width * textScale;
							float single7 = (float)glyph.height * textScale;
							float single8 = single4 + single6;
							float single9 = single5 - single7;
							Vector3 vector3 = new Vector3(single4, single5);
							Vector3 vector31 = new Vector3(single8, single5);
							Vector3 vector32 = new Vector3(single8, single9);
							Vector3 vector33 = new Vector3(single4, single9);
							float single10 = item.region.x + (float)glyph.x * single - single2;
							float single11 = item.region.yMax - (float)glyph.y * single1 - single3;
							float single12 = single10 + (float)glyph.width * single - single2;
							float single13 = single11 - (float)glyph.height * single1 + single3;
							if (base.Shadow)
							{
								dfFont.BitmappedFontRenderer.addTriangleIndices(vertices, triangles);
								Vector3 shadowOffset = base.ShadowOffset * textScale;
								vertices.Add(vector3 + shadowOffset);
								vertices.Add(vector31 + shadowOffset);
								vertices.Add(vector32 + shadowOffset);
								vertices.Add(vector33 + shadowOffset);
								Color32 color322 = this.applyOpacity(base.ShadowColor);
								colors.Add(color322);
								colors.Add(color322);
								colors.Add(color322);
								colors.Add(color322);
								uV.Add(new Vector2(single10, single11));
								uV.Add(new Vector2(single12, single11));
								uV.Add(new Vector2(single12, single13));
								uV.Add(new Vector2(single10, single13));
							}
							if (base.Outline)
							{
								for (int i = 0; i < (int)dfFont.BitmappedFontRenderer.OUTLINE_OFFSETS.Length; i++)
								{
									dfFont.BitmappedFontRenderer.addTriangleIndices(vertices, triangles);
									Vector3 oUTLINEOFFSETS = (dfFont.BitmappedFontRenderer.OUTLINE_OFFSETS[i] * (float)base.OutlineSize) * textScale;
									vertices.Add(vector3 + oUTLINEOFFSETS);
									vertices.Add(vector31 + oUTLINEOFFSETS);
									vertices.Add(vector32 + oUTLINEOFFSETS);
									vertices.Add(vector33 + oUTLINEOFFSETS);
									Color32 color323 = this.applyOpacity(base.OutlineColor);
									colors.Add(color323);
									colors.Add(color323);
									colors.Add(color323);
									colors.Add(color323);
									uV.Add(new Vector2(single10, single11));
									uV.Add(new Vector2(single12, single11));
									uV.Add(new Vector2(single12, single13));
									uV.Add(new Vector2(single10, single13));
								}
							}
							dfFont.BitmappedFontRenderer.addTriangleIndices(vertices, triangles);
							vertices.Add(vector3);
							vertices.Add(vector31);
							vertices.Add(vector32);
							vertices.Add(vector33);
							colors.Add(color32);
							colors.Add(color32);
							colors.Add(color321);
							colors.Add(color321);
							uV.Add(new Vector2(single10, single11));
							uV.Add(new Vector2(single12, single11));
							uV.Add(new Vector2(single12, single13));
							uV.Add(new Vector2(single10, single13));
							position.x = position.x + (float)(glyph.xadvance + kerning + base.CharacterSpacing) * textScale;
						}
					}
					num++;
					chr = item1;
				}
			}
			finally
			{
			}
		}

		private List<dfMarkupToken> tokenize(string text)
		{
			List<dfMarkupToken> dfMarkupTokens;
			try
			{
				if (this.tokens == null || this.tokens.Count <= 0 || !(this.tokens[0].Source == text))
				{
					if (!base.ProcessMarkup)
					{
						this.tokens = dfPlainTextTokenizer.Tokenize(text);
					}
					else
					{
						this.tokens = dfMarkupTokenizer.Tokenize(text);
					}
					for (int i = 0; i < this.tokens.Count; i++)
					{
						this.calculateTokenRenderSize(this.tokens[i]);
					}
					dfMarkupTokens = this.tokens;
				}
				else
				{
					dfMarkupTokens = this.tokens;
				}
			}
			finally
			{
			}
			return dfMarkupTokens;
		}

		private Color32 UIntToColor(uint color)
		{
			byte num = (byte)(color >> 24);
			byte num1 = (byte)(color >> 16);
			byte num2 = (byte)(color >> 8);
			return new Color32(num1, num2, (byte)color, num);
		}
	}

	[Serializable]
	public class GlyphDefinition : IComparable<dfFont.GlyphDefinition>
	{
		[SerializeField]
		public int id;

		[SerializeField]
		public int x;

		[SerializeField]
		public int y;

		[SerializeField]
		public int width;

		[SerializeField]
		public int height;

		[SerializeField]
		public int xoffset;

		[SerializeField]
		public int yoffset;

		[SerializeField]
		public int xadvance;

		[SerializeField]
		public bool rotated;

		public GlyphDefinition()
		{
		}

		public int CompareTo(dfFont.GlyphDefinition other)
		{
			return this.id.CompareTo(other.id);
		}
	}

	[Serializable]
	public class GlyphKerning : IComparable<dfFont.GlyphKerning>
	{
		public int first;

		public int second;

		public int amount;

		public GlyphKerning()
		{
		}

		public int CompareTo(dfFont.GlyphKerning other)
		{
			if (this.first == other.first)
			{
				return this.second.CompareTo(other.second);
			}
			return this.first.CompareTo(other.first);
		}
	}

	private class GlyphKerningList
	{
		private Dictionary<int, int> list;

		public GlyphKerningList()
		{
		}

		public void Add(dfFont.GlyphKerning kerning)
		{
			this.list[kerning.second] = kerning.amount;
		}

		public int GetKerning(int firstCharacter, int secondCharacter)
		{
			int num = 0;
			this.list.TryGetValue(secondCharacter, out num);
			return num;
		}
	}

	private class LineRenderInfo
	{
		public int startOffset;

		public int endOffset;

		public float lineWidth;

		public float lineHeight;

		private static dfList<dfFont.LineRenderInfo> pool;

		private static int poolIndex;

		public int length
		{
			get
			{
				return this.endOffset - this.startOffset + 1;
			}
		}

		static LineRenderInfo()
		{
			dfFont.LineRenderInfo.pool = new dfList<dfFont.LineRenderInfo>();
			dfFont.LineRenderInfo.poolIndex = 0;
		}

		private LineRenderInfo()
		{
		}

		public static dfFont.LineRenderInfo Obtain(int start, int end)
		{
			if (dfFont.LineRenderInfo.poolIndex >= dfFont.LineRenderInfo.pool.Count - 1)
			{
				dfFont.LineRenderInfo.pool.Add(new dfFont.LineRenderInfo());
			}
			dfList<dfFont.LineRenderInfo> lineRenderInfos = dfFont.LineRenderInfo.pool;
			int num = dfFont.LineRenderInfo.poolIndex;
			dfFont.LineRenderInfo.poolIndex = num + 1;
			dfFont.LineRenderInfo item = lineRenderInfos[num];
			item.startOffset = start;
			item.endOffset = end;
			item.lineHeight = 0f;
			return item;
		}

		public static void ResetPool()
		{
			dfFont.LineRenderInfo.poolIndex = 0;
		}
	}
}