using System;
using System.Reflection;

public class ConVar
{
	public ConVar()
	{
	}

	public static bool GetBool(string strName, bool strDefault)
	{
		bool num;
		object obj;
		string str = ConVar.GetString(strName, (!strDefault ? bool.FalseString : bool.TrueString));
		try
		{
			num = bool.Parse(str);
		}
		catch
		{
			string str1 = strName;
			if (!strDefault)
			{
				obj = null;
			}
			else
			{
				obj = 1;
			}
			num = ConVar.GetInt(str1, (float)obj) != 0;
		}
		return num;
	}

	public static float GetFloat(string strName, float strDefault)
	{
		string str = ConVar.GetString(strName, string.Empty);
		if (str.Length == 0)
		{
			return strDefault;
		}
		float single = strDefault;
		if (float.TryParse(str, out single))
		{
			return single;
		}
		return strDefault;
	}

	public static int GetInt(string strName, float strDefault)
	{
		return (int)ConVar.GetFloat(strName, strDefault);
	}

	public static string GetString(string strName, string strDefault)
	{
		ConsoleSystem.Arg arg = new ConsoleSystem.Arg(strName);
		if (arg.Invalid)
		{
			return strDefault;
		}
		Type[] typeArray = ConsoleSystem.FindTypes(arg.Class);
		if ((int)typeArray.Length == 0)
		{
			return strDefault;
		}
		Type[] typeArray1 = typeArray;
		for (int i = 0; i < (int)typeArray1.Length; i++)
		{
			Type type = typeArray1[i];
			FieldInfo field = type.GetField(arg.Function);
			if (field != null && field.IsStatic)
			{
				return field.GetValue(null).ToString();
			}
			PropertyInfo property = type.GetProperty(arg.Function);
			if (property != null && property.GetGetMethod().IsStatic)
			{
				return property.GetValue(null, null).ToString();
			}
		}
		return strDefault;
	}
}