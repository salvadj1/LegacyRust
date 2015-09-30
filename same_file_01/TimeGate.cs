using System;

public struct TimeGate
{
	[NonSerialized]
	private bool initialized;

	[NonSerialized]
	private long startTime;

	public bool behindOrAtTime
	{
		get
		{
			return (!this.initialized ? true : this.startTime >= TimeGate.timeSource);
		}
	}

	public bool behindTime
	{
		get
		{
			return (!this.initialized ? true : this.startTime > TimeGate.timeSource);
		}
	}

	public long elapsedMillis
	{
		get
		{
			return (!this.initialized ? (long)2147483647 : TimeGate.timeSource - this.startTime);
		}
		set
		{
			if (value != (long)2147483647)
			{
				this.startTime = TimeGate.timeSource - value;
				this.initialized = true;
			}
			else
			{
				this.initialized = false;
			}
		}
	}

	public double elapsedSeconds
	{
		get
		{
			return (!this.initialized ? Double.PositiveInfinity : (double)(TimeGate.timeSource - this.startTime) / 1000);
		}
		set
		{
			if (!double.IsPositiveInfinity(value))
			{
				this.startTime = TimeGate.timeSource - TimeGate.SecondsToMS(value);
				this.initialized = true;
			}
			else
			{
				this.initialized = false;
			}
		}
	}

	public bool passedOrAtTime
	{
		get
		{
			return (!this.initialized ? true : this.startTime <= TimeGate.timeSource);
		}
	}

	public bool passedTime
	{
		get
		{
			return (!this.initialized ? true : this.startTime < TimeGate.timeSource);
		}
	}

	public bool started
	{
		get
		{
			return this.initialized;
		}
	}

	public long timeInMillis
	{
		get
		{
			return (!this.initialized ? (long)0 : this.startTime);
		}
		set
		{
			this.startTime = value;
			this.initialized = true;
		}
	}

	public double timeInSeconds
	{
		get
		{
			return (!this.initialized ? 0 : (double)this.startTime / 1000);
		}
		set
		{
			this.startTime = TimeGate.SecondsToMS(value);
			this.initialized = true;
		}
	}

	private static long timeSource
	{
		get
		{
			return (long)NetCull.timeInMillis;
		}
	}

	public bool ElapsedMillis(long span)
	{
		return (span <= (long)0 || !this.initialized ? true : TimeGate.timeSource - this.startTime >= span);
	}

	public bool ElapsedSeconds(double seconds)
	{
		return (seconds <= 0 || !this.initialized ? true : TimeGate.timeSource - this.startTime >= TimeGate.SecondsToMS(seconds));
	}

	public bool FireMillis(long minimumElapsedTime)
	{
		return (minimumElapsedTime <= (long)0 ? true : this.RefireMillis(-minimumElapsedTime));
	}

	public static bool operator @false(TimeGate gate)
	{
		return gate.behindTime;
	}

	public static implicit operator TimeGate(double timeRemaining)
	{
		TimeGate mS = new TimeGate();
		mS.initialized = true;
		mS.startTime = TimeGate.SecondsToMS((double)TimeGate.timeSource / 1000 - timeRemaining);
		return mS;
	}

	public static implicit operator TimeGate(float timeRemaining)
	{
		return (double)timeRemaining;
	}

	public static implicit operator TimeGate(long timeRemaining)
	{
		TimeGate timeGate = new TimeGate();
		timeGate.initialized = true;
		timeGate.startTime = TimeGate.timeSource - timeRemaining;
		return timeGate;
	}

	public static implicit operator TimeGate(ulong timeRemaining)
	{
		TimeGate timeGate = new TimeGate();
		timeGate.initialized = true;
		timeGate.startTime = (long)(TimeGate.timeSource - timeRemaining);
		return timeGate;
	}

	public static implicit operator TimeGate(int timeRemaining)
	{
		TimeGate timeGate = new TimeGate();
		timeGate.initialized = true;
		timeGate.startTime = TimeGate.timeSource - (long)timeRemaining;
		return timeGate;
	}

	public static implicit operator TimeGate(uint timeRemaining)
	{
		TimeGate timeGate = new TimeGate();
		timeGate.initialized = true;
		timeGate.startTime = (long)(TimeGate.timeSource - (ulong)timeRemaining);
		return timeGate;
	}

	public static implicit operator TimeGate(short timeRemaining)
	{
		TimeGate timeGate = new TimeGate();
		timeGate.initialized = true;
		timeGate.startTime = TimeGate.timeSource - (long)timeRemaining;
		return timeGate;
	}

	public static implicit operator TimeGate(ushort timeRemaining)
	{
		TimeGate timeGate = new TimeGate();
		timeGate.initialized = true;
		timeGate.startTime = TimeGate.timeSource - (long)timeRemaining;
		return timeGate;
	}

	public static implicit operator TimeGate(byte timeRemaining)
	{
		TimeGate timeGate = new TimeGate();
		timeGate.initialized = true;
		timeGate.startTime = TimeGate.timeSource - (long)timeRemaining;
		return timeGate;
	}

	public static implicit operator TimeGate(sbyte timeRemaining)
	{
		TimeGate timeGate = new TimeGate();
		timeGate.initialized = true;
		timeGate.startTime = TimeGate.timeSource - (long)timeRemaining;
		return timeGate;
	}

	public static bool operator @true(TimeGate gate)
	{
		return gate.passedOrAtTime;
	}

	public bool RefireMillis(long intervalMS)
	{
		long num = TimeGate.timeSource;
		if (!this.initialized)
		{
			this.initialized = true;
			this.startTime = num;
			return true;
		}
		if (intervalMS == 0)
		{
			bool flag = num != this.startTime;
			this.startTime = num;
			return flag;
		}
		if (intervalMS < (long)0)
		{
			if (this.startTime - num > intervalMS)
			{
				return false;
			}
			this.startTime = num;
			return true;
		}
		if (num - this.startTime < intervalMS)
		{
			return false;
		}
		TimeGate timeGate = this;
		timeGate.startTime = timeGate.startTime + intervalMS;
		return true;
	}

	public bool RefireSeconds(double intervalSeconds)
	{
		return this.RefireMillis(TimeGate.SecondsToMS(intervalSeconds));
	}

	private static long SecondsToMS(double seconds)
	{
		return (long)Math.Floor(seconds * 1000);
	}
}