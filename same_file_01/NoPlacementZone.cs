using System;
using System.Collections.Generic;
using UnityEngine;

public class NoPlacementZone : MonoBehaviour
{
	public static List<NoPlacementZone> _zones;

	static NoPlacementZone()
	{
	}

	public NoPlacementZone()
	{
	}

	public static void AddZone(NoPlacementZone zone)
	{
		if (NoPlacementZone._zones == null)
		{
			NoPlacementZone._zones = new List<NoPlacementZone>();
		}
		if (NoPlacementZone._zones.Contains(zone))
		{
			return;
		}
		NoPlacementZone._zones.Add(zone);
	}

	public void Awake()
	{
		NoPlacementZone.AddZone(this);
	}

	public float GetRadius()
	{
		return base.transform.localScale.x;
	}

	public void OnDestroy()
	{
		NoPlacementZone.RemoveZone(this);
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 0.5f, 0.3f, 0.1f);
		Gizmos.DrawSphere(base.transform.position, this.GetRadius());
		Gizmos.color = Color.green;
		Gizmos.DrawCube(base.transform.position, Vector3.one * 0.5f);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1f, 0.5f, 0.3f, 0.8f);
		Gizmos.DrawWireSphere(base.transform.position, this.GetRadius());
		Gizmos.color = Color.green;
		Gizmos.DrawCube(base.transform.position, Vector3.one * 0.5f);
	}

	public static void RemoveZone(NoPlacementZone zone)
	{
		if (NoPlacementZone._zones.Contains(zone))
		{
			NoPlacementZone._zones.Remove(zone);
		}
	}

	public static bool ValidPos(Vector3 pos)
	{
		bool flag;
		List<NoPlacementZone>.Enumerator enumerator = NoPlacementZone._zones.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				NoPlacementZone current = enumerator.Current;
				if (Vector3.Distance(pos, current.transform.position) > current.GetRadius())
				{
					continue;
				}
				flag = false;
				return flag;
			}
			return true;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return flag;
	}
}