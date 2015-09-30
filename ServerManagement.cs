using Facepunch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using uLink;
using UnityEngine;

[RequireComponent(typeof(uLinkNetworkView))]
public class ServerManagement : Facepunch.MonoBehaviour
{
	[SerializeField]
	protected string defaultPlayerControllableKey = ":player_soldier";

	private static ServerManagement _serverMan;

	[NonSerialized]
	protected readonly List<PlayerClient> _playerClientList;

	[NonSerialized]
	[Obsolete("Use PlayerClient.All")]
	internal readonly LockedList<PlayerClient> lockedPlayerClientList;

	private static NetError? kickedNetError;

	private bool hasUnstickPosition;

	private Transform unstickTransform;

	private Vector3 nextUnstickPosition;

	protected bool blockFutureConnections;

	static ServerManagement()
	{
	}

	public ServerManagement() : this(new List<PlayerClient>())
	{
	}

	private ServerManagement(List<PlayerClient> pcList)
	{
		this.lockedPlayerClientList = new LockedList<PlayerClient>(pcList);
		this._playerClientList = pcList;
	}

	private void AddPlayerClientToList(PlayerClient pc)
	{
		this._playerClientList.Add(pc);
	}

	public virtual void AddPlayerSpawn(GameObject spawn)
	{
	}

	protected void Awake()
	{
		ServerManagement._serverMan = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		DestroysOnDisconnect.ApplyToGameObject(base.gameObject);
	}

	[RPC]
	protected void ClientFirstReady(uLink.NetworkMessageInfo info)
	{
	}

	[DebuggerHidden]
	[Obsolete("You should be using PlayerClient.FindAllWithName")]
	internal IEnumerable<PlayerClient> FindPlayerClientsByName(string name, StringComparison comparison)
	{
		ServerManagement.<FindPlayerClientsByName>c__Iterator2D variable = null;
		return variable;
	}

	[DebuggerHidden]
	[Obsolete("You should be using PlayerClient.FindAllWithString")]
	internal IEnumerable<PlayerClient> FindPlayerClientsByString(string name)
	{
		ServerManagement.<FindPlayerClientsByString>c__Iterator2C variable = null;
		return variable;
	}

	public static ServerManagement Get()
	{
		return ServerManagement._serverMan;
	}

	public static NetError GetLastKickReason(bool clear)
	{
		NetError? nullable = ServerManagement.kickedNetError;
		NetError netError = (!nullable.HasValue ? NetCull.lastError : nullable.Value);
		if (clear)
		{
			ServerManagement.kickedNetError = null;
		}
		return netError;
	}

	public static IEnumerable<uLink.NetworkPlayer> GetNetworkPlayersByName(string name)
	{
		return ServerManagement.GetNetworkPlayersByName(name, StringComparison.InvariantCultureIgnoreCase);
	}

	[DebuggerHidden]
	public static IEnumerable<uLink.NetworkPlayer> GetNetworkPlayersByName(string name, StringComparison comparison)
	{
		ServerManagement.<GetNetworkPlayersByName>c__Iterator2E variable = null;
		return variable;
	}

	[DebuggerHidden]
	public static IEnumerable<uLink.NetworkPlayer> GetNetworkPlayersByString(string partialNameOrIntID)
	{
		ServerManagement.<GetNetworkPlayersByString>c__Iterator2F variable = null;
		return variable;
	}

	public uLink.RPCMode GetNetworkPlayersInGroup(string group)
	{
		return uLink.RPCMode.Others;
	}

	public uLink.RPCMode GetNetworkPlayersInSameZone(PlayerClient client)
	{
		return uLink.RPCMode.Others;
	}

