using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public class SignalGrenade : RigidObj
{
	private float fuseLength = 3f;

	public GameObject explosionEffect;

	public AudioClip bounceSound;

	private float lastBounceTime;

	public SignalGrenade() : base(RigidObj.FeatureFlags.StreamInitialVelocity | RigidObj.FeatureFlags.StreamOwnerViewID | RigidObj.FeatureFlags.ServerCollisions)
	{
	}

	[RPC]
	private void ClientBounce(uLink.NetworkMessageInfo info)
	{
		InterpTimedEvent.Queue(this, "bounce", ref info);
	}

	protected override void OnDone()
	{
		UnityEngine.Object obj = UnityEngine.Object.Instantiate(this.explosionEffect, base.transform.position, Quaternion.LookRotation(Vector3.up));
		UnityEngine.Object.Destroy(obj, 60f);
	}

	protected override void OnHide()
	{
		if (base.renderer)
		{
			base.renderer.enabled = false;
		}
	}

	protected override bool OnInterpTimedEvent()
	{
		int num;
		string tag = InterpTimedEvent.Tag;
		if (tag != null)
		{
			if (SignalGrenade.<>f__switch$map9 == null)
			{
				SignalGrenade.<>f__switch$map9 = new Dictionary<string, int>(1)
				{
					{ "bounce", 0 }
				};
			}
			if (SignalGrenade.<>f__switch$map9.TryGetValue(tag, out num))
			{
				if (num == 0)
				{
					this.PlayClientBounce();
					return true;
				}
			}
		}
		return base.OnInterpTimedEvent();
	}

	protected override void OnShow()
	{
		if (base.renderer)
		{
			base.renderer.enabled = true;
		}
	}

	private void PlayClientBounce()
	{
		this.bounceSound.Play(this.rigidbody.position, 0.25f, UnityEngine.Random.Range(0.85f, 1.15f), 1f, 18f);
	}

	private new void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		base.uLink_OnNetworkInstantiate(info);
	}
}