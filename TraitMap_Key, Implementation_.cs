using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TraitMap<Key, Implementation> : TraitMap<Key>
where Key : TraitKey
where Implementation : TraitMap<Key, Implementation>
{
	[HideInInspector]
	[SerializeField]
	private Implementation B;

	private static bool anyRegistry;

	internal sealed override TraitMap<Key> __baseMap
	{
		get
		{
			return (object)this.B;
		}
	}

	public static ICollection<Implementation> AllRegistered
	{
		get
		{
			if (!TraitMap<Key, Implementation>.anyRegistry)
			{
				return new Implementation[0];
			}
			return TraitMap<Key, Implementation>.LookupRegister.dict.Values;
		}
	}

	public static bool AnyRegistered
	{
		get
		{
			return TraitMap<Key, Implementation>.anyRegistry;
		}
	}

	protected TraitMap()
	{
	}

	internal sealed override void BindToRegistry()
	{
		TraitMap<Key, Implementation>.LookupRegister.Add((Implementation)this);
	}

	public static bool ByName(string name, out Implementation map)
	{
		if (TraitMap<Key, Implementation>.anyRegistry)
		{
			return TraitMap<Key, Implementation>.LookupRegister.dict.TryGetValue(name, out map);
		}
		map = (Implementation)null;
		return false;
	}

	public static Implementation ByName(string name)
	{
		Implementation implementation;
		return (!TraitMap<Key, Implementation>.anyRegistry || !TraitMap<Key, Implementation>.LookupRegister.dict.TryGetValue(name, out implementation) ? (Implementation)null : implementation);
	}

	private static class LookupRegister
	{
		public readonly static Dictionary<string, Implementation> dict;

		static LookupRegister()
		{
			TraitMap<Key, Implementation>.LookupRegister.dict = new Dictionary<string, Implementation>(StringComparer.InvariantCultureIgnoreCase);
			TraitMap<Key, Implementation>.anyRegistry = true;
		}

		public static void Add(Implementation implementation)
		{
			TraitMap<Key, Implementation>.LookupRegister.dict[implementation.name] = implementation;
		}
	}
}