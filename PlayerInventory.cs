using System;
using System.Collections.Generic;
using uLink;
using UnityEngine;

public class PlayerInventory : CraftingInventory, FixedSizeInventory
{
	private const int _storageSpace = 30;

	private const int _beltSpace = 6;

	private const int _equipSpace = 4;

	public const int EquipmentStart = 36;

	public const int EquipmentEnd = 40;

	public const int NumEquipItems = 4;

	public const int BeltStart = 30;

	public const int BeltEnd = 36;

	public const int NumBeltItems = 6;

	public const int StorageStart = 0;

	public const int StorageEnd = 30;

	public const int NumStorageItems = 30;

	private const int TotalSlotCount = 40;

	private List<BlueprintDataBlock> _boundBPs;

	public bool bpDirty = true;

	[NonSerialized]
	private EquipmentWearer _equipmentWearer;

	private new EquipmentWearer equipmentWearer
	{
		get
		{
			EquipmentWearer equipmentWearer;
			if (!this._equipmentWearer)
			{
				EquipmentWearer local = base.GetLocal<EquipmentWearer>();
				EquipmentWearer equipmentWearer1 = local;
				this._equipmentWearer = local;
				equipmentWearer = equipmentWearer1;
			}
			else
			{
				equipmentWearer = this._equipmentWearer;
			}
			return equipmentWearer;
		}
	}

	public int fixedSlotCount
	{
		get
		{
			return 40;
		}
	}

	public PlayerInventory()
	{
	}

	protected override bool CheckSlotFlags(Inventory.SlotFlags itemSlotFlags, Inventory.SlotFlags slotFlags)
	{
		bool flag;
		if (!base.CheckSlotFlags(itemSlotFlags, slotFlags))
		{
			flag = false;
		}
		else
		{
			flag = ((slotFlags & Inventory.SlotFlags.Equip) != Inventory.SlotFlags.Equip ? true : (itemSlotFlags & slotFlags) == slotFlags);
		}
		return flag;
	}

	protected override void ConfigureSlots(int totalCount, ref Inventory.Slot.KindDictionary<Inventory.Slot.Range> ranges, ref Inventory.SlotFlags[] flags)
	{
		if (totalCount != 40)
		{
			Debug.LogError(string.Concat("Invalid size for player inventory ", totalCount), this);
		}
		ranges = PlayerInventory.LateLoaded.SlotRanges;
		flags = PlayerInventory.LateLoaded.EveryPlayerInventory;
		if (base.networkView.isMine)
		{
			this._boundBPs = new List<BlueprintDataBlock>();
		}
	}

	protected override void DoDeactivateItem()
	{
		if (this._activeItem != null)
		{
			IHeldItem heldItem = this._activeItem as IHeldItem;
			if (heldItem != null)
			{
				heldItem.OnDeactivate();
			}
		}
		this._activeItem = null;
		base.DoDeactivateItem();
	}

	protected override void DoSetActiveItem(InventoryItem item)
	{
		InventoryItem inventoryItem = this._activeItem;
		this._activeItem = item;
		if (inventoryItem != null)
		{
			IHeldItem heldItem = inventoryItem.iface as IHeldItem;
			if (heldItem != null)
			{
				heldItem.OnDeactivate();
			}
		}
		if (this._activeItem != null)
		{
			IHeldItem heldItem1 = this._activeItem as IHeldItem;
			if (heldItem1 != null)
			{
				heldItem1.OnActivate();
			}
		}
	}

	public bool GetArmorItem<IArmorItem>(ArmorModelSlot slot, out IArmorItem item)
	where IArmorItem : class, IInventoryItem
	{
		int num;
		IInventoryItem inventoryItem;
		switch (slot)
		{
			case ArmorModelSlot.Feet:
			{
				num = 39;
				break;
			}
			case ArmorModelSlot.Legs:
			{
				num = 38;
				break;
			}
			case ArmorModelSlot.Torso:
			{
				num = 37;
				break;
			}
			case ArmorModelSlot.Head:
			{
				num = 36;
				break;
			}
			default:
			{
				item = (IArmorItem)null;
				return false;
			}
		}
		if (!base.GetItem(num, out inventoryItem))
		{
			item = (IArmorItem)null;
			return false;
		}
		IArmorItem armorItem = (IArmorItem)(inventoryItem as IArmorItem);
		IArmorItem armorItem1 = armorItem;
		item = armorItem;
		return !object.ReferenceEquals(armorItem1, null);
	}

