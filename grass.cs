using System;

public class grass : ConsoleSystem
{
	[Client]
	[Saved]
	[User]
	public static bool on;

	[Client]
	[Saved]
	[User]
	public static bool forceredraw;

	[Client]
	[Saved]
	[User]
	public static bool displacement;

	[Client]
	[Saved]
	[User]
	public static float disp_trail_seconds;

	[Client]
	[Saved]
	[User]
	public static bool shadowcast
	{
		get
		{
			return FPGrass.castShadows;
		}
		set
		{
			FPGrass.castShadows = value;
		}
	}

	[Client]
	[Saved]
	[User]
	public static bool shadowreceive
	{
		get
		{
			return FPGrass.receiveShadows;
		}
		set
		{
			FPGrass.receiveShadows = value;
		}
	}

	static grass()
	{
		grass.on = FPGrass.Support.Supported;
		grass.forceredraw = false;
		grass.displacement = (!FPGrass.Support.Supported ? false : !FPGrass.Support.DisplacementExpensive);
		grass.disp_trail_seconds = 10f;
	}

	public grass()
	{
	}
}