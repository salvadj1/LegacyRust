using System;

public interface IEquipmentItem : IInventoryItem
{
	void OnEquipped();

	void OnUnEquipped();
}