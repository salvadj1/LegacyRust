using System;

public class RustPlayerClient : PlayerClient
{
	public RustPlayerClient()
	{
	}

	protected override void ClientInput()
	{
		if (MainMenu.IsVisible())
		{
			return;
		}
		if (ConsoleWindow.IsVisible())
		{
			return;
		}
		base.ClientInput();
	}
}