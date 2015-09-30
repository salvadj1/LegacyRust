using System;
using System.Collections.Generic;
using UnityEngine;

public class WoodBlockerTemp : MonoBehaviour
{
	public static List<WoodBlockerTemp> _blockers;

	public float numWood;

	public WoodBlockerTemp()
	{
	}

	private void Awake()
	{
		WoodBlockerTemp.TryInitBlockers();
		this.numWood = (float)UnityEngine.Random.Range(10, 15);
		WoodBlockerTemp._blockers.Add(this);
		UnityEngine.Object.Destroy(base.gameObject, 300f);
	}

	public void ConsumeWood(float consume)
	{
		WoodBlockerTemp woodBlockerTemp = this;
		woodBlockerTemp.numWood = woodBlockerTemp.numWood - consume;
		if (this.numWood < 0f)
		{
			this.numWood = 0f;
		}
	}

	public static WoodBlockerTemp GetBlockerForPoint(Vector3 point)
	{
		WoodBlockerTemp woodBlockerTemp;
		WoodBlockerTemp.TryInitBlockers();
		List<WoodBlockerTemp>.Enumerator enumerator = WoodBlockerTemp._blockers.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				WoodBlockerTemp current = enumerator.Current;
				if (Vector3.Distance(current.transform.position, point) >= 4f)
				{
					continue;
				}
				woodBlockerTemp = current;
				return woodBlockerTemp;
			}
			WoodBlockerTemp woodBlockerTemp1 = (WoodBlockerTemp)GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent("WoodBlockerTemp");
			woodBlockerTemp1.renderer.enabled = false;
			woodBlockerTemp1.collider.enabled = false;
			woodBlockerTemp1.transform.position = point;
			woodBlockerTemp1.name = "WBT";
			return woodBlockerTemp1;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return woodBlockerTemp;
	}

	public float GetWoodLeft()
	{
		return this.numWood;
	}

	public bool HasWood()
	{
		return this.numWood >= 1f;
	}

	public void OnDestroy()
	{
		WoodBlockerTemp._blockers.Remove(this);
	}

	private static void TryInitBlockers()
	{
		if (WoodBlockerTemp._blockers == null)
		{
			WoodBlockerTemp._blockers = new List<WoodBlockerTemp>();
		}
	}
}