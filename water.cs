using System;

public class water : ConsoleSystem
{
	[Client]
	[Saved]
	public static int level;

	[Client]
	[Saved]
	public static bool reflection;

	static water()
	{
		water.level = -1;
	}

	public water()
	{
	}
}