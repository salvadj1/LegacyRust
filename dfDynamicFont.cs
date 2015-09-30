using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Dynamic Font")]
[ExecuteInEditMode]
[Serializable]
public class dfDynamicFont : dfFontBase
{
	private static List<dfDynamicFont> loadedFonts;

	private static CharacterInfo[] glyphBuffer;

	[SerializeField]
	private Font baseFont;

	[SerializeField]
	private UnityEngine.Material material;

	[SerializeField]
	private int baseFontSize = -1;

	[SerializeField]
	private int baseline = -1;

	[SerializeField]
	private int lineHeight;

	private bool invalidatingDependentControls;

	private bool wasFontAtlasRebuilt;

	public Font BaseFont
	{
		get
		{
			return this.baseFont;
		}
		set
		{
			if (value != this.baseFont)
			{
				this.baseFont = value;
				dfGUIManager.RefreshAll(false);
			}
		}
	}

	public int Baseline
	{
		get
		{
			return this.baseline;
		}
		set
		{
			if (value != this.baseline)
			{
				this.baseline = value;
				dfGUIManager.RefreshAll(false);
			}
		}
	}

	public int Descent
	{
		get
		{
			return this.LineHeight - this.baseline;
		}
	}

	public override int FontSize
	{
		get
		{
			return this.baseFontSize;
		}
		set
		{
			if (value != this.baseFontSize)
			{
				this.baseFontSize = value;
				dfGUIManager.RefreshAll(false);
			}
		}
	}

	public override bool IsValid
	{
		get
		{
			return (!(this.baseFont != null) || !(this.Material != null) ? false : this.Texture != null);
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
			if (value != this.lineHeight)
			{
				this.lineHeight = value;
				dfGUIManager.RefreshAll(false);
			}
		}
	}

	public override UnityEngine.Material Material
	{
		get
		{
			this.material.mainTexture = this.baseFont.material.mainTexture;
			return this.material;
		}
		set
		{
			if (value != this.material)
			{
				this.material = value;
				dfGUIManager.RefreshAll(false);
			}
		}
	}

	public override UnityEngine.Texture Texture
	{
		get
		{
			return this.baseFont.material.mainTexture;
		}
	}

	static dfDynamicFont()
	{
		dfDynamicFont.loadedFonts = new List<dfDynamicFont>();
		dfDynamicFont.glyphBuffer = new CharacterInfo[1024];
	}

	public dfDynamicFont()
	{
	}

	private void ensureGlyphBufferCapacity(int size)
	{
		int length = (int)dfDynamicFont.glyphBuffer.Length;
		if (size < length)
		{
			return;
		}
		while (length < size)
		{
			length = length + 1024;
		}
		dfDynamicFont.glyphBuffer = new CharacterInfo[length];
	}

	public static dfDynamicFont FindByName(string name)
	{
		for (int i = 0; i < dfDynamicFont.loadedFonts.Count; i++)
		{
			if (string.Equals(dfDynamicFont.loadedFonts[i].name, name, StringComparison.InvariantCultureIgnoreCase))
			{
				return dfDynamicFont.loadedFonts[i];
			}
		}
		GameObject gameObject = Resources.Load(name) as GameObject;
		if (gameObject == null)
		{
			return null;
		}
		dfDynamicFont component = gameObject.GetComponent<dfDynamicFont>();
		if (component == null)
		{
			return null;
		}
		dfDynamicFont.loadedFonts.Add(component);
		return component;
	}

	private void getGlyphData(CharacterInfo[] result, string text, int size, FontStyle style)
	{
		if (text.Length > (int)dfDynamicFont.glyphBuffer.Length)
		{
			dfDynamicFont.glyphBuffer = new CharacterInfo[text.Length + 512];
		}
		for (int i = 0; i < text.Length; i++)
		{
			if (!this.baseFont.GetCharacterInfo(text[i], out result[i], size, style))
			{
				CharacterInfo characterInfo = new CharacterInfo()
				{
					index = -1,
					size = size,
					style = style,
					width = (float)size * 0.25f
				};
				result[i] = characterInfo;
			}
		}
	}

