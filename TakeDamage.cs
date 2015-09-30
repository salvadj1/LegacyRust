using RustProto;
using System;
using UnityEngine;

[AddComponentMenu("ID/Local/Take Damage")]
public class TakeDamage : IDLocal, IServerSaveable
{
	public const string DamageMessage = "OnHurt";

	public const string KillMessage = "OnKilled";

	public const string RepairMessage = "OnRepair";

	public const SendMessageOptions DamageMessageOptions = SendMessageOptions.DontRequireReceiver;

	public const SendMessageOptions RepairMessageOptions = SendMessageOptions.DontRequireReceiver;

	public const float kMinimumSetHealthValueWhenAlive = 0.001f;

	protected float _lastDamageTime;

	private bool takenodamage;

	[SerializeField]
	private float _maxHealth = 100f;

	private float _health;

	public bool playsHitNotification;

	public bool sendMessageWhenAlive = true;

	public bool sendMessageWhenKilled = true;

	public bool sendMessageWhenDead = true;

	public bool sendMessageRepair = true;

	public bool alive
	{
		get
		{
			return this.health > 0f;
		}
	}

	public bool dead
	{
		get
		{
			return this.health <= 0f;
		}
	}

	public float health
	{
		get
		{
			return this._health;
		}
		set
		{
			this._health = value;
		}
	}

	public float healthFraction
	{
		get
		{
			return this._health / this._maxHealth;
		}
	}

	public float healthLoss
	{
		get
		{
			return this._maxHealth - this.health;
		}
	}

	public float healthLossFraction
	{
		get
		{
			return 1f - this._health / this._maxHealth;
		}
	}

	public float maxHealth
	{
		get
		{
			return this._maxHealth;
		}
		set
		{
			this._maxHealth = value;
		}
	}

	public TakeDamage()
	{
	}

	protected virtual void ApplyDamageTypeList(ref DamageEvent damage, DamageTypeList damageTypes)
	{
		for (int i = 0; i < 6; i++)
		{
			if (!Mathf.Approximately(damageTypes[i], 0f))
			{
				damage.damageTypes = (DamageTypeFlags)((int)damage.damageTypes | 1 << (i & 31));
				damage.amount = damage.amount + damageTypes[i];
			}
		}
	}

	protected void Awake()
	{
		float single = TakeDamage.HealthAliveValueClamp(this._maxHealth);
		float single1 = single;
		this._health = single;
		this._maxHealth = single1;
	}

	public static string DamageIndexToString(int index)
	{
		return TakeDamage.DamageIndexToString((DamageTypeIndex)index);
	}

	public static string DamageIndexToString(DamageTypeIndex index)
	{
		string str;
		switch (index)
		{
			case DamageTypeIndex.damage_bullet:
			{
				str = "Bullet";
				break;
			}
			case DamageTypeIndex.damage_melee:
			{
				str = "Melee";
				break;
			}
			case DamageTypeIndex.damage_explosion:
			{
				str = "Explosion";
				break;
			}
			case DamageTypeIndex.damage_radiation:
			{
				str = "Radiation";
				break;
			}
			case DamageTypeIndex.damage_cold:
			{
				str = "Cold";
				break;
			}
			default:
			{
				str = "Generic";
				break;
			}
		}
		return str;
	}

	public RepairEvent Heal(IDBase healer, float amount)
	{
		RepairEvent repairEvent;
		this.Heal(healer, amount, out repairEvent);
		return repairEvent;
	}

	public RepairStatus Heal(IDBase healer, float amount, out RepairEvent repair)
	{
		repair = new RepairEvent();
		repair.doner = healer;
		repair.receiver = this;
		repair.givenAmount = amount;
		if (amount <= 0f)
		{
			repair.status = RepairStatus.Failed;
			repair.usedAmount = 0f;
			return RepairStatus.Failed;
		}
		if (this.dead)
		{
			repair.status = RepairStatus.FailedUnreparable;
			repair.usedAmount = 0f;
		}
		else if (this._health == this._maxHealth)
		{
			repair.status = RepairStatus.FailedFull;
			repair.usedAmount = 0f;
		}
		else if (this._health <= this._maxHealth - amount)
		{
			TakeDamage takeDamage = this;
			takeDamage._health = takeDamage._health + amount;
			repair.usedAmount = repair.givenAmount;
			if (this._health != this._maxHealth)
			{
				repair.status = RepairStatus.Applied;
			}
			else
			{
				repair.status = RepairStatus.AppliedFull;
			}
		}
		else
		{
			this._health = this._maxHealth;
			repair.usedAmount = this._maxHealth - this._health;
			repair.status = RepairStatus.AppliedPartial;
		}
		if (this.sendMessageRepair)
		{
			base.SendMessage("OnRepair", repair, SendMessageOptions.DontRequireReceiver);
		}
		return repair.status;
	}

