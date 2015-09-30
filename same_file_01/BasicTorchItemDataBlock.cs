using System;
using uLink;
using UnityEngine;

public class BasicTorchItemDataBlock : HeldItemDataBlock
{
	public GameObject FirstPersonLightPrefab;

	public GameObject ThirdPersonLightPrefab;

	public BasicTorchItemDataBlock()
	{
	}

	protected override IInventoryItem ConstructItem()
	{
		return new BasicTorchItemDataBlock.ITEM_TYPE(this);
	}

	public override void DoAction2(uLink.BitStream stream, ItemRepresentation itemRep, ref uLink.NetworkMessageInfo info)
	{
		this.Ignite(null, itemRep, null);
	}

	public override void DoAction3(uLink.BitStream stream, ItemRepresentation itemRep, ref uLink.NetworkMessageInfo info)
	{
		this.Extinguish(itemRep);
	}

	public void DoActualExtinguish(ItemRepresentation itemRep, IBasicTorchItem itemInstance, ViewModel vm)
	{
		if (itemInstance == null)
		{
			Debug.Log("inst null");
		}
		if (itemRep == null)
		{
			Debug.Log("rep null");
		}
		if (vm == null)
		{
			Debug.Log("vm null ");
		}
		itemInstance.Extinguish();
	}

	public void DoActualIgnite(ItemRepresentation itemRep, IBasicTorchItem itemInstance, ViewModel vm)
	{
		this.Ignite(vm, itemRep, itemInstance);
		itemRep.Action(2, uLink.RPCMode.Server);
	}

	public void Extinguish(ItemRepresentation itemRep)
	{
		(itemRep as TorchItemRep).RepExtinguish();
	}

	public void Ignite(ViewModel vm, ItemRepresentation itemRep, IBasicTorchItem torchItem)
	{
		if (torchItem != null)
		{
			torchItem.Ignite();
		}
		bool flag = vm != null;
		GameObject gameObject = null;
		if (flag)
		{
			Socket.Slot item = vm.socketMap["muzzle"];
			gameObject = item.socket.InstantiateAsChild(this.FirstPersonLightPrefab, false);
			if (torchItem != null)
			{
				torchItem.light = gameObject;
			}
		}
		else if ((torchItem == null || !torchItem.light) && (!itemRep.networkView.isMine || actor.forceThirdPerson))
		{
			if (this.ThirdPersonLightPrefab)
			{
				((BasicTorchItemRep)itemRep)._myLightPrefab = this.ThirdPersonLightPrefab;
			}
			((BasicTorchItemRep)itemRep).RepIgnite();
			if (((BasicTorchItemRep)itemRep)._myLight && torchItem != null)
			{
				torchItem.light = ((BasicTorchItemRep)itemRep)._myLight;
			}
		}
	}

	private sealed class ITEM_TYPE : BasicTorchItem<BasicTorchItemDataBlock>, IBasicTorchItem, IHeldItem, IInventoryItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(BasicTorchItemDataBlock BLOCK) : base(BLOCK)
		{
		}

		// privatescope
		internal bool IBasicTorchItem.get_isLit()
		{
			return base.isLit;
		}

		// privatescope
		internal GameObject IBasicTorchItem.get_light()
		{
			return base.light;
		}

		// privatescope
		internal void IBasicTorchItem.Ignite()
		{
			base.Ignite();
		}

		// privatescope
		internal void IBasicTorchItem.set_isLit(bool value)
		{
			base.isLit = value;
		}

		// privatescope
		internal void IBasicTorchItem.set_light(GameObject value)
		{
			base.light = value;
		}

		// privatescope
		internal void IHeldItem.AddMod(ItemModDataBlock mod)
		{
			base.AddMod(mod);
		}

		// privatescope
		internal int IHeldItem.FindMod(ItemModDataBlock mod)
		{
			return base.FindMod(mod);
		}

		// privatescope
		internal bool IHeldItem.get_canActivate()
		{
			return base.canActivate;
		}

		// privatescope
		internal bool IHeldItem.get_canDeactivate()
		{
			return base.canDeactivate;
		}

		// privatescope
		internal int IHeldItem.get_freeModSlots()
		{
			return base.freeModSlots;
		}

		// privatescope
		internal ItemModDataBlock[] IHeldItem.get_itemMods()
		{
			return base.itemMods;
		}

		// privatescope
		internal ItemRepresentation IHeldItem.get_itemRepresentation()
		{
			return base.itemRepresentation;
		}

		// privatescope
		internal ItemModFlags IHeldItem.get_modFlags()
		{
			return base.modFlags;
		}

		// privatescope
		internal int IHeldItem.get_totalModSlots()
		{
			return base.totalModSlots;
		}

		// privatescope
		internal int IHeldItem.get_usedModSlots()
		{
			return base.usedModSlots;
		}

		// privatescope
		internal ViewModel IHeldItem.get_viewModelInstance()
		{
			return base.viewModelInstance;
		}

		// privatescope
		internal void IHeldItem.OnActivate()
		{
			base.OnActivate();
		}

		// privatescope
		internal void IHeldItem.OnDeactivate()
		{
			base.OnDeactivate();
		}

		// privatescope
		internal void IHeldItem.set_itemRepresentation(ItemRepresentation value)
		{
			base.itemRepresentation = value;
		}

		// privatescope
		internal void IHeldItem.SetTotalModSlotCount(int count)
		{
			base.SetTotalModSlotCount(count);
		}

		// privatescope
		internal void IHeldItem.SetUsedModSlotCount(int count)
		{
			base.SetUsedModSlotCount(count);
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