	public Vector2 MeasureText(string text, int size, FontStyle style)
	{
		CharacterInfo[] characterInfoArray = this.RequestCharacters(text, size, style);
		float single = (float)size / (float)this.FontSize;
		int num = Mathf.CeilToInt((float)this.Baseline * single);
		Vector2 vector2 = new Vector2(0f, (float)num);
		for (int i = 0; i < text.Length; i++)
		{
			CharacterInfo characterInfo = characterInfoArray[i];
			float single1 = Mathf.Ceil(characterInfo.vert.x + characterInfo.vert.width);
			if (text[i] == ' ')
			{
				single1 = Mathf.Ceil(characterInfo.width * 1.25f);
			}
			else if (text[i] == '\t')
			{
				single1 = single1 + (float)(size * 4);
			}
			vector2.x = vector2.x + single1;
		}
		return vector2;
	}

	public override dfFontRendererBase ObtainRenderer()
	{
		return dfDynamicFont.DynamicFontRenderer.Obtain(this);
	}

	private void onFontAtlasRebuilt()
	{
		this.wasFontAtlasRebuilt = true;
		this.OnFontChanged();
	}

	private void OnFontChanged()
	{
		Func<UnityEngine.Object, bool> func = null;
		Func<dfControl, int> func1 = null;
		try
		{
			if (!this.invalidatingDependentControls)
			{
				dfGUIManager.RenderCallback renderCallback = null;
				renderCallback = (dfGUIManager manager) => {
					dfGUIManager.AfterRender -= renderCallback;
					this.invalidatingDependentControls = true;
					try
					{
						!this.wasFontAtlasRebuilt;
						UnityEngine.Object[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(dfControl));
						if (func == null)
						{
							func = (UnityEngine.Object x) => x is IDFMultiRender;
						}
						IEnumerable<dfControl> dfControls = ((IEnumerable<UnityEngine.Object>)objArray).Where<UnityEngine.Object>(func).Cast<dfControl>();
						if (func1 == null)
						{
							func1 = (dfControl x) => x.RenderOrder;
						}
						List<dfControl> list = dfControls.OrderBy<dfControl, int>(func1).ToList<dfControl>();
						for (int i = 0; i < list.Count; i++)
						{
							list[i].Invalidate();
						}
						if (this.wasFontAtlasRebuilt)
						{
							manager.Render();
						}
					}
					finally
					{
						this.wasFontAtlasRebuilt = false;
						this.invalidatingDependentControls = false;
					}
				};
				dfGUIManager.AfterRender += renderCallback;
			}
		}
		finally
		{
		}
	}

	public CharacterInfo[] RequestCharacters(string text, int size, FontStyle style)
	{
		if (this.baseFont == null)
		{
			throw new NullReferenceException(string.Concat("Base Font not assigned: ", base.name));
		}
		this.ensureGlyphBufferCapacity(size);
		if (!dfDynamicFont.loadedFonts.Contains(this))
		{
			Font font = this.baseFont;
			font.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Combine(font.textureRebuildCallback, new Font.FontTextureRebuildCallback(this.onFontAtlasRebuilt));
			dfDynamicFont.loadedFonts.Add(this);
		}
		this.baseFont.RequestCharactersInTexture(text, size, style);
		this.getGlyphData(dfDynamicFont.glyphBuffer, text, size, style);
		return dfDynamicFont.glyphBuffer;
	}

	public class DynamicFontRenderer : dfFontRendererBase
	{
		private static Queue<dfDynamicFont.DynamicFontRenderer> objectPool;

		private static Vector2[] OUTLINE_OFFSETS;

		private static int[] TRIANGLE_INDICES;

		private static Stack<Color32> textColors;

		private dfList<dfDynamicFont.LineRenderInfo> lines;

		private List<dfMarkupToken> tokens;

		public int LineCount
		{
			get
			{
				return this.lines.Count;
			}
		}

		public dfAtlas SpriteAtlas
		{
			get;
			set;
		}

		public dfRenderData SpriteBuffer
		{
			get;
			set;
		}

		static DynamicFontRenderer()
		{
			dfDynamicFont.DynamicFontRenderer.objectPool = new Queue<dfDynamicFont.DynamicFontRenderer>();
			dfDynamicFont.DynamicFontRenderer.OUTLINE_OFFSETS = new Vector2[] { new Vector2(-1f, -1f), new Vector2(-1f, 1f), new Vector2(1f, -1f), new Vector2(1f, 1f) };
			dfDynamicFont.DynamicFontRenderer.TRIANGLE_INDICES = new int[] { 0, 1, 3, 3, 1, 2 };
			dfDynamicFont.DynamicFontRenderer.textColors = new Stack<Color32>();
		}

		internal DynamicFontRenderer()
		{
		}

