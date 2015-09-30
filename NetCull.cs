using Facepunch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public static class NetCull
{
	public const bool canDestroy = false;

	public const bool canRemoveRPCs = false;

	private const bool ensureCanDestroy = false;

	private const bool ensureCanRemoveRPCS = false;

	public const bool kServer = false;

	public const bool kClient = true;

	private readonly static NetworkInstantiator.Destroyer destroyerFreeViewIDOnly;

	public static uLink.BitStream approvalData
	{
		get
		{
			return uLink.Network.approvalData;
		}
	}

	public static NetworkConfig config
	{
		get
		{
			return uLink.Network.config;
		}
	}

	public static uLink.NetworkPlayer[] connections
	{
		get
		{
			return uLink.Network.connections;
		}
	}

	[Obsolete("Use #if CLIENT (unless your trying to check if the client is connected.. then use NetCull.isClientRunning")]
	public static bool isClient
	{
		get
		{
			return NetCull.isClientRunning;
		}
	}

	public static bool isClientRunning
	{
		get
		{
			return uLink.Network.isClient;
		}
	}

	public static bool isMessageQueueRunning
	{
		get
		{
			return uLink.Network.isMessageQueueRunning;
		}
		set
		{
			uLink.Network.isMessageQueueRunning = value;
		}
	}

	public static bool isNotRunning
	{
		get
		{
			return (uLink.Network.isClient ? false : !uLink.Network.isServer);
		}
	}

	public static bool isRunning
	{
		get
		{
			return (uLink.Network.isClient ? true : uLink.Network.isServer);
		}
	}

	[Obsolete("Use #if SERVER (unless your trying to check if the server is running.. then use NetCull.isServerRunning")]
	public static bool isServer
	{
		get
		{
			return NetCull.isServerRunning;
		}
	}

	public static bool isServerRunning
	{
		get
		{
			return uLink.Network.isServer;
		}
	}

	public static NetError lastError
	{
		get
		{
			return uLink.Network.lastError.ToNetError();
		}
		set
		{
			uLink.Network.lastError = value._uLink();
		}
	}

	public static int listenPort
	{
		get
		{
			return uLink.Network.listenPort;
		}
	}

	public static double localTime
	{
		get
		{
			return uLink.Network.localTime;
		}
	}

	public static ulong localTimeInMillis
	{
		get
		{
			return uLink.Network.localTimeInMillis;
		}
	}

	public static uLink.NetworkPlayer player
	{
		get
		{
			return uLink.Network.player;
		}
	}

	public static double sendInterval
	{
		get
		{
			return NetCull.Send.Interval;
		}
	}

	public static float sendIntervalF
	{
		get
		{
			return NetCull.Send.IntervalF;
		}
	}

	public static float sendRate
	{
		get
		{
			return uLink.Network.sendRate;
		}
		set
		{
			uLink.Network.sendRate = value;
			NetCull.Send.Rate = uLink.Network.sendRate;
			NetCull.Send.Interval = 1 / (double)NetCull.Send.Rate;
			NetCull.Send.IntervalF = (float)NetCull.Send.Interval;
			Interpolation.sendRate = NetCull.Send.Rate;
		}
	}

	public static NetworkStatus status
	{
		get
		{
			return uLink.Network.status;
		}
	}

	public static double time
	{
		get
		{
			return uLink.Network.time;
		}
	}

	public static ulong timeInMillis
	{
		get
		{
			return uLink.Network.timeInMillis;
		}
	}

	static NetCull()
	{
		NetCull.destroyerFreeViewIDOnly = new NetworkInstantiator.Destroyer(NetCull.FreeViewIDOnly_Destroyer);
	}

	public static void CloseConnection(uLink.NetworkPlayer target, bool sendDisconnectionNotification)
	{
		uLink.Network.CloseConnection(target, sendDisconnectionNotification, 3);
	}

	public static NetError Connect(string host, int remotePort, string password, params object[] loginData)
	{
		return uLink.Network.Connect(host, remotePort, password, loginData).ToNetError();
	}

	public static void Disconnect(int timeout)
	{
		uLink.Network.Disconnect(timeout);
	}

	public static void Disconnect()
	{
		uLink.Network.Disconnect();
	}

	public static void DisconnectImmediate()
	{
		uLink.Network.DisconnectImmediate();
	}

	public static void DontDestroyWithNetwork(uLink.NetworkView view)
	{
		if (view)
		{
			view.instantiator.destroyer = NetCull.destroyerFreeViewIDOnly;
		}
	}

	public static void DontDestroyWithNetwork(GameObject go)
	{
		if (go)
		{
			NetCull.DontDestroyWithNetwork(go.GetComponent<uLinkNetworkView>());
		}
	}

	public static void DontDestroyWithNetwork(Facepunch.MonoBehaviour behaviour)
	{
		if (behaviour)
		{
			NetCull.DontDestroyWithNetwork(behaviour.networkView);
		}
	}

	public static void DontDestroyWithNetwork(Component component)
	{
		if (component)
		{
			NetCull.DontDestroyWithNetwork(component.GetComponent<uLinkNetworkView>());
		}
	}

	public static bool Found(this NetCull.PrefabSearch search)
	{
		return (int)search != 0;
	}

	private static void FreeViewIDOnly_Destroyer(uLink.NetworkView instance)
	{
	}

	public static bool IsNet(this NetCull.PrefabSearch search)
	{
		return (int)search > 1;
	}

	public static bool IsNetAutoPrefab(this NetCull.PrefabSearch search)
	{
		return (int)search == 3;
	}

	public static bool IsNetMainPrefab(this NetCull.PrefabSearch search)
	{
		return (int)search == 2;
	}

	public static bool IsNGC(this NetCull.PrefabSearch search)
	{
		return (int)search == 1;
	}

	public static NetCull.PrefabSearch LoadPrefab(string prefabName, out GameObject prefab)
	{
		NGC.Prefab prefab1;
		uLinkNetworkView _uLinkNetworkView;
		NetCull.PrefabSearch prefabSearch;
		if (string.IsNullOrEmpty(prefabName))
		{
			prefab = null;
			return NetCull.PrefabSearch.Missing;
		}
		if (prefabName.StartsWith(":"))
		{
			try
			{
				prefab = NetMainPrefab.Lookup<GameObject>(prefabName);
				prefabSearch = (!prefab ? NetCull.PrefabSearch.Missing : NetCull.PrefabSearch.NetMain);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				prefab = null;
				prefabSearch = NetCull.PrefabSearch.Missing;
			}
		}
		else if (!prefabName.StartsWith(";"))
		{
			try
			{
				if (NetCull.AutoPrefabs.all.TryGetValue(prefabName, out _uLinkNetworkView) && _uLinkNetworkView)
				{
					GameObject gameObject = _uLinkNetworkView.gameObject;
					GameObject gameObject1 = gameObject;
					prefab = gameObject;
					if (gameObject1)
					{
						prefabSearch = NetCull.PrefabSearch.NetAuto;
						return prefabSearch;
					}
				}
				prefab = null;
				prefabSearch = NetCull.PrefabSearch.Missing;
			}
			catch (Exception exception1)
			{
				UnityEngine.Debug.LogException(exception1);
				prefab = null;
				prefabSearch = NetCull.PrefabSearch.Missing;
			}
		}
		else
		{
			try
			{
				if (NGC.Prefab.Register.Find(prefabName, out prefab1))
				{
					NGCView nGCView = prefab1.prefab;
					if (!nGCView)
					{
						prefab = null;
						prefabSearch = NetCull.PrefabSearch.Missing;
					}
					else
					{
						prefab = nGCView.gameObject;
						prefabSearch = (!prefab ? NetCull.PrefabSearch.Missing : NetCull.PrefabSearch.NGC);
					}
				}
				else
				{
					prefab = null;
					prefabSearch = NetCull.PrefabSearch.Missing;
				}
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogException(exception2);
				prefab = null;
				prefabSearch = NetCull.PrefabSearch.Missing;
			}
		}
		return prefabSearch;
	}

	public static GameObject LoadPrefab(string prefabName)
	{
		GameObject gameObject;
		if ((int)NetCull.LoadPrefab(prefabName, out gameObject) == 0)
		{
			throw new MissingReferenceException(prefabName);
		}
		return gameObject;
	}

	public static NetCull.PrefabSearch LoadPrefabComponent<T>(string prefabName, out T component)
	where T : Component
	{
		UnityEngine.MonoBehaviour monoBehaviour;
		NetCull.PrefabSearch prefabSearch = NetCull.LoadPrefabView(prefabName, out monoBehaviour);
		if ((int)prefabSearch == 0)
		{
			component = (T)null;
		}
		else if (!typeof(UnityEngine.MonoBehaviour).IsAssignableFrom(typeof(T)) || !(monoBehaviour is T))
		{
			component = monoBehaviour.GetComponent<T>();
			if (!component)
			{
				prefabSearch = NetCull.PrefabSearch.Missing;
			}
		}
		else
		{
			component = (T)monoBehaviour;
		}
		return prefabSearch;
	}

	public static T LoadPrefabComponent<T>(string prefabName)
	where T : Component
	{
		T t;
		if ((int)NetCull.LoadPrefabComponent<T>(prefabName, out t) == 0)
		{
			throw new MissingReferenceException(prefabName);
		}
		return t;
	}

	public static NetCull.PrefabSearch LoadPrefabScript<T>(string prefabName, out T script)
	where T : UnityEngine.MonoBehaviour
	{
		UnityEngine.MonoBehaviour monoBehaviour;
		NetCull.PrefabSearch prefabSearch = NetCull.LoadPrefabView(prefabName, out monoBehaviour);
		if ((int)prefabSearch == 0)
		{
			script = (T)null;
		}
		else if (!(monoBehaviour is T))
		{
			script = monoBehaviour.GetComponent<T>();
			if (!script)
			{
				prefabSearch = NetCull.PrefabSearch.Missing;
			}
		}
		else
		{
			script = (T)monoBehaviour;
		}
		return prefabSearch;
	}

	public static T LoadPrefabScript<T>(string prefabName)
	where T : UnityEngine.MonoBehaviour
	{
		T t;
		if ((int)NetCull.LoadPrefabScript<T>(prefabName, out t) == 0)
		{
			throw new MissingReferenceException(prefabName);
		}
		return t;
	}

	public static NetCull.PrefabSearch LoadPrefabView(string prefabName, out UnityEngine.MonoBehaviour prefabView)
	{
		NGC.Prefab prefab;
		uLinkNetworkView _uLinkNetworkView;
		NetCull.PrefabSearch prefabSearch;
		if (string.IsNullOrEmpty(prefabName))
		{
			prefabView = null;
			return NetCull.PrefabSearch.Missing;
		}
		if (prefabName.StartsWith(":"))
		{
			try
			{
				prefabView = NetMainPrefab.Lookup<uLinkNetworkView>(prefabName);
				prefabSearch = (!prefabView ? NetCull.PrefabSearch.Missing : NetCull.PrefabSearch.NetMain);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				prefabView = null;
				prefabSearch = NetCull.PrefabSearch.Missing;
			}
		}
		else if (!prefabName.StartsWith(";"))
		{
			try
			{
				if (!NetCull.AutoPrefabs.all.TryGetValue(prefabName, out _uLinkNetworkView) || !_uLinkNetworkView)
				{
					prefabView = _uLinkNetworkView;
					prefabSearch = NetCull.PrefabSearch.Missing;
				}
				else
				{
					prefabView = null;
					prefabSearch = NetCull.PrefabSearch.NetAuto;
				}
			}
			catch (Exception exception1)
			{
				UnityEngine.Debug.LogException(exception1);
				prefabView = null;
				prefabSearch = NetCull.PrefabSearch.Missing;
			}
		}
		else
		{
			try
			{
				if (NGC.Prefab.Register.Find(prefabName, out prefab))
				{
					NGCView nGCView = prefab.prefab;
					UnityEngine.MonoBehaviour monoBehaviour = nGCView;
					prefabView = nGCView;
					if (monoBehaviour)
					{
						prefabSearch = NetCull.PrefabSearch.NGC;
						return prefabSearch;
					}
				}
				prefabView = null;
				prefabSearch = NetCull.PrefabSearch.Missing;
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogException(exception2);
				prefabView = null;
				prefabSearch = NetCull.PrefabSearch.Missing;
			}
		}
		return prefabSearch;
	}

	public static UnityEngine.MonoBehaviour LoadPrefabView(string prefabName)
	{
		UnityEngine.MonoBehaviour monoBehaviour;
		if ((int)NetCull.LoadPrefabView(prefabName, out monoBehaviour) == 0)
		{
			throw new MissingReferenceException(prefabName);
		}
		return monoBehaviour;
	}

	public static bool Missing(this NetCull.PrefabSearch search)
	{
		return (int)search == 0;
	}

	private static void OnPostUpdatePostCallbacks()
	{
		Interpolator.SyncronizeAll();
		CharacterInterpolatorBase.SyncronizeAll();
	}

	private static void OnPostUpdatePreCallbacks()
	{
	}

	private static void OnPreUpdatePostCallbacks()
	{
	}

	private static void OnPreUpdatePreCallbacks()
	{
	}

	public static void RegisterNetAutoPrefab(uLinkNetworkView viewPrefab)
	{
		if (viewPrefab)
		{
			string str = viewPrefab.name;
			try
			{
				NetCull.AutoPrefabs.all[str] = viewPrefab;
			}
			catch
			{
				UnityEngine.Debug.LogError(string.Concat("skipped duplicate prefab named ", str), viewPrefab);
				return;
			}
			NetworkInstantiator.AddPrefab(viewPrefab.gameObject);
		}
	}

	public static void ResynchronizeClock(double durationInSeconds)
	{
		uLink.Network.ResynchronizeClock(durationInSeconds);
	}

	[Obsolete("void NetCull.ResynchronizeClock(ulong) is deprecated, Bla bla bla don't use this", true)]
	public static void ResynchronizeClock(ulong intervalMillis)
	{
		uLink.Network.ResynchronizeClock(intervalMillis);
	}

	public static void RPC(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode)
	{
		view.RPC(flags, messageName, rpcMode, new object[0]);
	}

	public static void RPC(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target)
	{
		view.RPC(flags, messageName, target, new object[0]);
	}

	public static void RPC(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		view.RPC(flags, messageName, targets, new object[0]);
	}

	public static void RPC(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode)
	{
		view.RPC(messageName, rpcMode, new object[0]);
	}

	public static void RPC(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target)
	{
		view.RPC(messageName, target, new object[0]);
	}

	public static void RPC(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		view.RPC(messageName, targets, new object[0]);
	}

	public static void RPC<P0>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		view.RPC<P0>(flags, messageName, rpcMode, p0);
	}

	public static void RPC<P0>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		view.RPC<P0>(flags, messageName, target, p0);
	}

	public static void RPC<P0>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		view.RPC<P0>(flags, messageName, targets, p0);
	}

	public static void RPC<P0>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		view.RPC<P0>(messageName, rpcMode, p0);
	}

	public static void RPC<P0>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		view.RPC<P0>(messageName, target, p0);
	}

	public static void RPC<P0>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		view.RPC<P0>(messageName, targets, p0);
	}

	public static void RPC<P0, P1>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		view.RPC(flags, messageName, rpcMode, new object[] { p0, p1 });
	}

	public static void RPC<P0, P1>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		view.RPC(flags, messageName, target, new object[] { p0, p1 });
	}

	public static void RPC<P0, P1>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		view.RPC(flags, messageName, targets, new object[] { p0, p1 });
	}

	public static void RPC<P0, P1>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		view.RPC(messageName, rpcMode, new object[] { p0, p1 });
	}

	public static void RPC<P0, P1>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		view.RPC(messageName, target, new object[] { p0, p1 });
	}

	public static void RPC<P0, P1>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		view.RPC(messageName, targets, new object[] { p0, p1 });
	}

	public static void RPC<P0, P1, P2>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		view.RPC(flags, messageName, rpcMode, new object[] { p0, p1, p2 });
	}

	public static void RPC<P0, P1, P2>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		view.RPC(flags, messageName, target, new object[] { p0, p1, p2 });
	}

	public static void RPC<P0, P1, P2>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		view.RPC(flags, messageName, targets, new object[] { p0, p1, p2 });
	}

	public static void RPC<P0, P1, P2>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		view.RPC(messageName, rpcMode, new object[] { p0, p1, p2 });
	}

	public static void RPC<P0, P1, P2>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		view.RPC(messageName, target, new object[] { p0, p1, p2 });
	}

	public static void RPC<P0, P1, P2>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		view.RPC(messageName, targets, new object[] { p0, p1, p2 });
	}

	public static void RPC<P0, P1, P2, P3>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC(flags, messageName, rpcMode, new object[] { p0, p1, p2, p3 });
	}

	public static void RPC<P0, P1, P2, P3>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC(flags, messageName, target, new object[] { p0, p1, p2, p3 });
	}

	public static void RPC<P0, P1, P2, P3>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC(flags, messageName, targets, new object[] { p0, p1, p2, p3 });
	}

	public static void RPC<P0, P1, P2, P3>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC(messageName, rpcMode, new object[] { p0, p1, p2, p3 });
	}

	public static void RPC<P0, P1, P2, P3>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC(messageName, target, new object[] { p0, p1, p2, p3 });
	}

	public static void RPC<P0, P1, P2, P3>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC(messageName, targets, new object[] { p0, p1, p2, p3 });
	}

	public static void RPC<P0, P1, P2, P3, P4>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC(flags, messageName, rpcMode, new object[] { p0, p1, p2, p3, p4 });
	}

	public static void RPC<P0, P1, P2, P3, P4>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC(flags, messageName, target, new object[] { p0, p1, p2, p3, p4 });
	}

	public static void RPC<P0, P1, P2, P3, P4>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC(flags, messageName, targets, new object[] { p0, p1, p2, p3, p4 });
	}

	public static void RPC<P0, P1, P2, P3, P4>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC(messageName, rpcMode, new object[] { p0, p1, p2, p3, p4 });
	}

	public static void RPC<P0, P1, P2, P3, P4>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC(messageName, target, new object[] { p0, p1, p2, p3, p4 });
	}

	public static void RPC<P0, P1, P2, P3, P4>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC(messageName, targets, new object[] { p0, p1, p2, p3, p4 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC(flags, messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC(flags, messageName, target, new object[] { p0, p1, p2, p3, p4, p5 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC(flags, messageName, targets, new object[] { p0, p1, p2, p3, p4, p5 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC(messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC(messageName, target, new object[] { p0, p1, p2, p3, p4, p5 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC(messageName, targets, new object[] { p0, p1, p2, p3, p4, p5 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC(flags, messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC(flags, messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC(flags, messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC(messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC(messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC(messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC(flags, messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6, p7 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC(flags, messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6, p7 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC(flags, messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6, p7 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC(messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6, p7 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC(messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6, p7 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC(messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6, p7 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC(flags, messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC(flags, messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC(flags, messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC(messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC(messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC(messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC(flags, messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC(flags, messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC(flags, messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC(messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC(messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC(messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC(flags, messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC(flags, messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC(flags, messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC(messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC(messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC(messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC(flags, messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC(flags, messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Facepunch.NetworkView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC(flags, messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Facepunch.NetworkView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC(messageName, rpcMode, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Facepunch.NetworkView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC(messageName, target, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 });
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Facepunch.NetworkView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC(messageName, targets, new object[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 });
	}

	public static void RPC(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode)
	{
		view.RPC(flags, messageName, rpcMode);
	}

	public static void RPC(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target)
	{
		view.RPC(flags, messageName, target);
	}

	public static void RPC(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		view.RPC(flags, messageName, targets);
	}

	public static void RPC(NGCView view, string messageName, uLink.RPCMode rpcMode)
	{
		view.RPC(messageName, rpcMode);
	}

	public static void RPC(NGCView view, string messageName, uLink.NetworkPlayer target)
	{
		view.RPC(messageName, target);
	}

	public static void RPC(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		view.RPC(messageName, targets);
	}

	public static void RPC<P0>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		view.RPC<P0>(flags, messageName, rpcMode, p0);
	}

	public static void RPC<P0>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		view.RPC<P0>(flags, messageName, target, p0);
	}

	public static void RPC<P0>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		view.RPC<P0>(flags, messageName, targets, p0);
	}

	public static void RPC<P0>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		view.RPC<P0>(messageName, rpcMode, p0);
	}

	public static void RPC<P0>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		view.RPC<P0>(messageName, target, p0);
	}

	public static void RPC<P0>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		view.RPC<P0>(messageName, targets, p0);
	}

	public static void RPC<P0, P1>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		view.RPC<P0, P1>(flags, messageName, rpcMode, p0, p1);
	}

	public static void RPC<P0, P1>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		view.RPC<P0, P1>(flags, messageName, target, p0, p1);
	}

	public static void RPC<P0, P1>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		view.RPC<P0, P1>(flags, messageName, targets, p0, p1);
	}

	public static void RPC<P0, P1>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		view.RPC<P0, P1>(messageName, rpcMode, p0, p1);
	}

	public static void RPC<P0, P1>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		view.RPC<P0, P1>(messageName, target, p0, p1);
	}

	public static void RPC<P0, P1>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		view.RPC<P0, P1>(messageName, targets, p0, p1);
	}

	public static void RPC<P0, P1, P2>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		view.RPC<P0, P1, P2>(flags, messageName, rpcMode, p0, p1, p2);
	}

	public static void RPC<P0, P1, P2>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		view.RPC<P0, P1, P2>(flags, messageName, target, p0, p1, p2);
	}

	public static void RPC<P0, P1, P2>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		view.RPC<P0, P1, P2>(flags, messageName, targets, p0, p1, p2);
	}

	public static void RPC<P0, P1, P2>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		view.RPC<P0, P1, P2>(messageName, rpcMode, p0, p1, p2);
	}

	public static void RPC<P0, P1, P2>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		view.RPC<P0, P1, P2>(messageName, target, p0, p1, p2);
	}

	public static void RPC<P0, P1, P2>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		view.RPC<P0, P1, P2>(messageName, targets, p0, p1, p2);
	}

	public static void RPC<P0, P1, P2, P3>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC<P0, P1, P2, P3>(flags, messageName, rpcMode, p0, p1, p2, p3);
	}

	public static void RPC<P0, P1, P2, P3>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC<P0, P1, P2, P3>(flags, messageName, target, p0, p1, p2, p3);
	}

	public static void RPC<P0, P1, P2, P3>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC<P0, P1, P2, P3>(flags, messageName, targets, p0, p1, p2, p3);
	}

	public static void RPC<P0, P1, P2, P3>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC<P0, P1, P2, P3>(messageName, rpcMode, p0, p1, p2, p3);
	}

	public static void RPC<P0, P1, P2, P3>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC<P0, P1, P2, P3>(messageName, target, p0, p1, p2, p3);
	}

	public static void RPC<P0, P1, P2, P3>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		view.RPC<P0, P1, P2, P3>(messageName, targets, p0, p1, p2, p3);
	}

	public static void RPC<P0, P1, P2, P3, P4>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC<P0, P1, P2, P3, P4>(flags, messageName, rpcMode, p0, p1, p2, p3, p4);
	}

	public static void RPC<P0, P1, P2, P3, P4>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC<P0, P1, P2, P3, P4>(flags, messageName, target, p0, p1, p2, p3, p4);
	}

	public static void RPC<P0, P1, P2, P3, P4>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC<P0, P1, P2, P3, P4>(flags, messageName, targets, p0, p1, p2, p3, p4);
	}

	public static void RPC<P0, P1, P2, P3, P4>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC<P0, P1, P2, P3, P4>(messageName, rpcMode, p0, p1, p2, p3, p4);
	}

	public static void RPC<P0, P1, P2, P3, P4>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC<P0, P1, P2, P3, P4>(messageName, target, p0, p1, p2, p3, p4);
	}

	public static void RPC<P0, P1, P2, P3, P4>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		view.RPC<P0, P1, P2, P3, P4>(messageName, targets, p0, p1, p2, p3, p4);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC<P0, P1, P2, P3, P4, P5>(flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC<P0, P1, P2, P3, P4, P5>(flags, messageName, target, p0, p1, p2, p3, p4, p5);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC<P0, P1, P2, P3, P4, P5>(flags, messageName, targets, p0, p1, p2, p3, p4, p5);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC<P0, P1, P2, P3, P4, P5>(messageName, rpcMode, p0, p1, p2, p3, p4, p5);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC<P0, P1, P2, P3, P4, P5>(messageName, target, p0, p1, p2, p3, p4, p5);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		view.RPC<P0, P1, P2, P3, P4, P5>(messageName, targets, p0, p1, p2, p3, p4, p5);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6>(flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6>(flags, messageName, target, p0, p1, p2, p3, p4, p5, p6);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6>(flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6>(messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6>(messageName, target, p0, p1, p2, p3, p4, p5, p6);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6>(messageName, targets, p0, p1, p2, p3, p4, p5, p6);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NGCView view, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NGCView view, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NGCView view, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NGCView view, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NGCView view, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static void RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NGCView view, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		view.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (networkView)
		{
			NetCull.RPC(networkView, flags, messageName, rpcMode);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
		return false;
	}

	public static bool RPC(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (networkView)
		{
			NetCull.RPC(networkView, flags, messageName, target);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
		return false;
	}

	public static bool RPC(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (networkView)
		{
			NetCull.RPC(networkView, flags, messageName, targets);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
		return false;
	}

	public static bool RPC(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (networkView)
		{
			NetCull.RPC(networkView, messageName, rpcMode);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
		return false;
	}

	public static bool RPC(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (networkView)
		{
			NetCull.RPC(networkView, messageName, target);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
		return false;
	}

	public static bool RPC(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (networkView)
		{
			NetCull.RPC(networkView, messageName, targets);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
		return false;
	}

	public static bool RPC<P0>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0>(networkView, flags, messageName, rpcMode, p0);
		return true;
	}

	public static bool RPC<P0>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0>(networkView, flags, messageName, target, p0);
		return true;
	}

	public static bool RPC<P0>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0>(networkView, flags, messageName, targets, p0);
		return true;
	}

	public static bool RPC<P0>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (networkView)
		{
			NetCull.RPC<P0>(networkView, messageName, rpcMode, p0);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
		return false;
	}

	public static bool RPC<P0>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (networkView)
		{
			NetCull.RPC<P0>(networkView, messageName, target, p0);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
		return false;
	}

	public static bool RPC<P0>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (networkView)
		{
			NetCull.RPC<P0>(networkView, messageName, targets, p0);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
		return false;
	}

	public static bool RPC<P0, P1>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(networkView, flags, messageName, rpcMode, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(networkView, flags, messageName, target, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(networkView, flags, messageName, targets, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(networkView, messageName, rpcMode, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(networkView, messageName, target, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(networkView, messageName, targets, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1, P2>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(networkView, flags, messageName, rpcMode, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(networkView, flags, messageName, target, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(networkView, flags, messageName, targets, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(networkView, messageName, rpcMode, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(networkView, messageName, target, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(networkView, messageName, targets, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(networkView, flags, messageName, rpcMode, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(networkView, flags, messageName, target, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(networkView, flags, messageName, targets, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(networkView, messageName, rpcMode, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(networkView, messageName, target, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(networkView, messageName, targets, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(networkView, flags, messageName, rpcMode, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(networkView, flags, messageName, target, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(networkView, flags, messageName, targets, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(networkView, messageName, rpcMode, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(networkView, messageName, target, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(networkView, messageName, targets, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(networkView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(networkView, flags, messageName, target, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(networkView, flags, messageName, targets, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(networkView, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(networkView, messageName, target, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(networkView, messageName, targets, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(networkView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(networkView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(networkView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(networkView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(networkView, messageName, target, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(networkView, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(networkView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(networkView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(networkView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(networkView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(networkView, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(networkView, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(networkView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(networkView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(networkView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(networkView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(networkView, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(networkView, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(networkView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(networkView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(networkView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(networkView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(networkView, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(networkView, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(networkView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(networkView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(networkView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(networkView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(networkView, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(networkView, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(networkView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(networkView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(uLink.NetworkViewID viewID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(networkView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(uLink.NetworkViewID viewID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(networkView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(uLink.NetworkViewID viewID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(networkView, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(uLink.NetworkViewID viewID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(viewID);
		if (!networkView)
		{
			UnityEngine.Debug.LogError(string.Format("No Network View with id {0} to send RPC \"{1}\"", viewID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(networkView, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC((uLink.NetworkViewID)entID, flags, messageName, rpcMode);
		}
		NGCView nGCView = entID.ngcView;
		if (nGCView)
		{
			NetCull.RPC(nGCView, flags, messageName, rpcMode);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
		return false;
	}

	public static bool RPC(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC((uLink.NetworkViewID)entID, flags, messageName, target);
		}
		NGCView nGCView = entID.ngcView;
		if (nGCView)
		{
			NetCull.RPC(nGCView, flags, messageName, target);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
		return false;
	}

	public static bool RPC(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC((uLink.NetworkViewID)entID, flags, messageName, targets);
		}
		NGCView nGCView = entID.ngcView;
		if (nGCView)
		{
			NetCull.RPC(nGCView, flags, messageName, targets);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
		return false;
	}

	public static bool RPC(NetEntityID entID, string messageName, uLink.RPCMode rpcMode)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC((uLink.NetworkViewID)entID, messageName, rpcMode);
		}
		NGCView nGCView = entID.ngcView;
		if (nGCView)
		{
			NetCull.RPC(nGCView, messageName, rpcMode);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
		return false;
	}

	public static bool RPC(NetEntityID entID, string messageName, uLink.NetworkPlayer target)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC((uLink.NetworkViewID)entID, messageName, target);
		}
		NGCView nGCView = entID.ngcView;
		if (nGCView)
		{
			NetCull.RPC(nGCView, messageName, target);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
		return false;
	}

	public static bool RPC(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC((uLink.NetworkViewID)entID, messageName, targets);
		}
		NGCView nGCView = entID.ngcView;
		if (nGCView)
		{
			NetCull.RPC(nGCView, messageName, targets);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
		return false;
	}

	public static bool RPC<P0>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0>(nGCView, flags, messageName, rpcMode, p0);
		return true;
	}

	public static bool RPC<P0>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0>((uLink.NetworkViewID)entID, flags, messageName, target, p0);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0>(nGCView, flags, messageName, target, p0);
		return true;
	}

	public static bool RPC<P0>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0>((uLink.NetworkViewID)entID, flags, messageName, targets, p0);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0>(nGCView, flags, messageName, targets, p0);
		return true;
	}

	public static bool RPC<P0>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0>((uLink.NetworkViewID)entID, messageName, rpcMode, p0);
		}
		NGCView nGCView = entID.ngcView;
		if (nGCView)
		{
			NetCull.RPC<P0>(nGCView, messageName, rpcMode, p0);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
		return false;
	}

	public static bool RPC<P0>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0>((uLink.NetworkViewID)entID, messageName, target, p0);
		}
		NGCView nGCView = entID.ngcView;
		if (nGCView)
		{
			NetCull.RPC<P0>(nGCView, messageName, target, p0);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
		return false;
	}

	public static bool RPC<P0>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0>((uLink.NetworkViewID)entID, messageName, targets, p0);
		}
		NGCView nGCView = entID.ngcView;
		if (nGCView)
		{
			NetCull.RPC<P0>(nGCView, messageName, targets, p0);
			return true;
		}
		UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
		return false;
	}

	public static bool RPC<P0, P1>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0, p1);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(nGCView, flags, messageName, rpcMode, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1>((uLink.NetworkViewID)entID, flags, messageName, target, p0, p1);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(nGCView, flags, messageName, target, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1>((uLink.NetworkViewID)entID, flags, messageName, targets, p0, p1);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(nGCView, flags, messageName, targets, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1>((uLink.NetworkViewID)entID, messageName, rpcMode, p0, p1);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(nGCView, messageName, rpcMode, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1>((uLink.NetworkViewID)entID, messageName, target, p0, p1);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(nGCView, messageName, target, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1>((uLink.NetworkViewID)entID, messageName, targets, p0, p1);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1>(nGCView, messageName, targets, p0, p1);
		return true;
	}

	public static bool RPC<P0, P1, P2>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0, p1, p2);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(nGCView, flags, messageName, rpcMode, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2>((uLink.NetworkViewID)entID, flags, messageName, target, p0, p1, p2);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(nGCView, flags, messageName, target, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2>((uLink.NetworkViewID)entID, flags, messageName, targets, p0, p1, p2);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(nGCView, flags, messageName, targets, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2>((uLink.NetworkViewID)entID, messageName, rpcMode, p0, p1, p2);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(nGCView, messageName, rpcMode, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2>((uLink.NetworkViewID)entID, messageName, target, p0, p1, p2);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(nGCView, messageName, target, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2>((uLink.NetworkViewID)entID, messageName, targets, p0, p1, p2);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2>(nGCView, messageName, targets, p0, p1, p2);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0, p1, p2, p3);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(nGCView, flags, messageName, rpcMode, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3>((uLink.NetworkViewID)entID, flags, messageName, target, p0, p1, p2, p3);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(nGCView, flags, messageName, target, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3>((uLink.NetworkViewID)entID, flags, messageName, targets, p0, p1, p2, p3);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(nGCView, flags, messageName, targets, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3>((uLink.NetworkViewID)entID, messageName, rpcMode, p0, p1, p2, p3);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(nGCView, messageName, rpcMode, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3>((uLink.NetworkViewID)entID, messageName, target, p0, p1, p2, p3);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(nGCView, messageName, target, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3>((uLink.NetworkViewID)entID, messageName, targets, p0, p1, p2, p3);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3>(nGCView, messageName, targets, p0, p1, p2, p3);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0, p1, p2, p3, p4);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(nGCView, flags, messageName, rpcMode, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4>((uLink.NetworkViewID)entID, flags, messageName, target, p0, p1, p2, p3, p4);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(nGCView, flags, messageName, target, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4>((uLink.NetworkViewID)entID, flags, messageName, targets, p0, p1, p2, p3, p4);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(nGCView, flags, messageName, targets, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4>((uLink.NetworkViewID)entID, messageName, rpcMode, p0, p1, p2, p3, p4);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(nGCView, messageName, rpcMode, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4>((uLink.NetworkViewID)entID, messageName, target, p0, p1, p2, p3, p4);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(nGCView, messageName, target, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4>((uLink.NetworkViewID)entID, messageName, targets, p0, p1, p2, p3, p4);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4>(nGCView, messageName, targets, p0, p1, p2, p3, p4);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(nGCView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5>((uLink.NetworkViewID)entID, flags, messageName, target, p0, p1, p2, p3, p4, p5);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(nGCView, flags, messageName, target, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5>((uLink.NetworkViewID)entID, flags, messageName, targets, p0, p1, p2, p3, p4, p5);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(nGCView, flags, messageName, targets, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5>((uLink.NetworkViewID)entID, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(nGCView, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5>((uLink.NetworkViewID)entID, messageName, target, p0, p1, p2, p3, p4, p5);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(nGCView, messageName, target, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5>((uLink.NetworkViewID)entID, messageName, targets, p0, p1, p2, p3, p4, p5);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5>(nGCView, messageName, targets, p0, p1, p2, p3, p4, p5);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(nGCView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>((uLink.NetworkViewID)entID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(nGCView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>((uLink.NetworkViewID)entID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(nGCView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>((uLink.NetworkViewID)entID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(nGCView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>((uLink.NetworkViewID)entID, messageName, target, p0, p1, p2, p3, p4, p5, p6);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(nGCView, messageName, target, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>((uLink.NetworkViewID)entID, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(nGCView, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(nGCView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>((uLink.NetworkViewID)entID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(nGCView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>((uLink.NetworkViewID)entID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(nGCView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>((uLink.NetworkViewID)entID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(nGCView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>((uLink.NetworkViewID)entID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(nGCView, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>((uLink.NetworkViewID)entID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(nGCView, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(nGCView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>((uLink.NetworkViewID)entID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(nGCView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>((uLink.NetworkViewID)entID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(nGCView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>((uLink.NetworkViewID)entID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(nGCView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>((uLink.NetworkViewID)entID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(nGCView, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>((uLink.NetworkViewID)entID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(nGCView, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(nGCView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>((uLink.NetworkViewID)entID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(nGCView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>((uLink.NetworkViewID)entID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(nGCView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>((uLink.NetworkViewID)entID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(nGCView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>((uLink.NetworkViewID)entID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(nGCView, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>((uLink.NetworkViewID)entID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(nGCView, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(nGCView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>((uLink.NetworkViewID)entID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(nGCView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>((uLink.NetworkViewID)entID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(nGCView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>((uLink.NetworkViewID)entID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(nGCView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>((uLink.NetworkViewID)entID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(nGCView, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>((uLink.NetworkViewID)entID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(nGCView, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>((uLink.NetworkViewID)entID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(nGCView, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NetEntityID entID, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>((uLink.NetworkViewID)entID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(nGCView, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NetEntityID entID, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>((uLink.NetworkViewID)entID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(nGCView, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NetEntityID entID, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>((uLink.NetworkViewID)entID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(nGCView, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NetEntityID entID, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>((uLink.NetworkViewID)entID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(nGCView, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(NetEntityID entID, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		if (!entID.isNGC)
		{
			return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>((uLink.NetworkViewID)entID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		}
		NGCView nGCView = entID.ngcView;
		if (!nGCView)
		{
			UnityEngine.Debug.LogError(string.Format("No NGC View with id {0} to send RPC \"{1}\"", entID, messageName));
			return false;
		}
		NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(nGCView, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		return true;
	}

	public static bool RPC(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, flags, messageName, rpcMode);
	}

	public static bool RPC(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, flags, messageName, target);
	}

	public static bool RPC(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, flags, messageName, targets);
	}

	public static bool RPC(GameObject entity, string messageName, uLink.RPCMode rpcMode)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, messageName, rpcMode);
	}

	public static bool RPC(GameObject entity, string messageName, uLink.NetworkPlayer target)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, messageName, target);
	}

	public static bool RPC(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, messageName, targets);
	}

	public static bool RPC(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, flags, messageName, rpcMode);
	}

	public static bool RPC(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, flags, messageName, target);
	}

	public static bool RPC(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, flags, messageName, targets);
	}

	public static bool RPC(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, messageName, rpcMode);
	}

	public static bool RPC(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, messageName, target);
	}

	public static bool RPC(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, messageName, targets);
	}

	public static bool RPC(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, flags, messageName, rpcMode);
	}

	public static bool RPC(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, flags, messageName, target);
	}

	public static bool RPC(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, flags, messageName, targets);
	}

	public static bool RPC(Component entityComponent, string messageName, uLink.RPCMode rpcMode)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, messageName, rpcMode);
	}

	public static bool RPC(Component entityComponent, string messageName, uLink.NetworkPlayer target)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, messageName, target);
	}

	public static bool RPC(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC(netEntityID, messageName, targets);
	}

	public static bool RPC<P0>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, flags, messageName, rpcMode, p0);
	}

	public static bool RPC<P0>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, flags, messageName, target, p0);
	}

	public static bool RPC<P0>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, flags, messageName, targets, p0);
	}

	public static bool RPC<P0>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, messageName, rpcMode, p0);
	}

	public static bool RPC<P0>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, messageName, target, p0);
	}

	public static bool RPC<P0>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, messageName, targets, p0);
	}

	public static bool RPC<P0>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, flags, messageName, rpcMode, p0);
	}

	public static bool RPC<P0>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, flags, messageName, target, p0);
	}

	public static bool RPC<P0>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, flags, messageName, targets, p0);
	}

	public static bool RPC<P0>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, messageName, rpcMode, p0);
	}

	public static bool RPC<P0>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, messageName, target, p0);
	}

	public static bool RPC<P0>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, messageName, targets, p0);
	}

	public static bool RPC<P0>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, flags, messageName, rpcMode, p0);
	}

	public static bool RPC<P0>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, flags, messageName, target, p0);
	}

	public static bool RPC<P0>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, flags, messageName, targets, p0);
	}

	public static bool RPC<P0>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, messageName, rpcMode, p0);
	}

	public static bool RPC<P0>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, messageName, target, p0);
	}

	public static bool RPC<P0>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0>(netEntityID, messageName, targets, p0);
	}

	public static bool RPC<P0, P1>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, flags, messageName, rpcMode, p0, p1);
	}

	public static bool RPC<P0, P1>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, flags, messageName, target, p0, p1);
	}

	public static bool RPC<P0, P1>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, flags, messageName, targets, p0, p1);
	}

	public static bool RPC<P0, P1>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, messageName, rpcMode, p0, p1);
	}

	public static bool RPC<P0, P1>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, messageName, target, p0, p1);
	}

	public static bool RPC<P0, P1>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, messageName, targets, p0, p1);
	}

	public static bool RPC<P0, P1>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, flags, messageName, rpcMode, p0, p1);
	}

	public static bool RPC<P0, P1>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, flags, messageName, target, p0, p1);
	}

	public static bool RPC<P0, P1>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, flags, messageName, targets, p0, p1);
	}

	public static bool RPC<P0, P1>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, messageName, rpcMode, p0, p1);
	}

	public static bool RPC<P0, P1>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, messageName, target, p0, p1);
	}

	public static bool RPC<P0, P1>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, messageName, targets, p0, p1);
	}

	public static bool RPC<P0, P1>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, flags, messageName, rpcMode, p0, p1);
	}

	public static bool RPC<P0, P1>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, flags, messageName, target, p0, p1);
	}

	public static bool RPC<P0, P1>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, flags, messageName, targets, p0, p1);
	}

	public static bool RPC<P0, P1>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, messageName, rpcMode, p0, p1);
	}

	public static bool RPC<P0, P1>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, messageName, target, p0, p1);
	}

	public static bool RPC<P0, P1>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1>(netEntityID, messageName, targets, p0, p1);
	}

	public static bool RPC<P0, P1, P2>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, flags, messageName, rpcMode, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, flags, messageName, target, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, flags, messageName, targets, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, messageName, rpcMode, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, messageName, target, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, messageName, targets, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, flags, messageName, rpcMode, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, flags, messageName, target, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, flags, messageName, targets, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, messageName, rpcMode, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, messageName, target, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, messageName, targets, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, flags, messageName, rpcMode, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, flags, messageName, target, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, flags, messageName, targets, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, messageName, rpcMode, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, messageName, target, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2>(netEntityID, messageName, targets, p0, p1, p2);
	}

	public static bool RPC<P0, P1, P2, P3>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, flags, messageName, target, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, flags, messageName, targets, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, messageName, rpcMode, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, messageName, target, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, messageName, targets, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, flags, messageName, target, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, flags, messageName, targets, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, messageName, rpcMode, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, messageName, target, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, messageName, targets, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, flags, messageName, target, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, flags, messageName, targets, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, messageName, rpcMode, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, messageName, target, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3>(netEntityID, messageName, targets, p0, p1, p2, p3);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, messageName, target, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, messageName, targets, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, messageName, target, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, messageName, targets, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, messageName, target, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4>(netEntityID, messageName, targets, p0, p1, p2, p3, p4);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(GameObject entity, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(GameObject entity, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(GameObject entity, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(GameObject entity, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(GameObject entity, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(GameObject entity, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(UnityEngine.MonoBehaviour entityScript, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(UnityEngine.MonoBehaviour entityScript, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(UnityEngine.MonoBehaviour entityScript, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Component entityComponent, NetworkFlags flags, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, flags, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Component entityComponent, NetworkFlags flags, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, flags, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Component entityComponent, NetworkFlags flags, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, flags, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Component entityComponent, string messageName, uLink.RPCMode rpcMode, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, messageName, rpcMode, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Component entityComponent, string messageName, uLink.NetworkPlayer target, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, messageName, target, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	public static bool RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(Component entityComponent, string messageName, IEnumerable<uLink.NetworkPlayer> targets, P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) == 0)
		{
			return false;
		}
		return NetCull.RPC<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(netEntityID, messageName, targets, p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
	}

	[Conditional("SERVER")]
	public static void VerifyRPC(ref uLink.NetworkMessageInfo info, bool skipOwnerCheck = false)
	{
	}

	private static class AutoPrefabs
	{
		public static Dictionary<string, uLinkNetworkView> all;

		static AutoPrefabs()
		{
			NetCull.AutoPrefabs.all = new Dictionary<string, uLinkNetworkView>();
		}
	}

	public static class Callbacks
	{
		private static bool MADE_PRE;

		private static NetPreUpdate netPreUpdate;

		private static bool MADE_POST;

		private static NetPostUpdate netPostUpdate;

		private static InternalHelper internalHelper;

		internal static void BindUpdater(NetPreUpdate netUpdate)
		{
			NetCull.Callbacks.Replace<NetPreUpdate>(ref NetCull.Callbacks.netPreUpdate, netUpdate);
		}

		internal static void BindUpdater(NetPostUpdate netUpdate)
		{
			NetCull.Callbacks.Replace<NetPostUpdate>(ref NetCull.Callbacks.netPostUpdate, netUpdate);
		}

		internal static void FirePostUpdate(NetPostUpdate postUpdate)
		{
			if (postUpdate != NetCull.Callbacks.netPostUpdate || !NetCull.Callbacks.Updating())
			{
				return;
			}
			NetCull.OnPostUpdatePreCallbacks();
			if (NetCull.Callbacks.MADE_POST)
			{
				try
				{
					NetCull.Callbacks.POST.DELEGATE.Invoke();
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception, postUpdate);
				}
			}
			NetCull.OnPostUpdatePostCallbacks();
		}

		internal static void FirePreUpdate(NetPreUpdate preUpdate)
		{
			if (preUpdate != NetCull.Callbacks.netPreUpdate || !NetCull.Callbacks.Updating())
			{
				return;
			}
			NetCull.OnPreUpdatePreCallbacks();
			if (NetCull.Callbacks.MADE_PRE)
			{
				try
				{
					NetCull.Callbacks.PRE.DELEGATE.Invoke();
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception, preUpdate);
				}
			}
			NetCull.OnPreUpdatePostCallbacks();
		}

		private static void Replace<T>(ref T current, T replacement)
		where T : UnityEngine.MonoBehaviour
		{
			if (current == replacement)
			{
				return;
			}
			if (current)
			{
				UnityEngine.Debug.LogWarning(string.Concat((!replacement ? "Destroying " : "Replacing "), typeof(T)), current.gameObject);
				T t = current;
				NetCull.Callbacks.Resign<T>(ref current, current);
				if (t)
				{
					UnityEngine.Object.Destroy(t);
				}
				if (replacement)
				{
					UnityEngine.Debug.LogWarning(string.Concat("With ", typeof(T)), replacement);
				}
			}
			current = replacement;
		}

		private static void Resign<T>(ref T current, T resigning)
		where T : UnityEngine.MonoBehaviour
		{
			if (current == resigning)
			{
				current = (T)null;
			}
		}

		internal static void ResignUpdater(NetPreUpdate netUpdate)
		{
			NetCull.Callbacks.Resign<NetPreUpdate>(ref NetCull.Callbacks.netPreUpdate, netUpdate);
		}

		internal static void ResignUpdater(NetPostUpdate netUpdate)
		{
			NetCull.Callbacks.Resign<NetPostUpdate>(ref NetCull.Callbacks.netPostUpdate, netUpdate);
		}

		private static bool Updating()
		{
			if (!NetCull.Callbacks.internalHelper)
			{
				GameObject gameObject = GameObject.Find("uLinkInternalHelper");
				if (!gameObject)
				{
					return false;
				}
				NetCull.Callbacks.internalHelper = gameObject.GetComponent<InternalHelper>();
				if (!NetCull.Callbacks.internalHelper)
				{
					return false;
				}
			}
			return NetCull.Callbacks.internalHelper.enabled;
		}

		public static event NetCull.UpdateFunctor afterEveryUpdate
		{
			add
			{
				NetCull.Callbacks.POST.DELEGATE.Add(value, false);
			}
			remove
			{
				if (NetCull.Callbacks.MADE_POST)
				{
					NetCull.Callbacks.POST.DELEGATE.Remove(value);
				}
			}
		}

		public static event NetCull.UpdateFunctor afterNextUpdate
		{
			add
			{
				NetCull.Callbacks.POST.DELEGATE.Add(value, true);
			}
			remove
			{
				if (NetCull.Callbacks.MADE_POST)
				{
					NetCull.Callbacks.POST.DELEGATE.Remove(value);
				}
			}
		}

		public static event NetCull.UpdateFunctor beforeEveryUpdate
		{
			add
			{
				NetCull.Callbacks.PRE.DELEGATE.Add(value, false);
			}
			remove
			{
				if (NetCull.Callbacks.MADE_PRE)
				{
					NetCull.Callbacks.PRE.DELEGATE.Remove(value);
				}
			}
		}

		public static event NetCull.UpdateFunctor beforeNextUpdate
		{
			add
			{
				NetCull.Callbacks.PRE.DELEGATE.Add(value, true);
			}
			remove
			{
				if (NetCull.Callbacks.MADE_PRE)
				{
					NetCull.Callbacks.PRE.DELEGATE.Remove(value);
				}
			}
		}

		private static class POST
		{
			public readonly static NetCull.Callbacks.UpdateDelegate DELEGATE;

			static POST()
			{
				NetCull.Callbacks.POST.DELEGATE = new NetCull.Callbacks.UpdateDelegate();
				NetCull.Callbacks.MADE_POST = true;
			}
		}

		private static class PRE
		{
			public readonly static NetCull.Callbacks.UpdateDelegate DELEGATE;

			static PRE()
			{
				NetCull.Callbacks.PRE.DELEGATE = new NetCull.Callbacks.UpdateDelegate();
				NetCull.Callbacks.MADE_PRE = true;
			}
		}

		private class UpdateDelegate
		{
			private readonly HashSet<NetCull.UpdateFunctor> hashSet;

			private readonly List<NetCull.UpdateFunctor> list;

			private readonly List<NetCull.UpdateFunctor> invokation;

			private readonly HashSet<NetCull.UpdateFunctor> once1;

			private readonly HashSet<NetCull.UpdateFunctor> once2;

			private readonly HashSet<int> skip;

			private int count;

			private int iterPosition;

			private bool guarded;

			private bool onceSwap;

			public UpdateDelegate()
			{
			}

			public bool Add(NetCull.UpdateFunctor functor, bool oneTimeOnly)
			{
				if (!this.hashSet.Add(functor))
				{
					return false;
				}
				this.list.Add(functor);
				if (oneTimeOnly)
				{
					((!this.onceSwap ? this.once1 : this.once2)).Add(functor);
				}
				return true;
			}

			private bool HandleRemoval(NetCull.UpdateFunctor functor)
			{
				if (this.guarded)
				{
					int num = this.invokation.IndexOf(functor);
					if (num != -1)
					{
						this.invokation[num] = null;
						if (this.iterPosition < num)
						{
							this.skip.Add(num);
							return true;
						}
					}
				}
				return false;
			}

			public void Invoke()
			{
				UnityEngine.Object target;
				if (!this.guarded)
				{
					int count = this.list.Count;
					int num = count;
					this.count = count;
					if (num != 0)
					{
						this.iterPosition = -1;
						try
						{
							this.guarded = true;
							this.iterPosition = -1;
							this.invokation.AddRange(this.list);
							HashSet<NetCull.UpdateFunctor> updateFunctors = (!this.onceSwap ? this.once1 : this.once2);
							HashSet<NetCull.UpdateFunctor> updateFunctors1 = (!this.onceSwap ? this.once2 : this.once1);
							updateFunctors1.Clear();
							updateFunctors1.UnionWith(updateFunctors);
							this.onceSwap = !this.onceSwap;
							foreach (NetCull.UpdateFunctor updateFunctor in updateFunctors)
							{
								if (!this.hashSet.Remove(updateFunctor))
								{
									continue;
								}
								this.list.Remove(updateFunctor);
							}
							updateFunctors.Clear();
							while (true)
							{
								NetCull.Callbacks.UpdateDelegate updateDelegate = this;
								int num1 = updateDelegate.iterPosition + 1;
								num = num1;
								updateDelegate.iterPosition = num1;
								if (num >= this.count)
								{
									break;
								}
								if (!this.skip.Remove(this.iterPosition))
								{
									NetCull.UpdateFunctor item = this.invokation[this.iterPosition];
									try
									{
										item();
									}
									catch (Exception exception1)
									{
										Exception exception = exception1;
										try
										{
											target = item.Target as UnityEngine.Object;
										}
										catch
										{
											target = null;
										}
										UnityEngine.Debug.LogException(exception, target);
									}
								}
							}
						}
						finally
						{
							try
							{
								this.invokation.Clear();
							}
							finally
							{
								this.guarded = false;
							}
						}
						return;
					}
				}
			}

			public bool Remove(NetCull.UpdateFunctor functor)
			{
				if (!this.hashSet.Remove(functor))
				{
					if (!((!this.onceSwap ? this.once1 : this.once2)).Remove(functor))
					{
						return false;
					}
					return this.HandleRemoval(functor);
				}
				this.list.Remove(functor);
				((!this.onceSwap ? this.once1 : this.once2)).Remove(functor);
				this.HandleRemoval(functor);
				return true;
			}
		}
	}

	public enum PrefabSearch : sbyte
	{
		Missing,
		NGC,
		NetMain,
		NetAuto
	}

	[Serializable]
	public class RPCVerificationDropException : NetCull.RPCVerificationException
	{
		internal RPCVerificationDropException()
		{
		}
	}

	[Serializable]
	public abstract class RPCVerificationException : Exception
	{
		internal RPCVerificationException()
		{
		}
	}

	[Serializable]
	public class RPCVerificationLateException : NetCull.RPCVerificationDropException
	{
		internal RPCVerificationLateException()
		{
		}
	}

	[Serializable]
	public class RPCVerificationSenderException : NetCull.RPCVerificationException
	{
		public readonly uLink.NetworkPlayer Sender;

		internal RPCVerificationSenderException(uLink.NetworkPlayer Sender)
		{
			this.Sender = Sender;
		}
	}

	[Serializable]
	public class RPCVerificationWrongSenderException : NetCull.RPCVerificationSenderException
	{
		public readonly uLink.NetworkPlayer Owner;

		internal RPCVerificationWrongSenderException(uLink.NetworkPlayer Sender, uLink.NetworkPlayer Owner) : base(Sender)
		{
			this.Owner = Owner;
		}
	}

	private static class Send
	{
		public static float Rate;

		public static double Interval;

		public static float IntervalF;

		static Send()
		{
			NetCull.Send.Rate = uLink.Network.sendRate;
			NetCull.Send.Interval = 1 / (double)NetCull.sendRate;
			NetCull.Send.IntervalF = (float)NetCull.Send.Interval;
		}
	}

	public delegate void UpdateFunctor();
}