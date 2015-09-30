using System;
using uLink;

public struct NetClockTester
{
	public NetClockTester.Stamping Send;

	public NetClockTester.Stamping Receive;

	[NonSerialized]
	public ulong Count;

	public NetClockTester.Validity Results;

	public NetClockTester.ValidityFlags LastTestFlags;

	public bool Any
	{
		get
		{
			return this.Count > (long)0;
		}
	}

	public bool Empty
	{
		get
		{
			return this.Count == (long)0;
		}
	}

	public static NetClockTester Reset
	{
		get
		{
			return new NetClockTester();
		}
	}

	private static ulong Add(ulong a, long b)
	{
		if (b >= (long)0)
		{
			return a + b;
		}
		if (a <= -b)
		{
			return (ulong)0;
		}
		return a - -b;
	}

	private static long Subtract(ulong a, ulong b)
	{
		if (a > b)
		{
			return (long)(a - b);
		}
		if (a >= b)
		{
			return (long)0;
		}
		return (long)(-(b - a));
	}

	public static NetClockTester.ValidityFlags TestValidity(ref NetClockTester test, ref uLink.NetworkMessageInfo info, double intervalSec, NetClockTester.ValidityFlags testFor)
	{
		return NetClockTester.TestValidity(ref test, ref info, (long)Math.Floor(intervalSec * 1000), testFor);
	}

	public static NetClockTester.ValidityFlags TestValidity(ref NetClockTester test, ref uLink.NetworkMessageInfo info, long intervalMS, NetClockTester.ValidityFlags testFor)
	{
		NetClockTester.ValidityFlags validityFlag = NetClockTester.TestValidity(ref test, info.timestampInMillis, intervalMS);
		test.Results.Add(validityFlag & testFor);
		return validityFlag;
	}

	private static NetClockTester.ValidityFlags TestValidity(ref NetClockTester test, ulong timeStamp, long minimalSendRateMS)
	{
		NetClockTester.ValidityFlags validityFlag;
		ulong num = NetCull.timeInMillis;
		if (num >= timeStamp)
		{
			validityFlag = (NetClockTester.ValidityFlags)0;
		}
		else
		{
			validityFlag = NetClockTester.ValidityFlags.AheadOfServerTime;
		}
		NetClockTester.ValidityFlags validityFlag1 = validityFlag;
		if (test.Count <= (long)0)
		{
			long num1 = (long)0;
			ulong num2 = (ulong)num1;
			test.Receive.Sum = (ulong)num1;
			test.Send.Sum = num2;
			ulong num3 = timeStamp;
			num2 = num3;
			test.Send.First = num3;
			test.Send.Last = num2;
			ulong num4 = num;
			num2 = num4;
			test.Receive.First = num4;
			test.Receive.Last = num2;
			test.Count = (ulong)1;
			return validityFlag1;
		}
		long num5 = NetClockTester.Subtract(timeStamp, test.Send.Last);
		long num6 = NetClockTester.Subtract(num, test.Receive.Last);
		test.Send.Sum = NetClockTester.Add(test.Send.Sum, num5);
		test.Receive.Sum = NetClockTester.Add(test.Receive.Sum, num6);
		test.Count = test.Count + (long)1;
		test.Send.Last = timeStamp;
		test.Receive.Last = num;
		if (num5 < minimalSendRateMS)
		{
			validityFlag1 = validityFlag1 | NetClockTester.ValidityFlags.TooFrequent;
		}
		long num7 = NetClockTester.Subtract(test.Send.Last, test.Send.First);
		long num8 = NetClockTester.Subtract(test.Receive.Last, test.Receive.First);
		if (test.Count >= (long)5)
		{
			if (num7 > num8 * (long)2)
			{
				validityFlag1 = validityFlag1 | NetClockTester.ValidityFlags.OverTimed;
			}
		}
		else if (test.Count >= (long)3 && num7 > num8 * (long)4)
		{
			validityFlag1 = validityFlag1 | NetClockTester.ValidityFlags.OverTimed;
		}
		NetClockTester.ValidityFlags lastTestFlags = test.LastTestFlags;
		test.LastTestFlags = validityFlag1;
		if ((validityFlag1 & NetClockTester.ValidityFlags.TooFrequent) == NetClockTester.ValidityFlags.TooFrequent && (lastTestFlags & NetClockTester.ValidityFlags.TooFrequent) != NetClockTester.ValidityFlags.TooFrequent)
		{
			validityFlag1 = validityFlag1 & (NetClockTester.ValidityFlags.Valid | NetClockTester.ValidityFlags.OverTimed | NetClockTester.ValidityFlags.AheadOfServerTime);
			test.Count = (ulong)1;
			test.Send.First = test.Send.Last;
			test.Send.Sum = (ulong)0;
			if (num6 <= (long)0)
			{
				test.Receive.First = test.Receive.Last;
				test.Receive.Sum = (ulong)0;
			}
			else
			{
				test.Receive.First = (ulong)NetClockTester.Subtract(test.Receive.Last, (ulong)num6);
				test.Receive.Sum = (ulong)num6;
			}
		}
		return ((int)validityFlag1 != 0 ? validityFlag1 : NetClockTester.ValidityFlags.Valid);
	}

	public struct Stamping
	{
		public ulong Last;

		public ulong First;

		public ulong Sum;

		public long Duration
		{
			get
			{
				return NetClockTester.Subtract(this.Last, this.First);
			}
		}

		public long Variance
		{
			get
			{
				return (long)(this.Sum - NetClockTester.Subtract(this.Last, this.First));
			}
		}
	}

	public struct Validity
	{
		public uint TooFrequent;

		public uint OverTimed;

