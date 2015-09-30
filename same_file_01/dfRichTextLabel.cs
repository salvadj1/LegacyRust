using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Rich Text Label")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfRichTextLabel : dfControl, IDFMultiRender
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected dfDynamicFont font;

	[SerializeField]
	protected string text = "Rich Text Label";

	[SerializeField]
	protected int fontSize = 16;

	[SerializeField]
	protected int lineheight = 16;

	[SerializeField]
	protected dfTextScaleMode textScaleMode;

	[SerializeField]
	protected UnityEngine.FontStyle fontStyle;

	[SerializeField]
	protected bool preserveWhitespace;

	[SerializeField]
	protected string blankTextureSprite;

	[SerializeField]
	protected dfMarkupTextAlign align;

	[SerializeField]
	protected bool allowScrolling;

	[SerializeField]
	protected dfScrollbar horzScrollbar;

	[SerializeField]
	protected dfScrollbar vertScrollbar;

	[SerializeField]
	protected bool useScrollMomentum;

	private static dfRenderData clipBuffer;

	private dfList<dfRenderData> buffers = new dfList<dfRenderData>();

	private dfList<dfMarkupElement> elements;

	private dfMarkupBox viewportBox;

	private dfMarkupTag mouseDownTag;

	private Vector2 mouseDownScrollPosition = Vector2.zero;

	private Vector2 scrollPosition = Vector2.zero;

	private bool initialized;

	private bool isMouseDown;

	private Vector2 touchStartPosition = Vector2.zero;

	private Vector2 scrollMomentum = Vector2.zero;

	private bool isMarkupInvalidated = true;

	private Vector2 startSize = Vector2.zero;

	private PropertyChangedEventHandler<string> TextChanged;

	private PropertyChangedEventHandler<Vector2> ScrollPositionChanged;

	private dfRichTextLabel.LinkClickEventHandler LinkClicked;

	public bool AllowScrolling
	{
		get
		{
			return this.allowScrolling;
		}
		set
		{
			this.allowScrolling = value;
			if (!value)
			{
				this.ScrollPosition = Vector2.zero;
			}
		}
	}

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

	public string BlankTextureSprite
	{
		get
		{
			return this.blankTextureSprite;
		}
		set
		{
			if (value != this.blankTextureSprite)
			{
				this.blankTextureSprite = value;
				this.Invalidate();
			}
		}
	}

	public Vector2 ContentSize
	{
		get
		{
			if (this.viewportBox == null)
			{
				return base.Size;
			}
			return this.viewportBox.Size;
		}
	}

	public dfDynamicFont Font
	{
		get
		{
			return this.font;
		}
		set
		{
			if (value != this.font)
			{
				this.font = value;
				this.LineHeight = value.FontSize;
				this.Invalidate();
			}
		}
	}

	public int FontSize
	{
		get
		{
			return this.fontSize;
		}
		set
		{
			value = Mathf.Max(6, value);
			if (value != this.fontSize)
			{
				this.fontSize = value;
				this.Invalidate();
			}
			this.LineHeight = value;
		}
	}

	public UnityEngine.FontStyle FontStyle
	{
		get
		{
			return this.fontStyle;
		}
		set
		{
			if (value != this.fontStyle)
			{
				this.fontStyle = value;
				this.Invalidate();
			}
		}
	}

	public dfScrollbar HorizontalScrollbar
	{
		get
		{
			return this.horzScrollbar;
		}
		set
		{
			this.horzScrollbar = value;
			this.updateScrollbars();
		}
	}

	public int LineHeight
	{
		get
		{
			return this.lineheight;
		}
		set
		{
			value = Mathf.Max(this.FontSize, value);
			if (value != this.lineheight)
			{
				this.lineheight = value;
				this.Invalidate();
			}
		}
	}

	public bool PreserveWhitespace
	{
		get
		{
			return this.preserveWhitespace;
		}
		set
		{
			if (value != this.preserveWhitespace)
			{
				this.preserveWhitespace = value;
				this.Invalidate();
			}
		}
	}

	public Vector2 ScrollPosition
	{
		get
		{
			return this.scrollPosition;
		}
		set
		{
			if (!this.allowScrolling)
			{
				value = Vector2.zero;
			}
			Vector2 contentSize = this.ContentSize - base.Size;
			value = Vector2.Min(contentSize, value);
			value = Vector2.Max(Vector2.zero, value);
			value = value.RoundToInt();
			if ((value - this.scrollPosition).sqrMagnitude > 1.401298E-45f)
			{
				this.scrollPosition = value;
				this.updateScrollbars();
				this.OnScrollPositionChanged();
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
			value = base.getLocalizedValue(value);
			if (!string.Equals(this.text, value))
			{
				this.text = value;
				this.scrollPosition = Vector2.zero;
				this.Invalidate();
				this.OnTextChanged();
			}
		}
	}

	public dfMarkupTextAlign TextAlignment
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

	public bool UseScrollMomentum
	{
		get
		{
			return this.useScrollMomentum;
		}
		set
		{
			this.useScrollMomentum = value;
			this.scrollMomentum = Vector2.zero;
		}
	}

	public dfScrollbar VerticalScrollbar
	{
		get
		{
			return this.vertScrollbar;
		}
		set
		{
			this.vertScrollbar = value;
			this.updateScrollbars();
		}
	}

	static dfRichTextLabel()
	{
		dfRichTextLabel.clipBuffer = new dfRenderData(32);
	}

	public dfRichTextLabel()
	{
	}

	public override void Awake()
	{
		base.Awake();
		this.startSize = base.Size;
	}

	private void clipToViewport(dfRenderData renderData)
	{
		Plane[] clippingPlanes = this.GetClippingPlanes();
		Material material = renderData.Material;
		Matrix4x4 transform = renderData.Transform;
		dfRichTextLabel.clipBuffer.Clear();
		dfClippingUtil.Clip(clippingPlanes, renderData, dfRichTextLabel.clipBuffer);
		renderData.Clear();
		renderData.Merge(dfRichTextLabel.clipBuffer, false);
		renderData.Material = material;
		renderData.Transform = transform;
	}

	private void gatherRenderBuffers(dfMarkupBox box, dfList<dfRenderData> buffers)
	{
		dfIntersectionType viewportIntersection = this.getViewportIntersection(box);
		if (viewportIntersection == dfIntersectionType.None)
		{
			return;
		}
		dfRenderData material = box.Render();
		if (material != null)
		{
			if (material.Material == null && this.atlas != null)
			{
				material.Material = this.atlas.Material;
			}
			float units = base.PixelsToUnits();
			Vector2 num = -this.scrollPosition.Scale(1f, -1f).RoundToInt();
			Vector3 vector3 = (num + box.GetOffset().Scale(1f, -1f)) + this.pivot.TransformToUpperLeft(base.Size);
			dfList<Vector3> vertices = material.Vertices;
			Matrix4x4 matrix4x4 = base.transform.localToWorldMatrix;
			for (int i = 0; i < material.Vertices.Count; i++)
			{
				vertices[i] = matrix4x4.MultiplyPoint((vector3 + vertices[i]) * units);
			}
			if (viewportIntersection == dfIntersectionType.Intersecting)
			{
				this.clipToViewport(material);
			}
			buffers.Add(material);
		}
		for (int j = 0; j < box.Children.Count; j++)
		{
			this.gatherRenderBuffers(box.Children[j], buffers);
		}
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
		return base.Size.y / this.startSize.y;
	}

	private dfIntersectionType getViewportIntersection(dfMarkupBox box)
	{
		if (box.Display == dfMarkupDisplayType.none)
		{
			return dfIntersectionType.None;
		}
		Vector2 size = base.Size;
		Vector2 offset = box.GetOffset() - this.scrollPosition;
		Vector2 vector2 = offset + box.Size;
		if (vector2.x <= 0f || vector2.y <= 0f)
		{
			return dfIntersectionType.None;
		}
		if (offset.x >= size.x || offset.y >= size.y)
		{
			return dfIntersectionType.None;
		}
		if (offset.x >= 0f && offset.y >= 0f && vector2.x <= size.x && vector2.y <= size.y)
		{
			return dfIntersectionType.Inside;
		}
		return dfIntersectionType.Intersecting;
	}

	private dfMarkupTag hitTestTag(dfMouseEventArgs args)
	{
		Vector2 hitPosition = base.GetHitPosition(args) + this.scrollPosition;
		dfMarkupBox _dfMarkupBox = this.viewportBox.HitTest(hitPosition);
		if (_dfMarkupBox == null)
		{
			return null;
		}
		dfMarkupElement element = _dfMarkupBox.Element;
		while (element != null && !(element is dfMarkupTag))
		{
			element = element.Parent;
		}
		return element as dfMarkupTag;
	}

	private void horzScroll_ValueChanged(dfControl control, float value)
	{
		this.ScrollPosition = new Vector2(value, this.ScrollPosition.y);
	}

	[HideInInspector]
	private void initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		if (Application.isPlaying)
		{
			if (this.horzScrollbar != null)
			{
				this.horzScrollbar.ValueChanged += new PropertyChangedEventHandler<float>(this.horzScroll_ValueChanged);
			}
			if (this.vertScrollbar != null)
			{
				this.vertScrollbar.ValueChanged += new PropertyChangedEventHandler<float>(this.vertScroll_ValueChanged);
			}
		}
		this.Invalidate();
		this.ScrollPosition = Vector2.zero;
		this.updateScrollbars();
	}

	public override void Invalidate()
	{
		base.Invalidate();
		this.isMarkupInvalidated = true;
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		this.initialize();
	}

	internal override void OnDragEnd(dfDragEventArgs args)
	{
		base.OnDragEnd(args);
		this.isMouseDown = false;
	}

	public override void OnEnable()
	{
		base.OnEnable();
		if (this.size.sqrMagnitude <= 1.401298E-45f)
		{
			base.Size = new Vector2(320f, 200f);
			int num = 16;
			this.LineHeight = num;
			this.FontSize = num;
		}
	}

	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (args.Used)
		{
			base.OnKeyDown(args);
			return;
		}
		int fontSize = this.FontSize;
		int num = this.FontSize;
		if (args.KeyCode == KeyCode.LeftArrow)
		{
			dfRichTextLabel scrollPosition = this;
			scrollPosition.ScrollPosition = scrollPosition.ScrollPosition + new Vector2((float)(-fontSize), 0f);
			args.Use();
		}
		else if (args.KeyCode == KeyCode.RightArrow)
		{
			dfRichTextLabel _dfRichTextLabel = this;
			_dfRichTextLabel.ScrollPosition = _dfRichTextLabel.ScrollPosition + new Vector2((float)fontSize, 0f);
			args.Use();
		}
		else if (args.KeyCode == KeyCode.UpArrow)
		{
			dfRichTextLabel scrollPosition1 = this;
			scrollPosition1.ScrollPosition = scrollPosition1.ScrollPosition + new Vector2(0f, (float)(-num));
			args.Use();
		}
		else if (args.KeyCode == KeyCode.DownArrow)
		{
			dfRichTextLabel _dfRichTextLabel1 = this;
			_dfRichTextLabel1.ScrollPosition = _dfRichTextLabel1.ScrollPosition + new Vector2(0f, (float)num);
			args.Use();
		}
		base.OnKeyDown(args);
	}

	protected internal override void OnLocalize()
	{
		base.OnLocalize();
		this.Text = base.getLocalizedValue(this.text);
	}

	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		base.OnMouseDown(args);
		this.mouseDownTag = this.hitTestTag(args);
		this.mouseDownScrollPosition = this.scrollPosition;
		this.scrollMomentum = Vector2.zero;
		this.touchStartPosition = args.Position;
		this.isMouseDown = true;
	}

	protected internal override void OnMouseEnter(dfMouseEventArgs args)
	{
		base.OnMouseEnter(args);
		this.touchStartPosition = args.Position;
	}

	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		base.OnMouseMove(args);
		if (!this.allowScrolling)
		{
			return;
		}
		if ((args is dfTouchEventArgs ? true : this.isMouseDown) && (args.Position - this.touchStartPosition).magnitude > 5f)
		{
			Vector2 vector2 = args.MoveDelta.Scale(-1f, 1f);
			dfRichTextLabel scrollPosition = this;
			scrollPosition.ScrollPosition = scrollPosition.ScrollPosition + vector2;
			this.scrollMomentum = (this.scrollMomentum + vector2) * 0.5f;
		}
	}

	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		base.OnMouseUp(args);
		this.isMouseDown = false;
		if (Vector2.Distance(this.scrollPosition, this.mouseDownScrollPosition) <= 2f && this.hitTestTag(args) == this.mouseDownTag)
		{
			dfMarkupTag parent = this.mouseDownTag;
			while (parent != null && !(parent is dfMarkupTagAnchor))
			{
				parent = parent.Parent as dfMarkupTag;
			}
			if (parent is dfMarkupTagAnchor)
			{
				base.Signal("OnLinkClicked", new object[] { parent });
				if (this.LinkClicked != null)
				{
					this.LinkClicked(this, parent as dfMarkupTagAnchor);
				}
			}
		}
		this.mouseDownTag = null;
		this.mouseDownScrollPosition = this.scrollPosition;
	}

	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		try
		{
			if (!args.Used && this.allowScrolling)
			{
				int num = (!this.UseScrollMomentum ? 3 : 1);
				float single = (this.vertScrollbar == null ? (float)(this.FontSize * num) : this.vertScrollbar.IncrementAmount);
				this.ScrollPosition = new Vector2(this.scrollPosition.x, this.scrollPosition.y - single * args.WheelDelta);
				this.scrollMomentum = new Vector2(0f, -single * args.WheelDelta);
				args.Use();
				base.Signal("OnMouseWheel", new object[] { args });
			}
		}
		finally
		{
			base.OnMouseWheel(args);
		}
	}

	protected internal void OnScrollPositionChanged()
	{
		base.Invalidate();
		base.SignalHierarchy("OnScrollPositionChanged", new object[] { this.ScrollPosition });
		if (this.ScrollPositionChanged != null)
		{
			this.ScrollPositionChanged(this, this.ScrollPosition);
		}
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

	private void processMarkup()
	{
		this.releaseMarkupReferences();
		this.elements = dfMarkupParser.Parse(this, this.text);
		float textScaleMultiplier = this.getTextScaleMultiplier();
		int num = Mathf.CeilToInt((float)this.FontSize * textScaleMultiplier);
		int num1 = Mathf.CeilToInt((float)this.LineHeight * textScaleMultiplier);
		dfMarkupStyle _dfMarkupStyle = new dfMarkupStyle();
		dfMarkupStyle atlas = _dfMarkupStyle;
		atlas.Host = this;
		atlas.Atlas = this.Atlas;
		atlas.Font = this.Font;
		atlas.FontSize = num;
		atlas.FontStyle = this.FontStyle;
		atlas.LineHeight = num1;
		atlas.Color = base.ApplyOpacity(base.Color);
		atlas.Opacity = base.CalculateOpacity();
		atlas.Align = this.TextAlignment;
		atlas.PreserveWhitespace = this.preserveWhitespace;
		_dfMarkupStyle = atlas;
		dfMarkupBox _dfMarkupBox = new dfMarkupBox(null, dfMarkupDisplayType.block, _dfMarkupStyle)
		{
			Size = base.Size
		};
		this.viewportBox = _dfMarkupBox;
		for (int i = 0; i < this.elements.Count; i++)
		{
			dfMarkupElement item = this.elements[i];
			if (item != null)
			{
				item.PerformLayout(this.viewportBox, _dfMarkupStyle);
			}
		}
	}

	private void releaseMarkupReferences()
	{
		this.mouseDownTag = null;
		if (this.viewportBox != null)
		{
			this.viewportBox.Release();
		}
		if (this.elements != null)
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				this.elements[i].Release();
			}
			this.elements.Release();
		}
	}

	public dfList<dfRenderData> RenderMultiple()
	{
		dfList<dfRenderData> dfRenderDatas;
		if (!this.isVisible || this.Font == null)
		{
			return null;
		}
		if (!this.isControlInvalidated && this.viewportBox != null)
		{
			this.buffers.Clear();
			this.gatherRenderBuffers(this.viewportBox, this.buffers);
			return this.buffers;
		}
		try
		{
			if (this.isMarkupInvalidated)
			{
				this.isMarkupInvalidated = false;
				this.processMarkup();
			}
			this.viewportBox.FitToContents(false);
			this.updateScrollbars();
			this.buffers.Clear();
			this.gatherRenderBuffers(this.viewportBox, this.buffers);
			dfRenderDatas = this.buffers;
		}
		finally
		{
			this.isControlInvalidated = false;
		}
		return dfRenderDatas;
	}

	public void ScrollToBottom()
	{
		this.ScrollPosition = new Vector2(this.scrollPosition.x, 2.14748365E+09f);
	}

	public void ScrollToLeft()
	{
		this.ScrollPosition = new Vector2(0f, this.scrollPosition.y);
	}

	public void ScrollToRight()
	{
		this.ScrollPosition = new Vector2(2.14748365E+09f, this.scrollPosition.y);
	}

	public void ScrollToTop()
	{
		this.ScrollPosition = new Vector2(this.scrollPosition.x, 0f);
	}

	public override void Update()
	{
		base.Update();
		if (this.useScrollMomentum && !this.isMouseDown && this.scrollMomentum.magnitude > 0.1f)
		{
			dfRichTextLabel scrollPosition = this;
			scrollPosition.ScrollPosition = scrollPosition.ScrollPosition + this.scrollMomentum;
			dfRichTextLabel _dfRichTextLabel = this;
			_dfRichTextLabel.scrollMomentum = _dfRichTextLabel.scrollMomentum * (0.95f - Time.deltaTime);
		}
	}

	private void updateScrollbars()
	{
		if (this.horzScrollbar != null)
		{
			this.horzScrollbar.MinValue = 0f;
			this.horzScrollbar.MaxValue = this.ContentSize.x;
			this.horzScrollbar.ScrollSize = base.Size.x;
			this.horzScrollbar.Value = this.ScrollPosition.x;
		}
		if (this.vertScrollbar != null)
		{
			this.vertScrollbar.MinValue = 0f;
			this.vertScrollbar.MaxValue = this.ContentSize.y;
			this.vertScrollbar.ScrollSize = base.Size.y;
			this.vertScrollbar.Value = this.ScrollPosition.y;
		}
	}

	private void vertScroll_ValueChanged(dfControl control, float value)
	{
		this.ScrollPosition = new Vector2(this.scrollPosition.x, value);
	}

	public event dfRichTextLabel.LinkClickEventHandler LinkClicked
	{
		add
		{
			this.LinkClicked += value;
		}
		remove
		{
			this.LinkClicked -= value;
		}
	}

	public event PropertyChangedEventHandler<Vector2> ScrollPositionChanged
	{
		add
		{
			this.ScrollPositionChanged += value;
		}
		remove
		{
			this.ScrollPositionChanged -= value;
		}
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

	[dfEventCategory("Markup")]
	public delegate void LinkClickEventHandler(dfRichTextLabel sender, dfMarkupTagAnchor tag);
}