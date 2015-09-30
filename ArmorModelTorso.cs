using System;

[ArmorModelSlotClass(ArmorModelSlot.Torso)]
public sealed class ArmorModelTorso : ArmorModel<ArmorModelTorso>
{
	public ArmorModelTorso() : base(ArmorModelSlot.Torso)
	{
	}
}