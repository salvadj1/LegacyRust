using Facepunch;
using System;
using System.Collections.Generic;
using uLink;
using UnityEngine;

public class NetMainPrefab : ScriptableObject
{
	public const char prefixChar = ':';

	public const string prefixCharString = ":";

	[SerializeField]
	private IDMain _proxyPrefab;

	[SerializeField]
	private IDMain _serverPrefab;

	[SerializeField]
	private IDMain _localPrefab;

	[SerializeField]
	private IDRemote _localAppend;

	[SerializeField]
	private string _pathToLocalAppend;

	[SerializeField]
	private UnityEngine.Object _customInstantiator;

	[NonSerialized]
	public readonly Type MinimumTypeAllowed;

	private readonly NetworkInstantiator.Creator creator;

	private readonly NetworkInstantiator.Destroyer destroyer;

	private static NetInstance _currentNetInstance;

	private string _name;

	private string _originalName;

	private static bool ginit;

	private IDRemote localAppend
	{
		get
		{
			return this._localAppend;
		}
	}

	public Transform localAppendTransformInPrefab
	{
		get
		{
			return this.GetLocalAppendTransform(this.proxyPrefab);
		}
	}

	public IDMain localPrefab
	{
		get
		{
			return (!this._localPrefab ? this.proxyPrefab : this._localPrefab);
		}
	}

	public new string name
	{
		get
		{
			string str = base.name;
			if (str != this._originalName)
			{
				if (!Application.isPlaying || string.IsNullOrEmpty(this._originalName))
				{
					this._originalName = str;
					this._name = NetMainPrefab.DressName(str);
				}
				else
				{
					Debug.LogWarning("You can't rename proxy instantiations at runtime!", this);
				}
			}
			return this._name;
		}
	}

	public IDMain prefab
	{
		get
		{
			return this.proxyPrefab;
		}
	}

	public IDMain proxyPrefab
	{
		get
		{
			return (!this._proxyPrefab ? this._serverPrefab : this._proxyPrefab);
		}
	}

	public IDMain serverPrefab
	{
		get
		{
			return (!this._serverPrefab ? this.proxyPrefab : this._serverPrefab);
		}
	}

	internal static NetInstance zzz__currentNetInstance
	{
		get
		{
			return NetMainPrefab._currentNetInstance;
		}
	}

	public NetMainPrefab() : this(typeof(IDMain), false)
	{
	}

	protected NetMainPrefab(Type minimumType) : this(minimumType, true)
	{
	}

	private NetMainPrefab(Type minimumType, bool typeCheck)
	{
		if (typeCheck && !typeof(IDMain).IsAssignableFrom(minimumType))
		{
			throw new ArgumentOutOfRangeException("minimumType", "must be assignable to IDMain");
		}
		this.MinimumTypeAllowed = minimumType;
		this.CollectCallbacks(out this.creator, out this.destroyer);
	}

	protected uLink.NetworkView _Creator(string prefabName, NetworkInstantiateArgs args, uLink.NetworkMessageInfo info)
	{
		NetInstance netInstance = this.Summon(this.proxyPrefab, false, ref args);
		if (!netInstance)
		{
			return null;
		}
		Facepunch.NetworkView networkView = netInstance.networkView;
		if (!networkView)
		{
			return null;
		}
		info = new uLink.NetworkMessageInfo(info, networkView);
		NetInstance netInstance1 = NetMainPrefab._currentNetInstance;
		try
		{
			NetMainPrefab._currentNetInstance = netInstance;
			netInstance.info = info;
			netInstance.prepared = true;
			netInstance.local = args.viewID.isMine;
			bool flag = false;
			IDRemote dRemote = null;
			if (netInstance.local)
			{
				IDRemote dRemote1 = this.localAppend;
				if (dRemote1)
				{
					dRemote = NetMainPrefab.DoLocalAppend(dRemote1, netInstance.idMain, this.GetLocalAppendTransform(netInstance.idMain));
					flag = true;
				}
			}
			netInstance.zzz___onprecreate();
			this.StandardInitialization(flag, dRemote, netInstance, networkView, ref info);
			netInstance.zzz___onpostcreate();
		}
		finally
		{
			NetMainPrefab._currentNetInstance = netInstance1;
		}
		return networkView;
	}

	protected void _Destroyer(uLink.NetworkView networkView)
	{
		NetInstance netInstance = NetMainPrefab._currentNetInstance;
		try
		{
			NetInstance component = networkView.GetComponent<NetInstance>();
			NetMainPrefab._currentNetInstance = component;
			if (component)
			{
				component.zzz___onpredestroy();
			}
			UnityEngine.Object.Destroy(networkView.gameObject);
		}
		finally
		{
			NetMainPrefab._currentNetInstance = netInstance;
		}
	}

	protected virtual void CollectCallbacks(out NetworkInstantiator.Creator creator, out NetworkInstantiator.Destroyer destroyer)
	{
		creator = new NetworkInstantiator.Creator(this._Creator);
		destroyer = new NetworkInstantiator.Destroyer(this._Destroyer);
	}