	protected static bool GetOrigin(uLink.NetworkPlayer player, bool eyes, out Vector3 origin)
	{
		PlayerClient playerClient;
		Transform transforms;
		ServerManagement serverManagement = ServerManagement.Get();
		if (serverManagement && serverManagement.GetPlayerClient(player, out playerClient))
		{
			Controllable controllable = playerClient.controllable;
			if (controllable)
			{
				Character component = controllable.GetComponent<Character>();
				if (!component)
				{
					transforms = controllable.transform;
				}
				else
				{
					transforms = (!eyes || !component.eyesTransformReadOnly ? component.transform : component.eyesTransformReadOnly);
				}
				origin = transforms.position;
				return true;
			}
		}
		origin = new Vector3();
		return false;
	}

	public bool GetPlayerClient(GameObject go, out PlayerClient playerClient)
	{
		bool flag;
		List<PlayerClient>.Enumerator enumerator = this._playerClientList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerClient current = enumerator.Current;
				if (!current.controllable || !(current.controllable.gameObject == go))
				{
					continue;
				}
				playerClient = current;
				flag = true;
				return flag;
			}
			playerClient = null;
			return false;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return flag;
	}

	public bool GetPlayerClient(uLink.NetworkPlayer player, out PlayerClient playerClient)
	{
		bool flag;
		List<PlayerClient>.Enumerator enumerator = this._playerClientList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerClient current = enumerator.Current;
				if (current.netPlayer != player)
				{
					continue;
				}
				playerClient = current;
				flag = true;
				return flag;
			}
			playerClient = null;
			return false;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return flag;
	}

	[RPC]
	protected void KP(int err)
	{
		ServerManagement.kickedNetError = new NetError?((NetError)err);
	}

	public void LocalClientPoliteReady()
	{
		base.networkView.RPC("ClientFirstReady", uLink.RPCMode.Server, new object[0]);
	}

	protected void OnDestroy()
	{
		if (ServerManagement._serverMan == this)
		{
			ServerManagement._serverMan = null;
		}
	}

	private void RemovePlayerClientFromList(PlayerClient pc)
	{
		this._playerClientList.Remove(pc);
	}

	private void RemovePlayerClientFromListByNetworkPlayer(uLink.NetworkPlayer np)
	{
		PlayerClient playerClient;
		if (!this.GetPlayerClient(np, out playerClient))
		{
			UnityEngine.Debug.Log("Error, could not find PC for NP");
		}
		else
		{
			this.RemovePlayerClientFromList(playerClient);
		}
	}

	public virtual void RemovePlayerSpawn(GameObject spawn)
	{
	}

	[RPC]
	protected void RequestRespawn(bool campRequest, uLink.NetworkMessageInfo info)
	{
	}

	[RPC]
	protected void RS(float duration)
	{
		NetCull.ResynchronizeClock((double)duration);
	}

	public virtual void TeleportPlayer(uLink.NetworkPlayer move, Vector3 worldPoint)
	{
	}

	private void UnstickInvoke()
	{
		if (this.hasUnstickPosition)
		{
			try
			{
				if (this.unstickTransform)
				{
					this.unstickTransform.position = this.nextUnstickPosition;
					Character component = this.unstickTransform.GetComponent<Character>();
					if (component)
					{
						CCMotor cCMotor = component.ccmotor;
						if (cCMotor)
						{
							cCMotor.Teleport(this.nextUnstickPosition);
						}
					}
				}
			}
			finally
			{
				this.hasUnstickPosition = false;
			}
		}
	}

	[RPC]
	protected void UnstickMove(Vector3 point)
	{
		Transform transforms;
		PlayerClient playerClient = PlayerClient.localPlayerClient;
		if (playerClient)
		{
			Controllable controllable = playerClient.controllable;
			if (controllable)
			{
				Character component = controllable.GetComponent<Character>();
				transforms = (!component ? controllable.transform : component.transform);
				if (transforms)
				{
					this.hasUnstickPosition = true;
					this.nextUnstickPosition = point;
					this.unstickTransform = transforms;
					this.UnstickInvoke();
					base.Invoke("UnstickInvoke", 0.25f);
				}
			}
		}
	}
}