using Facepunch.Attributes;
using System;
using UnityEngine;

public sealed class NGCPrefabFieldAttribute : ObjectLookupFieldFixedTypeAttribute
{
	public NGCPrefabFieldAttribute() : base(PrefabLookupKinds.NGC, typeof(GameObject), Facepunch.Attributes.SearchMode.MainAsset, null)
	{
	}
}