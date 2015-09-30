using System;

public struct TimeStringFormatter
{
	public const string kArgumentTime = "<ꪻ뮪>";

	private const string kArgumentTimeReplacement = "{0}";

	public const string kPeriod = ".";

	public readonly string aDay;

	public readonly string days;

	public readonly string aHour;

	public readonly string hours;

	public readonly string aMinute;

	public readonly string minutes;

	public readonly string aSecond;

	public readonly string seconds;

	public readonly string lessThanASecond;

	private TimeStringFormatter(string aDay, string days, string aHour, string hours, string aMinute, string minutes, string aSecond, string seconds, string lessThanASecond)
	{
		this.aDay = aDay;
		this.days = days;
		this.aHour = aHour;
		this.hours = hours;
		this.aMinute = aMinute;
		this.minutes = minutes;
		this.aSecond = aSecond;
		this.seconds = seconds;
		this.lessThanASecond = lessThanASecond;
	}

	public static TimeStringFormatter Define(TimeStringFormatter.Qualifier qualifier)
	{
		return new TimeStringFormatter(TimeStringFormatter.Merge(qualifier.aDay), TimeStringFormatter.Merge(qualifier.days), TimeStringFormatter.Merge(qualifier.aHour), TimeStringFormatter.Merge(qualifier.hours), TimeStringFormatter.Merge(qualifier.aMinute), TimeStringFormatter.Merge(qualifier.minutes), TimeStringFormatter.Merge(qualifier.aSecond), TimeStringFormatter.Merge(qualifier.seconds), TimeStringFormatter.Merge(qualifier.lessThanASecond));
	}

	public static TimeStringFormatter Define(string prefix, TimeStringFormatter.Qualifier qualifier)
	{
		if (string.IsNullOrEmpty(prefix))
		{
			return TimeStringFormatter.Define(qualifier);
		}
		return new TimeStringFormatter(TimeStringFormatter.Merge(prefix, qualifier.aDay), TimeStringFormatter.Merge(prefix, qualifier.days), TimeStringFormatter.Merge(prefix, qualifier.aHour), TimeStringFormatter.Merge(prefix, qualifier.hours), TimeStringFormatter.Merge(prefix, qualifier.aMinute), TimeStringFormatter.Merge(prefix, qualifier.minutes), TimeStringFormatter.Merge(prefix, qualifier.aSecond), TimeStringFormatter.Merge(prefix, qualifier.seconds), TimeStringFormatter.Merge(prefix, qualifier.lessThanASecond));
	}

	public static TimeStringFormatter Define(TimeStringFormatter.Qualifier qualifier, string suffix)
	{
		if (string.IsNullOrEmpty(suffix))
		{
			return TimeStringFormatter.Define(qualifier);
		}
		return new TimeStringFormatter(TimeStringFormatter.Merge(qualifier.aDay, suffix), TimeStringFormatter.Merge(qualifier.days, suffix), TimeStringFormatter.Merge(qualifier.aHour, suffix), TimeStringFormatter.Merge(qualifier.hours, suffix), TimeStringFormatter.Merge(qualifier.aMinute, suffix), TimeStringFormatter.Merge(qualifier.minutes, suffix), TimeStringFormatter.Merge(qualifier.aSecond, suffix), TimeStringFormatter.Merge(qualifier.seconds, suffix), TimeStringFormatter.Merge(qualifier.lessThanASecond, suffix));
	}

	public static TimeStringFormatter Define(string prefix, TimeStringFormatter.Qualifier qualifier, string suffix)
	{
		if (string.IsNullOrEmpty(suffix))
		{
			if (string.IsNullOrEmpty(prefix))
			{
				return TimeStringFormatter.Define(qualifier);
			}
			return TimeStringFormatter.Define(prefix, qualifier);
		}
		if (string.IsNullOrEmpty(prefix))
		{
			return TimeStringFormatter.Define(qualifier, suffix);
		}
		return new TimeStringFormatter(TimeStringFormatter.Merge(prefix, qualifier.aDay, suffix), TimeStringFormatter.Merge(prefix, qualifier.days, suffix), TimeStringFormatter.Merge(prefix, qualifier.aHour, suffix), TimeStringFormatter.Merge(prefix, qualifier.hours, suffix), TimeStringFormatter.Merge(prefix, qualifier.aMinute, suffix), TimeStringFormatter.Merge(prefix, qualifier.minutes, suffix), TimeStringFormatter.Merge(prefix, qualifier.aSecond, suffix), TimeStringFormatter.Merge(prefix, qualifier.seconds, suffix), TimeStringFormatter.Merge(prefix, qualifier.lessThanASecond, suffix));
	}

