using System;

public abstract class BasicHealthKit<T> : InventoryItem<T>
where T : BasicHealthKitDataBlock
{
	protected BasicHealthKit(T db) : base(db)
	{
	}
}