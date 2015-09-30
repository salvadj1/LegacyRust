using System;
using UnityEngine;

public static class Resources
{
	private const string kDontUse = "Do not use Resources. Use Bundles.";

	private const bool kErrorNotWarning = false;

	public static UnityEngine.Object[] FindObjectsOfTypeAll(Type type)
	{
		return UnityEngine.Resources.FindObjectsOfTypeAll(type);
	}

	[Obsolete("Do not use Resources. Use Bundles.", false)]
	public static UnityEngine.Object Load(string path)
	{
		return UnityEngine.Resources.Load(path);
	}

	[Obsolete("Do not use Resources. Use Bundles.", false)]
	public static UnityEngine.Object Load(string path, Type type)
	{
		return UnityEngine.Resources.Load(path, type);
	}

	[Obsolete("Do not use Resources. Use Bundles.", false)]
	public static UnityEngine.Object[] LoadAll(string path)
	{
		return UnityEngine.Resources.LoadAll(path);
	}

	[Obsolete("Do not use Resources. Use Bundles.", false)]
	public static UnityEngine.Object[] LoadAll(string path, Type type)
	{
		return UnityEngine.Resources.LoadAll(path, type);
	}

	public static void UnloadAsset(UnityEngine.Object assetToUnload)
	{
		UnityEngine.Resources.UnloadAsset(assetToUnload);
	}

	public static AsyncOperation UnloadUnusedAssets()
	{
		return UnityEngine.Resources.UnloadUnusedAssets();
	}
}