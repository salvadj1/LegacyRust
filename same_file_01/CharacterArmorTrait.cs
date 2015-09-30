using System;
using UnityEngine;

public class CharacterArmorTrait : CharacterTrait
{
	[SerializeField]
	private ArmorModelGroup _defaultGroup;

	public ArmorModelGroup defaultGroup
	{
		get
		{
			return this._defaultGroup;
		}
	}

	public CharacterArmorTrait()
	{
	}
}