using System;

public class deathscreen : ConsoleSystem
{
	[Client]
	public static string reason;

	static deathscreen()
	{
		deathscreen.reason = "...";
	}

	public deathscreen()
	{
	}

	[Client]
	public static void show(ref ConsoleSystem.Arg arg)
	{
		DeathScreen.Show();
	}
}