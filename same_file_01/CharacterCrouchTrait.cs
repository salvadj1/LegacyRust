using System;
using UnityEngine;

public class CharacterCrouchTrait : CharacterTrait
{
	[SerializeField]
	private AnimationCurve _crouchCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f, 0f, 0f), new Keyframe(0.55f, -0.55f, 0f, 0f) });

	[SerializeField]
	private float _crouchToSpeedFraction = 1.3f;

	[SerializeField]
	private float _maxCrouchFraction = 0.9f;

	public AnimationCurve crouchCurve
	{
		get
		{
			return this._crouchCurve;
		}
	}

	public float crouchInSpeed
	{
		get
		{
			return -Mathf.Abs(this.crouchSpeedBase * this._crouchToSpeedFraction);
		}
	}

	public float crouchOutSpeed
	{
		get
		{
			return Mathf.Abs(this.crouchSpeedBase);
		}
	}

	private float crouchSpeedBase
	{
		get
		{
			Keyframe item = this._crouchCurve[0];
			Keyframe keyframe = this._crouchCurve[this._crouchCurve.length - 1];
			float single = keyframe.@value - item.@value;
			return single / (keyframe.time - item.time);
		}
	}

	public float crouchToSpeedFraction
	{
		get
		{
			return this._crouchToSpeedFraction;
		}
	}

	public CharacterCrouchTrait()
	{
	}

	public bool IsCrouching(float minHeight, float maxHeight, float currentHeight)
	{
		return Mathf.InverseLerp(minHeight, maxHeight, currentHeight) <= this._maxCrouchFraction;
	}
}