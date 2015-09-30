using System;

public struct ModViewModelAddArgs
{
	public readonly ViewModel vm;

	public ItemModRepresentation modRep;

	public readonly IHeldItem item;

	public readonly bool isMesh;

	public ModViewModelAddArgs(ViewModel vm, IHeldItem item, bool isMesh, ItemModRepresentation modRep)
	{
		this.vm = vm;
		this.item = item;
		this.isMesh = isMesh;
		this.modRep = modRep;
	}

	public ModViewModelAddArgs(ViewModel vm, IHeldItem item, bool isMesh) : this(vm, item, isMesh, null)
	{
	}
}