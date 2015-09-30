using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfInteractiveBase : dfControl
{
	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected string backgroundSprite;

	[SerializeField]
	protected string hoverSprite;

	[SerializeField]
	protected string disabledSprite;

	[SerializeField]
	protected string focusSprite;

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

	public string DisabledSprite
	{
		get
		{
			return this.disabledSprite;
		}
		set
		{
			if (value != this.disabledSprite)
			{
				this.disabledSprite = value;
				this.Invalidate();
			}
		}
	}

	public string FocusSprite
	{
		get
		{
			return this.focusSprite;
		}
		set
		{
			if (value != this.focusSprite)
			{
				this.focusSprite = value;
				this.Invalidate();
			}
		}
	}

	public string HoverSprite
	{
		get
		{
			return this.hoverSprite;
		}
		set
		{
			if (value != this.hoverSprite)
			{
				this.hoverSprite = value;
				this.Invalidate();
			}
		}
	}

	public dfInteractiveBase()
	{
	}

	public override Vector2 CalculateMinimumSize()
	{
		dfAtlas.ItemInfo backgroundSprite = this.getBackgroundSprite();
		if (backgroundSprite == null)
		{
			return base.CalculateMinimumSize();
		}
		RectOffset rectOffset = backgroundSprite.border;
		if (rectOffset.horizontal <= 0 && rectOffset.vertical <= 0)
		{
			return base.CalculateMinimumSize();
		}
		return Vector2.Max(base.CalculateMinimumSize(), new Vector2((float)rectOffset.horizontal, (float)rectOffset.vertical));
	}

	protected virtual Color32 getActiveColor()
	{
		if (base.IsEnabled)
		{
			return this.color;
		}
		if (!string.IsNullOrEmpty(this.disabledSprite) && this.Atlas != null && this.Atlas[this.DisabledSprite] != null)
		{
			return this.color;
		}
		return this.disabledColor;
	}

	protected internal virtual dfAtlas.ItemInfo getBackgroundSprite()
	{
		if (this.Atlas == null)
		{
			return null;
		}
		if (!base.IsEnabled)
		{
			dfAtlas.ItemInfo item = this.atlas[this.DisabledSprite];
			if (item != null)
			{
				return item;
			}
			return this.atlas[this.BackgroundSprite];
		}
		if (this.HasFocus)
		{
			dfAtlas.ItemInfo itemInfo = this.atlas[this.FocusSprite];
			if (itemInfo != null)
			{
				return itemInfo;
			}
			return this.atlas[this.BackgroundSprite];
		}
		if (this.isMouseHovering)
		{
			dfAtlas.ItemInfo item1 = this.atlas[this.HoverSprite];
			if (item1 != null)
			{
				return item1;
			}
		}
		return this.Atlas[this.BackgroundSprite];
	}

	protected internal override void OnGotFocus(dfFocusEventArgs args)
	{
		base.OnGotFocus(args);
		this.Invalidate();
	}

	protected internal override void OnLostFocus(dfFocusEventArgs args)
	{
		base.OnLostFocus(args);
		this.Invalidate();
	}

	protected internal override void OnMouseEnter(dfMouseEventArgs args)
	{
		base.OnMouseEnter(args);
		this.Invalidate();
	}

	protected internal override void OnMouseLeave(dfMouseEventArgs args)
	{
		base.OnMouseLeave(args);
		this.Invalidate();
	}

	protected internal virtual void renderBackground()
	{
		if (this.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo backgroundSprite = this.getBackgroundSprite();
		if (backgroundSprite == null)
		{
			return;
		}
		Color32 color32 = base.ApplyOpacity(this.getActiveColor());
		dfSprite.RenderOptions renderOption = new dfSprite.RenderOptions();
		dfSprite.RenderOptions upperLeft = renderOption;
		upperLeft.atlas = this.atlas;
		upperLeft.color = color32;
		upperLeft.fillAmount = 1f;
		upperLeft.offset = this.pivot.TransformToUpperLeft(base.Size);
		upperLeft.pixelsToUnits = base.PixelsToUnits();
		upperLeft.size = base.Size;
		upperLeft.spriteInfo = backgroundSprite;
		renderOption = upperLeft;
		if (backgroundSprite.border.horizontal != 0 || backgroundSprite.border.vertical != 0)
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
}