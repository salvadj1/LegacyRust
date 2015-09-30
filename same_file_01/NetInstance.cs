using Facepunch;
using System;
using uLink;
using UnityEngine;

[AddComponentMenu("")]
public sealed class NetInstance : IDLocal
{
	[NonSerialized]
	public CustomInstantiationArgs args;

	[NonSerialized]
	public bool prepared;

	[NonSerialized]
	public bool local;

	[NonSerialized]
	internal bool destroying;

	[NonSerialized]
	public uLink.NetworkMessageInfo info;

	[NonSerialized]
	public Facepunch.NetworkView networkView;

	[NonSerialized]
	public IDRemote localAppendage;

	[NonSerialized]
	public bool madeLocalAppendage;

	private readonly static DisposeCallbackList<NetInstance, NetInstance.CallbackFunction>.Function callbackFire;

	private DisposeCallbackList<NetInstance, NetInstance.CallbackFunction> preDestroy;

	private DisposeCallbackList<NetInstance, NetInstance.CallbackFunction> preCreate;

	private DisposeCallbackList<NetInstance, NetInstance.CallbackFunction> postCreate;

	public bool clientSide
	{
		get
		{
			return this.args.client;
		}
	}

	public static NetInstance current
	{
		get
		{
			return NetMainPrefab.zzz__currentNetInstance;
		}
	}

	public IPrefabCustomInstantiate customeInstantiateCreator
	{
		get
		{
			return this.args.customInstantiate;
		}
	}

	public bool isProxy
	{
		get
		{
			return (!this.prepared || !this.local ? false : !this.args.server);
		}
	}

	public NetMainPrefab netMain
	{
		get
		{
			return this.args.netMain;
		}
	}

	public IDMain prefab
	{
		get
		{
			return this.args.prefab;
		}
	}

	public uLink.NetworkView prefabNetworkView
	{
		get
		{
			return this.args.prefabNetworkView;
		}
	}

	public bool serverSide
	{
		get
		{
			return this.args.server;
		}
	}

	public bool wasCreatedByCustomInstantiate
	{
		get
		{
			return this.args.hasCustomInstantiator;
		}
	}

	static NetInstance()
	{
		NetInstance.callbackFire = new DisposeCallbackList<NetInstance, NetInstance.CallbackFunction>.Function(NetInstance.CallbackFire);
	}

	public NetInstance()
	{
		this.preDestroy = new DisposeCallbackList<NetInstance, NetInstance.CallbackFunction>(this, NetInstance.callbackFire);
		this.postCreate = new DisposeCallbackList<NetInstance, NetInstance.CallbackFunction>(this, NetInstance.callbackFire);
		this.preCreate = new DisposeCallbackList<NetInstance, NetInstance.CallbackFunction>(this, NetInstance.callbackFire);
	}

	private static void CallbackFire(NetInstance instance, NetInstance.CallbackFunction func)
	{
		func(instance);
	}

	public static bool IsCurrentlyDestroying(IDMain main)
	{
		NetInstance netInstance = NetInstance.current;
		return (!netInstance ? false : netInstance.idMain == main);
	}

	public static bool IsCurrentlyDestroying(IDLocal local)
	{
		NetInstance netInstance = NetInstance.current;
		return (!netInstance ? false : netInstance.idMain == local.idMain);
	}

	public static bool IsCurrentlyDestroying(IDRemote remote)
	{
		NetInstance netInstance = NetInstance.current;
		return (!netInstance ? false : netInstance.idMain == remote.idMain);
	}

	private void OnDestroy()
	{
		DisposeCallbackList<NetInstance, NetInstance.CallbackFunction> disposeCallbackList = DisposeCallbackList<NetInstance, NetInstance.CallbackFunction>.invalid;
		DisposeCallbackList<NetInstance, NetInstance.CallbackFunction> disposeCallbackList1 = disposeCallbackList;
		this.preDestroy = disposeCallbackList;
		DisposeCallbackList<NetInstance, NetInstance.CallbackFunction> disposeCallbackList2 = disposeCallbackList1;
		disposeCallbackList1 = disposeCallbackList2;
		this.preCreate = disposeCallbackList2;
		this.postCreate = disposeCallbackList1;
	}

	internal void zzz___onpostcreate()
	{
		this.postCreate.Dispose();
	}

	internal void zzz___onprecreate()
	{
		this.preCreate.Dispose();
	}

	internal void zzz___onpredestroy()
	{
		this.preDestroy.Dispose();
	}

	public event NetInstance.CallbackFunction onPostCreate
	{
		add
		{
			this.postCreate.Add(value);
		}
		remove
		{
			this.postCreate.Remove(value);
		}
	}

	public event NetInstance.CallbackFunction onPreCreate
	{
		add
		{
			this.preCreate.Add(value);
		}
		remove
		{
			this.preCreate.Remove(value);
		}
	}

	public event NetInstance.CallbackFunction onPreDestroy
	{
		add
		{
			this.preDestroy.Add(value);
		}
		remove
		{
			this.preDestroy.Remove(value);
		}
	}

	public delegate void CallbackFunction(NetInstance instance);
}