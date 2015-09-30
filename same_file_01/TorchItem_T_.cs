using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class TorchItem<T> : ThrowableItem<T>
where T : TorchItemDataBlock
{
	public float forceSecondaryTime
	{
		get;
		set;
	}

	public bool isLit
	{
		get;
		protected set;
	}

	public GameObject light
	{
		get;
		set;
	}

	public float realIgniteTime
	{
		get;
		set;
	}

	public float realThrowTime
	{
		get;
		set;
	}

	protected TorchItem(T db) : base(db)
	{
	}

	protected override void DestroyViewModel()
	{
		if (this.light)
		{
			UnityEngine.Object.Destroy(this.light);
			this.light = null;
		}
		base.DestroyViewModel();
	}

	public void Extinguish()
	{
		this.isLit = false;
		if (this.light)
		{
			UnityEngine.Object.Destroy(this.light);
			this.light = null;
		}
	}

	public void Ignite()
	{
		this.isLit = true;
	}

	public bool IsIgnited()
	{
		return this.isLit;
	}

	public override void ItemPreFrame(ref HumanController.InputSample sample)
	{
		base.ItemPreFrame(ref sample);
		if (this.realThrowTime != 0f && Time.time >= this.realThrowTime)
		{
			T t = this.datablock;
			t.DoActualThrow(base.itemRepresentation, this.iface as ITorchItem, base.viewModelInstance);
			this.realThrowTime = 0f;
		}
		if (this.realIgniteTime != 0f && Time.time >= this.realIgniteTime)
		{
			T t1 = this.datablock;
			t1.DoActualIgnite(base.itemRepresentation, this.iface as ITorchItem, base.viewModelInstance);
			this.realIgniteTime = 0f;
		}
		if (this.forceSecondaryTime != 0f && Time.time >= this.forceSecondaryTime)
		{
			this.SecondaryAttack(ref sample);
			this.forceSecondaryTime = 0f;
		}
	}

	public virtual void OnHolstered()
	{
		if (this.isLit)
		{
			this.Extinguish();
			this.realThrowTime = 0f;
			this.realIgniteTime = 0f;
			this.forceSecondaryTime = 0f;
			int num = 1;
			if (base.Consume(ref num))
			{
				base.inventory.RemoveItem(base.slot);
			}
		}
	}

	protected override void OnSetActive(bool isActive)
	{
		base.OnSetActive(isActive);
		if (!isActive)
		{
			this.OnHolstered();
		}
	}
}