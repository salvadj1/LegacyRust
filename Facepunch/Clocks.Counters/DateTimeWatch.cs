using System;
using System.Runtime.CompilerServices;

namespace Facepunch.Clocks.Counters
{
	public struct DateTimeWatch
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
				return TimeSpan.FromSeconds((double)((!double.IsPositiveInfinity(this.endTime) ? this.endTime : DateTimeWatch.TIME_SOURCE.NOW)) - this.deductSeconds - (double)this.startTime);
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
					return (double)(DateTimeWatch.TIME_SOURCE.NOW - this.deductSeconds - this.startTime);
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

		public static DateTimeWatch Reset
		{
			get
			{
				DateTimeWatch dateTimeWatch = new DateTimeWatch();
				dateTimeWatch.deductSeconds = 0;
				dateTimeWatch.endTime = Double.PositiveInfinity;
				dateTimeWatch.startTime = Double.NegativeInfinity;
				return dateTimeWatch;
			}
		}

		public static DateTimeWatch Restart
		{
			get
			{
				DateTimeWatch nOW = new DateTimeWatch();
				nOW.deductSeconds = 0;
				nOW.endTime = Double.PositiveInfinity;
				nOW.startTime = DateTimeWatch.TIME_SOURCE.NOW;
				return nOW;
			}
		}

		public void Start()
		{
			if (double.IsNegativeInfinity(this.startTime))
			{
				this.startTime = DateTimeWatch.TIME_SOURCE.NOW;
				this.deductSeconds = 0;
				this.endTime = Double.PositiveInfinity;
			}
			else if (!double.IsPositiveInfinity(this.endTime))
			{
				double num = this.endTime;
				this.endTime = Double.PositiveInfinity;
				DateTimeWatch nOW = this;
				nOW.deductSeconds = nOW.deductSeconds + ((double)DateTimeWatch.TIME_SOURCE.NOW - (double)num);
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
				this.endTime = DateTimeWatch.TIME_SOURCE.NOW;
			}
		}

		private static class TIME_SOURCE
		{
			[DecimalConstant(28, 0, 54, 902409669, 3735027712)]
			private readonly static decimal kTickToSecond;

			public readonly static DateTime Then;

			public readonly static long ThenTicks;

			public static double NOW
			{
				get
				{
					DateTime now = DateTime.Now;
					return (double)((now.Ticks - DateTimeWatch.TIME_SOURCE.ThenTicks) * new decimal(-559939584, 902409669, 54, false, 28));
				}
			}

			static TIME_SOURCE()
			{
				DateTimeWatch.TIME_SOURCE.kTickToSecond = new decimal(-559939584, 902409669, 54, false, 28);
				DateTimeWatch.TIME_SOURCE.Then = DateTime.Now;
				DateTimeWatch.TIME_SOURCE.ThenTicks = DateTimeWatch.TIME_SOURCE.Then.Ticks;
			}
		}
	}
}