		private static void addTriangleIndices(dfList<Vector3> verts, dfList<int> triangles)
		{
			int count = verts.Count;
			int[] tRIANGLEINDICES = dfDynamicFont.DynamicFontRenderer.TRIANGLE_INDICES;
			for (int i = 0; i < (int)tRIANGLEINDICES.Length; i++)
			{
				triangles.Add(count + tRIANGLEINDICES[i]);
			}
		}

		private static void addUVCoords(dfList<Vector2> uvs, CharacterInfo glyph)
		{
			Rect rect = glyph.uv;
			float single = rect.x;
			float single1 = rect.y + rect.height;
			float single2 = single + rect.width;
			float single3 = rect.y;
			if (!glyph.flipped)
			{
				uvs.Add(new Vector2(single, single1));
				uvs.Add(new Vector2(single2, single1));
				uvs.Add(new Vector2(single2, single3));
				uvs.Add(new Vector2(single, single3));
			}
			else
			{
				uvs.Add(new Vector2(single2, single3));
				uvs.Add(new Vector2(single2, single1));
				uvs.Add(new Vector2(single, single1));
				uvs.Add(new Vector2(single, single3));
			}
		}

		private Color32 applyOpacity(Color32 color)
		{
			color.a = (byte)(base.Opacity * 255f);
			return color;
		}

		private int calculateLineAlignment(dfDynamicFont.LineRenderInfo line)
		{
			float single = line.lineWidth;
			if (base.TextAlign == TextAlignment.Left || single < 1f)
			{
				return 0;
			}
			float single1 = 0f;
			single1 = (base.TextAlign != TextAlignment.Right ? (base.MaxSize.x - single) * 0.5f : base.MaxSize.x - single);
			return Mathf.CeilToInt(Mathf.Max(0f, single1));
		}

		private dfList<dfDynamicFont.LineRenderInfo> calculateLinebreaks()
		{
			dfList<dfDynamicFont.LineRenderInfo> lineRenderInfos;
			bool flag;
			try
			{
				if (this.lines == null)
				{
					this.lines = dfList<dfDynamicFont.LineRenderInfo>.Obtain();
					dfDynamicFont font = (dfDynamicFont)base.Font;
					int num = 0;
					int num1 = 0;
					int num2 = 0;
					int num3 = 0;
					float baseline = (float)font.Baseline * base.TextScale;
					while (num2 < this.tokens.Count)
					{
						Vector2 maxSize = base.MaxSize;
						if ((float)this.lines.Count * baseline > maxSize.y + baseline)
						{
							break;
						}
						dfMarkupToken item = this.tokens[num2];
						dfMarkupTokenType tokenType = item.TokenType;
						if (tokenType != dfMarkupTokenType.Newline)
						{
							int num4 = Mathf.CeilToInt((float)item.Width);
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
								this.lines.Add(dfDynamicFont.LineRenderInfo.Obtain(num1, num - 1));
								int num5 = num2 + 1;
								num2 = num5;
								num = num5;
								num1 = num5;
								num3 = 0;
							}
							else
							{
								this.lines.Add(dfDynamicFont.LineRenderInfo.Obtain(num1, num - 1));
								int num6 = num + 1;
								num = num6;
								num2 = num6;
								num1 = num6;
								num3 = 0;
							}
						}
						else
						{
							this.lines.Add(dfDynamicFont.LineRenderInfo.Obtain(num1, num2));
							int num7 = num2 + 1;
							num2 = num7;
							num = num7;
							num1 = num7;
							num3 = 0;
						}
					}
					if (num1 < this.tokens.Count)
					{
						this.lines.Add(dfDynamicFont.LineRenderInfo.Obtain(num1, this.tokens.Count - 1));
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

		private void calculateLineSize(dfDynamicFont.LineRenderInfo line)
		{
			dfDynamicFont font = (dfDynamicFont)base.Font;
			line.lineHeight = (float)font.Baseline * base.TextScale;
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
				int num = 0;
				char item = '\0';
				bool flag = (token.TokenType == dfMarkupTokenType.Whitespace ? true : token.TokenType == dfMarkupTokenType.Text);
				dfDynamicFont font = (dfDynamicFont)base.Font;
				if (flag)
				{
					int num1 = Mathf.CeilToInt((float)font.FontSize * base.TextScale);
					CharacterInfo[] characterInfoArray = font.RequestCharacters(token.Value, num1, FontStyle.Normal);
					for (int i = 0; i < token.Length; i++)
					{
						item = token[i];
						if (item != '\t')
						{
							CharacterInfo characterInfo = characterInfoArray[i];
							num = num + (item == ' ' ? Mathf.CeilToInt(characterInfo.width) : Mathf.CeilToInt(characterInfo.vert.x + characterInfo.vert.width));
							if (i > 0)
							{
								num = num + Mathf.CeilToInt((float)base.CharacterSpacing * base.TextScale);
							}
						}
						else
						{
							num = num + base.TabSize;
						}
					}
					token.Height = base.Font.LineHeight;
					token.Width = num;
				}
				else if (token.TokenType == dfMarkupTokenType.StartTag && token.Matches("sprite") && this.SpriteAtlas != null && token.AttributeCount == 1)
				{
					Texture2D texture = this.SpriteAtlas.Texture;
					float baseline = (float)font.Baseline * base.TextScale;
					string value = token.GetAttribute(0).Value.Value;
					dfAtlas.ItemInfo itemInfo = this.SpriteAtlas[value];
					if (itemInfo != null)
					{
						float single = itemInfo.region.width * (float)texture.width / (itemInfo.region.height * (float)texture.height);
						num = Mathf.CeilToInt(baseline * single);
					}
					token.Height = Mathf.CeilToInt(baseline);
					token.Width = num;
				}
			}
			finally
			{
			}
		}

		private void clipBottom(dfRenderData destination, int startIndex)
		{
			if (destination == null)
			{
				return;
			}
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
					uV[i + 3] = Vector2.Lerp(uV[i + 3], uV[i], single1);
					uV[i + 2] = Vector2.Lerp(uV[i + 2], uV[i + 1], single1);
					Color color = Color.Lerp(colors[i + 3], colors[i], single1);
					colors[i + 3] = color;
					colors[i + 2] = color;
				}
			}
		}

