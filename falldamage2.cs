using System;

internal class falldamage : ConsoleSystem
{
	[Admin]
	[Help("Fall velocity to begin fall damage calculations - min 18", "")]
	public static float min_vel;

	[Admin]
	[Help("Fall Velocity when damage of maxhealth will be applied", "")]
	public static float max_vel;

	[Admin]
	[Help("enable/disable fall damage", "")]
	public static bool enabled;

	[Admin]
	[Help("Average amount of time a leg injury lasts", "")]
	public static float injury_length;

	static falldamage()
	{
		falldamage.min_vel = 24f;
		falldamage.max_vel = 38f;
		falldamage.enabled = true;
		falldamage.injury_length = 40f;
	}

	public falldamage()
	{
	}
}