using System;
using UnityEngine;

public abstract class ConsumableItem<T> : InventoryItem<T>
where T : ConsumableDataBlock
{
	protected ConsumableItem(T db) : base(db)
	{
	}

	public bool GetCookableInfo(out int consumeCount, out ItemDataBlock cookedVersion, out int cookedCount, out int cookTempMin, out int burnTemp)
	{
		int num;
		burnTemp = (T)this.datablock.burnTemp;
		cookTempMin = (T)this.datablock.cookHeatRequirement;
		cookedVersion = (T)this.datablock.cookedVersion;
		if (!(T)this.datablock.cookable || !cookedVersion)
		{
			int num1 = 0;
			num = num1;
			consumeCount = num1;
			cookedCount = num;
			return false;
		}
		int num2 = Mathf.Min(2, base.uses);
		num = num2;
		consumeCount = num2;
		cookedCount = num;
		return consumeCount > 0;
	}
}