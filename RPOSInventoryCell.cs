using Facepunch;
using System;
using UnityEngine;

public class RPOSInventoryCell : UnityEngine.MonoBehaviour
{
	public UISprite _amountBackground;

	public UILabel _stackLabel;

	public UILabel _usesLabel;

	public UILabel _numberLabel;

	public UITexture _icon;

	public UISlicedSprite _background;

	public UISprite _darkener;

	private Color backupColor = Color.cyan;

	public Inventory _displayInventory;

	public byte _mySlot;

	public IInventoryItem _myDisplayItem;

	public static Material _myMaterial;

	private bool _locked;

	public UISprite[] modSprites;

	private UIAtlas.Sprite mod_empty;

	private UIAtlas.Sprite mod_full;

	private bool dragging;

	private RPOSInventoryCell lastLanding;

	private bool startedNoItem;

	public IInventoryItem slotItem
	{
		get
		{
			IInventoryItem inventoryItem;
			if (this._displayInventory && this._displayInventory.GetItem((int)this._mySlot, out inventoryItem))
			{
				return inventoryItem;
			}
			return null;
		}
	}

	static RPOSInventoryCell()
	{
	}

	public RPOSInventoryCell()
	{
	}

	public bool IsItemLocked()
	{
		return this._locked;
	}

	private void MakeEmpty()
	{
		this._myDisplayItem = null;
		this._icon.enabled = false;
		this._stackLabel.text = string.Empty;
		this._usesLabel.text = string.Empty;
		if (this._amountBackground)
		{
			this._amountBackground.enabled = false;
		}
		if ((int)this.modSprites.Length > 0)
		{
			for (int i = 0; i < (int)this.modSprites.Length; i++)
			{
				this.modSprites[i].enabled = false;
			}
		}
	}

	private void OnAltClick()
	{
		if (this.slotItem != null)
		{
			RPOS.GetRightClickMenu().SetItem(this.slotItem);
		}
	}

	private void OnAltLand(GameObject landing)
	{
		RPOSInventoryCell component = landing.GetComponent<RPOSInventoryCell>();
		if (!component)
		{
			return;
		}
		RPOS.ItemCellAltClicked(component);
	}

	private void OnClick()
	{
	}

	private void OnDragState(bool start)
	{
		if (start)
		{
			if (!this.dragging && !this.startedNoItem)
			{
				UICamera.Cursor.DropNotification = DropNotificationFlags.DragLand | DropNotificationFlags.AltLand | DropNotificationFlags.RegularHover | DropNotificationFlags.DragLandOutside;
				this.lastLanding = null;
				this.dragging = true;
				RPOS.Item_CellDragBegin(this);
				UICamera.Cursor.CurrentButton.ClickNotification = UICamera.ClickNotification.BasedOnDelta;
			}
		}
		else if (this.dragging)
		{
			this.dragging = false;
			if (!this.lastLanding)
			{
				RPOS.Item_CellReset();
			}
			else
			{
				this.dragging = false;
				RPOS.Item_CellDragEnd(this, this.lastLanding);
				UICamera.Cursor.Buttons.LeftValue.ClickNotification = UICamera.ClickNotification.None;
			}
		}
	}

	private void OnLand(GameObject landing)
	{
		this.lastLanding = landing.GetComponent<RPOSInventoryCell>();
	}

	private void OnLandOutside()
	{
		if (this._displayInventory.gameObject == RPOS.ObservedPlayer.gameObject)
		{
			RPOS.TossItem(this._mySlot);
		}
	}

	private void OnPress(bool start)
	{
		if (start)
		{
			this.startedNoItem = (this.slotItem == null ? true : this.IsItemLocked());
			if (this.startedNoItem)
			{
				UICamera.Cursor.CurrentButton.ClickNotification = UICamera.ClickNotification.None;
				UICamera.Cursor.DropNotification = (DropNotificationFlags)0;
			}
		}
	}

	private void OnTooltip(bool show)
	{
		IInventoryItem inventoryItem;
		ItemDataBlock itemDataBlock;
		if (!show || this._myDisplayItem == null)
		{
			inventoryItem = null;
		}
		else
		{
			inventoryItem = this._myDisplayItem;
		}
		IInventoryItem inventoryItem1 = inventoryItem;
		if (!show || this._myDisplayItem == null)
		{
			itemDataBlock = null;
		}
		else
		{
			itemDataBlock = this._myDisplayItem.datablock;
		}
		ItemToolTip.SetToolTip(itemDataBlock, inventoryItem1);
	}

