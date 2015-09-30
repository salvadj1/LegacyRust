using Facepunch;
using System;
using uLink;
using UnityEngine;

public class BowWeaponDataBlock : WeaponDataBlock
{
	public AudioClip drawArrowSound;

	public AudioClip fireArrowSound;

	public AudioClip cancelArrowSound;

	public float arrowSpeed;

	public float tooTiredLength = 8f;

	public float drawLength = 2f;

	public ItemDataBlock defaultAmmo;

	public GameObject arrowPrefab;

	public string arrowPickupString;

	private static HUDHitIndicator _hitIndicator;

	public BowWeaponDataBlock()
	{
	}

	public void ArrowReportHit(IDMain hitMain, ArrowMovement arrow, ItemRepresentation itemRepresentation, IBowWeaponItem itemInstance)
	{
		if (!hitMain)
		{
			return;
		}
		TakeDamage component = hitMain.GetComponent<TakeDamage>();
		if (!component)
		{
			return;
		}
		uLink.BitStream bitStream = new uLink.BitStream(false);
		bitStream.Write<NetEntityID>(NetEntityID.Get(hitMain), new object[0]);
		bitStream.Write<Vector3>(hitMain.transform.position, new object[0]);
		itemRepresentation.ActionStream(2, uLink.RPCMode.Server, bitStream);
		Character character = itemInstance.character;
		if (component && component.ShouldPlayHitNotification())
		{
			this.PlayHitNotification(arrow.transform.position, character);
		}
	}

	public void ArrowReportMiss(ArrowMovement arrow, ItemRepresentation itemRepresentation)
	{
		uLink.BitStream bitStream = new uLink.BitStream(false);
		bitStream.Write<Vector3>(arrow.transform.position, new object[0]);
		itemRepresentation.ActionStream(3, uLink.RPCMode.Server, bitStream);
	}

	protected override IInventoryItem ConstructItem()
	{
		return new BowWeaponDataBlock.ITEM_TYPE(this);
	}

