using System;
using UnityEngine;

public class CharacterRagdollTrait : CharacterTrait
{
	[SerializeField]
	private GameObject _ragdollPrefab;

	public GameObject ragdollPrefab
	{
		get
		{
			return this._ragdollPrefab;
		}
	}

	public CharacterRagdollTrait()
	{
	}
}