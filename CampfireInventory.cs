using System;

public class CampfireInventory : Inventory, IServerSaveable, FixedSizeInventory
{
	public int fixedSlotCount
	{
		get
		{
			return 8;
		}
	}

	public CampfireInventory()
	{
	}

	protected override bool CheckSlotFlags(Inventory.SlotFlags itemSlotFlags, Inventory.SlotFlags slotFlags)
	{
		return (int)(itemSlotFlags & slotFlags) != 0;
	}

	protected override void ConfigureSlots(int totalCount, ref Inventory.Slot.KindDictionary<Inventory.Slot.Range> ranges, ref Inventory.SlotFlags[] flags)
	{
		Inventory.Slot.KindDictionary<Inventory.Slot.Range> range = new Inventory.Slot.KindDictionary<Inventory.Slot.Range>();
		range[Inventory.Slot.Kind.Belt] = new Inventory.Slot.Range(0, 3);
		range[Inventory.Slot.Kind.Default] = new Inventory.Slot.Range(3, totalCount - 3);
		ranges = range;
		Inventory.SlotFlags[] slotFlagsArray = new Inventory.SlotFlags[totalCount];
		for (int i = 0; i < 3; i++)
		{
			slotFlagsArray[i] = (Inventory.SlotFlags)((int)slotFlagsArray[i] | 1024);
		}
		for (int j = 3; j < 6; j++)
		{
			slotFlagsArray[j] = (Inventory.SlotFlags)((int)slotFlagsArray[j] | 512);
		}
		slotFlagsArray[6] = (Inventory.SlotFlags)((int)slotFlagsArray[6] | 128);
		slotFlagsArray[7] = (Inventory.SlotFlags)((int)slotFlagsArray[7] | 256);
		flags = slotFlagsArray;
	}
}