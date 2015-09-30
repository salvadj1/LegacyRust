using Facepunch.Intersect;
using System;
using uLink;
using UnityEngine;

public struct QueuedShotDeathInfo
{
	public bool queued;

	public Vector3 localPoint;

	public Vector3 localNormal;

	public BodyPart bodyPart;

	public Transform transform;

	public bool exists
	{
		get
		{
			bool flag;
			if (!this.queued)
			{
				flag = false;
			}
			else
			{
				flag = this.transform;
			}
			return flag;
		}
	}

	public void LinkRagdoll(Transform thisRoot, GameObject ragdoll)
	{
		Transform transforms;
		if (!this.exists)
		{
			RagdollHelper.RecursiveLinkTransformsByName(ragdoll.transform, thisRoot);
		}
		else if (RagdollHelper.RecursiveLinkTransformsByName(ragdoll.transform, thisRoot, this.transform, out transforms))
		{
			Transform transforms1 = transforms;
			Rigidbody rigidbody = transforms1.rigidbody;
			if (rigidbody)
			{
				Vector3 vector3 = transforms1.TransformPoint(this.localPoint);
				Vector3 vector31 = transforms1.TransformDirection(this.localNormal);
				rigidbody.AddForceAtPosition(vector31 * 1000f, vector3);
			}
		}
	}

	public void Set(Character character, ref Vector3 localPoint, ref Angle2 localNormal, byte bodyPart, ref uLink.NetworkMessageInfo info)
	{
		this.Set(character.hitBoxSystem, ref localPoint, ref localNormal, bodyPart, ref info);
	}

	public void Set(IDMain idMain, ref Vector3 localPoint, ref Angle2 localNormal, byte bodyPart, ref uLink.NetworkMessageInfo info)
	{
		if (!(idMain is Character))
		{
			this.Set(idMain.GetRemote<HitBoxSystem>(), ref localPoint, ref localNormal, bodyPart, ref info);
		}
		else
		{
			this.Set((Character)idMain, ref localPoint, ref localNormal, bodyPart, ref info);
		}
	}

	public void Set(HitBoxSystem hitBoxSystem, ref Vector3 localPoint, ref Angle2 localNormal, byte bodyPart, ref uLink.NetworkMessageInfo info)
	{
		IDRemoteBodyPart dRemoteBodyPart;
		this.queued = true;
		this.localPoint = localPoint;
		this.localNormal = localNormal.forward;
		this.bodyPart = (BodyPart)bodyPart;
		if (this.bodyPart == BodyPart.Undefined)
		{
			this.transform = null;
		}
		else if (!hitBoxSystem.bodyParts.TryGetValue(this.bodyPart, out dRemoteBodyPart))
		{
			this.transform = null;
		}
		else
		{
			this.transform = dRemoteBodyPart.transform;
		}
	}
}