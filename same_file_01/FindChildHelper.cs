using System;
using UnityEngine;

public static class FindChildHelper
{
	private static Transform found;

	private static bool __FindChildByNameRecurse(string name, Transform parent)
	{
		if (parent.childCount == 0)
		{
			return false;
		}
		FindChildHelper.found = parent.Find(name);
		if (FindChildHelper.found)
		{
			return true;
		}
		int num = parent.childCount;
		for (int i = 0; i < num; i++)
		{
			Transform child = parent.GetChild(i);
			if (child.childCount > 0 && FindChildHelper.__FindChildByNameRecurse(name, child))
			{
				return true;
			}
		}
		return false;
	}

	private static bool _FindChildByNameRecurse(string name, Transform parent)
	{
		return FindChildHelper.__FindChildByNameRecurse(name, parent);
	}

	private static Transform _GetFound()
	{
		Transform transforms = FindChildHelper.found;
		FindChildHelper.found = null;
		return transforms;
	}

	[Obsolete("If this is being called in Start, Awake, or OnEnabled consider using the @PrefetchChildComponent on the variable.", false)]
	public static Transform FindChildByName(string name, Transform parent)
	{
		if (parent.name == name)
		{
			return parent;
		}
		if (FindChildHelper._FindChildByNameRecurse(name, parent))
		{
			return FindChildHelper._GetFound();
		}
		return FindChildHelper.NoChildNamed(name, parent);
	}

	[Obsolete("If this is being called in Start, Awake, or OnEnabled consider using the @PrefetchChildComponent on the variable.", false)]
	public static Transform FindChildByName(string name, GameObject parent)
	{
		if (parent.name == name)
		{
			return parent.transform;
		}
		if (FindChildHelper._FindChildByNameRecurse(name, parent.transform))
		{
			return FindChildHelper._GetFound();
		}
		return FindChildHelper.NoChildNamed(name, parent);
	}

	[Obsolete("If this is being called in Start, Awake, or OnEnabled consider using the @PrefetchChildComponent on the variable.", false)]
	public static Transform FindChildByName(string name, Component parent)
	{
		if (parent.name == name)
		{
			return parent.transform;
		}
		if (FindChildHelper._FindChildByNameRecurse(name, parent.transform))
		{
			return FindChildHelper._GetFound();
		}
		return FindChildHelper.NoChildNamed(name, parent);
	}

	public static Transform GetChildAtIndex(Transform transform, int i)
	{
		if (0 > i)
		{
			return null;
		}
		if (transform.childCount <= i)
		{
			return null;
		}
		return transform.GetChild(i);
	}

	private static Transform NoChildNamed(string name, UnityEngine.Object parent)
	{
		return null;
	}

	public static Transform RandomChild(Transform transform)
	{
		int num = transform.childCount;
		int num1 = num;
		if (num1 == 0)
		{
			return null;
		}
		if (num1 == 1)
		{
			return transform.GetChild(0);
		}
		return FindChildHelper.GetChildAtIndex(transform, UnityEngine.Random.Range(0, num));
	}
}