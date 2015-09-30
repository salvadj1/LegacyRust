using EditorHooksPrivate;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class EditorHooks
{
	static EditorHooks()
	{
		Type type = Type.GetType("EditorHooksEditor, Assembly-CSharp-Editor");
		try
		{
			if (type != null)
			{
				type.TypeInitializer.Invoke(null, null);
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(exception);
		}
	}

	[Conditional("UNITY_EDITOR")]
	public static void SetDirty(UnityEngine.Object obj)
	{
		if (Hooks._SetDirty != null)
		{
			Hooks._SetDirty(obj);
		}
	}
}