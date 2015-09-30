using System;
using System.Collections.Generic;

public class SaveStringPool
{
	private static Dictionary<string, int> prefabDictionary;

	static SaveStringPool()
	{
		SaveStringPool.prefabDictionary = new Dictionary<string, int>()
		{
			{ "StructureMasterPrefab", 1 }
		};
	}

	public SaveStringPool()
	{
	}

	public static string Convert(int iNum)
	{
		string key;
		Dictionary<string, int>.Enumerator enumerator = SaveStringPool.prefabDictionary.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, int> current = enumerator.Current;
				if (current.Value != iNum)
				{
					continue;
				}
				key = current.Key;
				return key;
			}
			return string.Empty;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return key;
	}

	public static int GetInt(string strName)
	{
		return SaveStringPool.prefabDictionary[strName];
	}
}