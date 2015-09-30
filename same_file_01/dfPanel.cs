using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Containers/Panel")]
[ExecuteInEditMode]
[Serializable]
public class dfPanel : dfControl
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected string backgroundSprite;

	[SerializeField]
	protected Color32 backgroundColor = UnityEngine.Color.white;

	[SerializeField]
	protected RectOffset padding = new RectOffset();

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
			value = base.getLocalizedValue(value);
			if (value != this.backgroundSprite)
			{
				this.backgroundSprite = value;
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

	public dfPanel()
	{
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
		base.Size = vector2 + new Vector2((float)this.padding.right, (float)this.padding.bottom);
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
		RectOffset padding = this.Padding;
		corners[0] = corners[0] + ((vector3 * (float)padding.left) * units) + ((vector33 * (float)padding.top) * units);
		corners[1] = corners[1] + ((vector31 * (float)padding.right) * units) + ((vector33 * (float)padding.top) * units);
		corners[2] = corners[2] + ((vector3 * (float)padding.left) * units) + ((vector32 * (float)padding.bottom) * units);
		return new Plane[] { new Plane(vector3, corners[0]), new Plane(vector31, corners[1]), new Plane(vector32, corners[2]), new Plane(vector33, corners[0]) };
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
	}

	protected internal override void OnLocalize()
	{
		base.OnLocalize();
		this.BackgroundSprite = base.getLocalizedValue(this.backgroundSprite);
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
}