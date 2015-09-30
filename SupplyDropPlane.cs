using System;
using uLink;
using UnityEngine;

public class SupplyDropPlane : IDMain
{
	public GameObject[] propellers;

	public Vector3 startPos;

	public Vector3 dropTargetPos;

	public Quaternion startAng;

	public float maxSpeed = 250f;

	private bool passedTarget;

	protected Vector3 targetPos;

	protected float lastDist = Single.PositiveInfinity;

	protected bool approachingTarget = true;

	protected float targetReachedTime;

	protected bool droppedPayload;

	public int TEMP_numCratesToDrop = 3;

	protected TransformInterpolator _interp;

	protected double _lastMoveTime;

	public SupplyDropPlane() : this(IDFlags.Unknown)
	{
	}

	protected SupplyDropPlane(IDFlags idFlags) : base(idFlags)
	{
	}

	[RPC]
	public void GetNetworkUpdate(Vector3 pos, Quaternion rot, uLink.NetworkMessageInfo info)
	{
		this._interp.SetGoals(pos, rot, info.timestamp);
	}

	private void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		this._interp = base.GetComponent<TransformInterpolator>();
		this._interp.running = true;
	}

	public void Update()
	{
		GameObject[] gameObjectArray = this.propellers;
		for (int i = 0; i < (int)gameObjectArray.Length; i++)
		{
			GameObject gameObject = gameObjectArray[i];
			gameObject.transform.RotateAroundLocal(Vector3.forward, 12f * Time.deltaTime);
		}
	}
}