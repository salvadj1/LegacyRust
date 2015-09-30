using System;
using uLink;
using UnityEngine;

public class SupplyCrate : IDMain, IInterpTimedEventReceiver
{
	public RigidbodyInterpolator _interp;

	protected bool _landed;

	protected bool _landing;

	protected uLink.RPCMode updateRPCMode = uLink.RPCMode.Others;

	public SupplyParachute chute;

	public GameObject landedEffect;

	public LootableObject lootableObject;

	public GameObject bubbleWrap;

	public SupplyCrate() : this(IDFlags.Unknown)
	{
	}

	protected SupplyCrate(IDFlags idFlags) : base(idFlags)
	{
	}

	[RPC]
	protected void GetNetworkUpdate(Vector3 pos, Quaternion rot, uLink.NetworkMessageInfo info)
	{
		this._interp.SetGoals(pos, rot, info.timestamp);
	}

	void IInterpTimedEventReceiver.OnInterpTimedEvent()
	{
		if (InterpTimedEvent.Tag != "LAND")
		{
			InterpTimedEvent.MarkUnhandled();
		}
		else
		{
			this.LandShared();
			GameObject gameObject = UnityEngine.Object.Instantiate(this.landedEffect, base.transform.position, base.transform.rotation) as GameObject;
			UnityEngine.Object.Destroy(gameObject, 2.5f);
			this._landed = true;
			this.chute.Landed();
		}
	}

	[RPC]
	public void Landed(uLink.NetworkMessageInfo info)
	{
		InterpTimedEvent.Queue(this, "LAND", ref info);
	}

	private void LandShared()
	{
		this._landed = true;
		if (this.lootableObject)
		{
			this.lootableObject.accessLocked = false;
		}
		UnityEngine.Object.Destroy(this.bubbleWrap);
	}

	private void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		this.lootableObject.accessLocked = true;
		this._interp.running = true;
		base.rigidbody.isKinematic = true;
	}
}