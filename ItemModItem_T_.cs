using System;

public abstract class ItemModItem<T> : InventoryItem<T>
where T : ItemModDataBlock
{
	protected ItemModItem(T db) : base(db)
	{
	}

	public override InventoryItem.MergeResult TryCombine(IInventoryItem otherItem)
	{
		IHeldItem heldItem = otherItem as IHeldItem;
		if (heldItem == null)
		{
			return InventoryItem.MergeResult.Failed;
		}
		if (heldItem.freeModSlots <= 0)
		{
			return InventoryItem.MergeResult.Failed;
		}
		if (!(otherItem.datablock is BulletWeaponDataBlock))
		{
			return base.TryCombine(otherItem);
		}
		if ((otherItem as IHeldItem).FindMod((T)this.datablock) != -1)
		{
			return InventoryItem.MergeResult.Failed;
		}
		return InventoryItem.MergeResult.Combined;
	}

	public override InventoryItem.MergeResult TryStack(IInventoryItem otherItem)
	{
		InventoryItem.MergeResult mergeResult = this.TryCombine(otherItem);
		if (mergeResult != InventoryItem.MergeResult.Failed)
		{
			return mergeResult;
		}
		return base.TryStack(otherItem);
	}
}