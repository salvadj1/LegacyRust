using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Label")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfLabel : dfControl, IDFMultiRender
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected dfFontBase font;

	[SerializeField]
	protected string backgroundSprite;

	[SerializeField]
	protected Color32 backgroundColor = UnityEngine.Color.white;

	[SerializeField]
	protected bool autoSize;

	[SerializeField]
	protected bool autoHeight;

	[SerializeField]
	protected bool wordWrap;

	[SerializeField]
	protected string text = "Label";

	[SerializeField]
	protected Color32 bottomColor = new Color32(255, 255, 255, 255);

	[SerializeField]
	protected UnityEngine.TextAlignment align;

	[SerializeField]
	protected dfVerticalAlignment vertAlign;

	[SerializeField]
	protected float textScale = 1f;

	[SerializeField]
	protected dfTextScaleMode textScaleMode;

	[SerializeField]
	protected int charSpacing;

	[SerializeField]
	protected bool colorizeSymbols;

	[SerializeField]
	protected bool processMarkup;

	[SerializeField]
	protected bool outline;

	[SerializeField]
	protected int outlineWidth = 1;

	[SerializeField]
	protected bool enableGradient;

	[SerializeField]
	protected Color32 outlineColor = UnityEngine.Color.black;

	[SerializeField]
	protected bool shadow;

	[SerializeField]
	protected Color32 shadowColor = UnityEngine.Color.black;

	[SerializeField]
	protected Vector2 shadowOffset = new Vector2(1f, -1f);

	[SerializeField]
	protected RectOffset padding = new RectOffset();

	[SerializeField]
	protected int tabSize = 48;

	[SerializeField]
	protected List<int> tabStops = new List<int>();

	private Vector2 startSize = Vector2.zero;

	private dfRenderData textRenderData;

	private dfList<dfRenderData> buffers = dfList<dfRenderData>.Obtain();

	private PropertyChangedEventHandler<string> TextChanged;

	public dfAtlas Atlas
	{
		get
		{
			if (this.atlas == null)
			{
				dfGUIManager manager = base.GetManager();
				if (manager != null)
				{
					dfAtlas defaultAtlas = manager.DefaultAtlas;
					dfAtlas dfAtla = defaultAtlas;
					this.atlas = defaultAtlas;
					return dfAtla;
				}
			}
			return this.atlas;
		}
		set
		{
			if (!dfAtlas.Equals(value, this.atlas))
			{
				this.atlas = value;
				this.Invalidate();
			}
		}
	}

	public bool AutoHeight
	{
		get
		{
			return (!this.autoHeight ? false : !this.autoSize);
		}
		set
		{
			if (value != this.autoHeight)
			{
				if (value)
				{
					this.autoSize = false;
				}
				this.autoHeight = value;
				this.Invalidate();
			}
		}
	}

	public bool AutoSize
	{
		get
		{
			return this.autoSize;
		}
		set
		{
			if (value != this.autoSize)
			{
				if (value)
				{
					this.autoHeight = false;
				}
				this.autoSize = value;
				this.Invalidate();
			}
		}
	}

	public Color32 BackgroundColor
	{
		get
		{
			return this.backgroundColor;
		}
		set
		{
			if (!object.Equals(value, this.backgroundColor))
			{
				this.backgroundColor = value;
				this.Invalidate();
			}
		}
	}

	public string BackgroundSprite
	{
		get
		{
			return this.backgroundSprite;
		}
		set
		{
			if (value != this.backgroundSprite)
			{
				this.backgroundSprite = value;
				this.Invalidate();
			}
		}
	}

	public Color32 BottomColor
	{
		get
		{
			return this.bottomColor;
		}
		set
		{
			if (!this.bottomColor.Equals(value))
			{
				this.bottomColor = value;
				this.OnColorChanged();
			}
		}
	}

	public int CharacterSpacing
	{
		get
		{
			return this.charSpacing;
		}
		set
		{
			value = Mathf.Max(0, value);
			if (value != this.charSpacing)
			{
				this.charSpacing = value;
				this.Invalidate();
			}
		}
	}

	public bool ColorizeSymbols
	{
		get
		{
			return this.colorizeSymbols;
		}
		set
		{
			if (value != this.colorizeSymbols)
			{
				this.colorizeSymbols = value;
				this.Invalidate();
			}
		}
	}

	public dfFontBase Font
	{
		get
		{
			if (this.font == null)
			{
				dfGUIManager manager = base.GetManager();
				if (manager != null)
				{
					this.font = manager.DefaultFont;
				}
			}
			return this.font;
		}
		set
		{
			if (value != this.font)
			{
				this.font = value;
				this.Invalidate();
			}
		}
	}

	public bool Outline
	{
		get
		{
			return this.outline;
		}
		set
		{
			if (value != this.outline)
			{
				this.outline = value;
				this.Invalidate();
			}
		}
	}

	public Color32 OutlineColor
	{
		get
		{
			return this.outlineColor;
		}
		set
		{
			if (!value.Equals(this.outlineColor))
			{
				this.outlineColor = value;
				this.Invalidate();
			}
		}
	}

	public int OutlineSize
	{
		get
		{
			return this.outlineWidth;
		}
		set
		{
			value = Mathf.Max(0, value);
			if (value != this.outlineWidth)
			{
				this.outlineWidth = value;
				this.Invalidate();
			}
		}
	}

	public RectOffset Padding
	{
		get
		{
			if (this.padding == null)
			{
				this.padding = new RectOffset();
			}
			return this.padding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.padding))
			{
				this.padding = value;
				this.Invalidate();
			}
		}
	}

	public bool ProcessMarkup
	{
		get
		{
			return this.processMarkup;
		}
		set
		{
			if (value != this.processMarkup)
			{
				this.processMarkup = value;
				this.Invalidate();
			}
		}
	}

	public bool Shadow
	{
		get
		{
			return this.shadow;
		}
		set
		{
			if (value != this.shadow)
			{
				this.shadow = value;
				this.Invalidate();
			}
		}
	}

	public Color32 ShadowColor
	{
		get
		{
			return this.shadowColor;
		}
		set
		{
			if (!value.Equals(this.shadowColor))
			{
				this.shadowColor = value;
				this.Invalidate();
			}
		}
	}

	public Vector2 ShadowOffset
	{
		get
		{
			return this.shadowOffset;
		}
		set
		{
			if (value != this.shadowOffset)
			{
				this.shadowOffset = value;
				this.Invalidate();
			}
		}
	}

	public bool ShowGradient
	{
		get
		{
			return this.enableGradient;
		}
		set
		{
			if (value != this.enableGradient)
			{
				this.enableGradient = value;
				this.Invalidate();
			}
		}
	}

	public int TabSize
	{
		get
		{
			return this.tabSize;
		}
		set
		{
			value = Mathf.Max(0, value);
			if (value != this.tabSize)
			{
				this.tabSize = value;
				this.Invalidate();
			}
		}
	}

	public List<int> TabStops
	{
		get
		{
			return this.tabStops;
		}
	}

	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			value = value.Replace("\\t", "\t").Replace("\\n", "\n");
			if (!string.Equals(value, this.text))
			{
				this.text = base.getLocalizedValue(value);
				this.OnTextChanged();
			}
		}
	}

	public UnityEngine.TextAlignment TextAlignment
	{
		get
		{
			return this.align;
		}
		set
		{
			if (value != this.align)
			{
				this.align = value;
				this.Invalidate();
			}
		}
	}

	public float TextScale
	{
		get
		{
			return this.textScale;
		}
		set
		{
			value = Mathf.Max(0.1f, value);
			if (!Mathf.Approximately(this.textScale, value))
			{
				this.textScale = value;
				this.Invalidate();
			}
		}
	}

	public dfTextScaleMode TextScaleMode
	{
		get
		{
			return this.textScaleMode;
		}
		set
		{
			this.textScaleMode = value;
			this.Invalidate();
		}
	}

	public dfVerticalAlignment VerticalAlignment
	{
		get
		{
			return this.vertAlign;
		}
		set
		{
			if (value != this.vertAlign)
			{
				this.vertAlign = value;
				this.Invalidate();
			}
		}
	}

	public bool WordWrap
	{
		get
		{
			return this.wordWrap;
		}
		set
		{
			if (value != this.wordWrap)
			{
				this.wordWrap = value;
				this.Invalidate();
			}
		}
	}

	public dfLabel()
	{
	}

	public override void Awake()
	{
		base.Awake();
		this.startSize = (!Application.isPlaying ? Vector2.zero : base.Size);
	}

	public override Vector2 CalculateMinimumSize()
	{
		if (this.Font == null)
		{
			return base.CalculateMinimumSize();
		}
		float fontSize = (float)this.Font.FontSize * this.TextScale * 0.75f;
		return Vector2.Max(base.CalculateMinimumSize(), new Vector2(fontSize, fontSize));
	}

	private Vector2 getAutoSizeDefault()
	{
		float single = (this.maxSize.x <= 1.401298E-45f ? 2.14748365E+09f : this.maxSize.x);
		return new Vector2(single, (this.maxSize.y <= 1.401298E-45f ? 2.14748365E+09f : this.maxSize.y));
	}

	private float getTextScaleMultiplier()
	{
		if (this.textScaleMode == dfTextScaleMode.None || !Application.isPlaying)
		{
			return 1f;
		}
		if (this.textScaleMode == dfTextScaleMode.ScreenResolution)
		{
			return (float)Screen.height / (float)this.manager.FixedHeight;
		}
		if (this.autoSize)
		{
			return 1f;
		}
		return base.Size.y / this.startSize.y;
	}

	private Vector3 getVertAlignOffset(dfFontRendererBase textRenderer)
	{
		float units = base.PixelsToUnits();
		Vector2 vector2 = textRenderer.MeasureString(this.text) * units;
		Vector3 vectorOffset = textRenderer.VectorOffset;
		float height = (base.Height - (float)this.padding.vertical) * units;
		if (vector2.y >= height)
		{
			return vectorOffset;
		}
		dfVerticalAlignment _dfVerticalAlignment = this.vertAlign;
		if (_dfVerticalAlignment == dfVerticalAlignment.Middle)
		{
			vectorOffset.y = vectorOffset.y - (height - vector2.y) * 0.5f;
		}
		else if (_dfVerticalAlignment == dfVerticalAlignment.Bottom)
		{
			vectorOffset.y = vectorOffset.y - (height - vector2.y);
		}
		return vectorOffset;
	}

	public override void Invalidate()
	{
		base.Invalidate();
		if (this.Font == null || !this.Font.IsValid)
		{
			return;
		}
		bool flag = this.size.sqrMagnitude <= 1.401298E-45f;
		if (!this.autoSize && !this.autoHeight && !flag)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.Text))
		{
			if (flag)
			{
				base.Size = new Vector2(150f, 24f);
			}
			if (this.AutoSize || this.AutoHeight)
			{
				base.Height = (float)Mathf.CeilToInt((float)this.Font.LineHeight * this.TextScale);
			}
			return;
		}
		using (dfFontRendererBase _dfFontRendererBase = this.obtainRenderer())
		{
			Vector2 num = _dfFontRendererBase.MeasureString(this.text).RoundToInt();
			if (this.AutoSize || flag)
			{
				this.size = num + new Vector2((float)this.padding.horizontal, (float)this.padding.vertical);
			}
			else if (this.AutoHeight)
			{
				this.size = new Vector2(this.size.x, num.y + (float)this.padding.vertical);
			}
		}
	}

	private dfFontRendererBase obtainRenderer()
	{
		Color32? nullable;
		bool size = base.Size.sqrMagnitude <= 1.401298E-45f;
		Vector2 vector2 = base.Size - new Vector2((float)this.padding.horizontal, (float)this.padding.vertical);
		Vector2 vector21 = (this.autoSize || size ? this.getAutoSizeDefault() : vector2);
		if (this.autoHeight)
		{
			vector21 = new Vector2(vector2.x, 2.14748365E+09f);
		}
		float units = base.PixelsToUnits();
		Vector3 upperLeft = (this.pivot.TransformToUpperLeft(base.Size) + new Vector3((float)this.padding.left, (float)(-this.padding.top))) * units;
		float textScale = this.TextScale * this.getTextScaleMultiplier();
		dfFontRendererBase wordWrap = this.Font.ObtainRenderer();
		wordWrap.WordWrap = this.WordWrap;
		wordWrap.MaxSize = vector21;
		wordWrap.PixelRatio = units;
		wordWrap.TextScale = textScale;
		wordWrap.CharacterSpacing = this.CharacterSpacing;
		wordWrap.VectorOffset = upperLeft.Quantize(units);
		wordWrap.MultiLine = true;
		wordWrap.TabSize = this.TabSize;
		wordWrap.TabStops = this.TabStops;
		wordWrap.TextAlign = (!this.autoSize ? this.TextAlignment : UnityEngine.TextAlignment.Left);
		wordWrap.ColorizeSymbols = this.ColorizeSymbols;
		wordWrap.ProcessMarkup = this.ProcessMarkup;
		wordWrap.DefaultColor = (!base.IsEnabled ? base.DisabledColor : base.Color);
		dfFontRendererBase _dfFontRendererBase = wordWrap;
		if (!this.enableGradient)
		{
			nullable = null;
		}
		else
		{
			nullable = new Color32?(this.BottomColor);
		}
		_dfFontRendererBase.BottomColor = nullable;
		wordWrap.OverrideMarkupColors = !base.IsEnabled;
		wordWrap.Opacity = base.CalculateOpacity();
		wordWrap.Outline = this.Outline;
		wordWrap.OutlineSize = this.OutlineSize;
		wordWrap.OutlineColor = this.OutlineColor;
		wordWrap.Shadow = this.Shadow;
		wordWrap.ShadowColor = this.ShadowColor;
		wordWrap.ShadowOffset = this.ShadowOffset;
		dfDynamicFont.DynamicFontRenderer atlas = wordWrap as dfDynamicFont.DynamicFontRenderer;
		if (atlas != null)
		{
			atlas.SpriteAtlas = this.Atlas;
			atlas.SpriteBuffer = this.renderData;
		}
		if (this.vertAlign != dfVerticalAlignment.Top)
		{
			wordWrap.VectorOffset = this.getVertAlignOffset(wordWrap);
		}
		return wordWrap;
	}

	public override void OnEnable()
	{
		bool flag;
		base.OnEnable();
		flag = (this.Font == null ? false : this.Font.IsValid);
		if (Application.isPlaying && !flag)
		{
			this.Font = base.GetManager().DefaultFont;
		}
		if (this.size.sqrMagnitude <= 1.401298E-45f)
		{
			base.Size = new Vector2(150f, 25f);
		}
	}

	protected internal override void OnLocalize()
	{
		base.OnLocalize();
		this.Text = base.getLocalizedValue(this.text);
	}

	protected internal void OnTextChanged()
	{
		this.Invalidate();
		base.Signal("OnTextChanged", new object[] { this.text });
		if (this.TextChanged != null)
		{
			this.TextChanged(this, this.text);
		}
	}

	protected internal virtual void renderBackground()
	{
		if (this.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo item = this.Atlas[this.backgroundSprite];
		if (item == null)
		{
			return;
		}
		Color32 color32 = base.ApplyOpacity(this.BackgroundColor);
		dfSprite.RenderOptions renderOption = new dfSprite.RenderOptions();
		dfSprite.RenderOptions upperLeft = renderOption;
		upperLeft.atlas = this.atlas;
		upperLeft.color = color32;
		upperLeft.fillAmount = 1f;
		upperLeft.offset = this.pivot.TransformToUpperLeft(base.Size);
		upperLeft.pixelsToUnits = base.PixelsToUnits();
		upperLeft.size = base.Size;
		upperLeft.spriteInfo = item;
		renderOption = upperLeft;
		if (item.border.horizontal != 0 || item.border.vertical != 0)
		{
			dfSlicedSprite.renderSprite(this.renderData, renderOption);
		}
		else
		{
			dfSprite.renderSprite(this.renderData, renderOption);
		}
	}

	public dfList<dfRenderData> RenderMultiple()
	{
		dfList<dfRenderData> dfRenderDatas;
		try
		{
			if (this.Atlas == null || this.Font == null || !this.isVisible || !this.Font.IsValid)
			{
				dfRenderDatas = null;
			}
			else
			{
				if (this.renderData == null)
				{
					this.renderData = dfRenderData.Obtain();
					this.textRenderData = dfRenderData.Obtain();
					this.isControlInvalidated = true;
				}
				if (this.isControlInvalidated)
				{
					this.buffers.Clear();
					this.renderData.Clear();
					this.renderData.Material = this.Atlas.Material;
					this.renderData.Transform = base.transform.localToWorldMatrix;
					this.buffers.Add(this.renderData);
					this.textRenderData.Clear();
					this.textRenderData.Material = this.Atlas.Material;
					this.textRenderData.Transform = base.transform.localToWorldMatrix;
					this.buffers.Add(this.textRenderData);
					this.renderBackground();
					if (!string.IsNullOrEmpty(this.Text))
					{
						bool flag = this.size.sqrMagnitude <= 1.401298E-45f;
						using (dfFontRendererBase _dfFontRendererBase = this.obtainRenderer())
						{
							_dfFontRendererBase.Render(this.text, this.textRenderData);
							if (this.AutoSize || flag)
							{
								base.Size = (_dfFontRendererBase.RenderedSize + new Vector2((float)this.padding.horizontal, (float)this.padding.vertical)).CeilToInt();
							}
							else if (this.AutoHeight)
							{
								float single = this.size.x;
								Vector2 renderedSize = _dfFontRendererBase.RenderedSize;
								base.Size = (new Vector2(single, renderedSize.y + (float)this.padding.vertical)).CeilToInt();
							}
						}
						this.updateCollider();
						dfRenderDatas = this.buffers;
					}
					else
					{
						if (this.AutoSize || this.AutoHeight)
						{
							base.Height = (float)Mathf.CeilToInt((float)this.Font.LineHeight * this.TextScale);
						}
						dfRenderDatas = this.buffers;
					}
				}
				else
				{
					for (int i = 0; i < this.buffers.Count; i++)
					{
						this.buffers[i].Transform = base.transform.localToWorldMatrix;
					}
					dfRenderDatas = this.buffers;
				}
			}
		}
		finally
		{
			this.isControlInvalidated = false;
		}
		return dfRenderDatas;
	}

	public override void Update()
	{
		if (this.autoSize)
		{
			this.autoHeight = false;
		}
		if (this.Font == null)
		{
			this.Font = base.GetManager().DefaultFont;
		}
		base.Update();
	}

	public event PropertyChangedEventHandler<string> TextChanged
	{
		add
		{
			this.TextChanged += value;
		}
		remove
		{
			this.TextChanged -= value;
		}
	}
}