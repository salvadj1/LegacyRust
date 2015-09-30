using System;
using UnityEngine;

public abstract class ResourceTypeItem<T> : InventoryItem<T>
where T : ResourceTypeItemDataBlock
{
	protected float _lastUseTime;

	public bool flammable
	{
		get
		{
			return (T)this.datablock.flammable;
		}
	}

	protected ResourceTypeItem(T db) : base(db)
	{
	}

	public bool GetCookableInfo(out int consumeCount, out ItemDataBlock cookedVersion, out int cookedCount, out int cookTempMin, out int burnTemp)
	{
		burnTemp = 999999999;
		cookTempMin = (T)this.datablock.cookHeatRequirement;
		cookedVersion = (T)this.datablock.cookedVersion;
		if (!(T)this.datablock.cookable || !cookedVersion)
		{
			int num = 0;
			int num1 = num;
			consumeCount = num;
			cookedCount = num1;
			return false;
		}
		consumeCount = Mathf.Min(2, base.uses);
		cookedCount = consumeCount * UnityEngine.Random.Range((T)this.datablock.numToGiveCookedMin, (T)this.datablock.numToGiveCookedMax + 1);
		if (cookedCount != 0)
		{
			return true;
		}
		consumeCount = 0;
		return false;
	}
}