using System;

namespace POSIX
{
	public static class Time
	{
		private readonly static DateTime epoch;

		public static double NowSeconds
		{
			get
			{
				return DateTime.UtcNow.Subtract(Time.epoch).TotalSeconds;
			}
		}

		public static TimeSpan NowSpan
		{
			get
			{
				return DateTime.UtcNow.Subtract(Time.epoch);
			}
		}

		public static int NowStamp
		{
			get
			{
				double totalSeconds = DateTime.UtcNow.Subtract(Time.epoch).TotalSeconds;
				int num = (int)totalSeconds;
				if ((double)num > totalSeconds)
				{
					num--;
				}
				return num;
			}
		}

		static Time()
		{
			Time.epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		}

		public static TimeSpan Elapsed(int timeStampStart, int timeStampEnd)
		{
			return TimeSpan.FromSeconds((double)(timeStampEnd - timeStampStart));
		}

		public static TimeSpan Elapsed(TimeSpan sinceEpochStart, TimeSpan sinceEpochEnd)
		{
			return sinceEpochEnd.Subtract(sinceEpochStart);
		}

		public static TimeSpan Elapsed(DateTime dateTimeStart, DateTime dateTimeEnd)
		{
			return dateTimeEnd.ToUniversalTime().Subtract(dateTimeStart.ToUniversalTime());
		}

		public static double ElapsedSeconds(int timeStampStart, int timeStampEnd)
		{
			return TimeSpan.FromSeconds((double)(timeStampEnd - timeStampStart)).TotalSeconds;
		}

		public static double ElapsedSeconds(TimeSpan sinceEpochStart, TimeSpan sinceEpochEnd)
		{
			return sinceEpochEnd.Subtract(sinceEpochStart).TotalSeconds;
		}

		public static double ElapsedSeconds(DateTime dateTimeStart, DateTime dateTimeEnd)
		{
			DateTime universalTime = dateTimeEnd.ToUniversalTime();
			return universalTime.Subtract(dateTimeStart.ToUniversalTime()).TotalSeconds;
		}

		public static double ElapsedSecondsSince(int timeStamp)
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = Time.epoch;
			TimeSpan timeSpan = utcNow.Subtract(dateTime.AddSeconds((double)timeStamp));
			return timeSpan.TotalSeconds;
		}

		public static double ElapsedSecondsSince(TimeSpan sinceEpoch)
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = Time.epoch;
			return utcNow.Subtract(dateTime.Add(sinceEpoch)).TotalSeconds;
		}

		public static double ElapsedSecondsSince(DateTime dateTime)
		{
			return DateTime.UtcNow.Subtract(dateTime.ToUniversalTime()).TotalSeconds;
		}

		public static TimeSpan ElapsedSince(int timeStamp)
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = Time.epoch;
			return utcNow.Subtract(dateTime.AddSeconds((double)timeStamp));
		}

		public static TimeSpan ElapsedSince(TimeSpan sinceEpoch)
		{
			DateTime utcNow = DateTime.UtcNow;
			return utcNow.Subtract(Time.epoch.Add(sinceEpoch));
		}

		public static TimeSpan ElapsedSince(DateTime dateTime)
		{
			return DateTime.UtcNow.Subtract(dateTime.ToUniversalTime());
		}

		public static int ElapsedStamp(int timeStampStart, int timeStampEnd)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)(timeStampEnd - timeStampStart));
			double totalSeconds = timeSpan.TotalSeconds;
			int num = (int)totalSeconds;
			if ((double)num > totalSeconds)
			{
				num--;
			}
			return num;
		}

		public static int ElapsedStamp(TimeSpan sinceEpochStart, TimeSpan sinceEpochEnd)
		{
			double totalSeconds = sinceEpochEnd.Subtract(sinceEpochStart).TotalSeconds;
			int num = (int)totalSeconds;
			if ((double)num > totalSeconds)
			{
				num--;
			}
			return num;
		}

		public static int ElapsedStamp(DateTime dateTimeStart, DateTime dateTimeEnd)
		{
			DateTime universalTime = dateTimeEnd.ToUniversalTime();
			double totalSeconds = universalTime.Subtract(dateTimeStart.ToUniversalTime()).TotalSeconds;
			int num = (int)totalSeconds;
			if ((double)num > totalSeconds)
			{
				num--;
			}
			return num;
		}

		public static int ElapsedStampSince(int timeStamp)
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = Time.epoch;
			TimeSpan timeSpan = utcNow.Subtract(dateTime.AddSeconds((double)timeStamp));
			double totalSeconds = timeSpan.TotalSeconds;
			int num = (int)totalSeconds;
			if ((double)num > totalSeconds)
			{
				num--;
			}
			return num;
		}

		public static int ElapsedStampSince(TimeSpan sinceEpoch)
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = Time.epoch;
			TimeSpan timeSpan = utcNow.Subtract(dateTime.Add(sinceEpoch));
			double totalSeconds = timeSpan.TotalSeconds;
			int num = (int)totalSeconds;
			if ((double)num > totalSeconds)
			{
				num--;
			}
			return num;
		}

		public static int ElapsedStampSince(DateTime dateTime)
		{
			DateTime utcNow = DateTime.UtcNow;
			double totalSeconds = utcNow.Subtract(dateTime.ToUniversalTime()).TotalSeconds;
			int num = (int)totalSeconds;
			if ((double)num > totalSeconds)
			{
				num--;
			}
			return num;
		}
	}
}