using System;
using System.Diagnostics;
using UnityEngine;

public class Assert
{
	public Assert()
	{
	}

	[Conditional("UNITY_EDITOR")]
	public static void Test(bool comparison, string message = "")
	{
		if (comparison)
		{
			return;
		}
	}

	[Conditional("UNITY_EDITOR")]
	public static void Throw(string message = "")
	{
		UnityEngine.Debug.LogError(message);
		UnityEngine.Debug.Break();
	}
}