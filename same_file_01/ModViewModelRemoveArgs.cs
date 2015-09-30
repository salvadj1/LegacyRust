using System;

public struct ModViewModelRemoveArgs
{
	public readonly ViewModel vm;

	public ItemModRepresentation modRep;

	public readonly IHeldItem item;

	public ModViewModelRemoveArgs(ViewModel vm, IHeldItem item, ItemModRepresentation modRep)
	{
		this.vm = vm;
		this.item = item;
		this.modRep = modRep;
	}

	public ModViewModelRemoveArgs(ViewModel vm, IHeldItem item) : this(vm, item, null)
	{
	}
}