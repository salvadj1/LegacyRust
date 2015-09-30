using System;
using System.Diagnostics;
using UnityEngine;

public class ServerQuitResponder : MonoBehaviour
{
	public ServerQuitResponder()
	{
	}

	[Conditional("ALLOW_SQR")]
	public static void WillChangeLevels()
	{
	}
}