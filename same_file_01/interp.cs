using System;

public class interp : ConsoleSystem
{
	[Admin]
	[Help("This value adds a fixed amount of delay ( in milliseconds ) to interp delay ( on clients ).", "")]
	public static int delayms
	{
		get
		{
			ulong num = Interpolation.delayMillis;
			if (num > (long)2147483647)
			{
				return 2147483647;
			}
			return (int)num;
		}
		set
		{
			if (value >= 0)
			{
				Interpolation.delayMillis = (ulong)value;
			}
			else
			{
				Interpolation.delayMillis = (ulong)0;
			}
		}
	}

	[Admin]
	[Help("This value determins how much time to append to interp delay ( on clients ) based on server.sendrate", "")]
	public static float ratio
	{
		get
		{
			return Interpolation.sendRateRatiof;
		}
		set
		{
			Interpolation.sendRateRatiof = value;
		}
	}

	public interp()
	{
	}
}