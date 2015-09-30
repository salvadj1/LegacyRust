using Facepunch.Build;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public class ClientInit : UnityEngine.MonoBehaviour
{
	public ClientInit()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		SteamClient.Create();
		ConsoleSystem.Run("config.load", false);
		ConsoleSystem.Run("serverfavourite.load", false);
		HudEnabled.Disable();
		DatablockDictionary.Initialize();
		Application.LoadLevelAdditive("GameUI");
		Connection.GameLoaded();
	}

	[DebuggerHidden]
	private IEnumerator uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection netDisconnect)
	{
		return new ClientInit.<uLink_OnDisconnectedFromServer>c__Iterator40();
	}
}