using System;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Ignore TimeScale Behaviour")]
public class IgnoreTimeScale : MonoBehaviour
{
	private float mTimeStart;

	private float mTimeDelta;

	private float mActual;

	private bool mTimeStarted;

	public float realTimeDelta
	{
		get
		{
			return this.mTimeDelta;
		}
	}

	public IgnoreTimeScale()
	{
	}

	private void OnEnable()
	{
		this.mTimeStarted = true;
		this.mTimeDelta = 0f;
		this.mTimeStart = Time.realtimeSinceStartup;
	}

	protected float UpdateRealTimeDelta()
	{
		if (!this.mTimeStarted)
		{
			this.mTimeStarted = true;
			this.mTimeStart = Time.realtimeSinceStartup;
			this.mTimeDelta = 0f;
		}
		else
		{
			float single = Time.realtimeSinceStartup;
			float single1 = single - this.mTimeStart;
			IgnoreTimeScale ignoreTimeScale = this;
			ignoreTimeScale.mActual = ignoreTimeScale.mActual + Mathf.Max(0f, single1);
			this.mTimeDelta = 0.001f * Mathf.Round(this.mActual * 1000f);
			IgnoreTimeScale ignoreTimeScale1 = this;
			ignoreTimeScale1.mActual = ignoreTimeScale1.mActual - this.mTimeDelta;
			this.mTimeStart = single;
		}
		return this.mTimeDelta;
	}
}