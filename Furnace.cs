using System;

public class Furnace : FireBarrel
{
	public Furnace()
	{
	}

	protected override float GetCookDuration()
	{
		return 30f;
	}
}