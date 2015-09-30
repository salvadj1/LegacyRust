using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class WeaponItem<T> : HeldItem<T>
where T : WeaponDataBlock
{
	protected bool lastFrameAttack;

	private bool wasSprinting;

	public virtual bool canPrimaryAttack
	{
		get
		{
			return Time.time >= this.nextPrimaryAttackTime;
		}
	}

	public virtual bool canReload
	{
		get
		{
			return false;
		}
	}

	public virtual bool canSecondaryAttack
	{
		get
		{
			return Time.time >= this.nextSecondaryAttackTime;
		}
	}

	public virtual bool deployed
	{
		get
		{
			return Time.time > this.deployFinishedTime;
		}
	}

	public float deployFinishedTime
	{
		get;
		set;
	}

	public double lastPrimaryMessageTime
	{
		get;
		set;
	}

	public float nextPrimaryAttackTime
	{
		get;
		set;
	}

	public float nextSecondaryAttackTime
	{
		get;
		set;
	}

	public virtual int possibleReloadCount
	{
		get
		{
			return 999;
		}
	}

	protected WeaponItem(T db) : base(db)
	{
	}

	protected override bool CanAim()
	{
		return (!this.deployed ? false : base.CanAim());
	}

	public override void ItemPostFrame(ref HumanController.InputSample sample)
	{
	}

	public override void ItemPreFrame(ref HumanController.InputSample sample)
	{
		base.ItemPreFrame(ref sample);
		if (sample.is_sprinting)
		{
			float single = Time.time + 0.1f;
			this.nextPrimaryAttackTime = (this.nextPrimaryAttackTime <= single ? single : this.nextPrimaryAttackTime);
			this.wasSprinting = true;
		}
		else if (this.wasSprinting)
		{
			float single1 = Time.time + 0.3f;
			this.nextPrimaryAttackTime = (this.nextPrimaryAttackTime <= single1 ? single1 : this.nextPrimaryAttackTime);
			this.wasSprinting = false;
		}
		if ((T)this.datablock.isSemiAuto)
		{
			if (sample.attack && this.lastFrameAttack)
			{
				sample.attack = false;
			}
			else if (!sample.attack && this.lastFrameAttack)
			{
				this.lastFrameAttack = false;
			}
			else if (sample.attack && !this.lastFrameAttack)
			{
				this.lastFrameAttack = true;
			}
		}
		if (sample.attack && this.canPrimaryAttack)
		{
			this.PrimaryAttack(ref sample);
		}
		if (sample.attack2 && this.canSecondaryAttack)
		{
			this.SecondaryAttack(ref sample);
		}
		if (sample.reload && this.canReload)
		{
			this.Reload(ref sample);
		}
	}

	protected override void OnSetActive(bool isActive)
	{
		float single = (T)this.datablock.deployLength;
		this.deployFinishedTime = Time.time + single;
		if (this.deployFinishedTime > this.nextPrimaryAttackTime)
		{
			float single1 = this.deployFinishedTime;
			this.nextPrimaryAttackTime = single1;
			this.nextSecondaryAttackTime = single1;
		}
		base.OnSetActive(isActive);
	}

	public virtual void PrimaryAttack(ref HumanController.InputSample sample)
	{
		this.nextPrimaryAttackTime = Time.time + 1f;
		Debug.Log("Primary Attack!");
	}

	public virtual void Reload(ref HumanController.InputSample sample)
	{
	}

	public virtual void SecondaryAttack(ref HumanController.InputSample sample)
	{
		this.nextSecondaryAttackTime = Time.time + 1f;
		Debug.Log("Secondary Attack!");
	}

	public bool ValidatePrimaryMessageTime(double timestamp)
	{
		if (timestamp - this.lastPrimaryMessageTime < (double)((T)this.datablock.fireRate * 0.95f))
		{
			return false;
		}
		if (timestamp > NetCull.time + 3)
		{
			return false;
		}
		this.lastPrimaryMessageTime = timestamp;
		return true;
	}
}