using System;

public abstract class EquipmentItem<T> : InventoryItem<T>
where T : EquipmentDataBlock
{
	protected EquipmentItem(T db) : base(db)
	{
	}

	public void OnEquipped()
	{
		this.datablock.OnEquipped(this.iface as IEquipmentItem);
	}

	public void OnUnEquipped()
	{
		this.datablock.OnUnEquipped(this.iface as IEquipmentItem);
	}
}