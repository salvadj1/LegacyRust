using System;
using System.Collections.Generic;
using uLink;
using UnityEngine;

public class BlueprintDataBlock : ToolDataBlock
{
	public ItemDataBlock resultItem;

	public int numResultItem = 1;

	public BlueprintDataBlock.IngredientEntry[] ingredients;

	public static BlueprintDataBlock.SlotChanceWeightedEntry[] defaultSlotChances;

	public static bool chancesInitalized;

	public float craftingDuration = 20f;

	public bool RequireWorkbench;

	private List<int> lastCanWorkResult;

	private List<int> lastCanWorkIngredientCount;

	static BlueprintDataBlock()
	{
	}

	public BlueprintDataBlock()
	{
		this.icon = "Items/BlueprintIcon";
	}

	public virtual bool CanWork(int amount, Inventory workbenchInv)
	{
		if (this.lastCanWorkResult != null)
		{
			this.lastCanWorkResult.Clear();
		}
		else
		{
			this.lastCanWorkResult = new List<int>();
		}
		if (this.lastCanWorkIngredientCount != null)
		{
			this.lastCanWorkIngredientCount.Clear();
		}
		else
		{
			this.lastCanWorkIngredientCount = new List<int>((int)this.ingredients.Length);
		}
		if (this.RequireWorkbench)
		{
			CraftingInventory component = workbenchInv.GetComponent<CraftingInventory>();
			if (!component || !component.AtWorkBench())
			{
				return false;
			}
		}
		BlueprintDataBlock.IngredientEntry[] ingredientEntryArray = this.ingredients;
		for (int i = 0; i < (int)ingredientEntryArray.Length; i++)
		{
			BlueprintDataBlock.IngredientEntry ingredientEntry = ingredientEntryArray[i];
			if (ingredientEntry.amount != 0)
			{
				int num = workbenchInv.CanConsume(ingredientEntry.Ingredient, ingredientEntry.amount * amount, this.lastCanWorkResult);
				if (num <= 0)
				{
					this.lastCanWorkResult.Clear();
					this.lastCanWorkIngredientCount.Clear();
					return false;
				}
				this.lastCanWorkIngredientCount.Add(num);
			}
		}
		return true;
	}

	public virtual bool CompleteWork(int amount, Inventory workbenchInv)
	{
		IInventoryItem inventoryItem;
		if (!this.CanWork(amount, workbenchInv))
		{
			return false;
		}
		int num = 0;
		for (int i = 0; i < (int)this.ingredients.Length; i++)
		{
			int num1 = this.ingredients[i].amount * amount;
			if (num1 != 0)
			{
				int item = this.lastCanWorkIngredientCount[i];
				for (int j = 0; j < item; j++)
				{
					int num2 = num;
					num = num2 + 1;
					int item1 = this.lastCanWorkResult[num2];
					if (workbenchInv.GetItem(item1, out inventoryItem) && inventoryItem.Consume(ref num1))
					{
						workbenchInv.RemoveItem(item1);
					}
				}
			}
		}
		workbenchInv.AddItemAmount(this.resultItem, amount * this.numResultItem);
		return true;
	}

	protected override IInventoryItem ConstructItem()
	{
		return new BlueprintDataBlock.ITEM_TYPE(this);
	}

	public virtual void DefaultChancesInit()
	{
		if (!BlueprintDataBlock.chancesInitalized)
		{
			BlueprintDataBlock.chancesInitalized = true;
			BlueprintDataBlock.defaultSlotChances = new BlueprintDataBlock.SlotChanceWeightedEntry[5];
			BlueprintDataBlock.defaultSlotChances[0].numSlots = 1;
			BlueprintDataBlock.defaultSlotChances[1].numSlots = 2;
			BlueprintDataBlock.defaultSlotChances[2].numSlots = 3;
			BlueprintDataBlock.defaultSlotChances[3].numSlots = 4;
			BlueprintDataBlock.defaultSlotChances[4].numSlots = 5;
			BlueprintDataBlock.defaultSlotChances[0].weight = 50f;
			BlueprintDataBlock.defaultSlotChances[1].weight = 40f;
			BlueprintDataBlock.defaultSlotChances[2].weight = 30f;
			BlueprintDataBlock.defaultSlotChances[3].weight = 20f;
			BlueprintDataBlock.defaultSlotChances[4].weight = 10f;
		}
	}

