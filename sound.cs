using System;
using UnityEngine;

public class sound : ConsoleSystem
{
	[Client]
	[Help("Global music volume", "")]
	[Saved]
	public static float music;

	[Client]
	[Help("Global sound volume", "")]
	[Saved]
	public static float volume
	{
		get
		{
			return AudioListener.volume;
		}
		set
		{
			AudioListener.volume = value;
		}
	}

	static sound()
	{
		sound.music = 0.4f;
	}

	public sound()
	{
	}
}