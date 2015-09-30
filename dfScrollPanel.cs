using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Containers/Scrollable Panel")]
[ExecuteInEditMode]
[Serializable]
public class dfScrollPanel : dfControl
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected string backgroundSprite;

	[SerializeField]
	protected Color32 backgroundColor = UnityEngine.Color.white;

	[SerializeField]
	protected bool autoReset = true;

	[SerializeField]
	protected bool autoLayout;

	[SerializeField]
	protected RectOffset scrollPadding = new RectOffset();

	[SerializeField]
	protected RectOffset flowPadding = new RectOffset();

	[SerializeField]
	protected dfScrollPanel.LayoutDirection flowDirection;

	[SerializeField]
	protected bool wrapLayout;

	[SerializeField]
	protected Vector2 scrollPosition = Vector2.zero;

	[SerializeField]
	protected int scrollWheelAmount = 10;

	[SerializeField]
	protected dfScrollbar horzScroll;

	[SerializeField]
	protected dfScrollbar vertScroll;

	[SerializeField]
	protected dfControlOrientation wheelDirection;

	[SerializeField]
	protected bool scrollWithArrowKeys;

	[SerializeField]
	protected bool useScrollMomentum;

	private bool initialized;

	private bool resetNeeded;

	private bool scrolling;

	private bool isMouseDown;

	private Vector2 touchStartPosition = Vector2.zero;

	private Vector2 scrollMomentum = Vector2.zero;

	private PropertyChangedEventHandler<Vector2> ScrollPositionChanged;

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

	public bool AutoLayout
	{
		get
		{
			return this.autoLayout;
		}
		set
		{
			if (value != this.autoLayout)
			{
				this.autoLayout = value;
				this.Reset();
			}
		}
	}

	public bool AutoReset
	{
		get
		{
			return this.autoReset;
		}
		set
		{
			if (value != this.autoReset)
			{
				this.autoReset = value;
				if (value)
				{
					this.Reset();
				}
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

	public override bool CanFocus
	{
		get
		{
			if (base.IsEnabled && base.IsVisible)
			{
				return true;
			}
			return base.CanFocus;
		}
	}

	public dfScrollPanel.LayoutDirection FlowDirection
	{
		get
		{
			return this.flowDirection;
		}
		set
		{
			if (value != this.flowDirection)
			{
				this.flowDirection = value;
				this.Reset();
			}
		}
	}

	public RectOffset FlowPadding
	{
		get
		{
			if (this.flowPadding == null)
			{
				this.flowPadding = new RectOffset();
			}
			return this.flowPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.flowPadding))
			{
				this.flowPadding = value;
				this.Reset();
			}
		}
	}

	public dfScrollbar HorzScrollbar
	{
		get
		{
			return this.horzScroll;
		}
		set
		{
			this.horzScroll = value;
			this.updateScrollbars();
		}
	}

	public RectOffset ScrollPadding
	{
		get
		{
			if (this.scrollPadding == null)
			{
				this.scrollPadding = new RectOffset();
			}
			return this.scrollPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.scrollPadding))
			{
				this.scrollPadding = value;
				this.Reset();
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
			Vector2 vector2 = this.calculateViewSize();
			Vector2 vector21 = new Vector2(this.size.x - (float)this.scrollPadding.horizontal, this.size.y - (float)this.scrollPadding.vertical);
			value = Vector2.Min(vector2 - vector21, value);
			value = Vector2.Max(Vector2.zero, value);
			value = value.RoundToInt();
			if ((value - this.scrollPosition).sqrMagnitude > 1.401298E-45f)
			{
				Vector2 vector22 = value - this.scrollPosition;
				this.scrollPosition = value;
				this.scrollChildControls(vector22);
				this.updateScrollbars();
			}
			this.OnScrollPositionChanged();
		}
	}

	public int ScrollWheelAmount
	{
		get
		{
			return this.scrollWheelAmount;
		}
		set
		{
			this.scrollWheelAmount = value;
		}
	}

	public bool ScrollWithArrowKeys
	{
		get
		{
			return this.scrollWithArrowKeys;
		}
		set
		{
			this.scrollWithArrowKeys = value;
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

	public dfScrollbar VertScrollbar
	{
		get
		{
			return this.vertScroll;
		}
		set
		{
			this.vertScroll = value;
			this.updateScrollbars();
		}
	}

	public dfControlOrientation WheelScrollDirection
	{
		get
		{
			return this.wheelDirection;
		}
		set
		{
			this.wheelDirection = value;
		}
	}

	public bool WrapLayout
	{
		get
		{
			return this.wrapLayout;
		}
		set
		{
			if (value != this.wrapLayout)
			{
				this.wrapLayout = value;
				this.Reset();
			}
		}
	}

	public dfScrollPanel()
	{
	}

	private void attachEvents(dfControl control)
	{
		control.IsVisibleChanged += new PropertyChangedEventHandler<bool>(this.childIsVisibleChanged);
		control.PositionChanged += new PropertyChangedEventHandler<Vector2>(this.childControlInvalidated);
		control.SizeChanged += new PropertyChangedEventHandler<Vector2>(this.childControlInvalidated);
		control.ZOrderChanged += new PropertyChangedEventHandler<int>(this.childOrderChanged);
	}

	[HideInInspector]
	private void AutoArrange()
	{
		this.SuspendLayout();
		try
		{
			this.scrollPadding = this.ScrollPadding.ConstrainPadding();
			this.flowPadding = this.FlowPadding.ConstrainPadding();
			float single = (float)this.scrollPadding.left + (float)this.flowPadding.left - this.scrollPosition.x;
			float single1 = (float)this.scrollPadding.top + (float)this.flowPadding.top - this.scrollPosition.y;
			float single2 = 0f;
			float single3 = 0f;
			for (int i = 0; i < this.controls.Count; i++)
			{
				dfControl item = this.controls[i];
				if (item.IsVisible && item.enabled && item.gameObject.activeSelf)
				{
					if (!(item == this.horzScroll) && !(item == this.vertScroll))
					{
						if (this.wrapLayout)
						{
							if (this.flowDirection == dfScrollPanel.LayoutDirection.Horizontal)
							{
								if (single + item.Width >= this.size.x - (float)this.scrollPadding.right)
								{
									single = (float)this.scrollPadding.left + (float)this.flowPadding.left;
									single1 = single1 + single3;
									single3 = 0f;
								}
							}
							else if (single1 + item.Height + (float)this.flowPadding.vertical >= this.size.y - (float)this.scrollPadding.bottom)
							{
								single1 = (float)this.scrollPadding.top + (float)this.flowPadding.top;
								single = single + single2;
								single2 = 0f;
							}
						}
						item.RelativePosition = new Vector2(single, single1);
						float width = item.Width + (float)this.flowPadding.horizontal;
						float height = item.Height + (float)this.flowPadding.vertical;
						single2 = Mathf.Max(width, single2);
						single3 = Mathf.Max(height, single3);
						if (this.flowDirection != dfScrollPanel.LayoutDirection.Horizontal)
						{
							single1 = single1 + height;
						}
						else
						{
							single = single + width;
						}
					}
				}
			}
			this.updateScrollbars();
		}
		finally
		{
			this.ResumeLayout();
		}
	}

	private Vector2 calculateMinChildPosition()
	{
		float single = Single.MaxValue;
		float single1 = Single.MaxValue;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl item = this.controls[i];
			if (item.enabled && item.gameObject.activeSelf)
			{
				Vector3 num = item.RelativePosition.FloorToInt();
				single = Mathf.Min(single, num.x);
				single1 = Mathf.Min(single1, num.y);
			}
		}
		return new Vector2(single, single1);
	}

	private Vector2 calculateViewSize()
	{
		Vector2 num = (new Vector2((float)this.scrollPadding.horizontal, (float)this.scrollPadding.vertical)).RoundToInt();
		Vector2 vector2 = base.Size.RoundToInt() - num;
		if (!base.IsVisible || this.controls.Count == 0)
		{
			return vector2;
		}
		Vector2 vector21 = Vector2.one * Single.MaxValue;
		Vector2 vector22 = Vector2.one * Single.MinValue;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl item = this.controls[i];
			if (!Application.isPlaying || item.IsVisible)
			{
				Vector2 num1 = item.RelativePosition.RoundToInt();
				Vector2 num2 = num1 + item.Size.RoundToInt();
				vector21 = Vector2.Min(num1, vector21);
				vector22 = Vector2.Max(num2, vector22);
			}
		}
		vector22 = Vector2.Max(vector22, vector2);
		return vector22 - vector21;
	}

	public void CenterChildControls()
	{
		if (this.controls.Count == 0)
		{
			return;
		}
		Vector2 vector2 = Vector2.one * Single.MaxValue;
		Vector2 vector21 = Vector2.one * Single.MinValue;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl item = this.controls[i];
			Vector2 relativePosition = item.RelativePosition;
			Vector2 size = relativePosition + item.Size;
			vector2 = Vector2.Min(vector2, relativePosition);
			vector21 = Vector2.Max(vector21, size);
		}
		Vector2 vector22 = vector21 - vector2;
		Vector2 size1 = (base.Size - vector22) * 0.5f;
		for (int j = 0; j < this.controls.Count; j++)
		{
			dfControl _dfControl = this.controls[j];
			_dfControl.RelativePosition = (_dfControl.RelativePosition - vector2) + size1;
		}
	}

	private void childControlInvalidated(dfControl control, Vector2 value)
	{
		this.onChildControlInvalidatedLayout();
	}

	private void childIsVisibleChanged(dfControl control, bool value)
	{
		this.onChildControlInvalidatedLayout();
	}

	private void childOrderChanged(dfControl control, int value)
	{
		this.onChildControlInvalidatedLayout();
	}

	private void detachEvents(dfControl control)
	{
		control.IsVisibleChanged -= new PropertyChangedEventHandler<bool>(this.childIsVisibleChanged);
		control.PositionChanged -= new PropertyChangedEventHandler<Vector2>(this.childControlInvalidated);
		control.SizeChanged -= new PropertyChangedEventHandler<Vector2>(this.childControlInvalidated);
		control.ZOrderChanged -= new PropertyChangedEventHandler<int>(this.childOrderChanged);
	}

	public void FitToContents()
	{
		if (this.controls.Count == 0)
		{
			return;
		}
		Vector2 vector2 = Vector2.zero;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl item = this.controls[i];
			Vector2 relativePosition = item.RelativePosition + item.Size;
			vector2 = Vector2.Max(vector2, relativePosition);
		}
		base.Size = vector2 + new Vector2((float)this.scrollPadding.right, (float)this.scrollPadding.bottom);
	}

	protected internal override Plane[] GetClippingPlanes()
	{
		if (!base.ClipChildren)
		{
			return null;
		}
		Vector3[] corners = base.GetCorners();
		Vector3 vector3 = base.transform.TransformDirection(Vector3.right);
		Vector3 vector31 = base.transform.TransformDirection(Vector3.left);
		Vector3 vector32 = base.transform.TransformDirection(Vector3.up);
		Vector3 vector33 = base.transform.TransformDirection(Vector3.down);
		float units = base.PixelsToUnits();
		RectOffset scrollPadding = this.ScrollPadding;
		corners[0] = corners[0] + ((vector3 * (float)scrollPadding.left) * units) + ((vector33 * (float)scrollPadding.top) * units);
		corners[1] = corners[1] + ((vector31 * (float)scrollPadding.right) * units) + ((vector33 * (float)scrollPadding.top) * units);
		corners[2] = corners[2] + ((vector3 * (float)scrollPadding.left) * units) + ((vector32 * (float)scrollPadding.bottom) * units);
		return new Plane[] { new Plane(vector3, corners[0]), new Plane(vector31, corners[1]), new Plane(vector32, corners[2]), new Plane(vector33, corners[0]) };
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
			if (this.horzScroll != null)
			{
				this.horzScroll.ValueChanged += new PropertyChangedEventHandler<float>(this.horzScroll_ValueChanged);
			}
			if (this.vertScroll != null)
			{
				this.vertScroll.ValueChanged += new PropertyChangedEventHandler<float>(this.vertScroll_ValueChanged);
			}
		}
		if (this.resetNeeded || this.autoLayout || this.autoReset)
		{
			this.Reset();
		}
		this.Invalidate();
		this.ScrollPosition = Vector2.zero;
		this.updateScrollbars();
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		this.initialize();
		if (this.resetNeeded && base.IsVisible)
		{
			this.resetNeeded = false;
			if (this.autoReset || this.autoLayout)
			{
				this.Reset();
			}
		}
	}

	[HideInInspector]
	private void onChildControlInvalidatedLayout()
	{
		if (this.scrolling || base.IsLayoutSuspended)
		{
			return;
		}
		if (this.autoLayout)
		{
			this.AutoArrange();
		}
		this.updateScrollbars();
		this.Invalidate();
	}

	protected internal override void OnControlAdded(dfControl child)
	{
		base.OnControlAdded(child);
		this.attachEvents(child);
		if (this.autoLayout)
		{
			this.AutoArrange();
		}
	}

	protected internal override void OnControlRemoved(dfControl child)
	{
		base.OnControlRemoved(child);
		if (child != null)
		{
			this.detachEvents(child);
		}
		if (!this.autoLayout)
		{
			this.updateScrollbars();
		}
		else
		{
			this.AutoArrange();
		}
	}

	public override void OnDestroy()
	{
		if (this.horzScroll != null)
		{
			this.horzScroll.ValueChanged -= new PropertyChangedEventHandler<float>(this.horzScroll_ValueChanged);
		}
		if (this.vertScroll != null)
		{
			this.vertScroll.ValueChanged -= new PropertyChangedEventHandler<float>(this.vertScroll_ValueChanged);
		}
		this.horzScroll = null;
		this.vertScroll = null;
	}

	internal override void OnDragStart(dfDragEventArgs args)
	{
		base.OnDragStart(args);
		if (args.Used)
		{
			this.isMouseDown = false;
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		if (this.size == Vector2.zero)
		{
			this.SuspendLayout();
			Camera camera = base.GetCamera();
			base.Size = new Vector3(camera.pixelWidth / 2f, camera.pixelHeight / 2f);
			this.ResumeLayout();
		}
		if (this.autoLayout)
		{
			this.AutoArrange();
		}
		this.updateScrollbars();
	}

	protected internal override void OnGotFocus(dfFocusEventArgs args)
	{
		if (args.Source != this)
		{
			this.ScrollIntoView(args.Source);
		}
		base.OnGotFocus(args);
	}

	protected internal override void OnIsVisibleChanged()
	{
		base.OnIsVisibleChanged();
		if (base.IsVisible && (this.autoReset || this.autoLayout))
		{
			this.Reset();
			this.updateScrollbars();
		}
	}

	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (!this.scrollWithArrowKeys || args.Used)
		{
			base.OnKeyDown(args);
			return;
		}
		float single = (this.horzScroll == null ? 1f : this.horzScroll.IncrementAmount);
		float single1 = (this.vertScroll == null ? 1f : this.vertScroll.IncrementAmount);
		if (args.KeyCode == KeyCode.LeftArrow)
		{
			dfScrollPanel scrollPosition = this;
			scrollPosition.ScrollPosition = scrollPosition.ScrollPosition + new Vector2(-single, 0f);
			args.Use();
		}
		else if (args.KeyCode == KeyCode.RightArrow)
		{
			dfScrollPanel _dfScrollPanel = this;
			_dfScrollPanel.ScrollPosition = _dfScrollPanel.ScrollPosition + new Vector2(single, 0f);
			args.Use();
		}
		else if (args.KeyCode == KeyCode.UpArrow)
		{
			dfScrollPanel scrollPosition1 = this;
			scrollPosition1.ScrollPosition = scrollPosition1.ScrollPosition + new Vector2(0f, -single1);
			args.Use();
		}
		else if (args.KeyCode == KeyCode.DownArrow)
		{
			dfScrollPanel _dfScrollPanel1 = this;
			_dfScrollPanel1.ScrollPosition = _dfScrollPanel1.ScrollPosition + new Vector2(0f, single1);
			args.Use();
		}
		base.OnKeyDown(args);
	}

	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		base.OnMouseDown(args);
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
		if ((args is dfTouchEventArgs || this.isMouseDown) && !args.Used && (args.Position - this.touchStartPosition).magnitude > 5f)
		{
			Vector2 vector2 = args.MoveDelta.Scale(-1f, 1f);
			dfScrollPanel scrollPosition = this;
			scrollPosition.ScrollPosition = scrollPosition.ScrollPosition + vector2;
			this.scrollMomentum = (this.scrollMomentum + vector2) * 0.5f;
			args.Use();
		}
		base.OnMouseMove(args);
	}

	protected internal override void OnMouseUp(dfMouseEventArgs args)
	{
		base.OnMouseUp(args);
		this.isMouseDown = false;
	}

	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		float single;
		try
		{
			if (!args.Used)
			{
				if (this.wheelDirection != dfControlOrientation.Horizontal)
				{
					single = (this.vertScroll == null ? (float)this.scrollWheelAmount : this.vertScroll.IncrementAmount);
				}
				else
				{
					single = (this.horzScroll == null ? (float)this.scrollWheelAmount : this.horzScroll.IncrementAmount);
				}
				float single1 = single;
				if (this.wheelDirection != dfControlOrientation.Horizontal)
				{
					this.ScrollPosition = new Vector2(this.scrollPosition.x, this.scrollPosition.y - single1 * args.WheelDelta);
					this.scrollMomentum = new Vector2(0f, -single1 * args.WheelDelta);
				}
				else
				{
					this.ScrollPosition = new Vector2(this.scrollPosition.x - single1 * args.WheelDelta, this.scrollPosition.y);
					this.scrollMomentum = new Vector2(-single1 * args.WheelDelta, 0f);
				}
				args.Use();
				base.Signal("OnMouseWheel", new object[] { args });
			}
		}
		finally
		{
			base.OnMouseWheel(args);
		}
	}

	protected override void OnRebuildRenderData()
	{
		if (this.Atlas == null || string.IsNullOrEmpty(this.backgroundSprite))
		{
			return;
		}
		dfAtlas.ItemInfo item = this.Atlas[this.backgroundSprite];
		if (item == null)
		{
			return;
		}
		this.renderData.Material = this.Atlas.Material;
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

	protected internal override void OnResolutionChanged(Vector2 previousResolution, Vector2 currentResolution)
	{
		base.OnResolutionChanged(previousResolution, currentResolution);
		this.resetNeeded = true;
	}

	protected internal void OnScrollPositionChanged()
	{
		this.Invalidate();
		base.SignalHierarchy("OnScrollPositionChanged", new object[] { this.ScrollPosition });
		if (this.ScrollPositionChanged != null)
		{
			this.ScrollPositionChanged(this, this.ScrollPosition);
		}
	}

	protected internal override void OnSizeChanged()
	{
		base.OnSizeChanged();
		if (this.autoReset || this.autoLayout)
		{
			this.Reset();
			return;
		}
		Vector2 vector2 = this.calculateMinChildPosition();
		if (vector2.x > (float)this.scrollPadding.left || vector2.y > (float)this.scrollPadding.top)
		{
			vector2 = vector2 - new Vector2((float)this.scrollPadding.left, (float)this.scrollPadding.top);
			vector2 = Vector2.Max(vector2, Vector2.zero);
			this.scrollChildControls(vector2);
		}
		this.updateScrollbars();
	}

	public void Reset()
	{
		try
		{
			this.SuspendLayout();
			if (!this.autoLayout)
			{
				this.scrollPadding = this.ScrollPadding.ConstrainPadding();
				Vector3 vector3 = this.calculateMinChildPosition();
				vector3 = vector3 - new Vector3((float)this.scrollPadding.left, (float)this.scrollPadding.top);
				for (int i = 0; i < this.controls.Count; i++)
				{
					dfControl item = this.controls[i];
					item.RelativePosition = item.RelativePosition - vector3;
				}
				this.scrollPosition = Vector2.zero;
			}
			else
			{
				Vector2 scrollPosition = this.ScrollPosition;
				this.ScrollPosition = Vector2.zero;
				this.AutoArrange();
				this.ScrollPosition = scrollPosition;
			}
			this.Invalidate();
			this.updateScrollbars();
		}
		finally
		{
			this.ResumeLayout();
		}
	}

	private void scrollChildControls(Vector3 delta)
	{
		try
		{
			this.scrolling = true;
			delta = delta.Scale(1f, -1f, 1f);
			for (int i = 0; i < this.controls.Count; i++)
			{
				dfControl item = this.controls[i];
				item.Position = (item.Position - delta).RoundToInt();
			}
		}
		finally
		{
			this.scrolling = false;
		}
	}

	public void ScrollIntoView(dfControl control)
	{
		Rect num = (new Rect(this.scrollPosition.x + (float)this.scrollPadding.left, this.scrollPosition.y + (float)this.scrollPadding.top, this.size.x - (float)this.scrollPadding.horizontal, this.size.y - (float)this.scrollPadding.vertical)).RoundToInt();
		Vector3 relativePosition = control.RelativePosition;
		Vector2 size = control.Size;
		while (!this.controls.Contains(control))
		{
			control = control.Parent;
			relativePosition = relativePosition + control.RelativePosition;
		}
		Rect rect = (new Rect(this.scrollPosition.x + relativePosition.x, this.scrollPosition.y + relativePosition.y, size.x, size.y)).RoundToInt();
		if (num.Contains(rect))
		{
			return;
		}
		Vector2 vector2 = this.scrollPosition;
		if (rect.xMin < num.xMin)
		{
			vector2.x = rect.xMin - (float)this.scrollPadding.left;
		}
		else if (rect.xMax > num.xMax)
		{
			vector2.x = rect.xMax - Mathf.Max(this.size.x, size.x) + (float)this.scrollPadding.horizontal;
		}
		if (rect.y < num.y)
		{
			vector2.y = rect.yMin - (float)this.scrollPadding.top;
		}
		else if (rect.yMax > num.yMax)
		{
			vector2.y = rect.yMax - Mathf.Max(this.size.y, size.y) + (float)this.scrollPadding.vertical;
		}
		this.ScrollPosition = vector2;
		this.scrollMomentum = Vector2.zero;
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
		if (this.useScrollMomentum && !this.isMouseDown && this.scrollMomentum.sqrMagnitude > 1.401298E-45f)
		{
			dfScrollPanel scrollPosition = this;
			scrollPosition.ScrollPosition = scrollPosition.ScrollPosition + this.scrollMomentum;
		}
		if (this.isControlInvalidated && this.autoLayout && base.IsVisible)
		{
			this.AutoArrange();
			this.updateScrollbars();
		}
		dfScrollPanel _dfScrollPanel = this;
		_dfScrollPanel.scrollMomentum = _dfScrollPanel.scrollMomentum * (0.95f - Time.deltaTime);
	}

	[HideInInspector]
	private void updateScrollbars()
	{
		Vector2 vector2 = this.calculateViewSize();
		Vector2 size = base.Size - new Vector2((float)this.scrollPadding.horizontal, (float)this.scrollPadding.vertical);
		if (this.horzScroll != null)
		{
			this.horzScroll.MinValue = 0f;
			this.horzScroll.MaxValue = vector2.x;
			this.horzScroll.ScrollSize = size.x;
			this.horzScroll.Value = Mathf.Max(0f, this.scrollPosition.x);
		}
		if (this.vertScroll != null)
		{
			this.vertScroll.MinValue = 0f;
			this.vertScroll.MaxValue = vector2.y;
			this.vertScroll.ScrollSize = size.y;
			this.vertScroll.Value = Mathf.Max(0f, this.scrollPosition.y);
		}
	}

	private void vertScroll_ValueChanged(dfControl control, float value)
	{
		this.ScrollPosition = new Vector2(this.scrollPosition.x, value);
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

	public enum LayoutDirection
	{
		Horizontal,
		Vertical
	}
}