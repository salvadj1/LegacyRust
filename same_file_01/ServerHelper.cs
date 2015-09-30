using System;
using System.Diagnostics;
using UnityEngine;

public static class ServerHelper
{
	[Conditional("SERVER")]
	public static void SetupForServer(GameObject obj)
	{
	}
}