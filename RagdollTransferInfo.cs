using System;
using System.Text;
using UnityEngine;

public struct RagdollTransferInfo
{
	public readonly string headBoneName;

	public readonly Transform headBone;

	public readonly bool providedHeadBone;

	public readonly bool providedHeadBoneName;

	public RagdollTransferInfo(string headBoneName)
	{
		this.headBoneName = headBoneName;
		this.headBone = null;
		this.providedHeadBone = false;
		this.providedHeadBoneName = headBoneName != null;
	}

	public RagdollTransferInfo(Transform transform)
	{
		this.providedHeadBone = transform;
		this.providedHeadBoneName = false;
		this.headBoneName = null;
		this.headBone = transform;
	}

	public bool FindHead(Transform root, out Transform headBone)
	{
		Transform transforms;
		if (this.providedHeadBoneName)
		{
			Transform transforms1 = root.Find(this.headBoneName);
			transforms = transforms1;
			headBone = transforms1;
			return transforms;
		}
		if (!this.providedHeadBone || !this.headBone)
		{
			headBone = root;
			return false;
		}
		StringBuilder stringBuilder = new StringBuilder();
		RagdollTransferInfo.FindNameRecurse(this.headBone, stringBuilder);
		Transform transforms2 = root.Find(stringBuilder.ToString());
		transforms = transforms2;
		headBone = transforms2;
		return transforms;
	}

	private static void FindNameRecurse(Transform child, StringBuilder sb)
	{
		Transform transforms = child.parent;
		if (transforms)
		{
			RagdollTransferInfo.FindNameRecurse(transforms, sb);
			if (sb.Length <= 0)
			{
				sb.Append(child.name);
			}
			else
			{
				sb.Append('/');
			}
		}
	}

	public static implicit operator RagdollTransferInfo(string headBoneName)
	{
		return new RagdollTransferInfo(headBoneName);
	}

	public static implicit operator RagdollTransferInfo(Transform transform)
	{
		return new RagdollTransferInfo(transform);
	}
}