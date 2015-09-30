using Facepunch.Intersect;
using System;
using uLink;
using UnityEngine;

public class BulletWeaponDataBlock : WeaponDataBlock
{
	public const int hitMask = 406721553;

	private const byte kDidNotHitNetworkView = 255;

	private const byte kDidHitNetworkViewWithoutBodyPart = 254;

	public AmmoItemDataBlock ammoType;

	public int maxClipAmmo;

	public GameObject tracerPrefab;

	public GameObject muzzleflashVM;

	public GameObject muzzleFlashWorld;

	public AudioClip fireSound;

	public AudioClip fireSound_Far;

	public AudioClip reloadSound;

	public AudioClip headshotSound;

	public AudioClip fireSound_SilencedFar;

	public AudioClip fireSound_Silenced;

	public AudioClip dryFireSound;

	public float fireSoundRange = 300f;

	public float bulletRange = 200f;

	public float recoilPitchMin;

	public float recoilPitchMax;

	public float recoilYawMin;

	public float recoilYawMax;

	public float recoilDuration;

	public float aimingRecoilSubtract = 0.5f;

	public float crouchRecoilSubtract = 0.2f;

	public float reloadDuration = 1.5f;

	public int maxEligableSlots = 5;

	public bool NoAimingAfterShot;

	public float aimSway;

	public float aimSwaySpeed = 1f;

	public BobEffect shotBob;

	private static bool weaponRecoil;

	private static bool headRecoil;

	static BulletWeaponDataBlock()
	{
		BulletWeaponDataBlock.weaponRecoil = true;
		BulletWeaponDataBlock.headRecoil = true;
	}

	public BulletWeaponDataBlock()
	{
	}

	public void Awake()
	{
	}

	protected override IInventoryItem ConstructItem()
	{
		return new BulletWeaponDataBlock.ITEM_TYPE(this);
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
		bool flag3;
		this.ReadHitInfo(stream, out gameObject, out flag, out flag1, out bodyPart, out dRemoteBodyPart, out netEntityID, out transforms, out vector3, out vector31, out flag2);
		if (flag2)
		{
			this.headshotSound.Play(gameObject.transform.position, 1f, 4f, 30f);
		}
		Transform transforms1 = rep.transform.parent;
		Vector3 vector32 = rep.muzzle.position;
		Vector3 vector33 = vector3;
		Socket.LocalSpace localSpace = rep.muzzle;
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
		if (!flag)
		{
			flag3 = false;
		}
		else
		{
			flag3 = (!dRemoteBodyPart || bodyPart.IsDefined() ? true : gameObject.GetComponent<TakeDamage>() != null);
		}
		this.DoWeaponEffects(transforms1, vector32, vector33, localSpace, false, componentInChildren, flag3, rep);
	}

	public override void DoAction2(uLink.BitStream stream, ItemRepresentation itemRep, ref uLink.NetworkMessageInfo info)
	{
	}

