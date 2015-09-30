using System;

public abstract class ArmorItem<T> : EquipmentItem<T>
where T : ArmorDataBlock
{
	protected ArmorItem(T db) : base(db)
	{
	}

	public virtual void ArmorUpdate(Inventory belongInv, int belongSlot)
	{
	}

	public override bool CanMoveToSlot(Inventory toinv, int toslot)
	{
		if (base.IsBroken() && toinv is PlayerInventory && PlayerInventory.IsEquipmentSlot(toslot))
		{
			return false;
		}
		return true;
	}

	public override void ConditionChanged(float oldCondition)
	{
	}

	public override void OnAddedTo(Inventory newInventory, int targetSlot)
	{
		base.OnAddedTo(newInventory, targetSlot);
		this.ArmorUpdate(newInventory, targetSlot);
	}

	public override void OnMovedTo(Inventory toInv, int toSlot)
	{
		base.OnMovedTo(toInv, toSlot);
		this.ArmorUpdate(toInv, toSlot);
	}
}