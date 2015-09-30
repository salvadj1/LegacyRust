using System;
using System.Diagnostics;
using UnityEngine;

namespace Facepunch.Clocks.Counters
{
	public struct SystemTimestamp
	{
		private const double ZeroDeductions = 0;

		private const double OneThousand = 1000;

		private const double ZeroElapsed = 0;

		private double startTime;

		private double endTime;

		private double deductSeconds;

		public TimeSpan Elapsed
		{
			get
			{
				if (double.IsNegativeInfinity(this.startTime))
				{
					return TimeSpan.Zero;
				}
				return TimeSpan.FromSeconds((double)((!double.IsPositiveInfinity(this.endTime) ? this.endTime : SystemTimestamp.TIME_SOURCE.NOW)) - this.deductSeconds - (double)this.startTime);
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
				if (double.IsNegativeInfinity(this.startTime))
				{
					return 0;
				}
				if (double.IsPositiveInfinity(this.endTime))
				{
					return (double)(SystemTimestamp.TIME_SOURCE.NOW - this.deductSeconds - this.startTime);
				}
				return (double)(this.endTime - this.deductSeconds - this.startTime);
			}
		}

		public bool IsRunning
		{
			get
			{
				return (!double.IsPositiveInfinity(this.endTime) ? false : !double.IsNegativeInfinity(this.startTime));
			}
		}

		public static SystemTimestamp Reset
		{
			get
			{
				SystemTimestamp systemTimestamp = new SystemTimestamp();
				systemTimestamp.deductSeconds = 0;
				systemTimestamp.endTime = Double.PositiveInfinity;
				systemTimestamp.startTime = Double.NegativeInfinity;
				return systemTimestamp;
			}
		}

		public static SystemTimestamp Restart
		{
			get
			{
				SystemTimestamp nOW = new SystemTimestamp();
				nOW.deductSeconds = 0;
				nOW.endTime = Double.PositiveInfinity;
				nOW.startTime = SystemTimestamp.TIME_SOURCE.NOW;
				return nOW;
			}
		}

		public void Start()
		{
			if (double.IsNegativeInfinity(this.startTime))
			{
				this.startTime = SystemTimestamp.TIME_SOURCE.NOW;
				this.deductSeconds = 0;
				this.endTime = Double.PositiveInfinity;
			}
			else if (!double.IsPositiveInfinity(this.endTime))
			{
				double num = this.endTime;
				this.endTime = Double.PositiveInfinity;
				SystemTimestamp nOW = this;
				nOW.deductSeconds = nOW.deductSeconds + ((double)SystemTimestamp.TIME_SOURCE.NOW - (double)num);
			}
		}

		public void Stop()
		{
			if (double.IsNegativeInfinity(this.startTime))
			{
				return;
			}
			if (double.IsPositiveInfinity(this.endTime))
			{
				this.endTime = SystemTimestamp.TIME_SOURCE.NOW;
			}
		}

		private static class TIME_SOURCE
		{
			private readonly static long ThenTimestamp;

			private readonly static long Frequency;

			private readonly static double ToSeconds;

			private readonly static bool IsHighResolution;

			public static double NOW
			{
				get
				{
					return (double)(Stopwatch.GetTimestamp() - SystemTimestamp.TIME_SOURCE.ThenTimestamp) * SystemTimestamp.TIME_SOURCE.ToSeconds;
				}
			}

			static TIME_SOURCE()
			{
				SystemTimestamp.TIME_SOURCE.ThenTimestamp = Stopwatch.GetTimestamp();
				SystemTimestamp.TIME_SOURCE.Frequency = Stopwatch.Frequency;
				SystemTimestamp.TIME_SOURCE.IsHighResolution = Stopwatch.IsHighResolution;
				SystemTimestamp.TIME_SOURCE.ToSeconds = (double)(new decimal(1) / SystemTimestamp.TIME_SOURCE.Frequency);
				string str = string.Format("SystemTimestampWatch settings={{IsHighResolution={0},Frequency={1},ToSecond={2}}}", SystemTimestamp.TIME_SOURCE.IsHighResolution, SystemTimestamp.TIME_SOURCE.Frequency, SystemTimestamp.TIME_SOURCE.ToSeconds);
				if (!SystemTimestamp.TIME_SOURCE.IsHighResolution)
				{
					UnityEngine.Debug.LogWarning(str);
				}
			}
		}
	}
}