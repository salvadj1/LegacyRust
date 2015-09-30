using System;
using UnityEngine;

public class FPGrassDisplacementRadius : FPGrassDisplacementObject
{
	private Vector3 startScale;

	public FPGrassDisplacementRadius()
	{
	}

	public override void DetachAndDestroy()
	{
		base.transform.parent = null;
		base.SetOn(false);
		UnityEngine.Object.Destroy(base.gameObject, 1f);
	}

	public override void Initialize()
	{
		this.startScale = this.myTransform.localScale;
		this.myTransform.localScale = Vector3.zero;
	}

	public override void UpdateDepression()
	{
		if (Mathf.Approximately(this.currentDepressionPercent, this.targetDepressionPercent))
		{
			return;
		}
		float single = Mathf.Lerp(this.currentDepressionPercent, this.targetDepressionPercent, Time.deltaTime * 5f);
		this.currentDepressionPercent = single;
		this.myTransform.localScale = this.startScale * this.currentDepressionPercent;
	}
}