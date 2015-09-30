using System;

public class lockentry : ConsoleSystem
{
	public lockentry()
	{
	}

	[Client]
	public static void hide(ref ConsoleSystem.Arg arg)
	{
		LockEntry.Hide();
	}

	[Client]
	public static void show(ref ConsoleSystem.Arg arg)
	{
		bool flag = false;
		bool.TryParse(arg.Args[0], out flag);
		LockEntry.Show(flag);
	}
}