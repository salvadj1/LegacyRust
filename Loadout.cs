using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public sealed class Loadout : ScriptableObject
{
	[SerializeField]
	private Loadout.Entry[] _inventory;

	[SerializeField]
	private Loadout.Entry[] _belt;

	[SerializeField]
	private Loadout.Entry[] _wearable;

	[SerializeField]
	private BlueprintDataBlock[] _defaultBlueprints;

	[NonSerialized]
	private Inventory.Addition[] _blankInventoryLoadout;

	[NonSerialized]
	private Loadout.Entry[] _minimumRequirements;

	public BlueprintDataBlock[] defaultBlueprints
	{
		get
		{
			return this._defaultBlueprints ?? Loadout.Empty.BlueprintArray;
		}
	}

	private Inventory.Addition[] emptyInventoryAdditions
	{
		get
		{
			this.GetAdditionArray(ref this._blankInventoryLoadout, false);
			return this._blankInventoryLoadout;
		}
	}

	private Loadout.Entry[] minimumRequirements
	{
		get
		{
			this.GetMinimumRequirementArray(ref this._minimumRequirements, false);
			return this._minimumRequirements;
		}
	}

	public Loadout()
	{
	}

	[DebuggerHidden]
	private static IEnumerable<Inventory.Addition> EnumerateAdditions(Loadout.Entry[][] arrays)
	{
		Loadout.<EnumerateAdditions>c__Iterator3C variable = null;
		return variable;
	}

	[DebuggerHidden]
	private static IEnumerable<Loadout.Entry> EnumerateRequired(Loadout.Entry[][] arrays)
	{
		Loadout.<EnumerateRequired>c__Iterator3D variable = null;
		return variable;
	}

	private void GetAdditionArray(ref Inventory.Addition[] array, bool forceUpdate)
	{
		if (forceUpdate || array == null)
		{
			array = (new List<Inventory.Addition>(Loadout.EnumerateAdditions(this.GetEntryArrays()))).ToArray();
		}
	}

	private Loadout.Entry[][] GetEntryArrays()
	{
		return new Loadout.Entry[][] { Loadout.LoadEntryArray(this._inventory, Inventory.Slot.Kind.Default), Loadout.LoadEntryArray(this._belt, Inventory.Slot.Kind.Belt), Loadout.LoadEntryArray(this._wearable, Inventory.Slot.Kind.Armor) };
	}

	private void GetMinimumRequirementArray(ref Loadout.Entry[] array, bool forceUpdate)
	{
		if (forceUpdate || array == null)
		{
			array = (new List<Loadout.Entry>(Loadout.EnumerateRequired(this.GetEntryArrays()))).ToArray();
		}
	}

	private static Loadout.Entry[] LoadEntryArray(Loadout.Entry[] array, Inventory.Slot.Kind kind)
	{
		array = array ?? Loadout.Empty.EntryArray;
		for (int i = 0; i < (int)array.Length; i++)
		{
			Loadout.Entry entry = array[i];
			entry.inferredSlotKind = kind;
			entry.inferredSlotOfKind = i;
		}
		return array;
	}

	private static class Empty
	{
		public readonly static Loadout.Entry[] EntryArray;

		public readonly static BlueprintDataBlock[] BlueprintArray;

		static Empty()
		{
			Loadout.Empty.EntryArray = new Loadout.Entry[0];
			Loadout.Empty.BlueprintArray = new BlueprintDataBlock[0];
		}
	}

	[Serializable]
	private class Entry
	{
		[SerializeField]
		private bool enabled;

		public ItemDataBlock item;

		[SerializeField]
		private int _useCount;

		[SerializeField]
		private bool _minimumRequirement;

		[NonSerialized]
		internal Inventory.Slot.Kind inferredSlotKind;

		[NonSerialized]
		internal int inferredSlotOfKind;

		public bool allowed
		{
			get
			{
				bool flag;
				if (!this.enabled || !this.item)
				{
					flag = false;
				}
				else
				{
					flag = (!this.item.IsSplittable() ? true : this._useCount > 0);
				}
				return flag;
			}
		}

		public bool forEmptyInventories
		{
			get
			{
				return (this._minimumRequirement ? false : this.allowed);
			}
		}

		public bool minimumRequirement
		{
			get
			{
				return (!this._minimumRequirement ? false : this.allowed);
			}
		}

		public int useCount
		{
			get
			{
				int num;
				if (!this.allowed)
				{
					num = 0;
				}
				else
				{
					num = (this.item._maxUses >= this._useCount ? (int)((byte)this._useCount) : this.item._maxUses);
				}
				return num;
			}
		}

		public Entry()
		{
		}

		public bool GetInventoryAddition(out Inventory.Addition addition)
		{
			Inventory.Addition addition1;
			if (!this.allowed)
			{
				addition1 = new Inventory.Addition();
				addition = addition1;
				return false;
			}
			addition = new Inventory.Addition();
			addition1 = addition;
			addition1.Ident = (Datablock.Ident)this.item;
			addition1.SlotPreference = Inventory.Slot.Preference.Define(this.inferredSlotKind, this.inferredSlotOfKind);
			addition1.UsesQuantity = this.useCount;
			addition = addition1;
			return true;
		}
	}
}