using System;

public class env : ConsoleSystem
{
	[Admin]
	[Help("The length of a day in real minutes", "")]
	public static float daylength;

	[Admin]
	[Help("The length of a night in real minutes", "")]
	public static float nightlength;

	static env()
	{
		env.daylength = 45f;
		env.nightlength = 15f;
	}

	public env()
	{
	}

	[Admin]
	[Help("Gets or sets the current time", "")]
	public static void time(ref ConsoleSystem.Arg arg)
	{
		if (!EnvironmentControlCenter.Singleton)
		{
			return;
		}
		float time = EnvironmentControlCenter.Singleton.GetTime();
		arg.ReplyWith(string.Concat("Current Time: ", time.ToString()));
	}
}