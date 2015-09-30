using System;

[ArmorModelSlotClass(ArmorModelSlot.Feet)]
public sealed class ArmorModelFeet : ArmorModel<ArmorModelFeet>
{
	public ArmorModelFeet() : base(ArmorModelSlot.Feet)
	{
	}
}