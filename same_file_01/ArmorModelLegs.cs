using System;

[ArmorModelSlotClass(ArmorModelSlot.Legs)]
public sealed class ArmorModelLegs : ArmorModel<ArmorModelLegs>
{
	public ArmorModelLegs() : base(ArmorModelSlot.Legs)
	{
	}
}