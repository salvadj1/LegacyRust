using System;
using uLink;
using UnityEngine;

public class StrikeGunDataBlock : ShotgunDataBlock
{
	public float[] strikeDurations;

	public AudioClip[] strikeSounds;

	public StrikeGunDataBlock()
	{
	}

	protected override IInventoryItem ConstructItem()
	{
		return new StrikeGunDataBlock.ITEM_TYPE(this);
	}

	public override string GetItemDescription()
	{
		return "Unreliable shotgun type weapon, uses homemade shells";
	}

	public virtual void Local_BeginStrikes(int numStrikes, ViewModel vm, ItemRepresentation itemRep, IStrikeGunItem itemInstance, ref HumanController.InputSample sample)
	{
		string str = string.Concat("strike", numStrikes);
		vm.Play(str, PlayMode.StopAll);
		AudioClip audioClip = this.strikeSounds[numStrikes - 1];
		audioClip.PlayLocal(Camera.main.transform, Vector3.zero, 1f, UnityEngine.Random.Range(0.96f, 1.03f), 2f, 2f, 0);
	}

	public virtual void Local_CancelStrikes(ViewModel vm, ItemRepresentation itemRep, IStrikeGunItem itemInstance, ref HumanController.InputSample sample)
	{
		vm.CrossFade("idle", 0.15f);
	}

	private sealed class ITEM_TYPE : StrikeGunItem<StrikeGunDataBlock>, IBulletWeaponItem, IHeldItem, IInventoryItem, IStrikeGunItem, IWeaponItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(StrikeGunDataBlock BLOCK) : base(BLOCK)
		{
		}

		// privatescope
		internal int IBulletWeaponItem.get_cachedCasings()
		{
			return base.cachedCasings;
		}

		// privatescope
		internal int IBulletWeaponItem.get_clipAmmo()
		{
			return base.clipAmmo;
		}

		// privatescope
		internal MagazineDataBlock IBulletWeaponItem.get_clipType()
		{
			return base.clipType;
		}

		// privatescope
		internal float IBulletWeaponItem.get_nextCasingsTime()
		{
			return base.nextCasingsTime;
		}

		// privatescope
		internal void IBulletWeaponItem.set_cachedCasings(int value)
		{
			base.cachedCasings = value;
		}

		// privatescope
		internal void IBulletWeaponItem.set_clipAmmo(int value)
		{
			base.clipAmmo = value;
		}

		// privatescope
		internal void IBulletWeaponItem.set_nextCasingsTime(float value)
		{
			base.nextCasingsTime = value;
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