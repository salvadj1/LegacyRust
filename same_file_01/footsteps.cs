using System;

public class footsteps : ConsoleSystem
{
	[Client]
	[Help("Footstep Quality, 0 = default sound, 1 = dynamic for local, 2 = dynamic for all. 0-2 (default 2)", "")]
	[Saved]
	public static int quality;

	static footsteps()
	{
		footsteps.quality = 2;
	}

	public footsteps()
	{
	}
}