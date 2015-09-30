using System;

public struct CraftingSession
{
	[NonSerialized]
	public BlueprintDataBlock blueprint;

	[NonSerialized]
	public float startTime;

	[NonSerialized]
	public float duration;

	[NonSerialized]
	public float progressSeconds;

	[NonSerialized]
	public float _progressPerSec;

	[NonSerialized]
	public ulong startTimeMillis;

	[NonSerialized]
	public ulong durationMillis;

	[NonSerialized]
	public ulong secondsCraftingFor;

	[NonSerialized]
	public int amount;

	[NonSerialized]
	public bool inProgress;

	public double percentComplete
	{
		get
		{
			if (!this.inProgress)
			{
				return 0;
			}
			return (double)(this.progressSeconds / this.duration);
		}
	}

	public float progressPerSec
	{
		get
		{
			return this._progressPerSec;
		}
		set
		{
			this._progressPerSec = value;
		}
	}

	public float remainingSeconds
	{
		get
		{
			return (float)(this.duration - this.progressSeconds) / this.progressPerSec;
		}
	}

	public bool Restart(Inventory inventory, int amount, BlueprintDataBlock blueprint, ulong startTimeMillis)
	{
		if (!blueprint || !blueprint.CanWork(amount, inventory))
		{
			this = new CraftingSession();
			return false;
		}
		this.blueprint = blueprint;
		this.startTime = (float)((double)((float)startTimeMillis) / 1000);
		this.duration = blueprint.craftingDuration * (float)amount;
		this.progressPerSec = 1f;
		this.progressSeconds = 0f;
		this.amount = amount;
		this.inProgress = true;
		return true;
	}
}