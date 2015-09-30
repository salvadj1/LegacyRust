using RustProto;
using System;
using UnityEngine;

public class HumanBodyTakeDamage : ProtectionTakeDamage
{
	private const string CheckLevelMethodName = "CheckLevels";

	private const string DoBleedMethodName = "DoBleed";

	public float _bleedInterval = 10f;

	public float _bleedingLevel;

	public float _bleedingLevelMax = 100f;

	private float lastBleedTime;

	public float checkLevelInterval = 2f;

	private IDBase bleedAttacker;

	private IDBase bleedingID;

	private float _healOverTime;

	private float _lastLevelCheckTime;

	private PlayerInventory _playerInv;

	public HumanBodyTakeDamage()
	{
	}

	public void AddBleedingLevel(float lvl)
	{
		this.SetBleedingLevel(this._bleedingLevel + lvl);
	}

	protected new void Awake()
	{
		base.Awake();
		this.checkLevelInterval = 1f;
		base.InvokeRepeating("CheckLevels", this.checkLevelInterval, this.checkLevelInterval);
		this._playerInv = base.GetComponent<PlayerInventory>();
	}

	public void Bandage(float amountToRestore)
	{
		this.SetBleedingLevel(Mathf.Clamp(this._bleedingLevel - amountToRestore, 0f, this._bleedingLevelMax));
		if (this._bleedingLevel <= 0f)
		{
			base.CancelInvoke("DoBleed");
		}
	}

	public void CheckLevels()
	{
	}

	public void DoBleed()
	{
		LifeStatus lifeStatu;
		if (!base.alive || this._bleedingLevel <= 0f)
		{
			base.CancelInvoke("DoBleed");
		}
		else
		{
			float single = this._bleedingLevel;
			Metabolism component = base.GetComponent<Metabolism>();
			if (component)
			{
				single = this._bleedingLevel * (!component.IsWarm() ? 1f : 0.4f);
			}
			lifeStatu = (!this.bleedAttacker || !this.bleedingID ? TakeDamage.HurtSelf(this.idMain, single, null) : TakeDamage.Hurt(this.bleedAttacker, this.bleedingID, single, null));
			if (lifeStatu != LifeStatus.IsAlive)
			{
				base.CancelInvoke("DoBleed");
			}
			else
			{
				float single1 = 0.2f;
				this.SetBleedingLevel(Mathf.Clamp(this._bleedingLevel - single1, 0f, this._bleedingLevel));
			}
		}
	}

	public virtual void HealOverTime(float amountToHeal)
	{
	}

	protected override LifeStatus Hurt(ref DamageEvent damage)
	{
		LifeStatus lifeStatu = base.Hurt(ref damage);
		if ((int)(damage.damageTypes & (DamageTypeFlags.damage_bullet | DamageTypeFlags.damage_melee | DamageTypeFlags.damage_explosion)) != 0)
		{
			this._healOverTime = 0f;
		}
		if (lifeStatu == LifeStatus.WasKilled)
		{
			base.CancelInvoke("DoBleed");
		}
		else if (lifeStatu == LifeStatus.IsAlive && base.healthLossFraction > 0.2f)
		{
			float single = damage.amount / base.maxHealth;
			if ((int)(damage.damageTypes & (DamageTypeFlags.damage_bullet | DamageTypeFlags.damage_melee)) != 0 && damage.amount > base.maxHealth * 0.05f)
			{
				int num = 0;
				if (single >= 0.25f)
				{
					num = 1;
				}
				else if (single >= 0.15f)
				{
					num = 2;
				}
				else if (single >= 0.05f)
				{
					num = 3;
				}
				if ((UnityEngine.Random.Range(0, num) == 1 ? true : num == 1))
				{
					this.AddBleedingLevel(Mathf.Clamp(damage.amount * 0.15f, 1f, base.maxHealth));
					this.bleedAttacker = damage.attacker.id;
					this.bleedingID = damage.victim.id;
				}
			}
		}
		return lifeStatu;
	}

	public virtual bool IsBleeding()
	{
		return this._bleedingLevel > 0f;
	}

	public override void LoadVitals(Vitals vitals)
	{
		base.LoadVitals(vitals);
		this._bleedingLevel = vitals.BleedSpeed;
		this._healOverTime = vitals.HealSpeed;
	}

	public override void SaveVitals(ref Vitals.Builder vitals)
	{
		base.SaveVitals(ref vitals);
		vitals.SetBleedSpeed(this._bleedingLevel);
		vitals.SetHealSpeed(this._healOverTime);
	}

	public override void ServerFrame()
	{
	}

	public void SetBleedingLevel(float lvl)
	{
		this._bleedingLevel = lvl;
		if (this._bleedingLevel <= 0f)
		{
			base.CancelInvoke("DoBleed");
		}
		else
		{
			base.CancelInvoke("DoBleed");
			base.InvokeRepeating("DoBleed", this._bleedInterval, this._bleedInterval);
		}
		base.SendMessage("BleedingLevelChanged", lvl, SendMessageOptions.DontRequireReceiver);
	}
}