using System;
using uLink;
using UnityEngine;

public class ToolDataBlock : ItemDataBlock
{
	public ToolDataBlock()
	{
	}

	public virtual bool CanWork(IToolItem tool, Inventory workbenchInv)
	{
		return false;
	}

	public virtual bool CompleteWork(IToolItem tool, Inventory workbenchInv)
	{
		return false;
	}

	protected override IInventoryItem ConstructItem()
	{
		return new ToolDataBlock.ITEM_TYPE(this);
	}

	public IInventoryItem GetFirstItemNotTool(IToolItem tool, Inventory workbenchInv)
	{
		IInventoryItem inventoryItem;
		IInventoryItem inventoryItem1;
		Inventory.OccupiedIterator occupiedIterator = workbenchInv.occupiedIterator;
		try
		{
			while (occupiedIterator.Next(out inventoryItem))
			{
				if (object.ReferenceEquals(inventoryItem, tool))
				{
					continue;
				}
				inventoryItem1 = inventoryItem;
				return inventoryItem1;
			}
			Debug.LogWarning("Could not find target item");
			return null;
		}
		finally
		{
			((IDisposable)(object)occupiedIterator).Dispose();
		}
		return inventoryItem1;
	}

	public virtual float GetWorkDuration(IToolItem tool)
	{
		return 1f;
	}

	private sealed class ITEM_TYPE : ToolItem<ToolDataBlock>, IInventoryItem, IToolItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(ToolDataBlock BLOCK) : base(BLOCK)
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