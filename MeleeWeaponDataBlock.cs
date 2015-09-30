using Facepunch.Movement;
using Rust;
using System;
using uLink;
using UnityEngine;

public class MeleeWeaponDataBlock : WeaponDataBlock
{
	public const int hitMask = 406721553;

	public float range = 2f;

	public GameObject impactEffect;

	public GameObject impactEffectFlesh;

	public GameObject impactEffectWood;

	public AudioClip impactSoundWood;

	public AudioClip swingSound;

	public float swingSoundDelay = 0.2f;

	public AudioClip impactSoundGeneric;

	public AudioClip[] impactSoundFlesh;

	public float midSwingDelay = 0.35f;

	public float gatherPerHitAmount = 0.25f;

	public bool gathersResources;

	public float caloriesPerSwing = 5f;

	public float worldSwingAnimationSpeed = 1f;

	[SerializeField]
	protected string _swingingMovementAnimationGroupName;

	public float[] efficiencies;

	public MeleeWeaponDataBlock()
	{
	}

	protected override IInventoryItem ConstructItem()
	{
		return new MeleeWeaponDataBlock.ITEM_TYPE(this);
	}

	public override void DoAction1(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
	{
		GameObject gameObject;
		NetEntityID netEntityID;
		if (!stream.ReadBoolean())
		{
			netEntityID = NetEntityID.unassigned;
			gameObject = null;
		}
		else
		{
			netEntityID = stream.Read<NetEntityID>(new object[0]);
			if (netEntityID.isUnassigned)
			{
				gameObject = null;
			}
			else
			{
				gameObject = netEntityID.gameObject;
				if (!gameObject)
				{
					netEntityID = NetEntityID.unassigned;
				}
			}
		}
		Vector3 vector3 = stream.ReadVector3();
		stream.ReadBoolean();
		this.EndSwingWorldAnimations(rep);
		if (gameObject)
		{
			Vector3 vector31 = rep.transform.position - vector3;
			Quaternion quaternion = Quaternion.LookRotation(vector31.normalized);
			this.DoMeleeEffects(rep.transform.position, vector3, quaternion, gameObject);
		}
	}

	public override void DoAction2(uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
	{
		NetEntityID netEntityID = stream.Read<NetEntityID>(new object[0]);
		if (!netEntityID.isUnassigned && !netEntityID.idBase)
		{
			return;
		}
	}

	public override void DoAction3(uLink.BitStream stream, ItemRepresentation itemRep, ref uLink.NetworkMessageInfo info)
	{
		this.StartSwingWorldAnimations(itemRep);
	}

	public virtual void DoMeleeEffects(Vector3 fromPos, Vector3 pos, Quaternion rot, GameObject hitObj)
	{
		if (!hitObj.CompareTag("Tree Collider"))
		{
			SurfaceInfo.DoImpact(hitObj, SurfaceInfoObject.ImpactType.Melee, pos, rot);
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(this.impactEffectWood, pos, rot) as GameObject;
		UnityEngine.Object.Destroy(gameObject, 1.5f);
		this.impactSoundWood.Play(pos, 1f, 2f, 10f);
	}

	private void EndSwingWorldAnimations(ItemRepresentation itemRep)
	{
		if (!string.IsNullOrEmpty(this._swingingMovementAnimationGroupName) && this._swingingMovementAnimationGroupName != this.animationGroupName)
		{
			itemRep.OverrideAnimationGroupName(null);
		}
	}

	public override float GetDamage()
	{
		return UnityEngine.Random.Range(this.damageMin, this.damageMax);
	}

	public virtual float GetRange()
	{
		return this.range;
	}

	public virtual void Local_FireWeapon(ViewModel vm, ItemRepresentation itemRep, IMeleeWeaponItem itemInstance, ref HumanController.InputSample sample)
	{
		this.StartSwingWorldAnimations(itemRep);
		if (vm)
		{
			vm.PlayFireAnimation();
		}
		itemInstance.QueueSwingSound(Time.time + this.swingSoundDelay);
		itemInstance.QueueMidSwing(Time.time + this.midSwingDelay);
		if (itemRep.networkViewIsMine)
		{
			itemRep.Action(3, uLink.RPCMode.Server);
		}
	}

	public virtual void Local_MidSwing(ViewModel vm, ItemRepresentation itemRep, IMeleeWeaponItem itemInstance, ref HumanController.InputSample sample)
	{
		BodyPart bodyPart;
		IDBase dBase;
		IDMain dMain;
		Character character = itemInstance.character;
		if (character == null)
		{
			return;
		}
		Ray ray = character.eyesRay;
		bool flag = false;
		Collider collider = null;
		Vector3 vector3 = Vector3.zero;
		Vector3 vector31 = Vector3.up;
		NetEntityID netEntityID = NetEntityID.unassigned;
		bool flag1 = false;
		flag = this.Physics2SphereCast(ray, 0.3f, this.GetRange(), 406721553, out vector3, out vector31, out collider, out bodyPart);
		bool flag2 = false;
		TakeDamage component = null;
		if (flag)
		{
			TransformHelpers.GetIDBaseFromCollider(collider, out dBase);
			if (!dBase)
			{
				dMain = null;
			}
			else
			{
				dMain = dBase.idMain;
			}
			IDMain dMain1 = dMain;
			if (dMain1)
			{
				netEntityID = NetEntityID.Get(dMain1);
				flag1 = !netEntityID.isUnassigned;
				component = dMain1.GetComponent<TakeDamage>();
				if (component && component.ShouldPlayHitNotification())
				{
					this.PlayHitNotification(vector3, character);
				}
			}
			flag2 = collider.gameObject.CompareTag("Tree Collider");
			if (flag2)
			{
				WoodBlockerTemp blockerForPoint = WoodBlockerTemp.GetBlockerForPoint(vector3);
				if (blockerForPoint.HasWood())
				{
					blockerForPoint.ConsumeWood(this.efficiencies[2]);
				}
				else
				{
					flag2 = false;
					Notice.Popup("", "There's no wood left here", 2f);
				}
			}
			this.DoMeleeEffects(ray.origin, vector3, Quaternion.LookRotation(vector31), collider.gameObject);
			if (vm && (component || flag2))
			{
				vm.CrossFade("pull_out", 0.05f, PlayMode.StopSameLayer, 1.1f);
			}
		}
		uLink.BitStream bitStream = new uLink.BitStream(false);
		if (!flag1)
		{
			bitStream.WriteBoolean(false);
			bitStream.WriteVector3(vector3);
		}
		else
		{
			bitStream.WriteBoolean(flag1);
			bitStream.Write<NetEntityID>(netEntityID, new object[0]);
			bitStream.WriteVector3(vector3);
		}
		bitStream.WriteBoolean(flag2);
		itemRep.ActionStream(1, uLink.RPCMode.Server, bitStream);
		this.EndSwingWorldAnimations(itemRep);
	}

	public virtual void Local_SecondaryFire(ViewModel vm, ItemRepresentation itemRep, IMeleeWeaponItem itemInstance, ref HumanController.InputSample sample)
	{
		RaycastHit raycastHit;
		Character character = itemInstance.character;
		if (character == null)
		{
			return;
		}
		if (Physics.SphereCast(character.eyesRay, 0.3f, out raycastHit, this.GetRange(), 406721553))
		{
			IDBase component = raycastHit.collider.gameObject.GetComponent<IDBase>();
			if (component)
			{
				NetEntityID netEntityID = NetEntityID.Get(component);
				if (component.GetLocal<RepairReceiver>() != null && netEntityID != NetEntityID.unassigned)
				{
					if (vm)
					{
						vm.PlayFireAnimation();
					}
					itemInstance.QueueSwingSound(Time.time + this.swingSoundDelay);
					itemRep.Action<NetEntityID>(2, uLink.RPCMode.Server, netEntityID);
				}
			}
		}
	}

	public bool Physics2SphereCast(Ray ray, float radius, float range, int hitMask, out Vector3 point, out Vector3 normal, out Collider hitCollider, out BodyPart part)
	{
		RaycastHit2 raycastHit2;
		RaycastHit raycastHit;
		RaycastHit raycastHit1;
		RaycastHit raycastHit3 = new RaycastHit();
		bool flag = false;
		bool flag1 = false;
		if (Physics.SphereCast(ray, radius, out raycastHit1, range - radius, hitMask & -131073))
		{
			flag1 = true;
			raycastHit3 = raycastHit1;
			if (Physics.Raycast(ray, out raycastHit, range, hitMask & -131073))
			{
				flag = true;
				if (raycastHit.distance < raycastHit1.distance)
				{
					raycastHit3 = raycastHit;
				}
			}
		}
		bool flag2 = (flag1 ? true : flag);
		if (Physics2.Raycast2(ray, out raycastHit2, range, hitMask) && (!flag2 || raycastHit2.distance < raycastHit3.distance))
		{
			point = raycastHit2.point;
			normal = raycastHit2.normal;
			hitCollider = raycastHit2.collider;
			part = raycastHit2.bodyPart;
			return true;
		}
		if (!flag2)
		{
			Collider[] colliderArray = Physics.OverlapSphere(ray.origin, 0.3f, 131072);
			if ((int)colliderArray.Length > 0)
			{
				point = ray.origin + (ray.direction * 0.5f);
				normal = ray.direction * -1f;
				hitCollider = colliderArray[0];
				part = BodyPart.Undefined;
				return true;
			}
		}
		if (flag2)
		{
			point = raycastHit3.point;
			normal = raycastHit3.normal;
			hitCollider = raycastHit3.collider;
			part = BodyPart.Undefined;
			return true;
		}
		point = ray.origin + (ray.direction * range);
		normal = ray.direction * -1f;
		hitCollider = null;
		part = BodyPart.Undefined;
		return false;
	}

	private void StartSwingWorldAnimations(ItemRepresentation itemRep)
	{
		if (!string.IsNullOrEmpty(this._swingingMovementAnimationGroupName) && this._swingingMovementAnimationGroupName != this.animationGroupName)
		{
			itemRep.OverrideAnimationGroupName(this._swingingMovementAnimationGroupName);
		}
		itemRep.PlayWorldAnimation(GroupEvent.Attack, this.worldSwingAnimationSpeed);
	}

	public virtual void SwingSound()
	{
		this.swingSound.PlayLocal(Camera.main.transform, Vector3.zero, 1f, UnityEngine.Random.Range(0.85f, 1f), 3f, 20f, 0);
	}

	private sealed class ITEM_TYPE : MeleeWeaponItem<MeleeWeaponDataBlock>, IHeldItem, IInventoryItem, IMeleeWeaponItem, IWeaponItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(MeleeWeaponDataBlock BLOCK) : base(BLOCK)
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
		internal float IMeleeWeaponItem.get_queuedSwingAttackTime()
		{
			return base.queuedSwingAttackTime;
		}

		// privatescope
		internal float IMeleeWeaponItem.get_queuedSwingSoundTime()
		{
			return base.queuedSwingSoundTime;
		}

		// privatescope
		internal void IMeleeWeaponItem.set_queuedSwingAttackTime(float value)
		{
			base.queuedSwingAttackTime = value;
		}

		// privatescope
		internal void IMeleeWeaponItem.set_queuedSwingSoundTime(float value)
		{
			base.queuedSwingSoundTime = value;
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