using System;

public interface IHeldItem : IInventoryItem
{
	bool canActivate
	{
		get;
	}

	bool canDeactivate
	{
		get;
	}

	int freeModSlots
	{
		get;
	}

	ItemModDataBlock[] itemMods
	{
		get;
	}

	ItemRepresentation itemRepresentation
	{
		get;
		set;
	}

	ItemModFlags modFlags
	{
		get;
	}

	int totalModSlots
	{
		get;
	}

	int usedModSlots
	{
		get;
	}

	ViewModel viewModelInstance
	{
		get;
	}

	void AddMod(ItemModDataBlock mod);

	int FindMod(ItemModDataBlock mod);

	void ItemPostFrame(ref HumanController.InputSample input);

	void ItemPreFrame(ref HumanController.InputSample input);

	void OnActivate();

	void OnDeactivate();

	void PreCameraRender();

	void SetTotalModSlotCount(int count);

	void SetUsedModSlotCount(int count);
}