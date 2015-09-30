using System;
using UnityEngine;

internal class net : ConsoleSystem
{
	public net()
	{
	}

	[Client]
	[Help("connect to a server", "string serverurl")]
	public static void connect(ref ConsoleSystem.Arg arg)
	{
		if (UnityEngine.Object.FindObjectOfType(typeof(ClientConnect)))
		{
			Debug.Log("Connect already in progress!");
			return;
		}
		if (NetCull.isClientRunning)
		{
			Debug.Log("Use net.disconnect before trying to connect to a new server.");
			return;
		}
		string[] strArrays = arg.GetString(0, string.Empty).Split(new char[] { ':' });
		if ((int)strArrays.Length != 2)
		{
			Debug.Log("Not a valid ip - or port missing");
			return;
		}
		string str = strArrays[0];
		int num = int.Parse(strArrays[1]);
		Debug.Log(string.Concat(new object[] { "Connecting to ", str, ":", num }));
		PlayerPrefs.SetString("net.lasturl", arg.GetString(0, string.Empty));
		if (!ClientConnect.Instance().DoConnect(str, num))
		{
			return;
		}
		LoadingScreen.Show();
		LoadingScreen.Update("connecting..");
	}

	[Client]
	[Help("disconnect from server", "")]
	public static void disconnect(ref ConsoleSystem.Arg arg)
	{
		if (NetCull.isClientRunning)
		{
			NetCull.Disconnect();
			return;
		}
		Debug.Log("You're not connected to a server.");
	}

	[Client]
	[Help("reconnect to last server", "")]
	public static void reconnect(ref ConsoleSystem.Arg arg)
	{
		if (!PlayerPrefs.HasKey("net.lasturl"))
		{
			Debug.Log("You havn't connected to a server yet");
		}
		else
		{
			ConsoleSystem.Run(string.Concat("net.connect ", PlayerPrefs.GetString("net.lasturl")), false);
		}
	}
}