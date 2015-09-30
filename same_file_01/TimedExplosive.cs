using System;
using UnityEngine;

public class TimedExplosive : IDLocal
{
	public float fuseLength = 5f;

	public float explosionRadius = 4f;

	public float damage = 100f;

	public GameObject explosionEffect;

	public AudioClip tickSound;

	private NGCView testView;

	public TimedExplosive()
	{
	}

	private void Awake()
	{
		this.testView = base.GetComponent<NGCView>();
		if (this.tickSound != null)
		{
			base.InvokeRepeating("TickSound", 0f, 1f);
		}
	}

	[RPC]
	public void ClientExplode()
	{
		UnityEngine.Object.Instantiate(this.explosionEffect, base.transform.position, base.transform.rotation);
		base.CancelInvoke();
	}

	public void OnDestroy()
	{
		base.CancelInvoke();
	}

	public void TickSound()
	{
		this.tickSound.Play(base.transform.position, 1f, 3f, 20f);
	}
}