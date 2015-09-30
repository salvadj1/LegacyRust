using System;
using System.Collections.Generic;
using uLink;
using UnityEngine;

public class ItemModDataBlock : ItemDataBlock
{
	[SerializeField]
	private string modRepresentationTypeName = "ItemModRepresentation";

	public ItemModFlags modFlag;

	public AudioClip onSound;

	public AudioClip offSound;

	private readonly Type minimumModRepresentationType;

	public bool hasModRepresentation
	{
		get
		{
			return !string.IsNullOrEmpty(this.modRepresentationTypeName);
		}
	}

	protected ItemModDataBlock(Type minimumModRepresentationType)
	{
		if (!typeof(ItemModRepresentation).IsAssignableFrom(minimumModRepresentationType))
		{
			throw new ArgumentOutOfRangeException("minimumModRepresentationType", minimumModRepresentationType, "!typeof(ItemModRepresentation).IsAssignableFrom(minimumModRepresentationType)");
		}
		this.minimumModRepresentationType = minimumModRepresentationType;
	}

	public ItemModDataBlock() : this(typeof(ItemModRepresentation))
	{
	}

	internal bool AddModRepresentationComponent(GameObject gameObject, out ItemModRepresentation rep)
	{
		ItemModDataBlock.g.TypePair typePair;
		if (this.hasModRepresentation)
		{
			if (!ItemModDataBlock.g.cachedTypeLookup.TryGetValue(base.name, out typePair) || typePair.typeString != this.modRepresentationTypeName)
			{
				typePair = new ItemModDataBlock.g.TypePair()
				{
					typeString = this.modRepresentationTypeName,
					type = Types.GetType(typePair.typeString, "Assembly-CSharp")
				};
				if (typePair.type == null)
				{
					Debug.LogError(string.Format("modRepresentationTypeName:{0} resolves to no type", typePair.typeString), this);
				}
				else if (!this.minimumModRepresentationType.IsAssignableFrom(typePair.type))
				{
					Debug.LogError(string.Format("modRepresentationTypeName:{0} resolved to {1} but {1} is not a {2}", typePair.typeString, typePair.type, this.minimumModRepresentationType), this);
					typePair.type = null;
				}
				ItemModDataBlock.g.cachedTypeLookup[base.name] = typePair;
			}
			if (typePair.type != null)
			{
				rep = (ItemModRepresentation)gameObject.AddComponent(typePair.type);
				if (rep)
				{
					this.CustomizeItemModRepresentation(rep);
					if (rep)
					{
						return true;
					}
				}
			}
		}
		rep = null;
		return false;
	}

	internal void BindAsLocal(ref ModViewModelAddArgs a)
	{
		this.InstallToViewModel(ref a);
	}

	internal void BindAsProxy(ItemModRepresentation rep)
	{
		this.InstallToItemModRepresentation(rep);
	}

	protected override IInventoryItem ConstructItem()
	{
		return new ItemModDataBlock.ITEM_TYPE(this);
	}

	protected virtual void CustomizeItemModRepresentation(ItemModRepresentation rep)
	{
	}

	protected virtual void InstallToItemModRepresentation(ItemModRepresentation rep)
	{
	}

	protected virtual bool InstallToViewModel(ref ModViewModelAddArgs a)
	{
		return false;
	}

	protected void OnDestroy()
	{
	}

	protected override void SecureWriteMemberValues(uLink.BitStream stream)
	{
		base.SecureWriteMemberValues(stream);
		stream.Write<ItemModFlags>(this.modFlag, new object[0]);
		stream.Write<string>(this.modRepresentationTypeName, new object[0]);
	}

	internal void UnBindAsLocal(ref ModViewModelRemoveArgs a)
	{
		this.UninstallFromViewModel(ref a);
	}

	internal void UnBindAsProxy(ItemModRepresentation rep)
	{
		this.UninstallFromItemModRepresentation(rep);
	}

