using System;

[AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=false)]
public sealed class ArmorModelSlotClassAttribute : Attribute
{
	public readonly ArmorModelSlot ArmorModelSlot;

	public ArmorModelSlotClassAttribute(ArmorModelSlot slot)
	{
		this.ArmorModelSlot = slot;
	}
}