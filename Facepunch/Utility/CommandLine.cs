using System;
using System.Collections.Generic;

namespace Facepunch.Utility
{
	public static class CommandLine
	{
		private static bool initialized;

		private static string commandline;

		private static Dictionary<string, string> switches;

		static CommandLine()
		{
			CommandLine.initialized = false;
			CommandLine.commandline = string.Empty;
			CommandLine.switches = new Dictionary<string, string>();
		}

		public static void Force(string val)
		{
			CommandLine.commandline = val;
			CommandLine.initialized = false;
		}

		public static string GetSwitch(string strName, string strDefault)
		{
			CommandLine.Initalize();
			string empty = string.Empty;
			if (!CommandLine.switches.TryGetValue(strName, out empty))
			{
				return strDefault;
			}
			return empty;
		}

		public static int GetSwitchInt(string strName, int iDefault)
		{
			CommandLine.Initalize();
			string empty = string.Empty;
			if (!CommandLine.switches.TryGetValue(strName, out empty))
			{
				return iDefault;
			}
			int num = iDefault;
			if (!int.TryParse(empty, out num))
			{
				return iDefault;
			}
			return num;
		}

		public static bool HasSwitch(string strName)
		{
			return CommandLine.switches.ContainsKey(strName);
		}

		private static void Initalize()
		{
			if (CommandLine.initialized)
			{
				return;
			}
			CommandLine.initialized = true;
			if (CommandLine.commandline == string.Empty)
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				for (int i = 0; i < (int)commandLineArgs.Length; i++)
				{
					string str = commandLineArgs[i];
					CommandLine.commandline = string.Concat(CommandLine.commandline, "\"", str, "\" ");
				}
			}
			if (CommandLine.commandline == string.Empty)
			{
				return;
			}
			string empty = string.Empty;
			string[] strArrays = Facepunch.Utility.String.SplitQuotesStrings(CommandLine.commandline);
			for (int j = 0; j < (int)strArrays.Length; j++)
			{
				string str1 = strArrays[j];
				if (str1.Length != 0)
				{
					if (str1[0] == '-' || str1[0] == '+')
					{
						if (empty != string.Empty && !CommandLine.switches.ContainsKey(empty))
						{
							CommandLine.switches.Add(empty, string.Empty);
						}
						empty = str1;
					}
					else if (empty != string.Empty)
					{
						if (!CommandLine.switches.ContainsKey(empty))
						{
							CommandLine.switches.Add(empty, str1);
						}
						empty = string.Empty;
					}
				}
			}
			if (empty != string.Empty && !CommandLine.switches.ContainsKey(empty))
			{
				CommandLine.switches.Add(empty, string.Empty);
			}
		}
	}
}