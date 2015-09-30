using Facepunch;
using System;
using uLink;
using UnityEngine;

public class TorchItemRep : ItemRepresentation
{
	private const bool defaultLit = false;

	public GameObject _myLight;

	public GameObject _myLightPrefab;

	public AudioClip StrikeSound;

	private bool lit;

	public TorchItemRep()
	{
	}

	private void KillLight()
	{
		if (this._myLight)
		{
			UnityEngine.Object.Destroy(this._myLight);
			this._myLight = null;
		}
	}

	protected new void OnDestroy()
	{
		this.KillLight();
		base.OnDestroy();
	}

	[RPC]
	protected void OnStatus(bool on)
	{
		if (on != this.lit)
		{
			if (!on)
			{
				this.RepExtinguish();
			}
			else
			{
				this.RepIgnite();
			}
			this.lit = on;
		}
	}

	public void RepExtinguish()
	{
		if (this.lit)
		{
			this.lit = false;
			this.KillLight();
		}
	}

	public void RepIgnite()
	{
		if (!this.lit)
		{
			this.lit = true;
			this.StrikeSound.Play(base.transform.position, 1f, 2f, 8f);
			this._myLight = this.muzzle.InstantiateAsChild(this._myLightPrefab, false);
		}
	}

	private void ServerRPC_Status(bool lit)
	{
		uLink.RPCMode rPCMode;
		Facepunch.NetworkView networkView = base.networkView;
		rPCMode = (lit ? uLink.RPCMode.OthersExceptOwnerBuffered : uLink.RPCMode.OthersExceptOwner);
		networkView.RPC<bool>("OnStatus", rPCMode, lit);
		this.lit = lit;
	}

	protected new void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		base.uLink_OnNetworkInstantiate(info);
		this.OnStatus(false);
	}
}