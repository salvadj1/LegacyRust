using Facepunch;
using System;
using uLink;
using UnityEngine;

public class BasicTorchItemRep : ItemRepresentation
{
	private const bool defaultLit = false;

	public GameObject _myLight;

	public GameObject _myLightPrefab;

	private bool lit;

	public BasicTorchItemRep()
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
}