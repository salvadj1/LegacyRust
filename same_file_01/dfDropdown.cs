using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Dropdown List")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfDropdown : dfInteractiveBase, IDFMultiRender
{
	[SerializeField]
	protected dfFontBase font;

	[SerializeField]
	protected int selectedIndex = -1;

	[SerializeField]
	protected dfControl triggerButton;

	[SerializeField]
	protected Color32 textColor = UnityEngine.Color.white;

	[SerializeField]
	protected float textScale = 1f;

	[SerializeField]
	protected RectOffset textFieldPadding = new RectOffset();

	[SerializeField]
	protected dfDropdown.PopupListPosition listPosition;

	[SerializeField]
	protected int listWidth;

	[SerializeField]
	protected int listHeight = 200;

	[SerializeField]
	protected RectOffset listPadding = new RectOffset();

	[SerializeField]
	protected dfScrollbar listScrollbar;

	[SerializeField]
	protected int itemHeight = 25;

	[SerializeField]
	protected string itemHighlight = string.Empty;

	[SerializeField]
	protected string itemHover = string.Empty;

	[SerializeField]
	protected string listBackground = string.Empty;

	[SerializeField]
	protected Vector2 listOffset = Vector2.zero;

	[SerializeField]
	protected string[] items = new string[0];

	[SerializeField]
	protected bool shadow;

	[SerializeField]
	protected Color32 shadowColor = UnityEngine.Color.black;

	[SerializeField]
	protected Vector2 shadowOffset = new Vector2(1f, -1f);

	[SerializeField]
	protected bool openOnMouseDown;

	private bool eventsAttached;

	private dfListbox popup;

	private dfRenderData textRenderData;

	private dfList<dfRenderData> buffers = dfList<dfRenderData>.Obtain();

	private dfDropdown.PopupEventHandler DropdownOpen;

	private dfDropdown.PopupEventHandler DropdownClose;

	private PropertyChangedEventHandler<int> SelectedIndexChanged;

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
				this.closePopup(true);
				this.font = value;
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
			value = Mathf.Max(1, value);
			if (value != this.itemHeight)
			{
				this.closePopup(true);
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
				this.closePopup(true);
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
			this.closePopup(true);
			if (value == null)
			{
				value = new string[0];
			}
			this.items = value;
			this.Invalidate();
		}
	}

	public string ListBackground
	{
		get
		{
			return this.listBackground;
		}
		set
		{
			if (value != this.listBackground)
			{
				this.closePopup(true);
				this.listBackground = value;
				this.Invalidate();
			}
		}
	}

	public Vector2 ListOffset
	{
		get
		{
			return this.listOffset;
		}
		set
		{
			if (Vector2.Distance(this.listOffset, value) > 1f)
			{
				this.listOffset = value;
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

	public dfDropdown.PopupListPosition ListPosition
	{
		get
		{
			return this.listPosition;
		}
		set
		{
			if (value != this.ListPosition)
			{
				this.closePopup(true);
				this.listPosition = value;
				this.Invalidate();
			}
		}
	}

	public dfScrollbar ListScrollbar
	{
		get
		{
			return this.listScrollbar;
		}
		set
		{
			if (value != this.listScrollbar)
			{
				this.listScrollbar = value;
				this.Invalidate();
			}
		}
	}

	public int MaxListHeight
	{
		get
		{
			return this.listHeight;
		}
		set
		{
			this.listHeight = value;
			this.Invalidate();
		}
	}

	public int MaxListWidth
	{
		get
		{
			return this.listWidth;
		}
		set
		{
			this.listWidth = value;
		}
	}

	public bool OpenOnMouseDown
	{
		get
		{
			return this.openOnMouseDown;
		}
		set
		{
			this.openOnMouseDown = value;
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
				if (this.popup != null)
				{
					this.popup.SelectedIndex = value;
				}
				this.selectedIndex = value;
				this.OnSelectedIndexChanged();
				this.Invalidate();
			}
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

	public Color32 TextColor
	{
		get
		{
			return this.textColor;
		}
		set
		{
			this.closePopup(true);
			this.textColor = value;
			this.Invalidate();
		}
	}

	public RectOffset TextFieldPadding
	{
		get
		{
			if (this.textFieldPadding == null)
			{
				this.textFieldPadding = new RectOffset();
			}
			return this.textFieldPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.textFieldPadding))
			{
				this.textFieldPadding = value;
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
				this.closePopup(true);
				this.textScale = value;
				this.Invalidate();
			}
		}
	}

	public dfControl TriggerButton
	{
		get
		{
			return this.triggerButton;
		}
		set
		{
			if (value != this.triggerButton)
			{
				this.detachChildEvents();
				this.triggerButton = value;
				this.attachChildEvents();
				this.Invalidate();
			}
		}
	}

	public dfDropdown()
	{
	}

	public void AddItem(string item)
	{
		string[] strArrays = new string[(int)this.items.Length + 1];
		Array.Copy(this.items, strArrays, (int)this.items.Length);
		strArrays[(int)this.items.Length] = item;
		this.items = strArrays;
	}

	private void attachChildEvents()
	{
		if (this.triggerButton != null && !this.eventsAttached)
		{
			this.eventsAttached = true;
			this.triggerButton.Click += new MouseEventHandler(this.trigger_Click);
		}
	}

	private Vector3 calculatePopupPosition(int height)
	{
		float units = base.PixelsToUnits();
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector3 = base.transform.position + (upperLeft * units);
		Vector3 scaledDirection = base.getScaledDirection(Vector3.down);
		Vector3 vector31 = base.transformOffset(this.listOffset) * units;
		Vector3 vector32 = vector3 + vector31;
		Vector2 size = base.Size;
		Vector3 vector33 = vector32 + ((scaledDirection * size.y) * units);
		Vector3 vector34 = vector3 + vector31;
		Vector2 vector2 = this.popup.Size;
		Vector3 vector35 = vector34 - ((scaledDirection * vector2.y) * units);
		if (this.listPosition == dfDropdown.PopupListPosition.Above)
		{
			return vector35;
		}
		if (this.listPosition == dfDropdown.PopupListPosition.Below)
		{
			return vector33;
		}
		Vector3 upperLeft1 = (this.popup.transform.parent.position / units) + this.popup.Parent.Pivot.TransformToUpperLeft(base.Size);
		Vector2 size1 = this.parent.Size;
		Vector3 vector36 = upperLeft1 + (scaledDirection * size1.y);
		Vector3 vector37 = vector33 / units;
		Vector2 vector21 = this.popup.Size;
		Vector3 vector38 = vector37 + (scaledDirection * vector21.y);
		if (vector38.y < vector36.y)
		{
			return vector35;
		}
		if (base.GetCamera().WorldToScreenPoint(vector38 * units).y <= 0f)
		{
			return vector35;
		}
		return vector33;
	}

	private Vector2 calculatePopupSize()
	{
		float single = (this.MaxListWidth <= 0 ? this.size.x : (float)this.MaxListWidth);
		int length = (int)this.items.Length * this.itemHeight + this.listPadding.vertical;
		if ((int)this.items.Length == 0)
		{
			length = this.itemHeight / 2 + this.listPadding.vertical;
		}
		return new Vector2(single, (float)Mathf.Min(this.MaxListHeight, length));
	}

	private void checkForPopupClose()
	{
		RaycastHit raycastHit;
		if (this.popup == null || !Input.GetMouseButtonDown(0))
		{
			return;
		}
		Camera camera = base.GetCamera();
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		if (this.popup.collider.Raycast(ray, out raycastHit, camera.farClipPlane))
		{
			return;
		}
		if (this.popup.Scrollbar != null && this.popup.Scrollbar.collider.Raycast(ray, out raycastHit, camera.farClipPlane))
		{
			return;
		}
		this.closePopup(true);
	}

	private void closePopup(bool allowOverride = true)
	{
		if (this.popup == null)
		{
			return;
		}
		this.popup.LostFocus -= new FocusEventHandler(this.popup_LostFocus);
		this.popup.SelectedIndexChanged -= new PropertyChangedEventHandler<int>(this.popup_SelectedIndexChanged);
		this.popup.ItemClicked -= new PropertyChangedEventHandler<int>(this.popup_ItemClicked);
		this.popup.KeyDown -= new KeyPressHandler(this.popup_KeyDown);
		if (!allowOverride)
		{
			UnityEngine.Object.Destroy(this.popup.gameObject);
			this.popup = null;
			return;
		}
		bool flag = false;
		if (this.DropdownClose != null)
		{
			this.DropdownClose(this, this.popup, ref flag);
		}
		if (!flag)
		{
			flag = base.Signal("OnDropdownClose", new object[] { this, this.popup });
		}
		if (!flag)
		{
			UnityEngine.Object.Destroy(this.popup.gameObject);
		}
		this.popup = null;
	}

	private void detachChildEvents()
	{
		if (this.triggerButton != null && this.eventsAttached)
		{
			this.triggerButton.Click -= new MouseEventHandler(this.trigger_Click);
			this.eventsAttached = false;
		}
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		if (!Application.isPlaying)
		{
			return;
		}
		if (!this.eventsAttached)
		{
			this.attachChildEvents();
		}
		if (this.popup != null && !this.popup.ContainsFocus)
		{
			this.closePopup(true);
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		this.closePopup(false);
		this.detachChildEvents();
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.closePopup(false);
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

	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		KeyCode keyCode = args.KeyCode;
		switch (keyCode)
		{
			case KeyCode.UpArrow:
			{
				this.SelectedIndex = Mathf.Max(0, this.selectedIndex - 1);
				break;
			}
			case KeyCode.DownArrow:
			{
				this.SelectedIndex = Mathf.Min((int)this.items.Length - 1, this.selectedIndex + 1);
				break;
			}
			case KeyCode.Home:
			{
				this.SelectedIndex = 0;
				break;
			}
			case KeyCode.End:
			{
				this.SelectedIndex = (int)this.items.Length - 1;
				break;
			}
			default:
			{
				if (keyCode == KeyCode.Return || keyCode == KeyCode.Space)
				{
					this.openPopup();
					break;
				}
				else
				{
					break;
				}
			}
		}
		base.OnKeyDown(args);
	}

	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		if (!this.openOnMouseDown || args.Used || args.Buttons != dfMouseButtons.Left || !(args.Source == this))
		{
			base.OnMouseDown(args);
		}
		else
		{
			args.Use();
			base.OnMouseDown(args);
			if (this.popup == null)
			{
				this.openPopup();
			}
			else
			{
				this.closePopup(true);
			}
		}
	}

	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		this.SelectedIndex = Mathf.Max(0, this.SelectedIndex - Mathf.RoundToInt(args.WheelDelta));
		args.Use();
		base.OnMouseWheel(args);
	}

	protected internal virtual void OnSelectedIndexChanged()
	{
		base.SignalHierarchy("OnSelectedIndexChanged", new object[] { this.selectedIndex });
		if (this.SelectedIndexChanged != null)
		{
			this.SelectedIndexChanged(this, this.selectedIndex);
		}
	}

	private void openPopup()
	{
		if (this.popup != null || (int)this.items.Length == 0)
		{
			return;
		}
		Vector2 vector2 = this.calculatePopupSize();
		this.popup = base.GetManager().AddControl<dfListbox>();
		this.popup.name = string.Concat(base.name, " - Dropdown List");
		this.popup.gameObject.hideFlags = HideFlags.DontSave;
		this.popup.Atlas = base.Atlas;
		this.popup.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Left;
		this.popup.Font = this.Font;
		this.popup.Pivot = dfPivotPoint.TopLeft;
		this.popup.Size = vector2;
		this.popup.Font = this.Font;
		this.popup.ItemHeight = this.ItemHeight;
		this.popup.ItemHighlight = this.ItemHighlight;
		this.popup.ItemHover = this.ItemHover;
		this.popup.ItemPadding = this.TextFieldPadding;
		this.popup.ItemTextColor = this.TextColor;
		this.popup.ItemTextScale = this.TextScale;
		this.popup.Items = this.Items;
		this.popup.ListPadding = this.ListPadding;
		this.popup.BackgroundSprite = this.ListBackground;
		this.popup.Shadow = this.Shadow;
		this.popup.ShadowColor = this.ShadowColor;
		this.popup.ShadowOffset = this.ShadowOffset;
		this.popup.ZOrder = 2147483647;
		if (vector2.y >= (float)this.MaxListHeight && this.listScrollbar != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.listScrollbar.gameObject) as GameObject;
			dfScrollbar component = gameObject.GetComponent<dfScrollbar>();
			float units = base.PixelsToUnits();
			Vector3 vector3 = this.popup.transform.TransformDirection(Vector3.right);
			Vector3 width = this.popup.transform.position + ((vector3 * (vector2.x - component.Width)) * units);
			component.transform.parent = this.popup.transform;
			component.transform.position = width;
			component.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Bottom;
			component.Height = this.popup.Height;
			dfListbox _dfListbox = this.popup;
			_dfListbox.Width = _dfListbox.Width - component.Width;
			this.popup.Scrollbar = component;
			this.popup.SizeChanged += new PropertyChangedEventHandler<Vector2>((dfControl control, Vector2 size) => component.Height = control.Height);
		}
		Vector2 vector21 = this.popup.Size;
		Vector3 vector31 = this.calculatePopupPosition((int)vector21.y);
		this.popup.transform.position = vector31;
		this.popup.transform.rotation = base.transform.rotation;
		this.popup.SelectedIndexChanged += new PropertyChangedEventHandler<int>(this.popup_SelectedIndexChanged);
		this.popup.LostFocus += new FocusEventHandler(this.popup_LostFocus);
		this.popup.ItemClicked += new PropertyChangedEventHandler<int>(this.popup_ItemClicked);
		this.popup.KeyDown += new KeyPressHandler(this.popup_KeyDown);
		this.popup.SelectedIndex = Mathf.Max(0, this.SelectedIndex);
		this.popup.EnsureVisible(this.popup.SelectedIndex);
		this.popup.Focus();
		if (this.DropdownOpen != null)
		{
			bool flag = false;
			this.DropdownOpen(this, this.popup, ref flag);
		}
		base.Signal("OnDropdownOpen", new object[] { this, this.popup });
	}

	private void popup_ItemClicked(dfControl control, int selectedIndex)
	{
		this.closePopup(true);
		base.Focus();
	}

	private void popup_KeyDown(dfControl control, dfKeyEventArgs args)
	{
		if (args.KeyCode == KeyCode.Escape || args.KeyCode == KeyCode.Return)
		{
			this.closePopup(true);
			base.Focus();
		}
	}

	private void popup_LostFocus(dfControl control, dfFocusEventArgs args)
	{
		if (this.popup != null && !this.popup.ContainsFocus)
		{
			this.closePopup(true);
		}
	}

	private void popup_SelectedIndexChanged(dfControl control, int selectedIndex)
	{
		this.SelectedIndex = selectedIndex;
		this.Invalidate();
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
		this.renderText(this.textRenderData);
		this.isControlInvalidated = false;
		this.updateCollider();
		return this.buffers;
	}

	private void renderText(dfRenderData buffer)
	{
		if (this.selectedIndex < 0 || this.selectedIndex >= (int)this.items.Length)
		{
			return;
		}
		string str = this.items[this.selectedIndex];
		float units = base.PixelsToUnits();
		Vector2 vector2 = new Vector2(this.size.x - (float)this.textFieldPadding.horizontal, this.size.y - (float)this.textFieldPadding.vertical);
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector3 = new Vector3(upperLeft.x + (float)this.textFieldPadding.left, upperLeft.y - (float)this.textFieldPadding.top, 0f) * units;
		Color32 color32 = (!base.IsEnabled ? base.DisabledColor : this.TextColor);
		using (dfFontRendererBase textScale = this.font.ObtainRenderer())
		{
			textScale.WordWrap = false;
			textScale.MaxSize = vector2;
			textScale.PixelRatio = units;
			textScale.TextScale = this.TextScale;
			textScale.VectorOffset = vector3;
			textScale.MultiLine = false;
			textScale.TextAlign = TextAlignment.Left;
			textScale.ProcessMarkup = true;
			textScale.DefaultColor = color32;
			textScale.OverrideMarkupColors = false;
			textScale.Opacity = base.CalculateOpacity();
			textScale.Shadow = this.Shadow;
			textScale.ShadowColor = this.ShadowColor;
			textScale.ShadowOffset = this.ShadowOffset;
			dfDynamicFont.DynamicFontRenderer atlas = textScale as dfDynamicFont.DynamicFontRenderer;
			if (atlas != null)
			{
				atlas.SpriteAtlas = base.Atlas;
				atlas.SpriteBuffer = buffer;
			}
			textScale.Render(str, buffer);
		}
	}

	private void trigger_Click(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (mouseEvent.Source == this.triggerButton && !mouseEvent.Used)
		{
			mouseEvent.Use();
			if (this.popup != null)
			{
				Debug.Log("Close popup");
				this.closePopup(true);
			}
			else
			{
				this.openPopup();
			}
		}
	}

	public override void Update()
	{
		base.Update();
		this.checkForPopupClose();
	}

	public event dfDropdown.PopupEventHandler DropdownClose
	{
		add
		{
			this.DropdownClose += value;
		}
		remove
		{
			this.DropdownClose -= value;
		}
	}

	public event dfDropdown.PopupEventHandler DropdownOpen
	{
		add
		{
			this.DropdownOpen += value;
		}
		remove
		{
			this.DropdownOpen -= value;
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

	[dfEventCategory("Popup")]
	public delegate void PopupEventHandler(dfDropdown dropdown, dfListbox popup, ref bool overridden);

	public enum PopupListPosition
	{
		Below,
		Above,
		Automatic
	}
}