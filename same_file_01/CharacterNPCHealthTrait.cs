using System;
using UnityEngine;

public class CharacterNPCHealthTrait : CharacterTrait
{
	[SerializeField]
	private float _initialHealth;

	public float initialHealth
	{
		get
		{
			return this._initialHealth;
		}
	}

	public CharacterNPCHealthTrait()
	{
	}
}