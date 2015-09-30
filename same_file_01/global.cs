using System;
using System.Reflection;
using UnityEngine;

public class global : ConsoleSystem
{
	[Client]
	[Help("When set to True, all console printing will go through Debug.Log", "")]
	[User]
	public static bool logprint;

	[Admin]
	[Client]
	[Help("Prints fps at said interval", "interval (seconds)")]
	[User]
	public static float fpslog;

	static global()
	{
		global.fpslog = -1f;
	}

	public global()
	{
	}

	public static string BuildFieldsString(ref FieldInfo field)
	{
		string str = "no help";
		object[] customAttributes = field.GetCustomAttributes(true);
		for (int i = 0; i < (int)customAttributes.Length; i++)
		{
			object obj = customAttributes[i];
			if (obj is ConsoleSystem.Help)
			{
				str = (obj as ConsoleSystem.Help).helpDescription;
			}
		}
		return string.Concat(field.Name, " : ", str);
	}

	public static string BuildMethodString(ref MethodInfo method)
	{
		string empty = string.Empty;
		string str = "no help";
		object[] customAttributes = method.GetCustomAttributes(true);
		for (int i = 0; i < (int)customAttributes.Length; i++)
		{
			object obj = customAttributes[i];
			if (obj is ConsoleSystem.Help)
			{
				empty = (obj as ConsoleSystem.Help).argsDescription;
				str = (obj as ConsoleSystem.Help).helpDescription;
				empty = string.Concat(" ", empty.Trim(), " ");
			}
		}
		return string.Concat(new string[] { method.Name, "(", empty, ") : ", str });
	}

	public static string BuildPropertyString(ref PropertyInfo field)
	{
		string str = "no help";
		object[] customAttributes = field.GetCustomAttributes(true);
		for (int i = 0; i < (int)customAttributes.Length; i++)
		{
			object obj = customAttributes[i];
			if (obj is ConsoleSystem.Help)
			{
				str = (obj as ConsoleSystem.Help).helpDescription;
			}
		}
		return string.Concat(field.Name, " : ", str);
	}

	[Client]
	[Help("Creates an error", "")]
	public static void create_error(ref ConsoleSystem.Arg arg)
	{
		Debug.LogError("this is an error");
	}

	[Admin]
	[Client]
	[Help("Prints something to the debug output", "string output")]
	[User]
	public static void echo(ref ConsoleSystem.Arg arg)
	{
		arg.ReplyWith(arg.ArgsStr);
	}

	[Admin]
	[Client]
	[Help("Search for a command", "string Name")]
	[User]
	public static void find(ref ConsoleSystem.Arg arg)
	{
		string str;
		if (!arg.HasArgs(1))
		{
			return;
		}
		string args = arg.Args[0];
		string empty = string.Empty;
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < (int)assemblies.Length; i++)
		{
			Type[] types = assemblies[i].GetTypes();
			for (int j = 0; j < (int)types.Length; j++)
			{
				if (types[j].IsSubclassOf(typeof(ConsoleSystem)))
				{
					MethodInfo[] methods = types[j].GetMethods();
					for (int k = 0; k < (int)methods.Length; k++)
					{
						if (methods[k].IsStatic)
						{
							if (!(args != "*") || types[j].Name.Contains(args) || methods[k].Name.Contains(args))
							{
								if (arg.CheckPermissions(methods[k].GetCustomAttributes(true)))
								{
									str = empty;
									empty = string.Concat(new string[] { str, types[j].Name, ".", global.BuildMethodString(ref methods[k]), "\n" });
								}
							}
						}
					}
					FieldInfo[] fields = types[j].GetFields();
					for (int l = 0; l < (int)fields.Length; l++)
					{
						if (fields[l].IsStatic)
						{
							if (!(args != "*") || types[j].Name.Contains(args) || fields[l].Name.Contains(args))
							{
								if (arg.CheckPermissions(fields[l].GetCustomAttributes(true)))
								{
									str = empty;
									empty = string.Concat(new string[] { str, types[j].Name, ".", global.BuildFieldsString(ref fields[l]), "\n" });
								}
							}
						}
					}
					PropertyInfo[] properties = types[j].GetProperties();
					for (int m = 0; m < (int)properties.Length; m++)
					{
						if (!(args != "*") || types[j].Name.Contains(args) || properties[m].Name.Contains(args))
						{
							if (arg.CheckPermissions(properties[m].GetCustomAttributes(true)))
							{
								str = empty;
								empty = string.Concat(new string[] { str, types[j].Name, ".", global.BuildPropertyString(ref properties[m]), "\n" });
							}
						}
					}
				}
			}
		}
		arg.ReplyWith(string.Concat("Finding ", args, ":\n", empty));
	}

	[Admin]
	[Client]
	[Help("Quits the game", "")]
	public static void quit(ref ConsoleSystem.Arg arg)
	{
		Application.Quit();
	}
}