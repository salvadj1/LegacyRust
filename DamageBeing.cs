using Facepunch;
using System;
using uLink;
using UnityEngine;

public struct DamageBeing
{
	public IDBase id;

	public BodyPart bodyPart
	{
		get
		{
			if (!(this.id is IDRemoteBodyPart) || !this.id)
			{
				return BodyPart.Undefined;
			}
			return ((IDRemoteBodyPart)this.id).bodyPart;
		}
	}

	public Character character
	{
		get
		{
			return this.idOwnerMain as Character;
		}
	}

	public PlayerClient client
	{
		get
		{
			if (!this.id)
			{
				return null;
			}
			IDMain dMain = this.idOwnerMain;
			if (!dMain)
			{
				return null;
			}
			if (dMain is Character)
			{
				return ((Character)dMain).playerClient;
			}
			if (dMain is IDeployedObjectMain)
			{
				DeployedObjectInfo deployedObjectInfo = ((IDeployedObjectMain)dMain).DeployedObjectInfo;
				if (deployedObjectInfo.valid)
				{
					return deployedObjectInfo.playerClient;
				}
			}
			Controllable component = dMain.GetComponent<Controllable>();
			if (!component)
			{
				return null;
			}
			PlayerClient playerClient = component.playerClient;
			if (!playerClient)
			{
				Facepunch.NetworkView networkView = component.networkView;
				if (networkView)
				{
					PlayerClient.Find(networkView.owner, out playerClient);
				}
			}
			return playerClient;
		}
	}

	public Controllable controllable
	{
		get
		{
			if (!this.id)
			{
				return null;
			}
			IDMain dMain = this.idOwnerMain;
			if (!dMain)
			{
				return null;
			}
			if (dMain is Character)
			{
				return ((Character)dMain).controllable;
			}
			if (dMain is IDeployedObjectMain)
			{
				DeployedObjectInfo deployedObjectInfo = ((IDeployedObjectMain)dMain).DeployedObjectInfo;
				if (deployedObjectInfo.valid)
				{
					return deployedObjectInfo.playerControllable;
				}
			}
			return null;
		}
	}

	public IDMain idMain
	{
		get
		{
			IDMain dMain;
			if (!this.id)
			{
				dMain = null;
			}
			else
			{
				dMain = this.id.idMain;
			}
			return dMain;
		}
	}

	public IDMain idOwnerMain
	{
		get
		{
			IDMain dMain;
			if (!this.id)
			{
				dMain = null;
			}
			else
			{
				dMain = this.id.idMain;
			}
			IDMain component = dMain;
			if (component)
			{
				if (component is RigidObj)
				{
					Facepunch.NetworkView networkView = ((RigidObj)component).ownerView;
					if (!networkView)
					{
						component = null;
					}
					else
					{
						component = networkView.GetComponent<IDMain>();
					}
				}
				else if (component is IDeployedObjectMain)
				{
					DeployedObjectInfo deployedObjectInfo = ((IDeployedObjectMain)component).DeployedObjectInfo;
					if (deployedObjectInfo.valid)
					{
						return deployedObjectInfo.playerCharacter;
					}
				}
			}
			return component;
		}
	}

	public Facepunch.NetworkView networkView
	{
		get
		{
			if (!this.id)
			{
				return null;
			}
			IDMain dMain = this.id.idMain;
			if (dMain)
			{
				return dMain.networkView;
			}
			return this.id.networkView;
		}
	}

	public uLink.NetworkViewID networkViewID
	{
		get
		{
			Facepunch.NetworkView networkView = this.networkView;
			if (!networkView)
			{
				return uLink.NetworkViewID.unassigned;
			}
			return networkView.viewID;
		}
	}

	public Facepunch.NetworkView ownerView
	{
		get
		{
			IDMain dMain;
			if (!this.id)
			{
				dMain = null;
			}
			else
			{
				dMain = this.id.idMain;
			}
			IDMain dMain1 = dMain;
			if (!(dMain1 is RigidObj))
			{
				return this.networkView;
			}
			return ((RigidObj)dMain1).ownerView;
		}
	}

	public uLink.NetworkViewID ownerViewID
	{
		get
		{
			Facepunch.NetworkView networkView = this.ownerView;
			if (!networkView)
			{
				return uLink.NetworkViewID.unassigned;
			}
			return networkView.viewID;
		}
	}

	public ulong userID
	{
		get
		{
			PlayerClient playerClient = this.client;
			if (playerClient)
			{
				return playerClient.userID;
			}
			return (ulong)0;
		}
	}

	public bool Equals(DamageBeing other)
	{
		return this.id == other.id;
	}

	public override bool Equals(object obj)
	{
		return object.Equals(this.id, obj);
	}

	public override int GetHashCode()
	{
		return (!this.id ? 0 : this.id.GetHashCode());
	}

	public bool IsDifferentPlayer(PlayerClient exclude)
	{
		if (!this.id)
		{
			return false;
		}
		IDMain dMain = this.idOwnerMain;
		if (!dMain)
		{
			dMain = this.id.idMain;
			if (!dMain)
			{
				return false;
			}
		}
		if (dMain is Character)
		{
			PlayerClient playerClient = ((Character)dMain).playerClient;
			return (!playerClient ? false : playerClient != exclude);
		}
		if (dMain is IDeployedObjectMain)
		{
			DeployedObjectInfo deployedObjectInfo = ((IDeployedObjectMain)dMain).DeployedObjectInfo;
			if (deployedObjectInfo.valid)
			{
				PlayerClient playerClient1 = deployedObjectInfo.playerClient;
				return (!playerClient1 ? false : playerClient1 != exclude);
			}
		}
		Controllable component = dMain.GetComponent<Controllable>();
		if (!component)
		{
			return false;
		}
		PlayerClient playerClient2 = component.playerClient;
		return (!playerClient2 ? false : playerClient2 != exclude);
	}

	public static bool operator @false(DamageBeing being)
	{
		return !being.id;
	}

	public static implicit operator IDBase(DamageBeing being)
	{
		return being.id;
	}

	public static bool operator @true(DamageBeing being)
	{
		return being.id;
	}

	public override string ToString()
	{
		if (!this.id)
		{
			return "{{null}}";
		}
		return string.Format("{{id=({0}),idMain=({1})}}", this.id, this.id.idMain);
	}
}