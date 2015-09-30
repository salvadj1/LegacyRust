using System;
using uLink;
using UnityEngine;

public class ResourceObject : IDMain
{
	private GenericSpawner _mySpawner;

	public Mesh[] visualMeshes;

	public Mesh[] collisionMeshes;

	public GameObject clientMeshChangeEffect;

	public ResourceTarget _resTarg;

	public MeshFilter _meshFilter;

	public MeshCollider _meshCollider;

	private int _pendingMeshIndex = -1;

	private int _lastModelIndex = -1;

	private NetEntityID myID;

	public ResourceObject() : base(IDFlags.Unknown)
	{
	}

	public void ChangeModelIndex(int index)
	{
		this._meshCollider.sharedMesh = this.collisionMeshes[index];
		this._meshFilter.sharedMesh = this.visualMeshes[index];
		this._lastModelIndex = index;
	}

	public void DelayedModelChangeIndex()
	{
		this.ChangeModelIndex(this._pendingMeshIndex);
	}

	[RPC]
	public void modelindex(int index, uLink.NetworkMessageInfo info)
	{
		bool flag = false;
		if (EnvironmentControlCenter.Singleton && EnvironmentControlCenter.Singleton.IsNight() && PlayerClient.GetLocalPlayer().controllable && Vector3.Distance(PlayerClient.GetLocalPlayer().controllable.transform.position, base.transform.position) > 20f)
		{
			flag = true;
		}
		if (this.clientMeshChangeEffect && this._lastModelIndex != -1 && !flag)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.clientMeshChangeEffect, base.transform.position, base.transform.rotation) as GameObject;
			UnityEngine.Object.Destroy(gameObject, 5f);
		}
		this._pendingMeshIndex = index;
		base.Invoke("DelayedModelChangeIndex", 0.15f);
	}

	private void NGC_OnInstantiate(NGCView view)
	{
		this.myID = NetEntityID.Get(this);
		this._resTarg = base.GetComponent<ResourceTarget>();
	}

	public void SetSpawner(GameObject spawner)
	{
		this._mySpawner = spawner.GetComponent<GenericSpawner>();
	}
}