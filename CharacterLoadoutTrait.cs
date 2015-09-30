using System;
using UnityEngine;

public class CharacterLoadoutTrait : CharacterTrait
{
	[SerializeField]
	private Loadout _loadout;

	public Loadout loadout
	{
		get
		{
			return this._loadout;
		}
	}

	public CharacterLoadoutTrait()
	{
	}
}