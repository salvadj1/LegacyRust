using System;
using System.Reflection;
using UnityEngine;

public static class TerrainHack
{
	private readonly static bool AbleToLocateOnTerrainChanged;

	private readonly static object[] TriggerTreeChangeValues;

	private static bool RanOnce;

	private static bool Working;

	private static MethodInfo OnTerrainChanged;

	static TerrainHack()
	{
		object obj;
		TerrainHack.OnTerrainChanged = typeof(Terrain).GetMethod("OnTerrainChanged", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (TerrainHack.OnTerrainChanged == null)
		{
			Debug.LogWarning("Couldnt locate method OnTerrainChanged");
		}
		else
		{
			Type type = Type.GetType("UnityEngine.TerrainChangedFlags, UnityEngine", false, false);
			if (type == null)
			{
				Debug.LogWarning("Couldnt locate enum TerrainChangedFlags.");
			}
			else
			{
				try
				{
					obj = Enum.Parse(type, "TreeInstances", false);
				}
				catch (Exception exception2)
				{
					Exception exception = exception2;
					Debug.LogException(exception);
					try
					{
						obj = Enum.ToObject(type, 2);
					}
					catch (Exception exception1)
					{
						Debug.LogException(exception);
						return;
					}
				}
				TerrainHack.AbleToLocateOnTerrainChanged = true;
				TerrainHack.TriggerTreeChangeValues = new object[] { obj };
			}
		}
	}

	public static void RefreshTreeTextures(Terrain terrain)
	{
		if (!terrain)
		{
			throw new NullReferenceException();
		}
		if (!TerrainHack.RanOnce)
		{
			TerrainHack.RanOnce = true;
			if (TerrainHack.AbleToLocateOnTerrainChanged)
			{
				try
				{
					TerrainHack.OnTerrainChanged.Invoke(terrain, TerrainHack.TriggerTreeChangeValues);
					TerrainHack.Working = true;
					return;
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					TerrainHack.Working = false;
				}
			}
		}
		if (!TerrainHack.Working)
		{
			terrain.Flush();
		}
		else
		{
			TerrainHack.OnTerrainChanged.Invoke(terrain, TerrainHack.TriggerTreeChangeValues);
		}
	}
}