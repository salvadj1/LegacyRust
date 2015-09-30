using System;
using UnityEngine;

public class RunConsoleCommand : MonoBehaviour
{
	public string consoleCommand = "echo Missing Console Command!";

	public bool asIfTypedIntoConsole;

	public RunConsoleCommand()
	{
	}

	public void RunCommandImmediately()
	{
		if (this.asIfTypedIntoConsole)
		{
			ConsoleWindow.singleton.RunCommand(this.consoleCommand);
			return;
		}
		ConsoleSystem.Run(this.consoleCommand, false);
	}
}