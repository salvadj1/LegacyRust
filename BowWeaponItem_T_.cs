using Rust;
using System;
using UnityEngine;

public abstract class BowWeaponItem<T> : WeaponItem<T>
where T : BowWeaponDataBlock
{
	private bool _arrowDrawn;

	private bool _tired;

	private float _completeDrawTime;

	private int _currentArrowID;

	public bool arrowDrawn
	{
		get
		{
			return this._arrowDrawn;
		}
		set
		{
			this._arrowDrawn = value;
		}
	}

	public override bool canPrimaryAttack
	{
		get
		{
			return Time.time >= base.nextPrimaryAttackTime;
		}
	}

	public override bool canReload
	{
		get
		{
			return true;
		}
	}

	public float completeDrawTime
	{
		get
		{
			return this._completeDrawTime;
		}
		set
		{
			this._completeDrawTime = value;
		}
	}

	public int currentArrowID
	{
		get
		{
			return this._currentArrowID;
		}
		set
		{
			this._currentArrowID = value;
		}
	}

	public bool tired
	{
		get
		{
			return this._tired;
		}
		set
		{
			this._tired = value;
		}
	}

	protected BowWeaponItem(T db) : base(db)
	{
	}

	public void ArrowReportHit(IDMain hitMain, ArrowMovement arrow)
	{
		T t = this.datablock;
		t.ArrowReportHit(hitMain, arrow, base.itemRepresentation, this.iface as IBowWeaponItem);
	}

	public void ArrowReportMiss(ArrowMovement arrow)
	{
		this.datablock.ArrowReportMiss(arrow, base.itemRepresentation);
	}

	protected override bool CanAim()
	{
		return (this.IsReloading() ? false : base.CanAim());
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

	public void ClearArrowID()
	{
		this.currentArrowID = 0;
	}

	public IInventoryItem FindAmmo()
	{
		return base.inventory.FindItem(this.GetDesiredArrow());
	}

	public int GenerateArrowID()
	{
		return UnityEngine.Random.Range(1, 65535);
	}

	public ItemDataBlock GetDesiredArrow()
	{
		return (T)this.datablock.defaultAmmo;
	}

	public bool IsArrowDrawing()
	{
		return this.completeDrawTime != -1f;
	}

	public bool IsArrowDrawingOrDrawn()
	{
		return (this.IsArrowDrawn() ? true : this.IsArrowDrawing());
	}

	public bool IsArrowDrawn()
	{
		return this.arrowDrawn;
	}

	public virtual bool IsReloading()
	{
		return false;
	}

	public override void ItemPreFrame(ref HumanController.InputSample sample)
	{
		ViewModel viewModel = base.viewModelInstance;
		if (!sample.attack || base.nextPrimaryAttackTime > Time.time)
		{
			if (this.IsArrowDrawn())
			{
				IInventoryItem inventoryItem = this.FindAmmo();
				if (inventoryItem != null)
				{
					int num = 1;
					if (inventoryItem.Consume(ref num))
					{
						base.inventory.RemoveItem(inventoryItem.slot);
					}
					T t = this.datablock;
					t.Local_FireArrow(viewModel, base.itemRepresentation, this.iface as IBowWeaponItem, ref sample);
				}
				else
				{
					Notice.Popup("", "No Arrows!", 4f);
					T t1 = this.datablock;
					t1.Local_CancelArrow(viewModel, base.itemRepresentation, this.iface as IBowWeaponItem, ref sample);
				}
			}
			else if (this.IsArrowDrawingOrDrawn())
			{
				T t2 = this.datablock;
				t2.Local_CancelArrow(viewModel, base.itemRepresentation, this.iface as IBowWeaponItem, ref sample);
			}
			sample.aim = false;
		}
		else
		{
			if (this.IsArrowDrawn())
			{
				float single = Time.time - this.completeDrawTime;
				if (single > 1f)
				{
					T t3 = this.datablock;
					t3.Local_GetTired(viewModel, base.itemRepresentation, this.iface as IBowWeaponItem, ref sample);
					this.tired = true;
				}
				if (single > (T)this.datablock.tooTiredLength)
				{
					T t4 = this.datablock;
					t4.Local_CancelArrow(viewModel, base.itemRepresentation, this.iface as IBowWeaponItem, ref sample);
				}
			}
			else if (!this.IsArrowDrawn() && !this.IsArrowDrawing())
			{
				if (this.FindAmmo() != null)
				{
					T t5 = this.datablock;
					t5.Local_ReadyArrow(viewModel, base.itemRepresentation, this.iface as IBowWeaponItem, ref sample);
				}
				else
				{
					Notice.Popup("", "No Arrows!", 4f);
					this.MakeReadyIn(2f);
				}
			}
			else if (this.completeDrawTime < Time.time)
			{
				this.arrowDrawn = true;
			}
			if (this.IsArrowDrawingOrDrawn() && Time.time - (this.completeDrawTime - 1f) > 0.5f)
			{
				sample.aim = true;
			}
		}
		if (sample.aim)
		{
			sample.yaw = sample.yaw * (T)this.datablock.aimSensitivtyPercent;
			sample.pitch = sample.pitch * (T)this.datablock.aimSensitivtyPercent;
		}
	}

	public void MakeReadyIn(float delay)
	{
		base.nextPrimaryAttackTime = Time.time + delay;
		this.tired = false;
		this.arrowDrawn = false;
		this.completeDrawTime = -1f;
	}

	protected override void OnSetActive(bool isActive)
	{
		if (!isActive)
		{
			this.MakeReadyIn(2f);
		}
		base.OnSetActive(isActive);
	}
}