using System;

public abstract class BloodDrawItem<T> : InventoryItem<T>
where T : BloodDrawDatablock
{
	protected BloodDrawItem(T db) : base(db)
	{
	}
}