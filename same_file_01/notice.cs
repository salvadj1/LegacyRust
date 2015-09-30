using System;
using UnityEngine;

public class notice : ConsoleSystem
{
	public notice()
	{
	}

	[Client]
	public static void inventory(ref ConsoleSystem.Arg arg)
	{
		string str = arg.GetString(0, "This is the text");
		PopupUI.singleton.CreateInventory(str);
	}

	[Client]
	public static void popup(ref ConsoleSystem.Arg arg)
	{
		float num = arg.GetFloat(0, 2f);
		string str = arg.GetString(1, "!");
		string str1 = arg.GetString(2, "This is the text");
		PopupUI.singleton.CreateNotice(num, str, str1);
	}

	[Client]
	public static void test(ref ConsoleSystem.Arg arg)
	{
		PopupUI.singleton.StartCoroutine("DoTests");
	}
}