using System;

public static class GameEvent
{
	public static void DoPlayerConnected(PlayerClient player)
	{
		if (GameEvent.PlayerConnected != null)
		{
			GameEvent.PlayerConnected(player);
		}
	}

	public static void DoQualitySettingsRefresh()
	{
		if (GameEvent.QualitySettingsRefresh != null)
		{
			GameEvent.QualitySettingsRefresh();
		}
	}

	public static event GameEvent.OnPlayerConnectedHandler PlayerConnected;

	public static event GameEvent.OnGenericEvent QualitySettingsRefresh;

	public delegate void OnGenericEvent();

	public delegate void OnPlayerConnectedHandler(PlayerClient player);
}