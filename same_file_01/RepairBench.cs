using System;
using uLink;
using UnityEngine;

[NGCAutoAddScript]
public class RepairBench : IDLocal
{
	public RepairBench()
	{
	}

	public bool CanRepair(Inventory ingredientInv)
	{
		BlueprintDataBlock blueprintDataBlock;
		IInventoryItem repairItem = this.GetRepairItem();
		if (repairItem == null || !repairItem.datablock.isRepairable)
		{
			return false;
		}
		if (!repairItem.IsDamaged())
		{
			return false;
		}
		if (!BlueprintDataBlock.FindBlueprintForItem<BlueprintDataBlock>(repairItem.datablock, out blueprintDataBlock))
		{
			return false;
		}
		for (int i = 0; i < (int)blueprintDataBlock.ingredients.Length; i++)
		{
			BlueprintDataBlock.IngredientEntry ingredientEntry = blueprintDataBlock.ingredients[i];
			int num = Mathf.CeilToInt((float)blueprintDataBlock.ingredients[i].amount * this.GetResourceScalar());
			if (num > 0 && ingredientInv.CanConsume(blueprintDataBlock.ingredients[i].Ingredient, num) <= 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool CompleteRepair(Inventory ingredientInv)
	{
		BlueprintDataBlock blueprintDataBlock;
		if (!this.CanRepair(ingredientInv))
		{
			return false;
		}
		IInventoryItem repairItem = this.GetRepairItem();
		if (!BlueprintDataBlock.FindBlueprintForItem<BlueprintDataBlock>(repairItem.datablock, out blueprintDataBlock))
		{
			return false;
		}
		for (int i = 0; i < (int)blueprintDataBlock.ingredients.Length; i++)
		{
			BlueprintDataBlock.IngredientEntry ingredientEntry = blueprintDataBlock.ingredients[i];
			int num = Mathf.RoundToInt((float)blueprintDataBlock.ingredients[i].amount * this.GetResourceScalar());
			if (num > 0)
			{
				while (num > 0)
				{
					int num1 = 0;
					IInventoryItem inventoryItem = ingredientInv.FindItem(ingredientEntry.Ingredient, out num1);
					if (inventoryItem == null)
					{
						return false;
					}
					if (inventoryItem.Consume(ref num))
					{
						ingredientInv.RemoveItem(inventoryItem.slot);
					}
				}
			}
		}
		float single = repairItem.maxcondition - repairItem.condition;
		float single1 = single * 0.2f + 0.05f;
		repairItem.SetMaxCondition(repairItem.maxcondition - single1);
		repairItem.SetCondition(repairItem.maxcondition);
		return true;
	}

	[RPC]
	protected void DoRepair(uLink.NetworkMessageInfo info)
	{
	}

	public IInventoryItem GetRepairItem()
	{
		IInventoryItem inventoryItem;
		base.GetComponent<Inventory>().GetItem(0, out inventoryItem);
		return inventoryItem;
	}

	public float GetResourceScalar()
	{
		IInventoryItem repairItem = this.GetRepairItem();
		if (repairItem == null)
		{
			return 0f;
		}
		return (repairItem.maxcondition - repairItem.condition) * 0.5f;
	}

	public bool HasRepairItem()
	{
		return this.GetRepairItem() != null;
	}
}