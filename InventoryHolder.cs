using Facepunch;
using System;
using uLink;
using UnityEngine;

public class InventoryHolder : IDLocalCharacter
{
	private const string TossItem_RPC = "TOSS";

	[NonSerialized]
	private CacheRef<Inventory> _inventory;

	[NonSerialized]
	private ItemRepresentation itemRep;

	[NonSerialized]
	private string _animationGroupNameCached;

	[NonSerialized]
	private ulong lastItemUseTime;

	[NonSerialized]
	private bool hasItem;

	[NonSerialized]
	private bool isPlayerInventory;

	public string animationGroupName
	{
		get
		{
			return this._animationGroupNameCached;
		}
	}

	public bool hasItemRepresentation
	{
		get
		{
			return this.hasItem;
		}
	}

	public IInventoryItem inputItem
	{
		get
		{
			IInventoryItem inventoryItem;
			Inventory inventory = this.inventory;
			if (!inventory)
			{
				inventoryItem = null;
			}
			else
			{
				inventoryItem = inventory.activeItem;
			}
			return inventoryItem;
		}
	}

	public Inventory inventory
	{
		get
		{
			if (!this._inventory.cached)
			{
				this._inventory = base.GetLocal<Inventory>();
			}
			return this._inventory.@value;
		}
	}

	public ItemRepresentation itemRepresentation
	{
		get
		{
			return this.itemRep;
		}
	}

	public ItemModFlags modFlags
	{
		get
		{
			if (this.hasItem && this.itemRep)
			{
				return this.itemRep.modFlags;
			}
			IHeldItem heldItem = this.inputItem as IHeldItem;
			if (object.ReferenceEquals(heldItem, null))
			{
				return ItemModFlags.Other;
			}
			return heldItem.modFlags;
		}
	}

	public InventoryHolder()
	{
	}

	public bool BeltUse(int beltNum)
	{
		PlayerInventory playerInventory;
		IInventoryItem inventoryItem;
		if (base.dead)
		{
			return false;
		}
		if (this.GetPlayerInventory(out playerInventory) && playerInventory.GetItem(30 + beltNum, out inventoryItem))
		{
			if (inventoryItem is IHeldItem)
			{
				IHeldItem heldItem = (IHeldItem)inventoryItem;
				IHeldItem heldItem1 = heldItem;
				if ((!heldItem.active ? !heldItem1.canActivate : !heldItem1.canDeactivate))
				{
					return false;
				}
			}
			if (this.ValidateAntiBeltSpam(NetCull.timeInMillis))
			{
				base.networkView.RPC<int>("DoBeltUse", uLink.RPCMode.Server, beltNum);
				return true;
			}
		}
		return false;
	}

	internal void ClearItemRepresentation(ItemRepresentation value)
	{
		if (this.hasItem && this.itemRep == value)
		{
			this.itemRep = null;
			this.hasItem = false;
			this._animationGroupNameCached = null;
		}
	}

	[RPC]
	protected void DoBeltUse(int beltNum)
	{
	}

	private bool GetPlayerInventory(out PlayerInventory inventory)
	{
		inventory = this.inventory as PlayerInventory;
		if (!inventory)
		{
			inventory = null;
			return false;
		}
		inventory = (PlayerInventory)this.inventory;
		return inventory;
	}

	public void InventoryModified()
	{
		if (base.localControlled)
		{
			RPOS.LocalInventoryModified();
		}
	}

	public void InvokeInputItemPostFrame(object item, ref HumanController.InputSample sample)
	{
		IHeldItem heldItem = item as IHeldItem;
		if (heldItem != null)
		{
			heldItem.ItemPostFrame(ref sample);
		}
	}

	public object InvokeInputItemPreFrame(ref HumanController.InputSample sample)
	{
		IHeldItem heldItem = this.inputItem as IHeldItem;
		if (heldItem != null)
		{
			heldItem.ItemPreFrame(ref sample);
		}
		return heldItem;
	}

	public void InvokeInputItemPreRender()
	{
		IHeldItem heldItem = this.inputItem as IHeldItem;
		if (heldItem != null)
		{
			heldItem.PreCameraRender();
		}
	}

	internal void SetItemRepresentation(ItemRepresentation value)
	{
		if (this.itemRep != value)
		{
			this.itemRep = value;
			this.hasItem = this.itemRep;
			if (!this.hasItem)
			{
				this._animationGroupNameCached = null;
			}
			else
			{
				this._animationGroupNameCached = this.itemRep.worldAnimationGroupName;
				if (this._animationGroupNameCached != null && this._animationGroupNameCached.Length == 1)
				{
					this._animationGroupNameCached = null;
				}
			}
		}
	}

	[NGCRPCSkip]
	[RPC]
	protected void TOSS(uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
	}

	public bool TossItem(int slot)
	{
		IInventoryItem inventoryItem;
		Facepunch.NetworkView networkView = base.networkView;
		if (!networkView || !networkView.isMine)
		{
			return false;
		}
		Inventory inventory = this.inventory;
		if (!inventory || !inventory.GetItem(slot, out inventoryItem))
		{
			return false;
		}
		NetCull.RPC<byte>(this, "TOSS", uLink.RPCMode.Server, Inventory.RPCInteger(slot));
		inventory.NULL_SLOT_FIX_ME(slot);
		return true;
	}

	private void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		if (info.networkView.isMine)
		{
			this.inventory.RequestFullUpdate();
		}
	}

	private bool ValidateAntiBeltSpam(ulong timestamp)
	{
		ulong num = NetCull.timeInMillis;
		if (num + (long)800 < this.lastItemUseTime)
		{
			return false;
		}
		this.lastItemUseTime = num;
		return true;
	}
}