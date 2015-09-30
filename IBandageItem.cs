using System;

public interface IBandageItem : IHeldItem, IInventoryItem
{
	float bandageStartTime
	{
		get;
		set;
	}

	float lastBandageTime
	{
		get;
		set;
	}

	bool lastFramePrimary
	{
		get;
		set;
	}
}