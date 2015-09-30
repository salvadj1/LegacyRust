using System;
using UnityEngine;

namespace Facepunch.Procedural
{
	public struct MillisClock
	{
		[NonSerialized]
		public ulong remain;

		[NonSerialized]
		public ulong duration;

		[NonSerialized]
		public bool once;

		public ClockStatus clockStatus
		{
			get
			{
				ClockStatus clockStatu;
				if (!this.once)
				{
					clockStatu = ClockStatus.Unset;
				}
				else if (this.remain != 0)
				{
					clockStatu = (this.remain >= this.duration ? ClockStatus.Negative : ClockStatus.WillElapse);
				}
				else
				{
					clockStatu = (this.duration != 0 ? ClockStatus.Elapsed : ClockStatus.Unset);
				}
				return clockStatu;
			}
		}

		public double percent
		{
			get
			{
				double num;
				if (this.remain != 0)
				{
					num = (this.remain < this.duration ? 1 - (double)((float)this.remain) / (double)((float)this.duration) : 0);
				}
				else
				{
					num = 1;
				}
				return num;
			}
		}

		public float percentf
		{
			get
			{
				float single;
				if (this.remain != 0)
				{
					single = (this.remain < this.duration ? (float)(1 - (double)((float)this.remain) / (double)((float)this.duration)) : 0f);
				}
				else
				{
					single = 1f;
				}
				return single;
			}
		}

		public Integration IntegrateTime(ulong millis)
		{
			if (!this.once || this.remain == 0 || this.duration == 0 || millis == 0)
			{
				return Integration.Stationary;
			}
			if (this.remain <= millis)
			{
				this.remain = (ulong)0;
				return Integration.Stationary;
			}
			MillisClock millisClock = this;
			millisClock.remain = millisClock.remain - millis;
			if (this.remain < this.duration)
			{
				return Integration.Moved;
			}
			return Integration.MovedDestination;
		}

		public bool IntegrateTime_Reached(ulong millis)
		{
			return (byte)(this.IntegrateTime(millis) & Integration.Stationary) == 1;
		}

		public ClockStatus ResetDurationMillis(ulong millis)
		{
			if (millis <= (long)1)
			{
				this.SetImmediate();
				return ClockStatus.DidElapse;
			}
			this.once = true;
			ulong num = millis;
			ulong num1 = num;
			this.duration = num;
			this.remain = num1;
			return ClockStatus.WillElapse;
		}

		public ClockStatus ResetDurationSeconds(double seconds)
		{
			return this.ResetDurationMillis((ulong)Math.Ceiling(seconds * 1000));
		}

		public ClockStatus ResetRandomDurationSeconds(double secondsMin, double secondsMax)
		{
			return this.ResetDurationSeconds(secondsMin + (double)UnityEngine.Random.@value * (secondsMax - secondsMin));
		}

		public void SetImmediate()
		{
			this.once = true;
			this.remain = (ulong)1;
			this.duration = (ulong)2;
		}
	}
}