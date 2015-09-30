using System;
using System.Collections.Generic;
using UnityEngine;

public class HardpointMaster : IDLocal
{
	public List<Hardpoint> _points;

	public HardpointMaster()
	{
	}

	public void AddHardpoint(Hardpoint point)
	{
		this._points.Add(point);
	}

	public void Awake()
	{
		this._points = new List<Hardpoint>();
	}

	public Hardpoint GetHardpointNear(Vector3 worldPos, float maxRange, Hardpoint.hardpoint_type type)
	{
		Hardpoint hardpoint;
		List<Hardpoint>.Enumerator enumerator = this._points.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Hardpoint current = enumerator.Current;
				if (current.type == type)
				{
					if (current.IsFree())
					{
						if (Vector3.Distance(current.transform.position, worldPos) > maxRange)
						{
							continue;
						}
						hardpoint = current;
						return hardpoint;
					}
				}
			}
			return null;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return hardpoint;
	}

	public Hardpoint GetHardpointNear(Vector3 worldPos, Hardpoint.hardpoint_type type)
	{
		return this.GetHardpointNear(worldPos, 3f, type);
	}

	public TransCarrier GetTransCarrier()
	{
		return this.idMain.GetLocal<TransCarrier>();
	}
}