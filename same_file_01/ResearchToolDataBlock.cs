using System;
using uLink;
using UnityEngine;

public class ResearchToolDataBlock : ToolDataBlock
{
	public ResearchToolDataBlock()
	{
	}

	public override bool CanWork(IToolItem tool, Inventory workbenchInv)
	{
		if (workbenchInv.occupiedSlotCount > 2)
		{
			Debug.Log("Too many items for research");
			return false;
		}
		IInventoryItem firstItemNotTool = base.GetFirstItemNotTool(tool, workbenchInv);
		if (firstItemNotTool != null && firstItemNotTool.datablock.isResearchable && BlueprintDataBlock.FindBlueprintForItem(firstItemNotTool.datablock))
		{
			return true;
		}
		Debug.Log("Can't work!?!!?");
		return false;
	}

	public override bool CompleteWork(IToolItem tool, Inventory workbenchInv)
	{
		BlueprintDataBlock blueprintDataBlock;
		if (!this.CanWork(tool, workbenchInv))
		{
			return false;
		}
		if (!BlueprintDataBlock.FindBlueprintForItem<BlueprintDataBlock>(base.GetFirstItemNotTool(tool, workbenchInv).datablock, out blueprintDataBlock))
		{
			return false;
		}
		workbenchInv.AddItem(blueprintDataBlock, Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, blueprintDataBlock.IsSplittable(), Inventory.Slot.Kind.Belt), 1);
		return true;
	}

	protected override IInventoryItem ConstructItem()
	{
		return new ResearchToolDataBlock.ITEM_TYPE(this);
	}

	public override string GetItemDescription()
	{
		return "Drag this onto another item to learn how to craft it. Requires 1 Paper.";
	}

	public override float GetWorkDuration(IToolItem tool)
	{
		return 30f;
	}

	private sealed class ITEM_TYPE : ResearchToolItem<ResearchToolDataBlock>, IInventoryItem, IResearchToolItem, IToolItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(ResearchToolDataBlock BLOCK) : base(BLOCK)
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