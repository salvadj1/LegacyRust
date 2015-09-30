using System;
using UnityEngine;

public class CharacterSoundsTrait : CharacterTrait
{
	[SerializeField]
	private AudioClipArray _attack;

	[SerializeField]
	private AudioClipArray _alert;

	[SerializeField]
	private AudioClipArray _idle;

	[SerializeField]
	private AudioClipArray _persuit;

	[SerializeField]
	private AudioClipArray _impact;

	[SerializeField]
	private AudioClipArray _death;

	public AudioClipArray alert
	{
		get
		{
			return this._alert;
		}
	}

	public AudioClipArray attack
	{
		get
		{
			return this._attack;
		}
	}

	public AudioClipArray death
	{
		get
		{
			return this._death;
		}
	}

	public AudioClipArray idle
	{
		get
		{
			return this._idle;
		}
	}

	public AudioClipArray impact
	{
		get
		{
			return this._impact;
		}
	}

	public AudioClipArray persuit
	{
		get
		{
			return this._persuit;
		}
	}

	public CharacterSoundsTrait()
	{
	}
}