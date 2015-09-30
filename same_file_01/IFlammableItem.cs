using System;

public interface IFlammableItem : IInventoryItem
{
	bool flammable
	{
		get;
	}
}