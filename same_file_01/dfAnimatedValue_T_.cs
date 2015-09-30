using System;
using UnityEngine;

public abstract class dfAnimatedValue<T>
where T : struct
{
	private T startValue;

	private T endValue;

	private float animLength;

	private float startTime;

	private dfEasingType easingType;

	private dfEasingFunctions.EasingFunction easingFunction;

	public dfEasingType EasingType
	{
		get
		{
			return this.easingType;
		}
		set
		{
			this.easingType = value;
			this.easingFunction = dfEasingFunctions.GetFunction(this.easingType);
		}
	}

	public T EndValue
	{
		get
		{
			return this.endValue;
		}
		set
		{
			this.endValue = value;
			this.startTime = Time.realtimeSinceStartup;
		}
	}

	public bool IsDone
	{
		get
		{
			return Time.realtimeSinceStartup - this.startTime >= this.Length;
		}
	}

	public float Length
	{
		get
		{
			return this.animLength;
		}
		set
		{
			this.animLength = value;
			this.startTime = Time.realtimeSinceStartup;
		}
	}

	public T StartValue
	{
		get
		{
			return this.startValue;
		}
		set
		{
			this.startValue = value;
			this.startTime = Time.realtimeSinceStartup;
		}
	}

	public T Value
	{
		get
		{
			float single = Time.realtimeSinceStartup - this.startTime;
			if (single >= this.animLength)
			{
				return this.endValue;
			}
			float single1 = Mathf.Clamp01(single / this.animLength);
			single1 = this.easingFunction(0f, 1f, single1);
			return this.Lerp(this.startValue, this.endValue, single1);
		}
	}

	protected internal dfAnimatedValue(T StartValue, T EndValue, float Time) : this()
	{
		this.startValue = StartValue;
		this.endValue = EndValue;
		this.animLength = Time;
	}

	protected internal dfAnimatedValue()
	{
		this.startTime = Time.realtimeSinceStartup;
		this.easingFunction = dfEasingFunctions.GetFunction(this.easingType);
	}

	protected abstract T Lerp(T startValue, T endValue, float time);

	public static implicit operator T(dfAnimatedValue<T> animated)
	{
		return animated.Value;
	}
}