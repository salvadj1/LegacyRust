using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Button")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfButton : dfInteractiveBase, IDFMultiRender
{
	[SerializeField]
	protected dfFontBase font;

	[SerializeField]
	protected string pressedSprite;

	[SerializeField]
	protected dfButton.ButtonState state;

	[SerializeField]
	protected dfControl @group;

	[SerializeField]
	protected string text = string.Empty;

	[SerializeField]
	protected UnityEngine.TextAlignment textAlign = UnityEngine.TextAlignment.Center;

	[SerializeField]
	protected dfVerticalAlignment vertAlign = dfVerticalAlignment.Middle;

	[SerializeField]
	protected Color32 textColor = UnityEngine.Color.white;

	[SerializeField]
	protected Color32 hoverText = UnityEngine.Color.white;

	[SerializeField]
	protected Color32 pressedText = UnityEngine.Color.white;

	[SerializeField]
	protected Color32 focusText = UnityEngine.Color.white;

	[SerializeField]
	protected Color32 disabledText = UnityEngine.Color.white;

	[SerializeField]
	protected Color32 hoverColor = UnityEngine.Color.white;

	[SerializeField]
	protected Color32 pressedColor = UnityEngine.Color.white;

	[SerializeField]
	protected Color32 focusColor = UnityEngine.Color.white;

	[SerializeField]
	protected float textScale = 1f;

	[SerializeField]
	protected dfTextScaleMode textScaleMode;

	[SerializeField]
	protected bool wordWrap;

	[SerializeField]
	protected RectOffset padding = new RectOffset();

	[SerializeField]
	protected bool textShadow;

	[SerializeField]
	protected Color32 shadowColor = UnityEngine.Color.black;

	[SerializeField]
	protected Vector2 shadowOffset = new Vector2(1f, -1f);

	[SerializeField]
	protected bool autoSize;

	private Vector2 startSize = Vector2.zero;

	private dfRenderData textRenderData;

	private dfList<dfRenderData> buffers = dfList<dfRenderData>.Obtain();

	private PropertyChangedEventHandler<dfButton.ButtonState> ButtonStateChanged;

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
				this.autoSize = value;
				if (value)
				{
					this.textAlign = UnityEngine.TextAlignment.Left;
				}
				this.Invalidate();
			}
		}
	}

	public dfControl ButtonGroup
	{
		get
		{
			return this.@group;
		}
		set
		{
			if (value != this.@group)
			{
				this.@group = value;
				this.Invalidate();
			}
		}
	}

	public Color32 DisabledTextColor
	{
		get
		{
			return this.disabledText;
		}
		set
		{
			this.disabledText = value;
			this.Invalidate();
		}
	}

	public Color32 FocusBackgroundColor
	{
		get
		{
			return this.focusColor;
		}
		set
		{
			this.focusColor = value;
			this.Invalidate();
		}
	}

	public Color32 FocusTextColor
	{
		get
		{
			return this.focusText;
		}
		set
		{
			this.focusText = value;
			this.Invalidate();
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
			}
			this.Invalidate();
		}
	}

	public Color32 HoverBackgroundColor
	{
		get
		{
			return this.hoverColor;
		}
		set
		{
			this.hoverColor = value;
			this.Invalidate();
		}
	}

	public Color32 HoverTextColor
	{
		get
		{
			return this.hoverText;
		}
		set
		{
			this.hoverText = value;
			this.Invalidate();
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

	public Color32 PressedBackgroundColor
	{
		get
		{
			return this.pressedColor;
		}
		set
		{
			this.pressedColor = value;
			this.Invalidate();
		}
	}

	public string PressedSprite
	{
		get
		{
			return this.pressedSprite;
		}
		set
		{
			if (value != this.pressedSprite)
			{
				this.pressedSprite = value;
				this.Invalidate();
			}
		}
	}

	public Color32 PressedTextColor
	{
		get
		{
			return this.pressedText;
		}
		set
		{
			this.pressedText = value;
			this.Invalidate();
		}
	}

	public bool Shadow
	{
		get
		{
			return this.textShadow;
		}
		set
		{
			if (value != this.textShadow)
			{
				this.textShadow = value;
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

	public dfButton.ButtonState State
	{
		get
		{
			return this.state;
		}
		set
		{
			if (value != this.state)
			{
				this.OnButtonStateChanged(value);
				this.Invalidate();
			}
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
			if (value != this.text)
			{
				this.text = base.getLocalizedValue(value);
				this.Invalidate();
			}
		}
	}

	public UnityEngine.TextAlignment TextAlignment
	{
		get
		{
			if (this.autoSize)
			{
				return UnityEngine.TextAlignment.Left;
			}
			return this.textAlign;
		}
		set
		{
			if (value != this.textAlign)
			{
				this.textAlign = value;
				this.Invalidate();
			}
		}
	}

	public Color32 TextColor
	{
		get
		{
			return this.textColor;
		}
		set
		{
			this.textColor = value;
			this.Invalidate();
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

	public dfButton()
	{
	}

	private void autoSizeToText()
	{
		if (this.Font == null || !this.Font.IsValid || string.IsNullOrEmpty(this.Text))
		{
			return;
		}
		using (dfFontRendererBase _dfFontRendererBase = this.obtainTextRenderer())
		{
			Vector2 vector2 = _dfFontRendererBase.MeasureString(this.Text);
			Vector2 vector21 = new Vector2(vector2.x + (float)this.padding.horizontal, vector2.y + (float)this.padding.vertical);
			base.Size = vector21;
		}
	}

	public override void Awake()
	{
		base.Awake();
		this.startSize = base.Size;
	}

	protected override Color32 getActiveColor()
	{
		switch (this.State)
		{
			case dfButton.ButtonState.Focus:
			{
				return this.FocusBackgroundColor;
			}
			case dfButton.ButtonState.Hover:
			{
				return this.HoverBackgroundColor;
			}
			case dfButton.ButtonState.Pressed:
			{
				return this.PressedBackgroundColor;
			}
			case dfButton.ButtonState.Disabled:
			{
				return base.DisabledColor;
			}
		}
		return base.Color;
	}

	protected internal override dfAtlas.ItemInfo getBackgroundSprite()
	{
		if (base.Atlas == null)
		{
			return null;
		}
		dfAtlas.ItemInfo item = null;
		switch (this.state)
		{
			case dfButton.ButtonState.Default:
			{
				item = this.atlas[this.backgroundSprite];
				break;
			}
			case dfButton.ButtonState.Focus:
			{
				item = this.atlas[this.focusSprite];
				break;
			}
			case dfButton.ButtonState.Hover:
			{
				item = this.atlas[this.hoverSprite];
				break;
			}
			case dfButton.ButtonState.Pressed:
			{
				item = this.atlas[this.pressedSprite];
				break;
			}
			case dfButton.ButtonState.Disabled:
			{
				item = this.atlas[this.disabledSprite];
				break;
			}
		}
		if (item == null)
		{
			item = this.atlas[this.backgroundSprite];
		}
		return item;
	}

	private Color32 getTextColorForState()
	{
		if (!base.IsEnabled)
		{
			return this.DisabledTextColor;
		}
		switch (this.state)
		{
			case dfButton.ButtonState.Default:
			{
				return this.TextColor;
			}
			case dfButton.ButtonState.Focus:
			{
				return this.FocusTextColor;
			}
			case dfButton.ButtonState.Hover:
			{
				return this.HoverTextColor;
			}
			case dfButton.ButtonState.Pressed:
			{
				return this.PressedTextColor;
			}
			case dfButton.ButtonState.Disabled:
			{
				return this.DisabledTextColor;
			}
		}
		return UnityEngine.Color.white;
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
		if (this.AutoSize)
		{
			this.autoSizeToText();
		}
	}

	private dfFontRendererBase obtainTextRenderer()
	{
		Vector2 size = base.Size - new Vector2((float)this.padding.horizontal, (float)this.padding.vertical);
		Vector2 vector2 = (!this.autoSize ? size : Vector2.one * 2.14748365E+09f);
		float units = base.PixelsToUnits();
		Vector3 upperLeft = (this.pivot.TransformToUpperLeft(base.Size) + new Vector3((float)this.padding.left, (float)(-this.padding.top))) * units;
		float textScale = this.TextScale * this.getTextScaleMultiplier();
		Color32 color32 = base.ApplyOpacity(this.getTextColorForState());
		dfFontRendererBase wordWrap = this.Font.ObtainRenderer();
		wordWrap.WordWrap = this.WordWrap;
		wordWrap.MultiLine = this.WordWrap;
		wordWrap.MaxSize = vector2;
		wordWrap.PixelRatio = units;
		wordWrap.TextScale = textScale;
		wordWrap.CharacterSpacing = 0;
		wordWrap.VectorOffset = upperLeft.Quantize(units);
		wordWrap.TabSize = 0;
		wordWrap.TextAlign = (!this.autoSize ? this.TextAlignment : UnityEngine.TextAlignment.Left);
		wordWrap.ProcessMarkup = true;
		wordWrap.DefaultColor = color32;
		wordWrap.OverrideMarkupColors = false;
		wordWrap.Opacity = base.CalculateOpacity();
		wordWrap.Shadow = this.Shadow;
		wordWrap.ShadowColor = this.ShadowColor;
		wordWrap.ShadowOffset = this.ShadowOffset;
		dfDynamicFont.DynamicFontRenderer atlas = wordWrap as dfDynamicFont.DynamicFontRenderer;
		if (atlas != null)
		{
			atlas.SpriteAtlas = base.Atlas;
			atlas.SpriteBuffer = this.renderData;
		}
		if (this.vertAlign != dfVerticalAlignment.Top)
		{
			wordWrap.VectorOffset = this.getVertAlignOffset(wordWrap);
		}
		return wordWrap;
	}

	protected virtual void OnButtonStateChanged(dfButton.ButtonState value)
	{
		if (!this.isEnabled && value != dfButton.ButtonState.Disabled)
		{
			return;
		}
		this.state = value;
		base.Signal("OnButtonStateChanged", new object[] { value });
		if (this.ButtonStateChanged != null)
		{
			this.ButtonStateChanged(this, value);
		}
		this.Invalidate();
	}

	protected internal override void OnClick(dfMouseEventArgs args)
	{
		if (this.@group != null)
		{
			dfButton[] componentsInChildren = base.transform.parent.GetComponentsInChildren<dfButton>();
			for (int i = 0; i < (int)componentsInChildren.Length; i++)
			{
				dfButton _dfButton = componentsInChildren[i];
				if (_dfButton != this && _dfButton.ButtonGroup == this.ButtonGroup && _dfButton != this)
				{
					_dfButton.State = dfButton.ButtonState.Default;
				}
			}
			if (!base.transform.IsChildOf(this.@group.transform))
			{
				base.Signal(this.@group.gameObject, "OnClick", new object[] { args });
			}
		}
		base.OnClick(args);
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
	}

	protected internal override void OnEnterFocus(dfFocusEventArgs args)
	{
		if (this.State != dfButton.ButtonState.Pressed)
		{
			this.State = dfButton.ButtonState.Focus;
		}
		base.OnEnterFocus(args);
	}

	protected internal override void OnIsEnabledChanged()
	{
		if (base.IsEnabled)
		{
			this.State = dfButton.ButtonState.Default;
		}
		else
		{
			this.State = dfButton.ButtonState.Disabled;
		}
		base.OnIsEnabledChanged();
	}

	protected internal override void OnKeyPress(dfKeyEventArgs args)
	{
		if (!this.IsInteractive || args.KeyCode != KeyCode.Space)
		{
			base.OnKeyPress(args);
			return;
		}
		Ray ray = new Ray();
		this.OnClick(new dfMouseEventArgs(this, dfMouseButtons.Left, 1, ray, Vector2.zero, 0f));
	}

	protected internal override void OnLeaveFocus(dfFocusEventArgs args)
	{
		this.State = dfButton.ButtonState.Default;
		base.OnLeaveFocus(args);
	}

	protected internal override void OnLocalize()
	{
		base.OnLocalize();
		this.Text = base.getLocalizedValue(this.text);
	}

	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		if (!(this.parent is dfTabstrip) || this.State != dfButton.ButtonState.Focus)
		{
			this.State = dfButton.ButtonState.Pressed;
		}
		base.OnMouseDown(args);
	}

	protected internal override void OnMouseEnter(dfMouseEventArgs args)
	{
		if (!(this.parent is dfTabstrip) || this.State != dfButton.ButtonState.Focus)
		{
			this.State = dfButton.ButtonState.Hover;
		}
		base.OnMouseEnter(args);
	}

	protected internal override void OnMouseLeave(dfMouseEventArgs args)
	{
		if (!this.ContainsFocus)
		{
			this.State = dfButton.ButtonState.Default;
		}
		else
		{
			this.State = dfButton.ButtonState.Focus;
		}
		base.OnMouseLeave(args);
	}

	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		if (this.isMouseHovering)
		{
			if (!(this.parent is dfTabstrip) || !this.ContainsFocus)
			{
				this.State = dfButton.ButtonState.Hover;
			}
			else
			{
				this.State = dfButton.ButtonState.Focus;
			}
		}
		else if (!this.HasFocus)
		{
			this.State = dfButton.ButtonState.Default;
		}
		else
		{
			this.State = dfButton.ButtonState.Focus;
		}
		base.OnMouseUp(args);
	}

	public dfList<dfRenderData> RenderMultiple()
	{
		if (!this.isVisible)
		{
			return null;
		}
		if (this.renderData == null)
		{
			this.renderData = dfRenderData.Obtain();
			this.textRenderData = dfRenderData.Obtain();
			this.isControlInvalidated = true;
		}
		if (!this.isControlInvalidated)
		{
			for (int i = 0; i < this.buffers.Count; i++)
			{
				this.buffers[i].Transform = base.transform.localToWorldMatrix;
			}
			return this.buffers;
		}
		this.isControlInvalidated = false;
		this.buffers.Clear();
		this.renderData.Clear();
		if (base.Atlas != null)
		{
			this.renderData.Material = base.Atlas.Material;
			this.renderData.Transform = base.transform.localToWorldMatrix;
			this.renderBackground();
			this.buffers.Add(this.renderData);
		}
		dfRenderData dfRenderDatum = this.renderText();
		if (dfRenderDatum != null && dfRenderDatum != this.renderData)
		{
			dfRenderDatum.Transform = base.transform.localToWorldMatrix;
			this.buffers.Add(dfRenderDatum);
		}
		this.updateCollider();
		return this.buffers;
	}

	private dfRenderData renderText()
	{
		if (this.Font == null || !this.Font.IsValid || string.IsNullOrEmpty(this.Text))
		{
			return null;
		}
		dfRenderData material = this.renderData;
		if (this.font is dfDynamicFont)
		{
			dfDynamicFont _dfDynamicFont = (dfDynamicFont)this.font;
			material = this.textRenderData;
			material.Clear();
			material.Material = _dfDynamicFont.Material;
		}
		using (dfFontRendererBase _dfFontRendererBase = this.obtainTextRenderer())
		{
			_dfFontRendererBase.Render(this.text, material);
		}
		return material;
	}

	public override void Update()
	{
		base.Update();
	}

	public event PropertyChangedEventHandler<dfButton.ButtonState> ButtonStateChanged
	{
		add
		{
			this.ButtonStateChanged += value;
		}
		remove
		{
			this.ButtonStateChanged -= value;
		}
	}

	public enum ButtonState
	{
		Default,
		Focus,
		Hover,
		Pressed,
		Disabled
	}
}