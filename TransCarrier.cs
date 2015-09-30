using System;
using System.Collections.Generic;
using UnityEngine;

public class TransCarrier : IDLocal, ICarriableTrans
{
	public HashSet<ICarriableTrans> _objs;

	private bool destroying;

	public TransCarrier()
	{
	}

	public virtual void AddObject(ICarriableTrans obj)
	{
		if (object.ReferenceEquals(obj, this))
		{
			return;
		}
		if (!this.destroying)
		{
			this.TryInit();
			this._objs.Add(obj);
			obj.OnAddedToCarrier(this);
		}
		else
		{
			Debug.LogWarning("Did not add object because the this carrier is destroying", this);
		}
	}

	public virtual void DropObjects()
	{
		if (this._objs == null)
		{
			return;
		}
		HashSet<ICarriableTrans> carriableTrans = this._objs;
		this._objs = null;
		foreach (ICarriableTrans carriableTran in carriableTrans)
		{
			if (carriableTran is UnityEngine.Object && !(UnityEngine.Object)carriableTran)
			{
				continue;
			}
			carriableTran.OnDroppedFromCarrier(this);
		}
	}

	public void DropObjects(bool andDisableAddingAfter)
	{
		try
		{
			this.DropObjects();
		}
		finally
		{
			if (andDisableAddingAfter)
			{
				this.destroying = true;
			}
		}
	}

	public virtual void OnAddedToCarrier(TransCarrier carrier)
	{
	}

	public virtual void OnDroppedFromCarrier(TransCarrier carrier)
	{
		this.DropObjects();
	}

	public virtual void RemoveObject(ICarriableTrans obj)
	{
		if (this._objs != null)
		{
			this._objs.Remove(obj);
		}
	}

	public void TryInit()
	{
		if (this._objs == null)
		{
			this._objs = new HashSet<ICarriableTrans>();
		}
	}
}