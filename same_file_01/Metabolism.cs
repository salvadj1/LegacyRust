using Facepunch;
using RustProto;
using System;
using uLink;
using UnityEngine;

public class Metabolism : IDLocalCharacter
{
	private bool _dirty;

	private float _lastTickTime;

	[NonSerialized]
	public float tickRate = 3f;

	[NonSerialized]
	public bool selfTick;

	[NonSerialized]
	public float hungerDamagePerMin = 5f;

	private float caloricLevel = 1250f;

	private float maxCaloricLevel = 3000f;

	private float caloriesPerHP = 5f;

	private float starvingDamagePerMin = 10f;

	private float waterLevelLitre = 30f;

	private float maxWaterLevelLitre = 30f;

	private float caloricMetabolicRate = 300f;

	private float caloricMetabolicRateMax = 3000f;

	private float hydrationMetablicRate = 0.125f;

	private float sweatWaterLossMax = 1.5f;

	private float radMetabolizationRate = 800f;

	private float damagePerRad = 0.06f;

	private float radiationLevel;

	private float maxRadiationLevel = 3000f;

	private float antiRads;

	private float antiRadUsePerMin = 3000f;

	private float _activityLevel;

	private float _targetActivityLevel;

	private float _lastConsumeTime;

	private float coreTemperature;

	private float _lastWarmTime = -1000f;

	private float lastVomitTime;

	private float nextVomitTime;

	private float poisonLevel;

	private float _lastDamageTime;

	private static bool dmg_metabolism;

	private static AudioClip vomitSound;

	static Metabolism()
	{
		Metabolism.dmg_metabolism = true;
	}

	public Metabolism()
	{
	}

	public void AddAntiRad(float addAntiRad)
	{
		Metabolism metabolism = this;
		metabolism.antiRads = metabolism.antiRads + addAntiRad;
		this.MakeDirty();
	}

	public void AddCalories(float numCalories)
	{
		Metabolism metabolism = this;
		metabolism.caloricLevel = metabolism.caloricLevel + numCalories;
		if (this.caloricLevel > this.maxCaloricLevel)
		{
			this.caloricLevel = this.maxCaloricLevel;
		}
		this.MakeDirty();
	}

	public void AddPoison(float amount)
	{
		Metabolism metabolism = this;
		metabolism.poisonLevel = metabolism.poisonLevel + amount;
		if (Time.time > this.nextVomitTime)
		{
			this.nextVomitTime = Time.time + 5f;
		}
		this.MakeDirty();
	}

	public void AddRads(float rads)
	{
		Metabolism metabolism = this;
		metabolism.radiationLevel = metabolism.radiationLevel + rads;
		this.MakeDirty();
	}

	public void AddWater(float litres)
	{
		Metabolism metabolism = this;
		metabolism.waterLevelLitre = metabolism.waterLevelLitre + litres;
		if (this.waterLevelLitre > this.maxWaterLevelLitre)
		{
			this.waterLevelLitre = this.maxWaterLevelLitre;
		}
		this.MakeDirty();
	}

	private void Awake()
	{
		CharacterMetabolismTrait trait = base.GetTrait<CharacterMetabolismTrait>();
		if (trait)
		{
			this.hungerDamagePerMin = trait.hungerDamagePerMin;
			this.selfTick = trait.selfTick;
			this.tickRate = trait.tickRate;
		}
		this._lastTickTime = Time.time;
	}

	public bool CanConsumeYet()
	{
		return this.GetNextConsumeTime() < Time.time;
	}

	public void DoNetworkUpdate()
	{
		if (this.IsDirty())
		{
			base.networkView.RPC("RecieveNetwork", base.networkView.owner, new object[] { this.caloricLevel, this.waterLevelLitre, this.radiationLevel, this.antiRads, this.coreTemperature, this.poisonLevel });
		}
		this.MakeClean();
	}

	public float GetActivityLevel()
	{
		return this._activityLevel;
	}