	public virtual void DoWeaponEffects(Transform soundTransform, Vector3 startPos, Vector3 endPos, Socket muzzleSocket, bool firstPerson, Component hitComponent, bool allowBlood, ItemRepresentation itemRep)
	{
		Vector3 vector3 = endPos - startPos;
		vector3.Normalize();
		bool flag = this.IsSilenced(itemRep);
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.tracerPrefab, startPos, Quaternion.LookRotation(vector3));
		Tracer component = gameObject.GetComponent<Tracer>();
		if (component)
		{
			component.Init(hitComponent, 406721553, this.GetBulletRange(itemRep), allowBlood);
		}
		if (flag)
		{
			component.startScale = Vector3.zero;
		}
		this.PlayFireSound(soundTransform, firstPerson, itemRep);
		if (!flag)
		{
			UnityEngine.Object.Destroy(muzzleSocket.InstantiateAsChild((!firstPerson ? this.muzzleFlashWorld : this.muzzleflashVM), false), 1f);
		}
	}

	public override InventoryItem.MenuItemResult ExecuteMenuOption(InventoryItem.MenuItem option, IInventoryItem item)
	{
		if (option == InventoryItem.MenuItem.Unload)
		{
			return InventoryItem.MenuItemResult.DoneOnServer;
		}
		return base.ExecuteMenuOption(option, item);
	}

	public virtual float GetBulletRange(ItemRepresentation itemRep)
	{
		if (!itemRep)
		{
			return this.bulletRange;
		}
		return this.bulletRange * (!this.IsSilenced(itemRep) ? 1f : 0.75f);
	}

	public virtual float GetDamage(ItemRepresentation itemRep)
	{
		return UnityEngine.Random.Range(this.damageMin, this.damageMax) * (!this.IsSilenced(itemRep) ? 1f : 0.8f);
	}

	public virtual AudioClip GetFarFireSound(ItemRepresentation itemRep)
	{
		if (this.IsSilenced(itemRep))
		{
			return this.fireSound_SilencedFar;
		}
		return this.fireSound_Far;
	}

	public virtual float GetFarFireSoundRangeMax()
	{
		return this.fireSoundRange;
	}

	public virtual float GetFarFireSoundRangeMin()
	{
		return this.GetFireSoundRangeMax() * 0.5f;
	}

	public virtual AudioClip GetFireSound(ItemRepresentation itemRep)
	{
		if (this.IsSilenced(itemRep))
		{
			return this.fireSound_Silenced;
		}
		return this.fireSound;
	}

	public virtual float GetFireSoundRangeMax()
	{
		return 60f;
	}

	public virtual float GetFireSoundRangeMin()
	{
		return 2f;
	}

	public virtual float GetGUIDamage()
	{
		return this.damageMin + (this.damageMax - this.damageMin) * 0.5f;
	}

	public override string GetItemDescription()
	{
		return "This is a weapon. Drag it to your belt (right side of screen) and press the corresponding number key to use it.";
	}

	public override byte GetMaxEligableSlots()
	{
		return (byte)this.maxEligableSlots;
	}

	public override void InstallData(IInventoryItem item)
	{
		base.InstallData(item);
		IBulletWeaponItem bulletWeaponItem = item as IBulletWeaponItem;
		this._maxUses = this.maxClipAmmo;
		bulletWeaponItem.clipAmmo = this.maxClipAmmo;
	}

	public virtual bool IsSilenced(ItemRepresentation itemRep)
	{
		return (itemRep.modFlags & ItemModFlags.Audio) == ItemModFlags.Audio;
	}

	public virtual void Local_DryFire(ViewModel vm, ItemRepresentation itemRep)
	{
		this.dryFireSound.PlayLocal(itemRep.transform, Vector3.zero, 1f, 0);
	}

	public virtual void Local_FireWeapon(ViewModel vm, ItemRepresentation itemRep, IBulletWeaponItem itemInstance, ref HumanController.InputSample sample)
	{
		Component component;
		Vector3 point;
		RaycastHit2 raycastHit2;
		IDMain dMain;
		Socket item;
		bool flag;
		bool flag1;
		Component component1;
		IDMain dMain1;
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
		NetEntityID netEntityID = NetEntityID.unassigned;
		bool flag2 = false;
		int num = 1;
		itemInstance.Consume(ref num);
		bool flag3 = Physics2.Raycast2(ray, out raycastHit2, this.GetBulletRange(itemRep), 406721553);
		TakeDamage takeDamage = null;
		if (!flag3)
		{
			dMain = null;
			point = ray.GetPoint(1000f);
			component = null;
		}
		else
		{
			point = raycastHit2.point;
			IDBase dBase = raycastHit2.id;
			if (!raycastHit2.remoteBodyPart)
			{
				component1 = raycastHit2.collider;
			}
			else
			{
				component1 = raycastHit2.remoteBodyPart;
			}
			component = component1;
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
					flag2 = true;
					takeDamage = dMain.GetComponent<TakeDamage>();
					if (takeDamage && takeDamage.ShouldPlayHitNotification())
					{
						this.PlayHitNotification(point, character);
					}
					bool flag4 = false;
					if (raycastHit2.remoteBodyPart)
					{
						BodyPart bodyPart = raycastHit2.remoteBodyPart.bodyPart;
						switch (bodyPart)
						{
							case BodyPart.Brain:
							case BodyPart.L_Eye:
							case BodyPart.R_Eye:
							{
							Label0:
								flag4 = true;
								break;
							}
							default:
							{
								switch (bodyPart)
								{
									case BodyPart.Head:
									case BodyPart.Jaw:
									{
										goto Label0;
									}
									case BodyPart.Scalp:
									case BodyPart.Nostrils:
									{
										flag4 = false;
										break;
									}
									default:
									{
										goto case BodyPart.Nostrils;
									}
								}
								break;
							}
						}
					}
					if (flag4)
					{
						this.headshotSound.Play();
					}
				}
			}
		}
		if (!flag3)
		{
			flag1 = false;
		}
		else
		{
			flag1 = (!raycastHit2.isHitboxHit || raycastHit2.bodyPart.IsDefined() ? true : takeDamage != null);
		}
		bool flag5 = flag1;
		if (!vm)
		{
			item = itemRep.muzzle;
			flag = false;
		}
		else
		{
			item = vm.socketMap["muzzle"].socket;
			flag = true;
		}
		Vector3 vector3 = item.position;
		this.DoWeaponEffects(character.transform, vector3, point, item, flag, component, flag5, itemRep);
		if (flag)
		{
			vm.PlayFireAnimation();
		}
		float single = 1f;
		if ((!sample.aim ? false : sample.crouch))
		{
			single = single - (this.aimingRecoilSubtract + this.crouchRecoilSubtract * 0.5f);
		}
		else if (sample.aim)
		{
			single = single - this.aimingRecoilSubtract;
		}
		else if (sample.crouch)
		{
			single = single - this.crouchRecoilSubtract;
		}
		single = Mathf.Clamp01(single);
		float single1 = UnityEngine.Random.Range(this.recoilPitchMin, this.recoilPitchMax) * single;
		float single2 = UnityEngine.Random.Range(this.recoilYawMin, this.recoilYawMax) * single;
		if (BulletWeaponDataBlock.weaponRecoil && character.recoilSimulation)
		{
			character.recoilSimulation.AddRecoil(this.recoilDuration, single1, single2);
		}
		HeadBob headBob = CameraMount.current.GetComponent<HeadBob>();
		if (headBob && this.shotBob && BulletWeaponDataBlock.headRecoil)
		{
			headBob.AddEffect(this.shotBob);
		}
		uLink.BitStream bitStream = new uLink.BitStream(false);
		this.WriteHitInfo(bitStream, ref ray, flag3, ref raycastHit2, flag2, netEntityID);
		itemRep.ActionStream(1, uLink.RPCMode.Server, bitStream);
	}

	public virtual void Local_Reload(ViewModel vm, ItemRepresentation itemRep, IBulletWeaponItem itemInstance, ref HumanController.InputSample sample)
	{
		if (vm)
		{
			vm.PlayReloadAnimation();
		}
		this.reloadSound.PlayLocal(itemRep.transform, Vector3.zero, 1f, 0);
		itemRep.Action(3, uLink.RPCMode.Server);
	}

	public virtual void PlayFireSound(Transform soundTransform, bool firstPerson, ItemRepresentation itemRep)
	{
		bool flag = this.IsSilenced(itemRep);
		AudioClip fireSound = this.GetFireSound(itemRep);
		float single = Vector3.Distance(soundTransform.position, Camera.main.transform.position);
		float farFireSoundRangeMin = this.GetFarFireSoundRangeMin();
		fireSound.PlayLocal(soundTransform, Vector3.zero, 1f, UnityEngine.Random.Range(0.92f, 1.08f), this.GetFireSoundRangeMin(), this.GetFireSoundRangeMax() * (!flag ? 1f : 1.5f), (!firstPerson ? 20 : 0));
		if (!firstPerson && single > farFireSoundRangeMin && !flag)
		{
			AudioClip farFireSound = this.GetFarFireSound(itemRep);
			if (farFireSound)
			{
				farFireSound.PlayLocal(soundTransform, Vector3.zero, 1f, UnityEngine.Random.Range(0.9f, 1.1f), 0f, this.GetFarFireSoundRangeMax(), 50);
			}
		}
	}

	public override void PopulateInfoWindow(ItemToolTip infoWindow, IInventoryItem tipItem)
	{
		infoWindow.AddItemTitle(this, tipItem, 0f);
		infoWindow.AddConditionInfo(tipItem);
		infoWindow.AddSectionTitle("Weapon Stats", 20f);
		float single = this.recoilPitchMax + this.recoilYawMax;
		float single1 = 60f;
		float single2 = 1f / this.fireRate;
		if (!this.isSemiAuto)
		{
			infoWindow.AddProgressStat("Fire Rate", single2, 12f, 15f);
		}
		else
		{
			infoWindow.AddBasicLabel("Semi Automatic Weapon", 15f);
		}
		infoWindow.AddProgressStat("Damage", this.GetGUIDamage(), 100f, 15f);
		infoWindow.AddProgressStat("Recoil", single, single1, 15f);
		infoWindow.AddProgressStat("Range", this.GetBulletRange(null), 200f, 15f);
		infoWindow.AddItemDescription(this, 15f);
		infoWindow.FinishPopulating();
	}

	protected virtual void ReadHitInfo(uLink.BitStream stream, out GameObject hitObj, out bool hitNetworkObj, out bool hitBodyPart, out BodyPart bodyPart, out IDRemoteBodyPart remoteBodyPart, out NetEntityID hitViewID, out Transform fromTransform, out Vector3 endPos, out Vector3 offset, out bool isHeadshot)
	{
		HitBoxSystem hitBoxSystem;
		bool flag;
		byte num = stream.ReadByte();
		if (num >= 255)
		{
			hitNetworkObj = false;
			hitBodyPart = false;
			bodyPart = BodyPart.Undefined;
		}
		else
		{
			hitNetworkObj = true;
			if (num >= 120)
			{
				hitBodyPart = false;
				bodyPart = BodyPart.Undefined;
			}
			else
			{
				hitBodyPart = true;
				bodyPart = (BodyPart)num;
			}
		}
		if (!hitNetworkObj)
		{
			hitViewID = NetEntityID.unassigned;
			hitObj = null;
			remoteBodyPart = null;
		}
		else
		{
			hitViewID = stream.Read<NetEntityID>(new object[0]);
			if (hitViewID.isUnassigned)
			{
				hitObj = null;
				remoteBodyPart = null;
			}
			else
			{
				hitObj = hitViewID.gameObject;
				if (!hitObj)
				{
					remoteBodyPart = null;
				}
				else
				{
					IDBase dBase = IDBase.Get(hitObj);
					if (!dBase)
					{
						remoteBodyPart = null;
					}
					else
					{
						IDMain dMain = dBase.idMain;
						if (!dMain)
						{
							remoteBodyPart = null;
						}
						else
						{
							hitBoxSystem = (!(dMain is Character) ? dMain.GetRemote<HitBoxSystem>() : ((Character)dMain).hitBoxSystem);
							if (!hitBoxSystem)
							{
								remoteBodyPart = null;
							}
							else
							{
								hitBoxSystem.bodyParts.TryGetValue((BodyPart)((int)bodyPart), out remoteBodyPart);
							}
						}
					}
				}
			}
		}
		endPos = stream.ReadVector3();
		offset = Vector3.zero;
		if (remoteBodyPart)
		{
			flag = false;
			fromTransform = remoteBodyPart.transform;
			BodyPart bodyPart1 = (BodyPart)((int)bodyPart);
			switch (bodyPart1)
			{
				case BodyPart.Brain:
				case BodyPart.L_Eye:
				case BodyPart.R_Eye:
				{
				Label0:
					isHeadshot = true;
					break;
				}
				default:
				{
					switch (bodyPart1)
					{
						case BodyPart.Head:
						case BodyPart.Jaw:
						{
							goto Label0;
						}
						case BodyPart.Scalp:
						case BodyPart.Nostrils:
						{
							isHeadshot = false;
							break;
						}
						default:
						{
							goto case BodyPart.Nostrils;
						}
					}
					break;
				}
			}
		}
		else if (!hitObj)
		{
			fromTransform = null;
			flag = false;
			isHeadshot = false;
		}
		else
		{
			fromTransform = hitObj.transform;
			flag = true;
			isHeadshot = false;
		}
		if (fromTransform)
		{
			offset = endPos;
			endPos = fromTransform.TransformPoint(endPos);
		}
	}

	public override int RetreiveMenuOptions(IInventoryItem item, InventoryItem.MenuItem[] results, int offset)
	{
		offset = base.RetreiveMenuOptions(item, results, offset);
		if (item.isInLocalInventory)
		{
			int num = offset;
			offset = num + 1;
			results[num] = InventoryItem.MenuItem.Unload;
		}
		return offset;
	}

	protected override void SecureWriteMemberValues(uLink.BitStream stream)
	{
		base.SecureWriteMemberValues(stream);
		stream.Write<int>(406721553, new object[0]);
		stream.Write<float>(this.crouchRecoilSubtract, new object[0]);
		stream.Write<int>(this.maxClipAmmo, new object[0]);
		stream.Write<float>(this.recoilPitchMin, new object[0]);
		stream.Write<float>(this.recoilPitchMax, new object[0]);
		stream.Write<float>(this.recoilYawMin, new object[0]);
		stream.Write<float>(this.recoilYawMax, new object[0]);
		stream.Write<float>(this.recoilDuration, new object[0]);
		stream.Write<float>(this.aimingRecoilSubtract, new object[0]);
		stream.Write<int>((!this.ammoType ? 0 : this.ammoType.uniqueID), new object[0]);
	}

	protected void WriteHitInfo(uLink.BitStream sendStream, ref Ray ray, bool didHit, ref RaycastHit2 hit)
	{
		bool flag;
		NetEntityID netEntityID;
		if (!didHit)
		{
			netEntityID = NetEntityID.unassigned;
			flag = false;
		}
		else
		{
			IDBase dBase = hit.id;
			if (!dBase || !dBase.idMain)
			{
				flag = false;
				netEntityID = NetEntityID.unassigned;
			}
			else
			{
				netEntityID = NetEntityID.Get(dBase.idMain);
				flag = !netEntityID.isUnassigned;
			}
		}
		this.WriteHitInfo(sendStream, ref ray, didHit, ref hit, flag, netEntityID);
	}

	protected virtual void WriteHitInfo(uLink.BitStream sendStream, ref Ray ray, bool didHit, ref RaycastHit2 hit, bool hitNetworkView, NetEntityID hitView)
	{
		Vector3 point;
		Transform transforms;
		if (!didHit)
		{
			sendStream.WriteByte(255);
			point = ray.GetPoint(1000f);
		}
		else if (!hitNetworkView)
		{
			sendStream.WriteByte(255);
			point = hit.point;
		}
		else
		{
			IDRemoteBodyPart dRemoteBodyPart = hit.remoteBodyPart;
			if (!dRemoteBodyPart)
			{
				sendStream.WriteByte(254);
				transforms = hitView.transform;
			}
			else
			{
				sendStream.WriteByte((byte)dRemoteBodyPart.bodyPart);
				transforms = dRemoteBodyPart.transform;
			}
			sendStream.Write<NetEntityID>(hitView, new object[0]);
			point = transforms.InverseTransformPoint(hit.point);
		}
		sendStream.WriteVector3(point);
	}

	private sealed class ITEM_TYPE : BulletWeaponItem<BulletWeaponDataBlock>, IBulletWeaponItem, IHeldItem, IInventoryItem, IWeaponItem
	{
		ItemDataBlock IInventoryItem.datablock
		{
			get
			{
				return this.datablock;
			}
		}

		public ITEM_TYPE(BulletWeaponDataBlock BLOCK) : base(BLOCK)
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