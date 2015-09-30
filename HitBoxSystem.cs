using Facepunch.Intersect;
using System;
using UnityEngine;

public class HitBoxSystem : Facepunch.Intersect.HitBoxSystem
{
	public HitBoxSystem()
	{
	}

	protected new void Awake()
	{
		base.Awake();
		this.CheckLayer();
	}

	private void CheckLayer()
	{
		if (base.gameObject.layer != 17)
		{
			base.gameObject.layer = 17;
		}
	}

	protected void Start()
	{
		this.CheckLayer();
	}
}