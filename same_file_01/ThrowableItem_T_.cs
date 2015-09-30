using System;
using UnityEngine;

public abstract class ThrowableItem<T> : WeaponItem<T>
where T : ThrowableItemDataBlock
{
	private float _holdingStartTime;

	private bool _holdingBack;

	private float _minReleaseTime;

	public virtual float heldThrowStrength
	{
		get
		{
			float single = Time.time - this.holdingStartTime;
			return Mathf.Clamp(single * (T)this.datablock.throwStrengthPerSec, (T)this.datablock.throwStrengthMin, (T)this.datablock.throwStrengthMin);
		}
	}

	public bool holdingBack
	{
		get
		{
			return this._holdingBack;
		}
		set
		{
			this._holdingBack = value;
		}
	}

	public float holdingStartTime
	{
		get
		{
			return this._holdingStartTime;
		}
		set
		{
			this._holdingStartTime = value;
		}
	}

	public float minReleaseTime
	{
		get
		{
			return this._minReleaseTime;
		}
		set
		{
			this._minReleaseTime = value;
		}
	}

	protected ThrowableItem(T db) : base(db)
	{
	}

	public virtual void BeginHoldingBack()
	{
		this.holdingStartTime = Time.time;
		this.holdingBack = true;
	}

	public virtual void EndHoldingBack()
	{
		this.holdingBack = false;
		this.holdingStartTime = 0f;
	}

	public override void ItemPreFrame(ref HumanController.InputSample sample)
	{
		base.ItemPreFrame(ref sample);
		if (this.holdingBack && !sample.attack && Time.time - this.holdingStartTime > this.minReleaseTime)
		{
			T t = this.datablock;
			t.AttackReleased(base.viewModelInstance, base.itemRepresentation, this.iface as IThrowableItem, ref sample);
			this.holdingBack = false;
		}
	}

	protected override void OnSetActive(bool isActive)
	{
		this.EndHoldingBack();
		base.OnSetActive(isActive);
	}

	public override void PrimaryAttack(ref HumanController.InputSample sample)
	{
		base.nextPrimaryAttackTime = Time.time + (T)this.datablock.fireRate;
		T t = this.datablock;
		t.PrimaryAttack(base.viewModelInstance, base.itemRepresentation, this.iface as IThrowableItem, ref sample);
	}

	public override void SecondaryAttack(ref HumanController.InputSample sample)
	{
		base.nextSecondaryAttackTime = Time.time + (T)this.datablock.fireRateSecondary;
		T t = this.datablock;
		t.SecondaryAttack(base.viewModelInstance, base.itemRepresentation, this.iface as IThrowableItem, ref sample);
	}
}