		private void clipRight(dfRenderData destination, int startIndex)
		{
			if (destination == null)
			{
				return;
			}
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

		public override float[] GetCharacterWidths(string text)
		{
			float single = 0f;
			return this.GetCharacterWidths(text, 0, text.Length - 1, out single);
		}

		public float[] GetCharacterWidths(string text, int startIndex, int endIndex, out float totalWidth)
		{
			totalWidth = 0f;
			dfDynamicFont font = (dfDynamicFont)base.Font;
			int num = Mathf.CeilToInt((float)font.FontSize * base.TextScale);
			CharacterInfo[] characterInfoArray = font.RequestCharacters(text, num, FontStyle.Normal);
			float[] pixelRatio = new float[text.Length];
			float single = 0f;
			float tabSize = 0f;
			int num1 = startIndex;
			while (num1 <= endIndex)
			{
				CharacterInfo characterInfo = characterInfoArray[num1];
				if (text[num1] != '\t')
				{
					tabSize = (text[num1] != ' ' ? tabSize + (characterInfo.vert.x + characterInfo.vert.width) : tabSize + characterInfo.width);
				}
				else
				{
					tabSize = tabSize + (float)base.TabSize;
				}
				pixelRatio[num1] = (tabSize - single) * base.PixelRatio;
				num1++;
				single = tabSize;
			}
			return pixelRatio;
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
			dfList<dfDynamicFont.LineRenderInfo> lineRenderInfos = this.calculateLinebreaks();
			float single = 0f;
			float item = 0f;
			for (int i = 0; i < lineRenderInfos.Count; i++)
			{
				single = Mathf.Max(lineRenderInfos[i].lineWidth, single);
				item = item + lineRenderInfos[i].lineHeight;
			}
			return new Vector2(single, item);
		}

		private Color multiplyColors(Color lhs, Color rhs)
		{
			return new Color(lhs.r * rhs.r, lhs.g * rhs.g, lhs.b * rhs.b, lhs.a * rhs.a);
		}

		public static dfFontRendererBase Obtain(dfDynamicFont font)
		{
			dfDynamicFont.DynamicFontRenderer dynamicFontRenderer = (dfDynamicFont.DynamicFontRenderer.objectPool.Count <= 0 ? new dfDynamicFont.DynamicFontRenderer() : dfDynamicFont.DynamicFontRenderer.objectPool.Dequeue());
			dynamicFontRenderer.Reset();
			dynamicFontRenderer.Font = font;
			return dynamicFontRenderer;
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
			dfDynamicFont.LineRenderInfo.ResetPool();
			base.BottomColor = null;
			dfDynamicFont.DynamicFontRenderer.objectPool.Enqueue(this);
		}

		public override void Render(string text, dfRenderData destination)
		{
			dfDynamicFont.DynamicFontRenderer.textColors.Clear();
			dfDynamicFont.DynamicFontRenderer.textColors.Push(Color.white);
			this.tokenize(text);
			dfList<dfDynamicFont.LineRenderInfo> lineRenderInfos = this.calculateLinebreaks();
			int num = 0;
			int num1 = 0;
			Vector3 vector3 = (base.VectorOffset / base.PixelRatio).CeilToInt();
			for (int i = 0; i < lineRenderInfos.Count; i++)
			{
				dfDynamicFont.LineRenderInfo item = lineRenderInfos[i];
				int count = destination.Vertices.Count;
				int num2 = (this.SpriteBuffer == null ? 0 : this.SpriteBuffer.Vertices.Count);
				this.renderLine(lineRenderInfos[i], dfDynamicFont.DynamicFontRenderer.textColors, vector3, destination);
				vector3.y = vector3.y - item.lineHeight;
				num = Mathf.Max((int)item.lineWidth, num);
				num1 = num1 + Mathf.CeilToInt(item.lineHeight);
				if (item.lineWidth > base.MaxSize.x)
				{
					this.clipRight(destination, count);
					this.clipRight(this.SpriteBuffer, num2);
				}
				this.clipBottom(destination, count);
				this.clipBottom(this.SpriteBuffer, num2);
			}
			Vector2 maxSize = base.MaxSize;
			float single = Mathf.Min(maxSize.x, (float)num);
			Vector2 vector2 = base.MaxSize;
			base.RenderedSize = new Vector2(single, Mathf.Min(vector2.y, (float)num1)) * base.TextScale;
		}

		private void renderLine(dfDynamicFont.LineRenderInfo line, Stack<Color32> colors, Vector3 position, dfRenderData destination)
		{
			position.x = position.x + (float)this.calculateLineAlignment(line);
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
				else if (item.Matches("sprite") && this.SpriteAtlas != null && this.SpriteBuffer != null)
				{
					this.renderSprite(item, colors.Peek(), position, this.SpriteBuffer);
				}
				else if (item.Matches("color"))
				{
					colors.Push(this.parseColor(item));
				}
				position.x = position.x + (float)item.Width;
			}
		}

