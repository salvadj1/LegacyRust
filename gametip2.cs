using System;

public class gametip : ConsoleSystem
{
	[Client]
	public static float scale;

	static gametip()
	{
		gametip.scale = 1f;
	}

	public gametip()
	{
	}
}