	public static TimeStringFormatter Define(TimeStringFormatter formatter, string lessThanASecond)
	{
		if (!object.ReferenceEquals(lessThanASecond, null))
		{
			formatter = new TimeStringFormatter(formatter.aDay, formatter.days, formatter.aHour, formatter.hours, formatter.aMinute, formatter.minutes, formatter.aSecond, formatter.seconds, TimeStringFormatter.Merge(lessThanASecond));
		}
		return formatter;
	}

	public static TimeStringFormatter Define(string prefix, TimeStringFormatter.Qualifier qualifier, string suffix, string lessThanASecond)
	{
		return TimeStringFormatter.Define(TimeStringFormatter.Define(prefix, qualifier, suffix), lessThanASecond);
	}

	private static string DoMerge(string value)
	{
		return value.Replace("{", "{{").Replace("}", "}}").Replace("<ꪻ뮪>", "{0}");
	}

	public string GetFormattingString(TimeSpan timePassed)
	{
		return this.GetFormattingString(timePassed, TimeStringFormatter.Rounding.Floor);
	}

	public string GetFormattingString(TimeSpan timePassed, TimeStringFormatter.Rounding rounding)
	{
		string str;
		object obj;
		string str1;
		int num = 2;
		int num1 = num;
		double num2 = 1;
		double num3 = num2;
		double num4 = TimeStringFormatter.Round(timePassed.TotalSeconds, rounding, num, num2);
		if (num4 <= 0)
		{
			str = this.lessThanASecond;
		}
		else if (num4 == 1)
		{
			str = this.aSecond;
		}
		else if (num4 >= 60)
		{
			int num5 = 2;
			num1 = num5;
			double num6 = 0.6;
			num3 = num6;
			double num7 = TimeStringFormatter.Round(timePassed.TotalMinutes, rounding, num5, num6);
			num4 = num7;
			if (num7 == 1)
			{
				str = this.aMinute;
			}
			else if (num4 >= 60)
			{
				int num8 = 2;
				num1 = num8;
				double num9 = 1;
				num3 = num9;
				double num10 = TimeStringFormatter.Round(timePassed.TotalHours, rounding, num8, num9);
				num4 = num10;
				if (num10 == 1)
				{
					str = this.aHour;
				}
				else if (num4 >= 24)
				{
					int num11 = 2;
					num1 = num11;
					double num12 = 0.24;
					num3 = num12;
					double num13 = TimeStringFormatter.Round(timePassed.TotalDays, rounding, num11, num12);
					num4 = num13;
					str = (num13 != 1 ? this.days : this.aDay);
				}
				else
				{
					str = this.hours;
				}
			}
			else
			{
				str = this.minutes;
			}
		}
		else
		{
			str = this.seconds;
		}
		if (rounding == TimeStringFormatter.Rounding.RoundedDecimal || rounding == TimeStringFormatter.Rounding.FancyDecimal || rounding == TimeStringFormatter.Rounding.RoundedFancyDecimal)
		{
			if (rounding == TimeStringFormatter.Rounding.RoundedDecimal || rounding == TimeStringFormatter.Rounding.RoundedFancyDecimal)
			{
				if (num1 != 2)
				{
					throw new NotSupportedException("We gotta add support for that");
				}
				str1 = num4.ToString("0.00");
			}
			else
			{
				str1 = num4.ToString();
			}
			obj = (rounding == TimeStringFormatter.Rounding.FancyDecimal || rounding == TimeStringFormatter.Rounding.RoundedFancyDecimal ? str1.Replace('.', ':') : str1);
		}
		else
		{
			obj = (rounding == TimeStringFormatter.Rounding.Decimal || double.IsNaN(num4) || double.IsInfinity(num4) ? num4 : (int)num4);
		}
		return string.Format(str, obj);
	}

	private static string Merge(string prefix)
	{
		return TimeStringFormatter.DoMerge(prefix ?? string.Empty);
	}

	private static string Merge(string prefix, string qualifier)
	{
		return TimeStringFormatter.DoMerge(string.Concat(prefix ?? string.Empty, qualifier ?? string.Empty));
	}

	private static string Merge(string prefix, string qualifier, string suffix)
	{
		return TimeStringFormatter.DoMerge(string.Concat(prefix ?? string.Empty, qualifier ?? string.Empty, suffix ?? string.Empty));
	}