		public uint AheadOfServerTime;

		public uint Valid;

		public NetClockTester.ValidityFlags Flags
		{
			get
			{
				if (this.TooFrequent > 0)
				{
					if (this.OverTimed > 0)
					{
						if (this.AheadOfServerTime > 0)
						{
							return NetClockTester.ValidityFlags.TooFrequent | NetClockTester.ValidityFlags.OverTimed | NetClockTester.ValidityFlags.AheadOfServerTime;
						}
						return NetClockTester.ValidityFlags.TooFrequent | NetClockTester.ValidityFlags.OverTimed;
					}
					if (this.AheadOfServerTime > 0)
					{
						return NetClockTester.ValidityFlags.TooFrequent | NetClockTester.ValidityFlags.AheadOfServerTime;
					}
					return NetClockTester.ValidityFlags.TooFrequent;
				}
				if (this.OverTimed > 0)
				{
					if (this.AheadOfServerTime > 0)
					{
						return NetClockTester.ValidityFlags.OverTimed | NetClockTester.ValidityFlags.AheadOfServerTime;
					}
					return NetClockTester.ValidityFlags.OverTimed;
				}
				if (this.AheadOfServerTime > 0)
				{
					return NetClockTester.ValidityFlags.AheadOfServerTime;
				}
				if (this.Valid > 0)
				{
					return NetClockTester.ValidityFlags.Valid;
				}
				return 0;
			}
		}

		public void Add(NetClockTester.ValidityFlags vf)
		{
			switch (vf & (NetClockTester.ValidityFlags.TooFrequent | NetClockTester.ValidityFlags.OverTimed | NetClockTester.ValidityFlags.AheadOfServerTime))
			{
				case 0:
				{
					if ((vf & NetClockTester.ValidityFlags.Valid) == NetClockTester.ValidityFlags.Valid)
					{
						NetClockTester.Validity valid = this;
						valid.Valid = valid.Valid + 1;
					}
					return;
				}
				case NetClockTester.ValidityFlags.Valid:
				case NetClockTester.ValidityFlags.Valid | NetClockTester.ValidityFlags.TooFrequent:
				case NetClockTester.ValidityFlags.Valid | NetClockTester.ValidityFlags.OverTimed:
				case NetClockTester.ValidityFlags.Valid | NetClockTester.ValidityFlags.TooFrequent | NetClockTester.ValidityFlags.OverTimed:
				case NetClockTester.ValidityFlags.Valid | NetClockTester.ValidityFlags.AheadOfServerTime:
				case NetClockTester.ValidityFlags.Valid | NetClockTester.ValidityFlags.TooFrequent | NetClockTester.ValidityFlags.AheadOfServerTime:
				case NetClockTester.ValidityFlags.Valid | NetClockTester.ValidityFlags.OverTimed | NetClockTester.ValidityFlags.AheadOfServerTime:
				{
					return;
				}
				case NetClockTester.ValidityFlags.TooFrequent:
				{
					NetClockTester.Validity tooFrequent = this;
					tooFrequent.TooFrequent = tooFrequent.TooFrequent + 1;
					return;
				}
				case NetClockTester.ValidityFlags.OverTimed:
				{
					NetClockTester.Validity overTimed = this;
					overTimed.OverTimed = overTimed.OverTimed + 1;
					return;
				}
				case NetClockTester.ValidityFlags.TooFrequent | NetClockTester.ValidityFlags.OverTimed:
				{
					NetClockTester.Validity validity = this;
					validity.OverTimed = validity.OverTimed + 1;
					NetClockTester.Validity tooFrequent1 = this;
					tooFrequent1.TooFrequent = tooFrequent1.TooFrequent + 1;
					return;
				}
				case NetClockTester.ValidityFlags.AheadOfServerTime:
				{
					NetClockTester.Validity aheadOfServerTime = this;
					aheadOfServerTime.AheadOfServerTime = aheadOfServerTime.AheadOfServerTime + 1;
					return;
				}
				case NetClockTester.ValidityFlags.TooFrequent | NetClockTester.ValidityFlags.AheadOfServerTime:
				{
					NetClockTester.Validity aheadOfServerTime1 = this;
					aheadOfServerTime1.AheadOfServerTime = aheadOfServerTime1.AheadOfServerTime + 1;
					NetClockTester.Validity validity1 = this;
					validity1.TooFrequent = validity1.TooFrequent + 1;
					return;
				}
				case NetClockTester.ValidityFlags.OverTimed | NetClockTester.ValidityFlags.AheadOfServerTime:
				{
					NetClockTester.Validity aheadOfServerTime2 = this;
					aheadOfServerTime2.AheadOfServerTime = aheadOfServerTime2.AheadOfServerTime + 1;
					NetClockTester.Validity overTimed1 = this;
					overTimed1.OverTimed = overTimed1.OverTimed + 1;
					return;
				}
				case NetClockTester.ValidityFlags.TooFrequent | NetClockTester.ValidityFlags.OverTimed | NetClockTester.ValidityFlags.AheadOfServerTime:
				{
					NetClockTester.Validity validity2 = this;
					validity2.AheadOfServerTime = validity2.AheadOfServerTime + 1;
					NetClockTester.Validity overTimed2 = this;
					overTimed2.OverTimed = overTimed2.OverTimed + 1;
					NetClockTester.Validity tooFrequent2 = this;
					tooFrequent2.TooFrequent = tooFrequent2.TooFrequent + 1;
					return;
				}
				default:
				{
					return;
				}
			}
		}
	}

	[Flags]
	public enum ValidityFlags
	{
		Valid = 1,
		TooFrequent = 2,
		OverTimed = 4,
		AheadOfServerTime = 8
	}
}