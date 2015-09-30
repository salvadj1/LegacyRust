using Facepunch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using uLink;
using UnityEngine;

[NGCAutoAddScript]
public class Inventory : IDLocal
{
	private const uLink.RPCMode ItemAction_RPCMode = uLink.RPCMode.Server;

	private const string GetNetUpdate_RPC = "GNUP";

	private const string ItemMove_RPC = "ITMV";

	private const string ItemMoveSelf_RPC = "ISMV";

	private const string DoItemAction_RPC = "IACT";

	private const string SetActiveItem_RPC = "IAST";

	private const string DeactivateItem_RPC = "ITDE";

	private const string ConfigureArmor_RPC = "CFAR";

	private const string Server_Request_Inventory_Update_Full = "SVUF";

	private const string MergeItems_RPC = "ITMG";

	private const string MergeItemsSelf_RPC = "ITSM";

	private const string SplitStack_RPCName = "ITSP";

	private const string Client_ItemEvent = "CLEV";

	private const string Server_Request_Inventory_Update_Cell = "SVUC";

	private const Inventory.SlotOperations SlotOperations_Mask = Inventory.SlotOperations.Stack | Inventory.SlotOperations.Combine | Inventory.SlotOperations.Move;

	private const Inventory.SlotOperations SlotOperations_Operations = Inventory.SlotOperations.Stack | Inventory.SlotOperations.Combine | Inventory.SlotOperations.Move;

	private const Inventory.SlotOperations SlotOperations_Options = 0;

	[NonSerialized]
	public InventoryItem _activeItem;

	[NonSerialized]
	private CacheRef<InventoryHolder> _inventoryHolder;

	[NonSerialized]
	private CacheRef<EquipmentWearer> _equipmentWearer;

	[NonSerialized]
	private Inventory.Slot.KindDictionary<Inventory.Slot.Range> slotRanges;

	[NonSerialized]
	private Inventory.Collection<InventoryItem> _collection_;

	[NonSerialized]
	private Inventory.SlotFlags[] _slotFlags;

	[NonSerialized]
	private ArmorModelMemberMap<ArmorDataBlock> lastNetworkedArmorDatablocks;

	[NonSerialized]
	private bool _collection_made_;

	[NonSerialized]
	private bool _locked;

	public IInventoryItem activeItem
	{
		get
		{
			IInventoryItem inventoryItem;
			if (this._activeItem != null)
			{
				inventoryItem = this._activeItem.iface;
			}
			else
			{
				inventoryItem = null;
			}
			return inventoryItem;
		}
	}

	public bool anyOccupiedSlots
	{
		get
		{
			return this.collection.HasAnyOccupant;
		}
	}

	public bool anyVacantSlots
	{
		get
		{
			return this.collection.HasVacancy;
		}
	}

	private Inventory.Collection<InventoryItem> collection
	{
		get
		{
			if (!this._collection_made_)
			{
				return Inventory.Collection<InventoryItem>.Default.Empty;
			}
			return this._collection_;
		}
	}

	public float? craftingCompletePercent
	{
		get
		{
			CraftingInventory craftingInventory = this as CraftingInventory;
			if (craftingInventory)
			{
				return craftingInventory.craftingCompletePercent;
			}
			return null;
		}
	}

	public float? craftingSecondsRemaining
	{
		get
		{
			CraftingInventory craftingInventory = this as CraftingInventory;
			if (craftingInventory)
			{
				return craftingInventory.craftingSecondsRemaining;
			}
			return null;
		}
	}

	public float craftingSpeed
	{
		get
		{
			CraftingInventory craftingInventory = this as CraftingInventory;
			if (craftingInventory == null)
			{
				return 0f;
			}
			return craftingInventory.craftingSpeedPerSec;
		}
	}

	public int dirtySlotCount
	{
		get
		{
			return this.collection.DirtyCount;
		}
	}

	public EquipmentWearer equipmentWearer
	{
		get
		{
			if (!this._equipmentWearer.cached)
			{
				this._equipmentWearer = base.GetLocal<EquipmentWearer>();
			}
			return this._equipmentWearer.@value;
		}
	}

	protected InventoryItem firstInventoryItem
	{
		get
		{
			InventoryItem inventoryItem;
			if (this.collection.GetByOrder(0, out inventoryItem))
			{
				return inventoryItem;
			}
			return null;
		}
	}

	public IInventoryItem firstItem
	{
		get
		{
			InventoryItem inventoryItem;
			if (!this.collection.GetByOrder(0, out inventoryItem))
			{
				return null;
			}
			return inventoryItem.iface;
		}
	}

	protected HumanController hackyNeedToFixHumanControllGetValue
	{
		get
		{
			HumanController humanController;
			Character character = this.idMain as Character;
			if (!character)
			{
				humanController = null;
			}
			else
			{
				humanController = character.controller as HumanController;
			}
			return humanController;
		}
	}

	public bool initialized
	{
		get
		{
			return this._collection_made_;
		}
	}

	public InventoryHolder inventoryHolder
	{
		get
		{
			if (!this._inventoryHolder.cached)
			{
				this._inventoryHolder = base.GetLocal<InventoryHolder>();
			}
			return this._inventoryHolder.@value;
		}
	}

	public bool isCrafting
	{
		get
		{
			CraftingInventory craftingInventory = this as CraftingInventory;
			return (!craftingInventory ? false : craftingInventory.isCrafting);
		}
	}

	public bool isCraftingInventory
	{
		get
		{
			return this is CraftingInventory;
		}
	}

	public bool locked
	{
		get
		{
			return this._locked;
		}
		set
		{
			this._locked = value;
		}
	}

	public bool noOccupiedSlots
	{
		get
		{
			return this.collection.HasNoOccupant;
		}
	}

	public bool noVacantSlots
	{
		get
		{
			return this.collection.HasNoVacancy;
		}
	}

	public Inventory.OccupiedIterator occupiedIterator
	{
		get
		{
			return new Inventory.OccupiedIterator(this);
		}
	}

	public Inventory.OccupiedReverseIterator occupiedReverseIterator
	{
		get
		{
			return new Inventory.OccupiedReverseIterator(this);
		}
	}

	public int occupiedSlotCount
	{
		get
		{
			return this.collection.OccupiedCount;
		}
	}

	public int slotCount
	{
		get
		{
			return this.collection.Capacity;
		}
	}

	public Inventory.VacantIterator vacantIterator
	{
		get
		{
			return new Inventory.VacantIterator(this);
		}
	}

	public int vacantSlotCount
	{
		get
		{
			return this.collection.VacantCount;
		}
	}

	public Inventory()
	{
	}

	public Inventory.AddExistingItemResult AddExistingItem(IInventoryItem iitem, bool forbidStacking)
	{
		return this.AddExistingItem(iitem, forbidStacking, false);
	}