	protected virtual void UninstallFromItemModRepresentation(ItemModRepresentation rep)
	{
	}

	protected virtual void UninstallFromViewModel(ref ModViewModelRemoveArgs a)
	{
	}

	private static class g
	{
		public static Dictionary<string, ItemModDataBlock.g.TypePair> cachedTypeLookup;

		static g()
		{
			ItemModDataBlock.g.cachedTypeLookup = new Dictionary<string, ItemModDataBlock.g.TypePair>();
		}

		public class TypePair
		{
			public string typeString;

			public Type type;

			public TypePair()
			{
			}
		}
	}

	private sealed class ITEM_TYPE : ItemModItem<ItemModDataBlock>, IInventoryItem, IItemModItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(ItemModDataBlock BLOCK) : base(BLOCK)
		{
		}

		// privatescope
		internal int IInventoryItem.AddUses(int count)
		{
			return base.AddUses(count);
		}

		// privatescope
		internal bool IInventoryItem.Consume(ref int count)
		{
			return base.Consume(ref count);
		}

		// privatescope
		internal void IInventoryItem.Deserialize(uLink.BitStream stream)
		{
			base.Deserialize(stream);
		}

		// privatescope
		internal bool IInventoryItem.get_active()
		{
			return base.active;
		}

		// privatescope
		internal Character IInventoryItem.get_character()
		{
			return base.character;
		}

		// privatescope
		internal float IInventoryItem.get_condition()
		{
			return base.condition;
		}

		// privatescope
		internal Controllable IInventoryItem.get_controllable()
		{
			return base.controllable;
		}

		// privatescope
		internal Controller IInventoryItem.get_controller()
		{
			return base.controller;
		}

		// privatescope
		internal bool IInventoryItem.get_dirty()
		{
			return base.dirty;
		}

		// privatescope
		internal bool IInventoryItem.get_doNotSave()
		{
			return base.doNotSave;
		}

		// privatescope
		internal IDMain IInventoryItem.get_idMain()
		{
			return base.idMain;
		}

		// privatescope
		internal Inventory IInventoryItem.get_inventory()
		{
			return base.inventory;
		}

		// privatescope
		internal bool IInventoryItem.get_isInLocalInventory()
		{
			return base.isInLocalInventory;
		}

		// privatescope
		internal float IInventoryItem.get_lastUseTime()
		{
			return base.lastUseTime;
		}

		// privatescope
		internal float IInventoryItem.get_maxcondition()
		{
			return base.maxcondition;
		}

		// privatescope
		internal int IInventoryItem.get_slot()
		{
			return base.slot;
		}

		// privatescope
		internal int IInventoryItem.get_uses()
		{
			return base.uses;
		}

		// privatescope
		internal float IInventoryItem.GetConditionPercent()
		{
			return base.GetConditionPercent();
		}

		// privatescope
		internal bool IInventoryItem.IsBroken()
		{
			return base.IsBroken();
		}

		// privatescope
		internal bool IInventoryItem.IsDamaged()
		{
			return base.IsDamaged();
		}

		// privatescope
		internal bool IInventoryItem.MarkDirty()
		{
			return base.MarkDirty();
		}

		// privatescope
		internal void IInventoryItem.Serialize(uLink.BitStream stream)
		{
			base.Serialize(stream);
		}

		// privatescope
		internal void IInventoryItem.set_lastUseTime(float value)
		{
			base.lastUseTime = value;
		}

		// privatescope
		internal void IInventoryItem.SetCondition(float condition)
		{
			base.SetCondition(condition);
		}

		// privatescope
		internal void IInventoryItem.SetMaxCondition(float condition)
		{
			base.SetMaxCondition(condition);
		}

		// privatescope
		internal void IInventoryItem.SetUses(int count)
		{
			base.SetUses(count);
		}

		// privatescope
		internal bool IInventoryItem.TryConditionLoss(float probability, float percentLoss)
		{
			return base.TryConditionLoss(probability, percentLoss);
		}
	}
}