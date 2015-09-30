using System;

public class gunshots : ConsoleSystem
{
	[Admin]
	public static bool aiscared;

	static gunshots()
	{
		gunshots.aiscared = true;
	}

	public gunshots()
	{
	}
}