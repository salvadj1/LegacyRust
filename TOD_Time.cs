using System;
using UnityEngine;

public class TOD_Time : MonoBehaviour
{
	public float DayLengthInMinutes = 30f;

	public bool ProgressDate = true;

	public bool ProgressMoonPhase = true;

	private TOD_Sky sky;

	public TOD_Time()
	{
	}

	protected void Start()
	{
		this.sky = base.GetComponent<TOD_Sky>();
	}

	protected void Update()
	{
		float dayLengthInMinutes = this.DayLengthInMinutes * 60f;
		float single = dayLengthInMinutes / 24f;
		float single1 = Time.deltaTime / single;
		float single2 = Time.deltaTime / (30f * dayLengthInMinutes) * 2f;
		TOD_CycleParameters cycle = this.sky.Cycle;
		cycle.Hour = cycle.Hour + single1;
		if (this.ProgressMoonPhase)
		{
			TOD_CycleParameters moonPhase = this.sky.Cycle;
			moonPhase.MoonPhase = moonPhase.MoonPhase + single2;
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
		}
		if (this.sky.Cycle.Hour >= 24f)
		{
			this.sky.Cycle.Hour = 0f;
			if (this.ProgressDate)
			{
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
}