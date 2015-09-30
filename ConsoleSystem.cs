using Facepunch.Util;
using Facepunch.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public class ConsoleSystem
{
	private static bool RegisteredLogCallback;

	private static bool LogCallbackWritesToConsole;

	private static Application.LogCallback LogCallback;

	public ConsoleSystem()
	{
	}

	public static string CollectSavedFields(Type type)
	{
		string empty = string.Empty;
		FieldInfo[] fields = type.GetFields();
		for (int i = 0; i < (int)fields.Length; i++)
		{
			if (fields[i].IsStatic)
			{
				if (Reflection.HasAttribute(fields[i], typeof(ConsoleSystem.Saved)))
				{
					string str = string.Concat(type.Name, ".");
					if (str == "global.")
					{
						str = string.Empty;
					}
					string str1 = empty;
					empty = string.Concat(new string[] { str1, str, fields[i].Name, " ", fields[i].GetValue(null).ToString(), "\n" });
				}
			}
		}
		return empty;
	}

	public static string CollectSavedFunctions(Type type)
	{
		string empty = string.Empty;
		MethodInfo[] methods = type.GetMethods();
		for (int i = 0; i < (int)methods.Length; i++)
		{
			if (methods[i].IsStatic)
			{
				if (Reflection.HasAttribute(methods[i], typeof(ConsoleSystem.Saved)))
				{
					if (methods[i].ReturnType == typeof(string))
					{
						empty = string.Concat(empty, methods[i].Invoke(null, null));
					}
				}
			}
		}
		return empty;
	}

	public static string CollectSavedProperties(Type type)
	{
		string empty = string.Empty;
		PropertyInfo[] properties = type.GetProperties();
		for (int i = 0; i < (int)properties.Length; i++)
		{
			if (properties[i].GetGetMethod().IsStatic)
			{
				if (Reflection.HasAttribute(properties[i], typeof(ConsoleSystem.Saved)))
				{
					string str = string.Concat(type.Name, ".");
					if (str == "global.")
					{
						str = string.Empty;
					}
					string str1 = empty;
					empty = string.Concat(new string[] { str1, str, properties[i].Name, " ", properties[i].GetValue(null, null).ToString(), "\n" });
				}
			}
		}
		return empty;
	}

	public static Type[] FindTypes(string className)
	{
		List<Type> types = new List<Type>();
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < (int)assemblies.Length; i++)
		{
			Type type = assemblies[i].GetType(className);
			if (type != null)
			{
				if (type.IsSubclassOf(typeof(ConsoleSystem)))
				{
					types.Add(type);
				}
			}
		}
		return types.ToArray();
	}

	public static void Log(object message)
	{
		Debug.Log(message);
	}

	public static void Log(object message, UnityEngine.Object context)
	{
		Debug.Log(message, context);
	}

	public static void LogError(object message)
	{
		Debug.LogError(message);
	}

	public static void LogError(object message, UnityEngine.Object context)
	{
		Debug.LogError(message, context);
	}

	public static void LogException(Exception exception)
	{
		Debug.LogException(exception);
	}

	public static void LogException(Exception exception, UnityEngine.Object context)
	{
		Debug.LogException(exception, context);
	}

	public static void LogWarning(object message)
	{
		Debug.LogWarning(message);
	}

	public static void LogWarning(object message, UnityEngine.Object context)
	{
		Debug.LogWarning(message, context);
	}

	public static void Print(object message, bool toLogFile = false)
	{
		ConsoleSystem.PrintLogType(LogType.Log, message, toLogFile);
	}

	public static void PrintError(object message, bool toLogFile = false)
	{
		ConsoleSystem.PrintLogType(LogType.Error, message, toLogFile);
	}

	private static void PrintLogType(LogType logType, string message, bool log = false)
	{
		if (global.logprint)
		{
			switch (logType)
			{
				case LogType.Error:
				{
					ConsoleSystem.LogError(message);
					return;
				}
				case LogType.Warning:
				{
					ConsoleSystem.LogWarning(message);
					return;
				}
				case LogType.Log:
				{
					ConsoleSystem.Log(message);
					return;
				}
			}
		}
		if (log && !ConsoleSystem.LogCallbackWritesToConsole)
		{
			try
			{
				((logType != LogType.Log ? Console.Error : Console.Out)).WriteLine("Print{0}:{1}", logType, message);
			}
			catch (Exception exception)
			{
				Console.Error.WriteLine("PrintLogType Log Exception\n:{0}", exception);
			}
		}
		if (ConsoleSystem.RegisteredLogCallback)
		{
			try
			{
				ConsoleSystem.LogCallback(message, string.Empty, logType);
			}
			catch (Exception exception1)
			{
				Console.Error.WriteLine("PrintLogType Exception\n:{0}", exception1);
			}
		}
	}

	private static void PrintLogType(LogType logType, object obj, bool log = false)
	{
		LogType logType1 = logType;
		object obj1 = obj;
		if (obj1 == null)
		{
			obj1 = "Null";
		}
		ConsoleSystem.PrintLogType(logType1, string.Concat(obj1), log);
	}

	public static void PrintWarning(object message, bool toLogFile = false)
	{
		ConsoleSystem.PrintLogType(LogType.Warning, message, toLogFile);
	}

	public static void RegisterLogCallback(Application.LogCallback Callback, bool CallbackWritesToConsole = false)
	{
		if (!ConsoleSystem.RegisteredLogCallback)
		{
			if (!object.ReferenceEquals(Callback, null))
			{
				Application.RegisterLogCallback(Callback);
				ConsoleSystem.RegisteredLogCallback = true;
				ConsoleSystem.LogCallbackWritesToConsole = CallbackWritesToConsole;
				ConsoleSystem.LogCallback = Callback;
			}
		}
		else if (Callback == ConsoleSystem.LogCallback)
		{
			ConsoleSystem.LogCallbackWritesToConsole = CallbackWritesToConsole;
		}
		else if (!object.ReferenceEquals(Callback, null))
		{
			Application.RegisterLogCallback(Callback);
			ConsoleSystem.LogCallback = Callback;
			ConsoleSystem.LogCallbackWritesToConsole = CallbackWritesToConsole;
		}
		else
		{
			Application.RegisterLogCallback(null);
			int num = 0;
			ConsoleSystem.RegisteredLogCallback = (bool)num;
			ConsoleSystem.LogCallbackWritesToConsole = (bool)num;
			ConsoleSystem.LogCallback = null;
		}
	}

	public static bool Run(string strCommand, bool bWantsFeedback = false)
	{
		string empty = string.Empty;
		bool flag = ConsoleSystem.RunCommand_Clientside(strCommand, out empty, bWantsFeedback);
		if (empty.Length > 0)
		{
			Debug.Log(empty);
		}
		return flag;
	}

	public static bool RunCommand(ref ConsoleSystem.Arg arg, bool bWantReply = true)
	{
		bool flag;
		Type[] typeArray = ConsoleSystem.FindTypes(arg.Class);
		if ((int)typeArray.Length == 0)
		{
			if (bWantReply)
			{
				arg.ReplyWith(string.Concat("Console class not found: ", arg.Class));
			}
			return false;
		}
		if (bWantReply)
		{
			arg.ReplyWith(string.Concat(new string[] { "command ", arg.Class, ".", arg.Function, " was executed" }));
		}
		Type[] typeArray1 = typeArray;
		for (int i = 0; i < (int)typeArray1.Length; i++)
		{
			Type type = typeArray1[i];
			MethodInfo method = type.GetMethod(arg.Function);
			if (method != null && method.IsStatic)
			{
				if (!arg.CheckPermissions(method.GetCustomAttributes(true)))
				{
					if (bWantReply)
					{
						arg.ReplyWith(string.Concat("No permission: ", arg.Class, ".", arg.Function));
					}
					return false;
				}
				object[] objArray = new ConsoleSystem.Arg[] { arg };
				try
				{
					method.Invoke(null, objArray);
					arg = objArray[0] as ConsoleSystem.Arg;
					return true;
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					Debug.LogWarning(string.Concat(new string[] { "Error: ", arg.Class, ".", arg.Function, " - ", exception.Message }));
					arg.ReplyWith(string.Concat(new string[] { "Error: ", arg.Class, ".", arg.Function, " - ", exception.Message }));
					flag = false;
				}
				return flag;
			}
			FieldInfo field = type.GetField(arg.Function);
			if (field != null && field.IsStatic)
			{
				if (!arg.CheckPermissions(field.GetCustomAttributes(true)))
				{
					if (bWantReply)
					{
						arg.ReplyWith(string.Concat("No permission: ", arg.Class, ".", arg.Function));
					}
					return false;
				}
				Type fieldType = field.FieldType;
				if (arg.HasArgs(1))
				{
					try
					{
						string str = field.GetValue(null).ToString();
						if (fieldType == typeof(float))
						{
							field.SetValue(null, float.Parse(arg.Args[0]));
						}
						if (fieldType == typeof(int))
						{
							field.SetValue(null, int.Parse(arg.Args[0]));
						}
						if (fieldType == typeof(string))
						{
							field.SetValue(null, arg.Args[0]);
						}
						if (fieldType == typeof(bool))
						{
							field.SetValue(null, bool.Parse(arg.Args[0]));
						}
						if (bWantReply)
						{
							arg.ReplyWith(string.Concat(new string[] { arg.Class, ".", arg.Function, ": changed ", Facepunch.Utility.String.QuoteSafe(str), " to ", Facepunch.Utility.String.QuoteSafe(field.GetValue(null).ToString()), " (", fieldType.Name, ")" }));
						}
					}
					catch (Exception exception2)
					{
						if (bWantReply)
						{
							arg.ReplyWith(string.Concat("error setting value: ", arg.Class, ".", arg.Function));
						}
					}
				}
				else if (bWantReply)
				{
					arg.ReplyWith(string.Concat(new string[] { arg.Class, ".", arg.Function, ": ", Facepunch.Utility.String.QuoteSafe(field.GetValue(null).ToString()), " (", fieldType.Name, ")" }));
				}
				return true;
			}
			PropertyInfo property = type.GetProperty(arg.Function);
			if (property != null && property.GetGetMethod().IsStatic && property.GetSetMethod().IsStatic)
			{
				if (!arg.CheckPermissions(property.GetCustomAttributes(true)))
				{
					if (bWantReply)
					{
						arg.ReplyWith(string.Concat("No permission: ", arg.Class, ".", arg.Function));
					}
					return false;
				}
				Type propertyType = property.PropertyType;
				if (arg.HasArgs(1))
				{
					try
					{
						string str1 = property.GetValue(null, null).ToString();
						if (propertyType == typeof(float))
						{
							property.SetValue(null, float.Parse(arg.Args[0]), null);
						}
						if (propertyType == typeof(int))
						{
							property.SetValue(null, int.Parse(arg.Args[0]), null);
						}
						if (propertyType == typeof(string))
						{
							property.SetValue(null, arg.Args[0], null);
						}
						if (propertyType == typeof(bool))
						{
							property.SetValue(null, bool.Parse(arg.Args[0]), null);
						}
						if (bWantReply)
						{
							arg.ReplyWith(string.Concat(new string[] { arg.Class, ".", arg.Function, ": changed ", Facepunch.Utility.String.QuoteSafe(str1), " to ", Facepunch.Utility.String.QuoteSafe(property.GetValue(null, null).ToString()), " (", propertyType.Name, ")" }));
						}
					}
					catch (Exception exception3)
					{
						if (bWantReply)
						{
							arg.ReplyWith(string.Concat("error setting value: ", arg.Class, ".", arg.Function));
						}
					}
				}
				else if (bWantReply)
				{
					arg.ReplyWith(string.Concat(new string[] { arg.Class, ".", arg.Function, ": ", Facepunch.Utility.String.QuoteSafe(property.GetValue(null, null).ToString()), " (", propertyType.Name, ")" }));
				}
				return true;
			}
		}
		if (bWantReply)
		{
			arg.ReplyWith(string.Concat("Command not found: ", arg.Class, ".", arg.Function));
		}
		return false;
	}

	public static bool RunCommand_Clientside(string strCommand, out string StrOutput, bool bWantsFeedback = false)
	{
		StrOutput = string.Empty;
		ConsoleSystem.Arg arg = new ConsoleSystem.Arg(strCommand);
		if (arg.Invalid)
		{
			return false;
		}
		if (!ConsoleSystem.RunCommand(ref arg, bWantsFeedback))
		{
			return false;
		}
		if (arg.Reply != null && arg.Reply.Length > 0)
		{
			StrOutput = arg.Reply;
		}
		return true;
	}

	public static void RunFile(string strFile)
	{
		string[] strArrays = strFile.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			string str = strArrays[i];
			if (str[0] != '#')
			{
				ConsoleSystem.Run(str, false);
			}
		}
	}

	public static string SaveToConfigString()
	{
		string empty = string.Empty;
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < (int)assemblies.Length; i++)
		{
			Type[] types = assemblies[i].GetTypes();
			for (int j = 0; j < (int)types.Length; j++)
			{
				if (types[j].IsSubclassOf(typeof(ConsoleSystem)))
				{
					empty = string.Concat(empty, ConsoleSystem.CollectSavedFields(types[j]));
					empty = string.Concat(empty, ConsoleSystem.CollectSavedProperties(types[j]));
					empty = string.Concat(empty, ConsoleSystem.CollectSavedFunctions(types[j]));
				}
			}
		}
		return empty;
	}

	public static bool UnregisterLogCallback(Application.LogCallback Callback)
	{
		if (!ConsoleSystem.RegisteredLogCallback || !(Callback == ConsoleSystem.LogCallback))
		{
			return false;
		}
		ConsoleSystem.RegisterLogCallback(null, false);
		return true;
	}

	[AttributeUsage(AttributeTargets.All)]
	public sealed class Admin : Attribute
	{
		public Admin()
		{
		}
	}

	public class Arg
	{
		public string Class;

		public string Function;

		public string ArgsStr;

		public string[] Args;

		public bool Invalid;

		public string Reply;

		public Arg(string rconCommand)
		{
			rconCommand = ConsoleSystem.Arg.RemoveInvalidCharacters(rconCommand);
			if (rconCommand.IndexOf('.') <= 0 || rconCommand.IndexOf(' ', 0, rconCommand.IndexOf('.')) != -1)
			{
				rconCommand = string.Concat("global.", rconCommand);
			}
			if (rconCommand.IndexOf('.') <= 0)
			{
				return;
			}
			this.Class = rconCommand.Substring(0, rconCommand.IndexOf('.'));
			if (this.Class.Length <= 1)
			{
				return;
			}
			this.Class = this.Class.ToLower();
			this.Function = rconCommand.Substring(this.Class.Length + 1);
			if (this.Function.Length <= 1)
			{
				return;
			}
			this.Invalid = false;
			if (this.Function.IndexOf(' ') <= 0)
			{
				return;
			}
			this.ArgsStr = this.Function.Substring(this.Function.IndexOf(' '));
			this.ArgsStr = this.ArgsStr.Trim();
			this.Args = Facepunch.Utility.String.SplitQuotesStrings(this.ArgsStr);
			this.Function = this.Function.Substring(0, this.Function.IndexOf(' '));
			this.Function.ToLower();
		}

		public bool CheckPermissions(object[] attributes)
		{
			object[] objArray = attributes;
			for (int i = 0; i < (int)objArray.Length; i++)
			{
				if (objArray[i] is ConsoleSystem.Client)
				{
					return true;
				}
			}
			return false;
		}

		public bool GetBool(int iArg, bool def = false)
		{
			return ConsoleSystem.Parse.DefaultBool(this.GetString(iArg, null), def);
		}

		public Enum GetEnum(Type enumType, int iArg, Enum def)
		{
			return ConsoleSystem.Parse.DefaultEnum(enumType, this.GetString(iArg, null), def);
		}

		public float GetFloat(int iArg, float def = 0f)
		{
			return ConsoleSystem.Parse.DefaultFloat(this.GetString(iArg, null), def);
		}

		public int GetInt(int iArg, int def = 0)
		{
			return ConsoleSystem.Parse.DefaultInt(this.GetString(iArg, null), def);
		}

		public string GetString(int iArg, string def = "")
		{
			if (!this.HasArgs(iArg + 1))
			{
				return def;
			}
			return ConsoleSystem.Parse.DefaultString(this.Args[iArg], def);
		}

		public ulong GetUInt64(int iArg, ulong def = 0)
		{
			return ConsoleSystem.Parse.DefaultUInt64(this.GetString(iArg, null), def);
		}

		public bool HasArgs(int iMinimum = 1)
		{
			if (this.Args == null)
			{
				return false;
			}
			return (int)this.Args.Length >= iMinimum;
		}

		private static string RemoveInvalidCharacters(string str)
		{
			if (str == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < str.Length; i++)
			{
				char chr = str[i];
				if (char.IsLetterOrDigit(chr) || char.IsPunctuation(chr) || char.IsSeparator(chr) || char.IsSymbol(chr))
				{
					stringBuilder.Append(chr);
				}
			}
			return stringBuilder.ToString();
		}

		public void ReplyWith(string strValue)
		{
			this.Reply = strValue;
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	public sealed class Client : Attribute
	{
		public Client()
		{
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	public sealed class Help : Attribute
	{
		public string helpDescription;

		public string argsDescription;

		public Help(string strHelp, string strArgs = "")
		{
			this.helpDescription = strHelp;
			this.argsDescription = strArgs;
		}
	}

	public static class Parse
	{
		private const bool kEnumCaseInsensitive = true;

		public static bool AttemptBool(string text, out bool value)
		{
			decimal num;
			decimal num1;
			if (bool.TryParse(text, out value))
			{
				return true;
			}
			if (text.Length != 0)
			{
				if (!char.IsLetter(text[0]))
				{
					if (decimal.TryParse(text, out num1))
					{
						value = num1 != new decimal(0);
						return true;
					}
				}
				else if (text.Length == 4)
				{
					if (string.Equals(text, "true", StringComparison.InvariantCultureIgnoreCase))
					{
						value = true;
						return true;
					}
				}
				else if (text.Length == 5)
				{
					if (string.Equals(text, "false", StringComparison.InvariantCultureIgnoreCase))
					{
						value = false;
						return true;
					}
				}
				else if (decimal.TryParse(text, out num))
				{
					value = num != new decimal(0);
					return true;
				}
			}
			return false;
		}

		public static bool AttemptEnum<TEnum>(string text, out TEnum value)
		where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return ConsoleSystem.Parse.VerifyEnum<TEnum>.TryParse(text, out value);
		}

		public static bool AttemptEnum(Type enumType, string text, out Enum value)
		{
			bool flag;
			try
			{
				value = (Enum)Enum.Parse(enumType, text, true);
				flag = true;
			}
			catch
			{
				try
				{
					value = (Enum)Enum.ToObject(enumType, long.Parse(text));
					flag = true;
				}
				catch
				{
					value = null;
					flag = false;
				}
			}
			return flag;
		}

		public static bool AttemptFloat(string text, out float value)
		{
			return float.TryParse(text, out value);
		}

		public static bool AttemptInt(string text, out int value)
		{
			return int.TryParse(text, out value);
		}

		public static bool AttemptObject(Type type, string value, out object boxed)
		{
			bool flag;
			try
			{
				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Boolean:
					{
						if (typeof(bool) != type)
						{
							break;
						}
						else
						{
							boxed = ConsoleSystem.Parse.Bool(value);
							flag = true;
							return flag;
						}
					}
					case TypeCode.Char:
					case TypeCode.Double:
					case TypeCode.Decimal:
					case TypeCode.DateTime:
					case TypeCode.Object | TypeCode.DateTime:
					{
						break;
					}
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					{
						if (!type.IsEnum)
						{
							break;
						}
						else
						{
							boxed = ConsoleSystem.Parse.Enum(type, value);
							flag = true;
							return flag;
						}
					}
					case TypeCode.Int32:
					{
						if (type == typeof(int))
						{
							boxed = ConsoleSystem.Parse.Int(value);
						}
						else if (!type.IsEnum)
						{
							break;
						}
						else
						{
							boxed = ConsoleSystem.Parse.Enum(type, value);
						}
						flag = true;
						return flag;
					}
					case TypeCode.Single:
					{
						if (typeof(float) != type)
						{
							break;
						}
						else
						{
							boxed = ConsoleSystem.Parse.Float(value);
							flag = true;
							return flag;
						}
					}
					case TypeCode.String:
					{
						if (typeof(string) != type)
						{
							break;
						}
						else
						{
							boxed = ConsoleSystem.Parse.String(value);
							flag = true;
							return flag;
						}
					}
					default:
					{
						goto case TypeCode.Object | TypeCode.DateTime;
					}
				}
				boxed = null;
				return false;
			}
			catch (Exception exception)
			{
				boxed = exception;
				flag = false;
			}
			return flag;
		}

		public static bool AttemptString(string text, out string value)
		{
			if (string.IsNullOrEmpty(text))
			{
				value = string.Empty;
				return false;
			}
			value = text;
			return true;
		}

		public static bool Bool(string text)
		{
			bool flag;
			if (!ConsoleSystem.Parse.AttemptBool(text, out flag))
			{
				throw new FormatException("not in the correct format.");
			}
			return flag;
		}

		public static bool DefaultBool(string text, bool @default)
		{
			bool flag;
			if (object.ReferenceEquals(text, null) || !ConsoleSystem.Parse.AttemptBool(text, out flag))
			{
				flag = @default;
			}
			return flag;
		}

		public static bool DefaultBool(string text)
		{
			return ConsoleSystem.Parse.DefaultBool(text, false);
		}

		public static TEnum DefaultEnum<TEnum>(string text, TEnum @default)
		where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			TEnum tEnum;
			if (object.ReferenceEquals(text, null) || !ConsoleSystem.Parse.AttemptEnum<TEnum>(text, out tEnum))
			{
				tEnum = @default;
			}
			return tEnum;
		}

		public static TEnum DefaultEnum<TEnum>(string text)
		where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return ConsoleSystem.Parse.DefaultEnum<TEnum>(text, default(TEnum));
		}

		public static Enum DefaultEnum(Type enumType, string text, Enum @default)
		{
			Enum @enum;
			if (object.ReferenceEquals(text, null) || !ConsoleSystem.Parse.AttemptEnum(enumType, text, out @enum))
			{
				@enum = @default;
			}
			return @enum;
		}

		public static Enum DefaultEnum(Type enumType, string text)
		{
			return ConsoleSystem.Parse.DefaultEnum(enumType, text, null);
		}

		public static float DefaultFloat(string text, float @default)
		{
			float single;
			if (object.ReferenceEquals(text, null) || !ConsoleSystem.Parse.AttemptFloat(text, out single))
			{
				single = @default;
			}
			return single;
		}

		public static float DefaultFloat(string text)
		{
			return ConsoleSystem.Parse.DefaultFloat(text, 0f);
		}

		public static int DefaultInt(string text, int @default)
		{
			int num;
			if (object.ReferenceEquals(text, null) || !ConsoleSystem.Parse.AttemptInt(text, out num))
			{
				num = @default;
			}
			return num;
		}

		public static int DefaultInt(string text)
		{
			return ConsoleSystem.Parse.DefaultInt(text, 0);
		}

		public static string DefaultString(string text, string @default)
		{
			string str;
			if (!ConsoleSystem.Parse.AttemptString(text, out str))
			{
				str = @default;
			}
			return str;
		}

		public static string DefaultString(string text)
		{
			return ConsoleSystem.Parse.DefaultString(text, string.Empty);
		}

		public static ulong DefaultUInt64(string text, ulong @default)
		{
			if (text == null)
			{
				return @default;
			}
			ulong num = @default;
			ulong.TryParse(text, out num);
			return num;
		}

		public static TEnum Enum<TEnum>(string text)
		where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			return ConsoleSystem.Parse.VerifyEnum<TEnum>.Parse(text);
		}

		public static Enum Enum(Type enumType, string text)
		{
			Enum obj;
			try
			{
				obj = (Enum)Enum.Parse(enumType, text, true);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				try
				{
					obj = (Enum)Enum.ToObject(enumType, long.Parse(text));
				}
				catch
				{
					throw exception;
				}
			}
			return obj;
		}

		public static float Float(string text)
		{
			return float.Parse(text);
		}

		public static int Int(string text)
		{
			return int.Parse(text);
		}

		public static bool IsSupported(Type type)
		{
			if (object.ReferenceEquals(type, null))
			{
				return false;
			}
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
				{
					return typeof(bool) == type;
				}
				case TypeCode.Char:
				case TypeCode.Double:
				case TypeCode.Decimal:
				case TypeCode.DateTime:
				case TypeCode.Object | TypeCode.DateTime:
				{
					return false;
				}
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				{
					return type.IsEnum;
				}
				case TypeCode.Int32:
				{
					return (typeof(int) == type ? true : type.IsEnum);
				}
				case TypeCode.Single:
				{
					return typeof(float) == type;
				}
				case TypeCode.String:
				{
					return typeof(string) == type;
				}
				default:
				{
					return false;
				}
			}
		}

		public static bool IsSupported<T>()
		{
			return ConsoleSystem.Parse.PrecachedSupport<T>.IsSupported;
		}

		public static string String(string text)
		{
			if (object.ReferenceEquals(text, null))
			{
				throw new ArgumentNullException("text");
			}
			if (text.Length == 1)
			{
				throw new FormatException("Cannot use empty strings.");
			}
			return text;
		}

		private static class PrecachedSupport<T>
		{
			public readonly static bool IsSupported;

			static PrecachedSupport()
			{
				ConsoleSystem.Parse.PrecachedSupport<T>.IsSupported = ConsoleSystem.Parse.IsSupported(typeof(T));
			}
		}

		private static class VerifyEnum<TEnum>
		where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			static VerifyEnum()
			{
				if (!typeof(TEnum).IsEnum)
				{
					throw new ArgumentException("TEnum", "Is not a enum type");
				}
			}

			public static TEnum Parse(string text)
			{
				TEnum obj;
				try
				{
					obj = (TEnum)Enum.Parse(typeof(TEnum), text, true);
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					try
					{
						obj = (TEnum)Enum.ToObject(typeof(TEnum), long.Parse(text));
					}
					catch
					{
						throw exception;
					}
				}
				return obj;
			}

			public static bool TryParse(string text, out TEnum value)
			{
				bool flag;
				try
				{
					value = (TEnum)Enum.Parse(typeof(TEnum), text, true);
					flag = true;
				}
				catch
				{
					try
					{
						value = (TEnum)Enum.ToObject(typeof(TEnum), long.Parse(text));
						flag = true;
					}
					catch
					{
						value = default(TEnum);
						flag = false;
					}
				}
				return flag;
			}
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	public sealed class Saved : Attribute
	{
		public Saved()
		{
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	public sealed class User : Attribute
	{
		public User()
		{
		}
	}
}