using System;
using UnityEngine;

public class input : ConsoleSystem
{
	[Client]
	[Help("The mouse sensitivity. Default is 5.0", "")]
	[Saved]
	public static float mousespeed;

	[Client]
	[Help("Should we flip the mouse pitch movement? Default is false", "")]
	[Saved]
	public static bool flipy;

	static input()
	{
		input.mousespeed = 5f;
	}

	public input()
	{
	}

	[Client]
	[Help("Internal use only", "")]
	public static void bind(ref ConsoleSystem.Arg args)
	{
		if (!args.HasArgs(3))
		{
			return;
		}
		string str = args.Args[0];
		string str1 = args.Args[1];
		string str2 = args.Args[2];
		GameInput.GameButton button = GameInput.GetButton(str);
		if (button != null)
		{
			button.Bind(str1, str2);
		}
	}

	[Client]
	[Help("Internal use only", "")]
	public static void keys(ref ConsoleSystem.Arg args)
	{
		Debug.Log(GameInput.GetConfig());
	}

	[Saved]
	public static string save_bound_keys()
	{
		return GameInput.GetConfig();
	}
}