	public override void DoAction1(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
	{
		Vector3 vector3 = stream.ReadVector3();
		this.FireArrow(vector3, stream.ReadQuaternion(), rep, null);
	}

	public virtual void DoWeaponEffects(Transform soundTransform, Vector3 startPos, Vector3 endPos, Socket muzzleSocket, bool firstPerson, Component hitComponent, bool allowBlood, ItemRepresentation itemRep)
	{
	}

	public void FireArrow(Vector3 pos, Quaternion ang, ItemRepresentation itemRep, IBowWeaponItem itemInstance)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.arrowPrefab, pos, ang);
		gameObject.GetComponent<ArrowMovement>().Init(this.arrowSpeed, itemRep, itemInstance, false);
		this.fireArrowSound.Play(pos, 1f, 2f, 10f);
	}

	public virtual float GetGUIDamage()
	{
		return 999f;
	}

	public override string GetItemDescription()
	{
		return "This is a weapon. Drag it to your belt (right side of screen) and press the corresponding number key to use it.";
	}

	public override byte GetMaxEligableSlots()
	{
		return (byte)0;
	}

	public override void InstallData(IInventoryItem item)
	{
		base.InstallData(item);
	}

	public virtual void Local_CancelArrow(ViewModel vm, ItemRepresentation itemRep, IBowWeaponItem itemInstance, ref HumanController.InputSample sample)
	{
		if (vm)
		{
			vm.CrossFade("cancelarrow", 0.15f);
		}
		this.MakeReadyIn(itemInstance, this.fireRate * 3f);
		this.cancelArrowSound.PlayLocal(Camera.main.transform, Vector3.zero, 1f, 1f, 3f, 20f, 0);
	}

	public virtual void Local_FireArrow(ViewModel vm, ItemRepresentation itemRep, IBowWeaponItem itemInstance, ref HumanController.InputSample sample)
	{
		if (vm)
		{
			vm.Play("fire_1", PlayMode.StopSameLayer);
		}
		this.MakeReadyIn(itemInstance, this.fireRate);
		Character character = itemInstance.character;
		if (character == null)
		{
			return;
		}
		Ray ray = character.eyesRay;
		Vector3 vector3 = character.eyesOrigin;
		this.FireArrow(vector3, character.eyesRotation, itemRep, itemInstance);
		uLink.BitStream bitStream = new uLink.BitStream(false);
		bitStream.WriteVector3(vector3);
		bitStream.WriteQuaternion(character.eyesRotation);
		itemRep.ActionStream(1, uLink.RPCMode.Server, bitStream);
	}

	public virtual void Local_GetTired(ViewModel vm, ItemRepresentation itemRep, IBowWeaponItem itemInstance, ref HumanController.InputSample sample)
	{
		if (itemInstance.tired)
		{
			return;
		}
		if (vm)
		{
			vm.CrossFade("tiredloop", 5f);
		}
	}

	public virtual void Local_ReadyArrow(ViewModel vm, ItemRepresentation itemRep, IBowWeaponItem itemInstance, ref HumanController.InputSample sample)
	{
		if (vm)
		{
			vm.Play("drawarrow", PlayMode.StopSameLayer);
		}
		itemInstance.completeDrawTime = Time.time + this.drawLength;
		this.drawArrowSound.PlayLocal(Camera.main.transform, Vector3.zero, 1f, 1f, 3f, 20f, 0);
	}

	public virtual void MakeReadyIn(IBowWeaponItem itemInstance, float delay)
	{
		itemInstance.MakeReadyIn(delay);
	}

	protected virtual new void PlayHitNotification(Vector3 point, Character shooterOrNull)
	{
		if (WeaponDataBlock._hitNotify || Bundling.Load<AudioClip>("content/shared/sfx/hitnotification", out WeaponDataBlock._hitNotify))
		{
			WeaponDataBlock._hitNotify.PlayLocal(Camera.main.transform, Vector3.zero, 1f, 1);
		}
		if (BowWeaponDataBlock._hitIndicator || Bundling.Load<HUDHitIndicator>("content/hud/HUDHitIndicator", out BowWeaponDataBlock._hitIndicator))
		{
			HUDHitIndicator.CreateIndicator(point, true, BowWeaponDataBlock._hitIndicator);
		}
	}

	public override void PopulateInfoWindow(ItemToolTip infoWindow, IInventoryItem tipItem)
	{
	}

	protected override void SecureWriteMemberValues(uLink.BitStream stream)
	{
		base.SecureWriteMemberValues(stream);
	}

	private sealed class ITEM_TYPE : BowWeaponItem<BowWeaponDataBlock>, IBowWeaponItem, IHeldItem, IInventoryItem, IWeaponItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(BowWeaponDataBlock BLOCK) : base(BLOCK)
		{
		}

		// privatescope
		internal void IBowWeaponItem.ArrowReportHit(IDMain hitMain, ArrowMovement arrow)
		{
			base.ArrowReportHit(hitMain, arrow);
		}

		// privatescope
		internal void IBowWeaponItem.ArrowReportMiss(ArrowMovement arrow)
		{
			base.ArrowReportMiss(arrow);
		}

		// privatescope
		internal IInventoryItem IBowWeaponItem.FindAmmo()
		{
			return base.FindAmmo();
		}

		// privatescope
		internal bool IBowWeaponItem.get_arrowDrawn()
		{
			return base.arrowDrawn;
		}

		// privatescope
		internal float IBowWeaponItem.get_completeDrawTime()
		{
			return base.completeDrawTime;
		}

		// privatescope
		internal int IBowWeaponItem.get_currentArrowID()
		{
			return base.currentArrowID;
		}

		// privatescope
		internal bool IBowWeaponItem.get_tired()
		{
			return base.tired;
		}

		// privatescope
		internal void IBowWeaponItem.MakeReadyIn(float delay)
		{
			base.MakeReadyIn(delay);
		}

		// privatescope
		internal void IBowWeaponItem.set_arrowDrawn(bool value)
		{
			base.arrowDrawn = value;
		}

		// privatescope
		internal void IBowWeaponItem.set_completeDrawTime(float value)
		{
			base.completeDrawTime = value;
		}

		// privatescope
		internal void IBowWeaponItem.set_currentArrowID(int value)
		{
			base.currentArrowID = value;
		}

		// privatescope
		internal void IBowWeaponItem.set_tired(bool value)
		{
			base.tired = value;
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