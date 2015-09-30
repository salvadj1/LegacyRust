using System;

public abstract class LockpickItem<T> : InventoryItem<T>
where T : LockpickItemDataBlock
{
	protected LockpickItem(T db) : base(db)
	{
	}
}