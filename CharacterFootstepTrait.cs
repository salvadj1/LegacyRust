using System;
using UnityEngine;

public class CharacterFootstepTrait : CharacterTrait
{
	[SerializeField]
	private AudioClipArray _defaultFootsteps;

	[SerializeField]
	private float _strideDist = 2.5f;

	[SerializeField]
	private float _minAudioDist = 3f;

	[SerializeField]
	private float _maxAudioDist = 30f;

	[SerializeField]
	private bool _animal;

	[SerializeField]
	private float _maxPerSecond = 6f;

	public bool animal
	{
		get
		{
			return this._animal;
		}
	}

	public AudioClipArray defaultFootsteps
	{
		get
		{
			return this._defaultFootsteps;
		}
	}

	public float maxAudioDist
	{
		get
		{
			return this._maxAudioDist;
		}
	}

	public float maxPerSecond
	{
		get
		{
			return this._maxPerSecond;
		}
	}

	public float minAudioDist
	{
		get
		{
			return this._minAudioDist;
		}
	}

	public float minInterval
	{
		get
		{
			return (!this.timeLimited ? 0f : 1f / this._maxPerSecond);
		}
	}

	public float sqrStrideDist
	{
		get
		{
			return this._strideDist * this._strideDist;
		}
	}

	public float strideDist
	{
		get
		{
			return this._strideDist;
		}
	}

	public bool timeLimited
	{
		get
		{
			return (this._maxPerSecond <= 0f ? false : !float.IsInfinity(this._maxPerSecond));
		}
	}

	public CharacterFootstepTrait()
	{
	}
}