	private static double Round(double total, TimeStringFormatter.Rounding rounding, int decimalPlaces, double fancyUnits)
	{
		if (total <= 0)
		{
			return 0;
		}
		switch (rounding)
		{
			case TimeStringFormatter.Rounding.Floor:
			{
				return Math.Floor(total);
			}
			case TimeStringFormatter.Rounding.Ceiling:
			{
				return Math.Ceiling(total);
			}
			case TimeStringFormatter.Rounding.Round:
			{
				return Math.Round(total);
			}
			case TimeStringFormatter.Rounding.Decimal:
			{
				fancyUnits = 1;
				decimalPlaces = 0;
				break;
			}
			case TimeStringFormatter.Rounding.RoundedDecimal:
			{
				fancyUnits = 1;
				break;
			}
			case TimeStringFormatter.Rounding.FancyDecimal:
			{
				decimalPlaces = 0;
				break;
			}
		}
		if (decimalPlaces == 0)
		{
			return total;
		}
		double num = Math.Floor(total);
		return num + Math.Floor((total - num) * fancyUnits * ((double)decimalPlaces * 10)) / (10 * (double)decimalPlaces);
	}

	public static class Ago
	{
		public const string kSuffix = " ago";

		public const string aDay = " a day ago";

		public const string days = " <ꪻ뮪> days ago";

		public const string aHour = " an hour ago";

		public const string hours = " <ꪻ뮪> hours ago";

		public const string aMinute = " a minute ago";

		public const string minutes = " <ꪻ뮪> minutes ago";

		public const string aSecond = " a second ago";

		public const string seconds = " <ꪻ뮪> seconds ago";

		public const string lessThanASecond = "";

		public readonly static TimeStringFormatter.Qualifier Qualifier;

		static Ago()
		{
			TimeStringFormatter.Ago.Qualifier = new TimeStringFormatter.Qualifier(" a day ago", " <ꪻ뮪> days ago", " an hour ago", " <ꪻ뮪> hours ago", " a minute ago", " <ꪻ뮪> minutes ago", " a second ago", " <ꪻ뮪> seconds ago", string.Empty);
		}

		public static class Period
		{
			public const string kSuffix = ".";

			public const string aDay = " a day ago.";

			public const string days = " <ꪻ뮪> days ago.";

			public const string aHour = " an hour ago.";

			public const string hours = " <ꪻ뮪> hours ago.";

			public const string aMinute = " a minute ago.";

			public const string minutes = " <ꪻ뮪> minutes ago.";

			public const string aSecond = " a second ago.";

			public const string seconds = " <ꪻ뮪> seconds ago.";

			public const string lessThanASecond = ".";

			public readonly static TimeStringFormatter.Qualifier Qualifier;

			static Period()
			{
				TimeStringFormatter.Ago.Period.Qualifier = new TimeStringFormatter.Qualifier(" a day ago.", " <ꪻ뮪> days ago.", " an hour ago.", " <ꪻ뮪> hours ago.", " a minute ago.", " <ꪻ뮪> minutes ago.", " a second ago.", " <ꪻ뮪> seconds ago.", ".");
			}
		}
	}

	public static class For
	{
		public const string kPrefix = " for";

		public const string aDay = " for a day";

		public const string days = " for <ꪻ뮪> days";

		public const string aHour = " for an hour";

		public const string hours = " for <ꪻ뮪> hours";

		public const string aMinute = " for a minute";

		public const string minutes = " for <ꪻ뮪> minutes";

		public const string aSecond = " for a second";

		public const string seconds = " for <ꪻ뮪> seconds";

		public const string lessThanASecond = "";

		public readonly static TimeStringFormatter.Qualifier Qualifier;

		static For()
		{
			TimeStringFormatter.For.Qualifier = new TimeStringFormatter.Qualifier(" for a day", " for <ꪻ뮪> days", " for an hour", " for <ꪻ뮪> hours", " for a minute", " for <ꪻ뮪> minutes", " for a second", " for <ꪻ뮪> seconds", string.Empty);
		}

		public static class Period
		{
			public const string kSuffix = ".";

			public const string aDay = " for a day.";

			public const string days = " for <ꪻ뮪> days.";

			public const string aHour = " for an hour.";

			public const string hours = " for <ꪻ뮪> hours.";

			public const string aMinute = " for a minute.";

			public const string minutes = " for <ꪻ뮪> minutes.";

			public const string aSecond = " for a second.";

			public const string seconds = " for <ꪻ뮪> seconds.";

			public const string lessThanASecond = ".";

			public readonly static TimeStringFormatter.Qualifier Qualifier;

			static Period()
			{
				TimeStringFormatter.For.Period.Qualifier = new TimeStringFormatter.Qualifier(" for a day.", " for <ꪻ뮪> days.", " for an hour.", " for <ꪻ뮪> hours.", " for a minute.", " for <ꪻ뮪> minutes.", " for a second.", " for <ꪻ뮪> seconds.", ".");
			}
		}
	}

	public struct Qualifier
	{
		public readonly string aDay;

		public readonly string days;

		public readonly string aHour;