	public float GetCalorieLevel()
	{
		return this.caloricLevel;
	}

	public float GetNextConsumeTime()
	{
		return this._lastConsumeTime + 3f;
	}

	public float GetRadLevel()
	{
		return this.radiationLevel;
	}

	public float GetRemainingCaloricSpace()
	{
		return this.maxCaloricLevel - this.caloricLevel;
	}

	public bool HasRadiationPoisoning()
	{
		return this.radiationLevel > 500f;
	}

	public bool IsCold()
	{
		return this.coreTemperature < 0f;
	}

	public bool IsDirty()
	{
		return this._dirty;
	}

	public bool IsPoisoned()
	{
		return this.poisonLevel > 1f;
	}

	public bool IsWarm()
	{
		return Time.time - this._lastWarmTime <= 1f;
	}

	public void LoadVitals(Vitals vitals)
	{
		this.caloricLevel = vitals.Calories;
		this.waterLevelLitre = vitals.Hydration;
		this.radiationLevel = vitals.Radiation;
		this.antiRads = vitals.RadiationAnti;
		this.coreTemperature = vitals.Temperature;
	}

	public void MakeClean()
	{
		this._dirty = false;
	}

	public void MakeDirty()
	{
		this._dirty = true;
	}

	public void MarkConsumptionTime()
	{
		this._lastConsumeTime = Time.time;
	}

	public void MarkDamageTime()
	{
		this._lastDamageTime = Time.time;
	}

	public void MarkWarm()
	{
		this._lastWarmTime = Time.time;
	}

	private void MetabolicFrame()
	{
		this.MetabolicUpdateFrame();
	}

	public LifeStatus MetabolicUpdateFrame()
	{
		return (!base.alive ? LifeStatus.IsDead : LifeStatus.IsAlive);
	}

	public void OnHurt(DamageEvent damage)
	{
		this.MarkDamageTime();
	}

	[RPC]
	public void RecieveNetwork(float calories, float water, float rad, float anti, float temp, float poison)
	{
		this.caloricLevel = calories;
		this.waterLevelLitre = water;
		this.radiationLevel = rad;
		this.antiRads = anti;
		this.coreTemperature = temp;
		this.poisonLevel = poison;
		if (temp >= 1f)
		{
			this._lastWarmTime = Time.time;
		}
		else if (temp < 0f)
		{
			this._lastWarmTime = -1000f;
		}
		RPOS.MetabolismUpdate();
	}

	public void SaveVitals(ref Vitals.Builder vitals)
	{
		vitals.SetCalories(this.caloricLevel);
		vitals.SetHydration(this.waterLevelLitre);
		vitals.SetRadiation(this.radiationLevel);
		vitals.SetRadiationAnti(this.antiRads);
		vitals.SetTemperature(this.coreTemperature);
	}

	public void SetTargetActivityLevel(float level)
	{
		this._targetActivityLevel = level;
	}

	public void SubtractCalories(float numCalories)
	{
		Metabolism metabolism = this;
		metabolism.caloricLevel = metabolism.caloricLevel - numCalories;
		if (this.caloricLevel < 0f)
		{
			this.caloricLevel = 0f;
		}
		this.MakeDirty();
	}

	public void SubtractPosion(float amount)
	{
		Metabolism metabolism = this;
		metabolism.poisonLevel = metabolism.poisonLevel - amount;
		if (this.poisonLevel <= 0f)
		{
			this.nextVomitTime = 0f;
		}
		this.MakeDirty();
	}

	public float TimeSinceHurt()
	{
		return Time.time - this._lastDamageTime;
	}

	[RPC]
	public void Vomit()
	{
		if (Metabolism.vomitSound == null)
		{
			Bundling.Load<AudioClip>("content/shared/sfx/vomit", out Metabolism.vomitSound);
		}
		Metabolism.vomitSound.Play(1f);
	}
}