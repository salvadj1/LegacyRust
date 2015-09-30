using System;
using UnityEngine;

namespace Facepunch.Clocks.Counters.Unity
{
	public struct NetworkTime
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
				return TimeSpan.FromSeconds((double)((!double.IsPositiveInfinity(this.endTime) ? this.endTime : NetworkTime.TIME_SOURCE.NOW)) - this.deductSeconds - (double)this.startTime);
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
					return (double)(NetworkTime.TIME_SOURCE.NOW - this.deductSeconds - this.startTime);
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

		public static NetworkTime Reset
		{
			get
			{
				NetworkTime networkTime = new NetworkTime();
				networkTime.deductSeconds = 0;
				networkTime.endTime = Double.PositiveInfinity;
				networkTime.startTime = Double.NegativeInfinity;
				return networkTime;
			}
		}

		public static NetworkTime Restart
		{
			get
			{
				NetworkTime nOW = new NetworkTime();
				nOW.deductSeconds = 0;
				nOW.endTime = Double.PositiveInfinity;
				nOW.startTime = NetworkTime.TIME_SOURCE.NOW;
				return nOW;
			}
		}

		public void Start()
		{
			if (double.IsNegativeInfinity(this.startTime))
			{
				this.startTime = NetworkTime.TIME_SOURCE.NOW;
				this.deductSeconds = 0;
				this.endTime = Double.PositiveInfinity;
			}
			else if (!double.IsPositiveInfinity(this.endTime))
			{
				double num = this.endTime;
				this.endTime = Double.PositiveInfinity;
				NetworkTime nOW = this;
				nOW.deductSeconds = nOW.deductSeconds + ((double)NetworkTime.TIME_SOURCE.NOW - (double)num);
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
				this.endTime = NetworkTime.TIME_SOURCE.NOW;
			}
		}

		private static class TIME_SOURCE
		{
			public static double NOW
			{
				get
				{
					return Network.time;
				}
			}
		}
	}
}