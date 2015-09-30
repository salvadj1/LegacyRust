using System;
using uLink;
using UnityEngine;

public class CraftingInventory : Inventory
{
	private const string CancelCraftingRPC = "CRFX";

	private const string CraftNetworkUpdateRPC = "CRFU";

	private const string CraftNetworkClearRPC = "CRFC";

	private const string StartCraftingRPC = "CRFS";

	public float _lastWorkBenchTime;

	protected bool _wasAtWorkbench;

	private double _lastThinkTime;

	private CraftingSession crafting;

	public new float? craftingCompletePercent
	{
		get
		{
			if (!this.crafting.inProgress)
			{
				return null;
			}
			return new float?((float)this.crafting.percentComplete);
		}
	}

	public new float? craftingSecondsRemaining
	{
		get
		{
			if (!this.crafting.inProgress)
			{
				return null;
			}
			return new float?((float)this.crafting.remainingSeconds);
		}
	}

	public float craftingSpeedPerSec
	{
		get
		{
			return this.crafting.progressPerSec;
		}
	}

	public new bool isCrafting
	{
		get
		{
			return this.crafting.inProgress;
		}
	}

	public new bool isCraftingInventory
	{
		get
		{
			return true;
		}
	}

	public CraftingInventory()
	{
	}

	public bool AtWorkBench()
	{
		return this._lastWorkBenchTime < 0f;
	}

	public bool CancelCrafting()
	{
		if (!this.crafting.inProgress)
		{
			return false;
		}
		base.networkView.RPC("CRFX", uLink.RPCMode.Server, new object[0]);
		this.crafting.inProgress = false;
		return true;
	}

	public void CraftThink()
	{
		if (this.crafting.inProgress)
		{
			double num = NetCull.time;
			float single = (float)(num - this._lastThinkTime);
			this.crafting.progressSeconds = Mathf.Clamp(this.crafting.progressSeconds + this.crafting.progressPerSec * single, 0f, this.crafting.duration);
			this._lastThinkTime = num;
		}
	}

	[NGCRPCSkip]
	[RPC]
	protected void CRFC()
	{
		this.UpdateCrafting(null, 0, 0f, 0f, 0f, 0f);
	}

	[NGCRPCSkip]
	[RPC]
	protected void CRFS(int amount, int blueprintUID, uLink.NetworkMessageInfo info)
	{
	}

	[NGCRPCSkip]
	[RPC]
	protected void CRFU(float start, float dur, float progresspersec, float progress, int blueprintUniqueID, int amount)
	{
		this.UpdateCrafting(CraftingInventory.FindBlueprint(blueprintUniqueID), amount, start, dur, progress, progresspersec);
	}

	[NGCRPCSkip]
	[RPC]
	protected void CRFX()
	{
	}

	private static BlueprintDataBlock FindBlueprint(int uniqueID)
	{
		if (uniqueID == 0)
		{
			return null;
		}
		return (BlueprintDataBlock)DatablockDictionary.GetByUniqueID(uniqueID);
	}

	public bool StartCrafting(BlueprintDataBlock blueprint, int amount)
	{
		if (!blueprint.CanWork(amount, this))
		{
			return false;
		}
		base.networkView.RPC("CRFS", uLink.RPCMode.Server, new object[] { amount, blueprint.uniqueID });
		return true;
	}

	protected void UpdateCrafting(BlueprintDataBlock blueprint, int amount, float start, float dur, float progress, float progresspersec)
	{
		Debug.Log(string.Format("Craft network update :{0}:", (!blueprint ? "NONE" : blueprint.name)), this);
		this._lastThinkTime = NetCull.time;
		this.crafting.blueprint = blueprint;
		this.crafting.inProgress = blueprint;
		this.crafting.startTime = start;
		this.crafting.duration = dur;
		this.crafting.progressSeconds = progress;
		this.crafting.progressPerSec = progresspersec;
		this.crafting.amount = amount;
		this.Refresh();
	}

	public bool ValidateCraftRequirements(BlueprintDataBlock bp)
	{
		if (!bp.RequireWorkbench)
		{
			return true;
		}
		if (this.AtWorkBench())
		{
			return true;
		}
		return false;
	}

	[RPC]
	public void wbi(bool at, uLink.NetworkMessageInfo info)
	{
		if (!at)
		{
			this._lastWorkBenchTime = Single.PositiveInfinity;
		}
		else
		{
			this._lastWorkBenchTime = Single.NegativeInfinity;
		}
	}
}