using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Scrollbar")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfScrollbar : dfControl
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected dfControlOrientation orientation;

	[SerializeField]
	protected float rawValue = 1f;

	[SerializeField]
	protected float minValue;

	[SerializeField]
	protected float maxValue = 100f;

	[SerializeField]
	protected float stepSize = 1f;

	[SerializeField]
	protected float scrollSize = 1f;

	[SerializeField]
	protected float increment = 1f;

	[SerializeField]
	protected dfControl thumb;

	[SerializeField]
	protected dfControl track;

	[SerializeField]
	protected dfControl incButton;

	[SerializeField]
	protected dfControl decButton;

	[SerializeField]
	protected RectOffset thumbPadding = new RectOffset();

	[SerializeField]
	protected bool autoHide;

	private Vector3 thumbMouseOffset = Vector3.zero;

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

	public bool AutoHide
	{
		get
		{
			return this.autoHide;
		}
		set
		{
			if (value != this.autoHide)
			{
				this.autoHide = value;
				this.Invalidate();
				this.doAutoHide();
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

	public dfControl DecButton
	{
		get
		{
			return this.decButton;
		}
		set
		{
			if (value != this.decButton)
			{
				this.decButton = value;
				this.Invalidate();
			}
		}
	}

	public dfControl IncButton
	{
		get
		{
			return this.incButton;
		}
		set
		{
			if (value != this.incButton)
			{
				this.incButton = value;
				this.Invalidate();
			}
		}
	}

	public float IncrementAmount
	{
		get
		{
			return this.increment;
		}
		set
		{
			value = Mathf.Max(0f, value);
			if (!Mathf.Approximately(value, this.increment))
			{
				this.increment = value;
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
				this.Value = this.Value;
				this.Invalidate();
				this.doAutoHide();
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
				this.Value = this.Value;
				this.Invalidate();
				this.doAutoHide();
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
				this.Value = this.Value;
				this.Invalidate();
				this.doAutoHide();
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
				this.Value = this.Value;
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
			}
		}
	}

	public RectOffset ThumbPadding
	{
		get
		{
			if (this.thumbPadding == null)
			{
				this.thumbPadding = new RectOffset();
			}
			return this.thumbPadding;
		}
		set
		{
			int num;
			if (this.orientation != dfControlOrientation.Horizontal)
			{
				num = 0;
				value.right = num;
				value.left = num;
			}
			else
			{
				num = 0;
				value.bottom = num;
				value.top = num;
			}
			if (!object.Equals(value, this.thumbPadding))
			{
				this.thumbPadding = value;
				this.updateThumb(this.rawValue);
			}
		}
	}

	public dfControl Track
	{
		get
		{
			return this.track;
		}
		set
		{
			if (value != this.track)
			{
				this.track = value;
				this.Invalidate();
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
			value = this.adjustValue(value);
			if (!Mathf.Approximately(value, this.rawValue))
			{
				this.rawValue = value;
				this.OnValueChanged();
			}
			this.updateThumb(this.rawValue);
		}
	}

	public dfScrollbar()
	{
	}

	private float adjustValue(float value)
	{
		float single = Mathf.Max(this.maxValue - this.minValue, 0f);
		float single1 = Mathf.Max(single - this.scrollSize, 0f) + this.minValue;
		float single2 = Mathf.Max(Mathf.Min(single1, value), this.minValue);
		return single2.Quantize(this.stepSize);
	}

	private void attachEvents()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.IncButton != null)
		{
			this.IncButton.MouseDown += new MouseEventHandler(this.incrementPressed);
			this.IncButton.MouseHover += new MouseEventHandler(this.incrementPressed);
		}
		if (this.DecButton != null)
		{
			this.DecButton.MouseDown += new MouseEventHandler(this.decrementPressed);
			this.DecButton.MouseHover += new MouseEventHandler(this.decrementPressed);
		}
	}

	public override Vector2 CalculateMinimumSize()
	{
		Vector2[] vector2Array = new Vector2[3];
		if (this.decButton != null)
		{
			vector2Array[0] = this.decButton.CalculateMinimumSize();
		}
		if (this.incButton != null)
		{
			vector2Array[1] = this.incButton.CalculateMinimumSize();
		}
		if (this.thumb != null)
		{
			vector2Array[2] = this.thumb.CalculateMinimumSize();
		}
		Vector2 vector2 = Vector2.zero;
		if (this.orientation != dfControlOrientation.Horizontal)
		{
			vector2.x = Mathf.Max(new float[] { vector2Array[0].x, vector2Array[1].x, vector2Array[2].x });
			vector2.y = vector2Array[0].y + vector2Array[1].y + vector2Array[2].y;
		}
		else
		{
			vector2.x = vector2Array[0].x + vector2Array[1].x + vector2Array[2].x;
			vector2.y = Mathf.Max(new float[] { vector2Array[0].y, vector2Array[1].y, vector2Array[2].y });
		}
		return Vector2.Max(vector2, base.CalculateMinimumSize());
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

	private void decrementPressed(dfControl sender, dfMouseEventArgs args)
	{
		if (args.Buttons.IsSet(dfMouseButtons.Left))
		{
			dfScrollbar value = this;
			value.Value = value.Value - this.IncrementAmount;
			args.Use();
		}
	}

	private void detachEvents()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.IncButton != null)
		{
			this.IncButton.MouseDown -= new MouseEventHandler(this.incrementPressed);
			this.IncButton.MouseHover -= new MouseEventHandler(this.incrementPressed);
		}
		if (this.DecButton != null)
		{
			this.DecButton.MouseDown -= new MouseEventHandler(this.decrementPressed);
			this.DecButton.MouseHover -= new MouseEventHandler(this.decrementPressed);
		}
	}

	private void doAutoHide()
	{
		if (!this.autoHide || !Application.isPlaying)
		{
			return;
		}
		if (Mathf.CeilToInt(this.ScrollSize) < Mathf.CeilToInt(this.maxValue - this.minValue))
		{
			base.Show();
		}
		else
		{
			base.Hide();
		}
	}

	private float getValueFromMouseEvent(dfMouseEventArgs args)
	{
		Vector3[] corners = this.track.GetCorners();
		Vector3 vector3 = corners[0];
		Vector3 vector31 = corners[(this.orientation != dfControlOrientation.Horizontal ? 2 : 1)];
		Plane plane = new Plane(base.transform.TransformDirection(Vector3.back), vector3);
		Ray ray = args.Ray;
		float single = 0f;
		if (!plane.Raycast(ray, out single))
		{
			return this.rawValue;
		}
		Vector3 vector32 = ray.origin + (ray.direction * single);
		if (args.Source == this.thumb)
		{
			vector32 = vector32 + this.thumbMouseOffset;
		}
		Vector3 vector33 = dfScrollbar.closestPoint(vector3, vector31, vector32, true);
		Vector3 vector34 = vector33 - vector3;
		float single1 = vector34.magnitude / (vector31 - vector3).magnitude;
		float single2 = this.minValue + (this.maxValue - this.minValue) * single1;
		return single2;
	}

	private void incrementPressed(dfControl sender, dfMouseEventArgs args)
	{
		if (args.Buttons.IsSet(dfMouseButtons.Left))
		{
			dfScrollbar value = this;
			value.Value = value.Value + this.IncrementAmount;
			args.Use();
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		this.detachEvents();
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.detachEvents();
	}

	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (this.Orientation != dfControlOrientation.Horizontal)
		{
			if (args.KeyCode == KeyCode.UpArrow)
			{
				dfScrollbar value = this;
				value.Value = value.Value - this.IncrementAmount;
				args.Use();
				return;
			}
			if (args.KeyCode == KeyCode.DownArrow)
			{
				dfScrollbar _dfScrollbar = this;
				_dfScrollbar.Value = _dfScrollbar.Value + this.IncrementAmount;
				args.Use();
				return;
			}
		}
		else
		{
			if (args.KeyCode == KeyCode.LeftArrow)
			{
				dfScrollbar value1 = this;
				value1.Value = value1.Value - this.IncrementAmount;
				args.Use();
				return;
			}
			if (args.KeyCode == KeyCode.RightArrow)
			{
				dfScrollbar _dfScrollbar1 = this;
				_dfScrollbar1.Value = _dfScrollbar1.Value + this.IncrementAmount;
				args.Use();
				return;
			}
		}
		base.OnKeyDown(args);
	}

	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		RaycastHit raycastHit;
		if (args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.Focus();
		}
		if (args.Source == this.incButton || args.Source == this.decButton)
		{
			return;
		}
		if (args.Source != this.track && args.Source != this.thumb || !args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.OnMouseDown(args);
			return;
		}
		if (args.Source != this.thumb)
		{
			this.updateFromTrackClick(args);
		}
		else
		{
			this.thumb.collider.Raycast(args.Ray, out raycastHit, 1000f);
			Vector3 center = this.thumb.transform.position + this.thumb.Pivot.TransformToCenter(this.thumb.Size * base.PixelsToUnits());
			this.thumbMouseOffset = center - raycastHit.point;
		}
		args.Use();
		base.Signal("OnMouseDown", new object[] { args });
	}

	protected internal override void OnMouseHover(dfMouseEventArgs args)
	{
		if ((args.Source == this.incButton || args.Source == this.decButton ? true : args.Source == this.thumb))
		{
			return;
		}
		if (args.Source != this.track || !args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.OnMouseHover(args);
			return;
		}
		this.updateFromTrackClick(args);
		args.Use();
		base.Signal("OnMouseHover", new object[] { args });
	}

	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		if (args.Source == this.incButton || args.Source == this.decButton)
		{
			return;
		}
		if (args.Source != this.track && args.Source != this.thumb || !args.Buttons.IsSet(dfMouseButtons.Left))
		{
			base.OnMouseMove(args);
			return;
		}
		this.Value = Mathf.Max(this.minValue, this.getValueFromMouseEvent(args) - this.scrollSize * 0.5f);
		args.Use();
		base.Signal("OnMouseMove", new object[] { args });
	}

	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		dfScrollbar value = this;
		value.Value = value.Value + this.IncrementAmount * -args.WheelDelta;
		args.Use();
		base.Signal("OnMouseWheel", new object[] { args });
	}

	protected override void OnRebuildRenderData()
	{
		this.updateThumb(this.rawValue);
		base.OnRebuildRenderData();
	}

	protected internal override void OnSizeChanged()
	{
		base.OnSizeChanged();
		this.updateThumb(this.rawValue);
	}

	protected internal virtual void OnValueChanged()
	{
		this.Invalidate();
		base.SignalHierarchy("OnValueChanged", new object[] { this.Value });
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, this.Value);
		}
	}

	public override void Start()
	{
		base.Start();
		this.attachEvents();
	}

	private void updateFromTrackClick(dfMouseEventArgs args)
	{
		float valueFromMouseEvent = this.getValueFromMouseEvent(args);
		if (valueFromMouseEvent > this.rawValue + this.scrollSize)
		{
			dfScrollbar value = this;
			value.Value = value.Value + this.scrollSize;
		}
		else if (valueFromMouseEvent < this.rawValue)
		{
			dfScrollbar _dfScrollbar = this;
			_dfScrollbar.Value = _dfScrollbar.Value - this.scrollSize;
		}
	}

	private void updateThumb(float rawValue)
	{
		float single;
		if (this.controls.Count == 0 || this.thumb == null || this.track == null || !base.IsVisible)
		{
			return;
		}
		float single1 = this.maxValue - this.minValue;
		if (single1 <= 0f || single1 <= this.scrollSize)
		{
			this.thumb.IsVisible = false;
			return;
		}
		this.thumb.IsVisible = true;
		float single2 = (this.orientation != dfControlOrientation.Horizontal ? this.track.Height : this.track.Width);
		if (this.orientation != dfControlOrientation.Horizontal)
		{
			Vector2 minimumSize = this.thumb.MinimumSize;
			single = Mathf.Max(this.scrollSize / single1 * single2, minimumSize.y);
		}
		else
		{
			Vector2 vector2 = this.thumb.MinimumSize;
			single = Mathf.Max(this.scrollSize / single1 * single2, vector2.x);
		}
		float single3 = single;
		Vector2 vector21 = (this.orientation != dfControlOrientation.Horizontal ? new Vector2(this.thumb.Width, single3) : new Vector2(single3, this.thumb.Height));
		if (this.Orientation != dfControlOrientation.Horizontal)
		{
			vector21.y = vector21.y - (float)this.thumbPadding.vertical;
		}
		else
		{
			vector21.x = vector21.x - (float)this.thumbPadding.horizontal;
		}
		this.thumb.Size = vector21;
		float single4 = (rawValue - this.minValue) / (single1 - this.scrollSize);
		float single5 = single4 * (single2 - single3);
		Vector3 vector3 = (this.orientation != dfControlOrientation.Horizontal ? Vector3.up : Vector3.right);
		Vector3 vector31 = (this.Orientation != dfControlOrientation.Horizontal ? new Vector3((this.track.Width - this.thumb.Width) * 0.5f, 0f) : new Vector3(0f, (this.track.Height - this.thumb.Height) * 0.5f));
		if (this.Orientation != dfControlOrientation.Horizontal)
		{
			vector31.y = (float)this.thumbPadding.top;
		}
		else
		{
			vector31.x = (float)this.thumbPadding.left;
		}
		if (this.thumb.Parent != this)
		{
			this.thumb.RelativePosition = (vector3 * single5) + vector31;
		}
		else
		{
			this.thumb.RelativePosition = (this.track.RelativePosition + vector31) + (vector3 * single5);
		}
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