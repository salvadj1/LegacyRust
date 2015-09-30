using System;
using UnityEngine;

public class CharacterInterpolatorTrait : CharacterTrait
{
	[SerializeField]
	private string _interpolatorComponentTypeName;

	[SerializeField]
	private int _bufferCapacity = -1;

	[SerializeField]
	private bool _allowExtrapolation;

	[SerializeField]
	private float _allowableTimeSpan = 0.1f;

	public float allowableTimeSpan
	{
		get
		{
			return this._allowableTimeSpan;
		}
	}

	public bool allowExtrapolation
	{
		get
		{
			return this._allowExtrapolation;
		}
	}

	public int bufferCapacity
	{
		get
		{
			return this._bufferCapacity;
		}
	}

	public string interpolatorComponentTypeName
	{
		get
		{
			return this._interpolatorComponentTypeName;
		}
	}

	public CharacterInterpolatorTrait()
	{
	}

	public virtual Interpolator AddInterpolator(IDMain main)
	{
		if (string.IsNullOrEmpty(this._interpolatorComponentTypeName))
		{
			return null;
		}
		Component component = main.gameObject.AddComponent(this._interpolatorComponentTypeName);
		Interpolator interpolator = component as Interpolator;
		if (interpolator)
		{
			interpolator.idMain = main;
			return interpolator;
		}
		Debug.LogError(string.Concat(this._interpolatorComponentTypeName, " is not a interpolator"));
		UnityEngine.Object.Destroy(component);
		return null;
	}
}