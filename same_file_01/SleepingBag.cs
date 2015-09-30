using Facepunch;
using System;
using UnityEngine;

[NGCAutoAddScript]
public class SleepingBag : DeployedRespawn, IContextRequestable, IContextRequestableQuick, IContextRequestableStatus, IContextRequestableText, IContextRequestablePointText, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	public string giveItemName;

	public SleepingBag()
	{
	}

	public ContextExecution ContextQuery(Controllable controllable, ulong timestamp)
	{
		return ContextExecution.Quick;
	}

	public ContextResponse ContextRespondQuick(Controllable controllable, ulong timestamp)
	{
		this.PlayerUse(controllable);
		return ContextResponse.DoneBreak;
	}

	public ContextStatusFlags ContextStatusPoll()
	{
		PlayerClient playerClient = PlayerClient.localPlayerClient;
		if (playerClient && playerClient.userID == this.creatorID)
		{
			return 0;
		}
		return ContextStatusFlags.SpriteFlag1;
	}

	public string ContextText(Controllable localControllable)
	{
		PlayerClient playerClient = localControllable.playerClient;
		if (playerClient && playerClient.userID == this.creatorID)
		{
			return "Pick Up";
		}
		return string.Empty;
	}

	bool IContextRequestablePointText.ContextTextPoint(out Vector3 worldPoint)
	{
		ContextRequestable.PointUtil.SpriteOrOrigin(this, out worldPoint);
		return true;
	}

	public void PlayerUse(Controllable controllable)
	{
		if (base.BelongsTo(controllable))
		{
			if (!this.IsValidToSpawn())
			{
				return;
			}
			if (controllable.GetComponent<Inventory>().AddItemAmount(DatablockDictionary.GetByName(this.giveItemName), 1) == 1)
			{
				return;
			}
		}
	}
}