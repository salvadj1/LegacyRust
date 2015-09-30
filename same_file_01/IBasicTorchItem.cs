using System;
using UnityEngine;

public interface IBasicTorchItem : IHeldItem, IInventoryItem
{
	bool isLit
	{
		get;
		set;
	}

	GameObject light
	{
		get;
		set;
	}

	void Extinguish();

	void Ignite();
}