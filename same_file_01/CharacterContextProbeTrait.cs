using System;
using UnityEngine;

public class CharacterContextProbeTrait : CharacterTrait
{
	[SerializeField]
	private float _rayLength = 3f;

	public float rayLength
	{
		get
		{
			return this._rayLength;
		}
	}

	public CharacterContextProbeTrait()
	{
	}
}