using System;
using UnityEngine;

public class BaseAIMovement : MonoBehaviour
{
	protected float desiredSpeed;

	protected float collisionRadius = 0.3f;

	public float lookDegreeSpeed = 80f;

	public float maxSlope = 45f;

	public BaseAIMovement()
	{
	}

	public virtual void DoMove(BasicWildLifeAI ai, ulong simMillis)
	{
	}

	public virtual float GetActualMovementSpeed()
	{
		return 0f;
	}

	public virtual void InitializeMovement(BasicWildLifeAI ai)
	{
	}

	public virtual bool IsStuck()
	{
		return false;
	}

	public virtual void ProcessNetworkUpdate(ref Vector3 origin, ref Quaternion rotation)
	{
		origin = origin;
		rotation = rotation;
	}

	public virtual void SetLookDirection(Vector3 worldDir)
	{
	}

	public virtual void SetMoveDirection(Vector3 worldDir, float speed)
	{
	}

	public virtual void SetMovePosition(Vector3 worldPos, float speed)
	{
	}

	public virtual void SetMoveTarget(GameObject target, float speed)
	{
	}

	public virtual void Stop()
	{
	}
}