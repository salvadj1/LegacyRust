using System;
using UnityEngine;

[Serializable]
public class TOD_CycleParameters
{
	public float Hour = 12f;

	public int Day = 1;

	public int Month = 3;

	public int Year = 2000;

	public float MoonPhase;

	public float Latitude;

	public float Longitude;

	public float UTC;

	public System.DateTime DateTime
	{
		get
		{
			this.CheckRange();
			int hour = (int)this.Hour;
			float single = (this.Hour - (float)hour) * 60f;
			int num = (int)single;
			int num1 = (int)((single - (float)num) * 60f);
			return new System.DateTime(this.Year, this.Month, this.Day, hour, num, num1);
		}
		set
		{
			this.Year = value.Year;
			this.Month = value.Month;
			this.Day = value.Day;
			this.Hour = (float)value.Hour + (float)value.Minute / 60f + (float)value.Second / 3600f;
		}
	}

	public long Ticks
	{
		get
		{
			return this.DateTime.Ticks;
		}
		set
		{
			this.DateTime = new System.DateTime(value);
		}
	}

	public TOD_CycleParameters()
	{
	}

	public void CheckRange()
	{
		this.Year = Mathf.Clamp(this.Year, 1, 9999);
		this.Month = Mathf.Clamp(this.Month, 1, 12);
		this.Day = Mathf.Clamp(this.Day, 1, System.DateTime.DaysInMonth(this.Year, this.Month));
		this.Hour = Mathf.Repeat(this.Hour, 24f);
		this.Longitude = Mathf.Clamp(this.Longitude, -180f, 180f);
		this.Latitude = Mathf.Clamp(this.Latitude, -90f, 90f);
		this.MoonPhase = Mathf.Clamp(this.MoonPhase, -1f, 1f);
	}
}