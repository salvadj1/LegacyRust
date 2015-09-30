using Facepunch;
using System;
using uLink;
using UnityEngine;

public class ConsoleNetworker : Facepunch.MonoBehaviour
{
	public static ConsoleNetworker singleton;

	public ConsoleNetworker()
	{
	}

	private void Awake()
	{
		ConsoleNetworker.singleton = this;
	}

	[RPC]
	public void CL_ConsoleCommand(string message, uLink.NetworkMessageInfo info)
	{
		if (!(ConsoleWindow)UnityEngine.Object.FindObjectOfType(typeof(ConsoleWindow)))
		{
			return;
		}
		if (!ConsoleSystem.Run(message, false))
		{
			Debug.Log(string.Concat("Unhandled command from server: ", message));
		}
	}

	[RPC]
	public void CL_ConsoleMessage(string message, uLink.NetworkMessageInfo info)
	{
		ConsoleWindow consoleWindow = (ConsoleWindow)UnityEngine.Object.FindObjectOfType(typeof(ConsoleWindow));
		if (!consoleWindow)
		{
			return;
		}
		consoleWindow.AddText(message, true);
	}

	public static void SendCommandToServer(string strCommand)
	{
		if (!ConsoleNetworker.singleton)
		{
			return;
		}
		ConsoleNetworker.singleton.networkView.RPC<string>("SV_RunConsoleCommand", uLink.RPCMode.Server, strCommand);
	}

	[RPC]
	public void SV_RunConsoleCommand(string cmd, uLink.NetworkMessageInfo info)
	{
	}
}