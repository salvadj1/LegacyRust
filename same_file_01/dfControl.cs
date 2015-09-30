using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public abstract class dfControl : MonoBehaviour, IComparable<dfControl>
{
	private const float MINIMUM_OPACITY = 0.0125f;

	private static uint versionCounter;

	[SerializeField]
	protected bool isEnabled = true;

	[SerializeField]
	protected bool isVisible = true;

	[SerializeField]
	protected bool isInteractive = true;

	[SerializeField]
	protected string tooltip;

	[SerializeField]
	protected dfPivotPoint pivot;

	[SerializeField]
	protected int zindex = -1;

	[SerializeField]
	protected Color32 color = new Color32(255, 255, 255, 255);

	[SerializeField]
	protected Color32 disabledColor = new Color32(255, 255, 255, 255);

	[SerializeField]
	protected Vector2 size = Vector2.zero;

	[SerializeField]
	protected Vector2 minSize = Vector2.zero;

	[SerializeField]
	protected Vector2 maxSize = Vector2.zero;

	[SerializeField]
	protected bool clipChildren;

	[SerializeField]
	protected int tabIndex = -1;

	[SerializeField]
	protected bool canFocus;

	[SerializeField]
	protected dfControl.AnchorLayout layout;

	[SerializeField]
	protected int renderOrder = -1;

	[SerializeField]
	protected bool isLocalized;

	[SerializeField]
	protected Vector2 hotZoneScale = Vector2.one;

	protected bool isControlInvalidated = true;

	protected dfControl parent;

	protected dfList<dfControl> controls = dfList<dfControl>.Obtain();

	protected dfGUIManager manager;

	protected dfLanguageManager languageManager;

	protected bool languageManagerChecked;

	protected int cachedChildCount;

	protected Vector3 cachedPosition = Vector3.one * Single.MinValue;

	protected Quaternion cachedRotation = Quaternion.identity;

	protected Vector3 cachedScale = Vector3.one;

	protected float cachedPixelSize;

	protected dfRenderData renderData;

	protected bool isMouseHovering;

	private object tag;

	protected bool isDisposing;

	private bool performingLayout;

	private Vector3[] cachedCorners = new Vector3[4];

	private Plane[] cachedClippingPlanes = new Plane[4];

	private uint version;

	private bool rendering;

	private ChildControlEventHandler ControlAdded;

	private ChildControlEventHandler ControlRemoved;

	private FocusEventHandler GotFocus;

	private FocusEventHandler EnterFocus;

	private FocusEventHandler LostFocus;

	private FocusEventHandler LeaveFocus;

	private PropertyChangedEventHandler<int> TabIndexChanged;

	private PropertyChangedEventHandler<Vector2> PositionChanged;

	private PropertyChangedEventHandler<Vector2> SizeChanged;

	private PropertyChangedEventHandler<Color32> ColorChanged;

	private PropertyChangedEventHandler<bool> IsVisibleChanged;

	private PropertyChangedEventHandler<bool> IsEnabledChanged;

	private PropertyChangedEventHandler<float> OpacityChanged;

	private PropertyChangedEventHandler<dfAnchorStyle> AnchorChanged;

	private PropertyChangedEventHandler<dfPivotPoint> PivotChanged;

	private PropertyChangedEventHandler<int> ZOrderChanged;

	private DragEventHandler DragStart;

	private DragEventHandler DragEnd;

	private DragEventHandler DragDrop;

	private DragEventHandler DragEnter;

	private DragEventHandler DragLeave;

	private DragEventHandler DragOver;

	private KeyPressHandler KeyPress;

	private KeyPressHandler KeyDown;

	private KeyPressHandler KeyUp;

	private ControlMultiTouchEventHandler MultiTouch;

	private MouseEventHandler MouseEnter;

	private MouseEventHandler MouseMove;

	private MouseEventHandler MouseHover;

	private MouseEventHandler MouseLeave;

	private MouseEventHandler MouseDown;

	private MouseEventHandler MouseUp;

	private MouseEventHandler MouseWheel;

	private MouseEventHandler Click;

	private MouseEventHandler DoubleClick;

	[SerializeField]
	public dfAnchorStyle Anchor
	{
		get
		{
			this.ensureLayoutExists();
			return this.layout.AnchorStyle;
		}
		set
		{
			this.ensureLayoutExists();
			if (value != this.layout.AnchorStyle)
			{
				this.layout.AnchorStyle = value;
				this.Invalidate();
				this.OnAnchorChanged();
			}
		}
	}

	public virtual bool CanFocus
	{
		get
		{
			return (!this.canFocus ? false : this.IsInteractive);
		}
		set
		{
			this.canFocus = value;
		}
	}

	public bool ClipChildren
	{
		get
		{
			return this.clipChildren;
		}
		set
		{
			if (value != this.clipChildren)
			{
				this.clipChildren = value;
				this.Invalidate();
			}
		}
	}

	public Color32 Color
	{
		get
		{
			return this.color;
		}
		set
		{
			if (!this.color.Equals(value))
			{
				this.color = value;
				this.OnColorChanged();
			}
		}
	}

	public virtual bool ContainsFocus
	{
		get
		{
			return dfGUIManager.ContainsFocus(this);
		}
	}

	public bool ContainsMouse
	{
		get
		{
			return this.isMouseHovering;
		}
	}

	public IList<dfControl> Controls
	{
		get
		{
			return this.controls;
		}
	}

	public Color32 DisabledColor
	{
		get
		{
			return this.disabledColor;
		}
		set
		{
			if (!value.Equals(this.disabledColor))
			{
				this.disabledColor = value;
				this.Invalidate();
			}
		}
	}

	public dfGUIManager GUIManager
	{
		get
		{
			return this.GetManager();
		}
	}

	public virtual bool HasFocus
	{
		get
		{
			return dfGUIManager.HasFocus(this);
		}
	}

	public float Height
	{
		get
		{
			return this.size.y;
		}
		set
		{
			this.Size = new Vector2(this.size.x, value);
		}
	}

	public Vector2 HotZoneScale
	{
		get
		{
			return this.hotZoneScale;
		}
		set
		{
			this.hotZoneScale = Vector2.Max(value, Vector2.zero);
			this.Invalidate();
		}
	}

	public bool IsEnabled
	{
		get
		{
			bool flag;
			if (!base.enabled)
			{
				return false;
			}
			if (base.gameObject != null && !base.gameObject.activeSelf)
			{
				return false;
			}
			if (this.parent == null)
			{
				flag = this.isEnabled;
			}
			else
			{
				flag = (!this.isEnabled ? false : this.parent.IsEnabled);
			}
			return flag;
		}
		set
		{
			if (value != this.isEnabled)
			{
				this.isEnabled = value;
				this.OnIsEnabledChanged();
			}
		}
	}

	public virtual bool IsInteractive
	{
		get
		{
			return this.isInteractive;
		}
		set
		{
			if (this.HasFocus && !value)
			{
				dfGUIManager.SetFocus(null);
			}
			this.isInteractive = value;
		}
	}

	protected bool IsLayoutSuspended
	{
		get
		{
			bool flag;
			if (this.performingLayout)
			{
				flag = true;
			}
			else
			{
				flag = (this.layout == null ? false : this.layout.IsLayoutSuspended);
			}
			return flag;
		}
	}

	public bool IsLocalized
	{
		get
		{
			return this.isLocalized;
		}
		set
		{
			this.isLocalized = value;
			if (value)
			{
				this.Localize();
			}
		}
	}

	protected bool IsPerformingLayout
	{
		get
		{
			if (this.performingLayout)
			{
				return true;
			}
			if (this.layout != null && this.layout.IsPerformingLayout)
			{
				return true;
			}
			return false;
		}
	}

	[SerializeField]
	public bool IsVisible
	{
		get
		{
			bool flag;
			if (this.parent != null)
			{
				flag = (!this.isVisible ? false : this.parent.IsVisible);
			}
			else
			{
				flag = this.isVisible;
			}
			return flag;
		}
		set
		{
			if (value != this.isVisible)
			{
				if (!Application.isPlaying || this.IsInteractive)
				{
					base.collider.enabled = value;
				}
				else
				{
					base.collider.enabled = false;
				}
				this.isVisible = value;
				this.OnIsVisibleChanged();
			}
		}
	}

	public Vector2 MaximumSize
	{
		get
		{
			return this.maxSize;
		}
		set
		{
			value = Vector2.Max(Vector2.zero, value.RoundToInt());
			if (value != this.maxSize)
			{
				this.maxSize = value;
				this.Invalidate();
			}
		}
	}

	public Vector2 MinimumSize
	{
		get
		{
			return this.minSize;
		}
		set
		{
			value = Vector2.Max(Vector2.zero, value.RoundToInt());
			if (value != this.minSize)
			{
				this.minSize = value;
				this.Invalidate();
			}
		}
	}

	public float Opacity
	{
		get
		{
			return (float)this.color.a / 255f;
		}
		set
		{
			value = Mathf.Max(0f, Mathf.Min(1f, value));
			if (value != (float)this.color.a / 255f)
			{
				this.color.a = (byte)(value * 255f);
				this.OnOpacityChanged();
			}
		}
	}

	public dfControl Parent
	{
		get
		{
			return this.parent;
		}
	}

	public dfPivotPoint Pivot
	{
		get
		{
			return this.pivot;
		}
		set
		{
			if (value != this.pivot)
			{
				Vector3 position = this.Position;
				this.pivot = value;
				Vector3 vector3 = this.Position - position;
				this.SuspendLayout();
				this.Position = position;
				for (int i = 0; i < this.controls.Count; i++)
				{
					dfControl item = this.controls[i];
					item.Position = item.Position + vector3;
				}
				this.ResumeLayout();
				this.OnPivotChanged();
			}
		}
	}

	public Vector3 Position
	{
		get
		{
			Vector3 units = base.transform.localPosition / this.PixelsToUnits();
			return units + this.pivot.TransformToUpperLeft(this.Size);
		}
		set
		{
			this.setPositionInternal(value);
		}
	}

	public Vector3 RelativePosition
	{
		get
		{
			return this.getRelativePosition();
		}
		set
		{
			this.setRelativePosition(value);
		}
	}

	[HideInInspector]
	public int RenderOrder
	{
		get
		{
			return this.renderOrder;
		}
	}

	public Vector2 Size
	{
		get
		{
			return this.size;
		}
		set
		{
			value = Vector2.Max(this.CalculateMinimumSize(), value);
			value.x = (this.maxSize.x <= 0f ? value.x : Mathf.Min(value.x, this.maxSize.x));
			value.y = (this.maxSize.y <= 0f ? value.y : Mathf.Min(value.y, this.maxSize.y));
			if ((value - this.size).sqrMagnitude <= 1.401298E-45f)
			{
				return;
			}
			this.size = value;
			this.OnSizeChanged();
		}
	}

	[HideInInspector]
	public int TabIndex
	{
		get
		{
			return this.tabIndex;
		}
		set
		{
			if (value != this.tabIndex)
			{
				this.tabIndex = Mathf.Max(-1, value);
				this.OnTabIndexChanged();
			}
		}
	}

	public object Tag
	{
		get
		{
			return this.tag;
		}
		set
		{
			this.tag = value;
		}
	}

	[SerializeField]
	public string Tooltip
	{
		get
		{
			return this.tooltip;
		}
		set
		{
			if (value != this.tooltip)
			{
				this.tooltip = value;
				this.Invalidate();
			}
		}
	}

	internal uint Version
	{
		get
		{
			return this.version;
		}
	}

	public float Width
	{
		get
		{
			return this.size.x;
		}
		set
		{
			this.Size = new Vector2(value, this.size.y);
		}
	}

	[HideInInspector]
	public int ZOrder
	{
		get
		{
			return this.zindex;
		}
		set
		{
			if (value != this.zindex)
			{
				this.zindex = Mathf.Max(-1, value);
				this.Invalidate();
				if (this.parent != null)
				{
					this.parent.SetControlIndex(this, value);
				}
				this.OnZOrderChanged();
			}
		}
	}

	static dfControl()
	{
	}

	protected dfControl()
	{
	}

	public T AddControl<T>()
	where T : dfControl
	{
		return (T)this.AddControl(typeof(T));
	}

	public dfControl AddControl(Type ControlType)
	{
		if (!typeof(dfControl).IsAssignableFrom(ControlType))
		{
			throw new InvalidCastException();
		}
		GameObject gameObject = new GameObject(ControlType.Name);
		gameObject.transform.parent = base.transform;
		gameObject.layer = base.gameObject.layer;
		Vector2 size = (this.Size * this.PixelsToUnits()) * 0.5f;
		gameObject.transform.localPosition = new Vector3(size.x, size.y, 0f);
		dfControl _dfControl = gameObject.AddComponent(ControlType) as dfControl;
		_dfControl.parent = this;
		_dfControl.zindex = -1;
		this.AddControl(_dfControl);
		return _dfControl;
	}

	public void AddControl(dfControl child)
	{
		if (child.transform == null)
		{
			throw new NullReferenceException("The child control does not have a Transform");
		}
		if (!this.controls.Contains(child))
		{
			this.controls.Add(child);
			child.parent = this;
			child.transform.parent = base.transform;
		}
		if (child.zindex == -1)
		{
			child.zindex = this.getMaxZOrder() + 1;
		}
		this.controls.Sort();
		this.OnControlAdded(child);
		child.Invalidate();
		this.Invalidate();
	}

	protected internal Color32 ApplyOpacity(Color32 color)
	{
		color.a = (byte)(this.CalculateOpacity() * 255f);
		return color;
	}

	[HideInInspector]
	public virtual void Awake()
	{
		if (base.transform.parent != null)
		{
			dfControl component = base.transform.parent.GetComponent<dfControl>();
			if (component != null)
			{
				this.parent = component;
				component.AddControl(this);
			}
			if (this.controls == null)
			{
				this.updateControlHierarchy(false);
			}
			if (!Application.isPlaying)
			{
				this.PerformLayout();
			}
		}
	}

	public virtual void BringToFront()
	{
		if (this.parent != null)
		{
			this.parent.SetControlIndex(this, this.parent.controls.Count - 1);
		}
		else
		{
			this.GetManager().BringToFront(this);
		}
		this.Invalidate();
	}

	public virtual Vector2 CalculateMinimumSize()
	{
		return this.MinimumSize;
	}

	protected internal float CalculateOpacity()
	{
		if (this.parent == null)
		{
			return this.Opacity;
		}
		return this.Opacity * this.parent.CalculateOpacity();
	}

	private static Vector3 closestPointOnLine(Vector3 start, Vector3 end, Vector3 test, bool clamp)
	{
		Vector3 vector3 = test - start;
		Vector3 vector31 = (end - start).normalized;
		float single = (end - start).magnitude;
		float single1 = Vector3.Dot(vector31, vector3);
		if (clamp)
		{
			if (single1 < 0f)
			{
				return start;
			}
			if (single1 > single)
			{
				return end;
			}
		}
		vector31 = vector31 * single1;
		return start + vector31;
	}

	public int CompareTo(dfControl other)
	{
		if (this.ZOrder < 0)
		{
			if (other.ZOrder < 0)
			{
				return 0;
			}
			return 1;
		}
		return this.ZOrder.CompareTo(other.ZOrder);
	}

	public bool Contains(dfControl child)
	{
		return (child == null ? false : child.transform.IsChildOf(base.transform));
	}

	public void Disable()
	{
		this.IsEnabled = false;
	}

	private static float distanceFromLine(Vector3 start, Vector3 end, Vector3 test)
	{
		Vector3 vector3 = start - end;
		float single = Vector3.Dot(test - end, vector3);
		if (single <= 0f)
		{
			return Vector3.Distance(test, end);
		}
		float single1 = Vector3.Dot(vector3, vector3);
		if (single1 <= single)
		{
			return Vector3.Distance(test, start);
		}
		Vector3 vector31 = end + (single / single1 * vector3);
		return Vector3.Distance(test, vector31);
	}

	public void DoClick()
	{
		Camera camera = this.GetCamera();
		Vector3 screenPoint = camera.WorldToScreenPoint(this.GetCenter());
		Ray ray = camera.ScreenPointToRay(screenPoint);
		this.OnClick(new dfMouseEventArgs(this, dfMouseButtons.Left, 1, ray, screenPoint, 0f));
	}

	public void Enable()
	{
		this.IsEnabled = true;
	}

	private void ensureLayoutExists()
	{
		if (this.layout != null)
		{
			this.layout.Attach(this);
		}
		else
		{
			this.layout = new dfControl.AnchorLayout(dfAnchorStyle.Top | dfAnchorStyle.Left, this);
		}
		for (int i = 0; this.Controls != null && i < this.Controls.Count; i++)
		{
			if (this.controls[i] != null)
			{
				this.controls[i].ensureLayoutExists();
			}
		}
	}

	public T Find<T>(string Name)
	where T : dfControl
	{
		if (base.name == Name && this is T)
		{
			return (T)this;
		}
		this.updateControlHierarchy(true);
		for (int i = 0; i < this.controls.Count; i++)
		{
			T item = (T)(this.controls[i] as T);
			if (item != null && item.name == Name)
			{
				return item;
			}
		}
		for (int j = 0; j < this.controls.Count; j++)
		{
			T t = this.controls[j].Find<T>(Name);
			if (t != null)
			{
				return t;
			}
		}
		return (T)null;
	}

	public dfControl Find(string Name)
	{
		if (base.name == Name)
		{
			return this;
		}
		this.updateControlHierarchy(true);
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl item = this.controls[i];
			if (item.name == Name)
			{
				return item;
			}
		}
		for (int j = 0; j < this.controls.Count; j++)
		{
			dfControl _dfControl = this.controls[j].Find(Name);
			if (_dfControl != null)
			{
				return _dfControl;
			}
		}
		return null;
	}

	public void Focus()
	{
		if (!this.CanFocus || this.HasFocus || !this.IsEnabled || !this.IsVisible)
		{
			return;
		}
		dfGUIManager.SetFocus(this);
		this.Invalidate();
	}

	public Bounds GetBounds()
	{
		Vector3[] corners = this.GetCorners();
		Vector3 vector3 = corners[0] + ((corners[3] - corners[0]) * 0.5f);
		Vector3 vector31 = vector3;
		Vector3 vector32 = vector3;
		for (int i = 0; i < (int)corners.Length; i++)
		{
			vector31 = Vector3.Min(vector31, corners[i]);
			vector32 = Vector3.Max(vector32, corners[i]);
		}
		return new Bounds(vector3, vector32 - vector31);
	}

	public Camera GetCamera()
	{
		dfGUIManager manager = this.GetManager();
		if (manager != null)
		{
			return manager.RenderCamera;
		}
		Debug.LogError("The Manager hosting this control could not be determined");
		return null;
	}

	public Vector3 GetCenter()
	{
		return base.transform.position + (this.Pivot.TransformToCenter(this.Size) * this.PixelsToUnits());
	}

	private dfList<dfControl> getChildControls()
	{
		int num = base.transform.childCount;
		dfList<dfControl> dfControls = dfList<dfControl>.Obtain();
		dfControls.EnsureCapacity(num);
		for (int i = 0; i < num; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				dfControl component = child.GetComponent<dfControl>();
				if (component != null)
				{
					dfControls.Add(component);
				}
			}
		}
		return dfControls;
	}

	protected internal virtual Plane[] GetClippingPlanes()
	{
		Vector3[] corners = this.GetCorners();
		Vector3 vector3 = base.transform.TransformDirection(Vector3.right);
		Vector3 vector31 = base.transform.TransformDirection(Vector3.left);
		Vector3 vector32 = base.transform.TransformDirection(Vector3.up);
		Vector3 vector33 = base.transform.TransformDirection(Vector3.down);
		this.cachedClippingPlanes[0] = new Plane(vector3, corners[0]);
		this.cachedClippingPlanes[1] = new Plane(vector31, corners[1]);
		this.cachedClippingPlanes[2] = new Plane(vector32, corners[2]);
		this.cachedClippingPlanes[3] = new Plane(vector33, corners[0]);
		return this.cachedClippingPlanes;
	}

	public Vector3[] GetCorners()
	{
		float units = this.PixelsToUnits();
		Matrix4x4 matrix4x4 = base.transform.localToWorldMatrix;
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(this.size);
		Vector3 vector3 = upperLeft + new Vector3(this.size.x, 0f);
		Vector3 vector31 = upperLeft + new Vector3(0f, -this.size.y);
		Vector3 vector32 = vector3 + new Vector3(0f, -this.size.y);
		this.cachedCorners[0] = matrix4x4.MultiplyPoint(upperLeft * units);
		this.cachedCorners[1] = matrix4x4.MultiplyPoint(vector3 * units);
		this.cachedCorners[2] = matrix4x4.MultiplyPoint(vector31 * units);
		this.cachedCorners[3] = matrix4x4.MultiplyPoint(vector32 * units);
		return this.cachedCorners;
	}

	public bool GetHitPosition(Ray ray, out Vector2 position)
	{
		Plane[] clippingPlanes;
		position = Vector2.one * Single.MinValue;
		Plane plane = new Plane(base.transform.TransformDirection(Vector3.back), base.transform.position);
		float single = 0f;
		if (!plane.Raycast(ray, out single))
		{
			return false;
		}
		Vector3 vector3 = ray.origin + (ray.direction * single);
		if (!this.ClipChildren)
		{
			clippingPlanes = null;
		}
		else
		{
			clippingPlanes = this.GetClippingPlanes();
		}
		Plane[] planeArray = clippingPlanes;
		if (planeArray != null && (int)planeArray.Length > 0)
		{
			for (int i = 0; i < (int)planeArray.Length; i++)
			{
				if (!planeArray[i].GetSide(vector3))
				{
					return false;
				}
			}
		}
		Vector3[] corners = this.GetCorners();
		Vector3 vector31 = corners[0];
		Vector3 vector32 = corners[1];
		Vector3 vector33 = corners[2];
		Vector3 vector34 = dfControl.closestPointOnLine(vector31, vector32, vector3, true);
		Vector3 vector35 = vector34 - vector31;
		float single1 = vector35.magnitude / (vector32 - vector31).magnitude;
		float single2 = this.size.x * single1;
		vector34 = dfControl.closestPointOnLine(vector31, vector33, vector3, true);
		Vector3 vector36 = vector34 - vector31;
		single1 = vector36.magnitude / (vector33 - vector31).magnitude;
		float single3 = this.size.y * single1;
		position = new Vector2(single2, single3);
		return true;
	}

	protected internal Vector2 GetHitPosition(dfMouseEventArgs args)
	{
		Vector2 vector2;
		this.GetHitPosition(args.Ray, out vector2);
		return vector2;
	}

	internal bool GetIsVisibleRaw()
	{
		return this.isVisible;
	}

	[HideInInspector]
	protected internal string getLocalizedValue(string key)
	{
		if (!this.IsLocalized || !Application.isPlaying)
		{
			return key;
		}
		if (this.languageManager == null)
		{
			if (this.languageManagerChecked)
			{
				return key;
			}
			this.languageManagerChecked = true;
			this.languageManager = this.GetManager().GetComponent<dfLanguageManager>();
			if (this.languageManager == null)
			{
				return key;
			}
		}
		return this.languageManager.GetValue(key);
	}

	public dfGUIManager GetManager()
	{
		dfGUIManager _dfGUIManager;
		if (this.manager != null || !base.gameObject.activeInHierarchy)
		{
			return this.manager;
		}
		if (this.parent != null && this.parent.manager != null)
		{
			dfGUIManager _dfGUIManager1 = this.parent.manager;
			_dfGUIManager = _dfGUIManager1;
			this.manager = _dfGUIManager1;
			return _dfGUIManager;
		}
		GameObject gameObject = base.gameObject;
		while (gameObject != null)
		{
			dfGUIManager component = gameObject.GetComponent<dfGUIManager>();
			if (component != null)
			{
				dfGUIManager _dfGUIManager2 = component;
				_dfGUIManager = _dfGUIManager2;
				this.manager = _dfGUIManager2;
				return _dfGUIManager;
			}
			if (gameObject.transform.parent != null)
			{
				gameObject = gameObject.transform.parent.gameObject;
			}
			else
			{
				break;
			}
		}
		dfGUIManager _dfGUIManager3 = UnityEngine.Object.FindObjectsOfType(typeof(dfGUIManager)).FirstOrDefault<UnityEngine.Object>() as dfGUIManager;
		if (_dfGUIManager3 == null)
		{
			return null;
		}
		dfGUIManager _dfGUIManager4 = _dfGUIManager3;
		_dfGUIManager = _dfGUIManager4;
		this.manager = _dfGUIManager4;
		return _dfGUIManager;
	}

	private int getMaxZOrder()
	{
		int num = -1;
		for (int i = 0; i < this.controls.Count; i++)
		{
			num = Mathf.Max(this.controls[i].zindex, num);
		}
		return num;
	}

	private Vector3 getRelativePosition()
	{
		if (base.transform.parent == null)
		{
			return Vector3.zero;
		}
		if (this.parent != null)
		{
			float units = this.PixelsToUnits();
			Vector3 vector3 = base.transform.parent.position;
			Vector3 vector31 = base.transform.position;
			Transform transforms = base.transform.parent;
			Vector3 upperLeft = transforms.InverseTransformPoint(vector3 / units);
			upperLeft = upperLeft + this.parent.pivot.TransformToUpperLeft(this.parent.size);
			Vector3 upperLeft1 = transforms.InverseTransformPoint(vector31 / units);
			upperLeft1 = upperLeft1 + this.pivot.TransformToUpperLeft(this.size);
			Vector3 vector32 = upperLeft1 - upperLeft;
			return vector32.Scale(1f, -1f, 1f);
		}
		dfGUIManager manager = this.GetManager();
		if (manager == null)
		{
			Debug.LogError("Cannot get position: View not found");
			return Vector3.zero;
		}
		float single = this.PixelsToUnits();
		Vector3 upperLeft2 = base.transform.position + (this.pivot.TransformToUpperLeft(this.size) * single);
		Plane[] clippingPlanes = manager.GetClippingPlanes();
		float distanceToPoint = clippingPlanes[0].GetDistanceToPoint(upperLeft2) / single;
		float distanceToPoint1 = clippingPlanes[3].GetDistanceToPoint(upperLeft2) / single;
		return (new Vector3(distanceToPoint, distanceToPoint1)).RoundToInt();
	}

	public dfControl GetRootContainer()
	{
		dfControl parent = this;
		while (parent.Parent != null)
		{
			parent = parent.Parent;
		}
		return parent;
	}

	protected internal Vector3 getScaledDirection(Vector3 direction)
	{
		Vector3 manager = this.GetManager().transform.localScale;
		direction = base.transform.TransformDirection(direction);
		return Vector3.Scale(direction, manager);
	}

	public Rect GetScreenRect()
	{
		Camera camera = this.GetCamera();
		Vector3[] corners = this.GetCorners();
		Vector3 screenPoint = camera.WorldToScreenPoint(corners[0]);
		Vector3 vector3 = camera.WorldToScreenPoint(corners[3]);
		return new Rect(screenPoint.x, (float)Screen.height - screenPoint.y, vector3.x - screenPoint.x, screenPoint.y - vector3.y);
	}

	public void Hide()
	{
		this.IsVisible = false;
	}

	private void initializeControl()
	{
		if (this.renderOrder == -1)
		{
			this.renderOrder = this.ZOrder;
		}
		if (base.transform.parent != null)
		{
			dfControl component = base.transform.parent.GetComponent<dfControl>();
			if (component != null)
			{
				component.AddControl(this);
			}
		}
		this.ensureLayoutExists();
		this.Invalidate();
		base.collider.isTrigger = false;
		if (Application.isPlaying && base.rigidbody == null)
		{
			Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
			rigidbody.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSave | HideFlags.NotEditable | HideFlags.HideAndDontSave;
			rigidbody.isKinematic = true;
			rigidbody.detectCollisions = false;
		}
		this.updateCollider();
	}

	public virtual void Invalidate()
	{
		this.updateVersion();
		this.isControlInvalidated = true;
		dfGUIManager manager = this.GetManager();
		if (manager != null)
		{
			manager.Invalidate();
		}
	}

	[HideInInspector]
	public virtual void LateUpdate()
	{
		if (this.layout != null && this.layout.HasPendingLayoutRequest)
		{
			this.layout.PerformLayout();
		}
	}

	public void Localize()
	{
		if (!this.IsLocalized)
		{
			return;
		}
		if (this.languageManager == null)
		{
			this.languageManager = this.GetManager().GetComponent<dfLanguageManager>();
			if (this.languageManager == null)
			{
				return;
			}
		}
		this.OnLocalize();
	}

	[HideInInspector]
	public void MakePixelPerfect(bool recursive = true)
	{
		this.size = this.size.RoundToInt();
		float units = this.PixelsToUnits();
		base.transform.position = (base.transform.position / units).RoundToInt() * units;
		this.cachedPosition = base.transform.localPosition;
		for (int i = 0; i < this.controls.Count && recursive; i++)
		{
			this.controls[i].MakePixelPerfect(true);
		}
		this.Invalidate();
	}

	[HideInInspector]
	protected internal virtual void OnAnchorChanged()
	{
		dfAnchorStyle anchorStyle = this.layout.AnchorStyle;
		this.Invalidate();
		this.ResetLayout(false, false);
		if (anchorStyle.IsAnyFlagSet(dfAnchorStyle.CenterHorizontal | dfAnchorStyle.CenterVertical))
		{
			this.PerformLayout();
		}
		if (this.AnchorChanged != null)
		{
			this.AnchorChanged(this, anchorStyle);
		}
	}

	[HideInInspector]
	public virtual void OnApplicationQuit()
	{
		this.RemoveAllEventHandlers();
	}

	protected internal virtual void OnClick(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnClick", new object[] { args });
			if (this.Click != null)
			{
				this.Click(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnClick(args);
		}
	}

	[HideInInspector]
	protected internal virtual void OnColorChanged()
	{
		this.Invalidate();
		if (this.ColorChanged != null)
		{
			this.ColorChanged(this, this.Color);
		}
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].OnColorChanged();
		}
	}

	[HideInInspector]
	protected internal virtual void OnControlAdded(dfControl child)
	{
		this.Invalidate();
		if (this.ControlAdded != null)
		{
			this.ControlAdded(this, child);
		}
		this.Signal("OnControlAdded", new object[] { this, child });
	}

	[HideInInspector]
	protected internal virtual void OnControlRemoved(dfControl child)
	{
		this.Invalidate();
		if (this.ControlRemoved != null)
		{
			this.ControlRemoved(this, child);
		}
		this.Signal("OnControlRemoved", new object[] { this, child });
	}

	[HideInInspector]
	public virtual void OnDestroy()
	{
		this.isDisposing = true;
		if (Application.isPlaying)
		{
			this.RemoveAllEventHandlers();
		}
		if (this.layout != null)
		{
			this.layout.Dispose();
		}
		if (this.parent != null && this.parent.controls != null && !this.parent.isDisposing && this.parent.controls.Remove(this))
		{
			dfControl _dfControl = this.parent;
			_dfControl.cachedChildCount = _dfControl.cachedChildCount - 1;
			this.parent.OnControlRemoved(this);
		}
		for (int i = 0; i < this.controls.Count; i++)
		{
			if (this.controls[i].layout != null)
			{
				this.controls[i].layout.Dispose();
				this.controls[i].layout = null;
			}
			this.controls[i].parent = null;
		}
		this.controls.Release();
		if (this.manager != null)
		{
			this.manager.Invalidate();
		}
		if (this.renderData != null)
		{
			this.renderData.Release();
		}
		this.layout = null;
		this.manager = null;
		this.parent = null;
		this.cachedClippingPlanes = null;
		this.cachedCorners = null;
		this.renderData = null;
		this.controls = null;
	}

	[HideInInspector]
	public virtual void OnDisable()
	{
		try
		{
			this.Invalidate();
			if (this.renderData != null)
			{
				this.renderData.Release();
				this.renderData = null;
			}
			if (dfGUIManager.HasFocus(this))
			{
				dfGUIManager.SetFocus(null);
			}
			this.OnIsEnabledChanged();
		}
		catch
		{
		}
	}

	protected internal virtual void OnDoubleClick(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDoubleClick", new object[] { args });
			if (this.DoubleClick != null)
			{
				this.DoubleClick(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDoubleClick(args);
		}
	}

	internal virtual void OnDragDrop(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragDrop", new object[] { args });
			if (!args.Used && this.DragDrop != null)
			{
				this.DragDrop(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragDrop(args);
		}
	}

	internal virtual void OnDragEnd(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragEnd", new object[] { args });
			if (!args.Used && this.DragEnd != null)
			{
				this.DragEnd(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragEnd(args);
		}
	}

	internal virtual void OnDragEnter(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragEnter", new object[] { args });
			if (!args.Used && this.DragEnter != null)
			{
				this.DragEnter(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragEnter(args);
		}
	}

	internal virtual void OnDragLeave(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragLeave", new object[] { args });
			if (!args.Used && this.DragLeave != null)
			{
				this.DragLeave(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragLeave(args);
		}
	}

	internal virtual void OnDragOver(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragOver", new object[] { args });
			if (!args.Used && this.DragOver != null)
			{
				this.DragOver(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragOver(args);
		}
	}

	internal virtual void OnDragStart(dfDragEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnDragStart", new object[] { args });
			if (!args.Used && this.DragStart != null)
			{
				this.DragStart(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnDragStart(args);
		}
	}

	[HideInInspector]
	public virtual void OnEnable()
	{
		if (Application.isPlaying)
		{
			base.collider.enabled = this.IsInteractive;
		}
		this.initializeControl();
		if (this.controls == null || this.controls.Count == 0)
		{
			this.updateControlHierarchy(false);
		}
		if (Application.isPlaying && this.IsLocalized)
		{
			this.Localize();
		}
		this.OnIsEnabledChanged();
	}

	protected internal virtual void OnEnterFocus(dfFocusEventArgs args)
	{
		this.Signal("OnEnterFocus", new object[] { args });
		if (this.EnterFocus != null)
		{
			this.EnterFocus(this, args);
		}
	}

	protected internal virtual void OnGotFocus(dfFocusEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnGotFocus", new object[] { args });
			if (this.GotFocus != null)
			{
				this.GotFocus(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnGotFocus(args);
		}
	}

	[HideInInspector]
	protected internal virtual void OnIsEnabledChanged()
	{
		if (dfGUIManager.ContainsFocus(this) && !this.IsEnabled)
		{
			dfGUIManager.SetFocus(null);
		}
		this.Invalidate();
		this.Signal("OnIsEnabledChanged", new object[] { this, this.IsEnabled });
		if (this.IsEnabledChanged != null)
		{
			this.IsEnabledChanged(this, this.isEnabled);
		}
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].OnIsEnabledChanged();
		}
	}

	[HideInInspector]
	protected internal virtual void OnIsVisibleChanged()
	{
		if (this.HasFocus && !this.IsVisible)
		{
			dfGUIManager.SetFocus(null);
		}
		this.Invalidate();
		this.Signal("OnIsVisibleChanged", new object[] { this, this.IsVisible });
		if (this.IsVisibleChanged != null)
		{
			this.IsVisibleChanged(this, this.isVisible);
		}
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].OnIsVisibleChanged();
		}
	}

	protected internal virtual void OnKeyDown(dfKeyEventArgs args)
	{
		if (this.IsInteractive && !args.Used)
		{
			if (args.KeyCode == KeyCode.Tab)
			{
				this.OnTabKeyPressed(args);
			}
			if (!args.Used)
			{
				this.Signal("OnKeyDown", new object[] { args });
				if (this.KeyDown != null)
				{
					this.KeyDown(this, args);
				}
			}
		}
		if (this.parent != null)
		{
			this.parent.OnKeyDown(args);
		}
	}

	protected internal virtual void OnKeyPress(dfKeyEventArgs args)
	{
		if (this.IsInteractive && !args.Used)
		{
			this.Signal("OnKeyPress", new object[] { args });
			if (this.KeyPress != null)
			{
				this.KeyPress(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnKeyPress(args);
		}
	}

	protected internal virtual void OnKeyUp(dfKeyEventArgs args)
	{
		if (this.IsInteractive)
		{
			this.Signal("OnKeyUp", new object[] { args });
			if (this.KeyUp != null)
			{
				this.KeyUp(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnKeyUp(args);
		}
	}

	protected internal virtual void OnLeaveFocus(dfFocusEventArgs args)
	{
		this.Signal("OnLeaveFocus", new object[] { args });
		if (this.LeaveFocus != null)
		{
			this.LeaveFocus(this, args);
		}
	}

	[HideInInspector]
	protected internal virtual void OnLocalize()
	{
	}

	protected internal virtual void OnLostFocus(dfFocusEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnLostFocus", new object[] { args });
			if (this.LostFocus != null)
			{
				this.LostFocus(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnLostFocus(args);
		}
	}

	protected internal virtual void OnMouseDown(dfMouseEventArgs args)
	{
		if ((this.Opacity <= 0.01f || !this.IsVisible || !this.IsEnabled || !this.CanFocus ? false : !this.ContainsFocus))
		{
			this.Focus();
		}
		if (!args.Used)
		{
			this.Signal("OnMouseDown", new object[] { args });
			if (this.MouseDown != null)
			{
				this.MouseDown(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseDown(args);
		}
	}

	protected internal virtual void OnMouseEnter(dfMouseEventArgs args)
	{
		this.isMouseHovering = true;
		if (!args.Used)
		{
			this.Signal("OnMouseEnter", new object[] { args });
			if (this.MouseEnter != null)
			{
				this.MouseEnter(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseEnter(args);
		}
	}

	protected internal virtual void OnMouseHover(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnMouseHover", new object[] { args });
			if (this.MouseHover != null)
			{
				this.MouseHover(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseHover(args);
		}
	}

	protected internal virtual void OnMouseLeave(dfMouseEventArgs args)
	{
		this.isMouseHovering = false;
		if (!args.Used)
		{
			this.Signal("OnMouseLeave", new object[] { args });
			if (this.MouseLeave != null)
			{
				this.MouseLeave(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseLeave(args);
		}
	}

	protected internal virtual void OnMouseMove(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnMouseMove", new object[] { args });
			if (this.MouseMove != null)
			{
				this.MouseMove(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseMove(args);
		}
	}

	protected internal virtual void OnMouseUp(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnMouseUp", new object[] { args });
			if (this.MouseUp != null)
			{
				this.MouseUp(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseUp(args);
		}
	}

	protected internal virtual void OnMouseWheel(dfMouseEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnMouseWheel", new object[] { args });
			if (this.MouseWheel != null)
			{
				this.MouseWheel(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMouseWheel(args);
		}
	}

	protected internal virtual void OnMultiTouch(dfTouchEventArgs args)
	{
		if (!args.Used)
		{
			this.Signal("OnMultiTouch", new object[] { args });
			if (this.MultiTouch != null)
			{
				this.MultiTouch(this, args);
			}
		}
		if (this.parent != null)
		{
			this.parent.OnMultiTouch(args);
		}
	}

	[HideInInspector]
	protected internal virtual void OnOpacityChanged()
	{
		this.Invalidate();
		if (this.OpacityChanged != null)
		{
			this.OpacityChanged(this, this.Opacity);
		}
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].OnOpacityChanged();
		}
	}

	[HideInInspector]
	protected internal virtual void OnPivotChanged()
	{
		this.Invalidate();
		if (this.Anchor.IsAnyFlagSet(dfAnchorStyle.CenterHorizontal | dfAnchorStyle.CenterVertical))
		{
			this.ResetLayout(false, false);
			this.PerformLayout();
		}
		if (this.PivotChanged != null)
		{
			this.PivotChanged(this, this.pivot);
		}
	}

	[HideInInspector]
	protected internal virtual void OnPositionChanged()
	{
		base.transform.hasChanged = false;
		if (this.renderData == null)
		{
			this.Invalidate();
		}
		else
		{
			this.updateVersion();
			this.GetManager().Invalidate();
		}
		this.ResetLayout(false, false);
		if (this.PositionChanged != null)
		{
			this.PositionChanged(this, this.Position);
		}
	}

	[HideInInspector]
	protected virtual void OnRebuildRenderData()
	{
	}

	protected internal virtual void OnResolutionChanged(Vector2 previousResolution, Vector2 currentResolution)
	{
		this.Invalidate();
		this.updateControlHierarchy(false);
		this.cachedPixelSize = 0f;
		Vector3 vector3 = base.transform.localPosition / (2f / previousResolution.y);
		Vector3 vector31 = vector3 * (2f / currentResolution.y);
		base.transform.localPosition = vector31;
		this.cachedPosition = vector31;
		this.layout.Attach(this);
		this.updateCollider();
		this.Signal("OnResolutionChanged", new object[] { this, previousResolution, currentResolution });
	}

	[HideInInspector]
	protected internal virtual void OnSizeChanged()
	{
		this.updateCollider();
		this.Invalidate();
		this.ResetLayout(false, false);
		if (this.Anchor.IsAnyFlagSet(dfAnchorStyle.CenterHorizontal | dfAnchorStyle.CenterVertical))
		{
			this.PerformLayout();
		}
		if (this.SizeChanged != null)
		{
			this.SizeChanged(this, this.Size);
		}
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].PerformLayout();
		}
	}

	[HideInInspector]
	protected internal virtual void OnTabIndexChanged()
	{
		this.Invalidate();
		if (this.TabIndexChanged != null)
		{
			this.TabIndexChanged(this, this.tabIndex);
		}
	}

	protected virtual void OnTabKeyPressed(dfKeyEventArgs args)
	{
		List<dfControl> list = (
			from c in this.GetManager().GetComponentsInChildren<dfControl>()
			where (!(c != this) || c.TabIndex < 0 || !c.IsInteractive || !c.CanFocus ? false : c.IsVisible)
			select c).ToList<dfControl>();
		if (list.Count == 0)
		{
			return;
		}
		list.Sort((dfControl lhs, dfControl rhs) => {
			if (lhs.TabIndex == rhs.TabIndex)
			{
				return lhs.RenderOrder.CompareTo(rhs.RenderOrder);
			}
			return lhs.TabIndex.CompareTo(rhs.TabIndex);
		});
		if (!args.Shift)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].TabIndex >= this.TabIndex)
				{
					list[i].Focus();
					args.Use();
					return;
				}
			}
			list[0].Focus();
			args.Use();
			return;
		}
		for (int j = list.Count - 1; j >= 0; j--)
		{
			if (list[j].TabIndex <= this.TabIndex)
			{
				list[j].Focus();
				args.Use();
				return;
			}
		}
		list[list.Count - 1].Focus();
		args.Use();
	}

	[HideInInspector]
	protected internal virtual void OnZOrderChanged()
	{
		this.Invalidate();
		if (this.ZOrderChanged != null)
		{
			this.ZOrderChanged(this, this.zindex);
		}
	}

	[HideInInspector]
	public virtual void PerformLayout()
	{
		if (this.isDisposing || this.performingLayout)
		{
			return;
		}
		try
		{
			this.performingLayout = true;
			this.ensureLayoutExists();
			this.layout.PerformLayout();
			this.Invalidate();
		}
		finally
		{
			this.performingLayout = false;
		}
	}

	protected internal float PixelsToUnits()
	{
		if (this.cachedPixelSize > 1.401298E-45f)
		{
			return this.cachedPixelSize;
		}
		dfGUIManager manager = this.GetManager();
		if (manager == null)
		{
			return 0.0026f;
		}
		float units = manager.PixelsToUnits();
		float single = units;
		this.cachedPixelSize = units;
		return single;
	}

	[HideInInspector]
	protected internal void RaiseEvent(string eventName, params object[] args)
	{
		FieldInfo fieldInfo = (
			from f in base.GetType().GetAllFields()
			where f.Name == eventName
			select f).FirstOrDefault<FieldInfo>();
		if (fieldInfo != null)
		{
			object value = fieldInfo.GetValue(this);
			if (value != null)
			{
				((Delegate)value).DynamicInvoke(args);
			}
		}
	}

	[HideInInspector]
	public void RebuildControlOrder()
	{
		bool flag = false;
		this.controls.Sort();
		int num = 0;
		while (num < this.controls.Count)
		{
			if (this.controls[num].ZOrder == num)
			{
				num++;
			}
			else
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		this.controls.Sort();
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].zindex = i;
		}
	}

	[HideInInspector]
	internal void RemoveAllEventHandlers()
	{
		FieldInfo[] array = (
			from f in (IEnumerable<FieldInfo>)base.GetType().GetAllFields()
			where typeof(Delegate).IsAssignableFrom(f.FieldType)
			select f).ToArray<FieldInfo>();
		for (int i = 0; i < (int)array.Length; i++)
		{
			array[i].SetValue(this, null);
		}
	}

	public void RemoveControl(dfControl child)
	{
		if (this.isDisposing)
		{
			return;
		}
		if (child.Parent == this)
		{
			child.parent = null;
		}
		if (this.controls.Remove(child))
		{
			this.OnControlRemoved(child);
			child.Invalidate();
			this.Invalidate();
		}
	}

	[HideInInspector]
	protected internal void RemoveEventHandlers(string EventName)
	{
		FieldInfo fieldInfo = (
			from f in base.GetType().GetAllFields()
			where (!typeof(Delegate).IsAssignableFrom(f.FieldType) ? false : f.Name == EventName)
			select f).FirstOrDefault<FieldInfo>();
		if (fieldInfo != null)
		{
			fieldInfo.SetValue(this, null);
		}
	}

	internal dfRenderData Render()
	{
		dfRenderData dfRenderDatum;
		if (this.rendering)
		{
			return this.renderData;
		}
		try
		{
			this.rendering = true;
			bool flag = this.isVisible;
			if (!flag || (!base.enabled ? true : !base.gameObject.activeSelf))
			{
				dfRenderDatum = null;
			}
			else
			{
				if (this.renderData == null)
				{
					this.renderData = dfRenderData.Obtain();
					this.isControlInvalidated = true;
				}
				if (this.isControlInvalidated)
				{
					this.renderData.Clear();
					this.OnRebuildRenderData();
					this.updateCollider();
				}
				this.renderData.Transform = base.transform.localToWorldMatrix;
				dfRenderDatum = this.renderData;
			}
		}
		finally
		{
			this.rendering = false;
			this.isControlInvalidated = false;
		}
		return dfRenderDatum;
	}

	[HideInInspector]
	public virtual void ResetLayout(bool recursive = false, bool force = false)
	{
		if (!force && (this.IsPerformingLayout ? true : this.IsLayoutSuspended))
		{
			return;
		}
		this.ensureLayoutExists();
		this.layout.Attach(this);
		this.layout.Reset(force);
		if (recursive)
		{
			for (int i = 0; i < this.Controls.Count; i++)
			{
				this.controls[i].ResetLayout(false, false);
			}
		}
	}

	[HideInInspector]
	public virtual void ResumeLayout()
	{
		this.ensureLayoutExists();
		this.layout.ResumeLayout();
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].ResumeLayout();
		}
	}

	public virtual void SendToBack()
	{
		if (this.parent != null)
		{
			this.parent.SetControlIndex(this, 0);
		}
		else
		{
			this.GetManager().SendToBack(this);
		}
		this.Invalidate();
	}

	protected internal void SetControlIndex(dfControl child, int zindex)
	{
		dfControl _dfControl = this.controls.FirstOrDefault((dfControl c) => (c.zindex != zindex ? false : c != child));
		if (_dfControl != null)
		{
			_dfControl.zindex = this.controls.IndexOf(child);
		}
		child.zindex = zindex;
		this.RebuildControlOrder();
	}

	private void setPositionInternal(Vector3 value)
	{
		value = value + this.pivot.UpperLeftToTransform(this.Size);
		value = value * this.PixelsToUnits();
		if ((value - this.cachedPosition).sqrMagnitude <= 1.401298E-45f)
		{
			return;
		}
		Vector3 vector3 = value;
		base.transform.localPosition = vector3;
		this.cachedPosition = vector3;
		this.OnPositionChanged();
	}

	private void setRelativePosition(Vector3 value)
	{
		if (base.transform.parent == null)
		{
			Debug.LogError("Cannot set relative position without a parent Transform.");
			return;
		}
		if ((value - this.getRelativePosition()).sqrMagnitude <= 1.401298E-45f)
		{
			return;
		}
		if (this.parent != null)
		{
			Vector3 units = (value.Scale(1f, -1f, 1f) + this.pivot.UpperLeftToTransform(this.size)) - this.parent.pivot.UpperLeftToTransform(this.parent.size);
			units = units * this.PixelsToUnits();
			if ((units - base.transform.localPosition).sqrMagnitude >= 1.401298E-45f)
			{
				base.transform.localPosition = units;
				this.cachedPosition = units;
				this.OnPositionChanged();
			}
			return;
		}
		dfGUIManager manager = this.GetManager();
		if (manager == null)
		{
			Debug.LogError("Cannot get position: View not found");
			return;
		}
		Vector3 corners = manager.GetCorners()[0];
		float single = this.PixelsToUnits();
		value = value.Scale(1f, -1f, 1f) * single;
		Vector3 transform = this.pivot.UpperLeftToTransform(this.Size) * single;
		Vector3 vector3 = (corners + manager.transform.TransformDirection(value)) + transform;
		if ((vector3 - this.cachedPosition).sqrMagnitude > 1.401298E-45f)
		{
			base.transform.position = vector3;
			this.cachedPosition = base.transform.localPosition;
			this.OnPositionChanged();
		}
	}

	internal void setRenderOrder(ref int order)
	{
		int num = order + 1;
		int num1 = num;
		order = num;
		this.renderOrder = num1;
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].setRenderOrder(ref order);
		}
	}

	public void Show()
	{
		this.IsVisible = true;
	}

	protected internal bool Signal(string eventName, params object[] args)
	{
		return this.Signal(base.gameObject, eventName, args);
	}

	[HideInInspector]
	protected internal bool Signal(GameObject target, string eventName, params object[] args)
	{
		Component[] components = target.GetComponents(typeof(MonoBehaviour));
		if (components == null || target == base.gameObject && (int)components.Length == 1)
		{
			return false;
		}
		if ((int)args.Length == 0 || !object.ReferenceEquals(args[0], this))
		{
			object[] objArray = new object[(int)args.Length + 1];
			Array.Copy(args, 0, objArray, 1, (int)args.Length);
			objArray[0] = this;
			args = objArray;
		}
		Type[] type = new Type[(int)args.Length];
		for (int i = 0; i < (int)type.Length; i++)
		{
			if (args[i] != null)
			{
				type[i] = args[i].GetType();
			}
			else
			{
				type[i] = typeof(object);
			}
		}
		bool flag = false;
		for (int j = 0; j < (int)components.Length; j++)
		{
			Component component = components[j];
			if (!(component == null) && component.GetType() != null)
			{
				if (!(component is MonoBehaviour) || ((MonoBehaviour)component).enabled)
				{
					if (component != this)
					{
						MethodInfo method = component.GetType().GetMethod(eventName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, type, null);
						IEnumerator enumerator = null;
						if (method != null)
						{
							enumerator = method.Invoke(component, args) as IEnumerator;
							if (enumerator != null)
							{
								((MonoBehaviour)component).StartCoroutine(enumerator);
							}
							flag = true;
						}
						else if ((int)args.Length != 0)
						{
							MethodInfo methodInfo = component.GetType().GetMethod(eventName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
							if (methodInfo != null)
							{
								enumerator = methodInfo.Invoke(component, null) as IEnumerator;
								if (enumerator != null)
								{
									((MonoBehaviour)component).StartCoroutine(enumerator);
								}
								flag = true;
							}
						}
					}
				}
			}
		}
		return flag;
	}

	protected internal bool SignalHierarchy(string eventName, params object[] args)
	{
		bool flag = false;
		for (Transform i = base.transform; !flag && i != null; i = i.parent)
		{
			flag = this.Signal(i.gameObject, eventName, args);
		}
		return flag;
	}

	[HideInInspector]
	public virtual void Start()
	{
	}

	[HideInInspector]
	public virtual void SuspendLayout()
	{
		this.ensureLayoutExists();
		this.layout.SuspendLayout();
		for (int i = 0; i < this.controls.Count; i++)
		{
			this.controls[i].SuspendLayout();
		}
	}

	protected internal Vector3 transformOffset(Vector3 offset)
	{
		Vector3 vector3 = offset.x * this.getScaledDirection(Vector3.right);
		Vector3 vector31 = offset.y * this.getScaledDirection(Vector3.down);
		return (vector3 + vector31) * this.PixelsToUnits();
	}

	public void Unfocus()
	{
		if (this.ContainsFocus)
		{
			dfGUIManager.SetFocus(null);
		}
	}

	[HideInInspector]
	public virtual void Update()
	{
		Transform transforms = base.transform;
		this.updateControlHierarchy(false);
		if (transforms.hasChanged)
		{
			if (Application.isPlaying)
			{
				if (this.cachedScale != transforms.localScale)
				{
					this.cachedScale = transforms.localScale;
					this.Invalidate();
				}
			}
			if ((this.cachedPosition - transforms.localPosition).sqrMagnitude > 1.401298E-45f)
			{
				this.cachedPosition = transforms.localPosition;
				this.OnPositionChanged();
			}
			if (this.cachedRotation != transforms.localRotation)
			{
				this.cachedRotation = transforms.localRotation;
				this.Invalidate();
			}
			transforms.hasChanged = false;
		}
	}

	[HideInInspector]
	protected internal virtual void updateCollider()
	{
		if (Application.isPlaying && !this.isInteractive)
		{
			return;
		}
		BoxCollider vector3 = base.collider as BoxCollider;
		if (vector3 == null)
		{
			vector3 = base.gameObject.AddComponent<BoxCollider>();
		}
		float units = this.PixelsToUnits();
		Vector2 vector2 = this.size * units;
		Vector3 center = this.pivot.TransformToCenter(vector2);
		vector3.size = new Vector3(vector2.x * this.hotZoneScale.x, vector2.y * this.hotZoneScale.y, 0.001f);
		vector3.center = center;
		if (!Application.isPlaying || this.IsInteractive)
		{
			vector3.enabled = (!base.enabled ? false : this.IsVisible);
		}
		else
		{
			vector3.enabled = false;
		}
	}

	internal void updateControlHierarchy(bool force = false)
	{
		int num = base.transform.childCount;
		if (!force && num == this.cachedChildCount)
		{
			return;
		}
		this.cachedChildCount = num;
		dfList<dfControl> childControls = this.getChildControls();
		for (int i = 0; i < childControls.Count; i++)
		{
			dfControl item = childControls[i];
			if (!this.controls.Contains(item))
			{
				item.parent = this;
				if (!Application.isPlaying)
				{
					item.ResetLayout(false, false);
				}
				this.OnControlAdded(item);
				item.updateControlHierarchy(false);
			}
		}
		for (int j = 0; j < this.controls.Count; j++)
		{
			dfControl _dfControl = this.controls[j];
			if (_dfControl == null || !childControls.Contains(_dfControl))
			{
				this.OnControlRemoved(_dfControl);
				if (_dfControl != null && _dfControl.parent == this)
				{
					_dfControl.parent = null;
				}
			}
		}
		this.controls.Release();
		this.controls = childControls;
		this.RebuildControlOrder();
	}

	protected internal void updateVersion()
	{
		UInt32 num = dfControl.versionCounter + 1;
		dfControl.versionCounter = num;
		this.version = num;
	}

	[HideInInspector]
	public event PropertyChangedEventHandler<dfAnchorStyle> AnchorChanged
	{
		add
		{
			this.AnchorChanged += value;
		}
		remove
		{
			this.AnchorChanged -= value;
		}
	}

	public event MouseEventHandler Click
	{
		add
		{
			this.Click += value;
		}
		remove
		{
			this.Click -= value;
		}
	}

	[HideInInspector]
	public event PropertyChangedEventHandler<Color32> ColorChanged
	{
		add
		{
			this.ColorChanged += value;
		}
		remove
		{
			this.ColorChanged -= value;
		}
	}

	[HideInInspector]
	public event ChildControlEventHandler ControlAdded
	{
		add
		{
			this.ControlAdded += value;
		}
		remove
		{
			this.ControlAdded -= value;
		}
	}

	[HideInInspector]
	public event ChildControlEventHandler ControlRemoved
	{
		add
		{
			this.ControlRemoved += value;
		}
		remove
		{
			this.ControlRemoved -= value;
		}
	}

	public event MouseEventHandler DoubleClick
	{
		add
		{
			this.DoubleClick += value;
		}
		remove
		{
			this.DoubleClick -= value;
		}
	}

	public event DragEventHandler DragDrop
	{
		add
		{
			this.DragDrop += value;
		}
		remove
		{
			this.DragDrop -= value;
		}
	}

	public event DragEventHandler DragEnd
	{
		add
		{
			this.DragEnd += value;
		}
		remove
		{
			this.DragEnd -= value;
		}
	}

	public event DragEventHandler DragEnter
	{
		add
		{
			this.DragEnter += value;
		}
		remove
		{
			this.DragEnter -= value;
		}
	}

	public event DragEventHandler DragLeave
	{
		add
		{
			this.DragLeave += value;
		}
		remove
		{
			this.DragLeave -= value;
		}
	}

	public event DragEventHandler DragOver
	{
		add
		{
			this.DragOver += value;
		}
		remove
		{
			this.DragOver -= value;
		}
	}

	public event DragEventHandler DragStart
	{
		add
		{
			this.DragStart += value;
		}
		remove
		{
			this.DragStart -= value;
		}
	}

	public event FocusEventHandler EnterFocus
	{
		add
		{
			this.EnterFocus += value;
		}
		remove
		{
			this.EnterFocus -= value;
		}
	}

	public event FocusEventHandler GotFocus
	{
		add
		{
			this.GotFocus += value;
		}
		remove
		{
			this.GotFocus -= value;
		}
	}

	public event PropertyChangedEventHandler<bool> IsEnabledChanged
	{
		add
		{
			this.IsEnabledChanged += value;
		}
		remove
		{
			this.IsEnabledChanged -= value;
		}
	}

	public event PropertyChangedEventHandler<bool> IsVisibleChanged
	{
		add
		{
			this.IsVisibleChanged += value;
		}
		remove
		{
			this.IsVisibleChanged -= value;
		}
	}

	public event KeyPressHandler KeyDown
	{
		add
		{
			this.KeyDown += value;
		}
		remove
		{
			this.KeyDown -= value;
		}
	}

	public event KeyPressHandler KeyPress
	{
		add
		{
			this.KeyPress += value;
		}
		remove
		{
			this.KeyPress -= value;
		}
	}

	public event KeyPressHandler KeyUp
	{
		add
		{
			this.KeyUp += value;
		}
		remove
		{
			this.KeyUp -= value;
		}
	}

	public event FocusEventHandler LeaveFocus
	{
		add
		{
			this.LeaveFocus += value;
		}
		remove
		{
			this.LeaveFocus -= value;
		}
	}

	public event FocusEventHandler LostFocus
	{
		add
		{
			this.LostFocus += value;
		}
		remove
		{
			this.LostFocus -= value;
		}
	}

	public event MouseEventHandler MouseDown
	{
		add
		{
			this.MouseDown += value;
		}
		remove
		{
			this.MouseDown -= value;
		}
	}

	public event MouseEventHandler MouseEnter
	{
		add
		{
			this.MouseEnter += value;
		}
		remove
		{
			this.MouseEnter -= value;
		}
	}

	public event MouseEventHandler MouseHover
	{
		add
		{
			this.MouseHover += value;
		}
		remove
		{
			this.MouseHover -= value;
		}
	}

	public event MouseEventHandler MouseLeave
	{
		add
		{
			this.MouseLeave += value;
		}
		remove
		{
			this.MouseLeave -= value;
		}
	}

	public event MouseEventHandler MouseMove
	{
		add
		{
			this.MouseMove += value;
		}
		remove
		{
			this.MouseMove -= value;
		}
	}

	public event MouseEventHandler MouseUp
	{
		add
		{
			this.MouseUp += value;
		}
		remove
		{
			this.MouseUp -= value;
		}
	}

	public event MouseEventHandler MouseWheel
	{
		add
		{
			this.MouseWheel += value;
		}
		remove
		{
			this.MouseWheel -= value;
		}
	}

	public event ControlMultiTouchEventHandler MultiTouch
	{
		add
		{
			this.MultiTouch += value;
		}
		remove
		{
			this.MultiTouch -= value;
		}
	}

	[HideInInspector]
	public event PropertyChangedEventHandler<float> OpacityChanged
	{
		add
		{
			this.OpacityChanged += value;
		}
		remove
		{
			this.OpacityChanged -= value;
		}
	}

	[HideInInspector]
	public event PropertyChangedEventHandler<dfPivotPoint> PivotChanged
	{
		add
		{
			this.PivotChanged += value;
		}
		remove
		{
			this.PivotChanged -= value;
		}
	}

	public event PropertyChangedEventHandler<Vector2> PositionChanged
	{
		add
		{
			this.PositionChanged += value;
		}
		remove
		{
			this.PositionChanged -= value;
		}
	}

	public event PropertyChangedEventHandler<Vector2> SizeChanged
	{
		add
		{
			this.SizeChanged += value;
		}
		remove
		{
			this.SizeChanged -= value;
		}
	}

	public event PropertyChangedEventHandler<int> TabIndexChanged
	{
		add
		{
			this.TabIndexChanged += value;
		}
		remove
		{
			this.TabIndexChanged -= value;
		}
	}

	[HideInInspector]
	public event PropertyChangedEventHandler<int> ZOrderChanged
	{
		add
		{
			this.ZOrderChanged += value;
		}
		remove
		{
			this.ZOrderChanged -= value;
		}
	}

	[Serializable]
	protected class AnchorLayout
	{
		[SerializeField]
		protected dfAnchorStyle anchorStyle;

		[SerializeField]
		protected dfAnchorMargins margins;

		[SerializeField]
		protected dfControl owner;

		private int suspendLayoutCounter;

		private bool performingLayout;

		private bool disposed;

		private bool pendingLayoutRequest;

		internal dfAnchorStyle AnchorStyle
		{
			get
			{
				return this.anchorStyle;
			}
			set
			{
				if (value != this.anchorStyle)
				{
					this.anchorStyle = value;
					this.Reset(false);
				}
			}
		}

		internal bool HasPendingLayoutRequest
		{
			get
			{
				return this.pendingLayoutRequest;
			}
		}

		internal bool IsLayoutSuspended
		{
			get
			{
				return this.suspendLayoutCounter > 0;
			}
		}

		internal bool IsPerformingLayout
		{
			get
			{
				return this.performingLayout;
			}
		}

		internal AnchorLayout(dfAnchorStyle anchorStyle)
		{
			this.anchorStyle = anchorStyle;
		}

		internal AnchorLayout(dfAnchorStyle anchorStyle, dfControl owner) : this(anchorStyle)
		{
			this.Attach(owner);
			this.Reset(false);
		}

		internal void Attach(dfControl ownerControl)
		{
			this.owner = ownerControl;
		}

		internal void Dispose()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.owner = null;
			}
		}

		private Vector2 getParentSize()
		{
			dfControl component = this.owner.transform.parent.GetComponent<dfControl>();
			if (component != null)
			{
				return component.Size;
			}
			return this.owner.GetManager().GetScreenSize();
		}

		private string getPath(dfControl owner)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			while (owner != null)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Insert(0, '/');
				}
				stringBuilder.Insert(0, owner.name);
				owner = owner.Parent;
			}
			return stringBuilder.ToString();
		}

		internal void PerformLayout()
		{
			if (this.disposed)
			{
				return;
			}
			if (this.suspendLayoutCounter <= 0)
			{
				this.performLayoutInternal();
			}
			else
			{
				this.pendingLayoutRequest = true;
			}
		}

		private void performLayoutAbsolute(Vector2 parentSize, Vector2 controlSize)
		{
			float num = this.margins.left;
			float single = this.margins.top;
			float num1 = num + controlSize.x;
			float single1 = single + controlSize.y;
			if (!this.anchorStyle.IsFlagSet(dfAnchorStyle.CenterHorizontal))
			{
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Left))
				{
					num = this.margins.left;
					num1 = num + controlSize.x;
				}
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Right))
				{
					num1 = parentSize.x - this.margins.right;
					if (!this.anchorStyle.IsFlagSet(dfAnchorStyle.Left))
					{
						num = num1 - controlSize.x;
					}
				}
			}
			else
			{
				num = (float)Mathf.RoundToInt((parentSize.x - controlSize.x) * 0.5f);
				num1 = (float)Mathf.RoundToInt(num + controlSize.x);
			}
			if (!this.anchorStyle.IsFlagSet(dfAnchorStyle.CenterVertical))
			{
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Top))
				{
					single = this.margins.top;
					single1 = single + controlSize.y;
				}
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Bottom))
				{
					single1 = parentSize.y - this.margins.bottom;
					if (!this.anchorStyle.IsFlagSet(dfAnchorStyle.Top))
					{
						single = single1 - controlSize.y;
					}
				}
			}
			else
			{
				single = (float)Mathf.RoundToInt((parentSize.y - controlSize.y) * 0.5f);
				single1 = (float)Mathf.RoundToInt(single + controlSize.y);
			}
			Vector2 vector2 = new Vector2(Mathf.Max(0f, num1 - num), Mathf.Max(0f, single1 - single));
			this.owner.Size = vector2;
			this.owner.RelativePosition = new Vector3(num, single);
		}

		protected void performLayoutInternal()
		{
			if ((this.margins == null || this.IsPerformingLayout || this.IsLayoutSuspended || this.owner == null ? true : !this.owner.gameObject.activeSelf))
			{
				return;
			}
			try
			{
				this.performingLayout = true;
				this.pendingLayoutRequest = false;
				Vector2 parentSize = this.getParentSize();
				Vector2 size = this.owner.Size;
				if (!this.anchorStyle.IsFlagSet(dfAnchorStyle.Proportional))
				{
					this.performLayoutAbsolute(parentSize, size);
				}
				else
				{
					this.performLayoutProportional(parentSize, size);
				}
			}
			finally
			{
				this.performingLayout = false;
			}
		}

		private void performLayoutProportional(Vector2 parentSize, Vector2 controlSize)
		{
			float single = this.margins.left * parentSize.x;
			float single1 = this.margins.right * parentSize.x;
			float single2 = this.margins.top * parentSize.y;
			float single3 = this.margins.bottom * parentSize.y;
			Vector3 relativePosition = this.owner.RelativePosition;
			Vector2 vector2 = controlSize;
			if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Left))
			{
				relativePosition.x = single;
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Right))
				{
					vector2.x = (this.margins.right - this.margins.left) * parentSize.x;
				}
			}
			else if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Right))
			{
				relativePosition.x = single1 - controlSize.x;
			}
			else if (this.anchorStyle.IsFlagSet(dfAnchorStyle.CenterHorizontal))
			{
				relativePosition.x = (parentSize.x - controlSize.x) * 0.5f;
			}
			if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Top))
			{
				relativePosition.y = single2;
				if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Bottom))
				{
					vector2.y = (this.margins.bottom - this.margins.top) * parentSize.y;
				}
			}
			else if (this.anchorStyle.IsFlagSet(dfAnchorStyle.Bottom))
			{
				relativePosition.y = single3 - controlSize.y;
			}
			else if (this.anchorStyle.IsFlagSet(dfAnchorStyle.CenterVertical))
			{
				relativePosition.y = (parentSize.y - controlSize.y) * 0.5f;
			}
			this.owner.Size = vector2;
			this.owner.RelativePosition = relativePosition;
			if (this.owner.GetManager().PixelPerfectMode)
			{
				this.owner.MakePixelPerfect(false);
			}
		}

		internal void Reset(bool force = false)
		{
			bool flag;
			if (this.owner == null || this.owner.transform.parent == null)
			{
				return;
			}
			if (force)
			{
				flag = false;
			}
			else
			{
				flag = (this.IsPerformingLayout ? true : this.IsLayoutSuspended);
			}
			if ((flag || this.owner == null ? true : !this.owner.gameObject.activeSelf))
			{
				return;
			}
			if (!this.anchorStyle.IsFlagSet(dfAnchorStyle.Proportional))
			{
				this.resetLayoutAbsolute();
			}
			else
			{
				this.resetLayoutProportional();
			}
		}

		private void resetLayoutAbsolute()
		{
			Vector3 relativePosition = this.owner.RelativePosition;
			Vector2 size = this.owner.Size;
			Vector2 parentSize = this.getParentSize();
			float single = relativePosition.x;
			float single1 = relativePosition.y;
			float single2 = parentSize.x - size.x - single;
			float single3 = parentSize.y - size.y - single1;
			if (this.margins == null)
			{
				this.margins = new dfAnchorMargins();
			}
			this.margins.left = single;
			this.margins.right = single2;
			this.margins.top = single1;
			this.margins.bottom = single3;
		}

		private void resetLayoutProportional()
		{
			Vector3 relativePosition = this.owner.RelativePosition;
			Vector2 size = this.owner.Size;
			Vector2 parentSize = this.getParentSize();
			float single = relativePosition.x;
			float single1 = relativePosition.y;
			float single2 = single + size.x;
			float single3 = single1 + size.y;
			if (this.margins == null)
			{
				this.margins = new dfAnchorMargins();
			}
			this.margins.left = single / parentSize.x;
			this.margins.right = single2 / parentSize.x;
			this.margins.top = single1 / parentSize.y;
			this.margins.bottom = single3 / parentSize.y;
		}

		internal void ResumeLayout()
		{
			bool flag = this.suspendLayoutCounter > 0;
			this.suspendLayoutCounter = Mathf.Max(0, this.suspendLayoutCounter - 1);
			if (flag && this.suspendLayoutCounter == 0 && this.pendingLayoutRequest)
			{
				this.PerformLayout();
			}
		}

		internal void SuspendLayout()
		{
			dfControl.AnchorLayout anchorLayout = this;
			anchorLayout.suspendLayoutCounter = anchorLayout.suspendLayoutCounter + 1;
		}

		public override string ToString()
		{
			if (this.owner == null)
			{
				return "NO OWNER FOR ANCHOR";
			}
			dfControl _dfControl = this.owner.parent;
			return string.Format("{0}.{1} - {2}", (_dfControl == null ? "SCREEN" : _dfControl.name), this.owner.name, this.margins);
		}
	}
}