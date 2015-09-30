using System;
using UnityEngine;

namespace Facepunch.Clocks.Counters.Unity
{
	public struct RealtimeSinceStartup
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
				return TimeSpan.FromSeconds((double)((!float.IsPositiveInfinity(this.endTime) ? this.endTime : RealtimeSinceStartup.TIME_SOURCE.NOW)) - this.deductSeconds - (double)this.startTime);
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
					return (double)((double)RealtimeSinceStartup.TIME_SOURCE.NOW - this.deductSeconds - (double)this.startTime);
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

		public static RealtimeSinceStartup Reset
		{
			get
			{
				RealtimeSinceStartup realtimeSinceStartup = new RealtimeSinceStartup();
				realtimeSinceStartup.deductSeconds = 0;
				realtimeSinceStartup.endTime = Single.PositiveInfinity;
				realtimeSinceStartup.startTime = Single.NegativeInfinity;
				return realtimeSinceStartup;
			}
		}

		public static RealtimeSinceStartup Restart
		{
			get
			{
				RealtimeSinceStartup nOW = new RealtimeSinceStartup();
				nOW.deductSeconds = 0;
				nOW.endTime = Single.PositiveInfinity;
				nOW.startTime = RealtimeSinceStartup.TIME_SOURCE.NOW;
				return nOW;
			}
		}

		public void Start()
		{
			if (float.IsNegativeInfinity(this.startTime))
			{
				this.startTime = RealtimeSinceStartup.TIME_SOURCE.NOW;
				this.deductSeconds = 0;
				this.endTime = Single.PositiveInfinity;
			}
			else if (!float.IsPositiveInfinity(this.endTime))
			{
				float single = this.endTime;
				this.endTime = Single.PositiveInfinity;
				RealtimeSinceStartup nOW = this;
				nOW.deductSeconds = nOW.deductSeconds + ((double)RealtimeSinceStartup.TIME_SOURCE.NOW - (double)single);
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
				this.endTime = RealtimeSinceStartup.TIME_SOURCE.NOW;
			}
		}

		private static class TIME_SOURCE
		{
			public static float NOW
			{
				get
				{
					return Time.realtimeSinceStartup;
				}
			}
		}
	}
}