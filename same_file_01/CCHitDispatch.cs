using System;
using UnityEngine;

[RequireComponent(typeof(CCDesc))]
public sealed class CCHitDispatch : MonoBehaviour
{
	[NonSerialized]
	private CCDesc ccdesc;

	[NonSerialized]
	private CCDesc.HitManager hitManager;

	[NonSerialized]
	private bool didSetup;

	public CCDesc CCDesc
	{
		get
		{
			if (Application.isPlaying)
			{
				if (!this.didSetup)
				{
					this.DoSetup();
				}
				return this.ccdesc;
			}
			return (!this.ccdesc ? base.GetComponent<CCDesc>() : this.ccdesc);
		}
	}

	public CCDesc.HitManager Hits
	{
		get
		{
			if (!this.didSetup)
			{
				this.DoSetup();
			}
			return this.hitManager;
		}
	}

	public CCHitDispatch()
	{
	}

	private void DoSetup()
	{
		if (!this.didSetup)
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.didSetup = true;
			CCDesc component = base.GetComponent<CCDesc>();
			CCDesc cCDesc = component;
			this.ccdesc = component;
			CCDesc.HitManager hitManager = new CCDesc.HitManager();
			CCDesc.HitManager hitManager1 = hitManager;
			this.hitManager = hitManager;
			cCDesc.AssignedHitManager = hitManager1;
		}
	}

	public static CCHitDispatch GetHitDispatch(CCDesc CCDesc)
	{
		if (!CCDesc)
		{
			return null;
		}
		if (!object.ReferenceEquals(CCDesc.AssignedHitManager, null))
		{
			return CCDesc.GetComponent<CCHitDispatch>();
		}
		CCHitDispatch component = CCDesc.GetComponent<CCHitDispatch>();
		if (component)
		{
			return component;
		}
		return CCDesc.gameObject.AddComponent<CCHitDispatch>();
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		CCDesc.HitManager hits = this.Hits;
		if (!object.ReferenceEquals(hits, null))
		{
			hits.Push(hit);
		}
	}

	private void OnDestroy()
	{
		if (this.didSetup && !object.ReferenceEquals(this.hitManager, null))
		{
			CCDesc.HitManager hitManager = this.hitManager;
			this.hitManager = null;
			if (this.ccdesc)
			{
				this.ccdesc.AssignedHitManager = null;
			}
			hitManager.Dispose();
		}
	}

	public event CCDesc.HitFilter OnHit
	{
		add
		{
			CCDesc.HitManager hits = this.Hits;
			if (!object.ReferenceEquals(hits, null))
			{
				hits.OnHit += value;
			}
		}
		remove
		{
			CCDesc.HitManager hits = this.Hits;
			if (!object.ReferenceEquals(hits, null))
			{
				hits.OnHit -= value;
			}
		}
	}
}