using Facepunch.Attributes;
using System;

public sealed class NetworkPrefabFieldAttribute : ObjectLookupFieldFixedTypeAttribute
{
	public NetworkPrefabFieldAttribute() : base(PrefabLookupKinds.Net, null, Facepunch.Attributes.SearchMode.MainAsset, null)
	{
	}
}