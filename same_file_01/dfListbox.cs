using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Listbox")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfListbox : dfInteractiveBase, IDFMultiRender
{
	[SerializeField]
	protected dfFontBase font;

	[SerializeField]
	protected RectOffset listPadding = new RectOffset();

	[SerializeField]
	protected int selectedIndex = -1;

	[SerializeField]
	protected Color32 itemTextColor = UnityEngine.Color.white;

	[SerializeField]
	protected float itemTextScale = 1f;

	[SerializeField]
	protected int itemHeight = 25;

	[SerializeField]
	protected RectOffset itemPadding = new RectOffset();

	[SerializeField]
	protected string[] items = new string[0];

	[SerializeField]
	protected string itemHighlight = string.Empty;

	[SerializeField]
	protected string itemHover = string.Empty;

	[SerializeField]
	protected dfScrollbar scrollbar;

	[SerializeField]
	protected bool animateHover;

	[SerializeField]
	protected bool shadow;

	[SerializeField]
	protected dfTextScaleMode textScaleMode;

	[SerializeField]
	protected Color32 shadowColor = UnityEngine.Color.black;

	[SerializeField]
	protected Vector2 shadowOffset = new Vector2(1f, -1f);

	[SerializeField]
	protected TextAlignment itemAlignment;

	private bool eventsAttached;

	private float scrollPosition;

	private int hoverIndex = -1;

	private float hoverTweenLocation;

	private Vector2 touchStartPosition = Vector2.zero;

	private Vector2 startSize = Vector2.zero;

	private dfRenderData textRenderData;

	private dfList<dfRenderData> buffers = dfList<dfRenderData>.Obtain();

	private PropertyChangedEventHandler<int> SelectedIndexChanged;

	private PropertyChangedEventHandler<int> ItemClicked;

	public bool AnimateHover
	{
		get
		{
			return this.animateHover;
		}
		set
		{
			this.animateHover = value;
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

	public TextAlignment ItemAlignment
	{
		get
		{
			return this.itemAlignment;
		}
		set
		{
			if (value != this.itemAlignment)
			{
				this.itemAlignment = value;
				this.Invalidate();
			}
		}
	}

	public int ItemHeight
	{
		get
		{
			return this.itemHeight;
		}
		set
		{
			this.scrollPosition = 0f;
			value = Mathf.Max(1, value);
			if (value != this.itemHeight)
			{
				this.itemHeight = value;
				this.Invalidate();
			}
		}
	}

	public string ItemHighlight
	{
		get
		{
			return this.itemHighlight;
		}
		set
		{
			if (value != this.itemHighlight)
			{
				this.itemHighlight = value;
				this.Invalidate();
			}
		}
	}

	public string ItemHover
	{
		get
		{
			return this.itemHover;
		}
		set
		{
			if (value != this.itemHover)
			{
				this.itemHover = value;
				this.Invalidate();
			}
		}
	}

	public RectOffset ItemPadding
	{
		get
		{
			if (this.itemPadding == null)
			{
				this.itemPadding = new RectOffset();
			}
			return this.itemPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!value.Equals(this.itemPadding))
			{
				this.itemPadding = value;
				this.Invalidate();
			}
		}
	}

	public string[] Items
	{
		get
		{
			if (this.items == null)
			{
				this.items = new string[0];
			}
			return this.items;
		}
		set
		{
			if (value != this.items)
			{
				this.scrollPosition = 0f;
				if (value == null)
				{
					value = new string[0];
				}
				this.items = value;
				this.Invalidate();
			}
		}
	}

	public Color32 ItemTextColor
	{
		get
		{
			return this.itemTextColor;
		}
		set
		{
			if (!value.Equals(this.itemTextColor))
			{
				this.itemTextColor = value;
				this.Invalidate();
			}
		}
	}

	public float ItemTextScale
	{
		get
		{
			return this.itemTextScale;
		}
		set
		{
			value = Mathf.Max(0.1f, value);
			if (!Mathf.Approximately(this.itemTextScale, value))
			{
				this.itemTextScale = value;
				this.Invalidate();
			}
		}
	}

	public RectOffset ListPadding
	{
		get
		{
			if (this.listPadding == null)
			{
				this.listPadding = new RectOffset();
			}
			return this.listPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.listPadding))
			{
				this.listPadding = value;
				this.Invalidate();
			}
		}
	}

	public dfScrollbar Scrollbar
	{
		get
		{
			return this.scrollbar;
		}
		set
		{
			this.scrollPosition = 0f;
			if (value != this.scrollbar)
			{
				this.detachScrollbarEvents();
				this.scrollbar = value;
				this.attachScrollbarEvents();
				this.Invalidate();
			}
		}
	}

	public float ScrollPosition
	{
		get
		{
			return this.scrollPosition;
		}
		set
		{
			if (!Mathf.Approximately(value, this.scrollPosition))
			{
				this.scrollPosition = this.constrainScrollPosition(value);
				this.Invalidate();
			}
		}
	}

	public int SelectedIndex
	{
		get
		{
			return this.selectedIndex;
		}
		set
		{
			value = Mathf.Max(-1, value);
			value = Mathf.Min((int)this.items.Length - 1, value);
			if (value != this.selectedIndex)
			{
				this.selectedIndex = value;
				this.EnsureVisible(value);
				this.OnSelectedIndexChanged();
				this.Invalidate();
			}
		}
	}

	public string SelectedItem
	{
		get
		{
			if (this.selectedIndex == -1)
			{
				return null;
			}
			return this.items[this.selectedIndex];
		}
	}

	public string SelectedValue
	{
		get
		{
			return this.items[this.selectedIndex];
		}
		set
		{
			this.selectedIndex = -1;
			int num = 0;
			while (num < (int)this.items.Length)
			{
				if (this.items[num] != value)
				{
					num++;
				}
				else
				{
					this.selectedIndex = num;
					break;
				}
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

	public dfListbox()
	{
	}

	private void attachScrollbarEvents()
	{
		if (this.scrollbar == null || this.eventsAttached)
		{
			return;
		}
		this.eventsAttached = true;
		this.scrollbar.ValueChanged += new PropertyChangedEventHandler<float>(this.scrollbar_ValueChanged);
		this.scrollbar.GotFocus += new FocusEventHandler(this.scrollbar_GotFocus);
	}

	public override void Awake()
	{
		base.Awake();
		this.startSize = base.Size;
	}

	private void clipQuads(dfRenderData buffer, int startIndex)
	{
		dfList<Vector3> vertices = buffer.Vertices;
		dfList<Vector2> uV = buffer.UV;
		float units = base.PixelsToUnits();
		Vector3 upperLeft = base.Pivot.TransformToUpperLeft(base.Size);
		float single = (upperLeft.y - (float)this.listPadding.top) * units;
		float single1 = single - (this.size.y - (float)this.listPadding.vertical) * units;
		for (int i = startIndex; i < vertices.Count; i = i + 4)
		{
			Vector3 item = vertices[i];
			Vector3 vector3 = vertices[i + 1];
			Vector3 item1 = vertices[i + 2];
			Vector3 vector31 = vertices[i + 3];
			float single2 = item.y - vector31.y;
			if (vector31.y < single1)
			{
				float single3 = 1f - Mathf.Abs(-single1 + item.y) / single2;
				item = new Vector3(item.x, Mathf.Max(item.y, single1), vector3.z);
				vertices[i] = item;
				vector3 = new Vector3(vector3.x, Mathf.Max(vector3.y, single1), vector3.z);
				vertices[i + 1] = vector3;
				item1 = new Vector3(item1.x, Mathf.Max(item1.y, single1), item1.z);
				vertices[i + 2] = item1;
				vector31 = new Vector3(vector31.x, Mathf.Max(vector31.y, single1), vector31.z);
				vertices[i + 3] = vector31;
				float item2 = uV[i + 3].y;
				Vector2 vector2 = uV[i];
				float single4 = Mathf.Lerp(item2, vector2.y, single3);
				Vector2 vector21 = uV[i + 3];
				uV[i + 3] = new Vector2(vector21.x, single4);
				Vector2 vector22 = uV[i + 2];
				uV[i + 2] = new Vector2(vector22.x, single4);
				single2 = Mathf.Abs(vector31.y - item.y);
			}
			if (item.y > single)
			{
				float single5 = Mathf.Abs(single - item.y) / single2;
				vertices[i] = new Vector3(item.x, Mathf.Min(single, item.y), item.z);
				vertices[i + 1] = new Vector3(vector3.x, Mathf.Min(single, vector3.y), vector3.z);
				vertices[i + 2] = new Vector3(item1.x, Mathf.Min(single, item1.y), item1.z);
				vertices[i + 3] = new Vector3(vector31.x, Mathf.Min(single, vector31.y), vector31.z);
				float item3 = uV[i].y;
				Vector2 vector23 = uV[i + 3];
				float single6 = Mathf.Lerp(item3, vector23.y, single5);
				Vector2 item4 = uV[i];
				uV[i] = new Vector2(item4.x, single6);
				Vector2 vector24 = uV[i + 1];
				uV[i + 1] = new Vector2(vector24.x, single6);
			}
		}
	}

	private float constrainScrollPosition(float value)
	{
		value = Mathf.Max(0f, value);
		int length = (int)this.items.Length * this.itemHeight;
		float single = this.size.y - (float)this.listPadding.vertical;
		if ((float)length < single)
		{
			return 0f;
		}
		return Mathf.Min(value, (float)length - single);
	}

	private void detachScrollbarEvents()
	{
		if (this.scrollbar == null || !this.eventsAttached)
		{
			return;
		}
		this.eventsAttached = false;
		this.scrollbar.ValueChanged -= new PropertyChangedEventHandler<float>(this.scrollbar_ValueChanged);
		this.scrollbar.GotFocus -= new FocusEventHandler(this.scrollbar_GotFocus);
	}

	public void EnsureVisible(int index)
	{
		int num = index * this.ItemHeight;
		if (this.scrollPosition > (float)num)
		{
			this.ScrollPosition = (float)num;
		}
		float single = this.size.y - (float)this.listPadding.vertical;
		if (this.scrollPosition + single < (float)(num + this.itemHeight))
		{
			this.ScrollPosition = (float)num - single + (float)this.itemHeight;
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

	public override void LateUpdate()
	{
		base.LateUpdate();
		if (!Application.isPlaying)
		{
			return;
		}
		this.attachScrollbarEvents();
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		this.detachScrollbarEvents();
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.detachScrollbarEvents();
	}

	protected internal virtual void OnItemClicked()
	{
		base.Signal("OnItemClicked", new object[] { this.selectedIndex });
		if (this.ItemClicked != null)
		{
			this.ItemClicked(this, this.selectedIndex);
		}
	}

	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		switch (args.KeyCode)
		{
			case KeyCode.UpArrow:
			{
				this.SelectedIndex = Mathf.Max(0, this.selectedIndex - 1);
				base.OnKeyDown(args);
				return;
			}
			case KeyCode.DownArrow:
			{
				dfListbox selectedIndex = this;
				selectedIndex.SelectedIndex = selectedIndex.SelectedIndex + 1;
				base.OnKeyDown(args);
				return;
			}
			case KeyCode.RightArrow:
			case KeyCode.LeftArrow:
			case KeyCode.Insert:
			{
				base.OnKeyDown(args);
				return;
			}
			case KeyCode.Home:
			{
				this.SelectedIndex = 0;
				base.OnKeyDown(args);
				return;
			}
			case KeyCode.End:
			{
				this.SelectedIndex = (int)this.items.Length;
				base.OnKeyDown(args);
				return;
			}
			case KeyCode.PageUp:
			{
				int num = this.SelectedIndex - Mathf.FloorToInt((this.size.y - (float)this.listPadding.vertical) / (float)this.itemHeight);
				this.SelectedIndex = Mathf.Max(0, num);
				base.OnKeyDown(args);
				return;
			}
			case KeyCode.PageDown:
			{
				dfListbox _dfListbox = this;
				_dfListbox.SelectedIndex = _dfListbox.SelectedIndex + Mathf.FloorToInt((this.size.y - (float)this.listPadding.vertical) / (float)this.itemHeight);
				base.OnKeyDown(args);
				return;
			}
			default:
			{
				base.OnKeyDown(args);
				return;
			}
		}
	}

	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		base.OnMouseDown(args);
		if (!(args is dfTouchEventArgs))
		{
			this.selectItemUnderMouse(args);
			return;
		}
		this.touchStartPosition = args.Position;
	}

	protected internal override void OnMouseEnter(dfMouseEventArgs args)
	{
		base.OnMouseEnter(args);
		this.touchStartPosition = args.Position;
	}

	protected internal override void OnMouseLeave(dfMouseEventArgs args)
	{
		base.OnMouseLeave(args);
		this.hoverIndex = -1;
	}

	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		base.OnMouseMove(args);
		if (!(args is dfTouchEventArgs))
		{
			this.updateItemHover(args);
			return;
		}
		if (Mathf.Abs(args.Position.y - this.touchStartPosition.y) < (float)(this.itemHeight / 2))
		{
			return;
		}
		float scrollPosition = this.ScrollPosition;
		Vector2 moveDelta = args.MoveDelta;
		this.ScrollPosition = Mathf.Max(0f, scrollPosition + moveDelta.y);
		this.synchronizeScrollbar();
		this.hoverIndex = -1;
	}

	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		this.hoverIndex = -1;
		base.OnMouseUp(args);
		if (args is dfTouchEventArgs && Mathf.Abs(args.Position.y - this.touchStartPosition.y) < (float)this.itemHeight)
		{
			this.selectItemUnderMouse(args);
		}
	}

	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		base.OnMouseWheel(args);
		this.ScrollPosition = Mathf.Max(0f, this.ScrollPosition - (float)((int)args.WheelDelta * this.ItemHeight));
		this.synchronizeScrollbar();
		this.updateItemHover(args);
	}

	protected internal virtual void OnSelectedIndexChanged()
	{
		base.SignalHierarchy("OnSelectedIndexChanged", new object[] { this.selectedIndex });
		if (this.SelectedIndexChanged != null)
		{
			this.SelectedIndexChanged(this, this.selectedIndex);
		}
	}

	private void renderHover()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if ((base.Atlas == null || !base.IsEnabled || this.hoverIndex < 0 || this.hoverIndex > (int)this.items.Length - 1 ? true : string.IsNullOrEmpty(this.ItemHover)))
		{
			return;
		}
		dfAtlas.ItemInfo item = base.Atlas[this.ItemHover];
		if (item == null)
		{
			return;
		}
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector3 = new Vector3(upperLeft.x + (float)this.listPadding.left, upperLeft.y - (float)this.listPadding.top + this.scrollPosition, 0f);
		float units = base.PixelsToUnits();
		int num = this.hoverIndex * this.itemHeight;
		if (!this.animateHover)
		{
			this.hoverTweenLocation = (float)num;
		}
		else
		{
			float single = Mathf.Abs(this.hoverTweenLocation - (float)num);
			float single1 = (this.size.y - (float)this.listPadding.vertical) * 0.5f;
			if (single > single1)
			{
				this.hoverTweenLocation = (float)num + Mathf.Sign(this.hoverTweenLocation - (float)num) * single1;
				single = single1;
			}
			float single2 = Time.deltaTime / units * 2f;
			this.hoverTweenLocation = Mathf.MoveTowards(this.hoverTweenLocation, (float)num, single2);
		}
		vector3.y = vector3.y - this.hoverTweenLocation.Quantize(units);
		Color32 color32 = base.ApplyOpacity(this.color);
		dfSprite.RenderOptions renderOption = new dfSprite.RenderOptions();
		dfSprite.RenderOptions units1 = renderOption;
		units1.atlas = this.atlas;
		units1.color = color32;
		units1.fillAmount = 1f;
		units1.pixelsToUnits = base.PixelsToUnits();
		units1.size = new Vector3(this.size.x - (float)this.listPadding.horizontal, (float)this.itemHeight);
		units1.spriteInfo = item;
		units1.offset = vector3;
		renderOption = units1;
		if (item.border.horizontal > 0 || item.border.vertical > 0)
		{
			dfSlicedSprite.renderSprite(this.renderData, renderOption);
		}
		else
		{
			dfSprite.renderSprite(this.renderData, renderOption);
		}
		if ((float)num != this.hoverTweenLocation)
		{
			this.Invalidate();
		}
	}

	private void renderItems(dfRenderData buffer)
	{
		if (this.font == null || this.items == null || (int)this.items.Length == 0)
		{
			return;
		}
		float units = base.PixelsToUnits();
		Vector2 vector2 = new Vector2(this.size.x - (float)this.itemPadding.horizontal - (float)this.listPadding.horizontal, (float)(this.itemHeight - this.itemPadding.vertical));
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector3 = new Vector3(upperLeft.x + (float)this.itemPadding.left + (float)this.listPadding.left, upperLeft.y - (float)this.itemPadding.top - (float)this.listPadding.top, 0f) * units;
		vector3.y = vector3.y + this.scrollPosition * units;
		Color32 color32 = (!base.IsEnabled ? base.DisabledColor : this.ItemTextColor);
		float single = upperLeft.y * units;
		float single1 = single - this.size.y * units;
		for (int i = 0; i < (int)this.items.Length; i++)
		{
			using (dfFontRendererBase itemTextScale = this.font.ObtainRenderer())
			{
				itemTextScale.WordWrap = false;
				itemTextScale.MaxSize = vector2;
				itemTextScale.PixelRatio = units;
				itemTextScale.TextScale = this.ItemTextScale * this.getTextScaleMultiplier();
				itemTextScale.VectorOffset = vector3;
				itemTextScale.MultiLine = false;
				itemTextScale.TextAlign = this.ItemAlignment;
				itemTextScale.ProcessMarkup = true;
				itemTextScale.DefaultColor = color32;
				itemTextScale.OverrideMarkupColors = false;
				itemTextScale.Opacity = base.CalculateOpacity();
				itemTextScale.Shadow = this.Shadow;
				itemTextScale.ShadowColor = this.ShadowColor;
				itemTextScale.ShadowOffset = this.ShadowOffset;
				dfDynamicFont.DynamicFontRenderer atlas = itemTextScale as dfDynamicFont.DynamicFontRenderer;
				if (atlas != null)
				{
					atlas.SpriteAtlas = base.Atlas;
					atlas.SpriteBuffer = this.renderData;
				}
				if (vector3.y - (float)this.itemHeight * units <= single)
				{
					itemTextScale.Render(this.items[i], buffer);
				}
				vector3.y = vector3.y - (float)this.itemHeight * units;
				itemTextScale.VectorOffset = vector3;
				if (vector3.y < single1)
				{
					break;
				}
			}
		}
	}

	public dfList<dfRenderData> RenderMultiple()
	{
		if (base.Atlas == null || this.Font == null)
		{
			return null;
		}
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
		this.buffers.Clear();
		this.renderData.Clear();
		this.renderData.Material = base.Atlas.Material;
		this.renderData.Transform = base.transform.localToWorldMatrix;
		this.buffers.Add(this.renderData);
		this.textRenderData.Clear();
		this.textRenderData.Material = base.Atlas.Material;
		this.textRenderData.Transform = base.transform.localToWorldMatrix;
		this.buffers.Add(this.textRenderData);
		this.renderBackground();
		int count = this.renderData.Vertices.Count;
		this.renderHover();
		this.renderSelection();
		this.renderItems(this.textRenderData);
		this.clipQuads(this.renderData, count);
		this.clipQuads(this.textRenderData, 0);
		this.isControlInvalidated = false;
		this.updateCollider();
		return this.buffers;
	}

	private void renderSelection()
	{
		if (base.Atlas == null || this.selectedIndex < 0)
		{
			return;
		}
		dfAtlas.ItemInfo item = base.Atlas[this.ItemHighlight];
		if (item == null)
		{
			return;
		}
		float units = base.PixelsToUnits();
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector3 = new Vector3(upperLeft.x + (float)this.listPadding.left, upperLeft.y - (float)this.listPadding.top + this.scrollPosition, 0f)
		{
			y = vector3.y - (float)(this.selectedIndex * this.itemHeight)
		};
		Color32 color32 = base.ApplyOpacity(this.color);
		dfSprite.RenderOptions renderOption = new dfSprite.RenderOptions();
		dfSprite.RenderOptions vector31 = renderOption;
		vector31.atlas = this.atlas;
		vector31.color = color32;
		vector31.fillAmount = 1f;
		vector31.pixelsToUnits = units;
		vector31.size = new Vector3(this.size.x - (float)this.listPadding.horizontal, (float)this.itemHeight);
		vector31.spriteInfo = item;
		vector31.offset = vector3;
		renderOption = vector31;
		if (item.border.horizontal > 0 || item.border.vertical > 0)
		{
			dfSlicedSprite.renderSprite(this.renderData, renderOption);
		}
		else
		{
			dfSprite.renderSprite(this.renderData, renderOption);
		}
	}

	private void scrollbar_GotFocus(dfControl control, dfFocusEventArgs args)
	{
		base.Focus();
	}

	private void scrollbar_ValueChanged(dfControl control, float value)
	{
		this.ScrollPosition = value;
	}

	private void selectItemUnderMouse(dfMouseEventArgs args)
	{
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(base.Size);
		float single = upperLeft.y + ((float)(-this.itemHeight) * ((float)this.selectedIndex - this.scrollPosition) - (float)this.listPadding.top);
		float single1 = ((float)this.selectedIndex - this.scrollPosition + 1f) * (float)this.itemHeight + (float)this.listPadding.vertical;
		float single2 = single1 - this.size.y;
		if (single2 > 0f)
		{
			single = single + single2;
		}
		Vector2 hitPosition = base.GetHitPosition(args);
		float single3 = hitPosition.y - (float)this.listPadding.top;
		if (single3 < 0f || single3 > this.size.y - (float)this.listPadding.bottom)
		{
			return;
		}
		this.SelectedIndex = (int)((this.scrollPosition + single3) / (float)this.itemHeight);
		this.OnItemClicked();
	}

	private void synchronizeScrollbar()
	{
		if (this.scrollbar == null)
		{
			return;
		}
		int length = (int)this.items.Length * this.itemHeight;
		float single = this.size.y - (float)this.listPadding.vertical;
		this.scrollbar.IncrementAmount = (float)this.itemHeight;
		this.scrollbar.MinValue = 0f;
		this.scrollbar.MaxValue = (float)length;
		this.scrollbar.ScrollSize = single;
		this.scrollbar.Value = this.scrollPosition;
	}

	public override void Update()
	{
		base.Update();
		if (this.size.magnitude == 0f)
		{
			this.size = new Vector2(200f, 150f);
		}
		if (this.animateHover && this.hoverIndex != -1)
		{
			float units = (float)(this.hoverIndex * this.itemHeight) * base.PixelsToUnits();
			if (Mathf.Abs(this.hoverTweenLocation - units) < 1f)
			{
				this.Invalidate();
			}
		}
		if (this.isControlInvalidated)
		{
			this.synchronizeScrollbar();
		}
	}

	private void updateItemHover(dfMouseEventArgs args)
	{
		RaycastHit raycastHit;
		Vector2 vector2;
		if (!Application.isPlaying)
		{
			return;
		}
		Ray ray = args.Ray;
		if (!base.collider.Raycast(ray, out raycastHit, 1000f))
		{
			this.hoverIndex = -1;
			this.hoverTweenLocation = 0f;
			return;
		}
		base.GetHitPosition(ray, out vector2);
		Vector3 upperLeft = base.Pivot.TransformToUpperLeft(base.Size);
		float single = upperLeft.y + ((float)(-this.itemHeight) * ((float)this.selectedIndex - this.scrollPosition) - (float)this.listPadding.top);
		float single1 = ((float)this.selectedIndex - this.scrollPosition + 1f) * (float)this.itemHeight + (float)this.listPadding.vertical;
		float single2 = single1 - this.size.y;
		if (single2 > 0f)
		{
			single = single + single2;
		}
		float single3 = vector2.y - (float)this.listPadding.top;
		int num = (int)(this.scrollPosition + single3) / this.itemHeight;
		if (num != this.hoverIndex)
		{
			this.hoverIndex = num;
			this.Invalidate();
		}
	}

	public event PropertyChangedEventHandler<int> ItemClicked
	{
		add
		{
			this.ItemClicked += value;
		}
		remove
		{
			this.ItemClicked -= value;
		}
	}

	public event PropertyChangedEventHandler<int> SelectedIndexChanged
	{
		add
		{
			this.SelectedIndexChanged += value;
		}
		remove
		{
			this.SelectedIndexChanged -= value;
		}
	}
}