	private Inventory.AddExistingItemResult AddExistingItem(IInventoryItem iitem, bool forbidStacking, bool mustBeUnassigned)
	{
		InventoryItem inventoryItem = iitem as InventoryItem;
		if (object.ReferenceEquals(inventoryItem, null) || mustBeUnassigned && inventoryItem.inventory)
		{
			return Inventory.AddExistingItemResult.BadItemArgument;
		}
		ItemDataBlock itemDataBlock = inventoryItem.datablock;
		Inventory.Addition addition = new Inventory.Addition();
		Inventory.Addition addition1 = addition;
		addition1.ItemDataBlock = itemDataBlock;
		addition1.UsesQuantity = inventoryItem.uses;
		addition1.SlotPreference = Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, (forbidStacking ? false : itemDataBlock.IsSplittable()), Inventory.Slot.Kind.Belt);
		addition = addition1;
		Inventory.Payload.Opt opt = Inventory.Payload.Opt.IgnoreSlotOffset | Inventory.Payload.Opt.ReuseItem;
		if (forbidStacking)
		{
			opt = (Inventory.Payload.Opt)((byte)(opt | Inventory.Payload.Opt.DoNotStack));
		}
		Inventory.Payload.Result result = this.AssignItem(ref addition, opt, inventoryItem);
		if ((byte)(result.flags & Inventory.Payload.Result.Flags.Complete) != 128)
		{
			if ((byte)(result.flags & Inventory.Payload.Result.Flags.Stacked) != 32)
			{
				return Inventory.AddExistingItemResult.Failed;
			}
			inventoryItem.SetUses(result.usesRemaining);
			return Inventory.AddExistingItemResult.PartiallyStacked;
		}
		if ((byte)(result.flags & Inventory.Payload.Result.Flags.AssignedInstance) == 64)
		{
			return Inventory.AddExistingItemResult.Moved;
		}
		if ((byte)(result.flags & Inventory.Payload.Result.Flags.Stacked) == 32)
		{
			inventoryItem.SetUses(0);
			return Inventory.AddExistingItemResult.CompletlyStacked;
		}
		UnityEngine.Debug.LogWarning("unhandled", this);
		return Inventory.AddExistingItemResult.Failed;
	}

	public IInventoryItem AddItem(ItemDataBlock datablock, Inventory.Slot.Preference slot, Inventory.Uses.Quantity uses)
	{
		Datablock.Ident ident = (Datablock.Ident)datablock;
		return this.AddItem(ref ident, slot, uses);
	}

	public IInventoryItem AddItem(ref Datablock.Ident ident, Inventory.Slot.Preference slot, Inventory.Uses.Quantity uses)
	{
		Inventory.Addition addition = new Inventory.Addition();
		Inventory.Addition addition1 = addition;
		addition1.ItemDataBlock = (ItemDataBlock)ident.datablock;
		addition1.SlotPreference = slot;
		addition1.UsesQuantity = uses;
		addition = addition1;
		return this.AddItem(ref addition);
	}

	public IInventoryItem AddItem(ref Inventory.Addition itemAdd)
	{
		return this.AddItem(ref itemAdd, 0, null);
	}

	private IInventoryItem AddItem(ref Inventory.Addition addition, Inventory.Payload.Opt flags, InventoryItem reuse)
	{
		Inventory.Payload.Result result = this.AssignItem(ref addition, flags, reuse);
		return Inventory.ResultToItem(ref result, flags);
	}

	public int AddItemAmount(ItemDataBlock datablock, int amount, Inventory.AmountMode mode, Inventory.Uses.Quantity perNonSplittableItemUseQuantity)
	{
		Inventory.Slot.Preference? nullable = null;
		return this.AddItemAmount(datablock, amount, mode, new Inventory.Uses.Quantity?(perNonSplittableItemUseQuantity), nullable);
	}

	public int AddItemAmount(ref Datablock.Ident ident, int amount, Inventory.AmountMode mode, Inventory.Uses.Quantity perNonSplittableItemUseQuantity)
	{
		Inventory.Slot.Preference? nullable = null;
		return this.AddItemAmount((ItemDataBlock)ident.datablock, amount, mode, new Inventory.Uses.Quantity?(perNonSplittableItemUseQuantity), nullable);
	}

	public int AddItemAmount(ItemDataBlock datablock, int amount, Inventory.AmountMode mode)
	{
		return this.AddItemAmount(datablock, amount, mode, null, null);
	}

	public int AddItemAmount(ref Datablock.Ident ident, int amount, Inventory.AmountMode mode)
	{
		Inventory.Uses.Quantity? nullable = null;
		Inventory.Slot.Preference? nullable1 = null;
		return this.AddItemAmount((ItemDataBlock)ident.datablock, amount, mode, nullable, nullable1);
	}

	public int AddItemAmount(ItemDataBlock datablock, int amount, Inventory.Uses.Quantity perNonSplittableItemUseQuantity)
	{
		Inventory.Slot.Preference? nullable = null;
		return this.AddItemAmount(datablock, amount, Inventory.AmountMode.Default, new Inventory.Uses.Quantity?(perNonSplittableItemUseQuantity), nullable);
	}

	public int AddItemAmount(ref Datablock.Ident ident, int amount, Inventory.Uses.Quantity perNonSplittableItemUseQuantity)
	{
		Inventory.Slot.Preference? nullable = null;
		return this.AddItemAmount((ItemDataBlock)ident.datablock, amount, Inventory.AmountMode.Default, new Inventory.Uses.Quantity?(perNonSplittableItemUseQuantity), nullable);
	}

	public int AddItemAmount(ItemDataBlock datablock, int amount)
	{
		return this.AddItemAmount(datablock, amount, Inventory.AmountMode.Default, null, null);
	}

	public int AddItemAmount(ref Datablock.Ident ident, int amount)
	{
		Inventory.Uses.Quantity? nullable = null;
		Inventory.Slot.Preference? nullable1 = null;
		return this.AddItemAmount((ItemDataBlock)ident.datablock, amount, Inventory.AmountMode.Default, nullable, nullable1);
	}

	public int AddItemAmount(ItemDataBlock datablock, int amount, Inventory.AmountMode mode, Inventory.Uses.Quantity perNonSplittableItemUseQuantity, Inventory.Slot.Preference slotPref)
	{
		return this.AddItemAmount(datablock, amount, mode, new Inventory.Uses.Quantity?(perNonSplittableItemUseQuantity), new Inventory.Slot.Preference?(slotPref));
	}

	public int AddItemAmount(ref Datablock.Ident ident, int amount, Inventory.AmountMode mode, Inventory.Uses.Quantity perNonSplittableItemUseQuantity, Inventory.Slot.Preference slotPref)
	{
		return this.AddItemAmount((ItemDataBlock)ident.datablock, amount, mode, new Inventory.Uses.Quantity?(perNonSplittableItemUseQuantity), new Inventory.Slot.Preference?(slotPref));
	}

	public int AddItemAmount(ItemDataBlock datablock, int amount, Inventory.AmountMode mode, Inventory.Slot.Preference slotPref)
	{
		Inventory.Uses.Quantity? nullable = null;
		return this.AddItemAmount(datablock, amount, mode, nullable, new Inventory.Slot.Preference?(slotPref));
	}

	public int AddItemAmount(ref Datablock.Ident ident, int amount, Inventory.AmountMode mode, Inventory.Slot.Preference slotPref)
	{
		Inventory.Uses.Quantity? nullable = null;
		return this.AddItemAmount((ItemDataBlock)ident.datablock, amount, mode, nullable, new Inventory.Slot.Preference?(slotPref));
	}

	public int AddItemAmount(ItemDataBlock datablock, int amount, Inventory.Uses.Quantity perNonSplittableItemUseQuantity, Inventory.Slot.Preference slotPref)
	{
		return this.AddItemAmount(datablock, amount, Inventory.AmountMode.Default, new Inventory.Uses.Quantity?(perNonSplittableItemUseQuantity), new Inventory.Slot.Preference?(slotPref));
	}

	public int AddItemAmount(ref Datablock.Ident ident, int amount, Inventory.Uses.Quantity perNonSplittableItemUseQuantity, Inventory.Slot.Preference slotPref)
	{
		return this.AddItemAmount((ItemDataBlock)ident.datablock, amount, Inventory.AmountMode.Default, new Inventory.Uses.Quantity?(perNonSplittableItemUseQuantity), new Inventory.Slot.Preference?(slotPref));
	}

	public int AddItemAmount(ItemDataBlock datablock, int amount, Inventory.Slot.Preference slotPref)
	{
		Inventory.Uses.Quantity? nullable = null;
		return this.AddItemAmount(datablock, amount, Inventory.AmountMode.Default, nullable, new Inventory.Slot.Preference?(slotPref));
	}

	public int AddItemAmount(ref Datablock.Ident ident, int amount, Inventory.Slot.Preference slotPref)
	{
		Inventory.Uses.Quantity? nullable = null;
		return this.AddItemAmount((ItemDataBlock)ident.datablock, amount, Inventory.AmountMode.Default, nullable, new Inventory.Slot.Preference?(slotPref));
	}

	private int AddItemAmount(ItemDataBlock datablock, int amount, Inventory.AmountMode mode, Inventory.Uses.Quantity? perNonSplittableItemQuantity, Inventory.Slot.Preference? slotPref)
	{
		Inventory.AddMultipleItemFlags addMultipleItemFlag;
		Inventory.Uses.Quantity quantity;
		if (!datablock)
		{
			return amount;
		}
		if (!datablock.IsSplittable())
		{
			if (mode == Inventory.AmountMode.OnlyStack)
			{
				return amount;
			}
			addMultipleItemFlag = Inventory.AddMultipleItemFlags.MustBeNonSplittable;
			quantity = (!perNonSplittableItemQuantity.HasValue ? Inventory.Uses.Quantity.Random : perNonSplittableItemQuantity.Value);
		}
		else
		{
			addMultipleItemFlag = Inventory.AddMultipleItemFlags.MustBeSplittable;
			switch (mode)
			{
				case Inventory.AmountMode.OnlyStack:
				{
					addMultipleItemFlag = addMultipleItemFlag | Inventory.AddMultipleItemFlags.DoNotCreateNewSplittableStacks;
					break;
				}
				case Inventory.AmountMode.OnlyCreateNew:
				{
					addMultipleItemFlag = addMultipleItemFlag | Inventory.AddMultipleItemFlags.DoNotStackSplittables;
					break;
				}
				case Inventory.AmountMode.IgnoreSplittables:
				{
					return amount;
				}
			}
			quantity = new Inventory.Uses.Quantity();
		}
		return this.AddMultipleItems(datablock, amount, quantity, addMultipleItemFlag, slotPref);
	}

	public void AddItems(Inventory.Addition[] itemAdds)
	{
		for (int i = 0; i < (int)itemAdds.Length; i++)
		{
			this.AddItem(ref itemAdds[i]);
		}
	}

	public IInventoryItem AddItemSomehow(ItemDataBlock item, Inventory.Slot.Kind? slotKindPref, int slotOffset, int usesCount)
	{
		IInventoryItem inventoryItem;
		if (!item || usesCount <= 0 && item.IsSplittable())
		{
			inventoryItem = null;
		}
		else
		{
			inventoryItem = this.AddItemSomehowWork(item, slotKindPref, slotOffset, usesCount);
		}
		return inventoryItem;
	}

	private IInventoryItem AddItemSomehowWork(ItemDataBlock item, Inventory.Slot.Kind? slotKindPref, int slotOffset, int usesCount)
	{
		Inventory.Slot.Kind value;
		int num;
		bool slotForKind;
		bool flag;
		Inventory.Addition addition = new Inventory.Addition();
		if (!slotKindPref.HasValue)
		{
			num = slotOffset;
			bool slotKind = this.GetSlotKind(num, out value, out slotOffset);
			slotForKind = slotKind;
			flag = slotKind;
		}
		else
		{
			value = slotKindPref.Value;
			slotForKind = this.GetSlotForKind(value, slotOffset, out num);
			flag = (slotForKind ? true : this.HasSlotsOfKind(value));
		}
		addition.Ident = (Datablock.Ident)item;
		addition.UsesQuantity = usesCount;
		if (flag)
		{
			if (slotForKind)
			{
				addition.SlotPreference = Inventory.Slot.Preference.Define(value, slotOffset);
				Inventory.Payload.Result result = this.AssignItem(ref addition, Inventory.Payload.Opt.RestrictToOffset, null);
				if ((byte)(result.flags & Inventory.Payload.Result.Flags.Complete) == 128)
				{
					return Inventory.ResultToItem(ref result, Inventory.Payload.Opt.RestrictToOffset);
				}
				if ((byte)(result.flags & Inventory.Payload.Result.Flags.Stacked) == 32)
				{
					int num1 = result.usesRemaining;
					usesCount = num1;
					addition.UsesQuantity = num1;
				}
			}
			addition.SlotPreference = value;
			Inventory.Payload.Result result1 = this.AssignItem(ref addition, 0, null);
			if ((byte)(result1.flags & Inventory.Payload.Result.Flags.Complete) == 128)
			{
				return Inventory.ResultToItem(ref result1, 0);
			}
			if ((byte)(result1.flags & Inventory.Payload.Result.Flags.Stacked) == 32)
			{
				int num2 = result1.usesRemaining;
				usesCount = num2;
				addition.UsesQuantity = num2;
			}
		}
		else if (num >= 0 && num < this.slotCount)
		{
			addition.SlotPreference = Inventory.Slot.Preference.Define(num);
			Inventory.Payload.Result result2 = this.AssignItem(ref addition, Inventory.Payload.Opt.RestrictToOffset, null);
			if ((byte)(result2.flags & Inventory.Payload.Result.Flags.Complete) == 128)
			{
				return Inventory.ResultToItem(ref result2, Inventory.Payload.Opt.RestrictToOffset);
			}
			if ((byte)(result2.flags & Inventory.Payload.Result.Flags.Stacked) == 32)
			{
				int num3 = result2.usesRemaining;
				usesCount = num3;
				addition.UsesQuantity = num3;
			}
		}
		Inventory.Slot.KindFlags kindFlag = Inventory.Slot.KindFlags.Default | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Armor;
		if (flag)
		{
			kindFlag = (Inventory.Slot.KindFlags)((byte)((byte)kindFlag & (byte)(~(byte)((byte)Inventory.Slot.Kind.Belt << (byte)(value & (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor))))));
		}
		addition.SlotPreference = Inventory.Slot.Preference.Define(kindFlag);
		return this.AddItem(ref addition);
	}

	private int AddMultipleItems(ItemDataBlock itemDB, int usesOrItemCountWhenNotSplittable, Inventory.Uses.Quantity nonSplittableUses, Inventory.AddMultipleItemFlags amif, Inventory.Slot.Preference? slotPreference)
	{
		bool flag;
		int num;
		Inventory.Addition value = new Inventory.Addition();
		Inventory.Addition addition = value;
		addition.ItemDataBlock = itemDB;
		value = addition;
		bool flag1 = itemDB.IsSplittable();
		if ((amif & (Inventory.AddMultipleItemFlags.MustBeSplittable | Inventory.AddMultipleItemFlags.MustBeNonSplittable) | (!flag1 ? Inventory.AddMultipleItemFlags.MustBeNonSplittable : Inventory.AddMultipleItemFlags.MustBeSplittable)) == (Inventory.AddMultipleItemFlags.MustBeSplittable | Inventory.AddMultipleItemFlags.MustBeNonSplittable))
		{
			return usesOrItemCountWhenNotSplittable;
		}
		if (!flag1)
		{
			value.UsesQuantity = nonSplittableUses;
			value.SlotPreference = (!slotPreference.HasValue ? Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, false, Inventory.Slot.Kind.Belt) : slotPreference.Value.CloneStackChange(false));
			while (usesOrItemCountWhenNotSplittable > 0 && (byte)(this.AssignItem(ref value, Inventory.Payload.Opt.DoNotStack | Inventory.Payload.Opt.IgnoreSlotOffset, null).flags & Inventory.Payload.Result.Flags.Complete) == 128)
			{
				usesOrItemCountWhenNotSplittable--;
			}
			return usesOrItemCountWhenNotSplittable;
		}
		if (usesOrItemCountWhenNotSplittable == 0)
		{
			return 0;
		}
		if ((amif & (Inventory.AddMultipleItemFlags.DoNotCreateNewSplittableStacks | Inventory.AddMultipleItemFlags.DoNotStackSplittables)) == (Inventory.AddMultipleItemFlags.DoNotCreateNewSplittableStacks | Inventory.AddMultipleItemFlags.DoNotStackSplittables))
		{
			return usesOrItemCountWhenNotSplittable;
		}
		int num1 = usesOrItemCountWhenNotSplittable / itemDB._maxUses;
		Inventory.Payload.Opt opt = Inventory.Payload.Opt.IgnoreSlotOffset;
		if ((amif & Inventory.AddMultipleItemFlags.DoNotStackSplittables) != Inventory.AddMultipleItemFlags.DoNotStackSplittables)
		{
			flag = false;
			if (!slotPreference.HasValue)
			{
				value.SlotPreference = Inventory.DefaultAddMultipleItemsSlotPreference(true);
			}
			else
			{
				value.SlotPreference = slotPreference.Value;
			}
		}
		else
		{
			flag = true;
			opt = (Inventory.Payload.Opt)((byte)(opt | Inventory.Payload.Opt.DoNotStack));
			if (!slotPreference.HasValue)
			{
				value.SlotPreference = Inventory.DefaultAddMultipleItemsSlotPreference(false);
			}
			else
			{
				value.SlotPreference = slotPreference.Value.CloneStackChange(false);
			}
		}
		if ((amif & Inventory.AddMultipleItemFlags.DoNotCreateNewSplittableStacks) == Inventory.AddMultipleItemFlags.DoNotCreateNewSplittableStacks)
		{
			opt = (Inventory.Payload.Opt)((byte)(opt | Inventory.Payload.Opt.DoNotAssign));
		}
		int num2 = 0;
		if (num1 > 0)
		{
			value.UsesQuantity = itemDB._maxUses;
			do
			{
				Inventory.Payload.Result result = this.AssignItem(ref value, opt, null);
				if ((byte)(result.flags & Inventory.Payload.Result.Flags.Complete) != 128)
				{
					if ((byte)(result.flags & Inventory.Payload.Result.Flags.Stacked) == 32)
					{
						num2 = num2 + (itemDB._maxUses - result.usesRemaining);
					}
					return usesOrItemCountWhenNotSplittable - num2;
				}
				num2 = num2 + itemDB._maxUses;
				if (!flag && (byte)(result.flags & Inventory.Payload.Result.Flags.AssignedInstance) == 64)
				{
					opt = (Inventory.Payload.Opt)((byte)(opt | Inventory.Payload.Opt.DoNotStack));
					flag = true;
				}
				num = num1 - 1;
				num1 = num;
			}
			while (num > 0);
		}
		if (num2 == usesOrItemCountWhenNotSplittable)
		{
			return 0;
		}
		int num3 = usesOrItemCountWhenNotSplittable - num2;
		value.UsesQuantity = num3;
		Inventory.Payload.Result result1 = this.AssignItem(ref value, opt, null);
		if ((byte)(result1.flags & (Inventory.Payload.Result.Flags.Complete | Inventory.Payload.Result.Flags.Stacked)) != 0)
		{
			num2 = num2 + (num3 - result1.usesRemaining);
		}
		return usesOrItemCountWhenNotSplittable - num2;
	}

	private Inventory.Payload.Result AssignItem(ref Inventory.Addition addition, Inventory.Payload.Opt flags, InventoryItem reuse)
	{
		return Inventory.Payload.AddItem(this, ref addition, flags, reuse);
	}

	protected void BindArmorModelsFromArmorDatablockMap(ArmorModelMemberMap<ArmorDataBlock> armorDatablockMap)
	{
		ArmorModel armorModel;
		this.lastNetworkedArmorDatablocks = armorDatablockMap;
		ArmorModelRenderer local = base.GetLocal<ArmorModelRenderer>();
		if (local)
		{
			ArmorModelMemberMap armorModelMemberMaps = new ArmorModelMemberMap();
			for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
			{
				ArmorDataBlock item = armorDatablockMap[i];
				ArmorModelSlot armorModelSlot = i;
				if (!item)
				{
					armorModel = null;
				}
				else
				{
					armorModel = item.GetArmorModel(i);
				}
				armorModelMemberMaps[armorModelSlot] = armorModel;
			}
			local.BindArmorModels(armorModelMemberMaps);
		}
	}

	public int CanConsume(ItemDataBlock db, int useCount, List<int> storeToList)
	{
		int num;
		Inventory.Collection<InventoryItem> collection = this.collection;
		if (useCount <= 0 || !db || collection.HasNoOccupant)
		{
			return 0;
		}
		if (storeToList == null)
		{
			return this.CanConsume(db, useCount);
		}
		int count = storeToList.Count;
		int num1 = 0;
		int num2 = db.uniqueID;
		Inventory.Collection<InventoryItem>.OccupiedCollection.Enumerator occupiedEnumerator = collection.OccupiedEnumerator;
		try
		{
			while (occupiedEnumerator.MoveNext())
			{
				InventoryItem current = occupiedEnumerator.Current;
				if (current.datablockUniqueID != num2)
				{
					continue;
				}
				useCount = useCount - current.uses;
				storeToList.Add(occupiedEnumerator.Slot);
				num1++;
				if (useCount > 0)
				{
					continue;
				}
				num = num1;
				return num;
			}
			if (num1 > 0)
			{
				storeToList.RemoveRange(count, num1);
			}
			return -useCount;
		}
		finally
		{
			((IDisposable)(object)occupiedEnumerator).Dispose();
		}
		return num;
	}

	public int CanConsume(ItemDataBlock db, int useCount)
	{
		int num;
		if (useCount <= 0 || this.collection.HasNoOccupant)
		{
			return 0;
		}
		int num1 = 0;
		int num2 = db.uniqueID;
		Inventory.Collection<InventoryItem>.OccupiedCollection.Enumerator occupiedEnumerator = this.collection.OccupiedEnumerator;
		try
		{
			while (occupiedEnumerator.MoveNext())
			{
				InventoryItem current = occupiedEnumerator.Current;
				if (current.datablockUniqueID != num2)
				{
					continue;
				}
				useCount = useCount - current.uses;
				num1++;
				if (useCount > 0)
				{
					continue;
				}
				num = num1;
				return num;
			}
			return -useCount;
		}
		finally
		{
			((IDisposable)(object)occupiedEnumerator).Dispose();
		}
		return num;
	}

	public bool CanItemFit(IInventoryItem iitem)
	{
		bool flag;
		InventoryItem inventoryItem = iitem as InventoryItem;
		ItemDataBlock itemDataBlock = inventoryItem.datablock;
		if (!itemDataBlock.IsSplittable())
		{
			return this.anyVacantSlots;
		}
		int num = inventoryItem.uses;
		Inventory.Collection<InventoryItem>.OccupiedCollection.Enumerator occupiedEnumerator = this.collection.OccupiedEnumerator;
		try
		{
			while (occupiedEnumerator.MoveNext())
			{
				InventoryItem current = occupiedEnumerator.Current;
				if (current.datablockUniqueID != inventoryItem.datablockUniqueID)
				{
					continue;
				}
				if (current != iitem)
				{
					int num1 = itemDataBlock._maxUses - current.uses;
					if (num1 < num)
					{
						num = num - num1;
					}
					else
					{
						flag = true;
						return flag;
					}
				}
			}
			return false;
		}
		finally
		{
			((IDisposable)(object)occupiedEnumerator).Dispose();
		}
		return flag;
	}

	[RPC]
	protected void CFAR(uLink.BitStream stream)
	{
		ArmorModelMemberMap<ArmorDataBlock> byUniqueID = new ArmorModelMemberMap<ArmorDataBlock>();
		for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			byUniqueID[i] = DatablockDictionary.GetByUniqueID(stream.ReadInt32()) as ArmorDataBlock;
		}
		this.BindArmorModelsFromArmorDatablockMap(byUniqueID);
	}

	protected virtual bool CheckSlotFlags(Inventory.SlotFlags itemSlotFlags, Inventory.SlotFlags slotFlags)
	{
		return true;
	}

	private bool CheckSlotFlagsAgainstSlot(Inventory.SlotFlags itemSlotFlags, int slot)
	{
		return this.CheckSlotFlags(itemSlotFlags, this.GetSlotFlags(slot));
	}

	public void Clear()
	{
		Inventory.Collection<InventoryItem>.OccupiedCollection.ReverseEnumerator occupiedReverseEnumerator = this.collection.OccupiedReverseEnumerator;
		try
		{
			while (occupiedReverseEnumerator.MoveNext())
			{
				this.DeleteItem(occupiedReverseEnumerator.Slot);
			}
		}
		finally
		{
			((IDisposable)(object)occupiedReverseEnumerator).Dispose();
		}
	}

	[RPC]
	protected void CLEV(byte itemEvent, int uniqueID)
	{
		ItemDataBlock byUniqueID = DatablockDictionary.GetByUniqueID(uniqueID);
		if (byUniqueID)
		{
			byUniqueID.OnItemEvent((InventoryItem.ItemEvent)itemEvent);
		}
	}

	protected virtual void ConfigureSlots(int totalCount, ref Inventory.Slot.KindDictionary<Inventory.Slot.Range> ranges, ref Inventory.SlotFlags[] flags)
	{
	}

	public void DeactivateItem()
	{
		this.DoDeactivateItem();
	}

	private static Inventory.Slot.Preference DefaultAddMultipleItemsSlotPreference(bool stack)
	{
		return Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, stack, Inventory.Slot.KindFlags.Belt);
	}

	private void DeleteItem(int slot)
	{
		this.RemoveItem(slot);
	}

	protected virtual void DoDeactivateItem()
	{
		this._activeItem = null;
	}

	protected virtual void DoSetActiveItem(InventoryItem item)
	{
		this._activeItem = item;
	}

	public IItemT FindItem<IItemT>()
	where IItemT : class, IInventoryItem
	{
		IItemT itemT;
		Inventory.Collection<InventoryItem>.OccupiedCollection.Enumerator occupiedEnumerator = this.collection.OccupiedEnumerator;
		try
		{
			while (occupiedEnumerator.MoveNext())
			{
				IItemT current = (IItemT)(occupiedEnumerator.Current.iface as IItemT);
				if (object.ReferenceEquals(current, null))
				{
					continue;
				}
				itemT = current;
				return itemT;
			}
			return (IItemT)null;
		}
		finally
		{
			((IDisposable)(object)occupiedEnumerator).Dispose();
		}
		return itemT;
	}

	public IInventoryItem FindItem(string itemDBName)
	{
		return this.FindItem(DatablockDictionary.GetByName(itemDBName));
	}

	public IInventoryItem FindItem(ItemDataBlock itemDB)
	{
		int num = 0;
		return this.FindItem(itemDB, out num);
	}

	public IInventoryItem FindItem(ItemDataBlock itemDB, out int totalNum)
	{
		IInventoryItem inventoryItem;
		bool flag = false;
		InventoryItem inventoryItem1 = null;
		int num = 0;
		int num1 = -1;
		int num2 = itemDB.uniqueID;
		Inventory.Collection<InventoryItem>.OccupiedCollection.Enumerator occupiedEnumerator = this.collection.OccupiedEnumerator;
		try
		{
			while (occupiedEnumerator.MoveNext())
			{
				InventoryItem current = occupiedEnumerator.Current;
				if (current.datablockUniqueID != num2)
				{
					continue;
				}
				int num3 = current.uses;
				if (!flag || num3 > num1)
				{
					inventoryItem1 = current;
					num1 = num3;
					flag = true;
				}
				num = num + num3;
			}
		}
		finally
		{
			((IDisposable)(object)occupiedEnumerator).Dispose();
		}
		totalNum = num;
		if (!flag)
		{
			inventoryItem = null;
		}
		else
		{
			inventoryItem = inventoryItem1.iface;
		}
		return inventoryItem;
	}

	[DebuggerHidden]
	public IEnumerable<IItemT> FindItems<IItemT>()
	where IItemT : class, IInventoryItem
	{
		Inventory.<FindItems>c__Iterator3B<IItemT> variable = null;
		return variable;
	}

	public T FindItemType<T>()
	where T : class, IInventoryItem
	{
		T t;
		Inventory.Collection<InventoryItem>.OccupiedCollection.Enumerator occupiedEnumerator = this.collection.OccupiedEnumerator;
		try
		{
			while (occupiedEnumerator.MoveNext())
			{
				T current = (T)(occupiedEnumerator.Current.iface as T);
				if (object.ReferenceEquals(current, null))
				{
					continue;
				}
				t = current;
				return t;
			}
			return (T)null;
		}
		finally
		{
			((IDisposable)(object)occupiedEnumerator).Dispose();
		}
		return t;
	}

	public Inventory.Transfer[] GenerateOptimizedInventoryListing(Inventory.Slot.KindFlags fallbackPlacement)
	{
		Inventory.Transfer[] transferArray;
		Inventory.Collection<InventoryItem> collection = this.collection;
		if (collection.HasNoOccupant)
		{
			return new Inventory.Transfer[0];
		}
		try
		{
			Inventory.Report.Begin();
			Inventory.Collection<InventoryItem>.OccupiedCollection.Enumerator occupiedEnumerator = collection.OccupiedEnumerator;
			try
			{
				while (occupiedEnumerator.MoveNext())
				{
					Inventory.Report.Take(occupiedEnumerator.Current);
				}
			}
			finally
			{
				((IDisposable)(object)occupiedEnumerator).Dispose();
			}
			transferArray = Inventory.Report.Build(fallbackPlacement);
		}
		finally
		{
			Inventory.Report.Recover();
		}
		return transferArray;
	}

	public Inventory.Transfer[] GenerateOptimizedInventoryListing(Inventory.Slot.KindFlags fallbackPlacement, bool randomize)
	{
		Inventory.Transfer[] transferArray = this.GenerateOptimizedInventoryListing(fallbackPlacement);
		if (randomize && (int)transferArray.Length > 0)
		{
			Inventory.Shuffle.Array<Inventory.Transfer>(transferArray);
			for (int i = 0; i < (int)transferArray.Length; i++)
			{
				transferArray[i].addition.SlotPreference = transferArray[i].addition.SlotPreference.CloneOffsetChange(i);
			}
		}
		return transferArray;
	}

	public bool GetItem(int slot, out IInventoryItem item)
	{
		InventoryItem inventoryItem;
		if (!this._collection_made_ || !this._collection_.Get(slot, out inventoryItem))
		{
			item = null;
			return false;
		}
		item = inventoryItem.iface;
		return true;
	}

	protected bool GetItem(int slot, out InventoryItem item)
	{
		if (!this._collection_made_)
		{
			item = null;
			return false;
		}
		return this._collection_.Get(slot, out item);
	}

	public Inventory.SlotFlags GetSlotFlags(int slot)
	{
		Inventory.SlotFlags slotFlag;
		if (this._slotFlags == null || (int)this._slotFlags.Length <= slot)
		{
			slotFlag = (Inventory.SlotFlags)0;
		}
		else
		{
			slotFlag = this._slotFlags[slot];
		}
		return slotFlag;
	}

	public bool GetSlotForKind(Inventory.Slot.Kind kind, int offset, out int slot)
	{
		Inventory.Slot.Range range;
		if (offset < 0 || !this.slotRanges.TryGetValue(kind, out range) || offset >= range.Count)
		{
			slot = -1;
			return false;
		}
		slot = range.Start + offset;
		return true;
	}

	public bool GetSlotKind(int slot, out Inventory.Slot.Kind kind, out int offset)
	{
		Inventory.Slot.Range range;
		if (slot >= 0 && slot < this.slotCount)
		{
			for (Inventory.Slot.Kind i = Inventory.Slot.Kind.Default; i < (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor); i = (Inventory.Slot.Kind)((byte)i + (byte)Inventory.Slot.Kind.Belt))
			{
				if (this.slotRanges.TryGetValue(i, out range))
				{
					offset = range.GetOffset(slot);
					if (offset != -1)
					{
						kind = i;
						return true;
					}
				}
			}
		}
		kind = Inventory.Slot.Kind.Default;
		offset = -1;
		return false;
	}

	public bool GetSlotsOfKind(Inventory.Slot.Kind kind, out Inventory.Slot.Range range)
	{
		return this.slotRanges.TryGetValue(kind, out range);
	}

	[RPC]
	protected void GNUP(byte[] data, uLink.NetworkMessageInfo info)
	{
		this.OnNetUpdate(new uLink.BitStream(data, false));
		this.Refresh();
	}

	public bool HasSlotsOfKind(Inventory.Slot.Kind kind)
	{
		return this.slotRanges.ContainsKey(kind);
	}

	[NGCRPCSkip]
	[RPC]
	protected void IACT(byte itemIndex, byte action, uLink.NetworkMessageInfo info)
	{
		InventoryItem inventoryItem;
		if (this.collection.Get((int)itemIndex, out inventoryItem))
		{
			inventoryItem.OnMenuOption((InventoryItem.MenuItem)action);
		}
	}

	[NGCRPCSkip]
	[RPC]
	protected void IAST(byte itemIndex, uLink.NetworkViewID itemRepID, uLink.NetworkMessageInfo info)
	{
		ItemRepresentation component;
		byte num = itemIndex;
		if (itemRepID == uLink.NetworkViewID.unassigned)
		{
			component = null;
		}
		else
		{
			component = uLink.NetworkView.Find(itemRepID).GetComponent<ItemRepresentation>();
		}
		this.SetActiveItemManually((int)num, component);
	}

	private void Initialize(int slotCount)
	{
		if (this._collection_made_)
		{
			this.Clear();
			this._collection_ = null;
			this._collection_made_ = false;
		}
		this._slotFlags = Inventory.Empty.SlotFlags;
		this._collection_ = new Inventory.Collection<InventoryItem>(slotCount);
		this._collection_made_ = true;
		this.slotRanges = new Inventory.Slot.KindDictionary<Inventory.Slot.Range>();
		this.slotRanges[Inventory.Slot.Kind.Default] = new Inventory.Slot.Range(0, slotCount);
		this.ConfigureSlots(slotCount, ref this.slotRanges, ref this._slotFlags);
		this._collection_.MarkCompletelyDirty();
	}

	protected bool InitializeThisFixedSizeInventory()
	{
		FixedSizeInventory fixedSizeInventory = this as FixedSizeInventory;
		if (object.ReferenceEquals(fixedSizeInventory, null))
		{
			return false;
		}
		int num = fixedSizeInventory.fixedSlotCount;
		if (this._collection_made_)
		{
			if (this._collection_.Capacity == num)
			{
				return false;
			}
			UnityEngine.Debug.LogError(string.Concat("Some how this inventory was already inititalized to a different size. It will be reinitialized. the original off size was ", this._collection_.Capacity), this);
		}
		this.Initialize(num);
		return true;
	}

	[RPC]
	protected void ISMV(byte fromSlot, byte toSlot, uLink.NetworkMessageInfo info)
	{
	}

	public bool IsSlotDirty(int slot)
	{
		return this.collection.IsDirty(slot);
	}

	public bool IsSlotFree(int slot)
	{
		return this.collection.IsVacant(slot);
	}

	public bool IsSlotOccupied(int slot)
	{
		return this.collection.IsOccupied(slot);
	}

	public bool IsSlotOffsetValid(Inventory.Slot.Kind kind, int offset)
	{
		int num;
		return this.GetSlotForKind(kind, offset, out num);
	}

	public bool IsSlotVacant(int slot)
	{
		return this.collection.IsVacant(slot);
	}

	public bool IsSlotWithinRange(int slot)
	{
		return this.collection.IsWithinRange(slot);
	}

	[NGCRPCSkip]
	[RPC]
	protected void ITDE(uLink.NetworkMessageInfo info)
	{
		this.DeactivateItem();
	}

	protected virtual void ItemAdded(int slot, IInventoryItem item)
	{
		FireBarrel local = base.GetLocal<FireBarrel>();
		if (local)
		{
			local.InvItemAdded();
		}
	}

	public Inventory.SlotOperationResult ItemCombinePredicted(NetEntityID toInvID, int fromSlot, int toSlot)
	{
		return this.ItemMergeRPCPred(toInvID, fromSlot, toSlot, true);
	}

	public Inventory.SlotOperationResult ItemCombinePredicted(Inventory toInventory, int fromSlot, int toSlot)
	{
		return this.ItemCombinePredicted(NetEntityID.Get(toInventory), fromSlot, toSlot);
	}

	public Inventory.SlotOperationResult ItemCombinePredicted(int fromSlot, int toSlot)
	{
		return this.ItemMergeRPCPred(fromSlot, toSlot, true);
	}

	public static Inventory.SlotOperationResult ItemCombinePredicted(NetEntityID fromInvID, NetEntityID toInvID, int fromSlot, int toSlot)
	{
		Inventory component = fromInvID.GetComponent<Inventory>();
		if (!component)
		{
			return Inventory.SlotOperationResult.Success_Combined | Inventory.SlotOperationResult.Error_OccupiedDestination | Inventory.SlotOperationResult.Error_MissingInventory;
		}
		return component.ItemCombinePredicted(toInvID, fromSlot, toSlot);
	}

	public Inventory.SlotOperationResult ItemMergePredicted(NetEntityID toInvID, int fromSlot, int toSlot)
	{
		return this.ItemMergeRPCPred(toInvID, fromSlot, toSlot, false);
	}

	public Inventory.SlotOperationResult ItemMergePredicted(Inventory toInventory, int fromSlot, int toSlot)
	{
		return this.ItemMergePredicted(NetEntityID.Get(toInventory), fromSlot, toSlot);
	}

	public Inventory.SlotOperationResult ItemMergePredicted(int fromSlot, int toSlot)
	{
		return this.ItemMergeRPCPred(fromSlot, toSlot, false);
	}

	public static Inventory.SlotOperationResult ItemMergePredicted(NetEntityID fromInvID, NetEntityID toInvID, int fromSlot, int toSlot)
	{
		Inventory component = fromInvID.GetComponent<Inventory>();
		if (!component)
		{
			return Inventory.SlotOperationResult.Success_Combined | Inventory.SlotOperationResult.Error_OccupiedDestination | Inventory.SlotOperationResult.Error_MissingInventory;
		}
		return component.ItemMergePredicted(toInvID, fromSlot, toSlot);
	}

	private void ItemMergeRPC(NetEntityID toInvID, int fromSlot, int toSlot, bool tryCombine)
	{
		NetCull.RPC<NetEntityID, byte, byte, bool>(this, "ITMG", uLink.RPCMode.Server, toInvID, (byte)fromSlot, (byte)toSlot, tryCombine);
	}

	private void ItemMergeRPC(int fromSlot, int toSlot, bool tryCombine)
	{
		NetCull.RPC<byte, byte, bool>(this, "ITSM", uLink.RPCMode.Server, (byte)fromSlot, (byte)toSlot, tryCombine);
	}

	private Inventory.SlotOperationResult ItemMergeRPCPred(NetEntityID toInvID, int fromSlot, int toSlot, bool tryCombine)
	{
		Inventory.SlotOperationResult slotOperationResult;
		Inventory component = toInvID.GetComponent<Inventory>();
		if (component != this)
		{
			Inventory.SlotOperationResult slotOperationResult1 = this.SlotOperation(fromSlot, component, toSlot, Inventory.SlotOperationsMerge(tryCombine));
			slotOperationResult = slotOperationResult1;
			if ((int)slotOperationResult1 > 0)
			{
				this.ItemMergeRPC(toInvID, fromSlot, toSlot, tryCombine);
			}
		}
		else
		{
			Inventory.SlotOperationResult slotOperationResult2 = this.SlotOperation(fromSlot, toSlot, Inventory.SlotOperationsMerge(tryCombine));
			slotOperationResult = slotOperationResult2;
			if ((int)slotOperationResult2 > 0)
			{
				this.ItemMergeRPC(fromSlot, toSlot, tryCombine);
			}
		}
		return slotOperationResult;
	}

	private Inventory.SlotOperationResult ItemMergeRPCPred(int fromSlot, int toSlot, bool tryCombine)
	{
		Inventory.SlotOperationResult slotOperationResult = this.SlotOperation(fromSlot, toSlot, Inventory.SlotOperationsMerge(tryCombine));
		Inventory.SlotOperationResult slotOperationResult1 = slotOperationResult;
		if ((int)slotOperationResult > 0)
		{
			this.ItemMergeRPC(fromSlot, toSlot, tryCombine);
		}
		return slotOperationResult1;
	}

	public Inventory.SlotOperationResult ItemMovePredicted(NetEntityID toInvID, int fromSlot, int toSlot)
	{
		return this.ItemMoveRPCPred(toInvID, fromSlot, toSlot);
	}

	public Inventory.SlotOperationResult ItemMovePredicted(Inventory toInventory, int fromSlot, int toSlot)
	{
		return this.ItemMovePredicted(NetEntityID.Get(toInventory), fromSlot, toSlot);
	}

	public Inventory.SlotOperationResult ItemMovePredicted(int fromSlot, int toSlot)
	{
		return this.ItemMovePredicted(fromSlot, toSlot);
	}

	public static Inventory.SlotOperationResult ItemMovePredicted(NetEntityID fromInvID, NetEntityID toInvID, int fromSlot, int toSlot)
	{
		Inventory component = fromInvID.GetComponent<Inventory>();
		if (!component)
		{
			return Inventory.SlotOperationResult.Success_Combined | Inventory.SlotOperationResult.Error_OccupiedDestination | Inventory.SlotOperationResult.Error_MissingInventory;
		}
		return component.ItemMovePredicted(toInvID, fromSlot, toSlot);
	}

	private void ItemMoveRPC(NetEntityID toInvID, int fromSlot, int toSlot)
	{
		NetCull.RPC<NetEntityID, byte, byte>(this, "ITMV", uLink.RPCMode.Server, toInvID, (byte)fromSlot, (byte)toSlot);
	}

	private void ItemMoveRPC(int fromSlot, int toSlot)
	{
		NetCull.RPC<byte, byte>(this, "ISMV", uLink.RPCMode.Server, (byte)fromSlot, (byte)toSlot);
	}

	private Inventory.SlotOperationResult ItemMoveRPCPred(NetEntityID toInvID, int fromSlot, int toSlot)
	{
		Inventory.SlotOperationResult slotOperationResult;
		Inventory component = toInvID.GetComponent<Inventory>();
		if (component != this)
		{
			Inventory.SlotOperationResult slotOperationResult1 = this.SlotOperation(fromSlot, component, toSlot, 4);
			slotOperationResult = slotOperationResult1;
			if ((int)slotOperationResult1 > 0)
			{
				this.ItemMoveRPC(toInvID, fromSlot, toSlot);
			}
		}
		else
		{
			Inventory.SlotOperationResult slotOperationResult2 = this.SlotOperation(fromSlot, toSlot, 4);
			slotOperationResult = slotOperationResult2;
			if ((int)slotOperationResult2 > 0)
			{
				this.ItemMoveRPC(fromSlot, toSlot);
			}
		}
		return slotOperationResult;
	}

	private Inventory.SlotOperationResult ItemMoveRPCPred(int fromSlot, int toSlot)
	{
		Inventory.SlotOperationResult slotOperationResult = this.SlotOperation(fromSlot, toSlot, 4);
		Inventory.SlotOperationResult slotOperationResult1 = slotOperationResult;
		if ((int)slotOperationResult > 0)
		{
			this.ItemMoveRPC(fromSlot, toSlot);
		}
		return slotOperationResult1;
	}

	protected virtual void ItemRemoved(int slot, IInventoryItem item)
	{
		FireBarrel local = base.GetLocal<FireBarrel>();
		if (local)
		{
			local.InvItemRemoved();
		}
	}

	[RPC]
	protected void ITMG(NetEntityID toInvID, byte fromSlot, byte toSlot, bool tryCombine, uLink.NetworkMessageInfo info)
	{
	}

	[RPC]
	protected void ITMV(NetEntityID toInvID, byte fromSlot, byte toSlot, uLink.NetworkMessageInfo info)
	{
	}

	[RPC]
	protected void ITSM(byte fromSlot, byte toSlot, bool tryCombine, uLink.NetworkMessageInfo info)
	{
	}

	[RPC]
	protected void ITSP(byte slotNumber, uLink.NetworkMessageInfo info)
	{
	}

	public bool MarkSlotClean(int slot)
	{
		return this.collection.MarkClean(slot);
	}

	public bool MarkSlotDirty(int slot)
	{
		return this.collection.MarkDirty(slot);
	}

	public bool MoveItemAtSlotToEmptySlot(Inventory toInv, int fromSlot, int toSlot)
	{
		InventoryItem inventoryItem;
		if (!toInv)
		{
			return false;
		}
		if (toInv == this && fromSlot == toSlot)
		{
			return false;
		}
		Inventory.Collection<InventoryItem> collection = this.collection;
		if (collection.HasNoOccupant)
		{
			return false;
		}
		if (!collection.Get(fromSlot, out inventoryItem))
		{
			return false;
		}
		ItemDataBlock itemDataBlock = inventoryItem.datablock;
		Inventory.Addition addition = new Inventory.Addition();
		Inventory.Addition addition1 = addition;
		addition1.ItemDataBlock = itemDataBlock;
		addition1.UsesQuantity = inventoryItem.uses;
		addition1.SlotPreference = Inventory.Slot.Preference.Define(toSlot, itemDataBlock.IsSplittable());
		addition = addition1;
		return !object.ReferenceEquals(toInv.AddItem(ref addition, Inventory.Payload.Opt.DoNotStack | Inventory.Payload.Opt.RestrictToOffset | Inventory.Payload.Opt.ReuseItem, inventoryItem), null);
	}

	public bool NetworkItemAction(int slot, InventoryItem.MenuItem option)
	{
		Facepunch.NetworkView networkView = base.networkView;
		if (!networkView)
		{
			return false;
		}
		networkView.RPC("IACT", uLink.RPCMode.Server, new object[] { (byte)slot, (byte)option });
		return true;
	}

	[Obsolete("This isnt right")]
	public void NULL_SLOT_FIX_ME(int slot)
	{
		this.DeleteItem(slot);
	}

	private void OnNetSlotUpdate(Inventory.Collection<InventoryItem> _collection, int slot, bool occupied, uLink.BitStream invdata)
	{
		InventoryItem inventoryItem;
		if (!occupied)
		{
			this.DeleteItem(slot);
		}
		else
		{
			int num = invdata.ReadInt32();
			bool flag = _collection.Get(slot, out inventoryItem);
			if (flag && inventoryItem.datablockUniqueID != num)
			{
				this.DeleteItem(slot);
				flag = false;
				inventoryItem = null;
			}
			if (!flag)
			{
				Inventory.Addition addition = new Inventory.Addition();
				Inventory.Addition maximum = addition;
				maximum.UniqueID = num;
				maximum.UsesQuantity = Inventory.Uses.Quantity.Maximum;
				maximum.SlotPreference = Inventory.Slot.Preference.Define(slot, false);
				addition = maximum;
				inventoryItem = this.AddItem(ref addition, Inventory.Payload.Opt.DoNotStack | Inventory.Payload.Opt.RestrictToOffset, null) as InventoryItem;
			}
			inventoryItem.Deserialize(invdata);
			if (flag)
			{
				_collection.MarkDirty(slot);
			}
		}
	}

	protected void OnNetUpdate(uLink.BitStream invdata)
	{
		Inventory.Collection<InventoryItem> collection;
		int num;
		int num1 = invdata.ReadByte();
		if (!this._collection_made_)
		{
			this.Initialize(num1);
			collection = this._collection_;
		}
		else
		{
			collection = this._collection_;
		}
		if (num1 != collection.Capacity)
		{
			this.Initialize(num1);
		}
		if (!invdata.ReadBoolean())
		{
			num = invdata.ReadByte();
			int num2 = 0;
			try
			{
				for (int i = 0; i < num; i++)
				{
					num2++;
					bool flag = invdata.ReadBoolean();
					this.OnNetSlotUpdate(collection, (int)invdata.ReadByte(), flag, invdata);
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception, this);
				UnityEngine.Debug.Log(string.Format("numItemsInUpdate = {0}, iterated pos = {1}", num, num2), this);
			}
		}
		else
		{
			num = num1;
			for (int j = 0; j < num; j++)
			{
				this.OnNetSlotUpdate(collection, j, invdata.ReadBoolean(), invdata);
			}
		}
	}

	public virtual void Refresh()
	{
	}

	public bool RemoveItem(int slot)
	{
		return this.RemoveItem(slot, null, false);
	}

	public bool RemoveItem(InventoryItem item)
	{
		if (object.ReferenceEquals(item, null))
		{
			return false;
		}
		if (item.inventory != this)
		{
			return false;
		}
		return this.RemoveItem(item.slot, item, true);
	}

	public bool RemoveItem(IInventoryItem item)
	{
		return this.RemoveItem(item as InventoryItem);
	}

	private bool RemoveItem(int slot, InventoryItem match, bool mustMatch)
	{
		InventoryItem inventoryItem;
		Inventory.Collection<InventoryItem> collection = this.collection;
		if (mustMatch && (!collection.Get(slot, out inventoryItem) || !object.ReferenceEquals(inventoryItem, match)) || !collection.Evict(slot, out inventoryItem))
		{
			return false;
		}
		if (inventoryItem == this._activeItem)
		{
			this.DeactivateItem();
		}
		this.ItemRemoved(slot, inventoryItem.iface);
		this.MarkSlotDirty(slot);
		return true;
	}

	protected void RequestCellUpdate(int cell)
	{
		NetCull.RPC<byte>(this, "SVUC", uLink.RPCMode.Server, Inventory.RPCInteger(cell));
	}

	public void RequestFullUpdate()
	{
		NetCull.RPC(this, "SVUF", uLink.RPCMode.Server);
	}

	public void ResetToReport(Inventory.Transfer[] items)
	{
		if (this._collection_made_)
		{
			this.Clear();
		}
		this.Initialize((int)items.Length);
		for (int i = 0; i < (int)items.Length; i++)
		{
			this.AssignItem(ref items[i].addition, Inventory.Payload.Opt.DoNotStack | Inventory.Payload.Opt.RestrictToOffset | Inventory.Payload.Opt.ReuseItem, items[i].item);
		}
	}

	private static IInventoryItem ResultToItem(ref Inventory.Payload.Result result, Inventory.Payload.Opt flags)
	{
		if ((byte)(result.flags & Inventory.Payload.Result.Flags.AssignedInstance) == 64)
		{
			return result.item.iface;
		}
		if ((byte)(flags & Inventory.Payload.Opt.AllowStackedItemsToBeReturned) != 32)
		{
			return null;
		}
		if ((byte)(result.flags & Inventory.Payload.Result.Flags.Stacked) != 32)
		{
			return null;
		}
		return result.item.iface;
	}

	public static byte RPCInteger(int i)
	{
		return (byte)i;
	}

	public static byte RPCInteger(byte i)
	{
		return i;
	}

	public static byte RPCInteger(uLink.BitStream stream)
	{
		return stream.Read<byte>(new object[0]);
	}

	public void SetActiveItemManually(int itemIndex, ItemRepresentation itemRep)
	{
		IInventoryItem inventoryItem;
		this.GetItem(itemIndex, out inventoryItem);
		((IHeldItem)inventoryItem).itemRepresentation = itemRep;
		this.DoSetActiveItem((InventoryItem)inventoryItem);
	}

	private Inventory.SlotOperationResult SlotOperation(int fromSlot, int toSlot, Inventory.SlotOperationsInfo info)
	{
		return this.SlotOperation(fromSlot, this, toSlot, info);
	}

	private Inventory.SlotOperationResult SlotOperation(int fromSlot, Inventory toInventory, int toSlot, Inventory.SlotOperationsInfo info)
	{
		InventoryItem inventoryItem;
		InventoryItem inventoryItem1;
		InventoryItem.MergeResult mergeResult;
		if ((byte)((Inventory.SlotOperations.Stack | Inventory.SlotOperations.Combine | Inventory.SlotOperations.Move) & info.SlotOperations) == 0)
		{
			return Inventory.SlotOperationResult.Success_Combined | Inventory.SlotOperationResult.Success_Moved | Inventory.SlotOperationResult.Error_OccupiedDestination | Inventory.SlotOperationResult.Error_MissingInventory | Inventory.SlotOperationResult.Error_EmptyDestinationSlot | Inventory.SlotOperationResult.Error_NoOpArgs;
		}
		if (!this || !toInventory)
		{
			return Inventory.SlotOperationResult.Success_Combined | Inventory.SlotOperationResult.Error_OccupiedDestination | Inventory.SlotOperationResult.Error_MissingInventory;
		}
		if (this == toInventory && toSlot == fromSlot)
		{
			return Inventory.SlotOperationResult.Success_Stacked | Inventory.SlotOperationResult.Error_OccupiedDestination | Inventory.SlotOperationResult.Error_SameSlot;
		}
		if (!this.GetItem(fromSlot, out inventoryItem))
		{
			return Inventory.SlotOperationResult.Success_Stacked | Inventory.SlotOperationResult.Success_Combined | Inventory.SlotOperationResult.Error_OccupiedDestination | Inventory.SlotOperationResult.Error_SameSlot | Inventory.SlotOperationResult.Error_MissingInventory | Inventory.SlotOperationResult.Error_EmptySourceSlot;
		}
		if (!toInventory.GetItem(toSlot, out inventoryItem1))
		{
			if ((byte)(Inventory.SlotOperations.Move & info.SlotOperations) == 0)
			{
				return Inventory.SlotOperationResult.Success_Moved | Inventory.SlotOperationResult.Error_OccupiedDestination | Inventory.SlotOperationResult.Error_EmptyDestinationSlot;
			}
			if (!this.MoveItemAtSlotToEmptySlot(toInventory, fromSlot, toSlot))
			{
				return Inventory.SlotOperationResult.Success_Stacked | Inventory.SlotOperationResult.Success_Combined | Inventory.SlotOperationResult.Success_Moved | Inventory.SlotOperationResult.Error_OccupiedDestination | Inventory.SlotOperationResult.Error_SameSlot | Inventory.SlotOperationResult.Error_MissingInventory | Inventory.SlotOperationResult.Error_EmptySourceSlot | Inventory.SlotOperationResult.Error_EmptyDestinationSlot | Inventory.SlotOperationResult.Error_SlotRange | Inventory.SlotOperationResult.Error_NoOpArgs | Inventory.SlotOperationResult.Error_Failed;
			}
			if (this)
			{
				this.MarkSlotDirty(fromSlot);
			}
			if (toInventory)
			{
				toInventory.MarkSlotDirty(toSlot);
			}
			return Inventory.SlotOperationResult.Success_Moved;
		}
		this.MarkSlotDirty(fromSlot);
		toInventory.MarkSlotDirty(toSlot);
		if ((byte)((Inventory.SlotOperations.Stack | Inventory.SlotOperations.Combine) & info.SlotOperations) != 1 || inventoryItem.datablockUniqueID != inventoryItem1.datablockUniqueID)
		{
			mergeResult = ((byte)((Inventory.SlotOperations.Stack | Inventory.SlotOperations.Combine) & info.SlotOperations) == 0 ? InventoryItem.MergeResult.Failed : inventoryItem.iface.TryCombine(inventoryItem1.iface));
		}
		else
		{
			mergeResult = inventoryItem.iface.TryStack(inventoryItem1.iface);
		}
		InventoryItem.MergeResult mergeResult1 = mergeResult;
		if (mergeResult1 == InventoryItem.MergeResult.Merged)
		{
			return Inventory.SlotOperationResult.Success_Stacked;
		}
		if (mergeResult1 == InventoryItem.MergeResult.Combined)
		{
			return Inventory.SlotOperationResult.Success_Combined;
		}
		if ((byte)(Inventory.SlotOperations.Move & info.SlotOperations) == 4)
		{
			return -8;
		}
		return Inventory.SlotOperationResult.NoOp;
	}

	private static Inventory.SlotOperations SlotOperationsMerge(bool tryCombine)
	{
		return (tryCombine ? Inventory.SlotOperations.Stack | Inventory.SlotOperations.Combine : Inventory.SlotOperations.Stack);
	}

	public bool SplitStack(int slotNumber)
	{
		InventoryItem inventoryItem;
		if (this.GetItem(slotNumber, out inventoryItem))
		{
			int num = inventoryItem.uses;
			if (num > 1 && this.anyVacantSlots && inventoryItem.datablock.IsSplittable())
			{
				int num1 = num / 2;
				int num2 = num1 - this.AddItemAmount(inventoryItem.datablock, num1, Inventory.AmountMode.OnlyCreateNew);
				if (num2 > 0)
				{
					num = num - num2;
					inventoryItem.SetUses(num);
					NetCull.RPC<byte>(this, "ITSP", uLink.RPCMode.Server, (byte)slotNumber);
					return true;
				}
			}
		}
		return false;
	}

	[RPC]
	protected void SVUC(byte cell, uLink.NetworkMessageInfo info)
	{
	}

	[RPC]
	protected void SVUF(uLink.NetworkMessageInfo info)
	{
	}

	public IngredientList<ItemDataBlock> ToIngredientList()
	{
		Inventory.Collection<InventoryItem> collection = this.collection;
		ItemDataBlock[] current = new ItemDataBlock[collection.OccupiedCount];
		Inventory.Collection<InventoryItem>.OccupiedCollection.Enumerator occupiedEnumerator = collection.OccupiedEnumerator;
		try
		{
			int num = 0;
			while (occupiedEnumerator.MoveNext())
			{
				int num1 = num;
				num = num1 + 1;
				current[num1] = occupiedEnumerator.Current.datablock;
			}
			Array.Resize<ItemDataBlock>(ref current, num);
		}
		finally
		{
			((IDisposable)(object)occupiedEnumerator).Dispose();
		}
		return new IngredientList<ItemDataBlock>(current);
	}

	public enum AddExistingItemResult
	{
		CompletlyStacked,
		Moved,
		PartiallyStacked,
		Failed,
		BadItemArgument
	}

	public struct Addition
	{
		public Datablock.Ident Ident;

		public Inventory.Uses.Quantity UsesQuantity;

		public Inventory.Slot.Preference SlotPreference;

		public ItemDataBlock ItemDataBlock
		{
			get
			{
				return (ItemDataBlock)this.Ident.datablock;
			}
			set
			{
				this.Ident = (Datablock.Ident)value;
			}
		}

		public string Name
		{
			get
			{
				string str;
				ItemDataBlock itemDataBlock = this.ItemDataBlock;
				if (!itemDataBlock)
				{
					str = null;
				}
				else
				{
					str = itemDataBlock.name;
				}
				return str;
			}
			set
			{
				this.Ident = value;
			}
		}

		public int UniqueID
		{
			get
			{
				ItemDataBlock itemDataBlock = this.ItemDataBlock;
				return (!itemDataBlock ? 0 : itemDataBlock.uniqueID);
			}
			set
			{
				this.Ident = value;
			}
		}
	}

	[Flags]
	private enum AddMultipleItemFlags
	{
		MustBeNonSplittable = 1,
		MustBeSplittable = 2,
		DoNotCreateNewSplittableStacks = 4,
		DoNotStackSplittables = 8
	}

	public enum AmountMode
	{
		Default,
		OnlyStack,
		OnlyCreateNew,
		IgnoreSplittables
	}

	private sealed class Collection<T>
	{
		[NonSerialized]
		private Inventory.Collection<T>.OccupiedCollection occupiedCollection;

		[NonSerialized]
		private Inventory.Collection<T>.VacantCollection vacantCollection;

		[NonSerialized]
		private T[] array;

		[NonSerialized]
		private byte[] indices;

		[NonSerialized]
		private Inventory.Mask occupied;

		[NonSerialized]
		private Inventory.Mask dirty;

		[NonSerialized]
		private int count;

		[NonSerialized]
		private int capacity;

		[NonSerialized]
		private int countDirty;

		[NonSerialized]
		private bool forcedDirty;

		public bool AnyVacantOrOccupied
		{
			get
			{
				return this.capacity > 0;
			}
		}

		public int Capacity
		{
			get
			{
				return this.capacity;
			}
		}

		public bool CompletelyDirty
		{
			get
			{
				return (this.countDirty != this.capacity ? false : this.capacity > 0);
			}
		}

		public int DirtyCount
		{
			get
			{
				return this.countDirty;
			}
		}

		public int FirstOccupied
		{
			get
			{
				if (this.count <= 0)
				{
					return -1;
				}
				return this.indices[0];
			}
		}

		public int FirstVacancy
		{
			get
			{
				if (this.count == this.capacity)
				{
					return -1;
				}
				for (int i = 0; i < 256; i++)
				{
					if (!this.occupied[i])
					{
						return i;
					}
				}
				throw new InvalidOperationException();
			}
		}

		public bool ForcedDirty
		{
			get
			{
				return this.forcedDirty;
			}
			set
			{
				if (value != this.forcedDirty && this.capacity > 0)
				{
					this.forcedDirty = value;
				}
			}
		}

		public bool HasAnyOccupant
		{
			get
			{
				return this.count > 0;
			}
		}

		public bool HasNoOccupant
		{
			get
			{
				return this.count == 0;
			}
		}

		public bool HasNoVacancy
		{
			get
			{
				return this.count == this.capacity;
			}
		}

		public bool HasVacancy
		{
			get
			{
				return this.count < this.capacity;
			}
		}

		public bool IsCompletelyVacant
		{
			get
			{
				return (this.count != 0 ? false : this.capacity > 0);
			}
		}

		public int LastOccupied
		{
			get
			{
				if (this.count <= 0)
				{
					return -1;
				}
				return this.indices[this.count - 1];
			}
		}

		public bool MarkedDirty
		{
			get
			{
				return (this.forcedDirty ? true : this.countDirty > 0);
			}
		}

		public Inventory.Collection<T>.OccupiedCollection Occupied
		{
			get
			{
				Inventory.Collection<T>.OccupiedCollection ts = this.occupiedCollection;
				if (ts == null)
				{
					Inventory.Collection<T>.OccupiedCollection ts1 = new Inventory.Collection<T>.OccupiedCollection(this);
					Inventory.Collection<T>.OccupiedCollection ts2 = ts1;
					this.occupiedCollection = ts1;
					ts = ts2;
				}
				return ts;
			}
		}

		public int OccupiedCount
		{
			get
			{
				return this.count;
			}
		}

		public Inventory.Collection<T>.OccupiedCollection.Enumerator OccupiedEnumerator
		{
			get
			{
				return new Inventory.Collection<T>.OccupiedCollection.Enumerator(this);
			}
		}

		public Inventory.Collection<T>.OccupiedCollection.ReverseEnumerator OccupiedReverseEnumerator
		{
			get
			{
				return new Inventory.Collection<T>.OccupiedCollection.ReverseEnumerator(this);
			}
		}

		public Inventory.Collection<T>.VacantCollection Vacant
		{
			get
			{
				Inventory.Collection<T>.VacantCollection ts = this.vacantCollection;
				if (ts == null)
				{
					Inventory.Collection<T>.VacantCollection ts1 = new Inventory.Collection<T>.VacantCollection(this);
					Inventory.Collection<T>.VacantCollection ts2 = ts1;
					this.vacantCollection = ts1;
					ts = ts2;
				}
				return ts;
			}
		}

		public int VacantCount
		{
			get
			{
				return this.capacity - this.count;
			}
		}

		public Inventory.Collection<T>.VacantCollection.Enumerator VacantEnumerator
		{
			get
			{
				return new Inventory.Collection<T>.VacantCollection.Enumerator(this);
			}
		}

		public Collection(int Capacity)
		{
			if (Capacity < 0 || Capacity > 256)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.capacity = Capacity;
			this.count = 0;
			this.array = new T[Capacity];
			this.indices = new byte[Capacity];
		}

		public bool Clean(out Inventory.Mask dirtyMask, out int numDirty)
		{
			return this.Clean(out dirtyMask, out numDirty, false);
		}

		public bool Clean(out Inventory.Mask dirtyMask, out int numDirty, bool dontActuallyClean)
		{
			if (this.countDirty <= 0)
			{
				dirtyMask = new Inventory.Mask();
				numDirty = 0;
				if (!this.forcedDirty)
				{
					return false;
				}
				if (!dontActuallyClean)
				{
					this.forcedDirty = false;
				}
				return true;
			}
			dirtyMask = this.dirty;
			numDirty = this.countDirty;
			if (!dontActuallyClean)
			{
				this.dirty = new Inventory.Mask();
				this.countDirty = 0;
				this.forcedDirty = false;
			}
			return true;
		}

		public void Contract()
		{
			this.Contract(new Inventory.Slot.Range(0, this.capacity));
		}

		public void Contract(Inventory.Slot.Range range)
		{
			int num;
			int start = range.Start;
			int count = start + range.Count;
			if (start < 0 || count > this.capacity)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (this.count == this.capacity || start == count)
			{
				return;
			}
			for (int i = 0; i < this.count; i++)
			{
				if (this.indices[i] >= start)
				{
					if (this.indices[i] >= count)
					{
						break;
					}
					else
					{
						do
						{
							int num1 = start;
							start = num1 + 1;
							int num2 = num1;
							if (num2 != this.indices[i])
							{
								this.array[num2] = this.array[this.indices[i]];
								this.array[this.indices[i]] = default(T);
								if (this.dirty.On((int)this.indices[i]))
								{
									Inventory.Collection<T> collection = this;
									collection.countDirty = collection.countDirty + 1;
								}
								this.indices[i] = (byte)num2;
								if (this.dirty.On(i))
								{
									Inventory.Collection<T> collection1 = this;
									collection1.countDirty = collection1.countDirty + 1;
								}
								if (start == count)
								{
									break;
								}
							}
							num = i + 1;
							i = num;
						}
						while (num < this.count && this.indices[i] < count);
					}
				}
			}
		}

		private bool DoReplace(bool equalityCheck, int slot, T value, out T replacedValue)
		{
			replacedValue = this.array[slot];
			if (equalityCheck && object.Equals(replacedValue, value))
			{
				return false;
			}
			this.array[slot] = value;
			if (this.dirty.On(slot))
			{
				Inventory.Collection<T> collection = this;
				collection.countDirty = collection.countDirty + 1;
			}
			return true;
		}

		private void DoSet(int slot, T value)
		{
			if (this.count == 0 || this.indices[0] > slot)
			{
				int num = this.count;
				for (int i = this.count - 1; i >= 0; i--)
				{
					this.indices[num] = this.indices[i];
					num--;
				}
				this.indices[0] = (byte)slot;
			}
			else
			{
				int num1 = this.count - 1;
				while (num1 >= 0)
				{
					if (this.indices[num1] <= slot)
					{
						this.indices[num1 + 1] = (byte)slot;
						break;
					}
					else
					{
						this.indices[num1 + 1] = this.indices[num1];
						num1--;
					}
				}
			}
			this.array[slot] = value;
			Inventory.Collection<T> collection = this;
			collection.count = collection.count + 1;
			if (this.dirty.On(slot))
			{
				Inventory.Collection<T> collection1 = this;
				collection1.countDirty = collection1.countDirty + 1;
			}
		}

		public bool Evict(int slot, out T value)
		{
			if (slot < 0 || slot >= this.capacity)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (!this.occupied.Off(slot))
			{
				value = default(T);
				return false;
			}
			for (int i = 0; i < this.count; i++)
			{
				if (this.indices[i] == slot)
				{
					for (int j = i + 1; j < this.count; j++)
					{
						this.indices[i] = this.indices[j];
						i++;
					}
					Inventory.Collection<T> collection = this;
					int num = collection.count - 1;
					int num1 = num;
					collection.count = num;
					this.indices[num1] = 0;
					value = this.array[slot];
					this.array[slot] = default(T);
					if (this.dirty.On(slot))
					{
						Inventory.Collection<T> collection1 = this;
						collection1.countDirty = collection1.countDirty + 1;
					}
					return true;
				}
			}
			throw new InvalidOperationException();
		}

		public bool Get(int slot, out T value)
		{
			if (slot < 0 || slot >= this.capacity)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (!this.occupied[slot])
			{
				value = default(T);
				return false;
			}
			value = this.array[slot];
			return true;
		}

		public bool GetByOrder(int index, out T value)
		{
			if (index >= this.count)
			{
				value = default(T);
				return false;
			}
			value = this.array[this.indices[index]];
			return true;
		}

		public bool IsDirty(int slot)
		{
			return (slot < 0 || slot >= this.capacity ? false : this.dirty[slot]);
		}

		public bool IsOccupied(int slot)
		{
			return (slot < 0 || slot >= this.capacity ? false : this.occupied[slot]);
		}

		public bool IsVacant(int slot)
		{
			return (slot < 0 || slot >= this.capacity ? false : !this.occupied[slot]);
		}

		public bool IsWithinRange(int slot)
		{
			return (slot < 0 ? false : slot < this.capacity);
		}

		public bool MarkClean(int slot)
		{
			if (slot < 0 || slot >= this.capacity || !this.dirty.Off(slot))
			{
				return false;
			}
			Inventory.Collection<T> collection = this;
			collection.countDirty = collection.countDirty - 1;
			return true;
		}

		public void MarkCompletelyClean()
		{
			this.dirty = new Inventory.Mask();
			this.countDirty = 0;
		}

		public void MarkCompletelyDirty()
		{
			this.dirty = new Inventory.Mask(0, this.capacity);
			this.countDirty = this.capacity;
		}

		public bool MarkDirty(int slot)
		{
			if (slot < 0 || slot >= this.capacity || !this.dirty.On(slot))
			{
				return false;
			}
			Inventory.Collection<T> collection = this;
			collection.countDirty = collection.countDirty + 1;
			return true;
		}

		public T[] OccupiedToArray()
		{
			T[] tArray = new T[this.count];
			for (int i = 0; i < this.count; i++)
			{
				tArray[i] = this.array[this.indices[i]];
			}
			return tArray;
		}

		public bool Occupy(int slot, T occupant)
		{
			if (slot < 0 || slot >= this.capacity)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (!this.occupied.On(slot))
			{
				return false;
			}
			this.DoSet(slot, occupant);
			return true;
		}

		public bool Supplant(int slot, T value, out T replacedValue, bool equalityCheck)
		{
			if (slot < 0 || slot >= this.capacity)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (!this.occupied.On(slot))
			{
				return this.DoReplace(equalityCheck, slot, value, out replacedValue);
			}
			replacedValue = default(T);
			return false;
		}

		public bool SupplantOrOccupy(int slot, T occupant, out T replacedValue, bool equalityCheck)
		{
			if (slot < 0 || slot >= this.capacity)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (!this.occupied.On(slot))
			{
				return this.DoReplace(equalityCheck, slot, occupant, out replacedValue);
			}
			replacedValue = default(T);
			this.DoSet(slot, occupant);
			return false;
		}

		public static class Default
		{
			public readonly static Inventory.Collection<T> Empty;

			static Default()
			{
				Inventory.Collection<T>.Default.Empty = new Inventory.Collection<T>(0);
			}
		}

		public sealed class OccupiedCollection : IEnumerable, IEnumerable<T>
		{
			public readonly Inventory.Collection<T> Collection;

			public int Count
			{
				get
				{
					return this.Collection.count;
				}
			}

			public bool Empty
			{
				get
				{
					return this.Collection.count == 0;
				}
			}

			internal OccupiedCollection(Inventory.Collection<T> collection)
			{
				this.Collection = collection;
			}

			public Inventory.Collection<T>.OccupiedCollection.Enumerator GetEnumerator()
			{
				return new Inventory.Collection<T>.OccupiedCollection.Enumerator(this.Collection);
			}

			IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public T[] ToArray()
			{
				return this.Collection.OccupiedToArray();
			}

			public struct Enumerator : IDisposable, IEnumerator, IEnumerator<T>
			{
				private Inventory.Collection<T> collection;

				private int indexPosition;

				public T Current
				{
					get
					{
						return this.collection.array[this.collection.indices[this.indexPosition]];
					}
				}

				public int Slot
				{
					get
					{
						return this.collection.indices[this.indexPosition];
					}
				}

				object System.Collections.IEnumerator.Current
				{
					get
					{
						return this.collection.array[this.collection.indices[this.indexPosition]];
					}
				}

				internal Enumerator(Inventory.Collection<T> collection)
				{
					this.collection = collection;
					this.indexPosition = -1;
				}

				public void Dispose()
				{
					this.collection = null;
				}

				public bool MoveNext()
				{
					Inventory.Collection<T>.OccupiedCollection.Enumerator enumerator = this;
					int num = enumerator.indexPosition + 1;
					int num1 = num;
					enumerator.indexPosition = num;
					return num1 < this.collection.count;
				}

				public void Reset()
				{
					this.indexPosition = -1;
				}
			}

			public struct ReverseEnumerator : IDisposable, IEnumerator, IEnumerator<T>
			{
				private Inventory.Collection<T> collection;

				private int indexPosition;

				public T Current
				{
					get
					{
						return this.collection.array[this.collection.indices[this.indexPosition]];
					}
				}

				public int Slot
				{
					get
					{
						return this.collection.indices[this.indexPosition];
					}
				}

				object System.Collections.IEnumerator.Current
				{
					get
					{
						return this.collection.array[this.collection.indices[this.indexPosition]];
					}
				}

				internal ReverseEnumerator(Inventory.Collection<T> collection)
				{
					this.collection = collection;
					this.indexPosition = collection.count;
				}

				public void Dispose()
				{
					this.collection = null;
				}

				public bool MoveNext()
				{
					Inventory.Collection<T>.OccupiedCollection.ReverseEnumerator reverseEnumerator = this;
					int num = reverseEnumerator.indexPosition - 1;
					int num1 = num;
					reverseEnumerator.indexPosition = num;
					return num1 >= 0;
				}

				public void Reset()
				{
					this.indexPosition = this.collection.count;
				}
			}
		}

		public sealed class VacantCollection : IEnumerable, IEnumerable<int>
		{
			public readonly Inventory.Collection<T> Collection;

			public int Count
			{
				get
				{
					return this.Collection.capacity - this.Collection.count;
				}
			}

			public bool Empty
			{
				get
				{
					return this.Collection.count == this.Collection.capacity;
				}
			}

			internal VacantCollection(Inventory.Collection<T> collection)
			{
				this.Collection = collection;
			}

			public Inventory.Collection<T>.VacantCollection.Enumerator GetEnumerator()
			{
				return new Inventory.Collection<T>.VacantCollection.Enumerator(this.Collection);
			}

			IEnumerator<int> System.Collections.Generic.IEnumerable<int>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public struct Enumerator : IDisposable, IEnumerator, IEnumerator<int>
			{
				private Inventory.Collection<T> collection;

				private int slotPosition;

				public int Current
				{
					get
					{
						return this.slotPosition;
					}
				}

				object System.Collections.IEnumerator.Current
				{
					get
					{
						return this.slotPosition;
					}
				}

				internal Enumerator(Inventory.Collection<T> collection)
				{
					this.collection = collection;
					this.slotPosition = -1;
				}

				public void Dispose()
				{
					this.collection = null;
				}

				public bool MoveNext()
				{
					do
					{
						Inventory.Collection<T>.VacantCollection.Enumerator enumerator = this;
						int num = enumerator.slotPosition + 1;
						int num1 = num;
						enumerator.slotPosition = num;
						if (num1 < this.collection.capacity)
						{
							continue;
						}
						return false;
					}
					while (this.collection.occupied[this.slotPosition]);
					return true;
				}

				public void Reset()
				{
					this.slotPosition = -1;
				}
			}
		}
	}

	public static class Constants
	{
		public const int MaximumSlotCount = 256;
	}

	private static class Empty
	{
		public readonly static Inventory.SlotFlags[] SlotFlags;

		static Empty()
		{
			Inventory.Empty.SlotFlags = new Inventory.SlotFlags[0];
		}
	}

	public struct Mask
	{
		public int a;

		public int b;

		public int c;

		public int d;

		public int e;

		public int f;

		public int g;

		public int h;

		public bool any
		{
			get
			{
				return (this.a != 0 || this.b != 0 || this.c != 0 || this.d != 0 || this.e != 0 || this.f != 0 || this.g != 0 ? true : this.h != 0);
			}
		}

		public int firstOnBit
		{
			get
			{
				int num = 0;
				int num1 = 0;
				if (this.a != 0)
				{
					num1 = this.a;
				}
				else
				{
					num++;
					if (this.b != 0)
					{
						num1 = this.b;
					}
					else
					{
						num++;
						if (this.c != 0)
						{
							num1 = this.c;
						}
						else
						{
							num++;
							if (this.d != 0)
							{
								num1 = this.d;
							}
							else
							{
								num++;
								if (this.e != 0)
								{
									num1 = this.e;
								}
								else
								{
									num++;
									if (this.f != 0)
									{
										num1 = this.f;
									}
									else
									{
										num++;
										if (this.g != 0)
										{
											num1 = this.g;
										}
										else
										{
											num++;
											if (this.h != 0)
											{
												num1 = this.h;
											}
											else
											{
												num++;
												num1 = 0;
											}
										}
									}
								}
							}
						}
					}
				}
				int num2 = 0;
				int num3 = 0;
				while (num3 < 32)
				{
					if ((num1 & 1 << (num3 & 31)) != 1 << (num3 & 31))
					{
						num2++;
						num3++;
					}
					else
					{
						break;
					}
				}
				return num * 32 + num2;
			}
		}

		public bool this[int bit]
		{
			get
			{
				if (bit < 128)
				{
					if (bit < 64)
					{
						if (bit < 32)
						{
							return (this.a & 1 << (bit & 31)) != 0;
						}
						return (this.b & 1 << (bit - 32 & 31)) != 0;
					}
					if (bit < 96)
					{
						return (this.c & 1 << (bit - 64 & 31)) != 0;
					}
					return (this.d & 1 << (bit - 96 & 31)) != 0;
				}
				if (bit < 192)
				{
					if (bit < 160)
					{
						return (this.e & 1 << (bit - 128 & 31)) != 0;
					}
					return (this.f & 1 << (bit - 160 & 31)) != 0;
				}
				if (bit < 224)
				{
					return (this.g & 1 << (bit - 192 & 31)) != 0;
				}
				return (this.h & 1 << (bit - 224 & 31)) != 0;
			}
			set
			{
				if (value)
				{
					if (bit < 128)
					{
						if (bit < 64)
						{
							if (bit >= 32)
							{
								Inventory.Mask mask = this;
								mask.b = mask.b | 1 << (bit - 32 & 31 & 31);
							}
							else
							{
								Inventory.Mask mask1 = this;
								mask1.a = mask1.a | 1 << (bit & 31 & 31);
							}
						}
						else if (bit >= 96)
						{
							Inventory.Mask mask2 = this;
							mask2.d = mask2.d | 1 << (bit - 96 & 31 & 31);
						}
						else
						{
							Inventory.Mask mask3 = this;
							mask3.c = mask3.c | 1 << (bit - 64 & 31 & 31);
						}
					}
					else if (bit < 192)
					{
						if (bit >= 160)
						{
							Inventory.Mask mask4 = this;
							mask4.f = mask4.f | 1 << (bit - 160 & 31 & 31);
						}
						else
						{
							Inventory.Mask mask5 = this;
							mask5.e = mask5.e | 1 << (bit - 128 & 31 & 31);
						}
					}
					else if (bit >= 224)
					{
						Inventory.Mask mask6 = this;
						mask6.h = mask6.h | 1 << (bit - 224 & 31 & 31);
					}
					else
					{
						Inventory.Mask mask7 = this;
						mask7.g = mask7.g | 1 << (bit - 192 & 31 & 31);
					}
				}
				else if (bit < 128)
				{
					if (bit < 64)
					{
						if (bit >= 32)
						{
							Inventory.Mask mask8 = this;
							mask8.b = mask8.b & ~(1 << (bit - 32 & 31 & 31));
						}
						else
						{
							Inventory.Mask mask9 = this;
							mask9.a = mask9.a & ~(1 << (bit & 31 & 31));
						}
					}
					else if (bit >= 96)
					{
						Inventory.Mask mask10 = this;
						mask10.d = mask10.d & ~(1 << (bit - 96 & 31 & 31));
					}
					else
					{
						Inventory.Mask mask11 = this;
						mask11.c = mask11.c & ~(1 << (bit - 64 & 31 & 31));
					}
				}
				else if (bit < 192)
				{
					if (bit >= 160)
					{
						Inventory.Mask mask12 = this;
						mask12.f = mask12.f & ~(1 << (bit - 160 & 31 & 31));
					}
					else
					{
						Inventory.Mask mask13 = this;
						mask13.e = mask13.e & ~(1 << (bit - 128 & 31 & 31));
					}
				}
				else if (bit >= 224)
				{
					Inventory.Mask mask14 = this;
					mask14.h = mask14.h & ~(1 << (bit - 224 & 31 & 31));
				}
				else
				{
					Inventory.Mask mask15 = this;
					mask15.g = mask15.g & ~(1 << (bit - 192 & 31 & 31));
				}
			}
		}

		public int lastOnBit
		{
			get
			{
				int num = 7;
				int num1 = 0;
				if (this.h != 0)
				{
					num1 = this.h;
				}
				else
				{
					num--;
					if (this.g != 0)
					{
						num1 = this.g;
					}
					else
					{
						num--;
						if (this.f != 0)
						{
							num1 = this.f;
						}
						else
						{
							num--;
							if (this.e != 0)
							{
								num1 = this.e;
							}
							else
							{
								num--;
								if (this.d != 0)
								{
									num1 = this.d;
								}
								else
								{
									num--;
									if (this.c != 0)
									{
										num1 = this.c;
									}
									else
									{
										num--;
										if (this.b != 0)
										{
											num1 = this.b;
										}
										else
										{
											num--;
											if (this.a == 0)
											{
												return -1;
											}
											num1 = this.a;
										}
									}
								}
							}
						}
					}
				}
				int num2 = 0;
				int num3 = 31;
				while (num3 >= 0)
				{
					if ((num1 & 1 << (num3 & 31)) != 1 << (num3 & 31))
					{
						num2++;
						num3--;
					}
					else
					{
						break;
					}
				}
				return num * 32 + num2;
			}
		}

		public Mask(bool defaultOn)
		{
			int num = (!defaultOn ? 0 : -1);
			int num1 = num;
			this.h = num;
			int num2 = num1;
			num1 = num2;
			this.g = num2;
			int num3 = num1;
			num1 = num3;
			this.f = num3;
			int num4 = num1;
			num1 = num4;
			this.e = num4;
			int num5 = num1;
			num1 = num5;
			this.d = num5;
			int num6 = num1;
			num1 = num6;
			this.c = num6;
			int num7 = num1;
			num1 = num7;
			this.b = num7;
			this.a = num1;
		}

		public Mask(int onStart, int onCount) : this(false)
		{
			int num = onStart;
			int num1 = onStart + onCount;
			while (num < 256 && num < num1)
			{
				this[num] = true;
				num++;
			}
		}

		public int CountOnBits()
		{
			uint num;
			int num1 = 0;
			if (this.a != 0)
			{
				num = (uint)this.a;
				while (num != 0)
				{
					num = num & num - 1;
					num1++;
				}
			}
			if (this.b != 0)
			{
				num = (uint)this.b;
				while (num != 0)
				{
					num = num & num - 1;
					num1++;
				}
			}
			if (this.c != 0)
			{
				num = (uint)this.c;
				while (num != 0)
				{
					num = num & num - 1;
					num1++;
				}
			}
			if (this.d != 0)
			{
				num = (uint)this.d;
				while (num != 0)
				{
					num = num & num - 1;
					num1++;
				}
			}
			if (this.e != 0)
			{
				num = (uint)this.e;
				while (num != 0)
				{
					num = num & num - 1;
					num1++;
				}
			}
			if (this.f != 0)
			{
				num = (uint)this.f;
				while (num != 0)
				{
					num = num & num - 1;
					num1++;
				}
			}
			if (this.g != 0)
			{
				num = (uint)this.g;
				while (num != 0)
				{
					num = num & num - 1;
					num1++;
				}
			}
			if (this.h != 0)
			{
				num = (uint)this.h;
				while (num != 0)
				{
					num = num & num - 1;
					num1++;
				}
			}
			return num1;
		}

		public bool Off(int bit)
		{
			int num;
			if (bit < 128)
			{
				if (bit < 64)
				{
					if (bit < 32)
					{
						num = 1 << (bit & 31);
						if (num == 0 || (this.a & num) != num)
						{
							return false;
						}
						Inventory.Mask mask = this;
						mask.a = mask.a & ~num;
						return true;
					}
					num = 1 << (bit - 32 & 31);
					if (num == 0 || (this.b & num) != num)
					{
						return false;
					}
					Inventory.Mask mask1 = this;
					mask1.b = mask1.b & ~num;
					return true;
				}
				if (bit < 96)
				{
					num = 1 << (bit - 64 & 31);
					if (num == 0 || (this.c & num) != num)
					{
						return false;
					}
					Inventory.Mask mask2 = this;
					mask2.c = mask2.c & ~num;
					return true;
				}
				num = 1 << (bit - 96 & 31);
				if (num == 0 || (this.d & num) != num)
				{
					return false;
				}
				Inventory.Mask mask3 = this;
				mask3.d = mask3.d & ~num;
				return true;
			}
			if (bit < 192)
			{
				if (bit < 160)
				{
					num = 1 << (bit - 128 & 31);
					if (num == 0 || (this.e & num) != num)
					{
						return false;
					}
					Inventory.Mask mask4 = this;
					mask4.e = mask4.e & ~num;
					return true;
				}
				num = 1 << (bit - 160 & 31);
				if (num == 0 || (this.f & num) != num)
				{
					return false;
				}
				Inventory.Mask mask5 = this;
				mask5.f = mask5.f & ~num;
				return true;
			}
			if (bit < 224)
			{
				num = 1 << (bit - 192 & 31);
				if (num == 0 || (this.g & num) != num)
				{
					return false;
				}
				Inventory.Mask mask6 = this;
				mask6.g = mask6.g & ~num;
				return true;
			}
			num = 1 << (bit - 224 & 31);
			if (num == 0 || (this.h & num) != num)
			{
				return false;
			}
			Inventory.Mask mask7 = this;
			mask7.h = mask7.h & ~num;
			return true;
		}

		public bool On(int bit)
		{
			int num;
			if (bit < 128)
			{
				if (bit < 64)
				{
					if (bit < 32)
					{
						num = 1 << (bit & 31);
						if (num == 0 || (this.a & num) != 0)
						{
							return false;
						}
						Inventory.Mask mask = this;
						mask.a = mask.a | num;
						return true;
					}
					num = 1 << (bit - 32 & 31);
					if (num == 0 || (this.b & num) != 0)
					{
						return false;
					}
					Inventory.Mask mask1 = this;
					mask1.b = mask1.b | num;
					return true;
				}
				if (bit < 96)
				{
					num = 1 << (bit - 64 & 31);
					if (num == 0 || (this.c & num) != 0)
					{
						return false;
					}
					Inventory.Mask mask2 = this;
					mask2.c = mask2.c | num;
					return true;
				}
				num = 1 << (bit - 96 & 31);
				if (num == 0 || (this.d & num) != 0)
				{
					return false;
				}
				Inventory.Mask mask3 = this;
				mask3.d = mask3.d | num;
				return true;
			}
			if (bit < 192)
			{
				if (bit < 160)
				{
					num = 1 << (bit - 128 & 31);
					if (num == 0 || (this.e & num) != 0)
					{
						return false;
					}
					Inventory.Mask mask4 = this;
					mask4.e = mask4.e | num;
					return true;
				}
				num = 1 << (bit - 160 & 31);
				if (num == 0 || (this.f & num) != 0)
				{
					return false;
				}
				Inventory.Mask mask5 = this;
				mask5.f = mask5.f | num;
				return true;
			}
			if (bit < 224)
			{
				num = 1 << (bit - 192 & 31);
				if (num == 0 || (this.g & num) != 0)
				{
					return false;
				}
				Inventory.Mask mask6 = this;
				mask6.g = mask6.g | num;
				return true;
			}
			num = 1 << (bit - 224 & 31);
			if (num == 0 || (this.h & num) != 0)
			{
				return false;
			}
			Inventory.Mask mask7 = this;
			mask7.h = mask7.h | num;
			return true;
		}
	}

	public struct OccupiedIterator : IDisposable
	{
		private Inventory.Collection<InventoryItem>.OccupiedCollection.Enumerator baseEnumerator;

		internal InventoryItem inventoryItem
		{
			get
			{
				return this.baseEnumerator.Current;
			}
		}

		public IInventoryItem item
		{
			get
			{
				return this.baseEnumerator.Current.iface;
			}
		}

		public int slot
		{
			get
			{
				return this.baseEnumerator.Slot;
			}
		}

		public OccupiedIterator(Inventory inventory)
		{
			this.baseEnumerator = inventory.collection.OccupiedEnumerator;
		}

		public void Dispose()
		{
			this.baseEnumerator.Dispose();
		}

		public bool Next()
		{
			return this.baseEnumerator.MoveNext();
		}

		internal bool Next(out InventoryItem item, out int slot)
		{
			if (!this.Next())
			{
				slot = -1;
				item = null;
				return false;
			}
			slot = this.baseEnumerator.Slot;
			item = this.baseEnumerator.Current;
			return true;
		}

		internal bool Next(int datablockUniqueID, out InventoryItem item, out int slot)
		{
			while (this.Next(out item, out slot))
			{
				if (item.datablockUniqueID != datablockUniqueID)
				{
					continue;
				}
				return true;
			}
			return false;
		}

		internal bool Next(ItemDataBlock datablock, out InventoryItem item, out int slot)
		{
			return this.Next(datablock.uniqueID, out item, out slot);
		}

		public bool Next(out IInventoryItem item, out int slot)
		{
			InventoryItem inventoryItem;
			if (!this.Next(out inventoryItem, out slot))
			{
				item = null;
				return false;
			}
			item = inventoryItem.iface;
			return true;
		}

		public bool Next(int datablockUniqueID, out IInventoryItem item, out int slot)
		{
			InventoryItem inventoryItem;
			if (!this.Next(datablockUniqueID, out inventoryItem, out slot))
			{
				item = null;
				return false;
			}
			item = inventoryItem.iface;
			return true;
		}

		internal bool Next(ItemDataBlock datablock, out IInventoryItem item, out int slot)
		{
			return this.Next(datablock.uniqueID, out item, out slot);
		}

		public bool Next<TItemInterface>(out TItemInterface item, out int slot)
		where TItemInterface : class, IInventoryItem
		{
			IInventoryItem inventoryItem;
			while (this.Next(out inventoryItem, out slot))
			{
				if (inventoryItem is TItemInterface)
				{
					item = (TItemInterface)this.inventoryItem.iface;
					return true;
				}
			}
			item = (TItemInterface)null;
			return false;
		}

		public bool Next<TItemInterface>(int datablockUniqueID, out TItemInterface item, out int slot)
		where TItemInterface : class, IInventoryItem
		{
			IInventoryItem inventoryItem;
			while (this.Next(datablockUniqueID, out inventoryItem, out slot))
			{
				if (inventoryItem is TItemInterface)
				{
					item = (TItemInterface)this.inventoryItem.iface;
					return true;
				}
			}
			item = (TItemInterface)null;
			return false;
		}

		public bool Next<TItemInterface>(ItemDataBlock datablock, out TItemInterface item, out int slot)
		where TItemInterface : class, IInventoryItem
		{
			return this.Next<TItemInterface>(datablock.uniqueID, out item, out slot);
		}

		public bool Next(out int slot)
		{
			InventoryItem inventoryItem;
			return this.Next(out inventoryItem, out slot);
		}

		public bool Next(int datablockUniqueID, out int slot)
		{
			InventoryItem inventoryItem;
			return this.Next(out inventoryItem, out slot);
		}

		public bool Next(ItemDataBlock datablock, out int slot)
		{
			InventoryItem inventoryItem;
			return this.Next(datablock.uniqueID, out inventoryItem, out slot);
		}

		public bool Next<TItemInterface>(out int slot)
		where TItemInterface : class, IInventoryItem
		{
			TItemInterface tItemInterface;
			return this.Next<TItemInterface>(out tItemInterface, out slot);
		}

		public bool Next<TItemInterface>(int datablockUniqueID, out int slot)
		where TItemInterface : class, IInventoryItem
		{
			TItemInterface tItemInterface;
			return this.Next<TItemInterface>(out tItemInterface, out slot);
		}

		public bool Next<TItemInterface>(ItemDataBlock datablock, out int slot)
		where TItemInterface : class, IInventoryItem
		{
			TItemInterface tItemInterface;
			return this.Next<TItemInterface>(datablock.uniqueID, out tItemInterface, out slot);
		}

		internal bool Next(out InventoryItem item)
		{
			int num;
			return this.Next(out item, out num);
		}

		internal bool Next(int datablockUniqueID, out InventoryItem item)
		{
			int num;
			return this.Next(datablockUniqueID, out item, out num);
		}

		internal bool Next(ItemDataBlock datablock, out InventoryItem item)
		{
			int num;
			return this.Next(datablock.uniqueID, out item, out num);
		}

		public bool Next(out IInventoryItem item)
		{
			int num;
			return this.Next(out item, out num);
		}

		public bool Next(int datablockUniqueID, out IInventoryItem item)
		{
			int num;
			return this.Next(datablockUniqueID, out item, out num);
		}

		internal bool Next(ItemDataBlock datablock, out IInventoryItem item)
		{
			int num;
			return this.Next(datablock.uniqueID, out item, out num);
		}

		public bool Next<TItemInterface>(out TItemInterface item)
		where TItemInterface : class, IInventoryItem
		{
			int num;
			return this.Next<TItemInterface>(out item, out num);
		}

		public bool Next<TItemInterface>(int datablockUniqueID, out TItemInterface item)
		where TItemInterface : class, IInventoryItem
		{
			int num;
			return this.Next<TItemInterface>(datablockUniqueID, out item, out num);
		}

		public bool Next<TItemInterface>(ItemDataBlock datablock, out TItemInterface item)
		where TItemInterface : class, IInventoryItem
		{
			int num;
			return this.Next<TItemInterface>(datablock.uniqueID, out item, out num);
		}

		public void Reset()
		{
			this.baseEnumerator.Reset();
		}
	}

	public struct OccupiedReverseIterator : IDisposable
	{
		private Inventory.Collection<InventoryItem>.OccupiedCollection.ReverseEnumerator baseEnumerator;

		internal InventoryItem inventoryItem
		{
			get
			{
				return this.baseEnumerator.Current;
			}
		}

		public IInventoryItem item
		{
			get
			{
				return this.baseEnumerator.Current.iface;
			}
		}

		public int slot
		{
			get
			{
				return this.baseEnumerator.Slot;
			}
		}

		public OccupiedReverseIterator(Inventory inventory)
		{
			this.baseEnumerator = inventory.collection.OccupiedReverseEnumerator;
		}

		public void Dispose()
		{
			this.baseEnumerator.Dispose();
		}

		public bool Next()
		{
			return this.baseEnumerator.MoveNext();
		}

		internal bool Next(out InventoryItem item, out int slot)
		{
			if (!this.Next())
			{
				slot = -1;
				item = null;
				return false;
			}
			slot = this.baseEnumerator.Slot;
			item = this.baseEnumerator.Current;
			return true;
		}

		internal bool Next(int datablockUniqueID, out InventoryItem item, out int slot)
		{
			while (this.Next(out item, out slot))
			{
				if (item.datablockUniqueID != datablockUniqueID)
				{
					continue;
				}
				return true;
			}
			return false;
		}

		internal bool Next(ItemDataBlock datablock, out InventoryItem item, out int slot)
		{
			return this.Next(datablock.uniqueID, out item, out slot);
		}

		public bool Next(out IInventoryItem item, out int slot)
		{
			InventoryItem inventoryItem;
			if (!this.Next(out inventoryItem, out slot))
			{
				item = null;
				return false;
			}
			item = inventoryItem.iface;
			return true;
		}

		public bool Next(int datablockUniqueID, out IInventoryItem item, out int slot)
		{
			InventoryItem inventoryItem;
			if (!this.Next(datablockUniqueID, out inventoryItem, out slot))
			{
				item = null;
				return false;
			}
			item = inventoryItem.iface;
			return true;
		}

		internal bool Next(ItemDataBlock datablock, out IInventoryItem item, out int slot)
		{
			return this.Next(datablock.uniqueID, out item, out slot);
		}

		public bool Next<TItemInterface>(out TItemInterface item, out int slot)
		where TItemInterface : class, IInventoryItem
		{
			IInventoryItem inventoryItem;
			while (this.Next(out inventoryItem, out slot))
			{
				if (inventoryItem is TItemInterface)
				{
					item = (TItemInterface)this.inventoryItem.iface;
					return true;
				}
			}
			item = (TItemInterface)null;
			return false;
		}

		public bool Next<TItemInterface>(int datablockUniqueID, out TItemInterface item, out int slot)
		where TItemInterface : class, IInventoryItem
		{
			IInventoryItem inventoryItem;
			while (this.Next(datablockUniqueID, out inventoryItem, out slot))
			{
				if (inventoryItem is TItemInterface)
				{
					item = (TItemInterface)this.inventoryItem.iface;
					return true;
				}
			}
			item = (TItemInterface)null;
			return false;
		}

		public bool Next<TItemInterface>(ItemDataBlock datablock, out TItemInterface item, out int slot)
		where TItemInterface : class, IInventoryItem
		{
			return this.Next<TItemInterface>(datablock.uniqueID, out item, out slot);
		}

		public bool Next(out int slot)
		{
			InventoryItem inventoryItem;
			return this.Next(out inventoryItem, out slot);
		}

		public bool Next(int datablockUniqueID, out int slot)
		{
			InventoryItem inventoryItem;
			return this.Next(out inventoryItem, out slot);
		}

		public bool Next(ItemDataBlock datablock, out int slot)
		{
			InventoryItem inventoryItem;
			return this.Next(datablock.uniqueID, out inventoryItem, out slot);
		}

		public bool Next<TItemInterface>(out int slot)
		where TItemInterface : class, IInventoryItem
		{
			TItemInterface tItemInterface;
			return this.Next<TItemInterface>(out tItemInterface, out slot);
		}

		public bool Next<TItemInterface>(int datablockUniqueID, out int slot)
		where TItemInterface : class, IInventoryItem
		{
			TItemInterface tItemInterface;
			return this.Next<TItemInterface>(out tItemInterface, out slot);
		}

		public bool Next<TItemInterface>(ItemDataBlock datablock, out int slot)
		where TItemInterface : class, IInventoryItem
		{
			TItemInterface tItemInterface;
			return this.Next<TItemInterface>(datablock.uniqueID, out tItemInterface, out slot);
		}

		internal bool Next(out InventoryItem item)
		{
			int num;
			return this.Next(out item, out num);
		}

		internal bool Next(int datablockUniqueID, out InventoryItem item)
		{
			int num;
			return this.Next(datablockUniqueID, out item, out num);
		}

		internal bool Next(ItemDataBlock datablock, out InventoryItem item)
		{
			int num;
			return this.Next(datablock.uniqueID, out item, out num);
		}

		public bool Next(out IInventoryItem item)
		{
			int num;
			return this.Next(out item, out num);
		}

		public bool Next(int datablockUniqueID, out IInventoryItem item)
		{
			int num;
			return this.Next(datablockUniqueID, out item, out num);
		}

		internal bool Next(ItemDataBlock datablock, out IInventoryItem item)
		{
			int num;
			return this.Next(datablock.uniqueID, out item, out num);
		}

		public bool Next<TItemInterface>(out TItemInterface item)
		where TItemInterface : class, IInventoryItem
		{
			int num;
			return this.Next<TItemInterface>(out item, out num);
		}

		public bool Next<TItemInterface>(int datablockUniqueID, out TItemInterface item)
		where TItemInterface : class, IInventoryItem
		{
			int num;
			return this.Next<TItemInterface>(datablockUniqueID, out item, out num);
		}

		public bool Next<TItemInterface>(ItemDataBlock datablock, out TItemInterface item)
		where TItemInterface : class, IInventoryItem
		{
			int num;
			return this.Next<TItemInterface>(datablock.uniqueID, out item, out num);
		}

		public void Reset()
		{
			this.baseEnumerator.Reset();
		}
	}

	private static class Payload
	{
		private const Inventory.Payload.Opt NoOp1_Mask = Inventory.Payload.Opt.DoNotStack | Inventory.Payload.Opt.DoNotAssign;

		private const Inventory.Payload.Opt NoOp2_Mask = Inventory.Payload.Opt.IgnoreSlotOffset | Inventory.Payload.Opt.RestrictToOffset;

		public static Inventory.Payload.Result AddItem(Inventory inventory, ref Inventory.Addition addition, Inventory.Payload.Opt options, InventoryItem reuseItem)
		{
			Inventory.Payload.Result result = new Inventory.Payload.Result();
			Inventory.Slot.Range range;
			int count;
			Inventory.Payload.StackResult stackResult;
			InventoryItem inventoryItem;
			Inventory.Payload.StackResult stackResult1;
			InventoryItem inventoryItem1;
			InventoryItem inventoryItem2;
			Inventory.Payload.StackArguments flags = new Inventory.Payload.StackArguments();
			Inventory.Payload.Assignment assignment = new Inventory.Payload.Assignment();
			bool flag;
			if ((byte)(options & (Inventory.Payload.Opt.DoNotStack | Inventory.Payload.Opt.DoNotAssign)) == 3 || (byte)(options & (Inventory.Payload.Opt.IgnoreSlotOffset | Inventory.Payload.Opt.RestrictToOffset)) == 12)
			{
				result.item = null;
				result.flags = Inventory.Payload.Result.Flags.OptionsResultedInNoOp;
				result.usesRemaining = 0;
			}
			else
			{
				ItemDataBlock itemDataBlock = addition.ItemDataBlock;
				if (!itemDataBlock)
				{
					result.item = null;
					result.flags = Inventory.Payload.Result.Flags.NoItemDatablock;
					result.usesRemaining = 0;
					return result;
				}
				Inventory.Slot.KindFlags primaryKindFlags = addition.SlotPreference.PrimaryKindFlags;
				Inventory.Slot.KindFlags secondaryKindFlags = addition.SlotPreference.SecondaryKindFlags;
				range = ((byte)(options & Inventory.Payload.Opt.IgnoreSlotOffset) != 4 ? Inventory.Payload.RangeArray.CalculateExplicitSlotPosition(inventory, ref addition.SlotPreference) : new Inventory.Slot.Range());
				bool flag1 = (byte)(options & Inventory.Payload.Opt.RestrictToOffset) == 8;
				bool any = range.Any;
				if (flag1 && !any)
				{
					result.item = null;
					result.flags = Inventory.Payload.Result.Flags.MissingRequiredOffset;
					result.usesRemaining = 0;
					return result;
				}
				if (!flag1)
				{
					Inventory.Payload.RangeArray.FillTemporaryRanges(ref Inventory.Payload.RangeArray.Primary, inventory, primaryKindFlags, range, true);
					Inventory.Payload.RangeArray.FillTemporaryRanges(ref Inventory.Payload.RangeArray.Secondary, inventory, secondaryKindFlags, range, false);
				}
				else
				{
					Inventory.Payload.RangeArray.FillTemporaryRanges(ref Inventory.Payload.RangeArray.Primary, inventory, 0, range, true);
					Inventory.Payload.RangeArray.FillTemporaryRanges(ref Inventory.Payload.RangeArray.Secondary, inventory, 0, range, false);
				}
				if (Inventory.Payload.RangeArray.Primary.Count == 0)
				{
					primaryKindFlags = (Inventory.Slot.KindFlags)0;
					if (Inventory.Payload.RangeArray.Secondary.Count != 0)
					{
						count = Inventory.Payload.RangeArray.Secondary.Count;
					}
					else
					{
						secondaryKindFlags = (Inventory.Slot.KindFlags)0;
						count = 0;
					}
				}
				else if (Inventory.Payload.RangeArray.Secondary.Count != 0)
				{
					count = Inventory.Payload.RangeArray.Primary.Count + Inventory.Payload.RangeArray.Secondary.Count;
				}
				else
				{
					secondaryKindFlags = (Inventory.Slot.KindFlags)0;
					count = Inventory.Payload.RangeArray.Primary.Count;
				}
				if (count == 0 || !any && (byte)((byte)(primaryKindFlags | secondaryKindFlags) & 7) == 0)
				{
					result.item = null;
					result.flags = Inventory.Payload.Result.Flags.NoSlotRanges;
					result.usesRemaining = 0;
				}
				else
				{
					int num = itemDataBlock._maxUses;
					bool flag2 = (byte)(options & Inventory.Payload.Opt.ReuseItem) == 16;
					if (!flag2 || !object.ReferenceEquals(reuseItem, null) && (!itemDataBlock.untransferable || !(reuseItem.inventory != inventory)))
					{
						Inventory.Collection<InventoryItem> collection = inventory.collection;
						result.usesRemaining = (!flag2 ? addition.UsesQuantity.CalculateCount(itemDataBlock) : reuseItem.uses);
						if ((byte)(options & Inventory.Payload.Opt.DoNotStack) == 1 || (byte)(addition.SlotPreference.Flags & Inventory.Slot.PreferenceFlags.Stack) != 8)
						{
							inventoryItem = null;
							stackResult = Inventory.Payload.StackResult.NoneNotMarked;
						}
						else
						{
							flags.collection = collection;
							flags.datablockUID = itemDataBlock.uniqueID;
							flags.splittable = itemDataBlock.IsSplittable();
							flags.useCount = result.usesRemaining;
							flags.prefFlags = addition.SlotPreference.Flags;
							Inventory.Payload.StackResult stackResult2 = Inventory.Payload.StackUses(ref flags, ref Inventory.Payload.RangeArray.Primary, out inventoryItem1);
							if (stackResult2 == Inventory.Payload.StackResult.NoneUnsplittable || stackResult2 == Inventory.Payload.StackResult.Complete)
							{
								InventoryItem inventoryItem3 = inventoryItem1;
								inventoryItem2 = inventoryItem3;
								inventoryItem = inventoryItem3;
								Inventory.Payload.StackResult stackResult3 = stackResult2;
								stackResult1 = stackResult3;
								stackResult = stackResult3;
							}
							else
							{
								stackResult1 = Inventory.Payload.StackUses(ref flags, ref Inventory.Payload.RangeArray.Secondary, out inventoryItem2);
								if (stackResult2 <= stackResult1)
								{
									inventoryItem = inventoryItem1 ?? inventoryItem2;
									stackResult = stackResult1;
								}
								else
								{
									inventoryItem = inventoryItem1 ?? inventoryItem2;
									stackResult = stackResult2;
								}
							}
							result.usesRemaining = flags.useCount;
						}
						if (stackResult != Inventory.Payload.StackResult.Complete)
						{
							if (stackResult != Inventory.Payload.StackResult.Partial)
							{
								result.flags = Inventory.Payload.Result.Flags.OptionsResultedInNoOp;
							}
							else
							{
								result.item = inventoryItem;
								result.flags = Inventory.Payload.Result.Flags.Stacked;
							}
							if ((byte)(options & Inventory.Payload.Opt.DoNotAssign) == 2)
							{
								result.item = inventoryItem;
								if (result.flags == Inventory.Payload.Result.Flags.OptionsResultedInNoOp)
								{
									result.flags = Inventory.Payload.Result.Flags.MissingRequiredOffset;
								}
							}
							else if (!collection.HasNoVacancy)
							{
								assignment.inventory = inventory;
								assignment.collection = collection;
								assignment.fresh = !flag2;
								assignment.item = (!assignment.fresh ? reuseItem : itemDataBlock.CreateItem() as InventoryItem);
								assignment.uses = result.usesRemaining;
								assignment.datablock = itemDataBlock;
								if (flag2 || !object.ReferenceEquals(assignment.item, null))
								{
									assignment.slot = -1;
									assignment.attemptsMade = 0;
									Inventory.Collection<InventoryItem>.VacantCollection.Enumerator vacantEnumerator = collection.VacantEnumerator;
									try
									{
										flag = (Inventory.Payload.AssignItemInsideRanges(ref vacantEnumerator, ref Inventory.Payload.RangeArray.Primary, ref assignment) ? true : Inventory.Payload.AssignItemInsideRanges(ref vacantEnumerator, ref Inventory.Payload.RangeArray.Secondary, ref assignment));
									}
									finally
									{
										vacantEnumerator.Dispose();
									}
									if (flag)
									{
										result.flags = (Inventory.Payload.Result.Flags)((byte)(result.flags | Inventory.Payload.Result.Flags.Complete | Inventory.Payload.Result.Flags.AssignedInstance));
										result.item = assignment.item;
										result.usesRemaining = result.usesRemaining - result.item.uses;
									}
									else if (assignment.attemptsMade <= 0)
									{
										result.flags = (Inventory.Payload.Result.Flags)((byte)(result.flags | Inventory.Payload.Result.Flags.NoSlotRanges));
										result.item = inventoryItem;
									}
									else
									{
										result.flags = (Inventory.Payload.Result.Flags)((byte)(result.flags | Inventory.Payload.Result.Flags.NoVacancy));
										result.item = inventoryItem;
									}
								}
								else
								{
									result.item = inventoryItem;
									result.flags = (Inventory.Payload.Result.Flags)((byte)(result.flags | (!assignment.fresh ? Inventory.Payload.Result.Flags.FailedToReuse : Inventory.Payload.Result.Flags.FailedToCreate)));
								}
							}
							else
							{
								result.item = inventoryItem;
								result.flags = (Inventory.Payload.Result.Flags)((byte)(result.flags | Inventory.Payload.Result.Flags.NoVacancy));
							}
						}
						else
						{
							result.item = inventoryItem;
							result.flags = Inventory.Payload.Result.Flags.Complete | Inventory.Payload.Result.Flags.Stacked;
						}
					}
					else
					{
						result.flags = Inventory.Payload.Result.Flags.FailedToReuse;
						result.item = null;
						result.usesRemaining = 0;
					}
				}
			}
			return result;
		}

		private static bool AssignItem(ref Inventory.Payload.Assignment args)
		{
			if (args.inventory.CheckSlotFlagsAgainstSlot(args.datablock._itemFlags, args.slot) && args.item.CanMoveToSlot(args.inventory, args.slot))
			{
				args.attemptsMade = args.attemptsMade + 1;
				if (args.collection.Occupy(args.slot, args.item))
				{
					if (!args.fresh && args.item.inventory)
					{
						args.item.inventory.RemoveItem(args.item.slot);
					}
					args.item.SetUses(args.uses);
					args.item.OnAddedTo(args.inventory, args.slot);
					args.inventory.ItemAdded(args.slot, args.item.iface);
					return true;
				}
			}
			return false;
		}

		private static bool AssignItemInsideRanges(ref Inventory.Collection<InventoryItem>.VacantCollection.Enumerator enumerator, ref Inventory.Payload.RangeArray.Holder ranges, ref Inventory.Payload.Assignment args)
		{
			bool flag;
			for (int i = 0; i < ranges.Count; i++)
			{
				if (ranges.Range[i].Count == 1)
				{
					args.slot = ranges.Range[i].Start;
					if (args.collection.IsOccupied(args.slot))
					{
						goto Label0;
					}
					if (Inventory.Payload.AssignItem(ref args))
					{
						return true;
					}
				}
				enumerator.Reset();
				do
				{
				Label3:
					if (!enumerator.MoveNext())
					{
						goto Label0;
					}
					args.slot = enumerator.Current;
					switch (ranges.Range[i].ContainEx(args.slot))
					{
						case -1:
						{
							goto Label3;
						}
						case 0:
						{
							flag = false;
							break;
						}
						case 1:
						{
							flag = true;
							break;
						}
						default:
						{
							goto case 0;
						}
					}
					if (!flag)
					{
						continue;
					}
					goto Label0;
				}
				while (!Inventory.Payload.AssignItem(ref args));
				return true;
			Label0:
			}
			return false;
		}

		private static Inventory.Payload.StackResult StackUses(ref Inventory.Payload.StackArguments args, ref Inventory.Payload.RangeArray.Holder ranges, out InventoryItem item)
		{
			Inventory.Payload.StackWork current = new Inventory.Payload.StackWork();
			bool flag;
			Inventory.Payload.StackResult stackResult;
			if (ranges.Count == 0)
			{
				item = null;
				return Inventory.Payload.StackResult.NoRange;
			}
			if ((byte)(args.prefFlags & Inventory.Slot.PreferenceFlags.Stack) != 8)
			{
				item = null;
				return Inventory.Payload.StackResult.NoneNotMarked;
			}
			if (!args.splittable)
			{
				item = null;
				return Inventory.Payload.StackResult.NoneUnsplittable;
			}
			current.gotFirstUsage = false;
			current.firstUsage = null;
			int num = args.useCount;
			bool flag1 = false;
			int slot = -1;
			Inventory.Collection<InventoryItem>.OccupiedCollection.Enumerator occupiedEnumerator = new Inventory.Collection<InventoryItem>.OccupiedCollection.Enumerator();
			try
			{
				for (int i = 0; i < ranges.Count; i++)
				{
					if (ranges.Range[i].Count != 1)
					{
						if (!flag1)
						{
							occupiedEnumerator = args.collection.OccupiedEnumerator;
							flag1 = true;
						}
						else if (ranges.Range[i].Start < slot)
						{
							occupiedEnumerator.Reset();
						}
						else if (ranges.Range[i].Start == slot)
						{
							current.slot = slot;
							current.instance = occupiedEnumerator.Current;
							if (Inventory.Payload.StackUsesSlot(ref args, ref current))
							{
								item = (!current.gotFirstUsage ? current.instance : current.firstUsage);
								stackResult = Inventory.Payload.StackResult.Complete;
								return stackResult;
							}
						}
						while (true)
						{
							bool flag2 = occupiedEnumerator.MoveNext();
							flag = flag2;
							if (!flag2)
							{
								break;
							}
							slot = occupiedEnumerator.Slot;
							if (ranges.Range[i].Start <= slot)
							{
								if (slot - ranges.Range[i].Start >= ranges.Range[i].Count)
								{
									break;
								}
								else
								{
									current.slot = slot;
									current.instance = occupiedEnumerator.Current;
									if (Inventory.Payload.StackUsesSlot(ref args, ref current))
									{
										item = (!current.gotFirstUsage ? current.instance : current.firstUsage);
										stackResult = Inventory.Payload.StackResult.Complete;
										return stackResult;
									}
								}
							}
						}
						if (!flag)
						{
							slot = 257;
						}
					}
					else
					{
						Inventory.Collection<InventoryItem> collection = args.collection;
						int start = ranges.Range[i].Start;
						int num1 = start;
						current.slot = start;
						if (collection.Get(num1, out current.instance) && Inventory.Payload.StackUsesSlot(ref args, ref current))
						{
							item = (!current.gotFirstUsage ? current.instance : current.firstUsage);
							stackResult = Inventory.Payload.StackResult.Complete;
							return stackResult;
						}
					}
				}
				if (!current.gotFirstUsage)
				{
					item = null;
					return Inventory.Payload.StackResult.None;
				}
				item = current.firstUsage;
				return (args.useCount >= num ? Inventory.Payload.StackResult.None_FoundFull : Inventory.Payload.StackResult.Partial);
			}
			finally
			{
				if (flag1)
				{
					occupiedEnumerator.Dispose();
				}
			}
			return stackResult;
		}

		private static bool StackUsesSlot(ref Inventory.Payload.StackArguments args, ref Inventory.Payload.StackWork work)
		{
			if (work.instance.datablockUniqueID != args.datablockUID)
			{
				return false;
			}
			int num = args.useCount;
			args.useCount = args.useCount - work.instance.AddUses(args.useCount);
			if (num != args.useCount)
			{
				args.collection.MarkDirty(work.slot);
				if (args.useCount == 0)
				{
					return true;
				}
				if (!work.gotFirstUsage)
				{
					work.firstUsage = work.instance;
					work.gotFirstUsage = true;
				}
			}
			return false;
		}

		private struct Assignment
		{
			public Inventory.Collection<InventoryItem> collection;

			public Inventory inventory;

			public InventoryItem item;

			public ItemDataBlock datablock;

			public int slot;

			public int uses;

			public bool fresh;

			public int attemptsMade;
		}

		[Flags]
		public enum Opt : byte
		{
			DoNotStack = 1,
			DoNotAssign = 2,
			IgnoreSlotOffset = 4,
			RestrictToOffset = 8,
			ReuseItem = 16,
			AllowStackedItemsToBeReturned = 32
		}

		private static class RangeArray
		{
			private const int ArrayElementCount = 6;

			public static Inventory.Payload.RangeArray.Holder Primary;

			public static Inventory.Payload.RangeArray.Holder Secondary;

			static RangeArray()
			{
				Inventory.Payload.RangeArray.Primary = new Inventory.Payload.RangeArray.Holder(new Inventory.Slot.Range[6]);
				Inventory.Payload.RangeArray.Secondary = new Inventory.Payload.RangeArray.Holder(new Inventory.Slot.Range[6]);
			}

			public static Inventory.Slot.Range CalculateExplicitSlotPosition(Inventory inventory, ref Inventory.Slot.Preference pref)
			{
				Inventory.Slot.Range range;
				Inventory.Slot.Offset offset = pref.Offset;
				if (!offset.Specified)
				{
					return new Inventory.Slot.Range();
				}
				if (!offset.HasOffsetOfKind)
				{
					range = new Inventory.Slot.Range(0, inventory.slotCount);
				}
				else if (!inventory.slotRanges.TryGetValue(offset.OffsetOfKind, out range))
				{
					return new Inventory.Slot.Range();
				}
				int slotOffset = offset.SlotOffset;
				if (range.Count <= slotOffset)
				{
					return new Inventory.Slot.Range();
				}
				return new Inventory.Slot.Range(range.Start + slotOffset, 1);
			}

			private static bool CheckSlotKindFlag(Inventory inventory, Inventory.Slot.KindFlags flags, Inventory.Slot.KindFlags flag, Inventory.Slot.Kind kind, ref int start, ref int count)
			{
				Inventory.Slot.Range range;
				if ((byte)(flags & flag) == (byte)flag && inventory.slotRanges.TryGetValue(kind, out range) && range.Any)
				{
					if (range.End <= inventory.slotCount)
					{
						start = range.Start;
						count = range.Count;
						return true;
					}
				}
				return false;
			}

			public static void FillTemporaryRanges(ref Inventory.Payload.RangeArray.Holder temp, Inventory inventory, Inventory.Slot.KindFlags kindFlags, Inventory.Slot.Range explicitSlot, bool insertExplicitSlot)
			{
				int start;
				kindFlags = (Inventory.Slot.KindFlags)((byte)(kindFlags & (Inventory.Slot.KindFlags.Default | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Armor)));
				temp.Count = 0;
				int num = 0;
				int num1 = 0;
				if (!explicitSlot.Any)
				{
					start = -1;
				}
				else
				{
					if (insertExplicitSlot)
					{
						Inventory.Slot.Range[] range = temp.Range;
						int count = temp.Count;
						int num2 = count;
						temp.Count = count + 1;
						range[num2] = explicitSlot;
					}
					start = explicitSlot.Start;
				}
				for (Inventory.Slot.Kind i = Inventory.Slot.Kind.Default; i < (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor); i = (Inventory.Slot.Kind)((byte)i + (byte)Inventory.Slot.Kind.Belt))
				{
					if (Inventory.Payload.RangeArray.CheckSlotKindFlag(inventory, kindFlags, (Inventory.Slot.KindFlags)((byte)((byte)Inventory.Slot.Kind.Belt << (byte)(i & (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor)))), i, ref num, ref num1))
					{
						temp.Insert(ref num, ref num1, start);
					}
				}
			}

			public struct Holder
			{
				public int Count;

				public readonly Inventory.Slot.Range[] Range;

				public Holder(Inventory.Slot.Range[] array)
				{
					this.Count = 0;
					this.Range = array;
				}

				public void Insert(ref int start, ref int count, int gougeIndex)
				{
					Inventory.Slot.RangePair rangePair;
					int num;
					int num1;
					Inventory.Slot.Range range = new Inventory.Slot.Range(start, count);
					if (gougeIndex == -1)
					{
						Inventory.Slot.Range[] rangeArray = this.Range;
						Inventory.Payload.RangeArray.Holder holder = this;
						int num2 = holder.Count;
						num = num2;
						holder.Count = num2 + 1;
						rangeArray[num] = range;
					}
					else
					{
						switch (range.Gouge(gougeIndex, out rangePair))
						{
							case 1:
							{
								Inventory.Slot.Range[] a = this.Range;
								Inventory.Payload.RangeArray.Holder holder1 = this;
								int num3 = holder1.Count;
								num1 = num3;
								holder1.Count = num3 + 1;
								a[num1] = rangePair.A;
								break;
							}
							case 2:
							{
								Inventory.Slot.Range[] range1 = this.Range;
								Inventory.Payload.RangeArray.Holder holder2 = this;
								int num4 = holder2.Count;
								num1 = num4;
								holder2.Count = num4 + 1;
								range1[num1] = rangePair.A;
								Inventory.Slot.Range[] b = this.Range;
								Inventory.Payload.RangeArray.Holder holder3 = this;
								int num5 = holder3.Count;
								num1 = num5;
								holder3.Count = num5 + 1;
								b[num1] = rangePair.B;
								break;
							}
						}
					}
					int num6 = 0;
					num = num6;
					count = num6;
					start = num;
				}
			}
		}

		public struct Result
		{
			public InventoryItem item;

			public Inventory.Payload.Result.Flags flags;

			public int usesRemaining;

			[Flags]
			public enum Flags : byte
			{
				OptionsResultedInNoOp = 0,
				NoItemDatablock = 1,
				MissingRequiredOffset = 2,
				NoSlotRanges = 3,
				FailedToCreate = 4,
				FailedToReuse = 5,
				DidNotCreate = 6,
				NoVacancy = 16,
				Stacked = 32,
				AssignedInstance = 64,
				Complete = 128
			}
		}

		private struct StackArguments
		{
			public Inventory.Collection<InventoryItem> collection;

			public Inventory.Slot.PreferenceFlags prefFlags;

			public int useCount;

			public int datablockUID;

			public bool splittable;
		}

		private enum StackResult : byte
		{
			None,
			NoneNotMarked,
			NoneUnsplittable,
			NoRange,
			None_FoundFull,
			Partial,
			Complete
		}

		private struct StackWork
		{
			public bool gotFirstUsage;

			public InventoryItem firstUsage;

			public int slot;

			public InventoryItem instance;
		}
	}

	private class Report
	{
		private int amount;

		private bool Disposed;

		private Inventory.Report dumpNext;

		private Inventory.Report typeNext;

		private Inventory.Report first;

		private ItemDataBlock datablock;

		private InventoryItem item;

		private bool splittable;

		private int length;

		private int maxUses;

		private static Inventory.Report dump;

		private static int dumpSize;

		private readonly static Dictionary<int, Inventory.Report> dict;

		private static bool begun;

		private static int totalItemCount;

		static Report()
		{
			Inventory.Report.dict = new Dictionary<int, Inventory.Report>();
		}

		public Report()
		{
		}

		public static void Begin()
		{
			if (Inventory.Report.begun)
			{
				throw new InvalidOperationException();
			}
			Inventory.Report.begun = true;
			Inventory.Report.totalItemCount = 0;
		}

		public static Inventory.Transfer[] Build(Inventory.Slot.KindFlags fallbackKindFlags)
		{
			Inventory.Transfer transfer = new Inventory.Transfer();
			if (!Inventory.Report.begun)
			{
				throw new InvalidOperationException();
			}
			Inventory.Transfer[] transferArray = new Inventory.Transfer[Inventory.Report.totalItemCount];
			int num = 0;
			foreach (KeyValuePair<int, Inventory.Report> keyValuePair in Inventory.Report.dict)
			{
				Inventory.Report value = keyValuePair.Value;
				transfer.addition.Ident = (Datablock.Ident)value.datablock;
				int num1 = value.length;
				value = value.first;
				bool flag = value.splittable;
				for (int i = 0; i < num1; i++)
				{
					transfer.addition.SlotPreference = Inventory.Slot.Preference.Define(num, false, fallbackKindFlags);
					transfer.addition.UsesQuantity = Inventory.Uses.Quantity.Manual(value.amount);
					transfer.item = value.item;
					int num2 = num;
					num = num2 + 1;
					transferArray[num2] = transfer;
					Inventory.Report report = value;
					value = value.typeNext;
					if (!report.Disposed)
					{
						report.Disposed = true;
						report.dumpNext = Inventory.Report.dump;
						object obj = null;
						Inventory.Report report1 = (Inventory.Report)obj;
						report.typeNext = (Inventory.Report)obj;
						report.first = report1;
						report.datablock = null;
						report.item = null;
						Inventory.Report.dump = report;
						Inventory.Report.dumpSize = Inventory.Report.dumpSize + 1;
					}
				}
			}
			Inventory.Report.dict.Clear();
			Inventory.Report.begun = false;
			return transferArray;
		}

		private static Inventory.Report Create()
		{
			Inventory.Report report;
			if (Inventory.Report.dumpSize <= 0)
			{
				report = new Inventory.Report();
			}
			else
			{
				report = Inventory.Report.dump;
				int num = Inventory.Report.dumpSize - 1;
				Inventory.Report.dumpSize = num;
				if (num != 0)
				{
					Inventory.Report.dump = report.dumpNext;
				}
				else
				{
					Inventory.Report.dump = null;
				}
				report.dumpNext = null;
				report.Disposed = false;
				report.amount = 0;
			}
			return report;
		}

		public static void Recover()
		{
			if (Inventory.Report.begun)
			{
				foreach (Inventory.Report value in Inventory.Report.dict.Values)
				{
					if (value.Disposed)
					{
						continue;
					}
					value.Disposed = true;
					value.dumpNext = Inventory.Report.dump;
					object obj = null;
					Inventory.Report report = (Inventory.Report)obj;
					value.typeNext = (Inventory.Report)obj;
					value.first = report;
					value.datablock = null;
					value.item = null;
					Inventory.Report.dump = value;
					Inventory.Report.dumpSize = Inventory.Report.dumpSize + 1;
				}
				Inventory.Report.dict.Clear();
			}
		}

		public static void Take(InventoryItem item)
		{
			Inventory.Report report;
			int num = item.uses;
			int num1 = item.datablockUniqueID;
			if (!Inventory.Report.dict.TryGetValue(num1, out report))
			{
				ItemDataBlock itemDataBlock = item.datablock;
				if (itemDataBlock.transferable)
				{
					Inventory.Report report1 = Inventory.Report.Create();
					report1.amount = num;
					report1.splittable = itemDataBlock.IsSplittable();
					report1.first = report1;
					report1.length = 1;
					report1.datablock = itemDataBlock;
					report1.item = item;
					if (report1.splittable)
					{
						report1.maxUses = item.maxUses;
					}
					Inventory.Report.dict.Add(item.datablockUniqueID, report1);
					Inventory.Report.totalItemCount = Inventory.Report.totalItemCount + 1;
				}
			}
			else
			{
				Inventory.Report report2 = report.first;
				if (!report.splittable)
				{
					Inventory.Report report3 = Inventory.Report.Create();
					report3.typeNext = report2;
					report3.amount = num;
					report3.item = item;
					report.first = report3;
					Inventory.Report report4 = report;
					report4.length = report4.length + 1;
					Inventory.Report.totalItemCount = Inventory.Report.totalItemCount + 1;
				}
				else
				{
					int num2 = report2.amount + num;
					if (num2 <= item.maxUses)
					{
						report.first.amount = num2;
					}
					else
					{
						Inventory.Report report5 = Inventory.Report.Create();
						report5.typeNext = report2;
						report5.amount = num2 - report.maxUses;
						report5.item = item;
						report2.amount = report.maxUses;
						report.first = report5;
						Inventory.Report report6 = report;
						report6.length = report6.length + 1;
						Inventory.Report.totalItemCount = Inventory.Report.totalItemCount + 1;
					}
				}
			}
		}
	}

	private static class Shuffle
	{
		private readonly static System.Random r;

		static Shuffle()
		{
			Inventory.Shuffle.r = new System.Random();
		}

		public static void Array<T>(T[] array)
		{
			for (int i = (int)array.Length - 1; i > 0; i--)
			{
				int num = Inventory.Shuffle.r.Next(i);
				if (num != i)
				{
					T t = array[i];
					array[i] = array[num];
					array[num] = t;
				}
			}
		}
	}

	public static class Slot
	{
		public const Inventory.Slot.Kind KindBegin = Inventory.Slot.Kind.Default;

		public const Inventory.Slot.Kind KindLast = Inventory.Slot.Kind.Armor;

		public const Inventory.Slot.Kind KindFirst = Inventory.Slot.Kind.Default;

		public const Inventory.Slot.Kind KindEnd = Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor;

		public const int KindCount = 3;

		private const Inventory.Slot.Kind HiddenKind_Explicit = 4;

		private const Inventory.Slot.Kind HiddenKind_Null = 5;

		public const int NumberOfKinds = 3;

		public const Inventory.Slot.KindFlags KindFlagsMask_Kind = Inventory.Slot.KindFlags.Default | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Armor;

		private const int PrimaryShift = 4;

		public enum Kind : byte
		{
			Default,
			Belt,
			Armor
		}

		public struct KindDictionary<TValue> : IEnumerable, IDictionary<Inventory.Slot.Kind, TValue>, ICollection<KeyValuePair<Inventory.Slot.Kind, TValue>>, IEnumerable<KeyValuePair<Inventory.Slot.Kind, TValue>>
		{
			private Inventory.Slot.KindDictionary<TValue>.Member mDefault;

			private Inventory.Slot.KindDictionary<TValue>.Member mBelt;

			private Inventory.Slot.KindDictionary<TValue>.Member mArmor;

			private sbyte count;

			public int Count
			{
				get
				{
					return (int)this.count;
				}
			}

			public TValue this[Inventory.Slot.Kind kind]
			{
				get
				{
					Inventory.Slot.KindDictionary<TValue>.Member member = this.GetMember(kind);
					if (!member.Defined)
					{
						throw new KeyNotFoundException();
					}
					return member.Value;
				}
				set
				{
					if (this.GetMember(kind).Defined)
					{
						this.SetMember(kind, new Inventory.Slot.KindDictionary<TValue>.Member(value));
					}
					else
					{
						this.SetMember(kind, new Inventory.Slot.KindDictionary<TValue>.Member(value));
						Inventory.Slot.KindDictionary<TValue> kindDictionary = this;
						kindDictionary.count = (sbyte)(kindDictionary.count + 1);
					}
				}
			}

			bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Inventory.Slot.Kind,TValue>>.IsReadOnly
			{
				get
				{
					return false;
				}
			}

			ICollection<Inventory.Slot.Kind> System.Collections.Generic.IDictionary<Inventory.Slot.Kind,TValue>.Keys
			{
				get
				{
					Inventory.Slot.Kind[] kindArray = new Inventory.Slot.Kind[(int)this.count];
					int num = 0;
					for (Inventory.Slot.Kind i = Inventory.Slot.Kind.Default; i < (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor); i = (Inventory.Slot.Kind)((byte)i + (byte)Inventory.Slot.Kind.Belt))
					{
						if (this.GetMember(i).Defined)
						{
							int num1 = num;
							num = num1 + 1;
							kindArray[num1] = i;
						}
					}
					return kindArray;
				}
			}

			ICollection<TValue> System.Collections.Generic.IDictionary<Inventory.Slot.Kind,TValue>.Values
			{
				get
				{
					TValue[] value = new TValue[(int)this.count];
					int num = 0;
					for (Inventory.Slot.Kind i = Inventory.Slot.Kind.Default; i < (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor); i = (Inventory.Slot.Kind)((byte)i + (byte)Inventory.Slot.Kind.Belt))
					{
						Inventory.Slot.KindDictionary<TValue>.Member member = this.GetMember(i);
						if (member.Defined)
						{
							int num1 = num;
							num = num1 + 1;
							value[num1] = member.Value;
						}
					}
					return value;
				}
			}

			public void Clear()
			{
				for (Inventory.Slot.Kind i = Inventory.Slot.Kind.Default; (int)this.count > 0 && i < (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor); i = (Inventory.Slot.Kind)((byte)i + (byte)Inventory.Slot.Kind.Belt))
				{
					this.Remove(i);
				}
			}

			public bool ContainsKey(Inventory.Slot.Kind key)
			{
				return (key < Inventory.Slot.Kind.Default || key >= (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor) ? false : this.GetMember(key).Defined);
			}

			public Inventory.Slot.KindDictionary<TValue>.Enumerator GetEnumerator()
			{
				return new Inventory.Slot.KindDictionary<TValue>.Enumerator(this);
			}

			private Inventory.Slot.KindDictionary<TValue>.Member GetMember(Inventory.Slot.Kind kind)
			{
				switch (kind)
				{
					case Inventory.Slot.Kind.Default:
					{
						return this.mDefault;
					}
					case Inventory.Slot.Kind.Belt:
					{
						return this.mBelt;
					}
					case Inventory.Slot.Kind.Armor:
					{
						return this.mArmor;
					}
				}
				throw new ArgumentNullException("Unimplemented kind");
			}

			public bool Remove(Inventory.Slot.Kind key)
			{
				if (!this.GetMember(key).Defined)
				{
					return false;
				}
				this.SetMember(key, new Inventory.Slot.KindDictionary<TValue>.Member());
				Inventory.Slot.KindDictionary<TValue> kindDictionary = this;
				kindDictionary.count = (sbyte)(kindDictionary.count - 1);
				return true;
			}

			private void SetMember(Inventory.Slot.Kind kind, Inventory.Slot.KindDictionary<TValue>.Member member)
			{
				switch (kind)
				{
					case Inventory.Slot.Kind.Default:
					{
						this.mDefault = member;
						break;
					}
					case Inventory.Slot.Kind.Belt:
					{
						this.mBelt = member;
						break;
					}
					case Inventory.Slot.Kind.Armor:
					{
						this.mArmor = member;
						break;
					}
					default:
					{
						throw new ArgumentNullException("Unimplemented kind");
					}
				}
			}

			void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Inventory.Slot.Kind,TValue>>.Add(KeyValuePair<Inventory.Slot.Kind, TValue> item)
			{
				this[item.Key] = item.Value;
			}

			bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Inventory.Slot.Kind,TValue>>.Contains(KeyValuePair<Inventory.Slot.Kind, TValue> item)
			{
				bool flag;
				try
				{
					Inventory.Slot.KindDictionary<TValue>.Member member = this.GetMember(item.Key);
					if (member.Defined)
					{
						KeyValuePair<Inventory.Slot.Kind, TValue> keyValuePair = new KeyValuePair<Inventory.Slot.Kind, TValue>(item.Key, member.Value);
						flag = object.Equals(keyValuePair, item);
					}
					else
					{
						flag = false;
					}
				}
				catch
				{
					flag = false;
				}
				return flag;
			}

			void System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Inventory.Slot.Kind,TValue>>.CopyTo(KeyValuePair<Inventory.Slot.Kind, TValue>[] array, int arrayIndex)
			{
				for (Inventory.Slot.Kind i = Inventory.Slot.Kind.Default; i < (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor); i = (Inventory.Slot.Kind)((byte)i + (byte)Inventory.Slot.Kind.Belt))
				{
					Inventory.Slot.KindDictionary<TValue>.Member member = this.GetMember(i);
					if (member.Defined)
					{
						int num = arrayIndex;
						arrayIndex = num + 1;
						array[num] = new KeyValuePair<Inventory.Slot.Kind, TValue>(i, member.Value);
					}
				}
			}

			bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Inventory.Slot.Kind,TValue>>.Remove(KeyValuePair<Inventory.Slot.Kind, TValue> item)
			{
				bool flag;
				try
				{
					Inventory.Slot.KindDictionary<TValue>.Member member = this.GetMember(item.Key);
					if (member.Defined)
					{
						KeyValuePair<Inventory.Slot.Kind, TValue> keyValuePair = new KeyValuePair<Inventory.Slot.Kind, TValue>(item.Key, member.Value);
						if (!object.Equals(keyValuePair, item))
						{
							flag = false;
						}
						else
						{
							this.SetMember(item.Key, new Inventory.Slot.KindDictionary<TValue>.Member());
							flag = true;
						}
					}
					else
					{
						flag = false;
					}
				}
				catch
				{
					flag = false;
				}
				return flag;
			}

			void System.Collections.Generic.IDictionary<Inventory.Slot.Kind,TValue>.Add(Inventory.Slot.Kind key, TValue value)
			{
				if (this.GetMember(key).Defined)
				{
					throw new ArgumentException("Key was already set to a value");
				}
				this.SetMember(key, new Inventory.Slot.KindDictionary<TValue>.Member(value));
				Inventory.Slot.KindDictionary<TValue> kindDictionary = this;
				kindDictionary.count = (sbyte)(kindDictionary.count + 1);
			}

			IEnumerator<KeyValuePair<Inventory.Slot.Kind, TValue>> System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<Inventory.Slot.Kind,TValue>>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public bool TryGetValue(Inventory.Slot.Kind key, out TValue value)
			{
				bool flag;
				try
				{
					Inventory.Slot.KindDictionary<TValue>.Member member = this.GetMember(key);
					if (member.Defined)
					{
						value = member.Value;
						return true;
					}
					value = default(TValue);
					return false;
				}
				catch (ArgumentNullException argumentNullException)
				{
					value = default(TValue);
					flag = false;
				}
				return flag;
			}

			public struct Enumerator : IDisposable, IEnumerator, IEnumerator<KeyValuePair<Inventory.Slot.Kind, TValue>>
			{
				private Inventory.Slot.KindDictionary<TValue> dict;

				private int kind;

				public KeyValuePair<Inventory.Slot.Kind, TValue> Current
				{
					get
					{
						Inventory.Slot.KindDictionary<TValue>.Member member = this.dict.GetMember((Inventory.Slot.Kind)((byte)this.kind));
						return new KeyValuePair<Inventory.Slot.Kind, TValue>((Inventory.Slot.Kind)((byte)this.kind), member.Value);
					}
				}

				object System.Collections.IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				public Enumerator(Inventory.Slot.KindDictionary<TValue> dict)
				{
					this.dict = dict;
					this.kind = -1;
				}

				public void Dispose()
				{
					this.dict = new Inventory.Slot.KindDictionary<TValue>();
				}

				public bool MoveNext()
				{
					Inventory.Slot.Kind kind;
					do
					{
						Inventory.Slot.KindDictionary<TValue>.Enumerator enumerator = this;
						int num = enumerator.kind + 1;
						int num1 = num;
						enumerator.kind = num;
						byte num2 = (byte)num1;
						kind = (Inventory.Slot.Kind)num2;
						if (num2 < 3)
						{
							continue;
						}
						return false;
					}
					while (!this.dict.GetMember(kind).Defined);
					return true;
				}

				public void Reset()
				{
					this.kind = -1;
				}
			}

			private struct Member
			{
				public TValue Value;

				public bool Defined;

				public Member(TValue value)
				{
					this.Value = value;
					this.Defined = true;
				}
			}
		}

		[Flags]
		public enum KindFlags : byte
		{
			Default = 1,
			Belt = 2,
			Armor = 4
		}

		public struct Offset
		{
			private Inventory.Slot.Kind kind;

			private byte offset;

			public bool ExplicitSlot
			{
				get
				{
					return (int)this.kind == 4;
				}
			}

			public bool HasOffsetOfKind
			{
				get
				{
					return this.kind < (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor);
				}
			}

			public static Inventory.Slot.Offset None
			{
				get
				{
					return new Inventory.Slot.Offset(5, 0);
				}
			}

			public Inventory.Slot.Kind OffsetOfKind
			{
				get
				{
					if (!this.HasOffsetOfKind)
					{
						throw new InvalidOperationException("You must check HasOffsetOfKind == true before requesting this value");
					}
					return this.kind;
				}
			}

			public int SlotOffset
			{
				get
				{
					return this.offset;
				}
			}

			public bool Specified
			{
				get
				{
					bool flag;
					if (this.kind < (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor))
					{
						flag = true;
					}
					else
					{
						flag = ((int)this.kind < 4 ? false : (int)this.kind < 5);
					}
					return flag;
				}
			}

			public Offset(int offset)
			{
				this.offset = (byte)offset;
				this.kind = (Inventory.Slot.Kind)4;
			}

			public Offset(Inventory.Slot.Kind kind, int offset)
			{
				this.kind = kind;
				this.offset = (byte)offset;
			}

			public override string ToString()
			{
				if (!this.Specified)
				{
					return "[Unspecified]";
				}
				if (!this.HasOffsetOfKind)
				{
					return string.Format("[{0}]", this.SlotOffset);
				}
				return string.Format("[{0}+{1}]", this.OffsetOfKind, this.SlotOffset);
			}
		}

		public struct Preference
		{
			private const bool kDefaultStack = true;

			public readonly Inventory.Slot.PreferenceFlags Flags;

			private readonly byte offset;

			public bool HasOffset
			{
				get
				{
					return (byte)(this.Flags & Inventory.Slot.PreferenceFlags.Offset) == 128;
				}
			}

			public bool IsDefined
			{
				get
				{
					return (byte)(this.Flags & (Inventory.Slot.PreferenceFlags.Secondary_Default | Inventory.Slot.PreferenceFlags.Secondary_Belt | Inventory.Slot.PreferenceFlags.Secondary_Armor | Inventory.Slot.PreferenceFlags.Primary_Default | Inventory.Slot.PreferenceFlags.Primary_Belt | Inventory.Slot.PreferenceFlags.Primary_Armor | Inventory.Slot.PreferenceFlags.Offset)) != 0;
				}
			}

			public bool IsUndefined
			{
				get
				{
					return (byte)(this.Flags & (Inventory.Slot.PreferenceFlags.Secondary_Default | Inventory.Slot.PreferenceFlags.Secondary_Belt | Inventory.Slot.PreferenceFlags.Secondary_Armor | Inventory.Slot.PreferenceFlags.Primary_Default | Inventory.Slot.PreferenceFlags.Primary_Belt | Inventory.Slot.PreferenceFlags.Primary_Armor | Inventory.Slot.PreferenceFlags.Offset)) == 0;
				}
			}

			public Inventory.Slot.Offset Offset
			{
				get
				{
					if ((byte)(this.Flags & Inventory.Slot.PreferenceFlags.Offset) == 128)
					{
						uint flags = (uint)((byte)(this.Flags & (Inventory.Slot.PreferenceFlags.Secondary_Default | Inventory.Slot.PreferenceFlags.Secondary_Belt | Inventory.Slot.PreferenceFlags.Secondary_Armor | Inventory.Slot.PreferenceFlags.Stack | Inventory.Slot.PreferenceFlags.Primary_Default | Inventory.Slot.PreferenceFlags.Primary_Belt | Inventory.Slot.PreferenceFlags.Primary_Armor)) >> 4);
						if (flags == 0)
						{
							return new Inventory.Slot.Offset((int)this.offset);
						}
						if ((flags & flags - 1) == 0)
						{
							Inventory.Slot.Kind kind = Inventory.Slot.Kind.Default;
							while (true)
							{
								UInt32 num = flags >> 1;
								flags = num;
								if (num == 0)
								{
									break;
								}
								kind = (Inventory.Slot.Kind)((byte)kind + (byte)Inventory.Slot.Kind.Belt);
							}
							return new Inventory.Slot.Offset(kind, (int)this.offset);
						}
					}
					return Inventory.Slot.Offset.None;
				}
			}

			public Inventory.Slot.KindFlags PrimaryKindFlags
			{
				get
				{
					return (Inventory.Slot.KindFlags)((byte)((byte)((byte)this.Flags >> (byte)Inventory.Slot.PreferenceFlags.Secondary_Armor) & 7));
				}
			}

			public Inventory.Slot.KindFlags SecondaryKindFlags
			{
				get
				{
					return (Inventory.Slot.KindFlags)((byte)(this.Flags & (Inventory.Slot.PreferenceFlags.Secondary_Default | Inventory.Slot.PreferenceFlags.Secondary_Belt | Inventory.Slot.PreferenceFlags.Secondary_Armor)));
				}
			}

			public bool Stack
			{
				get
				{
					return (byte)(this.Flags & Inventory.Slot.PreferenceFlags.Stack) == 8;
				}
			}

			private Preference(Inventory.Slot.PreferenceFlags preferenceFlags, int primaryOffset)
			{
				this.Flags = preferenceFlags;
				this.offset = (byte)primaryOffset;
			}

			public Inventory.Slot.Preference CloneOffsetChange(int newOffset)
			{
				return new Inventory.Slot.Preference(this.Flags, newOffset);
			}

			public Inventory.Slot.Preference CloneStackChange(bool stack)
			{
				if (stack)
				{
					return new Inventory.Slot.Preference((Inventory.Slot.PreferenceFlags)((byte)(this.Flags | Inventory.Slot.PreferenceFlags.Stack)), (int)this.offset);
				}
				return new Inventory.Slot.Preference((Inventory.Slot.PreferenceFlags)((byte)(this.Flags & (Inventory.Slot.PreferenceFlags.Secondary_Default | Inventory.Slot.PreferenceFlags.Secondary_Belt | Inventory.Slot.PreferenceFlags.Secondary_Armor | Inventory.Slot.PreferenceFlags.Primary_Default | Inventory.Slot.PreferenceFlags.Primary_Belt | Inventory.Slot.PreferenceFlags.Primary_Armor | Inventory.Slot.PreferenceFlags.Offset))), (int)this.offset);
			}

			public static Inventory.Slot.Preference Define(int slotNumber, bool stack, Inventory.Slot.KindFlags fallbackSlots)
			{
				Inventory.Slot.PreferenceFlags preferenceFlag = (Inventory.Slot.PreferenceFlags)((byte)(fallbackSlots & (Inventory.Slot.KindFlags.Default | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Armor)));
				if (stack)
				{
					preferenceFlag = (Inventory.Slot.PreferenceFlags)((byte)(preferenceFlag | Inventory.Slot.PreferenceFlags.Stack));
				}
				if (slotNumber < 0)
				{
					slotNumber = 0;
				}
				else
				{
					preferenceFlag = (Inventory.Slot.PreferenceFlags)((byte)(preferenceFlag | Inventory.Slot.PreferenceFlags.Offset));
				}
				return new Inventory.Slot.Preference(preferenceFlag, slotNumber);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.Kind startSlotKind, int offsetOfSlotKind, bool stack, Inventory.Slot.KindFlags fallbackSlotKinds)
			{
				Inventory.Slot.PreferenceFlags preferenceFlag = (Inventory.Slot.PreferenceFlags)((byte)(fallbackSlotKinds & (Inventory.Slot.KindFlags.Default | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Armor)));
				if (stack)
				{
					preferenceFlag = (Inventory.Slot.PreferenceFlags)((byte)(preferenceFlag | Inventory.Slot.PreferenceFlags.Stack));
				}
				if (offsetOfSlotKind < 0)
				{
					offsetOfSlotKind = 0;
				}
				else
				{
					preferenceFlag = (Inventory.Slot.PreferenceFlags)((byte)(preferenceFlag | Inventory.Slot.PreferenceFlags.Offset));
				}
				Inventory.Slot.PreferenceFlags preferenceFlag1 = (Inventory.Slot.PreferenceFlags)((byte)((byte)Inventory.Slot.Kind.Belt << (byte)(startSlotKind & (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor))));
				preferenceFlag = (Inventory.Slot.PreferenceFlags)((byte)((byte)preferenceFlag & (byte)(~preferenceFlag1)));
				preferenceFlag = (Inventory.Slot.PreferenceFlags)((byte)((byte)preferenceFlag | (byte)((byte)preferenceFlag1 << (byte)Inventory.Slot.PreferenceFlags.Secondary_Armor)));
				return new Inventory.Slot.Preference(preferenceFlag, offsetOfSlotKind);
			}

			public static Inventory.Slot.Preference Define(int offsetOfSlotKind, bool stack)
			{
				return Inventory.Slot.Preference.Define(offsetOfSlotKind, stack, 0);
			}

			public static Inventory.Slot.Preference Define(int offsetOfSlotKind, Inventory.Slot.KindFlags fallbackSlotKinds)
			{
				return Inventory.Slot.Preference.Define(offsetOfSlotKind, true, fallbackSlotKinds);
			}

			public static Inventory.Slot.Preference Define(int offsetOfSlotKind, Inventory.Slot.Kind fallbackSlotKind)
			{
				return Inventory.Slot.Preference.Define(offsetOfSlotKind, true, (Inventory.Slot.KindFlags)((byte)((byte)Inventory.Slot.Kind.Belt << (byte)(fallbackSlotKind & (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor)))));
			}

			public static Inventory.Slot.Preference Define(int offsetOfSlotKind)
			{
				return Inventory.Slot.Preference.Define(offsetOfSlotKind, true, 0);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.Kind startSlotKind, int offsetOfSlotKind, bool stack)
			{
				return Inventory.Slot.Preference.Define(startSlotKind, offsetOfSlotKind, stack, 0);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.Kind startSlotKind, int offsetOfSlotKind, Inventory.Slot.KindFlags fallbackSlotKinds)
			{
				return Inventory.Slot.Preference.Define(startSlotKind, offsetOfSlotKind, true, fallbackSlotKinds);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.Kind startSlotKind, int offsetOfSlotKind, Inventory.Slot.Kind fallbackSlotKind)
			{
				return Inventory.Slot.Preference.Define(startSlotKind, offsetOfSlotKind, true, (Inventory.Slot.KindFlags)((byte)((byte)Inventory.Slot.Kind.Belt << (byte)(fallbackSlotKind & (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor)))));
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.Kind startSlotKind, int offsetOfSlotKind)
			{
				return Inventory.Slot.Preference.Define(startSlotKind, offsetOfSlotKind, true, 0);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.KindFlags firstPreferenceSlotKinds, bool stack, Inventory.Slot.KindFlags secondPreferenceSlotKinds)
			{
				Inventory.Slot.PreferenceFlags preferenceFlag = (Inventory.Slot.PreferenceFlags)((byte)((byte)(secondPreferenceSlotKinds & (Inventory.Slot.KindFlags.Default | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Armor)) & (byte)(~firstPreferenceSlotKinds)));
				if (stack)
				{
					preferenceFlag = (Inventory.Slot.PreferenceFlags)((byte)(preferenceFlag | Inventory.Slot.PreferenceFlags.Stack));
				}
				preferenceFlag = (Inventory.Slot.PreferenceFlags)((byte)((byte)preferenceFlag | (byte)((byte)firstPreferenceSlotKinds << (byte)Inventory.Slot.KindFlags.Armor)));
				return new Inventory.Slot.Preference(preferenceFlag, 0);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.Kind firstPreferenceSlotKind, bool stack, Inventory.Slot.KindFlags secondPreferenceSlotKinds)
			{
				return Inventory.Slot.Preference.Define((Inventory.Slot.KindFlags)((byte)((byte)Inventory.Slot.Kind.Belt << (byte)(firstPreferenceSlotKind & (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor)))), stack, secondPreferenceSlotKinds);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.Kind firstPreferenceSlotKind, bool stack, Inventory.Slot.Kind secondPreferenceSlotKind)
			{
				return Inventory.Slot.Preference.Define((Inventory.Slot.KindFlags)((byte)((byte)Inventory.Slot.Kind.Belt << (byte)(firstPreferenceSlotKind & (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor)))), stack, (Inventory.Slot.KindFlags)((byte)((byte)Inventory.Slot.Kind.Belt << (byte)(secondPreferenceSlotKind & (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor)))));
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.KindFlags firstPreferenceSlotKind, bool stack, Inventory.Slot.Kind secondPreferenceSlotKind)
			{
				return Inventory.Slot.Preference.Define(firstPreferenceSlotKind, stack, (Inventory.Slot.KindFlags)((byte)((byte)Inventory.Slot.Kind.Belt << (byte)(secondPreferenceSlotKind & (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor)))));
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.Kind slotsOfKind, bool stack)
			{
				return Inventory.Slot.Preference.Define(slotsOfKind, stack, 0);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.KindFlags slotsOfKinds, bool stack)
			{
				return Inventory.Slot.Preference.Define(slotsOfKinds, stack, 0);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.Kind firstPreferenceSlotKind, Inventory.Slot.Kind secondPreferenceSlotKind)
			{
				return Inventory.Slot.Preference.Define(firstPreferenceSlotKind, true, secondPreferenceSlotKind);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.KindFlags firstPreferenceSlotKinds, Inventory.Slot.KindFlags secondPreferenceSlotKinds)
			{
				return Inventory.Slot.Preference.Define(firstPreferenceSlotKinds, true, secondPreferenceSlotKinds);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.Kind firstPreferenceSlotKind, Inventory.Slot.KindFlags secondPreferenceSlotKinds)
			{
				return Inventory.Slot.Preference.Define(firstPreferenceSlotKind, true, secondPreferenceSlotKinds);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.KindFlags firstPreferenceSlotKinds, Inventory.Slot.Kind secondPreferenceSlotKind)
			{
				return Inventory.Slot.Preference.Define(firstPreferenceSlotKinds, true, secondPreferenceSlotKind);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.Kind slotsOfKind)
			{
				return Inventory.Slot.Preference.Define(slotsOfKind, true, 0);
			}

			public static Inventory.Slot.Preference Define(Inventory.Slot.KindFlags slotsOfKinds)
			{
				return Inventory.Slot.Preference.Define(slotsOfKinds, true, 0);
			}

			public static implicit operator Preference(int slot)
			{
				return new Inventory.Slot.Preference(Inventory.Slot.PreferenceFlags.Stack | Inventory.Slot.PreferenceFlags.Offset, (int)slot);
			}

			public static implicit operator Preference(Inventory.Slot.Kind kind)
			{
				return new Inventory.Slot.Preference((Inventory.Slot.PreferenceFlags)((byte)((byte)((byte)((byte)((byte)Inventory.Slot.Kind.Belt << (byte)(kind & (Inventory.Slot.Kind.Belt | Inventory.Slot.Kind.Armor))) & 7) << 4) | 8)), 0);
			}

			public static implicit operator Preference(Inventory.Slot.KindFlags kindFlags)
			{
				return new Inventory.Slot.Preference((Inventory.Slot.PreferenceFlags)((byte)((byte)((byte)(kindFlags & (Inventory.Slot.KindFlags.Default | Inventory.Slot.KindFlags.Belt | Inventory.Slot.KindFlags.Armor)) << 4) | 8)), 0);
			}

			public override string ToString()
			{
				Inventory.Slot.KindFlags primaryKindFlags = this.PrimaryKindFlags;
				Inventory.Slot.KindFlags secondaryKindFlags = this.SecondaryKindFlags;
				Inventory.Slot.Offset offset = this.Offset;
				if ((int)secondaryKindFlags == 0)
				{
					if (!offset.Specified)
					{
						if ((int)primaryKindFlags == 0)
						{
							return "[Undefined]";
						}
						if (this.Stack)
						{
							return string.Format("[{0} (stack)]", primaryKindFlags);
						}
						return string.Format("[{0}]", primaryKindFlags);
					}
					if (!offset.HasOffsetOfKind)
					{
						if (this.Stack)
						{
							return string.Format("[{0} (stack)]", offset.SlotOffset);
						}
						return string.Format("[{0}]", offset.SlotOffset);
					}
					if (this.Stack)
					{
						return string.Format("[{0}+{1} (stack)]", offset.OffsetOfKind, offset.SlotOffset);
					}
					return string.Format("[{0}+{1}]", offset.OffsetOfKind, offset.SlotOffset);
				}
				if (!offset.Specified)
				{
					if ((int)primaryKindFlags == 0)
					{
						if (this.Stack)
						{
							return string.Format("[|{1} (stack)]", secondaryKindFlags);
						}
						return string.Format("[|{1}]", secondaryKindFlags);
					}
					if (this.Stack)
					{
						return string.Format("[{0}|{1} (stack)]", primaryKindFlags, secondaryKindFlags);
					}
					return string.Format("[{0}|{1}]", primaryKindFlags, secondaryKindFlags);
				}
				if (!offset.HasOffsetOfKind)
				{
					if (this.Stack)
					{
						return string.Format("[{0}|{1} (stack)]", offset.SlotOffset, secondaryKindFlags);
					}
					return string.Format("[{0}|{1}]", offset.SlotOffset, secondaryKindFlags);
				}
				if (this.Stack)
				{
					return string.Format("[{0}+{1}|{2} (stack)]", offset.OffsetOfKind, offset.SlotOffset, secondaryKindFlags);
				}
				return string.Format("[{0}+{1}|{2}]", offset.OffsetOfKind, offset.SlotOffset, secondaryKindFlags);
			}
		}

		[Flags]
		public enum PreferenceFlags : byte
		{
			Primary_ExplicitSlot = 0,
			Secondary_Default = 1,
			Secondary_Belt = 2,
			Secondary_Armor = 4,
			Stack = 8,
			Primary_Default = 16,
			Primary_Belt = 32,
			Primary_Armor = 64,
			Offset = 128
		}

		public struct Range
		{
			public readonly int Start;

			public readonly int Count;

			public bool Any
			{
				get
				{
					return this.Count > 0;
				}
			}

			public int End
			{
				get
				{
					return this.Start + this.Count;
				}
			}

			public int Last
			{
				get
				{
					return (this.Count > 1 ? this.Start + (this.Count - 1) : this.Start);
				}
			}

			public Range(int start, int length)
			{
				this.Start = start;
				this.Count = length;
			}

			public sbyte ContainEx(int i)
			{
				if (this.Start > i)
				{
					return -1;
				}
				if (i - this.Start < this.Count)
				{
					return 0;
				}
				return 1;
			}

			public bool Contains(int i)
			{
				bool flag;
				if (this.Count <= 0)
				{
					flag = false;
				}
				else if (this.Start == i)
				{
					flag = true;
				}
				else
				{
					flag = (this.Start >= i ? false : this.Start + this.Count > i);
				}
				return flag;
			}

			public int GetOffset(int i)
			{
				if (!this.Contains(i))
				{
					return -1;
				}
				return i - this.Start;
			}

			public int Gouge(int i, out Inventory.Slot.RangePair pair)
			{
				if (this.Count <= 0 || this.Count == 1 && i == this.Start)
				{
					pair = new Inventory.Slot.RangePair();
					return 0;
				}
				if (i < this.Start || i >= this.Start + this.Count)
				{
					pair = new Inventory.Slot.RangePair(this);
					return 1;
				}
				if (i == this.Start)
				{
					pair = new Inventory.Slot.RangePair(new Inventory.Slot.Range(this.Start + 1, this.Count - 1));
					return 1;
				}
				if (i == this.Start + this.Count - 1)
				{
					pair = new Inventory.Slot.RangePair(new Inventory.Slot.Range(this.Start, this.Count - 1));
					return 1;
				}
				pair = new Inventory.Slot.RangePair(new Inventory.Slot.Range(this.Start, i - this.Start), new Inventory.Slot.Range(i + 1, this.Count - (i - this.Start + 1)));
				return 2;
			}

			public int Index(int offset)
			{
				int start = this.Start + offset;
				return (!this.Contains(start) ? -1 : start);
			}

			public override string ToString()
			{
				return string.Format("[{0}:{1}]", this.Start, this.Count);
			}
		}

		public struct RangePair
		{
			public readonly Inventory.Slot.Range A;

			public readonly Inventory.Slot.Range B;

			public RangePair(Inventory.Slot.Range A, Inventory.Slot.Range B)
			{
				this.A = A;
				this.B = B;
			}

			public RangePair(Inventory.Slot.Range AB)
			{
				this.A = AB;
				this.B = AB;
			}
		}
	}

	[Flags]
	public enum SlotFlags
	{
		Safe = -2147483648,
		Belt = 1,
		Storage = 2,
		Equip = 4,
		Head = 8,
		Chest = 16,
		Legs = 32,
		Feet = 64,
		FuelBasic = 128,
		Debris = 256,
		Raw = 512,
		Cooked = 1024
	}

	public enum SlotOperationResult : sbyte
	{
		Error_OccupiedDestination = -8,
		Error_SameSlot = -7,
		Error_MissingInventory = -6,
		Error_EmptySourceSlot = -5,
		Error_EmptyDestinationSlot = -4,
		Error_SlotRange = -3,
		Error_NoOpArgs = -2,
		Error_Failed = -1,
		NoOp = 0,
		Success_Stacked = 1,
		Success_Combined = 2,
		Success_Moved = 4
	}

	private enum SlotOperations : byte
	{
		Stack = 1,
		Combine = 2,
		Move = 4
	}

	private struct SlotOperationsInfo
	{
		[NonSerialized]
		public readonly Inventory.SlotOperations SlotOperations;

		public SlotOperationsInfo(Inventory.SlotOperations SlotOperations)
		{
			this.SlotOperations = SlotOperations;
		}

		public override bool Equals(object obj)
		{
			return (!(obj is Inventory.SlotOperationsInfo) ? false : this.Equals((Inventory.SlotOperationsInfo)obj));
		}

		public bool Equals(Inventory.SlotOperationsInfo other)
		{
			return (byte)(this.SlotOperations & (Inventory.SlotOperations.Stack | Inventory.SlotOperations.Combine | Inventory.SlotOperations.Move)) == (byte)(other.SlotOperations & (Inventory.SlotOperations.Stack | Inventory.SlotOperations.Combine | Inventory.SlotOperations.Move));
		}

		public override int GetHashCode()
		{
			return (byte)(this.SlotOperations & (Inventory.SlotOperations.Stack | Inventory.SlotOperations.Combine | Inventory.SlotOperations.Move)) << 16;
		}

		public static implicit operator SlotOperationsInfo(Inventory.SlotOperations ops)
		{
			return new Inventory.SlotOperationsInfo(ops);
		}

		public override string ToString()
		{
			return this.SlotOperations.ToString();
		}
	}

	public struct Transfer
	{
		public InventoryItem item;

		public Inventory.Addition addition;
	}

	public static class Uses
	{
		public enum Quantifier : byte
		{
			Default,
			Manual,
			Minimum,
			Maximum,
			StackSize,
			Random
		}

		public struct Quantity
		{
			public readonly Inventory.Uses.Quantifier Quantifier;

			private readonly byte manualAmount;

			public readonly static Inventory.Uses.Quantity Default;

			public readonly static Inventory.Uses.Quantity Minimum;

			public readonly static Inventory.Uses.Quantity Maximum;

			public readonly static Inventory.Uses.Quantity Random;

			public int ManualAmount
			{
				get
				{
					if (this.Quantifier != Inventory.Uses.Quantifier.Manual)
					{
						return -1;
					}
					return this.manualAmount;
				}
			}

			static Quantity()
			{
				Inventory.Uses.Quantity.Default = new Inventory.Uses.Quantity(Inventory.Uses.Quantifier.Default, 0);
				Inventory.Uses.Quantity.Minimum = new Inventory.Uses.Quantity(Inventory.Uses.Quantifier.Minimum, 0);
				Inventory.Uses.Quantity.Maximum = new Inventory.Uses.Quantity(Inventory.Uses.Quantifier.Maximum, 0);
				Inventory.Uses.Quantity.Random = new Inventory.Uses.Quantity(Inventory.Uses.Quantifier.Random, 0);
			}

			private Quantity(Inventory.Uses.Quantifier quantifier, byte manualAmount)
			{
				this.Quantifier = quantifier;
				this.manualAmount = manualAmount;
			}

			public int CalculateCount(ItemDataBlock datablock)
			{
				int num;
				switch (this.Quantifier)
				{
					case Inventory.Uses.Quantifier.Default:
					{
						return datablock._spawnUsesMin + (datablock._spawnUsesMax - datablock._spawnUsesMin) / 2;
					}
					case Inventory.Uses.Quantifier.Manual:
					{
						if (this.manualAmount != 0)
						{
							num = (this.manualAmount <= datablock._maxUses ? (int)this.manualAmount : datablock._maxUses);
						}
						else
						{
							num = 1;
						}
						return num;
					}
					case Inventory.Uses.Quantifier.Minimum:
					{
						return datablock._spawnUsesMin;
					}
					case Inventory.Uses.Quantifier.Maximum:
					{
						return datablock._spawnUsesMax;
					}
					case Inventory.Uses.Quantifier.StackSize:
					{
						return datablock._maxUses;
					}
					case Inventory.Uses.Quantifier.Random:
					{
						return UnityEngine.Random.Range(datablock._spawnUsesMin, datablock._spawnUsesMax + 1);
					}
				}
				throw new NotImplementedException();
			}

			public static Inventory.Uses.Quantity Manual(int amount)
			{
				return new Inventory.Uses.Quantity(Inventory.Uses.Quantifier.Manual, (byte)amount);
			}

			public static implicit operator Quantity(int amount)
			{
				return Inventory.Uses.Quantity.Manual(amount);
			}

			public override string ToString()
			{
				if (this.Quantifier != Inventory.Uses.Quantifier.Manual)
				{
					return this.Quantifier.ToString();
				}
				return this.manualAmount.ToString();
			}

			public static bool TryParse(string text, out Inventory.Uses.Quantity uses)
			{
				int num;
				bool flag;
				if (int.TryParse(text, out num))
				{
					if (num == 0)
					{
						uses = Inventory.Uses.Quantity.Random;
					}
					else if (num < 0)
					{
						uses = Inventory.Uses.Quantity.Minimum;
					}
					else if (num <= 255)
					{
						uses = num;
					}
					else
					{
						uses = Inventory.Uses.Quantity.Maximum;
					}
					return true;
				}
				if (string.Equals(text, "min", StringComparison.InvariantCultureIgnoreCase))
				{
					uses = Inventory.Uses.Quantity.Minimum;
					return true;
				}
				if (string.Equals(text, "max", StringComparison.InvariantCultureIgnoreCase))
				{
					uses = Inventory.Uses.Quantity.Maximum;
					return true;
				}
				try
				{
					switch ((byte)Enum.Parse(typeof(Inventory.Uses.Quantifier), text, true))
					{
						case 0:
						{
							uses = Inventory.Uses.Quantity.Default;
							flag = true;
							break;
						}
						case 1:
						case 4:
						{
							throw new NotImplementedException();
						}
						case 2:
						{
							uses = Inventory.Uses.Quantity.Minimum;
							flag = true;
							break;
						}
						case 3:
						{
							uses = Inventory.Uses.Quantity.Maximum;
							flag = true;
							break;
						}
						case 5:
						{
							uses = Inventory.Uses.Quantity.Random;
							flag = true;
							break;
						}
						default:
						{
							throw new NotImplementedException();
						}
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
					uses = Inventory.Uses.Quantity.Default;
					flag = false;
				}
				return flag;
			}
		}
	}

	public struct VacantIterator : IDisposable
	{
		private Inventory.Collection<InventoryItem>.VacantCollection.Enumerator baseEnumerator;

		public int slot
		{
			get
			{
				return this.baseEnumerator.Current;
			}
		}

		public VacantIterator(Inventory inventory)
		{
			this.baseEnumerator = inventory.collection.VacantEnumerator;
		}

		public void Dispose()
		{
			this.baseEnumerator.Dispose();
		}

		public bool Next()
		{
			return this.baseEnumerator.MoveNext();
		}

		public bool Next(out int slot)
		{
			if (!this.Next())
			{
				slot = -1;
				return false;
			}
			slot = this.baseEnumerator.Current;
			return true;
		}

		public void Reset()
		{
			this.baseEnumerator.Reset();
		}
	}
}