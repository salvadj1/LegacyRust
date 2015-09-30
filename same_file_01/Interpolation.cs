using System;

public static class Interpolation
{
	private const float kDefaultSendRateRatio = 1.5f;

	private const int kDefaultDelayMillis = 20;

	private const float kDefaultSendRate = 5f;

	private static double _ratio;

	private static ulong _totalDelayMillis;

	private static ulong _delayFromSendRateMillis;

	private static ulong _delayMillis;

	private static double _delaySeconds;

	private static double _totalDelaySeconds;

	private static double _deltaSeconds;

	private static double _delayFromSendRateSeconds;

	private static float _sendRate;

	public static Interpolation.TimingData @struct;

	public static ulong delayFromSendRateMillis
	{
		get
		{
			return Interpolation._delayFromSendRateMillis;
		}
	}

	public static double delayFromSendRateSeconds
	{
		get
		{
			return Interpolation._delayFromSendRateSeconds;
		}
	}

	public static float delayFromSendRateSecondsf
	{
		get
		{
			return (float)Interpolation._delayFromSendRateSeconds;
		}
	}

	public static ulong delayMillis
	{
		get
		{
			return Interpolation._delayMillis;
		}
		set
		{
			if (value != Interpolation._delayMillis)
			{
				Interpolation.BindTiming(value, Interpolation._ratio, Interpolation._sendRate);
			}
		}
	}

	public static double delaySeconds
	{
		get
		{
			return Interpolation._delaySeconds;
		}
		set
		{
			if (value >= 0.0005)
			{
				Interpolation.delayMillis = (ulong)Math.Round(value * 1000);
			}
			else
			{
				Interpolation.delayMillis = (ulong)0;
			}
		}
	}

	public static float delaySecondsf
	{
		get
		{
			return (float)Interpolation._delaySeconds;
		}
		set
		{
			Interpolation.delaySeconds = (double)value;
		}
	}

	public static double deltaSeconds
	{
		get
		{
			return Interpolation._deltaSeconds;
		}
	}

	public static float deltaSecondsf
	{
		get
		{
			return (float)Interpolation._deltaSeconds;
		}
	}

	public static double localTime
	{
		get
		{
			return NetCull.localTime + Interpolation._deltaSeconds;
		}
	}

	public static ulong localTimeInMillis
	{
		get
		{
			ulong num = NetCull.localTimeInMillis;
			if (num >= Interpolation._totalDelayMillis)
			{
				num = num - Interpolation._totalDelayMillis;
			}
			else
			{
				num = (ulong)0;
			}
			return num;
		}
	}

	public static float sendRate
	{
		get
		{
			return Interpolation._sendRate;
		}
		set
		{
			if (value != Interpolation._sendRate)
			{
				Interpolation.BindTiming(Interpolation._delayMillis, Interpolation._ratio, value);
			}
		}
	}

	public static double sendRateRatio
	{
		get
		{
			return Interpolation._ratio;
		}
		set
		{
			if (value != Interpolation._ratio)
			{
				Interpolation.BindTiming(Interpolation._delayMillis, value, Interpolation._sendRate);
			}
		}
	}

	public static float sendRateRatiof
	{
		get
		{
			return (float)Interpolation._ratio;
		}
		set
		{
			Interpolation.sendRateRatio = (double)value;
		}
	}

	public static double time
	{
		get
		{
			return NetCull.time + Interpolation._deltaSeconds;
		}
	}

	public static ulong timeInMillis
	{
		get
		{
			ulong num = NetCull.timeInMillis;
			if (num >= Interpolation._totalDelayMillis)
			{
				num = num - Interpolation._totalDelayMillis;
			}
			else
			{
				num = (ulong)0;
			}
			return num;
		}
	}

	public static ulong totalDelayMillis
	{
		get
		{
			return Interpolation._totalDelayMillis;
		}
	}

	public static double totalDelaySeconds
	{
		get
		{
			return Interpolation._totalDelaySeconds;
		}
	}

	public static float totalDelaySecondsf
	{
		get
		{
			return (float)Interpolation._totalDelaySeconds;
		}
	}

	static Interpolation()
	{
		Interpolation.BindTiming((ulong)20, 1.5, 5f);
	}

	public static ulong AddDelayToTimeStampMillis(ulong timestamp)
	{
		return timestamp + Interpolation._totalDelayMillis;
	}

	public static double AddDelayToTimeStampSeconds(double timeStamp)
	{
		return timeStamp + Interpolation._totalDelaySeconds;
	}

	public static void BindTiming(ulong? delayMillis, double? sendRateRatio, float? sendRate)
	{
		Interpolation.BindTiming((!delayMillis.HasValue ? Interpolation._delayMillis : delayMillis.Value), (!sendRateRatio.HasValue ? Interpolation._ratio : sendRateRatio.Value), (!sendRate.HasValue ? Interpolation._sendRate : sendRate.Value));
	}

