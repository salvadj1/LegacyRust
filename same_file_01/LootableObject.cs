using Facepunch;
using System;
using uLink;
using UnityEngine;

[NGCAutoAddScript]
[RequireComponent(typeof(Inventory))]
public class LootableObject : IDLocal, IUseable, IContextRequestable, IContextRequestableQuick, IContextRequestableText, IContextRequestablePointText, IComponentInterface<IUseable, Facepunch.MonoBehaviour, Useable>, IComponentInterface<IUseable, Facepunch.MonoBehaviour>, IComponentInterface<IUseable>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	private const string kAnimation_OpenIdle = "opened idle";

	private const string kAnimation_Open = "open";

	private const string kAnimation_CloseIdle = "closed idle";

	private const string kAnimation_Close = "close";

	[SerializeField]
	private LootSpawnListReference _lootSpawnListName;

	public float LootCycle;

	public float lifeTime;

	[PrefetchComponent]
	public Inventory _inventory;

	private Useable _useable;

	protected uLink.NetworkPlayer _currentlyUsingPlayer;

	public bool destroyOnEmpty;

	public int NumberOfSlots = 12;

	public bool lateSized;

	[NonSerialized]
	public bool accessLocked;

	public RPOSLootWindow lootWindowOverride;

	private bool thisClientIsInWindow;

	protected string occupierText;

	private bool sentSetLooter;

	private uLink.NetworkPlayer sentLooter;

	public LootSpawnList _spawnList
	{
		get
		{
			return this._lootSpawnListName.list;
		}
		set
		{
			this._lootSpawnListName.list = value;
		}
	}

	public LootableObject()
	{
	}

	public void CancelInvokes()
	{
		if (this.LootCycle > 0f)
		{
			base.CancelInvoke("TryAddLoot");
		}
		if (this.lifeTime > 0f)
		{
			base.CancelInvoke("DelayedDestroy");
		}
	}

	[RPC]
	protected void ClearLooter()
	{
		this.occupierText = null;
		this._currentlyUsingPlayer = uLink.NetworkPlayer.unassigned;
		if (this.thisClientIsInWindow)
		{
			try
			{
				try
				{
					RPOS.CloseLootWindow();
				}
				catch (Exception exception)
				{
					Debug.LogError(exception);
				}
			}
			finally
			{
				this.thisClientIsInWindow = false;
			}
		}
	}

	public void ClientClosedLootWindow()
	{
		try
		{
			if (this.IsLocalLooting())
			{
				NetCull.RPC(this, "StopLooting", uLink.RPCMode.Server);
			}
		}
		finally
		{
			if (this.thisClientIsInWindow)
			{
				this.thisClientIsInWindow = false;
			}
		}
	}

	protected virtual ContextResponse ContextRespond_OpenLoot(Controllable controllable, ulong timestamp)
	{
		return ContextRequestable.UseableForwardFromContextRespond(this, controllable, this._useable);
	}

	public virtual string ContextText(Controllable localControllable)
	{
		PlayerClient playerClient;
		if (this._currentlyUsingPlayer == uLink.NetworkPlayer.unassigned)
		{
			return "Search";
		}
		if (this.occupierText == null)
		{
			if (PlayerClient.Find(this._currentlyUsingPlayer, out playerClient))
			{
				this.occupierText = string.Format("Occupied by {0}", playerClient.userName);
			}
			else
			{
				this.occupierText = "Occupied";
			}
		}
		return this.occupierText;
	}

	public virtual bool ContextTextPoint(out Vector3 worldPoint)
	{
		if (!ContextRequestable.PointUtil.SpriteOrOrigin(base.transform, out worldPoint))
		{
			return true;
		}
		worldPoint.y = worldPoint.y + 0.15f;
		return true;
	}

	public bool IsLocalLooting()
	{
		bool flag;
		if (this.thisClientIsInWindow)
		{
			flag = true;
		}
		else
		{
			flag = (this._currentlyUsingPlayer != NetCull.player ? false : this._currentlyUsingPlayer != uLink.NetworkPlayer.unassigned);
		}
		return flag;
	}

	protected void OnDestroy()
	{
		UseableUtility.OnDestroy(this, this._useable);
	}

	public void OnUseEnter(Useable use)
	{
	}

	public void OnUseExit(Useable use, UseExitReason reason)
	{
	}

	public void RadialCheck()
	{
		if (this._useable.user && Vector3.Distance(this._useable.user.transform.position, base.transform.position) > 5f)
		{
			this._useable.Eject();
			base.CancelInvoke("RadialCheck");
		}
	}

	[RPC]
	protected void SetLooter(uLink.NetworkPlayer ply)
	{
		this.occupierText = null;
		if (ply != uLink.NetworkPlayer.unassigned)
		{
			if (ply == NetCull.player)
			{
				if (!this.thisClientIsInWindow)
				{
					try
					{
						this._currentlyUsingPlayer = ply;
						RPOS.OpenLootWindow(this);
						this.thisClientIsInWindow = true;
					}
					catch (Exception exception)
					{
						Debug.LogError(exception, this);
						NetCull.RPC(this, "StopLooting", uLink.RPCMode.Server);
						this.thisClientIsInWindow = false;
						ply = uLink.NetworkPlayer.unassigned;
					}
				}
			}
			else if (this._currentlyUsingPlayer == NetCull.player && NetCull.player != uLink.NetworkPlayer.unassigned)
			{
				this.ClearLooter();
			}
			this._currentlyUsingPlayer = ply;
		}
		else
		{
			this.ClearLooter();
		}
	}

	[RPC]
	protected void StopLooting(uLink.NetworkMessageInfo info)
	{
		if (this._currentlyUsingPlayer == info.sender)
		{
			this._useable.Eject();
		}
	}

	[RPC]
	protected void TakeAll()
	{
	}
}