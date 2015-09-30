using Facepunch.Procedural;
using System;

public class BasicWildLifeMovement : BaseAIMovement
{
	[NonSerialized]
	protected Direction look;

	protected float actualMoveSpeedPerSec;

	public float simRate = 5f;

	public float moveCastOffset = 0.25f;

	private float hullLength = 0.1f;

	public BasicWildLifeMovement()
	{
	}
}