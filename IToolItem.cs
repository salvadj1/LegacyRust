using System;

public interface IToolItem : IInventoryItem
{
	bool canWork
	{
		get;
	}

	float workDuration
	{
		get;
	}

	void CancelWork();

	void CompleteWork();

	void StartWork();
}