		public readonly string hours;

		public readonly string aMinute;

		public readonly string minutes;

		public readonly string aSecond;

		public readonly string seconds;

		public readonly string lessThanASecond;

		public Qualifier(string aDay, string days, string aHour, string hours, string aMinute, string minutes, string aSecond, string seconds, string lessThanASecond)
		{
			this.aDay = aDay;
			this.days = days;
			this.aHour = aHour;
			this.hours = hours;
			this.aMinute = aMinute;
			this.minutes = minutes;
			this.aSecond = aSecond;
			this.seconds = seconds;
			this.lessThanASecond = lessThanASecond;
		}
	}

	public static class Quantity
	{
		public const string kPrefix = " ";

		public const string aDay = " a day";

		public const string days = " <ꪻ뮪> days";

		public const string aHour = " an hour";

		public const string hours = " <ꪻ뮪> hours";

		public const string aMinute = " a minute";

		public const string minutes = " <ꪻ뮪> minutes";

		public const string aSecond = " a second";

		public const string seconds = " <ꪻ뮪> seconds";

		public const string lessThanASecond = "";

		public readonly static TimeStringFormatter.Qualifier Qualifier;

		static Quantity()
		{
			TimeStringFormatter.Quantity.Qualifier = new TimeStringFormatter.Qualifier(" a day", " <ꪻ뮪> days", " an hour", " <ꪻ뮪> hours", " a minute", " <ꪻ뮪> minutes", " a second", " <ꪻ뮪> seconds", string.Empty);
		}

		public static class Period
		{
			public const string kSuffix = ".";

			public const string aDay = " a day.";

			public const string days = " <ꪻ뮪> days.";

			public const string aHour = " an hour.";

			public const string hours = " <ꪻ뮪> hours.";

			public const string aMinute = " a minute.";

			public const string minutes = " <ꪻ뮪> minutes.";

			public const string aSecond = " a second.";

			public const string seconds = " <ꪻ뮪> seconds.";

			public const string lessThanASecond = ".";

			public readonly static TimeStringFormatter.Qualifier Qualifier;

			static Period()
			{
				TimeStringFormatter.Quantity.Period.Qualifier = new TimeStringFormatter.Qualifier(" a day.", " <ꪻ뮪> days.", " an hour.", " <ꪻ뮪> hours.", " a minute.", " <ꪻ뮪> minutes.", " a second.", " <ꪻ뮪> seconds.", ".");
			}
		}
	}

	public enum Rounding
	{
		Floor,
		Ceiling,
		Round,
		Decimal,
		RoundedDecimal,
		FancyDecimal,
		RoundedFancyDecimal
	}

	public static class SinceAgo
	{
		public const string kPrefix = " since";

		public const string aDay = " since a day ago";

		public const string days = " since <ꪻ뮪> days ago";

		public const string aHour = " since an hour ago";

		public const string hours = " since <ꪻ뮪> hours ago";

		public const string aMinute = " since a minute ago";

		public const string minutes = " since <ꪻ뮪> minutes ago";

		public const string aSecond = " since a second ago";

		public const string seconds = " since <ꪻ뮪> seconds ago";

		public const string lessThanASecond = "";

		public readonly static TimeStringFormatter.Qualifier Qualifier;

		static SinceAgo()
		{
			TimeStringFormatter.SinceAgo.Qualifier = new TimeStringFormatter.Qualifier(" since a day ago", " since <ꪻ뮪> days ago", " since an hour ago", " since <ꪻ뮪> hours ago", " since a minute ago", " since <ꪻ뮪> minutes ago", " since a second ago", " since <ꪻ뮪> seconds ago", string.Empty);
		}

		public static class Period
		{
			public const string kSuffix = ".";

			public const string aDay = " since a day ago.";

			public const string days = " since <ꪻ뮪> days ago.";

			public const string aHour = " since an hour ago.";

			public const string hours = " since <ꪻ뮪> hours ago.";

			public const string aMinute = " since a minute ago.";

			public const string minutes = " since <ꪻ뮪> minutes ago.";

			public const string aSecond = " since a second ago.";

			public const string seconds = " since <ꪻ뮪> seconds ago.";

			public const string lessThanASecond = ".";

			public readonly static TimeStringFormatter.Qualifier Qualifier;

			static Period()
			{
				TimeStringFormatter.SinceAgo.Period.Qualifier = new TimeStringFormatter.Qualifier(" since a day ago.", " since <ꪻ뮪> days ago.", " since an hour ago.", " since <ꪻ뮪> hours ago.", " since a minute ago.", " since <ꪻ뮪> minutes ago.", " since a second ago.", " since <ꪻ뮪> seconds ago.", ".");
			}
		}
	}
}