	public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option, IInventoryItem item)
	{
		if (option == InventoryItem.MenuItem.Study)
		{
			return InventoryItem.MenuItemResult.DoneOnServer;
		}
		return base.ExecuteMenuOption(option, item);
	}

	public static bool FindBlueprintForItem<T>(ItemDataBlock item, out T blueprint)
	where T : BlueprintDataBlock
	{
		ItemDataBlock[] all = DatablockDictionary.All;
		for (int i = 0; i < (int)all.Length; i++)
		{
			T t = (T)(all[i] as T);
			if (t && t.resultItem == item)
			{
				blueprint = t;
				return true;
			}
		}
		Debug.LogWarning("Could not find blueprint foritem");
		blueprint = (T)null;
		return false;
	}

	public static bool FindBlueprintForItem(ItemDataBlock item)
	{
		BlueprintDataBlock blueprintDataBlock;
		return BlueprintDataBlock.FindBlueprintForItem<BlueprintDataBlock>(item, out blueprintDataBlock);
	}

	public override string GetItemDescription()
	{
		return "This is an item Blueprint. Study it to learn how to craft the item it represents!";
	}

	public override float GetWorkDuration(IToolItem tool)
	{
		return this.craftingDuration;
	}

	public override void InstallData(IInventoryItem item)
	{
		base.InstallData(item);
	}

	public virtual int MaxAmount(Inventory workbenchInv)
	{
		int num = 2147483647;
		BlueprintDataBlock.IngredientEntry[] ingredientEntryArray = this.ingredients;
		for (int i = 0; i < (int)ingredientEntryArray.Length; i++)
		{
			BlueprintDataBlock.IngredientEntry ingredientEntry = ingredientEntryArray[i];
			int num1 = 0;
			if (workbenchInv.FindItem(ingredientEntry.Ingredient, out num1) != null)
			{
				int num2 = num1 / ingredientEntry.amount;
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		return (num != 2147483647 ? num : 0);
	}

	public override void PopulateInfoWindow(ItemToolTip infoWindow, IInventoryItem tipItem)
	{
		infoWindow.AddItemTitle(this, tipItem, 0f);
		infoWindow.AddSectionTitle("Ingredients", 15f);
		for (int i = 0; i < (int)this.ingredients.Length; i++)
		{
			string ingredient = this.ingredients[i].Ingredient.name;
			if (this.ingredients[i].amount > 1)
			{
				ingredient = string.Concat(ingredient, " x", this.ingredients[i].amount);
			}
			infoWindow.AddBasicLabel(ingredient, 15f);
		}
		infoWindow.AddSectionTitle("Result Item", 15f);
		infoWindow.AddBasicLabel(this.resultItem.name, 15f);
		infoWindow.AddItemDescription(this, 15f);
		infoWindow.FinishPopulating();
	}

	public override int RetreiveMenuOptions(IInventoryItem item, InventoryItem.MenuItem[] results, int offset)
	{
		offset = base.RetreiveMenuOptions(item, results, offset);
		if (item.isInLocalInventory)
		{
			int num = offset;
			offset = num + 1;
			results[num] = InventoryItem.MenuItem.Study;
		}
		return offset;
	}

	protected override void SecureWriteMemberValues(uLink.BitStream stream)
	{
		base.SecureWriteMemberValues(stream);
		stream.Write<float>(this.craftingDuration, new object[0]);
		if (this.ingredients != null)
		{
			BlueprintDataBlock.IngredientEntry[] ingredientEntryArray = this.ingredients;
			for (int i = 0; i < (int)ingredientEntryArray.Length; i++)
			{
				BlueprintDataBlock.IngredientEntry ingredientEntry = ingredientEntryArray[i];
				if (ingredientEntry != null)
				{
					if (!ingredientEntry.Ingredient)
					{
						stream.Write<int>(ingredientEntry.amount, new object[0]);
					}
					else
					{
						stream.Write<int>(ingredientEntry.Ingredient.uniqueID ^ ingredientEntry.amount, new object[0]);
					}
				}
			}
		}
		if (this.resultItem)
		{
			stream.Write<int>(this.resultItem.uniqueID, new object[0]);
		}
		if (BlueprintDataBlock.defaultSlotChances != null)
		{
			BlueprintDataBlock.SlotChanceWeightedEntry[] slotChanceWeightedEntryArray = BlueprintDataBlock.defaultSlotChances;
			for (int j = 0; j < (int)slotChanceWeightedEntryArray.Length; j++)
			{
				stream.Write<float>(slotChanceWeightedEntryArray[j].weight, new object[0]);
			}
			BlueprintDataBlock.SlotChanceWeightedEntry[] slotChanceWeightedEntryArray1 = BlueprintDataBlock.defaultSlotChances;
			for (int k = 0; k < (int)slotChanceWeightedEntryArray1.Length; k++)
			{
				stream.Write<byte>(slotChanceWeightedEntryArray1[k].numSlots, new object[0]);
			}
		}
	}

	public virtual void UseItem(IBlueprintItem item)
	{
	}

	[Serializable]
	public class IngredientEntry
	{
		public ItemDataBlock Ingredient;

		public int amount;

		public IngredientEntry()
		{
		}
	}

	private sealed class ITEM_TYPE : BlueprintItem<BlueprintDataBlock>, IBlueprintItem, IInventoryItem, IToolItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(BlueprintDataBlock BLOCK) : base(BLOCK)
		{
		}

		// privatescope
		internal int IInventoryItem.AddUses(int count)
		{
			return base.AddUses(count);
		}

		// privatescope
		internal bool IInventoryItem.Consume(ref int count)
		{
			return base.Consume(ref count);
		}

		// privatescope
		internal void IInventoryItem.Deserialize(uLink.BitStream stream)
		{
			base.Deserialize(stream);
		}

		// privatescope
		internal bool IInventoryItem.get_active()
		{
			return base.active;
		}

		// privatescope
		internal Character IInventoryItem.get_character()
		{
			return base.character;
		}

		// privatescope
		internal float IInventoryItem.get_condition()
		{
			return base.condition;
		}

		// privatescope
		internal Controllable IInventoryItem.get_controllable()
		{
			return base.controllable;
		}

		// privatescope
		internal Controller IInventoryItem.get_controller()
		{
			return base.controller;
		}

		// privatescope
		internal bool IInventoryItem.get_dirty()
		{
			return base.dirty;
		}

		// privatescope
		internal bool IInventoryItem.get_doNotSave()
		{
			return base.doNotSave;
		}

		// privatescope
		internal IDMain IInventoryItem.get_idMain()
		{
			return base.idMain;
		}

		// privatescope
		internal Inventory IInventoryItem.get_inventory()
		{
			return base.inventory;
		}

		// privatescope
		internal bool IInventoryItem.get_isInLocalInventory()
		{
			return base.isInLocalInventory;
		}

		// privatescope
		internal float IInventoryItem.get_lastUseTime()
		{
			return base.lastUseTime;
		}

		// privatescope
		internal float IInventoryItem.get_maxcondition()
		{
			return base.maxcondition;
		}

		// privatescope
		internal int IInventoryItem.get_slot()
		{
			return base.slot;
		}

		// privatescope
		internal int IInventoryItem.get_uses()
		{
			return base.uses;
		}

		// privatescope
		internal float IInventoryItem.GetConditionPercent()
		{
			return base.GetConditionPercent();
		}

		// privatescope
		internal bool IInventoryItem.IsBroken()
		{
			return base.IsBroken();
		}

		// privatescope
		internal bool IInventoryItem.IsDamaged()
		{
			return base.IsDamaged();
		}

		// privatescope
		internal bool IInventoryItem.MarkDirty()
		{
			return base.MarkDirty();
		}

		// privatescope
		internal void IInventoryItem.Serialize(uLink.BitStream stream)
		{
			base.Serialize(stream);
		}

		// privatescope
		internal void IInventoryItem.set_lastUseTime(float value)
		{
			base.lastUseTime = value;
		}

		// privatescope
		internal void IInventoryItem.SetCondition(float condition)
		{
			base.SetCondition(condition);
		}

		// privatescope
		internal void IInventoryItem.SetMaxCondition(float condition)
		{
			base.SetMaxCondition(condition);
		}

		// privatescope
		internal void IInventoryItem.SetUses(int count)
		{
			base.SetUses(count);
		}

		// privatescope
		internal bool IInventoryItem.TryConditionLoss(float probability, float percentLoss)
		{
			return base.TryConditionLoss(probability, percentLoss);
		}
	}

	[Serializable]
	public class SlotChanceWeightedEntry : WeightSelection.WeightedEntry
	{
		public byte numSlots;

		public SlotChanceWeightedEntry()
		{
		}
	}
}