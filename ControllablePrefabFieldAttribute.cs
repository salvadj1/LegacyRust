using Facepunch.Attributes;
using System;

public sealed class ControllablePrefabFieldAttribute : ObjectLookupFieldFixedTypeAttribute
{
	public ControllablePrefabFieldAttribute() : base(PrefabLookupKinds.Controllable, typeof(ControllablePrefab), Facepunch.Attributes.SearchMode.MainAsset, null)
	{
	}
}