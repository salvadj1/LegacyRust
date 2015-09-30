using Facepunch.Actor;
using System;
using UnityEngine;

public class CharacterActorRigTrait : CharacterTrait
{
	[SerializeField]
	private ActorRig _actorRig;

	public ActorRig actorRig
	{
		get
		{
			return this._actorRig;
		}
	}

	public CharacterActorRigTrait()
	{
	}
}