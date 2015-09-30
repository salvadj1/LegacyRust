using Rust;
using System;
using System.Collections.Generic;
using uLink;
using UnityEngine;

[RequireComponent(typeof(uLinkNetworkView))]
public class PlayerClient : IDMain
{
	private const ulong kAutoReclockInitialDelay = 8000L;

	private const ulong kAutoReclockInterval = 420000L;

	private const ulong kAutoReclockMS_Base = 3000L;

	private const ulong kAutoReclockMS_AddMax = 500L;

	public static PlayerClient localPlayerClient;

	private Controllable _controllable;

	public uLink.NetworkPlayer netPlayer;

	private uLink.NetworkMessageInfo instantiationinfo;

	private int _playerID;

	[NonSerialized]
	private bool boundUserID;

	public ulong userID;

	public string userName;

	[NonSerialized]
	public bool firstReady;

	private int lastInputFrame = -2147483648;

	private ulong nextAutoReclockTime;

	public static LockedList<PlayerClient> All
	{
		get
		{
			ServerManagement serverManagement = ServerManagement.Get();
			if (!serverManagement)
			{
				return LockedList<PlayerClient>.Empty;
			}
			return serverManagement.lockedPlayerClientList;
		}
	}

	public Controllable controllable
	{
		get
		{
			return this._controllable;
		}
	}

	public double instantiationTimeStamp
	{
		get
		{
			return this.instantiationinfo.timestamp;
		}
	}

	public bool local
	{
		get
		{
			return (!PlayerClient.localPlayerClient ? false : PlayerClient.localPlayerClient == this);
		}
	}

	public Controllable rootControllable
	{
		get
		{
			return this._controllable;
		}
	}

	public Controllable topControllable
	{
		get
		{
			return this._controllable;
		}
	}

	public PlayerClient() : base(IDFlags.Unknown)
	{
	}

	private void Awake()
	{
		this._playerID = uLink.NetworkPlayer.unassigned.id;
	}

	protected virtual void ClientInput()
	{
	}

	public static bool Find(uLink.NetworkPlayer player, out PlayerClient pc)
	{
		int num = player.id;
		if (num != uLink.NetworkPlayer.unassigned.id && !(player == uLink.NetworkPlayer.server))
		{
			return PlayerClient.g.playerIDDict.TryGetValue(num, out pc);
		}
		pc = null;
		return false;
	}

	public static bool Find(uLink.NetworkPlayer player, out PlayerClient pc, bool throwIfNotFound)
	{
		if (!throwIfNotFound)
		{
			return PlayerClient.Find(player, out pc);
		}
		if (!PlayerClient.Find(player, out pc))
		{
			throw new ArgumentException("There was no PlayerClient for that player", "player");
		}
		return true;
	}

	public static IEnumerable<PlayerClient> FindAllWithName(string name, StringComparison comparison)
	{
		if (!string.IsNullOrEmpty(name))
		{
			ServerManagement serverManagement = ServerManagement.Get();
			ServerManagement serverManagement1 = serverManagement;
			if (serverManagement)
			{
				return serverManagement1.FindPlayerClientsByName(name, comparison);
			}
		}
		return EmptyArray<PlayerClient>.emptyEnumerable;
	}

	public static IEnumerable<PlayerClient> FindAllWithName(string name)
	{
		return PlayerClient.FindAllWithName(name, StringComparison.InvariantCultureIgnoreCase);
	}

	public static IEnumerable<PlayerClient> FindAllWithString(string partialNameOrIDInt)
	{
		ServerManagement serverManagement = ServerManagement.Get();
		if (serverManagement == null)
		{
			return EmptyArray<PlayerClient>.emptyEnumerable;
		}
		if (string.IsNullOrEmpty(partialNameOrIDInt))
		{
			return EmptyArray<PlayerClient>.emptyEnumerable;
		}
		return serverManagement.FindPlayerClientsByString(partialNameOrIDInt);
	}

	public static bool FindByUserID(ulong userID, out PlayerClient client)
	{
		if (userID == 0)
		{
			client = null;
			return false;
		}
		return PlayerClient.g.userIDDict.TryGetValue(userID, out client);
	}

	public static PlayerClient GetLocalPlayer()
	{
		return PlayerClient.localPlayerClient;
	}

