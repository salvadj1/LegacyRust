using System;
using System.Text.RegularExpressions;

namespace Facepunch.Utility
{
	public static class String
	{
		public static string QuoteSafe(string str)
		{
			str = str.Replace("\"", "\\\"");
			str = str.TrimEnd(new char[] { '\\' });
			return string.Concat("\"", str, "\"");
		}

		public static string[] SplitQuotesStrings(string input)
		{
			input = input.Replace("\\\"", "&qute;");
			MatchCollection matchCollections = (new Regex("\"([^\"]+)\"|'([^']+)'|\\S+", RegexOptions.Compiled)).Matches(input);
			string[] strArrays = new string[matchCollections.Count];
			for (int i = 0; i < matchCollections.Count; i++)
			{
				strArrays[i] = matchCollections[i].Groups[0].Value.Trim(new char[] { ' ', '\"' });
				strArrays[i] = strArrays[i].Replace("&qute;", "\"");
			}
			return strArrays;
		}
	}
}