	private static float HealthAliveValueClamp(float newHealth)
	{
		return (newHealth >= 0.001f ? newHealth : 0.001f);
	}

	public static LifeStatus Hurt(IDBase attacker, IDBase victim, TakeDamage.Quantity damageQuantity, out DamageEvent damage, object extraData = null)
	{
		return TakeDamage.HurtShared(attacker, victim, damageQuantity, out damage, extraData);
	}

	public static LifeStatus Hurt(IDBase attacker, IDBase victim, TakeDamage.Quantity damageQuantity, object extraData = null)
	{
		return TakeDamage.HurtShared(attacker, victim, damageQuantity, extraData);
	}

	protected virtual LifeStatus Hurt(ref DamageEvent damage)
	{
		if (this.dead)
		{
			damage.status = LifeStatus.IsDead;
		}
		else if (this.health <= damage.amount)
		{
			damage.status = LifeStatus.WasKilled;
		}
		else
		{
			damage.status = LifeStatus.IsAlive;
		}
		this.ProcessDamageEvent(ref damage);
		if (this.ShouldRelayDamageEvent(ref damage))
		{
			base.SendMessage("OnHurt", damage, SendMessageOptions.DontRequireReceiver);
		}
		if (damage.status == LifeStatus.WasKilled)
		{
			base.SendMessage("OnKilled", damage, SendMessageOptions.DontRequireReceiver);
		}
		return damage.status;
	}

	public static LifeStatus HurtSelf(IDBase victim, TakeDamage.Quantity damageQuantity, out DamageEvent damage, object extraData = null)
	{
		return TakeDamage.HurtShared(victim, victim, damageQuantity, out damage, extraData);
	}

	public static LifeStatus HurtSelf(IDBase victim, TakeDamage.Quantity damageQuantity, object extraData = null)
	{
		return TakeDamage.HurtShared(victim, victim, damageQuantity, extraData);
	}

	private static LifeStatus HurtShared(IDBase attacker, IDBase victim, TakeDamage.Quantity damageQuantity, out DamageEvent damage, object extraData = null)
	{
		damage = new DamageEvent();
		TakeDamage takeDamage;
		if (victim)
		{
			IDMain dMain = victim.idMain;
			if (dMain)
			{
				takeDamage = (!(dMain is Character) ? dMain.GetLocal<TakeDamage>() : ((Character)dMain).takeDamage);
				if (takeDamage && !takeDamage.takenodamage)
				{
					takeDamage.MarkDamageTime();
					damage.victim.id = victim;
					damage.attacker.id = attacker;
					damage.amount = damageQuantity.@value;
					damage.sender = takeDamage;
					damage.status = (!takeDamage.dead ? LifeStatus.IsAlive : LifeStatus.IsDead);
					damage.damageTypes = (DamageTypeFlags)0;
					damage.extraData = extraData;
					if ((int)damageQuantity.Unit == -1)
					{
						takeDamage.ApplyDamageTypeList(ref damage, damageQuantity.list);
					}
					takeDamage.Hurt(ref damage);
					return damage.status;
				}
			}
		}
		damage.victim.id = null;
		damage.attacker.id = null;
		damage.amount = 0f;
		damage.sender = null;
		damage.damageTypes = (DamageTypeFlags)0;
		damage.status = LifeStatus.Failed;
		damage.extraData = extraData;
		return LifeStatus.Failed;
	}

	private static LifeStatus HurtShared(IDBase attacker, IDBase victim, TakeDamage.Quantity damageQuantity, object extraData = null)
	{
		DamageEvent damageEvent;
		return TakeDamage.HurtShared(attacker, victim, damageQuantity, out damageEvent, extraData);
	}

	public static LifeStatus Kill(IDBase attacker, IDBase victim, out DamageEvent damage, object extraData = null)
	{
		return TakeDamage.HurtShared(attacker, victim, TakeDamage.Quantity.AllHealth, out damage, extraData);
	}

	public static LifeStatus Kill(IDBase attacker, IDBase victim, object extraData = null)
	{
		return TakeDamage.HurtShared(attacker, victim, TakeDamage.Quantity.AllHealth, extraData);
	}

	public static LifeStatus KillSelf(IDBase victim, object extraData = null)
	{
		return TakeDamage.HurtShared(victim, victim, TakeDamage.Quantity.AllHealth, extraData);
	}

	public static LifeStatus KillSelf(IDBase victim, out DamageEvent damage, object extraData = null)
	{
		return TakeDamage.HurtShared(victim, victim, TakeDamage.Quantity.AllHealth, out damage, extraData);
	}

	public virtual void LoadVitals(Vitals vitals)
	{
		this.health = vitals.Health;
		if (this.health <= 0f)
		{
			Debug.Log(string.Concat("LOAD VITALS - HEALTH WAS ", this.health));
			this.health = 1f;
		}
	}

