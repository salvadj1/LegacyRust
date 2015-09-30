using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Containers/Tab Control/Tab Page Container")]
[ExecuteInEditMode]
[Serializable]
public class dfTabContainer : dfControl
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected string backgroundSprite;

	[SerializeField]
	protected RectOffset padding = new RectOffset();

	[SerializeField]
	protected int selectedIndex;

	private PropertyChangedEventHandler<int> SelectedIndexChanged;

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
				this.arrangeTabPages();
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
			if (value != this.selectedIndex)
			{
				this.selectPageByIndex(value);
			}
		}
	}

	public dfTabContainer()
	{
	}

	public dfControl AddTabPage()
	{
		dfPanel _dfPanel = this.controls.Where((dfControl i) => i is dfPanel).FirstOrDefault() as dfPanel;
		string str = string.Concat("Tab Page ", this.controls.Count + 1);
		dfPanel atlas = base.AddControl<dfPanel>();
		atlas.name = str;
		atlas.Atlas = this.Atlas;
		atlas.Anchor = dfAnchorStyle.All;
		atlas.ClipChildren = true;
		if (_dfPanel != null)
		{
			atlas.Atlas = _dfPanel.Atlas;
			atlas.BackgroundSprite = _dfPanel.BackgroundSprite;
		}
		this.arrangeTabPages();
		this.Invalidate();
		return atlas;
	}

	private void arrangeTabPages()
	{
		if (this.padding == null)
		{
			this.padding = new RectOffset(0, 0, 0, 0);
		}
		Vector3 vector3 = new Vector3((float)this.padding.left, (float)this.padding.top);
		Vector2 vector2 = new Vector2(this.size.x - (float)this.padding.horizontal, this.size.y - (float)this.padding.vertical);
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfPanel item = this.controls[i] as dfPanel;
			if (item != null)
			{
				item.Size = vector2;
				item.RelativePosition = vector3;
			}
		}
	}

	private void attachEvents(dfControl control)
	{
		control.IsVisibleChanged += new PropertyChangedEventHandler<bool>(this.control_IsVisibleChanged);
		control.PositionChanged += new PropertyChangedEventHandler<Vector2>(this.childControlInvalidated);
		control.SizeChanged += new PropertyChangedEventHandler<Vector2>(this.childControlInvalidated);
	}

	private void childControlInvalidated(dfControl control, Vector2 value)
	{
		this.onChildControlInvalidatedLayout();
	}

	private void control_IsVisibleChanged(dfControl control, bool value)
	{
		this.onChildControlInvalidatedLayout();
	}

	private void detachEvents(dfControl control)
	{
		control.IsVisibleChanged -= new PropertyChangedEventHandler<bool>(this.control_IsVisibleChanged);
		control.PositionChanged -= new PropertyChangedEventHandler<Vector2>(this.childControlInvalidated);
		control.SizeChanged -= new PropertyChangedEventHandler<Vector2>(this.childControlInvalidated);
	}

	private void onChildControlInvalidatedLayout()
	{
		if (base.IsLayoutSuspended)
		{
			return;
		}
		this.arrangeTabPages();
		this.Invalidate();
	}

	protected internal override void OnControlAdded(dfControl child)
	{
		base.OnControlAdded(child);
		this.attachEvents(child);
		this.arrangeTabPages();
	}

	protected internal override void OnControlRemoved(dfControl child)
	{
		base.OnControlRemoved(child);
		this.detachEvents(child);
		this.arrangeTabPages();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		if (this.size.sqrMagnitude < 1.401298E-45f)
		{
			base.Size = new Vector2(256f, 256f);
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

	protected internal virtual void OnSelectedIndexChanged(int Index)
	{
		base.SignalHierarchy("OnSelectedIndexChanged", new object[] { Index });
		if (this.SelectedIndexChanged != null)
		{
			this.SelectedIndexChanged(this, Index);
		}
	}

	private void selectPageByIndex(int value)
	{
		value = Mathf.Max(Mathf.Min(value, this.controls.Count - 1), -1);
		if (value == this.selectedIndex)
		{
			return;
		}
		this.selectedIndex = value;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfControl item = this.controls[i];
			if (item != null)
			{
				item.IsVisible = i == value;
			}
		}
		this.arrangeTabPages();
		this.Invalidate();
		this.OnSelectedIndexChanged(value);
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