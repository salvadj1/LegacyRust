using System;
using System.Runtime.CompilerServices;

public static class dfStringExtensions
{
	public static bool Contains(this string value, string pattern, bool caseInsensitive)
	{
		if (!caseInsensitive)
		{
			return value.IndexOf(pattern) != -1;
		}
		return value.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) != -1;
	}

	public static string MakeRelativePath(this string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return string.Empty;
		}
		return path.Substring(path.IndexOf("Assets/", StringComparison.InvariantCultureIgnoreCase));
	}
}