	private void SetItem(IInventoryItem item)
	{
		if (item == null)
		{
			this.MakeEmpty();
			return;
		}
		this._myDisplayItem = item;
		if (!item.datablock.IsSplittable())
		{
			this._stackLabel.color = Color.yellow;
			this._stackLabel.text = (item.datablock._maxUses <= item.datablock.GetMinUsesForDisplay() ? string.Empty : item.uses.ToString());
		}
		else
		{
			this._stackLabel.color = Color.white;
			if (item.uses <= 1)
			{
				this._stackLabel.text = string.Empty;
			}
			else
			{
				UILabel uILabel = this._stackLabel;
				int num = item.uses;
				uILabel.text = string.Concat("x", num.ToString());
			}
		}
		if (this._amountBackground)
		{
			if (this._stackLabel.text != string.Empty)
			{
				Vector2 vector2 = this._stackLabel.font.CalculatePrintedSize(this._stackLabel.text, true, UIFont.SymbolStyle.None);
				this._amountBackground.enabled = true;
				Transform vector3 = this._amountBackground.transform;
				float single = vector2.x;
				Vector3 vector31 = this._stackLabel.transform.localScale;
				vector3.localScale = new Vector3(single * vector31.x + 12f, 16f, 1f);
			}
			else
			{
				this._amountBackground.enabled = false;
			}
		}
		if (ItemDataBlock.LoadIconOrUnknown<Texture>(item.datablock.icon, ref item.datablock.iconTex))
		{
			Material material = TextureMaterial.GetMaterial(RPOSInventoryCell._myMaterial, item.datablock.iconTex);
			this._icon.material = (UIMaterial)material;
			this._icon.enabled = true;
		}
		IHeldItem heldItem = item as IHeldItem;
		IHeldItem heldItem1 = heldItem;
		int num1 = (heldItem != null ? heldItem1.totalModSlots : 0);
		int num2 = (num1 != 0 ? heldItem1.usedModSlots : 0);
		for (int i = 0; i < (int)this.modSprites.Length; i++)
		{
			if (i >= num1)
			{
				this.modSprites[i].enabled = false;
			}
			else
			{
				this.modSprites[i].enabled = true;
				this.modSprites[i].sprite = (i >= num2 ? this.mod_empty : this.mod_full);
				this.modSprites[i].spriteName = this.modSprites[i].sprite.name;
			}
		}
		if (item.IsBroken())
		{
			this._icon.color = Color.red;
		}
		else if (item.condition / item.maxcondition > 0.4f)
		{
			this._icon.color = Color.white;
		}
		else
		{
			this._icon.color = Color.yellow;
		}
	}

	public void SetItemLocked(bool locked)
	{
		this._locked = locked;
		if (!this._locked)
		{
			this._icon.color = Color.white;
		}
		else
		{
			this._icon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		}
	}

	private void Start()
	{
		if (!RPOSInventoryCell._myMaterial)
		{
			Bundling.Load<Material>("content/item/mat/ItemIconShader", out RPOSInventoryCell._myMaterial);
		}
		this._icon.enabled = false;
		if ((int)this.modSprites.Length > 0)
		{
			this.mod_empty = this.modSprites[0].atlas.GetSprite("slot_empty");
			this.mod_full = this.modSprites[0].atlas.GetSprite("slot_full");
		}
	}

	private void Update()
	{
		IInventoryItem inventoryItem;
		if (this._displayInventory)
		{
			if (!RPOS.Item_IsClickedCell(this))
			{
				this._displayInventory.GetItem((int)this._mySlot, out inventoryItem);
				if (this._displayInventory.MarkSlotClean((int)this._mySlot) || !object.ReferenceEquals(this._myDisplayItem, inventoryItem))
				{
					this.SetItem(inventoryItem);
				}
			}
			else
			{
				this.MakeEmpty();
			}
			if (!RPOS.IsOpen && this._darkener)
			{
				if (this.backupColor == Color.cyan)
				{
					this.backupColor = this._darkener.color;
				}
				if (this._myDisplayItem == null || this._displayInventory._activeItem != this._myDisplayItem)
				{
					this._darkener.color = this.backupColor;
				}
				else
				{
					this._darkener.color = Color.grey;
				}
			}
		}
	}
}