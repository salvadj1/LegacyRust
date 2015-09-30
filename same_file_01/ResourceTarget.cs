using Facepunch;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTarget : Facepunch.MonoBehaviour
{
	[SerializeField]
	public List<ResourceGivePair> resourcesAvailable;

	public float gatherEfficiencyMultiplier = 1f;

	private float gatherProgress;

	public ResourceTarget.ResourceTargetType type;

	private int startingTotal;

	[NonSerialized]
	private bool _initialized;

	public ResourceTarget()
	{
	}

	public enum ResourceTargetType
	{
		Animal = 0,
		WoodPile = 1,
		StaticTree = 2,
		Rock1 = 3,
		Rock2 = 4,
		LAST = 5,
		Rock3 = 5
	}
}