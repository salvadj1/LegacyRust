using System;
using UnityEngine;

public class CharacterItemAttachmentTrait : CharacterTrait
{
	[SerializeField]
	private Socket.ConfigBodyPart _socket;

	public Socket.ConfigBodyPart socket
	{
		get
		{
			return this._socket;
		}
	}

	public CharacterItemAttachmentTrait()
	{
	}
}