	public List<BlueprintDataBlock> GetBoundBPs()
	{
		return this._boundBPs;
	}

	public static bool IsBeltSlot(int slot)
	{
		return (slot < 30 ? false : slot < 36);
	}

	public static bool IsEquipmentSlot(int slot)
	{
		return (slot < 36 ? false : slot < 40);
	}

	protected override void ItemAdded(int slot, IInventoryItem item)
	{
		if (PlayerInventory.IsEquipmentSlot(slot))
		{
			IEquipmentItem equipmentItem = item as IEquipmentItem;
			if (equipmentItem != null)
			{
				equipmentItem.OnEquipped();
				this.UpdateEquipment();
			}
		}
	}

	protected override void ItemRemoved(int slot, IInventoryItem item)
	{
		if (PlayerInventory.IsEquipmentSlot(slot))
		{
			IEquipmentItem equipmentItem = item as IEquipmentItem;
			if (equipmentItem != null)
			{
				equipmentItem.OnUnEquipped();
				this.UpdateEquipment();
			}
		}
	}

	public bool KnowsBP(BlueprintDataBlock bp)
	{
		return (!bp ? false : this._boundBPs.Contains(bp));
	}

	public void MakeBPsDirty()
	{
		this.bpDirty = true;
	}

	[NGCRPCSkip]
	[RPC]
	public void ReceiveBoundBPs(byte[] data, uLink.NetworkMessageInfo info)
	{
		this._boundBPs = this._boundBPs ?? new List<BlueprintDataBlock>();
		this._boundBPs.Clear();
		uLink.BitStream bitStream = new uLink.BitStream(data, false);
		int num = bitStream.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			ItemDataBlock byUniqueID = DatablockDictionary.GetByUniqueID(bitStream.ReadInt32());
			if (byUniqueID)
			{
				this._boundBPs.Add(byUniqueID as BlueprintDataBlock);
			}
		}
		this.Refresh();
	}

	public override void Refresh()
	{
		InventoryHolder inventoryHolder = base.inventoryHolder;
		if (inventoryHolder)
		{
			inventoryHolder.InventoryModified();
		}
	}

	protected void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		if (info.networkView.isMine)
		{
			base.InitializeThisFixedSizeInventory();
		}
	}

	private void UpdateEquipment()
	{
		EquipmentWearer equipmentWearer = this.equipmentWearer;
		if (equipmentWearer)
		{
			equipmentWearer.EquipmentUpdate();
		}
	}

	private static class LateLoaded
	{
		public readonly static Inventory.SlotFlags[] EveryPlayerInventory;

		public static Inventory.Slot.KindDictionary<Inventory.Slot.Range> SlotRanges;

		static LateLoaded()
		{
			PlayerInventory.LateLoaded.EveryPlayerInventory = new Inventory.SlotFlags[40];
			for (int i = 0; i < 40; i++)
			{
				Inventory.SlotFlags slotFlag = (Inventory.SlotFlags)0;
				if (PlayerInventory.IsBeltSlot(i))
				{
					slotFlag = slotFlag | Inventory.SlotFlags.Belt;
				}
				if (i == 30)
				{
					slotFlag = slotFlag | Inventory.SlotFlags.Safe;
				}
				if (PlayerInventory.IsEquipmentSlot(i))
				{
					slotFlag = slotFlag | Inventory.SlotFlags.Equip;
					switch (i)
					{
						case 36:
						{
							slotFlag = slotFlag | Inventory.SlotFlags.Head;
							break;
						}
						case 37:
						{
							slotFlag = slotFlag | Inventory.SlotFlags.Chest;
							break;
						}
						case 38:
						{
							slotFlag = slotFlag | Inventory.SlotFlags.Legs;
							break;
						}
						case 39:
						{
							slotFlag = slotFlag | Inventory.SlotFlags.Feet;
							break;
						}
					}
				}
				PlayerInventory.LateLoaded.EveryPlayerInventory[i] = slotFlag;
			}
			PlayerInventory.LateLoaded.SlotRanges[Inventory.Slot.Kind.Default] = new Inventory.Slot.Range(0, 30);
			PlayerInventory.LateLoaded.SlotRanges[Inventory.Slot.Kind.Belt] = new Inventory.Slot.Range(30, 6);
			PlayerInventory.LateLoaded.SlotRanges[Inventory.Slot.Kind.Armor] = new Inventory.Slot.Range(36, 4);
		}
	}
}