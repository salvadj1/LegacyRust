using System;

public interface IBowWeaponItem : IHeldItem, IInventoryItem, IWeaponItem
{
	bool arrowDrawn
	{
		get;
		set;
	}

	float completeDrawTime
	{
		get;
		set;
	}

	int currentArrowID
	{
		get;
		set;
	}

	bool tired
	{
		get;
		set;
	}

	void ArrowReportHit(IDMain hitMain, ArrowMovement arrow);

	void ArrowReportMiss(ArrowMovement arrow);

	IInventoryItem FindAmmo();

	void MakeReadyIn(float delay);
}