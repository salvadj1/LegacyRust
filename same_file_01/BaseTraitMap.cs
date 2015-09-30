using System;
using UnityEngine;

public abstract class BaseTraitMap : ScriptableObject
{
	[NonSerialized]
	private bool bound;

	internal BaseTraitMap()
	{
	}

	internal void BIND_REGISTRATION()
	{
		if (!this.bound)
		{
			this.BindToRegistry();
			this.bound = true;
		}
	}

	internal abstract void BindToRegistry();
}