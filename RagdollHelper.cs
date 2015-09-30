using Facepunch.Actor;
using System;
using UnityEngine;

public class RagdollHelper : MonoBehaviour
{
	public RagdollHelper()
	{
	}

	private static void _RecursiveLinkTransformsByName(Transform ragdoll, Transform body)
	{
		for (int i = 0; i < ragdoll.childCount; i++)
		{
			Transform childAtIndex = FindChildHelper.GetChildAtIndex(ragdoll, i);
			Transform transforms = FindChildHelper.FindChildByName(childAtIndex.name, body);
			if (transforms)
			{
				childAtIndex.position = transforms.position;
				childAtIndex.rotation = transforms.rotation;
			}
			RagdollHelper._RecursiveLinkTransformsByName(childAtIndex, body);
		}
	}

	private static void _RecursiveLinkTransformsByName(Transform ragdoll, Transform body, Transform bodyMatchTransform, ref Transform ragdollMatchTransform, ref bool foundMatch)
	{
		ragdollMatchTransform = null;
		for (int i = 0; i < ragdoll.childCount; i++)
		{
			Transform childAtIndex = FindChildHelper.GetChildAtIndex(ragdoll, i);
			Transform transforms = FindChildHelper.FindChildByName(childAtIndex.name, body);
			if (transforms)
			{
				childAtIndex.position = transforms.position;
				childAtIndex.rotation = transforms.rotation;
				if (!foundMatch && transforms == bodyMatchTransform)
				{
					foundMatch = true;
					ragdollMatchTransform = childAtIndex;
				}
				if (!foundMatch)
				{
					RagdollHelper._RecursiveLinkTransformsByName(childAtIndex, transforms, bodyMatchTransform, ref ragdollMatchTransform, ref foundMatch);
				}
				else
				{
					RagdollHelper._RecursiveLinkTransformsByName(childAtIndex, transforms);
				}
			}
		}
	}

	public static void RecursiveLinkTransformsByName(Transform ragdoll, Transform body)
	{
		BoneStructure component = body.GetComponent<BoneStructure>();
		if (component)
		{
			BoneStructure boneStructures = ragdoll.GetComponent<BoneStructure>();
			if (boneStructures)
			{
				BoneStructure.ParentDownOrdered.Enumerator enumerator = component.parentDown.GetEnumerator();
				try
				{
					BoneStructure.ParentDownOrdered.Enumerator enumerator1 = boneStructures.parentDown.GetEnumerator();
					try
					{
						while (enumerator.MoveNext() && enumerator1.MoveNext())
						{
							Transform current = enumerator.Current;
							Transform transforms = enumerator1.Current;
							transforms.position = current.position;
							transforms.rotation = current.rotation;
						}
					}
					finally
					{
						((IDisposable)(object)enumerator1).Dispose();
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				return;
			}
		}
		RagdollHelper._RecursiveLinkTransformsByName(ragdoll, body);
	}

	public static bool RecursiveLinkTransformsByName(Transform ragdoll, Transform body, Transform bodyMatchTransform, out Transform ragdollMatchTransform)
	{
		bool flag;
		if (!bodyMatchTransform)
		{
			ragdollMatchTransform = null;
			RagdollHelper.RecursiveLinkTransformsByName(ragdoll, body);
			return false;
		}
		if (body == bodyMatchTransform)
		{
			ragdollMatchTransform = ragdoll;
			RagdollHelper.RecursiveLinkTransformsByName(ragdoll, body);
			return true;
		}
		BoneStructure component = body.GetComponent<BoneStructure>();
		if (component)
		{
			BoneStructure boneStructures = ragdoll.GetComponent<BoneStructure>();
			if (boneStructures)
			{
				BoneStructure.ParentDownOrdered.Enumerator enumerator = component.parentDown.GetEnumerator();
				try
				{
					BoneStructure.ParentDownOrdered.Enumerator enumerator1 = boneStructures.parentDown.GetEnumerator();
					try
					{
						while (enumerator.MoveNext() && enumerator1.MoveNext())
						{
							Transform current = enumerator.Current;
							Transform transforms = enumerator1.Current;
							transforms.position = current.position;
							transforms.rotation = current.rotation;
							if (current != bodyMatchTransform)
							{
								continue;
							}
							ragdollMatchTransform = transforms;
							while (enumerator.MoveNext() && enumerator1.MoveNext())
							{
								current = enumerator.Current;
								transforms = enumerator1.Current;
								transforms.position = current.position;
								transforms.rotation = current.rotation;
							}
							flag = true;
							return flag;
						}
					}
					finally
					{
						((IDisposable)(object)enumerator1).Dispose();
					}
					ragdollMatchTransform = null;
					return false;
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				return flag;
			}
		}
		bool flag1 = false;
		ragdollMatchTransform = null;
		RagdollHelper._RecursiveLinkTransformsByName(ragdoll, body, bodyMatchTransform, ref ragdollMatchTransform, ref flag1);
		return flag1;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}