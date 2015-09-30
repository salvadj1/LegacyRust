using Facepunch.Attributes;
using System;

public sealed class CharacterPrefabFieldAttribute : ObjectLookupFieldFixedTypeAttribute
{
	public CharacterPrefabFieldAttribute() : base(PrefabLookupKinds.Character, typeof(CharacterPrefab), Facepunch.Attributes.SearchMode.MainAsset, null)
	{
	}
}