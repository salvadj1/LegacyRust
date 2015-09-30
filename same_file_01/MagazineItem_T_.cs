using System;
using UnityEngine;

public abstract class MagazineItem<T> : InventoryItem<T>
where T : MagazineDataBlock
{
	private int? lastUsesStringCount;

	private string lastUsesString;

	public int numEmptyBulletSlots
	{
		get
		{
			return this.maxUses - base.uses;
		}
	}

	public override string toolTip
	{
		get
		{
			int num = base.uses;
			int? nullable = this.lastUsesStringCount;
			if ((nullable.GetValueOrDefault() != num ? true : !nullable.HasValue))
			{
				if (num > 0)
				{
					T t = this.datablock;
					this.lastUsesString = string.Format("{0} ({1})", t.name, this.lastUsesStringCount);
				}
				else
				{
					T t1 = this.datablock;
					this.lastUsesString = string.Concat("Empty ", t1.name);
				}
				this.lastUsesStringCount = new int?(num);
			}
			return this.lastUsesString;
		}
	}

	protected MagazineItem(T db) : base(db)
	{
	}
}