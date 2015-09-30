using System;

internal class terrain : ConsoleSystem
{
	[Client]
	public static bool manual;

	[Client]
	[Help("The interval (seconds) to force tree redrawing when there is no camera movement. Set to zero if you do not want forced tree drawing", "")]
	public static float idleinterval;

	static terrain()
	{
		terrain.idleinterval = 3.2f;
	}

	public terrain()
	{
	}

	[Client]
	public static void flush(ref ConsoleSystem.Arg arg)
	{
		TerrainControl.ter_flush();
	}

	[Client]
	public static void flushtrees(ref ConsoleSystem.Arg arg)
	{
		TerrainControl.ter_flushtrees();
	}

	[Client]
	public static void mat(ref ConsoleSystem.Arg arg)
	{
		TerrainControl.ter_mat();
	}

	[Client]
	public static void reassign(ref ConsoleSystem.Arg arg)
	{
		TerrainControl.ter_reassign();
	}

	[Client]
	public static void reassign_nocopy(ref ConsoleSystem.Arg arg)
	{
		TerrainControl.ter_reassign_nocopy();
	}
}