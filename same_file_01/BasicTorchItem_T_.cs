using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class BasicTorchItem<T> : HeldItem<T>
where T : BasicTorchItemDataBlock
{
	private float lastTickTime;

	private float consumeAmount;

	public bool isLit
	{
		get;
		set;
	}

	public GameObject light
	{
		get;
		set;
	}

	protected BasicTorchItem(T db) : base(db)
	{
	}

	public virtual void Extinguish()
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

	protected override void OnSetActive(bool isActive)
	{
		if (!isActive)
		{
			this.lastTickTime = -1f;
			T t = this.datablock;
			t.DoActualExtinguish(base.itemRepresentation, this.iface as IBasicTorchItem, base.viewModelInstance);
			base.OnSetActive(isActive);
		}
		else
		{
			this.lastTickTime = Time.time;
			this.consumeAmount = 0f;
			base.OnSetActive(isActive);
			T t1 = this.datablock;
			t1.DoActualIgnite(base.itemRepresentation, this.iface as IBasicTorchItem, base.viewModelInstance);
		}
	}
}