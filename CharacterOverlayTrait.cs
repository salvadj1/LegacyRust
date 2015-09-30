using System;
using UnityEngine;

public class CharacterOverlayTrait : CharacterTrait
{
	[SerializeField]
	private string _overlayComponentName = "LocalDamageDisplay";

	[SerializeField]
	private Texture2D _damageOverlay;

	[SerializeField]
	private Texture2D _damageOverlay2;

	[SerializeField]
	private ScriptableObject _takeDamageBob;

	[SerializeField]
	private ScriptableObject _meleeBob;

	public Texture2D damageOverlay
	{
		get
		{
			return this._damageOverlay;
		}
	}

	public Texture2D damageOverlay2
	{
		get
		{
			return this._damageOverlay2;
		}
	}

	public ScriptableObject meleeBob
	{
		get
		{
			return this._meleeBob;
		}
	}

	public string overlayComponentName
	{
		get
		{
			return this._overlayComponentName;
		}
	}

	public ScriptableObject takeDamageBob
	{
		get
		{
			return this._takeDamageBob;
		}
	}

	public CharacterOverlayTrait()
	{
	}
}