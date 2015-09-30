using System;
using UnityEngine;

public class DeviceTime : MonoBehaviour
{
	public TOD_Sky sky;

	public DeviceTime()
	{
	}

	protected void OnEnable()
	{
		if (this.sky)
		{
			DateTime now = DateTime.Now;
			this.sky.Cycle.Year = now.Year;
			this.sky.Cycle.Month = now.Month;
			this.sky.Cycle.Day = now.Day;
			this.sky.Cycle.Hour = (float)now.Hour + (float)now.Minute / 60f;
		}
		else
		{
			Debug.LogError("Sky instance reference not set. Disabling script.");
			base.enabled = false;
		}
	}
}