	private Facepunch.NetworkView Create(ref CustomInstantiationArgs args, out IDMain instance)
	{
		Facepunch.NetworkView networkView;
		Facepunch.NetworkView networkView1;
		if (float.IsNaN(args.position.x) || float.IsNaN(args.position.y) || float.IsNaN(args.position.z))
		{
			Debug.LogWarning(string.Concat("NetMainPrefab -> Create -  args.position = ", args.position));
			Debug.LogWarning("This means you're creating an object with a bad position!");
		}
		NetInstance netInstance = NetMainPrefab._currentNetInstance;
		try
		{
			NetMainPrefab._currentNetInstance = null;
			if (args.hasCustomInstantiator)
			{
				instance = null;
				try
				{
					instance = args.customInstantiate.CustomInstantiatePrefab(ref args);
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					Debug.LogError(string.Format("Thrown Exception during custom instantiate via '{0}' with instantiation '{2}'\r\ndefault instantiation will now occur --  exception follows..\r\n{1}", args.customInstantiate, exception, this), this);
					if (instance)
					{
						UnityEngine.Object.Destroy(instance);
					}
					instance = null;
				}
				try
				{
					networkView = instance.networkView;
					if (networkView == null)
					{
						Debug.LogWarning(string.Format("The custom instantiator '{0}' with instantiation '{1}' did not return a idmain with a network view. so its being added", args.customInstantiate, this), this);
						networkView = instance.gameObject.AddComponent<uLinkNetworkView>();
					}
				}
				catch (Exception exception3)
				{
					Exception exception2 = exception3;
					networkView = null;
					Debug.LogError(string.Format("The custom instantiator '{0}' did not instantiate a IDMain with a networkview or something else with instantiation '{2}'.. \r\n {1}", args.customInstantiate, exception2, this), this);
				}
				if (networkView)
				{
					networkView1 = networkView;
					return networkView1;
				}
			}
			Facepunch.NetworkView networkView2 = (Facepunch.NetworkView)NetworkInstantiatorUtility.Instantiate(args.prefabNetworkView, args.args);
			instance = networkView2.GetComponent<IDMain>();
			networkView1 = networkView2;
		}
		finally
		{
			NetMainPrefab._currentNetInstance = netInstance;
		}
		return networkView1;
	}

