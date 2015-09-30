using System;
using UnityEngine;

public class CharacterCCMotorTrait : CharacterTrait
{
	[SerializeField]
	private CCMotorSettings _settings;

	[SerializeField]
	private CCTotemPole _prefab;

	[SerializeField]
	private bool _canControl = true;

	[SerializeField]
	private bool _sendFallMessage;

	[SerializeField]
	private bool _sendLandMessage;

	[SerializeField]
	private bool _sendJumpMessage;

	[SerializeField]
	private bool _sendExternalVelocityMessage;

	[SerializeField]
	private bool _sendJumpFailureMessage;

	[SerializeField]
	private bool _enableColliderOnInit = true;

	[SerializeField]
	private float _minTimeBetweenJumps = 1f;

	[SerializeField]
	private CCMotor.StepMode _stepMode = CCMotor.StepMode.Elsewhere;

	public bool canControl
	{
		get
		{
			return this._canControl;
		}
	}

	public bool enableColliderOnInit
	{
		get
		{
			return this._enableColliderOnInit;
		}
	}

	public float minTimeBetweenJumps
	{
		get
		{
			return this._minTimeBetweenJumps;
		}
	}

	public CCTotemPole prefab
	{
		get
		{
			return this._prefab;
		}
	}

	public bool sendExternalVelocityMessage
	{
		get
		{
			return this._sendExternalVelocityMessage;
		}
	}

	public bool sendFallMessage
	{
		get
		{
			return this._sendFallMessage;
		}
	}

	public bool sendJumpFailureMessage
	{
		get
		{
			return this._sendJumpFailureMessage;
		}
	}

	public bool sendJumpMessage
	{
		get
		{
			return this._sendJumpMessage;
		}
	}

	public bool sendLandMessage
	{
		get
		{
			return this._sendLandMessage;
		}
	}

	public CCMotorSettings settings
	{
		get
		{
			return this._settings;
		}
	}

	public CCMotor.StepMode stepMode
	{
		get
		{
			return this._stepMode;
		}
	}

	public CharacterCCMotorTrait()
	{
	}
}