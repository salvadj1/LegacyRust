using Facepunch;
using System;
using uLink;
using UnityEngine;

public class EnvironmentControlCenter : NetBehaviour
{
	public static EnvironmentControlCenter Singleton;

	private TOD_Sky sky;

	static EnvironmentControlCenter()
	{
	}

	public EnvironmentControlCenter()
	{
	}

	protected void Awake()
	{
		EnvironmentControlCenter.Singleton = this;
	}

	[RPC]
	private void CL_UpdateSkyState(uLink.BitStream stream)
	{
		env.daylength = stream.Read<float>(new object[0]);
		env.nightlength = stream.Read<float>(new object[0]);
		this.sky.Cycle.MoonPhase = stream.Read<float>(new object[0]);
		this.sky.Components.Animation.CloudUV = stream.Read<Vector4>(new object[0]);
		this.sky.Cycle.Year = stream.Read<int>(new object[0]);
		this.sky.Cycle.Month = stream.Read<byte>(new object[0]);
		this.sky.Cycle.Day = stream.Read<byte>(new object[0]);
		float single = stream.Read<float>(new object[0]);
		if (Mathf.Abs(this.sky.Cycle.Hour - single) > 0.0166666675f && this.sky.Cycle.Hour > 0.05f)
		{
			this.sky.Cycle.Hour = single;
		}
	}

	public float GetTime()
	{
		if (this.sky == null)
		{
			return 0f;
		}
		return this.sky.Cycle.Hour;
	}

	public bool IsNight()
	{
		if (!this.sky)
		{
			return false;
		}
		return this.sky.IsNight;
	}

	private void OnDestroy()
	{
		if (EnvironmentControlCenter.Singleton == this)
		{
			EnvironmentControlCenter.Singleton = null;
		}
	}

	protected void Update()
	{
		if (this.sky == null)
		{
			this.sky = (TOD_Sky)UnityEngine.Object.FindObjectOfType(typeof(TOD_Sky));
			if (this.sky == null)
			{
				return;
			}
		}
		float single = env.daylength * 60f;
		if (this.sky.IsNight)
		{
			single = env.nightlength * 60f;
		}
		float single1 = single / 24f;
		float single2 = Time.deltaTime / single1;
		float single3 = Time.deltaTime / (30f * single) * 2f;
		TOD_CycleParameters cycle = this.sky.Cycle;
		cycle.Hour = cycle.Hour + single2;
		TOD_CycleParameters moonPhase = this.sky.Cycle;
		moonPhase.MoonPhase = moonPhase.MoonPhase + single3;
		if (this.sky.Cycle.MoonPhase < -1f)
		{
			TOD_CycleParameters tODCycleParameter = this.sky.Cycle;
			tODCycleParameter.MoonPhase = tODCycleParameter.MoonPhase + 2f;
		}
		else if (this.sky.Cycle.MoonPhase > 1f)
		{
			TOD_CycleParameters cycle1 = this.sky.Cycle;
			cycle1.MoonPhase = cycle1.MoonPhase - 2f;
		}
		if (this.sky.Cycle.Hour >= 24f)
		{
			this.sky.Cycle.Hour = 0f;
			int num = DateTime.DaysInMonth(this.sky.Cycle.Year, this.sky.Cycle.Month);
			TOD_CycleParameters tODCycleParameter1 = this.sky.Cycle;
			int day = tODCycleParameter1.Day + 1;
			int num1 = day;
			tODCycleParameter1.Day = day;
			if (num1 > num)
			{
				this.sky.Cycle.Day = 1;
				TOD_CycleParameters cycle2 = this.sky.Cycle;
				int month = cycle2.Month + 1;
				num1 = month;
				cycle2.Month = month;
				if (num1 > 12)
				{
					this.sky.Cycle.Month = 1;
					TOD_CycleParameters year = this.sky.Cycle;
					year.Year = year.Year + 1;
				}
			}
		}
	}
}