	public static void InputFunction(GameObject req)
	{
		if (req && PlayerClient.localPlayerClient && PlayerClient.localPlayerClient._controllable && PlayerClient.localPlayerClient._controllable.gameObject == req && PlayerClient.localPlayerClient.lastInputFrame != Time.frameCount)
		{
			PlayerClient.localPlayerClient.lastInputFrame = Time.frameCount;
			PlayerClient.localPlayerClient.ClientInput();
		}
	}

	protected new void OnDestroy()
	{
		try
		{
			int num = uLink.NetworkPlayer.unassigned.id;
			if (this._playerID != num)
			{
				try
				{
					try
					{
						if (object.ReferenceEquals(PlayerClient.g.playerIDDict[this._playerID], this))
						{
							PlayerClient.g.playerIDDict.Remove(this._playerID);
						}
					}
					catch (Exception exception)
					{
						Debug.LogException(exception, this);
					}
				}
				finally
				{
					this._playerID = num;
				}
			}
			if (this.boundUserID)
			{
				try
				{
					try
					{
						if (object.ReferenceEquals(PlayerClient.g.userIDDict[this.userID], this))
						{
							PlayerClient.g.userIDDict.Remove(this.userID);
						}
					}
					catch (Exception exception1)
					{
						Debug.LogException(exception1, this);
					}
				}
				finally
				{
					this.boundUserID = false;
				}
			}
			if (PlayerClient.localPlayerClient == this)
			{
				PlayerClient.localPlayerClient = null;
			}
		}
		finally
		{
			base.OnDestroy();
		}
	}

	private void OnDisable()
	{
		if (this.local && !base.destroying && !NetInstance.IsCurrentlyDestroying(this))
		{
			Debug.LogWarning("The local player got disabled", this);
		}
	}

	private void OnEnable()
	{
		if (!this.local)
		{
			Debug.LogWarning("Something tried to enable a non local player.. setting enabled to false", this);
			base.enabled = false;
		}
	}

	internal void OnRootControllableEntered(Controllable controllable)
	{
		if (this._controllable)
		{
			Debug.LogWarning("There was a controllable for player client already", this);
		}
		this._controllable = controllable;
	}

	internal void OnRootControllableExited(Controllable controllable)
	{
		if (this._controllable == controllable)
		{
			this._controllable = null;
		}
		else
		{
			Debug.LogWarning("The controllable exited did not match that of the existing value", this);
		}
	}

	public void ProcessLocalPlayerPreRender()
	{
		if (this._controllable)
		{
			this._controllable.masterControllable.ProcessLocalPlayerPreRender();
		}
	}

	private void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		this.netPlayer = info.networkView.owner;
		uLink.BitStream bitStream = info.networkView.initialData;
		this.userID = bitStream.ReadUInt64();
		this.userName = bitStream.ReadString();
		base.name = string.Concat(new string[] { "Player ", this.userName, " (", this.userID.ToString(), ")" });
		this.instantiationinfo = info;
		this._playerID = this.netPlayer.id;
		PlayerClient.g.playerIDDict[this._playerID] = this;
		PlayerClient.g.userIDDict[this.userID] = this;
		this.boundUserID = true;
		if (PlayerClient.localPlayerClient || !base.networkView.isMine)
		{
			base.enabled = false;
		}
		else
		{
			PlayerClient.localPlayerClient = this;
			base.enabled = true;
			this.nextAutoReclockTime = NetCull.localTimeInMillis + (long)8000;
		}
	}

	private void Update()
	{
		if (this.lastInputFrame != Time.frameCount && (!this._controllable || !this._controllable.masterControllable.forwardsPlayerClientInput))
		{
			this.lastInputFrame = Time.frameCount;
			this.ClientInput();
		}
		if (NetCull.isClientRunning && !Globals.isLoading)
		{
			ulong num = NetCull.localTimeInMillis;
			if (num >= this.nextAutoReclockTime)
			{
				try
				{
					ulong num1 = Math.Min(num - this.nextAutoReclockTime, (ulong)500);
					NetCull.ResynchronizeClock((double)((float)((long)3000 + num1)) / 1000);
					num = num + num1;
				}
				finally
				{
					this.nextAutoReclockTime = num + (long)420000;
				}
			}
		}
	}

	private static class g
	{
		public static Dictionary<int, PlayerClient> playerIDDict;

		public static Dictionary<ulong, PlayerClient> userIDDict;

		static g()
		{
			PlayerClient.g.playerIDDict = new Dictionary<int, PlayerClient>();
			PlayerClient.g.userIDDict = new Dictionary<ulong, PlayerClient>();
		}
	}
}