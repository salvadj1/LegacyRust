using System;

public abstract class AmmoItem<T> : InventoryItem<T>
where T : AmmoItemDataBlock
{
	protected AmmoItem(T db) : base(db)
	{
	}
}