		private void renderSprite(dfMarkupToken token, Color32 color, Vector3 position, dfRenderData destination)
		{
			try
			{
				string value = token.GetAttribute(0).Value.Value;
				dfAtlas.ItemInfo item = this.SpriteAtlas[value];
				if (item != null)
				{
					dfSprite.RenderOptions renderOption = new dfSprite.RenderOptions();
					dfSprite.RenderOptions spriteAtlas = renderOption;
					spriteAtlas.atlas = this.SpriteAtlas;
					spriteAtlas.color = color;
					spriteAtlas.fillAmount = 1f;
					spriteAtlas.offset = position;
					spriteAtlas.pixelsToUnits = base.PixelRatio;
					spriteAtlas.size = new Vector2((float)token.Width, (float)token.Height);
					spriteAtlas.spriteInfo = item;
					renderOption = spriteAtlas;
					dfSprite.renderSprite(this.SpriteBuffer, renderOption);
				}
			}
			finally
			{
			}
		}

		private void renderText(dfMarkupToken token, Color32 color, Vector3 position, dfRenderData renderData)
		{
			try
			{
				dfDynamicFont font = (dfDynamicFont)base.Font;
				int num = Mathf.CeilToInt((float)font.FontSize * base.TextScale);
				FontStyle fontStyle = FontStyle.Normal;
				int descent = font.Descent;
				dfList<Vector3> vertices = renderData.Vertices;
				dfList<int> triangles = renderData.Triangles;
				dfList<Vector2> uV = renderData.UV;
				dfList<Color32> colors = renderData.Colors;
				string value = token.Value;
				float characterSpacing = position.x;
				float single = position.y;
				CharacterInfo[] characterInfoArray = font.RequestCharacters(value, num, fontStyle);
				renderData.Material = font.Material;
				Color32 color32 = this.applyOpacity(this.multiplyColors(color, base.DefaultColor));
				Color32 color321 = color32;
				if (base.BottomColor.HasValue)
				{
					Color color1 = color;
					Color32? bottomColor = base.BottomColor;
					color321 = this.applyOpacity(this.multiplyColors(color1, bottomColor.Value));
				}
				for (int i = 0; i < value.Length; i++)
				{
					if (i > 0)
					{
						characterSpacing = characterSpacing + (float)base.CharacterSpacing * base.TextScale;
					}
					CharacterInfo characterInfo = characterInfoArray[i];
					float fontSize = (float)font.FontSize + characterInfo.vert.y - (float)num + (float)descent;
					float single1 = characterSpacing + characterInfo.vert.x;
					float single2 = single + fontSize;
					float single3 = single1 + characterInfo.vert.width;
					float single4 = single2 + characterInfo.vert.height;
					Vector3 vector3 = new Vector3(single1, single2) * base.PixelRatio;
					Vector3 vector31 = new Vector3(single3, single2) * base.PixelRatio;
					Vector3 vector32 = new Vector3(single3, single4) * base.PixelRatio;
					Vector3 vector33 = new Vector3(single1, single4) * base.PixelRatio;
					if (base.Shadow)
					{
						dfDynamicFont.DynamicFontRenderer.addTriangleIndices(vertices, triangles);
						Vector3 shadowOffset = base.ShadowOffset * base.PixelRatio;
						vertices.Add(vector3 + shadowOffset);
						vertices.Add(vector31 + shadowOffset);
						vertices.Add(vector32 + shadowOffset);
						vertices.Add(vector33 + shadowOffset);
						Color32 color322 = this.applyOpacity(base.ShadowColor);
						colors.Add(color322);
						colors.Add(color322);
						colors.Add(color322);
						colors.Add(color322);
						dfDynamicFont.DynamicFontRenderer.addUVCoords(uV, characterInfo);
					}
					if (base.Outline)
					{
						for (int j = 0; j < (int)dfDynamicFont.DynamicFontRenderer.OUTLINE_OFFSETS.Length; j++)
						{
							dfDynamicFont.DynamicFontRenderer.addTriangleIndices(vertices, triangles);
							Vector3 oUTLINEOFFSETS = (dfDynamicFont.DynamicFontRenderer.OUTLINE_OFFSETS[j] * (float)base.OutlineSize) * base.PixelRatio;
							vertices.Add(vector3 + oUTLINEOFFSETS);
							vertices.Add(vector31 + oUTLINEOFFSETS);
							vertices.Add(vector32 + oUTLINEOFFSETS);
							vertices.Add(vector33 + oUTLINEOFFSETS);
							Color32 color323 = this.applyOpacity(base.OutlineColor);
							colors.Add(color323);
							colors.Add(color323);
							colors.Add(color323);
							colors.Add(color323);
							dfDynamicFont.DynamicFontRenderer.addUVCoords(uV, characterInfo);
						}
					}
					dfDynamicFont.DynamicFontRenderer.addTriangleIndices(vertices, triangles);
					vertices.Add(vector3);
					vertices.Add(vector31);
					vertices.Add(vector32);
					vertices.Add(vector33);
					colors.Add(color32);
					colors.Add(color32);
					colors.Add(color321);
					colors.Add(color321);
					dfDynamicFont.DynamicFontRenderer.addUVCoords(uV, characterInfo);
					characterSpacing = characterSpacing + (float)Mathf.CeilToInt(characterInfo.vert.x + characterInfo.vert.width);
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

	private class LineRenderInfo
	{
		public int startOffset;

		public int endOffset;

		public float lineWidth;

		public float lineHeight;

		private static dfList<dfDynamicFont.LineRenderInfo> pool;

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
			dfDynamicFont.LineRenderInfo.pool = new dfList<dfDynamicFont.LineRenderInfo>();
			dfDynamicFont.LineRenderInfo.poolIndex = 0;
		}

		private LineRenderInfo()
		{
		}

		public static dfDynamicFont.LineRenderInfo Obtain(int start, int end)
		{
			if (dfDynamicFont.LineRenderInfo.poolIndex >= dfDynamicFont.LineRenderInfo.pool.Count - 1)
			{
				dfDynamicFont.LineRenderInfo.pool.Add(new dfDynamicFont.LineRenderInfo());
			}
			dfList<dfDynamicFont.LineRenderInfo> lineRenderInfos = dfDynamicFont.LineRenderInfo.pool;
			int num = dfDynamicFont.LineRenderInfo.poolIndex;
			dfDynamicFont.LineRenderInfo.poolIndex = num + 1;
			dfDynamicFont.LineRenderInfo item = lineRenderInfos[num];
			item.startOffset = start;
			item.endOffset = end;
			item.lineHeight = 0f;
			return item;
		}

		public static void ResetPool()
		{
			dfDynamicFont.LineRenderInfo.poolIndex = 0;
		}
	}
}