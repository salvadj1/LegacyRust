using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Containers/Tab Control/Tab Strip")]
[ExecuteInEditMode]
[Serializable]
public class dfTabstrip : dfControl
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected string backgroundSprite;

	[SerializeField]
	protected RectOffset layoutPadding = new RectOffset();

	[SerializeField]
	protected Vector2 scrollPosition = Vector2.zero;

	[SerializeField]
	protected int selectedIndex;

	[SerializeField]
	protected dfTabContainer pageContainer;

	[SerializeField]
	protected bool allowKeyboardNavigation = true;

	private PropertyChangedEventHandler<int> SelectedIndexChanged;

	public bool AllowKeyboardNavigation
	{
		get
		{
			return this.allowKeyboardNavigation;
		}
		set
		{
			this.allowKeyboardNavigation = value;
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
				this.Invalidate();
			}
		}
	}

	public RectOffset LayoutPadding
	{
		get
		{
			if (this.layoutPadding == null)
			{
				this.layoutPadding = new RectOffset();
			}
			return this.layoutPadding;
		}
		set
		{
			value = value.ConstrainPadding();
			if (!object.Equals(value, this.layoutPadding))
			{
				this.layoutPadding = value;
				this.arrangeTabs();
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
				this.selectTabByIndex(value);
			}
		}
	}

	public dfTabContainer TabPages
	{
		get
		{
			return this.pageContainer;
		}
		set
		{
			if (this.pageContainer != value)
			{
				this.pageContainer = value;
				if (value != null)
				{
					while (value.Controls.Count < this.controls.Count)
					{
						value.AddTabPage();
					}
				}
				this.pageContainer.SelectedIndex = this.SelectedIndex;
				this.Invalidate();
			}
		}
	}

	public dfTabstrip()
	{
	}

	public dfControl AddTab(string Text = "")
	{
		dfButton _dfButton = this.controls.Where((dfControl i) => i is dfButton).FirstOrDefault() as dfButton;
		string str = string.Concat("Tab ", this.controls.Count + 1);
		if (string.IsNullOrEmpty(Text))
		{
			Text = str;
		}
		dfButton atlas = base.AddControl<dfButton>();
		atlas.name = str;
		atlas.Atlas = this.Atlas;
		atlas.Text = Text;
		atlas.ButtonGroup = this;
		if (_dfButton != null)
		{
			atlas.Atlas = _dfButton.Atlas;
			atlas.Font = _dfButton.Font;
			atlas.AutoSize = _dfButton.AutoSize;
			atlas.Size = _dfButton.Size;
			atlas.BackgroundSprite = _dfButton.BackgroundSprite;
			atlas.DisabledSprite = _dfButton.DisabledSprite;
			atlas.FocusSprite = _dfButton.FocusSprite;
			atlas.HoverSprite = _dfButton.HoverSprite;
			atlas.PressedSprite = _dfButton.PressedSprite;
			atlas.Shadow = _dfButton.Shadow;
			atlas.ShadowColor = _dfButton.ShadowColor;
			atlas.ShadowOffset = _dfButton.ShadowOffset;
			atlas.TextColor = _dfButton.TextColor;
			atlas.TextAlignment = _dfButton.TextAlignment;
			RectOffset padding = _dfButton.Padding;
			atlas.Padding = new RectOffset(padding.left, padding.right, padding.top, padding.bottom);
		}
		if (this.pageContainer != null)
		{
			this.pageContainer.AddTabPage();
		}
		this.arrangeTabs();
		this.Invalidate();
		return atlas;
	}

	private void arrangeTabs()
	{
		this.SuspendLayout();
		try
		{
			this.layoutPadding = this.layoutPadding.ConstrainPadding();
			float single = (float)this.layoutPadding.left - this.scrollPosition.x;
			float single1 = (float)this.layoutPadding.top - this.scrollPosition.y;
			float single2 = 0f;
			float single3 = 0f;
			for (int i = 0; i < base.Controls.Count; i++)
			{
				dfControl item = this.controls[i];
				if (item.IsVisible && item.enabled && item.gameObject.activeSelf)
				{
					item.RelativePosition = new Vector2(single, single1);
					float width = item.Width + (float)this.layoutPadding.horizontal;
					float height = item.Height + (float)this.layoutPadding.vertical;
					single2 = Mathf.Max(width, single2);
					single3 = Mathf.Max(height, single3);
					single = single + width;
				}
			}
		}
		finally
		{
			this.ResumeLayout();
		}
	}

	private void attachEvents(dfControl control)
	{
		control.IsVisibleChanged += new PropertyChangedEventHandler<bool>(this.control_IsVisibleChanged);
		control.PositionChanged += new PropertyChangedEventHandler<Vector2>(this.childControlInvalidated);
		control.SizeChanged += new PropertyChangedEventHandler<Vector2>(this.childControlInvalidated);
		control.ZOrderChanged += new PropertyChangedEventHandler<int>(this.childControlZOrderChanged);
	}

	private void childControlInvalidated(dfControl control, Vector2 value)
	{
		this.onChildControlInvalidatedLayout();
	}

	private void childControlZOrderChanged(dfControl control, int value)
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

	public void DisableTab(int index)
	{
		if (this.selectedIndex >= 0 && this.selectedIndex <= this.controls.Count - 1)
		{
			this.controls[index].Disable();
		}
	}

	public void EnableTab(int index)
	{
		if (this.selectedIndex >= 0 && this.selectedIndex <= this.controls.Count - 1)
		{
			this.controls[index].Enable();
		}
	}

	private void onChildControlInvalidatedLayout()
	{
		if (base.IsLayoutSuspended)
		{
			return;
		}
		this.arrangeTabs();
		this.Invalidate();
	}

	protected internal override void OnClick(dfMouseEventArgs args)
	{
		if (this.controls.Contains(args.Source))
		{
			this.SelectedIndex = args.Source.ZOrder;
		}
		base.OnClick(args);
	}

	private void OnClick(dfControl sender, dfMouseEventArgs args)
	{
		if (!this.controls.Contains(args.Source))
		{
			return;
		}
		this.SelectedIndex = args.Source.ZOrder;
	}

	protected internal override void OnControlAdded(dfControl child)
	{
		base.OnControlAdded(child);
		this.attachEvents(child);
		this.arrangeTabs();
	}

	protected internal override void OnControlRemoved(dfControl child)
	{
		base.OnControlRemoved(child);
		this.detachEvents(child);
		this.arrangeTabs();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		if (this.size.sqrMagnitude < 1.401298E-45f)
		{
			base.Size = new Vector2(256f, 26f);
		}
		if (Application.isPlaying)
		{
			this.selectTabByIndex(Mathf.Max(this.selectedIndex, 0));
		}
	}

	protected internal override void OnGotFocus(dfFocusEventArgs args)
	{
		if (this.controls.Contains(args.GotFocus))
		{
			this.SelectedIndex = args.GotFocus.ZOrder;
		}
		base.OnGotFocus(args);
	}

	protected internal override void OnKeyDown(dfKeyEventArgs args)
	{
		if (args.Used)
		{
			return;
		}
		if (this.allowKeyboardNavigation)
		{
			if (args.KeyCode == KeyCode.LeftArrow || args.KeyCode == KeyCode.Tab && args.Shift)
			{
				this.SelectedIndex = Mathf.Max(0, this.SelectedIndex - 1);
				args.Use();
				return;
			}
			if (args.KeyCode == KeyCode.RightArrow || args.KeyCode == KeyCode.Tab)
			{
				dfTabstrip selectedIndex = this;
				selectedIndex.SelectedIndex = selectedIndex.SelectedIndex + 1;
				args.Use();
				return;
			}
		}
		base.OnKeyDown(args);
	}

	protected internal override void OnLostFocus(dfFocusEventArgs args)
	{
		base.OnLostFocus(args);
		if (this.controls.Contains(args.LostFocus))
		{
			this.showSelectedTab();
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

	protected internal virtual void OnSelectedIndexChanged()
	{
		base.SignalHierarchy("OnSelectedIndexChanged", new object[] { this.SelectedIndex });
		if (this.SelectedIndexChanged != null)
		{
			this.SelectedIndexChanged(this, this.SelectedIndex);
		}
	}

	private void selectTabByIndex(int value)
	{
		value = Mathf.Max(Mathf.Min(value, this.controls.Count - 1), -1);
		if (value == this.selectedIndex)
		{
			return;
		}
		this.selectedIndex = value;
		for (int i = 0; i < this.controls.Count; i++)
		{
			dfButton item = this.controls[i] as dfButton;
			if (item != null)
			{
				if (i != value)
				{
					item.State = dfButton.ButtonState.Default;
				}
				else
				{
					item.State = dfButton.ButtonState.Focus;
				}
			}
		}
		this.Invalidate();
		this.OnSelectedIndexChanged();
		if (this.pageContainer != null)
		{
			this.pageContainer.SelectedIndex = value;
		}
	}

	private void showSelectedTab()
	{
		if (this.selectedIndex >= 0 && this.selectedIndex <= this.controls.Count - 1)
		{
			dfButton item = this.controls[this.selectedIndex] as dfButton;
			if (item != null && !item.ContainsMouse)
			{
				item.State = dfButton.ButtonState.Focus;
			}
		}
	}

	public override void Update()
	{
		base.Update();
		if (this.isControlInvalidated)
		{
			this.arrangeTabs();
		}
		this.showSelectedTab();
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