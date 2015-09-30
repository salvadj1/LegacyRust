using Facepunch.MeshBatch;
using RustProto;
using RustProto.Helpers;
using System;
using UnityEngine;

[NGCAutoAddScript]
public class DeployableObject : IDMain, IDeployedObjectMain, IServerSaveable, IServerSaveNotify, ICarriableTrans
{
	public bool decayProtector;

	public bool cantPlaceOn;

	public bool doEdgeCheck;

	public float maxEdgeDifferential = 1f;

	public float maxSlope = 30f;

	public ulong creatorID;

	public ulong ownerID;

	public string ownerName = string.Empty;

	public bool handleDeathHere;

	public GameObject corpseObject;

	public TransCarrier _carrier;

	public GameObject clientDeathEffect;

	private EnvDecay _EnvDecay;

	[NonSerialized]
	private HealthDimmer healthDimmer;

	DeployedObjectInfo IDeployedObjectMain.DeployedObjectInfo
	{
		get
		{
			DeployedObjectInfo deployedObjectInfo = new DeployedObjectInfo();
			deployedObjectInfo.userID = this.ownerID;
			deployedObjectInfo.valid = this.ownerID != (long)0;
			return deployedObjectInfo;
		}
	}

	public DeployableObject() : this(IDFlags.Unknown)
	{
	}

	protected DeployableObject(IDFlags flags) : base(flags)
	{
	}

	public void Awake()
	{
	}

	public bool BelongsTo(Controllable controllable)
	{
		if (!controllable)
		{
			return false;
		}
		PlayerClient playerClient = controllable.playerClient;
		if (!playerClient)
		{
			return false;
		}
		return playerClient.userID == this.ownerID;
	}

	public void CacheCreator()
	{
	}

	[RPC]
	public void Client_OnKilled()
	{
		if (this.clientDeathEffect)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.clientDeathEffect, base.transform.position, base.transform.rotation) as GameObject;
			UnityEngine.Object.Destroy(gameObject, 5f);
		}
	}

	[RPC]
	public void ClientHealthUpdate(float newHealth)
	{
		this.healthDimmer.UpdateHealthAmount(this, newHealth, false);
	}

	public TransCarrier GetCarrier()
	{
		return this._carrier;
	}

	[RPC]
	public void GetOwnerInfo(ulong creator, ulong owner)
	{
		this.creatorID = creator;
		this.ownerID = owner;
	}

	public void GrabCarrier()
	{
		RaycastHit raycastHit;
		bool flag;
		MeshBatchInstance meshBatchInstance;
		Ray ray = new Ray(base.transform.position + (Vector3.up * 0.01f), Vector3.down);
		if (Facepunch.MeshBatch.MeshBatchPhysics.Raycast(ray, out raycastHit, 5f, out flag, out meshBatchInstance))
		{
			IDMain dMain = (!flag ? IDBase.GetMain(raycastHit.collider) : meshBatchInstance.idMain);
			if (dMain)
			{
				TransCarrier local = dMain.GetLocal<TransCarrier>();
				if (local)
				{
					local.AddObject(this);
				}
			}
		}
	}

	void IServerSaveNotify.PostLoad()
	{
	}

	public static bool IsValidLocation(Vector3 location, Vector3 surfaceNormal, UnityEngine.Quaternion rotation, DeployableObject prefab)
	{
		if (prefab.doEdgeCheck)
		{
			return false;
		}
		if (Vector3.Angle(surfaceNormal, Vector3.up) <= prefab.maxSlope)
		{
			return true;
		}
		return false;
	}

	public void OnAddedToCarrier(TransCarrier carrier)
	{
		this._carrier = carrier;
	}

	public new void OnDestroy()
	{
		if (this._carrier)
		{
			this._carrier.RemoveObject(this);
			this._carrier = null;
		}
		base.OnDestroy();
	}

	public void OnDroppedFromCarrier(TransCarrier carrier)
	{
		this._carrier = null;
	}

	protected void OnPoolAlive()
	{
		this.ownerID = (ulong)0;
		this.ownerName = string.Empty;
		this.creatorID = (ulong)0;
	}

	protected void OnPoolRetire()
	{
		this.healthDimmer.Reset();
	}

	public void ReadObjectSave(ref SavedObject saveobj)
	{
		if (saveobj.HasDeployable)
		{
			this.creatorID = saveobj.Deployable.CreatorID;
			this.ownerID = saveobj.Deployable.OwnerID;
		}
	}

	public void Touched()
	{
		TransCarrier carrier = this.GetCarrier();
		if (!carrier)
		{
			return;
		}
		IDMain dMain = carrier.idMain;
		if (!dMain)
		{
			return;
		}
		if (dMain is StructureComponent)
		{
			((StructureComponent)dMain).Touched();
		}
	}

	public void WriteObjectSave(ref SavedObject.Builder saveobj)
	{
		NetEntityID netEntityID;
		using (Recycler<objectDeployable, objectDeployable.Builder> recycler = objectDeployable.Recycler())
		{
			objectDeployable.Builder builder = recycler.OpenBuilder();
			builder.SetCreatorID(this.creatorID);
			builder.SetOwnerID(this.ownerID);
			saveobj.SetDeployable(builder);
		}
		using (Recycler<objectICarriableTrans, objectICarriableTrans.Builder> recycler1 = objectICarriableTrans.Recycler())
		{
			objectICarriableTrans.Builder builder1 = recycler1.OpenBuilder();
			if (!this._carrier || (int)NetEntityID.Of(this._carrier, out netEntityID) == 0)
			{
				builder1.ClearTransCarrierID();
			}
			else
			{
				builder1.SetTransCarrierID(netEntityID.id);
			}
			saveobj.SetCarriableTrans(builder1);
		}
	}
}