	public static IDRemote DoLocalAppend(IDRemote localAppend, IDMain instance, Transform appendPoint)
	{
		Transform transforms = localAppend.transform;
		if (localAppend.transform != localAppend.transform.root)
		{
			Debug.LogWarning("The localAppend transform was not a root");
		}
		IDRemote dRemote = (IDRemote)UnityEngine.Object.Instantiate(localAppend, appendPoint.TransformPoint(transforms.localPosition), appendPoint.rotation * transforms.localRotation);
		Transform transforms1 = dRemote.transform;
		transforms1.parent = appendPoint;
		transforms1.localPosition = transforms.localPosition;
		transforms1.localRotation = transforms.localRotation;
		transforms1.localScale = transforms.localScale;
		dRemote.idMain = instance;
		IDRemote[] componentsInChildren = instance.GetComponentsInChildren<IDRemote>();
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			IDRemote dRemote1 = componentsInChildren[i];
			if (!dRemote1.idMain)
			{
				dRemote1.idMain = instance;
			}
		}
		return dRemote;
	}

	public static string DressName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("name cannot be null or empty", "name");
		}
		if (name[0] != ':')
		{
			for (int i = name.Length - 1; i >= 0; i--)
			{
				if (char.IsUpper(name, i))
				{
					Debug.LogWarning(string.Format("the name \":{0}\" contains upper case characters. it should not.", name));
					return string.Concat(":", name.ToLower());
				}
			}
			return string.Concat(":", name);
		}
		int length = name.Length;
		if (length == 1)
		{
			throw new ArgumentException("if name includes the prefix char it must be followed by at least one more char.", "name");
		}
		for (int j = length - 1; j > 0; j--)
		{
			if (char.IsUpper(name, j))
			{
				Debug.LogWarning(string.Format("the name \"{0}\" contains upper case characters. it should not.", name));
				return name.ToLower();
			}
		}
		string lower = name.ToLower();
		if (lower != name)
		{
			Debug.LogWarning(string.Format("the name \"{0}\" contains upper case characters. it should not.", name));
		}
		if (lower[0] == ':')
		{
			return lower;
		}
		return string.Concat(":", lower);
	}

	public static void EnsurePrefabName(string name)
	{
		NetMainPrefabNameException netMainPrefabNameException;
		if (!NetMainPrefab.ValidatePrefabNameOrMakeException(name, out netMainPrefabNameException))
		{
			throw netMainPrefabNameException;
		}
	}

	public static Transform GetLocalAppendTransform(IDMain instanceOrPrefab, string _pathToLocalAppend)
	{
		if (!instanceOrPrefab)
		{
			return null;
		}
		if (string.IsNullOrEmpty(_pathToLocalAppend))
		{
			return instanceOrPrefab.transform;
		}
		Transform transforms = instanceOrPrefab.transform.FindChild(_pathToLocalAppend);
		if (!transforms)
		{
			Debug.LogError(string.Concat("The transform path:\"", _pathToLocalAppend, "\" is no longer valid for given transform. returning the transform of the main"), instanceOrPrefab);
			transforms = instanceOrPrefab.transform;
		}
		return transforms;
	}

	public Transform GetLocalAppendTransform(IDMain instanceOrPrefab)
	{
		return NetMainPrefab.GetLocalAppendTransform(instanceOrPrefab, this._pathToLocalAppend);
	}

	public static void IssueLocallyAppended(IDRemote appended, IDMain instance)
	{
		appended.BroadcastMessage("OnLocallyAppended", instance, SendMessageOptions.DontRequireReceiver);
	}

	public static T Lookup<T>(string key)
	where T : UnityEngine.Object
	{
		NetMainPrefab netMainPrefab;
		if (!NetMainPrefab.ginit)
		{
			return (T)null;
		}
		if (!NetMainPrefab.g.dict.TryGetValue(key, out netMainPrefab))
		{
			Debug.LogWarning(string.Concat("There was no registered proxy with key ", key));
			return (T)null;
		}
		if (typeof(NetMainPrefab).IsAssignableFrom(typeof(T)))
		{
			return (T)netMainPrefab;
		}
		if (typeof(GameObject).IsAssignableFrom(typeof(T)))
		{
			return (T)netMainPrefab.prefab.gameObject;
		}
		if (!typeof(Component).IsAssignableFrom(typeof(T)))
		{
			return (T)null;
		}
		if (typeof(IDMain).IsAssignableFrom(typeof(T)))
		{
			return (T)netMainPrefab.prefab;
		}
		return (T)netMainPrefab.prefab.GetComponent(typeof(T));
	}

	public static T LookupInChildren<T>(string key)
	where T : Component
	{
		NetMainPrefab netMainPrefab;
		if (!NetMainPrefab.ginit)
		{
			return (T)null;
		}
		if (NetMainPrefab.g.dict.TryGetValue(key, out netMainPrefab))
		{
			return netMainPrefab.prefab.GetComponentInChildren<T>();
		}
		Debug.LogWarning(string.Concat("There was no registered proxy with key ", key));
		return (T)null;
	}

	public void Register(bool forceReplace)
	{
		NetworkInstantiator.Add(this.name, this.creator, this.destroyer, forceReplace);
		NetMainPrefab.g.dict[this.name] = this;
	}

	private bool ShouldDoStandardInitialization(NetInstance instance)
	{
		bool flag;
		UnityEngine.Object obj = this._customInstantiator;
		try
		{
			this._customInstantiator = instance;
			if (instance.args.hasCustomInstantiator)
			{
				try
				{
					flag = instance.args.customInstantiate.InitializePrefabInstance(instance);
					return flag;
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					Debug.LogError(string.Format("A exception was thrown during InitializePrefabInstance with '{0}' as custom instantiate, prefab '{1}' instance '{2}'.\r\ndoing standard initialization..\r\n{3}", new object[] { instance.args.customInstantiate, this, instance.args.prefab, exception }), instance);
				}
			}
			flag = true;
		}
		finally
		{
			this._customInstantiator = obj;
		}
		return flag;
	}

	protected virtual void StandardInitialization(bool didAppend, IDRemote appended, NetInstance instance, Facepunch.NetworkView view, ref uLink.NetworkMessageInfo info)
	{
		if (didAppend)
		{
			NetMainPrefab.IssueLocallyAppended(appended, instance.idMain);
		}
		if (this.ShouldDoStandardInitialization(instance))
		{
			NetworkInstantiatorUtility.BroadcastOnNetworkInstantiate(view, "uLink_OnNetworkInstantiate", info);
		}
	}

	private NetInstance Summon(IDMain prefab, bool isServer, ref NetworkInstantiateArgs niargs)
	{
		IDMain dMain;
		CustomInstantiationArgs customInstantiationArg = new CustomInstantiationArgs(this, this._customInstantiator, prefab, ref niargs, isServer);
		Facepunch.NetworkView networkView = this.Create(ref customInstantiationArg, out dMain);
		NetInstance netInstance = networkView.gameObject.AddComponent<NetInstance>();
		netInstance.args = customInstantiationArg;
		netInstance.idMain = dMain;
		netInstance.prepared = false;
		netInstance.networkView = networkView;
		return netInstance;
	}

	public static bool ValidatePrefabNameOrMakeException(string name, out NetMainPrefabNameException e)
	{
		if (name == null)
		{
			e = new NetMainPrefabNameException("name", name, "null");
		}
		else if (name.Length >= 2)
		{
			if (name[0] == ':')
			{
				e = null;
				return true;
			}
			e = new NetMainPrefabNameException("name", name, "name did not begin with the prefix character");
		}
		else
		{
			e = new NetMainPrefabNameException("name", name, "name must include the prefix character and at least one other after");
		}
		return false;
	}

	private static class g
	{
		public static Dictionary<string, NetMainPrefab> dict;

		static g()
		{
			NetMainPrefab.g.dict = new Dictionary<string, NetMainPrefab>();
			NetMainPrefab.ginit = true;
		}
	}
}