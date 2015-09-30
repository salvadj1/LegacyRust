using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public sealed class ArmorModelGroup : ScriptableObject, IEnumerable, IEnumerable<ArmorModel>
{
	[SerializeField]
	private ArmorModelCollection collection;

	public ArmorModelMemberMap armorModelMemberMap
	{
		get
		{
			return this.collection.ToMemberMap();
		}
	}

	public ArmorModel this[ArmorModelSlot slot]
	{
		get
		{
			return this.collection[slot];
		}
	}

	public ArmorModelGroup()
	{
	}

	public T GetArmorModel<T>()
	where T : ArmorModel, new()
	{
		return this.collection.GetArmorModel<T>();
	}

	public IEnumerator<ArmorModel> GetEnumerator()
	{
		return this.collection.GetEnumerator();
	}

	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)this.collection).GetEnumerator();
	}
}