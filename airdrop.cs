using System;

public class airdrop : ConsoleSystem
{
	[Admin]
	public static int min_players;

	static airdrop()
	{
		airdrop.min_players = 50;
	}

	public airdrop()
	{
	}

	[Admin]
	public static void drop(ref ConsoleSystem.Arg arg)
	{
	}
}