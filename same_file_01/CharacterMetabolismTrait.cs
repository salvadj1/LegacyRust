using System;
using UnityEngine;

public class CharacterMetabolismTrait : CharacterTrait
{
	[SerializeField]
	private float _tickRate = 3f;

	[SerializeField]
	private bool _selfTick;

	[SerializeField]
	private float _hungerDamagePerMin = 5f;

	public float hungerDamagePerMin
	{
		get
		{
			return this._hungerDamagePerMin;
		}
	}

	public bool selfTick
	{
		get
		{
			return this._selfTick;
		}
	}

	public float tickRate
	{
		get
		{
			return this._tickRate;
		}
	}

	public CharacterMetabolismTrait()
	{
	}
}