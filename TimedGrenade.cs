using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public class TimedGrenade : RigidObj
{
	private float fuseLength = 3f;

	public GameObject explosionEffect;

	public float explosionRadius = 30f;

	public float damage = 200f;

	public IDMain myOwner;

	public AudioClip bounceSound;

	private float lastBounceTime;

	public TimedGrenade() : base(RigidObj.FeatureFlags.StreamInitialVelocity | RigidObj.FeatureFlags.StreamOwnerViewID | RigidObj.FeatureFlags.ServerCollisions)
	{
	}

	[RPC]
	private void ClientBounce(uLink.NetworkMessageInfo info)
	{
		InterpTimedEvent.Queue(this, "bounce", ref info);
	}

	protected override void OnDone()
	{
		base.collider.enabled = false;
		Vector3 vector3 = this.rigidbody.position;
		if (this.explosionEffect)
		{
			UnityEngine.Object.Instantiate(this.explosionEffect, vector3, Quaternion.identity);
		}
		Collider[] colliderArray = Physics.OverlapSphere(vector3, this.explosionRadius, 134217728);
		for (int i = 0; i < (int)colliderArray.Length; i++)
		{
			Rigidbody rigidbody = colliderArray[i].attachedRigidbody;
			if (rigidbody && !rigidbody.isKinematic)
			{
				rigidbody.AddExplosionForce(500f, vector3, this.explosionRadius, 2f);
			}
		}
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
			if (TimedGrenade.<>f__switch$mapA == null)
			{
				TimedGrenade.<>f__switch$mapA = new Dictionary<string, int>(1)
				{
					{ "bounce", 0 }
				};
			}
			if (TimedGrenade.<>f__switch$mapA.TryGetValue(tag, out num))
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