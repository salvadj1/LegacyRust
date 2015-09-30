using System;
using System.IO;

public class config : ConsoleSystem
{
	public const string defaultConfig = "\r\necho default config\r\ninput.bind Left A None\r\ninput.bind Right D None\r\ninput.bind Up W None\r\ninput.bind Down S None\r\ninput.bind Jump Space None\r\ninput.bind Duck LeftControl None\r\ninput.bind Sprint LeftShift None\r\ninput.bind Fire Mouse0 None\r\ninput.bind AltFire Mouse1 None\r\ninput.bind Reload R None\r\ninput.bind Use E None\r\ninput.bind Inventory Tab None\r\ninput.bind Flashlight F None\r\ninput.bind Laser G None\r\ninput.bind Voice V None\r\ninput.bind Chat Return T\r\nrender.update\r\n";

	public config()
	{
	}

	public static string ConfigName()
	{
		return "cfg/client.cfg";
	}

	[Admin]
	[Client]
	[Help("Load the current config from config.cfg", "")]
	[User]
	public static void load(ref ConsoleSystem.Arg arg)
	{
		string str = config.ConfigName();
		string str1 = "\r\necho default config\r\ninput.bind Left A None\r\ninput.bind Right D None\r\ninput.bind Up W None\r\ninput.bind Down S None\r\ninput.bind Jump Space None\r\ninput.bind Duck LeftControl None\r\ninput.bind Sprint LeftShift None\r\ninput.bind Fire Mouse0 None\r\ninput.bind AltFire Mouse1 None\r\ninput.bind Reload R None\r\ninput.bind Use E None\r\ninput.bind Inventory Tab None\r\ninput.bind Flashlight F None\r\ninput.bind Laser G None\r\ninput.bind Voice V None\r\ninput.bind Chat Return T\r\nrender.update\r\n";
		if (File.Exists(str))
		{
			str1 = File.ReadAllText(str);
		}
		ConsoleSystem.RunFile(str1);
		arg.ReplyWith(string.Concat("Loaded ", str));
	}

	[Admin]
	[Client]
	[Help("Save the current config to config.cfg", "")]
	[User]
	public static void save(ref ConsoleSystem.Arg arg)
	{
		if (!Directory.Exists("cfg"))
		{
			Directory.CreateDirectory("cfg");
		}
		File.WriteAllText(config.ConfigName(), ConsoleSystem.SaveToConfigString());
		arg.ReplyWith("Saved config.cfg");
	}
}