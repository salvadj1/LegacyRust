using System;
using UnityEngine;

public class CharacterTOETrait : CharacterTrait
{
	[SerializeField]
	private float _attackMinimumDistance = 1.5f;

	[SerializeField]
	private float _attackMaximumDistance = 3f;

	[SerializeField]
	private float _seekMaximumDistance = 30f;

	[SerializeField]
	private float _persuitMaximumDistance = 40f;

	[SerializeField]
	private float _attackDuration = 1.5f;

	public float attackDurationInSeconds
	{
		get
		{
			return this._attackDuration;
		}
	}

	public float attackMaximumDistance
	{
		get
		{
			return this._attackMaximumDistance;
		}
	}

	public float attackMinimumDistance
	{
		get
		{
			return this._attackMinimumDistance;
		}
	}

	public float persuitMaximumDistance
	{
		get
		{
			return this._persuitMaximumDistance;
		}
	}

	public float seekMaximumDistance
	{
		get
		{
			return this._seekMaximumDistance;
		}
	}

	public CharacterTOETrait()
	{
	}
}