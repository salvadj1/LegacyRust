using System;
using uLink;
using UnityEngine;

public class ShotgunDataBlock : BulletWeaponDataBlock
{
	public int numPellets = 6;

	public float xSpread = 8f;

	public float ySpread = 8f;

	public ShotgunDataBlock()
	{
	}

	protected override IInventoryItem ConstructItem()
	{
		return new ShotgunDataBlock.ITEM_TYPE(this);
	}

	public override void DoAction1(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
	{
		GameObject gameObject;
		NetEntityID netEntityID;
		IDRemoteBodyPart dRemoteBodyPart;
		bool flag;
		bool flag1;
		bool flag2;
		BodyPart bodyPart;
		Vector3 vector3;
		Vector3 vector31;
		Transform transforms;
		Component componentInChildren;
		this.DoWeaponEffects(rep.transform.parent, rep.muzzle, false, rep);
		float bulletRange = this.GetBulletRange(rep);
		for (int i = 0; i < this.numPellets; i++)
		{
			this.ReadHitInfo(stream, out gameObject, out flag, out flag1, out bodyPart, out dRemoteBodyPart, out netEntityID, out transforms, out vector3, out vector31, out flag2);
			if (dRemoteBodyPart)
			{
				componentInChildren = dRemoteBodyPart;
			}
			else if (!gameObject)
			{
				componentInChildren = null;
			}
			else
			{
				componentInChildren = gameObject.GetComponentInChildren<CapsuleCollider>();
			}
			Component component = componentInChildren;
			this.MakeTracer(rep.muzzle.position, vector3, bulletRange, component, flag);
		}
	}

	public virtual void DoWeaponEffects(Transform soundTransform, Socket muzzleSocket, bool firstPerson, ItemRepresentation itemRep)
	{
		this.PlayFireSound(soundTransform, firstPerson, itemRep);
		UnityEngine.Object.Destroy(muzzleSocket.InstantiateAsChild((!firstPerson ? this.muzzleFlashWorld : this.muzzleflashVM), false), 1f);
	}

	public virtual void FireSingleBullet(uLink.BitStream sendStream, Ray ray, ItemRepresentation itemRep, out Component hitComponent, out bool allowBlood)
	{
		RaycastHit2 raycastHit2;
		Vector3 point;
		IDMain dMain;
		Component component;
		IDMain dMain1;
		NetEntityID netEntityID = NetEntityID.unassigned;
		bool flag = false;
		bool flag1 = Physics2.Raycast2(ray, out raycastHit2, this.GetBulletRange(itemRep), 406721553);
		if (!flag1)
		{
			dMain = null;
			point = ray.GetPoint(this.GetBulletRange(itemRep));
			hitComponent = null;
		}
		else
		{
			point = raycastHit2.point;
			IDBase dBase = raycastHit2.id;
			if (!raycastHit2.remoteBodyPart)
			{
				component = raycastHit2.collider;
			}
			else
			{
				component = raycastHit2.remoteBodyPart;
			}
			hitComponent = component;
			if (!dBase)
			{
				dMain1 = null;
			}
			else
			{
				dMain1 = dBase.idMain;
			}
			dMain = dMain1;
			if (dMain)
			{
				netEntityID = NetEntityID.Get(dMain);
				if (!netEntityID.isUnassigned)
				{
					TakeDamage takeDamage = dMain.GetComponent<TakeDamage>();
					if (takeDamage)
					{
						flag = true;
						if (takeDamage.ShouldPlayHitNotification())
						{
							this.PlayHitNotification(point, null);
						}
					}
				}
			}
		}
		this.WriteHitInfo(sendStream, ref ray, flag1, ref raycastHit2, flag, netEntityID);
		allowBlood = (!flag1 ? false : flag);
	}

	public override float GetGUIDamage()
	{
		return base.GetGUIDamage() * (float)this.numPellets;
	}

	public override void Local_FireWeapon(ViewModel vm, ItemRepresentation itemRep, IBulletWeaponItem itemInstance, ref HumanController.InputSample sample)
	{
		bool flag;
		Socket socket;
		Character character = itemInstance.character;
		if (character == null)
		{
			return;
		}
		if (itemInstance.clipAmmo <= 0)
		{
			return;
		}
		Ray ray = character.eyesRay;
		int num = 1;
		itemInstance.Consume(ref num);
		uLink.BitStream bitStream = new uLink.BitStream(false);
		float bulletRange = this.GetBulletRange(itemRep);
		for (int i = 0; i < this.numPellets; i++)
		{
			Ray ray1 = ray;
			ray1.direction = (Quaternion.LookRotation(ray.direction) * Quaternion.Euler(UnityEngine.Random.Range(-this.xSpread, this.xSpread), UnityEngine.Random.Range(-this.ySpread, this.ySpread), 0f)) * Vector3.forward;
			Component component = null;
			this.FireSingleBullet(bitStream, ray1, itemRep, out component, out flag);
			this.MakeTracer(ray1.origin, ray1.origin + (ray1.direction * bulletRange), bulletRange, component, flag);
		}
		itemRep.ActionStream(1, uLink.RPCMode.Server, bitStream);
		bool flag1 = vm;
		if (!flag1)
		{
			socket = itemRep.muzzle;
		}
		else
		{
			socket = vm.muzzle;
		}
		this.DoWeaponEffects(character.transform, socket, flag1, itemRep);
		if (flag1)
		{
			vm.PlayFireAnimation();
		}
		float single = 1f;
		if (sample.aim)
		{
			single = single - this.aimingRecoilSubtract;
		}
		else if (sample.crouch)
		{
			single = single - this.crouchRecoilSubtract;
		}
		float single1 = UnityEngine.Random.Range(this.recoilPitchMin, this.recoilPitchMax) * single;
		float single2 = UnityEngine.Random.Range(this.recoilYawMin, this.recoilYawMax) * single;
		RecoilSimulation recoilSimulation = character.recoilSimulation;
		if (recoilSimulation)
		{
			recoilSimulation.AddRecoil(this.recoilDuration, single1, single2);
		}
		HeadBob headBob = CameraMount.current.GetComponent<HeadBob>();
		if (headBob && this.shotBob)
		{
			headBob.AddEffect(this.shotBob);
		}
	}

	public virtual void MakeTracer(Vector3 startPos, Vector3 endPos, float range, Component component, bool allowBlood)
	{
		Vector3 vector3 = endPos - startPos;
		vector3.Normalize();
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.tracerPrefab, startPos, Quaternion.LookRotation(vector3));
		Tracer tracer = gameObject.GetComponent<Tracer>();
		if (tracer)
		{
			tracer.Init(component, 406721553, range, allowBlood);
		}
	}

	protected override void SecureWriteMemberValues(uLink.BitStream stream)
	{
		base.SecureWriteMemberValues(stream);
		stream.Write<float>(this.xSpread, new object[0]);
		stream.Write<int>(this.numPellets, new object[0]);
		stream.Write<float>(this.ySpread, new object[0]);
	}

	private sealed class ITEM_TYPE : BulletWeaponItem<ShotgunDataBlock>, IBulletWeaponItem, IHeldItem, IInventoryItem, IWeaponItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(ShotgunDataBlock BLOCK) : base(BLOCK)
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