	public void MarkDamageTime()
	{
		this._lastDamageTime = Time.time;
	}

	protected void ProcessDamageEvent(ref DamageEvent damage)
	{
		if (this.takenodamage)
		{
			return;
		}
		LifeStatus lifeStatu = damage.status;
		if (lifeStatu == LifeStatus.IsAlive)
		{
			TakeDamage takeDamage = this;
			takeDamage._health = takeDamage._health - damage.amount;
		}
		else if (lifeStatu == LifeStatus.WasKilled)
		{
			this._health = 0f;
		}
	}

	public virtual void SaveVitals(ref Vitals.Builder vitals)
	{
		vitals.SetHealth(this.health);
	}

	public virtual void ServerFrame()
	{
	}

	public virtual void SetGodMode(bool on)
	{
		this.takenodamage = on;
	}

	public bool ShouldPlayHitNotification()
	{
		return (!this.playsHitNotification ? false : this.alive);
	}

	protected bool ShouldRelayDamageEvent(ref DamageEvent damage)
	{
		switch (damage.status)
		{
			case LifeStatus.IsAlive:
			{
				return this.sendMessageWhenAlive;
			}
			case LifeStatus.WasKilled:
			{
				return this.sendMessageWhenKilled;
			}
			case LifeStatus.IsDead:
			{
				return this.sendMessageWhenDead;
			}
		}
		Debug.LogWarning(string.Concat("Unhandled LifeStatus ", damage.status), this);
		return false;
	}

	public float TimeSinceHurt()
	{
		return Time.time - this._lastDamageTime;
	}

	public override string ToString()
	{
		return string.Format("[{0}: health={1}]", base.ToString(), this.health);
	}

	public struct Quantity
	{
		public readonly TakeDamage.Unit Unit;

		internal readonly float @value;

		internal readonly DamageTypeList list;

		public static TakeDamage.Quantity AllHealth
		{
			get
			{
				return new TakeDamage.Quantity(TakeDamage.Unit.AllHealth, null, Single.PositiveInfinity);
			}
		}

		public object BoxedValue
		{
			get
			{
				object obj;
				if ((int)this.Unit > 0)
				{
					obj = this.@value;
				}
				else
				{
					obj = this.list;
				}
				return obj;
			}
		}

		public DamageTypeList DamageTypeList
		{
			get
			{
				if ((int)this.Unit > 0)
				{
					throw new InvalidOperationException("Quantity is of HealthPoints");
				}
				return this.list;
			}
		}

		public float HealthPoints
		{
			get
			{
				if ((int)this.Unit < -1)
				{
					throw new InvalidOperationException("Quantity is of DamageTypeList");
				}
				return this.@value;
			}
		}

		public bool IsAllHealthPoints
		{
			get
			{
				return (int)this.Unit == 2;
			}
		}

		public bool IsDamageTypeList
		{
			get
			{
				return (int)this.Unit == -1;
			}
		}

		public bool IsHealthPoints
		{
			get
			{
				return (int)this.Unit > 0;
			}
		}

		public bool Specified
		{
			get
			{
				return (int)this.Unit != 0;
			}
		}

		private Quantity(TakeDamage.Unit Measurement, DamageTypeList DamageTypeList, float Value)
		{
			this.Unit = Measurement;
			this.list = DamageTypeList;
			this.@value = Value;
		}

		public static implicit operator Quantity(int HealthPoints)
		{
			return new TakeDamage.Quantity((HealthPoints != 0 ? TakeDamage.Unit.HealthPoints : TakeDamage.Unit.Unspecified), null, (float)((HealthPoints >= 0 ? HealthPoints : -HealthPoints)));
		}

		public static implicit operator Quantity(float HealthPoints)
		{
			TakeDamage.Unit unit;
			if (HealthPoints != 0f)
			{
				unit = (!float.IsInfinity(HealthPoints) ? TakeDamage.Unit.HealthPoints : TakeDamage.Unit.AllHealth);
			}
			else
			{
				unit = TakeDamage.Unit.Unspecified;
			}
			return new TakeDamage.Quantity(unit, null, HealthPoints);
		}

		public static implicit operator Quantity(DamageTypeList DamageTypeList)
		{
			return new TakeDamage.Quantity((!object.ReferenceEquals(DamageTypeList, null) ? TakeDamage.Unit.HealthPoints | TakeDamage.Unit.AllHealth | TakeDamage.Unit.List : TakeDamage.Unit.Unspecified), DamageTypeList, 0f);
		}

		public override string ToString()
		{
			return string.Format("[{0}:{1}]", this.Unit, this.BoxedValue);
		}
	}

	public enum Unit : sbyte
	{
		List = -1,
		Unspecified = 0,
		HealthPoints = 1,
		AllHealth = 2
	}
}