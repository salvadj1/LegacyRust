using Rust;
using System;

public abstract class ResearchToolItem<T> : ToolItem<T>
where T : ToolDataBlock
{
	protected ResearchToolItem(T db) : base(db)
	{
	}

	public override InventoryItem.MergeResult TryCombine(IInventoryItem otherItem)
	{
		BlueprintDataBlock blueprintDataBlock;
		PlayerInventory playerInventory = base.inventory as PlayerInventory;
		if (!playerInventory || otherItem.inventory != playerInventory)
		{
			return InventoryItem.MergeResult.Failed;
		}
		ItemDataBlock itemDataBlock = otherItem.datablock;
		if (!itemDataBlock || !itemDataBlock.isResearchable)
		{
			Notice.Popup("", "You can't research this", 4f);
			return InventoryItem.MergeResult.Failed;
		}
		if (!playerInventory.AtWorkBench())
		{
			Notice.Popup("", "You must be at a workbench to do this.", 4f);
			return InventoryItem.MergeResult.Failed;
		}
		if (!BlueprintDataBlock.FindBlueprintForItem<BlueprintDataBlock>(otherItem.datablock, out blueprintDataBlock))
		{
			Notice.Popup("", "You can't research this.. No Blueprint Available!...", 4f);
			return InventoryItem.MergeResult.Failed;
		}
		if (!playerInventory.KnowsBP(blueprintDataBlock))
		{
			return InventoryItem.MergeResult.Combined;
		}
		Notice.Popup("", "You already know how to make this!", 4f);
		return InventoryItem.MergeResult.Failed;
	}
}