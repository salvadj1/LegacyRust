using System;
using uLink;
using UnityEngine;

public abstract class BandageItem<T> : HeldItem<T>
where T : BandageDataBlock
{
	private float _bandageStartTime;

	private bool _lastFramePrimary;

	private float _lastBandageTime;

	public float bandageStartTime
	{
		get
		{
			return this._bandageStartTime;
		}
		set
		{
			this._bandageStartTime = value;
		}
	}

	public float lastBandageTime
	{
		get
		{
			return this._lastBandageTime;
		}
		set
		{
			this._lastBandageTime = value;
		}
	}

	public bool lastFramePrimary
	{
		get
		{
			return this._lastFramePrimary;
		}
		set
		{
			this._lastFramePrimary = value;
		}
	}

	protected BandageItem(T db) : base(db)
	{
	}

	public virtual bool CanBandage()
	{
		HumanBodyTakeDamage component = base.inventory.gameObject.GetComponent<HumanBodyTakeDamage>();
		return (component.IsBleeding() || component.healthLossFraction > 0f && this.datablock.DoesGiveBlood() ? Time.time - this.lastBandageTime > 1.5f : false);
	}

	public void CancelBandage()
	{
		RPOS.SetActionProgress(false, null, 0f);
		this.bandageStartTime = -1f;
	}

	public void FinishBandage()
	{
		this.bandageStartTime = -1f;
		RPOS.SetActionProgress(false, null, 0f);
		int num = 1;
		if (base.Consume(ref num))
		{
			base.inventory.RemoveItem(base.slot);
		}
		base.itemRepresentation.Action(3, uLink.RPCMode.Server);
	}

	public override void ItemPreFrame(ref HumanController.InputSample sample)
	{
		base.ItemPreFrame(ref sample);
		if (!sample.attack || !this.CanBandage())
		{
			if (this.lastFramePrimary)
			{
				this.CancelBandage();
			}
			this.lastFramePrimary = false;
		}
		else
		{
			this.Primary(ref sample);
		}
	}

	public virtual void Primary(ref HumanController.InputSample sample)
	{
		this.lastFramePrimary = true;
		sample.crouch = true;
		sample.walk = 0f;
		sample.strafe = 0f;
		sample.jump = false;
		sample.sprint = false;
		if (this.bandageStartTime == -1f)
		{
			this.StartBandage();
		}
		float single = Time.time - this.bandageStartTime;
		float single1 = Mathf.Clamp(single / (T)this.datablock.bandageDuration, 0f, 1f);
		string empty = string.Empty;
		bool flag = this.datablock.DoesGiveBlood();
		bool flag1 = this.datablock.DoesBandage();
		if (flag1 && !flag)
		{
			empty = "Bandaging...";
		}
		else if (flag1 && flag)
		{
			empty = "Bandage + Transfusion...";
		}
		else if (!flag1 && flag)
		{
			empty = "Transfusing...";
		}
		RPOS.SetActionProgress(true, empty, single1);
		if (single1 >= 1f)
		{
			this.FinishBandage();
		}
	}

	public void StartBandage()
	{
		this.bandageStartTime = Time.time;
	}
}