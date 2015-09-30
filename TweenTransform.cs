using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Transform")]
public class TweenTransform : UITweener
{
	public Transform @from;

	public Transform to;

	private Transform mTrans;

	public TweenTransform()
	{
	}

	public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
	{
		TweenTransform tweenTransform = UITweener.Begin<TweenTransform>(go, duration);
		tweenTransform.@from = from;
		tweenTransform.to = to;
		return tweenTransform;
	}

	protected override void OnUpdate(float factor)
	{
		if (this.@from != null && this.to != null)
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			this.mTrans.position = (this.@from.position * (1f - factor)) + (this.to.position * factor);
			this.mTrans.localScale = (this.@from.localScale * (1f - factor)) + (this.to.localScale * factor);
			this.mTrans.rotation = Quaternion.Slerp(this.@from.rotation, this.to.rotation, factor);
		}
	}
}