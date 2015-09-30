using System;

public interface IMagazineItem : IInventoryItem
{
	int numEmptyBulletSlots
	{
		get;
	}
}