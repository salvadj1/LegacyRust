using Facepunch;
using Facepunch.Load;
using Facepunch.Load.Downloaders;
using Facepunch.Traits;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class RustLoader : Facepunch.MonoBehaviour, IRustLoaderTasks
{
	[SerializeField]
	public string releaseBundleLoaderFilePath = "bundles/manifest.txt";

	[SerializeField]
	public GameObject[] messageReceivers;

	[NonSerialized]
	private Loader loader;

	public bool destroyGameObjectOnceLoaded;

	[NonSerialized]
	private string preloadedJsonLoaderText;

	[NonSerialized]
	private string preloadedJsonLoaderRoot;

	[NonSerialized]
	private bool preloadedJsonLoader;

	[NonSerialized]
	private string preloadedJsonLoaderError;

	bool IRustLoaderTasks.Active
	{
		get
		{
			return this.loader != null;
		}
	}

	IDownloadTask IRustLoaderTasks.ActiveGroup
	{
		get
		{
			if (this.loader == null)
			{
				return null;
			}
			return this.loader.CurrentGroup;
		}
	}

	IEnumerable<IDownloadTask> IRustLoaderTasks.ActiveJobs
	{
		get
		{
			RustLoader.<>c__Iterator8 variable = null;
			return variable;
		}
	}

	IEnumerable<IDownloadTask> IRustLoaderTasks.Groups
	{
		get
		{
			RustLoader.<>c__Iterator7 variable = null;
			return variable;
		}
	}

	IEnumerable<IDownloadTask> IRustLoaderTasks.Jobs
	{
		get
		{
			RustLoader.<>c__Iterator9 variable = null;
			return variable;
		}
	}

	IDownloadTask IRustLoaderTasks.Overall
	{
		get
		{
			return this.loader;
		}
	}

	public IRustLoaderTasks Tasks
	{
		get
		{
			return this;
		}
	}

	public RustLoader()
	{
	}

	public void AddMessageReceiver(GameObject newReceiver)
	{
		if (!newReceiver)
		{
			return;
		}
		if (this.messageReceivers == null)
		{
			this.messageReceivers = new GameObject[] { newReceiver };
		}
		else if (Array.IndexOf<GameObject>(this.messageReceivers, newReceiver) == -1)
		{
			Array.Resize<GameObject>(ref this.messageReceivers, (int)this.messageReceivers.Length + 1);
			this.messageReceivers[(int)this.messageReceivers.Length - 1] = newReceiver;
		}
	}

	private void Callback_OnBundleAllLoaded(AssetBundle[] bundles, Item[] items)
	{
		this.DispatchLoadMessage("OnRustBundleCompleteLoaded", this);
	}

	private void Callback_OnBundleGroupLoaded(AssetBundle[] bundles, Item[] items)
	{
		this.DispatchLoadMessage("OnRustBundleGroupLoaded", this);
	}

	private void Callback_OnBundleLoaded(AssetBundle bundle, Item item)
	{
		this.DispatchLoadMessage("OnRustBundleLoaded", this);
	}

	private void DispatchLoadMessage(string message, object value)
	{
		if (this.messageReceivers != null)
		{
			GameObject[] gameObjectArray = this.messageReceivers;
			for (int i = 0; i < (int)gameObjectArray.Length; i++)
			{
				GameObject gameObject = gameObjectArray[i];
				if (gameObject)
				{
					gameObject.SendMessage(message, value, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	private static void OnResourcesLoaded()
	{
		BaseTraitMap[] baseTraitMapArray = Bundling.LoadAll<BaseTraitMap>();
		for (int i = 0; i < (int)baseTraitMapArray.Length; i++)
		{
			BaseTraitMap baseTraitMap = baseTraitMapArray[i];
			if (baseTraitMap)
			{
				try
				{
					Binder.BindMap(baseTraitMap);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(exception, baseTraitMap);
				}
			}
		}
		DatablockDictionary.Initialize();
		NetMainPrefab[] netMainPrefabArray = Bundling.LoadAll<NetMainPrefab>();
		for (int j = 0; j < (int)netMainPrefabArray.Length; j++)
		{
			NetMainPrefab netMainPrefab = netMainPrefabArray[j];
			try
			{
				netMainPrefab.Register(true);
			}
			catch (Exception exception1)
			{
				UnityEngine.Debug.LogException(exception1, netMainPrefab);
			}
		}
		uLinkNetworkView[] uLinkNetworkViewArray = Bundling.LoadAll<uLinkNetworkView>();
		for (int k = 0; k < (int)uLinkNetworkViewArray.Length; k++)
		{
			uLinkNetworkView _uLinkNetworkView = uLinkNetworkViewArray[k];
			try
			{
				NetCull.RegisterNetAutoPrefab(_uLinkNetworkView);
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogException(exception2, _uLinkNetworkView);
			}
		}
		NGC.Register(NGCConfiguration.Load());
	}

	public void ServerInit()
	{
		UnityEngine.Object.Destroy(base.GetComponent<RustLoaderInstantiateOnComplete>());
	}

	public void SetPreloadedManifest(string text, string path, string error)
	{
		if (this.loader != null)
		{
			throw new InvalidOperationException("The loader has already begun. Its too late!");
		}
		this.preloadedJsonLoaderText = text;
		this.preloadedJsonLoaderRoot = path ?? string.Empty;
		this.preloadedJsonLoader = true;
		this.preloadedJsonLoaderError = error;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		RustLoader.<Start>c__IteratorA variable = null;
		return variable;
	}
}