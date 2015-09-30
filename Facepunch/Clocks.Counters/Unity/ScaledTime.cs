using System;
using UnityEngine;

namespace Facepunch.Clocks.Counters.Unity
{
	public struct ScaledTime
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
				return TimeSpan.FromSeconds((double)((!float.IsPositiveInfinity(this.endTime) ? this.endTime : ScaledTime.TIME_SOURCE.NOW)) - this.deductSeconds - (double)this.startTime);
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
					return (double)((double)ScaledTime.TIME_SOURCE.NOW - this.deductSeconds - (double)this.startTime);
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

		public static ScaledTime Reset
		{
			get
			{
				ScaledTime scaledTime = new ScaledTime();
				scaledTime.deductSeconds = 0;
				scaledTime.endTime = Single.PositiveInfinity;
				scaledTime.startTime = Single.NegativeInfinity;
				return scaledTime;
			}
		}

		public static ScaledTime Restart
		{
			get
			{
				ScaledTime nOW = new ScaledTime();
				nOW.deductSeconds = 0;
				nOW.endTime = Single.PositiveInfinity;
				nOW.startTime = ScaledTime.TIME_SOURCE.NOW;
				return nOW;
			}
		}

		public void Start()
		{
			if (float.IsNegativeInfinity(this.startTime))
			{
				this.startTime = ScaledTime.TIME_SOURCE.NOW;
				this.deductSeconds = 0;
				this.endTime = Single.PositiveInfinity;
			}
			else if (!float.IsPositiveInfinity(this.endTime))
			{
				float single = this.endTime;
				this.endTime = Single.PositiveInfinity;
				ScaledTime nOW = this;
				nOW.deductSeconds = nOW.deductSeconds + ((double)ScaledTime.TIME_SOURCE.NOW - (double)single);
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
				this.endTime = ScaledTime.TIME_SOURCE.NOW;
			}
		}

		private static class TIME_SOURCE
		{
			public static float NOW
			{
				get
				{
					return Time.time;
				}
			}
		}
	}
}