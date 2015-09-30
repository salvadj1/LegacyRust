using System;
using uLink;

public interface IInventoryItem
{
	bool active
	{
		get;
	}

	Character character
	{
		get;
	}

	float condition
	{
		get;
	}

	Controllable controllable
	{
		get;
	}

	Controller controller
	{
		get;
	}

	ItemDataBlock datablock
	{
		get;
	}

	bool dirty
	{
		get;
	}

	bool doNotSave
	{
		get;
	}

	IDMain idMain
	{
		get;
	}

	Inventory inventory
	{
		get;
	}

	bool isInLocalInventory
	{
		get;
	}

	float lastUseTime
	{
		get;
		set;
	}

	float maxcondition
	{
		get;
	}

	int slot
	{
		get;
	}

	string toolTip
	{
		get;
	}

	int uses
	{
		get;
	}

	int AddUses(int count);

	bool Consume(ref int count);

	void Deserialize(BitStream stream);

	float GetConditionPercent();

	bool IsBroken();

	bool IsDamaged();

	bool MarkDirty();

	void OnAddedTo(Inventory inv, int slot);

	InventoryItem.MenuItemResult OnMenuOption(InventoryItem.MenuItem option);

	void OnMovedTo(Inventory inv, int slot);

	void Serialize(BitStream stream);

	void SetCondition(float condition);

	void SetMaxCondition(float condition);

	void SetUses(int count);

	InventoryItem.MergeResult TryCombine(IInventoryItem other);

	bool TryConditionLoss(float probability, float percentLoss);

	InventoryItem.MergeResult TryStack(IInventoryItem other);
}