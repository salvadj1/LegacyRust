using System;
using uLink;
using UnityEngine;

public class HandGrenadeDataBlock : ThrowableItemDataBlock
{
	public AudioClip pullPinSound;

	public HandGrenadeDataBlock()
	{
	}

	public override void AttackReleased(ViewModel vm, ItemRepresentation itemRep, IThrowableItem itemInstance, ref HumanController.InputSample sample)
	{
		Debug.Log("Attack released!!!");
		vm.Play("throw");
		vm.PlayQueued("deploy");
		this.GetHandGrenadeItemInstance(itemInstance).nextPrimaryAttackTime = Time.time + 1f;
		this.GetHandGrenadeItemInstance(itemInstance).nextSecondaryAttackTime = Time.time + 1f;
		Character component = PlayerClient.GetLocalPlayer().controllable.GetComponent<Character>();
		Vector3 vector3 = component.eyesOrigin;
		Vector3 vector31 = component.eyesAngles.forward;
		uLink.BitStream bitStream = new uLink.BitStream(false);
		bitStream.WriteVector3(vector3);
		bitStream.WriteVector3(vector31 * this.GetHandGrenadeItemInstance(itemInstance).heldThrowStrength);
		Debug.Log(string.Concat("Throw strength is : ", this.GetHandGrenadeItemInstance(itemInstance).heldThrowStrength));
		this.GetHandGrenadeItemInstance(itemInstance).EndHoldingBack();
		itemRep.ActionStream(1, uLink.RPCMode.Server, bitStream);
	}

	protected override IInventoryItem ConstructItem()
	{
		return new HandGrenadeDataBlock.ITEM_TYPE(this);
	}

	public override void DoAction1(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
	{
	}

	public IHandGrenadeItem GetHandGrenadeItemInstance(IInventoryItem itemInstance)
	{
		return itemInstance as IHandGrenadeItem;
	}

	public override void PrimaryAttack(ViewModel vm, ItemRepresentation itemRep, IThrowableItem itemInstance, ref HumanController.InputSample sample)
	{
		base.PrimaryAttack(vm, itemRep, itemInstance, ref sample);
		vm.Play("pull_pin");
		this.pullPinSound.Play();
		this.GetHandGrenadeItemInstance(itemInstance).nextPrimaryAttackTime = Time.time + 1000f;
		this.GetHandGrenadeItemInstance(itemInstance).nextSecondaryAttackTime = Time.time + 1000f;
	}

	protected override GameObject ThrowItem(ItemRepresentation rep, IThrowableItem item, Vector3 origin, Vector3 forward, uLink.NetworkViewID owner)
	{
		forward.Normalize();
		Vector3 vector3 = forward * 20f;
		Vector3 vector31 = origin + (forward * 0.5f);
		return this.SpawnThrowItem(owner, this.throwObjectPrefab, vector31, Quaternion.LookRotation(Vector3.up), vector3);
	}

	private sealed class ITEM_TYPE : HandGrenadeItem<HandGrenadeDataBlock>, IHandGrenadeItem, IHeldItem, IInventoryItem, IThrowableItem, IWeaponItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(HandGrenadeDataBlock BLOCK) : base(BLOCK)
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