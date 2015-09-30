using System;

public class gizmos : ConsoleSystem
{
	[Client]
	public static bool carrier;

	static gizmos()
	{
		gizmos.carrier = true;
	}

	public gizmos()
	{
	}
}