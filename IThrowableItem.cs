using System;

public interface IThrowableItem : IHeldItem, IInventoryItem, IWeaponItem
{
	float heldThrowStrength
	{
		get;
	}

	bool holdingBack
	{
		get;
		set;
	}

	float holdingStartTime
	{
		get;
		set;
	}

	float minReleaseTime
	{
		get;
		set;
	}

	void BeginHoldingBack();

	void EndHoldingBack();
}