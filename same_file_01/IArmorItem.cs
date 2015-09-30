using System;

public interface IArmorItem : IEquipmentItem, IInventoryItem
{
	void ArmorUpdate(Inventory belongInv, int belongSlot);
}