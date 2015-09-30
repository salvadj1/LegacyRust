using System;
using System.Collections.Generic;

public class SpikeWall : IDLocal
{
	public float returnFraction = 0.2f;

	public float dmgPerTick = 20f;

	public float baseReturnDmg = 5f;

	public List<TakeDamage> _touching;

	private bool running;

	public SpikeWall()
	{
	}
}