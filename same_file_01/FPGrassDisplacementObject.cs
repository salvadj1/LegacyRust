using System;
using UnityEngine;

public class FPGrassDisplacementObject : MonoBehaviour
{
	protected Transform myTransform;

	protected float currentDepressionPercent;

	protected float targetDepressionPercent = 1f;

	public FPGrassDisplacementObject()
	{
	}

	public void Awake()
	{
		this.myTransform = base.transform;
		this.Initialize();
	}

	public virtual void DetachAndDestroy()
	{
	}

	public virtual void Initialize()
	{
	}

	public void SetDepressionAmount(float percent)
	{
		this.targetDepressionPercent = percent;
	}

	public void SetOn(bool on)
	{
		this.targetDepressionPercent = (!on ? 0f : 1f);
	}

	public void Update()
	{
		this.UpdateDepression();
	}

	public virtual void UpdateDepression()
	{
	}
}