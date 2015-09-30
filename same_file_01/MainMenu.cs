using Facepunch.Cursor;
using Facepunch.Utility;
using System;
using uLink;
using UnityEngine;

public class MainMenu : UnityEngine.MonoBehaviour
{
	public Camera blurCamera;

	public dfPanel screenServers;

	public dfPanel screenOptions;

	public UnlockCursorNode cursorManager;

	public static MainMenu singleton;

	public MainMenu()
	{
	}

	private void Awake()
	{
		MainMenu.singleton = this;
		LockCursorManager.onEscapeKey += new EscapeKeyEventHandler(this.Show);
		this.screenServers.Hide();
		this.screenOptions.Hide();
	}

	public void DoExit()
	{
		ConsoleSystem.Run("quit", false);
	}

	public void Hide()
	{
		base.GetComponent<dfPanel>().Hide();
		this.cursorManager.On = false;
		this.blurCamera.enabled = false;
		LoadingScreen.Hide();
		HudEnabled.Enable();
	}

	private void HideAllBut(dfPanel but)
	{
		if (this.screenServers && this.screenServers != but)
		{
			this.screenServers.Hide();
		}
		if (this.screenOptions && this.screenOptions != but)
		{
			this.screenOptions.Hide();
		}
	}

	public static bool IsVisible()
	{
		return (!MainMenu.singleton ? false : MainMenu.singleton.GetComponent<dfPanel>().IsVisible);
	}

	public void LoadBackground()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject.transform.parent.gameObject);
		Application.LoadLevel("MenuBackground");
	}

	private void LogDisconnect(NetError error, uLink.NetworkDisconnection? disconnection = null)
	{
		if (error != NetError.NoError)
		{
			Debug.LogWarning(error);
		}
		if (disconnection.HasValue)
		{
			Debug.Log(disconnection);
		}
	}

	private void OnDestroy()
	{
		LockCursorManager.onEscapeKey -= new EscapeKeyEventHandler(this.Show);
	}

	public void Show()
	{
		base.GetComponent<dfPanel>().Show();
		this.cursorManager.On = true;
		this.blurCamera.enabled = true;
		HudEnabled.Disable();
	}

	public void ShowInformation(string text)
	{
		ConsoleSystem.Run(string.Concat("notice.popup 5 \"\" ", Facepunch.Utility.String.QuoteSafe(text)), false);
	}

	public void ShowOptions()
	{
		this.HideAllBut(this.screenOptions);
		if (this.screenOptions)
		{
			if (!this.screenOptions.IsVisible)
			{
				this.screenOptions.Show();
			}
			else
			{
				this.screenOptions.Hide();
			}
			this.screenOptions.SendToBack();
		}
	}

	public void ShowServerlist()
	{
		this.HideAllBut(this.screenServers);
		if (this.screenServers)
		{
			if (!this.screenServers.IsVisible)
			{
				this.screenServers.Show();
			}
			else
			{
				this.screenServers.Hide();
			}
			this.screenServers.SendToBack();
		}
	}

	private void Start()
	{
		this.cursorManager = LockCursorManager.CreateCursorUnlockNode(false, "Main Menu");
		this.Show();
		if (!UnityEngine.Object.FindObjectOfType(typeof(ClientConnect)))
		{
			this.LoadBackground();
		}
	}

	private void uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection netDisconnect)
	{
		NetError lastKickReason = ServerManagement.GetLastKickReason(true);
		this.LogDisconnect(lastKickReason, new uLink.NetworkDisconnection?(netDisconnect));
		DisableOnConnectedState.OnDisconnected();
		ConsoleSystem.Run("gameui.show", false);
		this.LoadBackground();
		if (lastKickReason == NetError.NoError)
		{
			this.ShowInformation("Disconnected from server.");
		}
		else
		{
			this.ShowInformation(string.Concat("Disconnected (", lastKickReason.NiceString(), ")"));
		}
		LoadingScreen.Hide();
	}

	private void uLink_OnFailedToConnect(uLink.NetworkConnectionError ulink_error)
	{
		this.LogDisconnect(ulink_error.ToNetError(), null);
		DisableOnConnectedState.OnDisconnected();
		ConsoleSystem.Run("gameui.show", false);
		this.LoadBackground();
		if (ulink_error.ToNetError() == NetError.NoError)
		{
			this.ShowInformation("Failed to connect.");
		}
		else
		{
			this.ShowInformation(string.Concat("Failed to connect (", ulink_error.ToNetError().ToString(), ")"));
		}
		LoadingScreen.Hide();
	}

	private void Update()
	{
		if (NetCull.isClientRunning)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (!MainMenu.IsVisible())
				{
					this.Show();
				}
				else
				{
					this.Hide();
				}
			}
		}
	}
}