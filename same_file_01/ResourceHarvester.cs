using System;
using UnityEngine;

[Serializable]
public class ResourceHarvester : UnityEngine.Object
{
	public float[] efficiencies;

	public ResourceHarvester()
	{
	}

	public static string ResourceDBNameForType(ResourceType hitType)
	{
		ResourceType resourceType = hitType;
		if (resourceType == ResourceType.Wood)
		{
			return "Wood";
		}
		if (resourceType == ResourceType.Meat)
		{
			return "Raw Meat";
		}
		return string.Empty;
	}

	public float ResourceEfficiencyForType(ResourceTarget.ResourceTargetType type)
	{
		return 0f;
	}
}