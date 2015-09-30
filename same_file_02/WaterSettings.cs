using System;
using UnityEngine;

public class WaterSettings : MonoBehaviour
{
	public WaterSettings()
	{
	}

	private void OnDestroy()
	{
		GameEvent.QualitySettingsRefresh -= new GameEvent.OnGenericEvent(this.RefreshSettings);
	}

	protected void RefreshSettings()
	{
		WaterBase component = base.GetComponent<WaterBase>();
		PlanarReflection planarReflection = base.GetComponent<PlanarReflection>();
		if (!component)
		{
			return;
		}
		if (render.level > 0.8f)
		{
			component.waterQuality = WaterQuality.High;
			component.edgeBlend = true;
		}
		else if (render.level <= 0.5f)
		{
			component.waterQuality = WaterQuality.Low;
			component.edgeBlend = false;
		}
		else
		{
			component.waterQuality = WaterQuality.Medium;
			component.edgeBlend = false;
		}
		if (water.level != -1)
		{
			component.waterQuality = (WaterQuality)Mathf.Clamp(water.level - 1, 0, 2);
			component.edgeBlend = water.level == 2;
		}
		if (planarReflection)
		{
			planarReflection.reflectionMask = 13111296;
			if (!water.reflection)
			{
				planarReflection.reflectionMask = 8388608;
			}
		}
	}

	private void Start()
	{
		GameEvent.QualitySettingsRefresh += new GameEvent.OnGenericEvent(this.RefreshSettings);
		this.RefreshSettings();
	}
}