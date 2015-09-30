using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Slider")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfSlider : dfControl
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected string backgroundSprite;

	[SerializeField]
	protected dfControlOrientation orientation;

	[SerializeField]
	protected float rawValue = 10f;

	[SerializeField]
	protected float minValue;

	[SerializeField]
	protected float maxValue = 100f;

	[SerializeField]
	protected float stepSize = 1f;

	[SerializeField]
	protected float scrollSize = 1f;

	[SerializeField]
	protected dfControl thumb;

	[SerializeField]
	protected dfControl fillIndicator;

	[SerializeField]
	protected dfProgressFillMode fillMode = dfProgressFillMode.Fill;

	[SerializeField]
	protected RectOffset fillPadding = new RectOffset();

	[SerializeField]
	protected Vector2 thumbOffset = Vector2.zero;

	[SerializeField]
	protected bool rightToLeft;

	private PropertyChangedEventHandler<float> ValueChanged;

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

	public dfProgressFillMode FillMode
	{
		get
		{
			return this.fillMode;
		}
		set
		{
			if (value != this.fillMode)
			{
				this.fillMode = value;
				this.Invalidate();
			}
		}
	}

	public RectOffset FillPadding
	{
		get
		{
			if (this.fillPadding == null)
			{
				this.fillPadding = new RectOffset();
			}
			return this.fillPadding;
		}
		set
		{
			if (!object.Equals(value, this.fillPadding))
			{
				this.fillPadding = value;
				this.updateValueIndicators(this.rawValue);
				this.Invalidate();
			}
		}
	}

	public float MaxValue
	{
		get
		{
			return this.maxValue;
		}
		set
		{
			if (value != this.maxValue)
			{
				this.maxValue = value;
				if (this.rawValue > value)
				{
					this.Value = value;
				}
				this.Invalidate();
			}
		}
	}

	public float MinValue
	{
		get
		{
			return this.minValue;
		}
		set
		{
			if (value != this.minValue)
			{
				this.minValue = value;
				if (this.rawValue < value)
				{
					this.Value = value;
				}
				this.Invalidate();
			}
		}
	}

	public dfControlOrientation Orientation
	{
		get
		{
			return this.orientation;
		}
		set
		{
			if (value != this.orientation)
			{
				this.orientation = value;
				this.Invalidate();
				this.updateValueIndicators(this.rawValue);
			}
		}
	}

	public dfControl Progress
	{
		get
		{
			return this.fillIndicator;
		}
		set
		{
			if (value != this.fillIndicator)
			{
				this.fillIndicator = value;
				this.Invalidate();
				this.updateValueIndicators(this.rawValue);
			}
		}
	}

	public bool RightToLeft
	{
		get
		{
			return this.rightToLeft;
		}
		set
		{
			if (value != this.rightToLeft)
			{
				this.rightToLeft = value;
				this.updateValueIndicators(this.rawValue);
			}
		}
	}

	public float ScrollSize
	{
		get
		{
			return this.scrollSize;
		}
		set
		{
			value = Mathf.Max(0f, value);
			if (value != this.scrollSize)
			{
				this.scrollSize = value;
				this.Invalidate();
			}
		}
	}

	public float StepSize
	{
		get
		{
			return this.stepSize;
		}
		set
		{
			value = Mathf.Max(0f, value);
			if (value != this.stepSize)
			{
				this.stepSize = value;
				this.Value = this.rawValue.Quantize(value);
				this.Invalidate();
			}
		}
	}

	public dfControl Thumb
	{
		get
		{
			return this.thumb;
		}
		set
		{
			if (value != this.thumb)
			{
				this.thumb = value;
				this.Invalidate();
				this.updateValueIndicators(this.rawValue);
			}
		}
	}

	public Vector2 ThumbOffset
	{
		get
		{
			return this.thumbOffset;
		}
		set
		{
			if (Vector2.Distance(value, this.thumbOffset) > 1.401298E-45f)
			{
				this.thumbOffset = value;
				this.updateValueIndicators(this.rawValue);
			}
		}
	}

	public float Value
	{
		get
		{
			return this.rawValue;
		}
		set
		{
			value = Mathf.Max(this.minValue, Mathf.Min(this.maxValue, value)).Quantize(this.stepSize);
			if (!Mathf.Approximately(value, this.rawValue))
			{
				this.rawValue = value;
				this.OnValueChanged();
			}
		}
	}

	public dfSlider()
	{
	}

	private static Vector3 closestPoint(Vector3 start, Vector3 end, Vector3 test, bool clamp)
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

	private Vector3[] getEndPoints(bool convertToWorld = false)
	{
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(base.Size);
		Vector3 vector3 = new Vector3(upperLeft.x, upperLeft.y - this.size.y * 0.5f);
		Vector3 vector31 = vector3 + new Vector3(this.size.x, 0f);
		if (this.orientation == dfControlOrientation.Vertical)
		{
			vector3 = new Vector3(upperLeft.x + this.size.x * 0.5f, upperLeft.y);
			vector31 = vector3 - new Vector3(0f, this.size.y);
		}
		if (convertToWorld)
		{
			float units = base.PixelsToUnits();
			Matrix4x4 matrix4x4 = base.transform.localToWorldMatrix;
			vector3 = matrix4x4.MultiplyPoint(vector3 * units);
			vector31 = matrix4x4.MultiplyPoint(vector31 * units);
		}
		return new Vector3[] { vector3, vector31 };
	}

	private float getValueFromMouseEvent(dfMouseEventArgs args)
	{
		Vector3[] endPoints = this.getEndPoints(true);
		Vector3 vector3 = endPoints[0];
		Vector3 vector31 = endPoints[1];
		Plane plane = new Plane(base.transform.TransformDirection(Vector3.back), vector3);
		Ray ray = args.Ray;
		float single = 0f;
		if (!plane.Raycast(ray, out single))
		{
			return this.rawValue;
		}
		Vector3 vector32 = ray.origin + (ray.direction * single);
		Vector3 vector33 = dfSlider.closestPoint(vector3, vector31, vector32, true);
		Vector3 vector34 = vector33 - vector3;
		float single1 = vector34.magnitude / (vector31 - vector3).magnitude;
		float single2 = this.minValue + (this.maxValue - this.minValue) * single1;
		if (this.orientation == dfControlOrientation.Vertical || this.rightToLeft)
		{
			single2 = this.maxValue - single2;
		}
		return single2;
	}

	public override void OnEnable()
	{
		if (this.size.magnitude < 1.401298E-45f)
		{
			this.size = new Vector2(100f, 25f);
		}
		base.OnEnable();
		this.updateValueIndicators(this.rawValue);
	}

	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (this.Orientation != dfControlOrientation.Horizontal)
		{
			if (args.KeyCode == KeyCode.UpArrow)
			{
				dfSlider value = this;
				value.Value = value.Value - this.ScrollSize;
				args.Use();
				return;
			}
			if (args.KeyCode == KeyCode.DownArrow)
			{
				dfSlider _dfSlider = this;
				_dfSlider.Value = _dfSlider.Value + this.ScrollSize;
				args.Use();
				return;
			}
		}
		else
		{
			if (args.KeyCode == KeyCode.LeftArrow)
			{
				dfSlider value1 = this;
				value1.Value = value1.Value - this.ScrollSize;
				args.Use();
				return;
			}
			if (args.KeyCode == KeyCode.RightArrow)
			{
				dfSlider _dfSlider1 = this;
				_dfSlider1.Value = _dfSlider1.Value + this.ScrollSize;
				args.Use();
				return;
			}
		}
		base.OnKeyDown(args);
	}

	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		if (!args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.OnMouseMove(args);
			return;
		}
		base.Focus();
		this.Value = this.getValueFromMouseEvent(args);
		args.Use();
		base.Signal("OnMouseDown", new object[] { args });
		base.RaiseEvent("MouseDown", new object[] { this, args });
	}

	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		if (!args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.OnMouseMove(args);
			return;
		}
		this.Value = this.getValueFromMouseEvent(args);
		args.Use();
		base.Signal("OnMouseMove", new object[] { args });
		base.RaiseEvent("MouseMove", new object[] { this, args });
	}

	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		int num = (this.orientation != dfControlOrientation.Horizontal ? 1 : -1);
		dfSlider value = this;
		value.Value = value.Value + this.scrollSize * args.WheelDelta * (float)num;
		args.Use();
		base.Signal("OnMouseWheel", new object[] { args });
		base.RaiseEvent("MouseWheel", new object[] { this, args });
	}

	protected override void OnRebuildRenderData()
	{
		if (this.Atlas == null)
		{
			return;
		}
		this.renderData.Material = this.Atlas.Material;
		this.renderBackground();
	}

	protected internal override void OnSizeChanged()
	{
		base.OnSizeChanged();
		this.updateValueIndicators(this.rawValue);
	}

	protected internal virtual void OnValueChanged()
	{
		this.Invalidate();
		this.updateValueIndicators(this.rawValue);
		base.SignalHierarchy("OnValueChanged", new object[] { this.Value });
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, this.Value);
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
		Color32 color32 = base.ApplyOpacity((!base.IsEnabled ? this.disabledColor : this.color));
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

	public override void Start()
	{
		base.Start();
		this.updateValueIndicators(this.rawValue);
	}

	private void updateValueIndicators(float rawValue)
	{
		if (this.thumb != null)
		{
			Vector3[] endPoints = this.getEndPoints(true);
			Vector3 vector3 = endPoints[1] - endPoints[0];
			float single = this.maxValue - this.minValue;
			float single1 = (rawValue - this.minValue) / single * vector3.magnitude;
			Vector3 units = this.thumbOffset * base.PixelsToUnits();
			Vector3 vector31 = (endPoints[0] + (vector3.normalized * single1)) + units;
			if (this.orientation == dfControlOrientation.Vertical || this.rightToLeft)
			{
				vector31 = (endPoints[1] + (-vector3.normalized * single1)) + units;
			}
			this.thumb.Pivot = dfPivotPoint.MiddleCenter;
			this.thumb.transform.position = vector31;
		}
		if (this.fillIndicator == null)
		{
			return;
		}
		RectOffset fillPadding = this.FillPadding;
		float single2 = (rawValue - this.minValue) / (this.maxValue - this.minValue);
		Vector3 vector32 = new Vector3((float)fillPadding.left, (float)fillPadding.top);
		Vector2 vector2 = this.size - new Vector2((float)fillPadding.horizontal, (float)fillPadding.vertical);
		dfSprite _dfSprite = this.fillIndicator as dfSprite;
		if (_dfSprite != null && this.fillMode == dfProgressFillMode.Fill)
		{
			_dfSprite.FillAmount = single2;
		}
		else if (this.orientation != dfControlOrientation.Horizontal)
		{
			vector2.y = base.Height * single2 - (float)fillPadding.vertical;
		}
		else
		{
			vector2.x = base.Width * single2 - (float)fillPadding.horizontal;
		}
		this.fillIndicator.Size = vector2;
		this.fillIndicator.RelativePosition = vector32;
	}

	public event PropertyChangedEventHandler<float> ValueChanged
	{
		add
		{
			this.ValueChanged += value;
		}
		remove
		{
			this.ValueChanged -= value;
		}
	}
}