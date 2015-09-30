using System;
using UnityEngine;

[Serializable]
public class LootSpawnListReference
{
	[SerializeField]
	private string name;

	[NonSerialized]
	private bool once;

	[NonSerialized]
	private LootSpawnList _list;

	public LootSpawnList list
	{
		get
		{
			if (!this.once)
			{
				this.once = true;
				this._list = DatablockDictionary.GetLootSpawnListByName(this.name ?? string.Empty);
			}
			return this._list;
		}
		set
		{
			this.name = (!value ? string.Empty : value.name);
			this._list = value;
			this.once = true;
		}
	}

	public LootSpawnListReference()
	{
		this.name = string.Empty;
	}

	public static explicit operator LootSpawnList(LootSpawnListReference reference)
	{
		if (object.ReferenceEquals(reference, null))
		{
			return null;
		}
		return reference.list;
	}

	public static bool operator @false(LootSpawnListReference reference)
	{
		return (object.ReferenceEquals(reference, null) ? true : !reference.list);
	}

	public static bool operator @true(LootSpawnListReference reference)
	{
		bool flag;
		if (object.ReferenceEquals(reference, null))
		{
			flag = false;
		}
		else
		{
			flag = reference.list;
		}
		return flag;
	}
}