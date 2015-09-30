using System;
using UnityEngine;

public class CharacterWalkSpeedTrait : CharacterTrait
{
	[SerializeField]
	private float _jog = 3f;

	[SerializeField]
	private float _run = 6f;

	[SerializeField]
	private float _walk = 1.8f;

	public float jog
	{
		get
		{
			return this._jog;
		}
	}

	public float run
	{
		get
		{
			return this._run;
		}
	}

	public float walk
	{
		get
		{
			return this._walk;
		}
	}

	public CharacterWalkSpeedTrait()
	{
	}

	public bool IsJoggingOrRunningAtSpeed(float metersPerSecond)
	{
		return (this._jog >= this._run ? this._run <= metersPerSecond : this._jog <= metersPerSecond);
	}

	public bool IsRunningAtSpeed(float metersPerSecond)
	{
		return (this._jog >= this._run ? this._run > metersPerSecond : this._run <= metersPerSecond);
	}
}