using System;
using UnityEngine;

public abstract class StrikeGunItem<T> : BulletWeaponItem<T>
where T : StrikeGunDataBlock
{
	public bool beganFiring;

	public float actualFireTime;

	protected StrikeGunItem(T db) : base(db)
	{
	}

	public virtual void CancelAttack(ref HumanController.InputSample sample)
	{
		if (this.beganFiring)
		{
			ViewModel viewModel = base.viewModelInstance;
			T t = this.datablock;
			t.Local_CancelStrikes(base.viewModelInstance, base.itemRepresentation, this.iface as IStrikeGunItem, ref sample);
			base.nextPrimaryAttackTime = Time.time + 1f;
			this.ResetFiring();
		}
	}

	public override void ItemPreFrame(ref HumanController.InputSample sample)
	{
		base.ItemPreFrame(ref sample);
		if (!sample.attack && this.beganFiring)
		{
			this.CancelAttack(ref sample);
			sample.attack = false;
		}
		if (sample.attack && base.clipAmmo == 0 && this.canReload)
		{
			this.Reload(ref sample);
		}
		if (this.beganFiring && sample.attack && Time.time > this.actualFireTime)
		{
			base.PrimaryAttack(ref sample);
			this.ResetFiring();
		}
	}

	protected override void OnSetActive(bool isActive)
	{
		this.ResetFiring();
		base.OnSetActive(isActive);
	}

	public override void PrimaryAttack(ref HumanController.InputSample sample)
	{
		if (!this.beganFiring)
		{
			int num = UnityEngine.Random.Range(1, (int)(T)this.datablock.strikeDurations.Length + 1);
			num = Mathf.Clamp(num, 1, (int)(T)this.datablock.strikeDurations.Length);
			this.actualFireTime = Time.time + (T)this.datablock.strikeDurations[num - 1];
			T t = this.datablock;
			t.Local_BeginStrikes(num, base.viewModelInstance, base.itemRepresentation, this.iface as IStrikeGunItem, ref sample);
			this.beganFiring = true;
		}
	}

	public void ResetFiring()
	{
		this.actualFireTime = 0f;
		this.beganFiring = false;
	}
}