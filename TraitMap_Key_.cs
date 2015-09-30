using Facepunch.Abstract;
using System;
using UnityEngine;

public abstract class TraitMap<Key> : BaseTraitMap
where Key : TraitKey
{
	[HideInInspector]
	[SerializeField]
	private Key[] K;

	[NonSerialized]
	private KeyTypeInfo<Key>.TraitDictionary dict;

	[NonSerialized]
	private bool createdDict;

	internal abstract TraitMap<Key> __baseMap
	{
		get;
	}

	private KeyTypeInfo<Key>.TraitDictionary map
	{
		get
		{
			if (!this.createdDict)
			{
				this.dict = new KeyTypeInfo<Key>.TraitDictionary(this.K);
				TraitMap<Key> _BaseMap = this.__baseMap;
				if (_BaseMap)
				{
					_BaseMap.map.MergeUpon(this.dict);
				}
				this.createdDict = true;
			}
			return this.dict;
		}
	}

	protected TraitMap()
	{
	}

	public Key GetTrait(Type traitType)
	{
		return this.map.TryGet(traitType);
	}

	public T GetTrait<T>()
	where T : Key
	{
		return this.map.TryGetSoftCast<T>();
	}
}