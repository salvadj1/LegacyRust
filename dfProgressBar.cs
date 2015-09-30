using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Progress Bar")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfProgressBar : dfControl
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected string backgroundSprite;

	[SerializeField]
	protected string progressSprite;

	[SerializeField]
	protected Color32 progressColor = UnityEngine.Color.white;

	[SerializeField]
	protected float rawValue = 0.25f;

	[SerializeField]
	protected float minValue;

	[SerializeField]
	protected float maxValue = 1f;

	[SerializeField]
	protected dfProgressFillMode fillMode;

	[SerializeField]
	protected RectOffset padding = new RectOffset();

	[SerializeField]
	protected bool actAsSlider;

	private PropertyChangedEventHandler<float> ValueChanged;

	public bool ActAsSlider
	{
		get
		{
			return this.actAsSlider;
		}
		set
		{
			this.actAsSlider = value;
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
				this.setDefaultSize(value);
				this.Invalidate();
			}
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
			if (!object.Equals(value, this.padding))
			{
				this.padding = value;
				this.Invalidate();
			}
		}
	}

	public Color32 ProgressColor
	{
		get
		{
			return this.progressColor;
		}
		set
		{
			if (!object.Equals(value, this.progressColor))
			{
				this.progressColor = value;
				this.Invalidate();
			}
		}
	}

	public string ProgressSprite
	{
		get
		{
			return this.progressSprite;
		}
		set
		{
			if (value != this.progressSprite)
			{
				this.progressSprite = value;
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
			value = Mathf.Max(this.minValue, Mathf.Min(this.maxValue, value));
			if (!Mathf.Approximately(value, this.rawValue))
			{
				this.rawValue = value;
				this.OnValueChanged();
			}
		}
	}

	public dfProgressBar()
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
		Vector3 vector3 = new Vector3(upperLeft.x + (float)this.padding.left, upperLeft.y - this.size.y * 0.5f);
		Vector3 vector31 = vector3 + new Vector3(this.size.x - (float)this.padding.right, 0f);
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
		Vector3 vector33 = dfProgressBar.closestPoint(vector3, vector31, vector32, true);
		Vector3 vector34 = vector33 - vector3;
		float single1 = vector34.magnitude / (vector31 - vector3).magnitude;
		float single2 = this.minValue + (this.maxValue - this.minValue) * single1;
		return single2;
	}

	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		try
		{
			if (this.actAsSlider)
			{
				float single = (this.maxValue - this.minValue) * 0.1f;
				if (args.KeyCode == KeyCode.LeftArrow)
				{
					dfProgressBar value = this;
					value.Value = value.Value - single;
					args.Use();
				}
				else if (args.KeyCode == KeyCode.RightArrow)
				{
					dfProgressBar _dfProgressBar = this;
					_dfProgressBar.Value = _dfProgressBar.Value + single;
					args.Use();
				}
			}
		}
		finally
		{
			base.OnKeyDown(args);
		}
	}

	protected internal override void OnMouseDown(dfMouseEventArgs args)
	{
		try
		{
			if (this.actAsSlider)
			{
				if (args.Buttons.IsSet(dfMouseButtons.Left))
				{
					base.Focus();
					this.Value = this.getValueFromMouseEvent(args);
					args.Use();
				}
			}
		}
		finally
		{
			base.OnMouseDown(args);
		}
	}

	protected internal override void OnMouseMove(dfMouseEventArgs args)
	{
		try
		{
			if (this.actAsSlider)
			{
				if (args.Buttons.IsSet(dfMouseButtons.Left))
				{
					this.Value = this.getValueFromMouseEvent(args);
					args.Use();
				}
			}
		}
		finally
		{
			base.OnMouseMove(args);
		}
	}

	protected internal override void OnMouseWheel(dfMouseEventArgs args)
	{
		try
		{
			if (this.actAsSlider)
			{
				float single = (this.maxValue - this.minValue) * 0.1f;
				dfProgressBar value = this;
				value.Value = value.Value + single * (float)Mathf.RoundToInt(-args.WheelDelta);
				args.Use();
			}
		}
		finally
		{
			base.OnMouseWheel(args);
		}
	}

	protected override void OnRebuildRenderData()
	{
		if (this.Atlas == null)
		{
			return;
		}
		this.renderData.Material = this.Atlas.Material;
		this.renderBackground();
		this.renderProgressFill();
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

	private void renderBackground()
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
		Color32 color32 = base.ApplyOpacity((!base.IsEnabled ? base.DisabledColor : base.Color));
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

	private void renderProgressFill()
	{
		if (this.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo item = this.Atlas[this.progressSprite];
		if (item == null)
		{
			return;
		}
		Vector3 vector3 = new Vector3((float)this.padding.left, (float)(-this.padding.top));
		Vector2 vector2 = new Vector2(this.size.x - (float)this.padding.horizontal, this.size.y - (float)this.padding.vertical);
		float single = 1f;
		float single1 = this.maxValue - this.minValue;
		float single2 = (this.rawValue - this.minValue) / single1;
		dfProgressFillMode _dfProgressFillMode = this.fillMode;
		if (_dfProgressFillMode == dfProgressFillMode.Stretch)
		{
			vector2.x * single2 >= (float)item.border.horizontal;
		}
		if (_dfProgressFillMode != dfProgressFillMode.Fill)
		{
			vector2.x = Mathf.Max((float)item.border.horizontal, vector2.x * single2);
		}
		else
		{
			single = single2;
		}
		Color32 color32 = base.ApplyOpacity((!base.IsEnabled ? base.DisabledColor : this.ProgressColor));
		dfSprite.RenderOptions renderOption = new dfSprite.RenderOptions();
		dfSprite.RenderOptions upperLeft = renderOption;
		upperLeft.atlas = this.atlas;
		upperLeft.color = color32;
		upperLeft.fillAmount = single;
		upperLeft.offset = this.pivot.TransformToUpperLeft(base.Size) + vector3;
		upperLeft.pixelsToUnits = base.PixelsToUnits();
		upperLeft.size = vector2;
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

	private void setDefaultSize(string spriteName)
	{
		if (this.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo item = this.Atlas[spriteName];
		if (this.size == Vector2.zero && item != null)
		{
			base.Size = item.sizeInPixels;
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