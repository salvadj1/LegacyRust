using System;

public interface IMeleeWeaponItem : IHeldItem, IInventoryItem, IWeaponItem
{
	float queuedSwingAttackTime
	{
		get;
		set;
	}

	float queuedSwingSoundTime
	{
		get;
		set;
	}

	void QueueMidSwing(float time);

	void QueueSwingSound(float time);
}