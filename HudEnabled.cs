using System;
using System.Collections.Generic;
using UnityEngine;

public class HudEnabled : MonoBehaviour
{
	private static bool On;

	private static bool GReady;

	public HudEnabled()
	{
	}

	private void Awake()
	{
		GameObject gameObject = base.gameObject;
		HudEnabled.G.All.Add(gameObject);
		gameObject.SetActive(HudEnabled.On);
	}

	public static void Disable()
	{
		HudEnabled.Set(false);
	}

	public static void Enable()
	{
		HudEnabled.Set(true);
	}

	private void OnDestroy()
	{
		if (HudEnabled.GReady)
		{
			HudEnabled.G.All.Remove(base.gameObject);
		}
	}

	public static void Set(bool enable)
	{
		if (HudEnabled.On != enable)
		{
			HudEnabled.Toggle();
		}
	}

	public static void Toggle()
	{
		HudEnabled.On = !HudEnabled.On;
		if (HudEnabled.GReady)
		{
			foreach (GameObject all in HudEnabled.G.All)
			{
				all.SetActive(HudEnabled.On);
			}
		}
	}

	private static class G
	{
		public readonly static HashSet<GameObject> All;

		static G()
		{
			HudEnabled.G.All = new HashSet<GameObject>();
			HudEnabled.GReady = true;
		}
	}
}