	public static void BindTiming(ulong delayMillis, double sendRateRatio, float sendRate)
	{
		Interpolation._sendRate = sendRate;
		Interpolation._ratio = sendRateRatio;
		if (sendRate == 0f || sendRateRatio == 0 || sendRate < 0f != sendRateRatio < 0)
		{
			Interpolation._delayFromSendRateMillis = (ulong)0;
		}
		else
		{
			Interpolation._delayFromSendRateMillis = (ulong)Math.Ceiling(1000 * sendRateRatio / (double)sendRate);
		}
		Interpolation._delayMillis = delayMillis;
		Interpolation._totalDelayMillis = Interpolation._delayFromSendRateMillis + Interpolation._delayMillis;
		Interpolation._delaySeconds = (double)((float)Interpolation._delayMillis) * 0.001;
		Interpolation._delayFromSendRateSeconds = (double)((float)Interpolation._delayFromSendRateMillis) * 0.001;
		Interpolation._totalDelaySeconds = (double)((float)Interpolation._totalDelayMillis) * 0.001;
		Interpolation._deltaSeconds = -Interpolation._totalDelaySeconds;
		Interpolation.@struct = Interpolation.Capture();
	}

	public static void BindTiming()
	{
		Interpolation.BindTiming(Interpolation._delayMillis, Interpolation._ratio, Interpolation._sendRate);
	}

	public static void BindTimingNetCull(ulong delayMillis, double sendRateRatio)
	{
		Interpolation.BindTiming(delayMillis, sendRateRatio, NetCull.sendRate);
	}

	public static void BindTimingNetCull(ulong? delayMillis, double? sendRateRatio)
	{
		Interpolation.BindTiming((!delayMillis.HasValue ? Interpolation._delayMillis : delayMillis.Value), (!sendRateRatio.HasValue ? Interpolation._ratio : sendRateRatio.Value), NetCull.sendRate);
	}

	public static void BindTimingNetCull()
	{
		Interpolation.BindTiming(Interpolation._delayMillis, Interpolation._ratio, NetCull.sendRate);
	}

	public static Interpolation.TimingData Capture()
	{
		return new Interpolation.TimingData(Interpolation._ratio, Interpolation._deltaSeconds, Interpolation._totalDelaySeconds, Interpolation._delaySeconds, Interpolation._delayFromSendRateSeconds, Interpolation._totalDelayMillis, Interpolation._delayFromSendRateMillis, Interpolation._delayMillis, Interpolation._sendRate);
	}

	public static ulong GetInterpolationTimeMillis(ulong timestamp)
	{
		if (timestamp < Interpolation._totalDelayMillis)
		{
			return (ulong)0;
		}
		return timestamp - Interpolation._totalDelayMillis;
	}

	public static double GetInterpolationTimeSeconds(double timeStamp)
	{
		return timeStamp + Interpolation._deltaSeconds;
	}

	public struct TimingData
	{
		public readonly double sendRateRatio;

		public readonly double deltaSeconds;

		public readonly double totalDelaySeconds;

		public readonly double delaySeconds;

		public readonly double delayFromSendRateSeconds;

		public readonly float sendRateRatioF;

		public readonly float deltaSecondsF;

		public readonly float totalDelaySecondsF;

		public readonly float delaySecondsF;

		public readonly float delayFromSendRateSecondsF;

		public readonly ulong totalDelayMillis;

		public readonly ulong delayFromSendRateMillis;

		public readonly ulong delayMillis;

		public readonly float sendRate;

		public TimingData(double sendRateRatio, double deltaSeconds, double totalDelaySeconds, double delaySeconds, double delayFromSendRateSeconds, ulong totalDelayMillis, ulong delayFromSendRateMillis, ulong delayMillis, float sendRate)
		{
			this.sendRateRatio = sendRateRatio;
			this.deltaSeconds = deltaSeconds;
			this.totalDelaySeconds = totalDelaySeconds;
			this.delaySeconds = delaySeconds;
			this.delayFromSendRateSeconds = delayFromSendRateSeconds;
			this.totalDelayMillis = totalDelayMillis;
			this.delayFromSendRateMillis = delayFromSendRateMillis;
			this.delayMillis = delayMillis;
			this.sendRate = sendRate;
			this.sendRateRatioF = (float)sendRateRatio;
			this.deltaSecondsF = (float)deltaSeconds;
			this.totalDelaySecondsF = (float)totalDelaySeconds;
			this.delaySecondsF = (float)delaySeconds;
			this.delayFromSendRateSecondsF = (float)delayFromSendRateSeconds;
		}
	}
}