using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using uLink;
using UnityEngine;

public class ClientConnect : UnityEngine.MonoBehaviour
{
	[NonSerialized]
	private GameObject levelLoader;

	public ClientConnect()
	{
	}

	public bool DoConnect(string strURL, int iPort)
	{
		unsafe
		{
			bool flag;
			SteamClient.Needed();
			NetCull.config.timeoutDelay = 60f;
			if (ClientConnect.Steam_GetSteamID() == 0)
			{
				LoadingScreen.Update("connection failed (no steam detected)");
				UnityEngine.Object.Destroy(base.gameObject);
				return false;
			}
			byte[] numArray = new byte[1024];
			IntPtr intPtr = Marshal.AllocHGlobal(1024);
			uint num = ClientConnect.SteamClient_GetAuth(intPtr, 1024);
			byte[] numArray1 = new byte[num];
			Marshal.Copy(intPtr, numArray1, 0, (int)num);
			Marshal.FreeHGlobal(intPtr);
			uLink.BitStream bitStream = new uLink.BitStream(false);
			bitStream.WriteInt32(1069);
			bitStream.WriteByte(2);
			bitStream.WriteUInt64(ClientConnect.Steam_GetSteamID());
			bitStream.WriteString(Marshal.PtrToStringAnsi(ClientConnect.Steam_GetDisplayname()));
			bitStream.WriteBytes(numArray1);
			try
			{
				NetError netError = NetCull.Connect(strURL, iPort, string.Empty, new object[] { bitStream });
				if (netError == NetError.NoError)
				{
					SteamClient.SteamClient_OnJoinServer(strURL, iPort);
					return true;
				}
				else
				{
					LoadingScreen.Update(string.Concat("connection failed (", netError, ")"));
					UnityEngine.Object.Destroy(base.gameObject);
					flag = false;
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				UnityEngine.Object.Destroy(base.gameObject);
				flag = false;
			}
			return flag;
		}
	}

	public static ClientConnect Instance()
	{
		GameObject gameObject = new GameObject();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		return gameObject.AddComponent<ClientConnect>();
	}

	[DebuggerHidden]
	private IEnumerator LoadLevel(string levelName)
	{
		ClientConnect.<LoadLevel>c__Iterator3F variable = null;
		return variable;
	}

	[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
	public static extern IntPtr Steam_GetDisplayname();

	[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
	public static extern ulong Steam_GetSteamID();

	[DllImport("librust", CharSet=CharSet.None, ExactSpelling=false)]
	public static extern uint SteamClient_GetAuth(IntPtr pData, int iMaxLength);

	private void uLink_OnConnectedToServer()
	{
		LoadingScreen.Update("connected!");
		uLink.BitStream bitStream = new uLink.BitStream((byte[])NetCull.approvalData.ReadObject(typeof(byte[]).TypeHandle, new object[0]), false);
		string str = bitStream.ReadString();
		NetCull.sendRate = bitStream.ReadSingle();
		string str1 = bitStream.ReadString();
		bitStream.ReadBoolean();
		bitStream.ReadBoolean();
		if (bitStream.bytesRemaining > 8)
		{
			ulong num = bitStream.ReadUInt64();
			SteamClient.SteamUser_AdvertiseGame(num, bitStream.ReadUInt32(), bitStream.ReadInt32());
		}
		UnityEngine.Debug.Log(string.Concat("Server Name: \"", str1, "\""));
		UnityEngine.Debug.Log(string.Concat("Level Name: \"", str, "\""));
		UnityEngine.Debug.Log(string.Concat("Send Rate: ", NetCull.sendRate));
		NetCull.isMessageQueueRunning = false;
		base.StartCoroutine(this.LoadLevel(str));
		DisableOnConnectedState.OnConnected();
	}

	private void uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection netDisconnect)
	{
		if (this.levelLoader)
		{
			UnityEngine.Object.Destroy(this.levelLoader);
		}
		try
		{
			DisableOnConnectedState.OnDisconnected();
		}
		finally
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void uLink_OnFailedToConnect(uLink.NetworkConnectionError ulink_error)
	{
		if (this.levelLoader)
		{
			UnityEngine.Object.Destroy(this.levelLoader);
		}
		if (!MainMenu.singleton)
		{
			NetError netError = ulink_error.ToNetError();
			if (netError != NetError.NoError)
			{
				UnityEngine.Debug.LogError(netError.NiceString());
			}
		}
		try
		{
			DisableOnConnectedState.OnDisconnected();
		}
		finally
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}