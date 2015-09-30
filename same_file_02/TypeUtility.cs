using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public static class TypeUtility
{
	private static bool ginit;

	private readonly static string[] hintsAQN;

	static TypeUtility()
	{
		TypeUtility.hintsAQN = new string[] { ", Version=", ", Culture=", ", PublicKeyToken=" };
	}

	private static bool ContainsAQN(string text)
	{
		int num = text.IndexOf(", ");
		if (num != -1)
		{
			for (int i = 0; i < (int)TypeUtility.hintsAQN.Length; i++)
			{
				if (text.IndexOf(TypeUtility.hintsAQN[i], num) != -1)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool Parse(string text, bool ignoreCase, out Type type)
	{
		type = Type.GetType(text, false, ignoreCase);
		if (object.ReferenceEquals(type, null))
		{
			return false;
		}
		return true;
	}

	private static bool Parse(string text, out Type type)
	{
		if (TypeUtility.Parse(text, false, out type))
		{
			return true;
		}
		if (!TypeUtility.ContainsAQN(text))
		{
			return TypeUtility.Parse(text, true, out type);
		}
		string str = TypeUtility.g.StrippedName(text);
		return (TypeUtility.Parse(str, false, out type) || TypeUtility.Parse(text, true, out type) ? true : TypeUtility.Parse(str, true, out type));
	}

	private static bool Parse(Type requiredBase, string text, bool ignoreCase, out Type type)
	{
		if (TypeUtility.Parse(text, ignoreCase, out type))
		{
			if (requiredBase.IsAssignableFrom(type))
			{
				return true;
			}
			type = null;
		}
		return false;
	}

	private static bool Parse(Type requiredType, string text, out Type type)
	{
		if (TypeUtility.Parse(requiredType, text, false, out type))
		{
			return true;
		}
		if (!TypeUtility.ContainsAQN(text))
		{
			return TypeUtility.Parse(requiredType, text, true, out type);
		}
		string str = TypeUtility.g.StrippedName(text);
		return (TypeUtility.Parse(requiredType, str, false, out type) || TypeUtility.Parse(requiredType, text, true, out type) ? true : TypeUtility.Parse(requiredType, str, true, out type));
	}

	public static Type Parse(string text)
	{
		Type type;
		if (object.ReferenceEquals(text, null))
		{
			throw new ArgumentNullException("text");
		}
		if (text.Length == 0)
		{
			throw new ArgumentException("text.Length==0", "text");
		}
		if (!TypeUtility.Parse(text, out type))
		{
			throw new ArgumentException("could not get type", text);
		}
		return type;
	}

	public static Type Parse<TRequiredBaseClass>(string text)
	where TRequiredBaseClass : class
	{
		Type type;
		if (object.ReferenceEquals(text, null))
		{
			throw new ArgumentNullException("text");
		}
		if (text.Length == 0)
		{
			throw new ArgumentException("text.Length==0", "text");
		}
		if (!TypeUtility.Parse(typeof(TRequiredBaseClass), text, out type))
		{
			throw new ArgumentException(string.Concat("could not get type that would match base class ", typeof(TRequiredBaseClass)), text);
		}
		return type;
	}

	public static bool TryParse(string text, out Type type)
	{
		if (!string.IsNullOrEmpty(text))
		{
			return TypeUtility.Parse(text, out type);
		}
		type = null;
		return false;
	}

	public static bool TryParse<TRequiredBaseClass>(string text, out Type type)
	where TRequiredBaseClass : class
	{
		if (string.IsNullOrEmpty(text))
		{
			type = null;
			return false;
		}
		return TypeUtility.Parse(typeof(TRequiredBaseClass), text, out type);
	}

	public static string VersionlessName(this Type type)
	{
		if (object.ReferenceEquals(type, null))
		{
			return null;
		}
		return TypeUtility.g.StrippedName(type);
	}

	public static string VersionlessName<T>()
	{
		return typeof(T).VersionlessName();
	}

	private static class g
	{
		private readonly static Dictionary<Type, string> strippedNames;

		static g()
		{
			TypeUtility.ginit = true;
			TypeUtility.g.strippedNames = new Dictionary<Type, string>();
		}

		public static string StrippedName(Type type)
		{
			string str;
			if (!TypeUtility.g.strippedNames.TryGetValue(type, out str))
			{
				Dictionary<Type, string> types = TypeUtility.g.strippedNames;
				string str1 = TypeUtility.g.expression.replace(type.AssemblyQualifiedName);
				str = str1;
				types[type] = str1;
			}
			return str;
		}

		public static string StrippedName(string assemblyQualifiedName)
		{
			return TypeUtility.g.expression.replace(assemblyQualifiedName);
		}

		private static class expression
		{
			private const RegexOptions kRegexOptions = RegexOptions.Compiled;

			public readonly static Regex version;

			public readonly static Regex culture;

			public readonly static Regex publicKeyToken;

			static expression()
			{
				TypeUtility.g.expression.version = new Regex(", Version=\\d+.\\d+.\\d+.\\d+", RegexOptions.Compiled);
				TypeUtility.g.expression.culture = new Regex(", Culture=\\w+", RegexOptions.Compiled);
				TypeUtility.g.expression.publicKeyToken = new Regex(", PublicKeyToken=\\w+", RegexOptions.Compiled);
			}

			public static string replace(string assemblyQualifiedName)
			{
				return TypeUtility.g.expression.version.Replace(assemblyQualifiedName, string.Empty);
			}
		}
	}
}