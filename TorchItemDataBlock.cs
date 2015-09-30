using System;
using uLink;
using UnityEngine;

public class TorchItemDataBlock : ThrowableItemDataBlock
{
	public GameObject FirstPersonLightPrefab;

	public GameObject ThirdPersonLightPrefab;

	public AudioClip StrikeSound;

	public TorchItemDataBlock()
	{
	}

	protected override IInventoryItem ConstructItem()
	{
		return new TorchItemDataBlock.ITEM_TYPE(this);
	}

	public override void DoAction1(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
	{
		(rep as TorchItemRep).RepExtinguish();
	}

	public override void DoAction2(uLink.BitStream stream, ItemRepresentation itemRep, ref uLink.NetworkMessageInfo info)
	{
		this.Ignite(null, itemRep, null);
	}

	public override void DoAction3(uLink.BitStream stream, ItemRepresentation itemRep, ref uLink.NetworkMessageInfo info)
	{
	}

	public void DoActualIgnite(ItemRepresentation itemRep, IThrowableItem itemInstance, ViewModel vm)
	{
		this.Ignite(vm, itemRep, this.GetTorchInstance(itemInstance));
		itemRep.Action(2, uLink.RPCMode.Server);
	}

	public override void DoActualThrow(ItemRepresentation itemRep, IThrowableItem itemInstance, ViewModel vm)
	{
		Character component = PlayerClient.GetLocalPlayer().controllable.GetComponent<Character>();
		Vector3 vector3 = component.eyesOrigin;
		Vector3 vector31 = component.eyesAngles.forward;
		if (vm)
		{
			vm.PlayQueued("deploy");
		}
		this.GetTorchInstance(itemInstance).Extinguish();
		int num = 1;
		if (itemInstance.Consume(ref num))
		{
			itemInstance.inventory.RemoveItem(itemInstance.slot);
		}
		uLink.BitStream bitStream = new uLink.BitStream(false);
		bitStream.WriteVector3(vector3);
		bitStream.WriteVector3(vector31);
		itemRep.ActionStream(1, uLink.RPCMode.Server, bitStream);
	}

	public ITorchItem GetTorchInstance(IThrowableItem itemInstance)
	{
		return itemInstance as ITorchItem;
	}

	public TorchItemRep GetTorchRep(ItemRepresentation rep)
	{
		return rep as TorchItemRep;
	}

	public void Ignite(ViewModel vm, ItemRepresentation itemRep, ITorchItem torchItem)
	{
		if (torchItem != null)
		{
			torchItem.Ignite();
		}
		bool flag = vm != null;
		GameObject gameObject = null;
		if (flag)
		{
			this.StrikeSound.Play();
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
				((TorchItemRep)itemRep)._myLightPrefab = this.ThirdPersonLightPrefab;
			}
			((TorchItemRep)itemRep).RepIgnite();
			if (((TorchItemRep)itemRep)._myLight && torchItem != null)
			{
				torchItem.light = ((TorchItemRep)itemRep)._myLight;
			}
		}
	}

	public void OnExtinguish(ViewModel vm, ItemRepresentation itemRep, ITorchItem torchItem)
	{
	}

	public override void PrimaryAttack(ViewModel vm, ItemRepresentation itemRep, IThrowableItem itemInstance, ref HumanController.InputSample sample)
	{
		ITorchItem torchInstance = this.GetTorchInstance(itemInstance);
		if (torchInstance.isLit)
		{
			return;
		}
		if (vm)
		{
			vm.Play("ignite");
		}
		torchInstance.realIgniteTime = Time.time + 0.8f;
		torchInstance.nextPrimaryAttackTime = Time.time + 1.5f;
		torchInstance.nextSecondaryAttackTime = Time.time + 1.5f;
	}

