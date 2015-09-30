using System;
using UnityEngine;

public class CharacterAnimationTrait : CharacterTrait
{
	[SerializeField]
	private MovementAnimationSetup _movementAnimationSetup;

	[SerializeField]
	private string _defaultGroupName = "noitem";

	public string defaultGroupName
	{
		get
		{
			return this._defaultGroupName;
		}
	}

	public MovementAnimationSetup movementAnimationSetup
	{
		get
		{
			return this._movementAnimationSetup;
		}
	}

	public CharacterAnimationTrait()
	{
	}
}