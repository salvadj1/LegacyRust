using System;
using UnityEngine;

namespace Facepunch.Clocks.Counters.Unity
{
	public struct FixedTime
	{
		private const double ZeroDeductions = 0;

		private const double OneThousand = 1000;

		private const double ZeroElapsed = 0;

		private float startTime;

		private float endTime;

		private double deductSeconds;

		public TimeSpan Elapsed
		{
			get
			{
				if (float.IsNegativeInfinity(this.startTime))
				{
					return TimeSpan.Zero;
				}
				return TimeSpan.FromSeconds((double)((!float.IsPositiveInfinity(this.endTime) ? this.endTime : FixedTime.TIME_SOURCE.NOW)) - this.deductSeconds - (double)this.startTime);
			}
		}

		public long ElapsedMilliseconds
		{
			get
			{
				return (long)Math.Floor(this.ElapsedSeconds * 1000);
			}
		}

		public double ElapsedSeconds
		{
			get
			{
				if (float.IsNegativeInfinity(this.startTime))
				{
					return 0;
				}
				if (float.IsPositiveInfinity(this.endTime))
				{
					return (double)((double)FixedTime.TIME_SOURCE.NOW - this.deductSeconds - (double)this.startTime);
				}
				return (double)((double)this.endTime - this.deductSeconds - (double)this.startTime);
			}
		}

		public bool IsRunning
		{
			get
			{
				return (!float.IsPositiveInfinity(this.endTime) ? false : !float.IsNegativeInfinity(this.startTime));
			}
		}

		public static FixedTime Reset
		{
			get
			{
				FixedTime fixedTime = new FixedTime();
				fixedTime.deductSeconds = 0;
				fixedTime.endTime = Single.PositiveInfinity;
				fixedTime.startTime = Single.NegativeInfinity;
				return fixedTime;
			}
		}

		public static FixedTime Restart
		{
			get
			{
				FixedTime nOW = new FixedTime();
				nOW.deductSeconds = 0;
				nOW.endTime = Single.PositiveInfinity;
				nOW.startTime = FixedTime.TIME_SOURCE.NOW;
				return nOW;
			}
		}

		public void Start()
		{
			if (float.IsNegativeInfinity(this.startTime))
			{
				this.startTime = FixedTime.TIME_SOURCE.NOW;
				this.deductSeconds = 0;
				this.endTime = Single.PositiveInfinity;
			}
			else if (!float.IsPositiveInfinity(this.endTime))
			{
				float single = this.endTime;
				this.endTime = Single.PositiveInfinity;
				FixedTime nOW = this;
				nOW.deductSeconds = nOW.deductSeconds + ((double)FixedTime.TIME_SOURCE.NOW - (double)single);
			}
		}

		public void Stop()
		{
			if (float.IsNegativeInfinity(this.startTime))
			{
				return;
			}
			if (float.IsPositiveInfinity(this.endTime))
			{
				this.endTime = FixedTime.TIME_SOURCE.NOW;
			}
		}

		private static class TIME_SOURCE
		{
			public static float NOW
			{
				get
				{
					return Time.fixedTime;
				}
			}
		}
	}
}