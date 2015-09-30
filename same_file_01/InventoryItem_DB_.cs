using System;
using uLink;
using UnityEngine;

public abstract class InventoryItem<DB> : InventoryItem
where DB : ItemDataBlock
{
	public readonly DB datablock;

	protected sealed override ItemDataBlock __infrastructure_db
	{
		get
		{
			return (object)this.datablock;
		}
	}

	public bool doNotSave
	{
		get
		{
			return (!this.datablock ? false : this.datablock.doesNotSave);
		}
	}

	public override string toolTip
	{
		get
		{
			string conditionString = this.GetConditionString();
			if (string.IsNullOrEmpty(conditionString))
			{
				return this.datablock.name;
			}
			return string.Concat(conditionString, " ", this.datablock.name);
		}
	}

	protected InventoryItem(DB datablock) : base(datablock)
	{
		this.datablock = datablock;
	}

	protected override void OnBitStreamRead(uLink.BitStream stream)
	{
		InventoryItem.DeserializeSharedProperties(stream, this, this.datablock);
	}

	protected override void OnBitStreamWrite(uLink.BitStream stream)
	{
		InventoryItem.SerializeSharedProperties(stream, this, this.datablock);
	}

	public override InventoryItem.MenuItemResult OnMenuOption(InventoryItem.MenuItem option)
	{
		InventoryItem.MenuItemResult menuItemResult = this.datablock.ExecuteMenuOption(option, this.iface);
		InventoryItem.MenuItemResult menuItemResult1 = menuItemResult;
		if (menuItemResult1 == InventoryItem.MenuItemResult.Unhandled || menuItemResult1 == InventoryItem.MenuItemResult.DoneOnServer)
		{
			base.inventory.NetworkItemAction(base.slot, option);
		}
		return menuItemResult;
	}

	public override void OnMovedTo(Inventory inv, int slot)
	{
	}

	public override string ToString()
	{
		Inventory inventory = base.inventory;
		string str = (!this.datablock ? InventoryItem<DB>.tostringhelper.nullDatablockString : this.datablock.name);
		if (!inventory)
		{
			return string.Format("[{0} (unbound slot {1}) with ({2} uses)]", str, base.slot, base.uses);
		}
		return string.Format("[{0} (on {1}[{2}]) with ({3} uses)]", new object[] { str, inventory.name, base.slot, base.uses });
	}

	public override InventoryItem.MergeResult TryCombine(IInventoryItem other)
	{
		ItemDataBlock itemDataBlock = other.datablock;
		ItemDataBlock.CombineRecipe matchingRecipe = this.datablock.GetMatchingRecipe(itemDataBlock);
		if (matchingRecipe == null)
		{
			return InventoryItem.MergeResult.Failed;
		}
		int num = other.uses;
		if (num < matchingRecipe.amountToLoseOther)
		{
			return InventoryItem.MergeResult.Failed;
		}
		if (base.uses < matchingRecipe.amountToLose)
		{
			return InventoryItem.MergeResult.Failed;
		}
		Inventory inventory = other.inventory;
		int num1 = 0;
		int num2 = base.uses / matchingRecipe.amountToLose;
		num1 = Mathf.Min(num2, num / matchingRecipe.amountToLoseOther);
		int num3 = 0;
		num3 = (!matchingRecipe.resultItem.IsSplittable() ? num1 : Mathf.CeilToInt((float)num1 / (float)num3));
		if (num3 > inventory.vacantSlotCount)
		{
			return InventoryItem.MergeResult.Failed;
		}
		int num4 = num1 * matchingRecipe.amountToLoseOther;
		if (other.Consume(ref num4))
		{
			inventory.RemoveItem(other.slot);
		}
		inventory.AddItemAmount(matchingRecipe.resultItem, num1, Inventory.AmountMode.Default);
		int num5 = num1 * matchingRecipe.amountToLose;
		if (base.Consume(ref num5))
		{
			base.inventory.RemoveItem(base.slot);
		}
		return InventoryItem.MergeResult.Failed;
	}

	public override InventoryItem.MergeResult TryStack(IInventoryItem other)
	{
		int num = base.uses;
		if (num == 0)
		{
			return InventoryItem.MergeResult.Failed;
		}
		DB dB = (DB)(other.datablock as DB);
		if (dB && dB == this.datablock)
		{
			if (other.uses == this.maxUses)
			{
				return InventoryItem.MergeResult.Failed;
			}
			if (this.datablock.IsSplittable())
			{
				IInventoryItem inventoryItem = other;
				InventoryItem<DB> inventoryItem1 = this;
				int num1 = inventoryItem.AddUses(num);
				if (num1 == 0)
				{
					return InventoryItem.MergeResult.Failed;
				}
				if (inventoryItem1.Consume(ref num1))
				{
					inventoryItem1.inventory.RemoveItem(inventoryItem1.slot);
				}
				return InventoryItem.MergeResult.Merged;
			}
		}
		return InventoryItem.MergeResult.Failed;
	}

	private static class tostringhelper
	{
		public readonly static string nullDatablockString;

		static tostringhelper()
		{
			InventoryItem<DB>.tostringhelper.nullDatablockString = string.Format("NULL<{0}>", typeof(DB).FullName);
		}
	}
}