using System;
using UnityEngine;

public abstract class MeleeWeaponItem<T> : WeaponItem<T>
where T : MeleeWeaponDataBlock
{
	private float _queuedSwingAttackTime;

	private float _queuedSwingSoundTime;

	public float queuedSwingAttackTime
	{
		get
		{
			return this._queuedSwingAttackTime;
		}
		set
		{
			this._queuedSwingAttackTime = value;
		}
	}

	public float queuedSwingSoundTime
	{
		get
		{
			return this._queuedSwingSoundTime;
		}
		set
		{
			this._queuedSwingSoundTime = value;
		}
	}

	protected MeleeWeaponItem(T db) : base(db)
	{
	}

	protected override bool CanSetActivate(bool wantsTrue)
	{
		bool flag;
		if (!base.CanSetActivate(wantsTrue))
		{
			flag = false;
		}
		else
		{
			flag = (wantsTrue ? true : base.nextPrimaryAttackTime <= Time.time);
		}
		return flag;
	}

	public override void ItemPreFrame(ref HumanController.InputSample sample)
	{
		base.ItemPreFrame(ref sample);
		if (this.queuedSwingAttackTime > 0f && this.queuedSwingAttackTime < Time.time)
		{
			T t = this.datablock;
			t.Local_MidSwing(base.viewModelInstance, base.itemRepresentation, this.iface as IMeleeWeaponItem, ref sample);
			this.queuedSwingAttackTime = -1f;
		}
		if (this.queuedSwingSoundTime > 0f && this.queuedSwingSoundTime < Time.time)
		{
			this.datablock.SwingSound();
			this.queuedSwingSoundTime = -1f;
		}
	}

	protected override void OnSetActive(bool isActive)
	{
		this.queuedSwingSoundTime = -1f;
		this.queuedSwingAttackTime = -1f;
		base.OnSetActive(isActive);
	}

	public override void PrimaryAttack(ref HumanController.InputSample sample)
	{
		float single = (T)this.datablock.fireRate;
		Metabolism local = base.inventory.GetLocal<Metabolism>();
		if (local && local.GetCalorieLevel() <= 0f)
		{
			single = (T)this.datablock.fireRate * 2f;
		}
		float single1 = Time.time + single;
		base.nextSecondaryAttackTime = single1;
		base.nextPrimaryAttackTime = single1;
		T t = this.datablock;
		t.Local_FireWeapon(base.viewModelInstance, base.itemRepresentation, this.iface as IMeleeWeaponItem, ref sample);
	}

	public virtual void QueueMidSwing(float time)
	{
		this.queuedSwingAttackTime = time;
	}

	public virtual void QueueSwingSound(float time)
	{
		this.queuedSwingSoundTime = time;
	}

	public override void SecondaryAttack(ref HumanController.InputSample sample)
	{
		float single = Time.time + (T)this.datablock.fireRate;
		base.nextPrimaryAttackTime = single;
		base.nextSecondaryAttackTime = single;
	}
}