	public override void SecondaryAttack(ViewModel vm, ItemRepresentation itemRep, IThrowableItem itemInstance, ref HumanController.InputSample sample)
	{
		ITorchItem torchInstance = this.GetTorchInstance(itemInstance);
		if (!torchInstance.isLit)
		{
			this.PrimaryAttack(vm, itemRep, itemInstance, ref sample);
			torchInstance.forceSecondaryTime = Time.time + 1.51f;
			return;
		}
		if (vm)
		{
			vm.Play("throw");
		}
		torchInstance.realThrowTime = Time.time + 0.5f;
		torchInstance.nextPrimaryAttackTime = Time.time + 1.5f;
		torchInstance.nextSecondaryAttackTime = Time.time + 1.5f;
	}

	private sealed class ITEM_TYPE : TorchItem<TorchItemDataBlock>, IHeldItem, IInventoryItem, IThrowableItem, ITorchItem, IWeaponItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(TorchItemDataBlock BLOCK) : base(BLOCK)
		{
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

		// privatescope
		internal bool IThrowableItem.get_holdingBack()
		{
			return base.holdingBack;
		}

		// privatescope
		internal float IThrowableItem.get_holdingStartTime()
		{
			return base.holdingStartTime;
		}

		// privatescope
		internal float IThrowableItem.get_minReleaseTime()
		{
			return base.minReleaseTime;
		}

		// privatescope
		internal void IThrowableItem.set_holdingBack(bool value)
		{
			base.holdingBack = value;
		}

		// privatescope
		internal void IThrowableItem.set_holdingStartTime(float value)
		{
			base.holdingStartTime = value;
		}

		// privatescope
		internal void IThrowableItem.set_minReleaseTime(float value)
		{
			base.minReleaseTime = value;
		}

		// privatescope
		internal void ITorchItem.Extinguish()
		{
			base.Extinguish();
		}

		// privatescope
		internal float ITorchItem.get_forceSecondaryTime()
		{
			return base.forceSecondaryTime;
		}

		// privatescope
		internal bool ITorchItem.get_isLit()
		{
			return base.isLit;
		}

		// privatescope
		internal GameObject ITorchItem.get_light()
		{
			return base.light;
		}

		// privatescope
		internal float ITorchItem.get_realIgniteTime()
		{
			return base.realIgniteTime;
		}

		// privatescope
		internal float ITorchItem.get_realThrowTime()
		{
			return base.realThrowTime;
		}

		// privatescope
		internal void ITorchItem.Ignite()
		{
			base.Ignite();
		}

		// privatescope
		internal void ITorchItem.set_forceSecondaryTime(float value)
		{
			base.forceSecondaryTime = value;
		}

		// privatescope
		internal void ITorchItem.set_light(GameObject value)
		{
			base.light = value;
		}

		// privatescope
		internal void ITorchItem.set_realIgniteTime(float value)
		{
			base.realIgniteTime = value;
		}

		// privatescope
		internal void ITorchItem.set_realThrowTime(float value)
		{
			base.realThrowTime = value;
		}

		// privatescope
		internal bool IWeaponItem.get_canAim()
		{
			return base.canAim;
		}

		// privatescope
		internal float IWeaponItem.get_deployFinishedTime()
		{
			return base.deployFinishedTime;
		}

		// privatescope
		internal float IWeaponItem.get_nextPrimaryAttackTime()
		{
			return base.nextPrimaryAttackTime;
		}

		// privatescope
		internal float IWeaponItem.get_nextSecondaryAttackTime()
		{
			return base.nextSecondaryAttackTime;
		}

		// privatescope
		internal void IWeaponItem.set_deployFinishedTime(float value)
		{
			base.deployFinishedTime = value;
		}

		// privatescope
		internal void IWeaponItem.set_nextPrimaryAttackTime(float value)
		{
			base.nextPrimaryAttackTime = value;
		}

		// privatescope
		internal void IWeaponItem.set_nextSecondaryAttackTime(float value)
		{
			base.nextSecondaryAttackTime = value;
		}

		// privatescope
		internal bool IWeaponItem.ValidatePrimaryMessageTime(double timestamp)
		{
			return base.ValidatePrimaryMessageTime(timestamp);
		}
	}
}