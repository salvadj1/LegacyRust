using System;
using uLink;
using UnityEngine;

public sealed class FlareObj : RigidObj
{
	private GameObject lightInstance;

	public AudioClip StrikeSound;

	public GameObject lightPrefab;

	public FlareObj() : base(RigidObj.FeatureFlags.StreamInitialVelocity)
	{
	}

	protected override void OnDone()
	{
	}

	protected override void OnHide()
	{
		if (this.lightInstance)
		{
			this.lightInstance.SetActive(false);
		}
		if (base.renderer)
		{
			base.renderer.enabled = false;
		}
	}

	protected override void OnShow()
	{
		if (this.lightInstance)
		{
			this.lightInstance.SetActive(true);
		}
		if (base.renderer)
		{
			base.renderer.enabled = true;
		}
	}

	private new void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		this.lightInstance = UnityEngine.Object.Instantiate(this.lightPrefab, base.transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.identity) as GameObject;
		this.lightInstance.transform.parent = base.transform;
		base.uLink_OnNetworkInstantiate(info);
	}
}