using System;
using UnityEngine;

public class CharacterMeleeTrait : CharacterTrait
{
	[SerializeField]
	private float _minDamage = 15f;

	[SerializeField]
	private float _maxDamage = 25f;

	public float maxDamage
	{
		get
		{
			return this._maxDamage;
		}
	}

	public float minDamage
	{
		get
		{
			return this._minDamage;
		}
	}

	public float randomDamage
	{
		get
		{
			return this._minDamage + (this._maxDamage - this._minDamage) * UnityEngine.Random.@value;
		}
	}

	public CharacterMeleeTrait()
	{
	}
}