using Rust.Steam;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SteamClient : MonoBehaviour
{
	public static GameObject steamClientObject;

	protected static Vector3 vOldPosition;

	static SteamClient()
	{
		SteamClient.vOldPosition = Vector3.zero;
	}

	public SteamClient()
	{
	}

	public static void Create()
	{
		if (!SteamClient.SteamClient_Init())
		{
			Application.Quit();
			return;
		}
		SteamClient.steamClientObject = new GameObject();
		UnityEngine.Object.DontDestroyOnLoad(SteamClient.steamClientObject);
		SteamClient.steamClientObject.AddComponent<SteamClient>();
		SteamClient.steamClientObject.name = "SteamClient";
	}

	public static void Needed()
	{
		if (SteamClient.steamClientObject != null)
		{
			return;
		}
		SteamClient.Create();
	}

	public void OnDestroy()
	{
		SteamClient.SteamClient_Shutdown();
	}

	public void Start()
	{
		SteamGroups.Init();
	}

	[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
	public static extern void SteamClient_Cycle();

	[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
	public static extern bool SteamClient_Init();

	[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
	public static extern void SteamClient_OnJoinServer(string strHost, int iIP);

	[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
	public static extern void SteamClient_Shutdown();

	[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
	public static extern void SteamUser_AdvertiseGame(ulong serverid, uint serverip, int serverport);

	public void Update()
	{
		SteamClient.SteamClient_Cycle();
	}
}