using System;

public class chat : ConsoleSystem
{
	[Admin]
	[Client]
	[Help("Enable or disable chat displaying", "")]
	public static bool enabled;

	static chat()
	{
		chat.enabled = true;
	}

	public chat()
	{
	}

	[Client]
	public static void @add(ref ConsoleSystem.Arg arg)
	{
		if (!chat.enabled)
		{
			return;
		}
		string str = arg.GetString(0, string.Empty);
		string str1 = arg.GetString(1, string.Empty);
		if (str == string.Empty || str1 == string.Empty)
		{
			return;
		}
		ChatUI.AddLine(str, str1);
	}
}