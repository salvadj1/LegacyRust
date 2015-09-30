using System;
using System.Collections.Generic;
using UnityEngine;

public class Radiation : IDLocalCharacter
{
	[NonSerialized]
	private List<RadiationZone> radiationZones;

	public Radiation()
	{
	}

	public void AddRadiationZone(RadiationZone zone)
	{
		if (zone.CanAddToRadiation(this))
		{
			List<RadiationZone> radiationZones = this.radiationZones;
			if (radiationZones == null)
			{
				List<RadiationZone> radiationZones1 = new List<RadiationZone>();
				List<RadiationZone> radiationZones2 = radiationZones1;
				this.radiationZones = radiationZones1;
				radiationZones = radiationZones2;
			}
			radiationZones.Add(zone);
		}
	}

	public float CalculateExposure(bool countArmor)
	{
		if (this.radiationZones == null || this.radiationZones.Count == 0)
		{
			return 0f;
		}
		Vector3 vector3 = base.origin;
		float exposureForPos = 0f;
		foreach (RadiationZone radiationZone in this.radiationZones)
		{
			exposureForPos = exposureForPos + radiationZone.GetExposureForPos(vector3);
		}
		if (countArmor)
		{
			HumanBodyTakeDamage humanBodyTakeDamage = base.takeDamage as HumanBodyTakeDamage;
			if (humanBodyTakeDamage)
			{
				float armorValue = humanBodyTakeDamage.GetArmorValue(4);
				if (armorValue > 0f)
				{
					exposureForPos = exposureForPos * (1f - Mathf.Clamp(armorValue / 200f, 0f, 1f));
				}
			}
		}
		return exposureForPos;
	}

	public float GetRadExposureScalar(float exposure)
	{
		return Mathf.Clamp01(exposure / 1000f);
	}

	private void OnDestroy()
	{
		if (this.radiationZones != null)
		{
			foreach (RadiationZone radiationZone in this.radiationZones)
			{
				if (!radiationZone)
				{
					continue;
				}
				radiationZone.RemoveFromRadiation(this);
			}
			this.radiationZones = null;
		}
	}

	public void RemoveRadiationZone(RadiationZone zone)
	{
		if (this.radiationZones != null && this.radiationZones.Remove(zone))
		{
			zone.RemoveFromRadiation(this);
		}
	}
}