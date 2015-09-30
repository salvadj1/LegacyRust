using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class BulletWeaponItem<T> : WeaponItem<T>
where T : BulletWeaponDataBlock
{
	private float reloadStartTime;

	private int cachedNumReloads;

	public float nextAimTime;

	public int cachedCasings
	{
		get;
		set;
	}

	public override bool canPrimaryAttack
	{
		get
		{
			return (!base.canPrimaryAttack ? false : this.clipAmmo > 0);
		}
	}

	public override bool canReload
	{
		get
		{
			if (base.nextPrimaryAttackTime <= Time.time && this.clipAmmo < (T)this.datablock.maxClipAmmo)
			{
				IInventoryItem inventoryItem = base.inventory.FindItem((T)this.datablock.ammoType);
				if (inventoryItem != null && inventoryItem.uses > 0)
				{
					return true;
				}
			}
			return false;
		}
	}

	public int clipAmmo
	{
		get
		{
			return base.uses;
		}
		set
		{
			base.SetUses(value);
		}
	}

	public MagazineDataBlock clipType
	{
		get;
		protected set;
	}

	public float nextCasingsTime
	{
		get;
		set;
	}

	public override int possibleReloadCount
	{
		get
		{
			return this.cachedNumReloads;
		}
	}

	protected BulletWeaponItem(T db) : base(db)
	{
	}

	public virtual void ActualReload()
	{
		this.ActualReload_COD();
	}

	public virtual void ActualReload_COD()
	{
		this.reloadStartTime = Time.time;
		base.nextPrimaryAttackTime = Time.time + (T)this.datablock.reloadDuration;
		Inventory inventory = base.inventory;
		int num = base.uses;
		int num1 = (T)this.datablock.maxClipAmmo;
		if (num == num1)
		{
			return;
		}
		int num2 = num1 - num;
		int num3 = 0;
		while (num < num1)
		{
			IInventoryItem inventoryItem = inventory.FindItem((T)this.datablock.ammoType);
			if (inventoryItem != null)
			{
				int num4 = num2;
				if (inventoryItem.Consume(ref num2))
				{
					inventory.RemoveItem(inventoryItem.slot);
				}
				num3 = num3 + (num4 - num2);
				if (num2 != 0)
				{
					continue;
				}
				break;
			}
			else
			{
				break;
			}
		}
		if (num3 > 0)
		{
			base.AddUses(num3);
		}
		inventory.Refresh();
	}

	public virtual void CacheReloads()
	{
		this.cachedNumReloads = 0;
	}

	protected override bool CanAim()
	{
		return (this.IsReloading() || !base.CanAim() ? false : Time.time > this.nextAimTime);
	}

	protected override bool CanSetActivate(bool value)
	{
		bool flag;
		if (!base.CanSetActivate(value))
		{
			flag = false;
		}
		else
		{
			flag = (value ? true : base.nextPrimaryAttackTime <= Time.time);
		}
		return flag;
	}

	public virtual bool IsReloading()
	{
		return (this.reloadStartTime == -1f ? false : Time.time < this.reloadStartTime + (T)this.datablock.reloadDuration);
	}

	public override void ItemPreFrame(ref HumanController.InputSample sample)
	{
		if (sample.attack && this.clipAmmo == 0 && base.nextPrimaryAttackTime <= Time.time)
		{
			this.datablock.Local_DryFire(base.viewModelInstance, base.itemRepresentation);
			base.nextPrimaryAttackTime = Time.time + 1f;
			sample.attack = false;
		}
		base.ItemPreFrame(ref sample);
		if (sample.aim && (T)this.datablock.aimSway > 0f)
		{
			float single = Time.time * (T)this.datablock.aimSwaySpeed;
			float single1 = (T)this.datablock.aimSway * (!sample.crouch ? 1f : 0.6f);
			sample.yaw = sample.yaw + (Mathf.PerlinNoise(single, single) - 0.5f) * single1 * Time.deltaTime;
			sample.pitch = sample.pitch + (Mathf.PerlinNoise(single + 0.1f, single + 0.2f) - 0.5f) * single1 * Time.deltaTime;
		}
	}

	public override void PrimaryAttack(ref HumanController.InputSample sample)
	{
		base.nextPrimaryAttackTime = Time.time + (T)this.datablock.fireRate;
		if ((T)this.datablock.NoAimingAfterShot)
		{
			this.nextAimTime = Time.time + (T)this.datablock.fireRate;
		}
		ViewModel viewModel = base.viewModelInstance;
		if (actor.forceThirdPerson)
		{
			viewModel = null;
		}
		T t = this.datablock;
		t.Local_FireWeapon(viewModel, base.itemRepresentation, this.iface as IBulletWeaponItem, ref sample);
	}

	public override void Reload(ref HumanController.InputSample sample)
	{
		T t = this.datablock;
		t.Local_Reload(base.viewModelInstance, base.itemRepresentation, this.iface as IBulletWeaponItem, ref sample);
		this.ActualReload();
		base.inventory.Refresh();
	}
}