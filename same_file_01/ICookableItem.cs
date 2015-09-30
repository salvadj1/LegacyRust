using System;

public interface ICookableItem : IInventoryItem
{
	bool GetCookableInfo(out int consumeCount, out ItemDataBlock cookedVersion, out int cookedCount, out int cookTempMin, out int burnTemp);
}