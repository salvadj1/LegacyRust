using System;
using UnityEngine;

public sealed class FPGrassAssets : MonoBehaviour, IFPGrassAsset
{
	public UnityEngine.Object[] All;

	public FPGrassAssets()
	{
	}

	public bool Contains(UnityEngine.Object asset)
	{
		return Array.IndexOf<UnityEngine.Object>(this.All, asset) != -1;
	}
}