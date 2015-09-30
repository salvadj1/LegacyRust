using System;

public abstract class GunpowderItem<T> : InventoryItem<T>
where T : GunpowderDataBlock
{
	protected GunpowderItem(T db) : base(db)
	{
	}
}