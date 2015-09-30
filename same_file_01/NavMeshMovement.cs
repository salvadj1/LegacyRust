using System;
using UnityEngine;

public class NavMeshMovement : BaseAIMovement
{
	public NavMeshAgent _agent;

	public Transform movementTransform;

	public float targetLookRotation;

	private Vector3 lastStuckPos = Vector3.zero;

	public NavMeshMovement()
	{
	}

	public void Awake()
	{
	}

	public override bool IsStuck()
	{
		Vector3 vector3 = base.transform.InverseTransformDirection(this._agent.velocity);
		return (!this._agent.hasPath || this._agent.speed <= 0.5f ? false : vector3.z < this._agent.speed * 0.25f);
	}

	public override void ProcessNetworkUpdate(ref Vector3 origin, ref Quaternion rotation)
	{
		Vector3 vector3;
		Vector3 vector31;
		TransformHelpers.GetGroundInfo(origin + new Vector3(0f, 0.25f, 0f), 10f, out vector3, out vector31);
		Vector3 vector32 = rotation * Vector3.up;
		float single = Vector3.Angle(vector32, vector31);
		if (single > 20f)
		{
			vector31 = Vector3.Slerp(vector32, vector31, 20f / single);
		}
		origin = vector3;
		rotation = TransformHelpers.LookRotationForcedUp(rotation, vector31);
	}

	public bool RemoveIfNotOnNavmesh()
	{
		if (!(this._agent == null) && this._agent.enabled)
		{
			return false;
		}
		TakeDamage.KillSelf(base.GetComponent<IDBase>(), null);
		return true;
	}

	public virtual void SetAgentAiming(bool enabled)
	{
		this._agent.updateRotation = enabled;
	}

	public override void SetLookDirection(Vector3 worldDir)
	{
		this._agent.SetDestination(base.transform.position);
		this._agent.Stop();
		this.SetAgentAiming(false);
		if (worldDir == Vector3.zero)
		{
			return;
		}
		this.movementTransform.rotation = Quaternion.LookRotation(worldDir);
	}

	public override void SetMoveDirection(Vector3 worldDir, float speed)
	{
		this.SetAgentAiming(true);
		this._agent.SetDestination(this.movementTransform.position + (worldDir * 30f));
		this._agent.speed = speed;
	}

	public override void SetMovePosition(Vector3 worldPos, float speed)
	{
		this.SetAgentAiming(true);
		this._agent.SetDestination(worldPos);
		this._agent.speed = speed;
	}

	public override void SetMoveTarget(GameObject target, float speed)
	{
		this.SetAgentAiming(true);
		Vector3 vector3 = target.transform.position - base.transform.position;
		this._agent.SetDestination(target.transform.position + (vector3.normalized * 0.5f));
		this._agent.speed = speed;
	}

	public override void Stop()
	{
		if (this.RemoveIfNotOnNavmesh())
		{
			return;
		}
		this._agent.Stop();
		this.SetAgentAiming(false);
		this.desiredSpeed = 0f;
	}
}