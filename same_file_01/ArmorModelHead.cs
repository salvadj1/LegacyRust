using System;

[ArmorModelSlotClass(ArmorModelSlot.Head)]
public sealed class ArmorModelHead : ArmorModel<ArmorModelHead>
{
	public ArmorModelHead() : base(ArmorModelSlot.Head)
	{
	}
}