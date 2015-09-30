using System;
using UnityEngine;

public class EnvDecay : IDLocal
{
	public float decayMultiplier = 1f;

	public bool ambientDecay;

	protected float lastDecayThink;

	protected TakeDamage _takeDamage;

	protected DeployableObject _deployable;

	public EnvDecay()
	{
	}

	public void Awake()
	{
		base.enabled = false;
	}
}