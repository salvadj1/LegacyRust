using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FavouriteList
{
	private static List<string> faveList;

	static FavouriteList()
	{
		FavouriteList.faveList = new List<string>();
	}

	public static void Add(string strName)
	{
		if (FavouriteList.Contains(strName))
		{
			return;
		}
		if (strName.Length < 8)
		{
			return;
		}
		FavouriteList.faveList.Add(strName);
	}

	public static void Clear()
	{
		FavouriteList.faveList.Clear();
	}

	public static bool Contains(string strName)
	{
		return FavouriteList.faveList.Contains(strName);
	}

	public static void Load()
	{
		FavouriteList.Clear();
		if (!File.Exists("cfg/favourites.cfg"))
		{
			return;
		}
		string str = File.ReadAllText("cfg/favourites.cfg");
		if (string.IsNullOrEmpty(str))
		{
			return;
		}
		Debug.Log("Running cfg/favourites.cfg");
		ConsoleSystem.RunFile(str);
	}

	public static bool Remove(string strName)
	{
		if (!FavouriteList.Contains(strName))
		{
			return false;
		}
		return FavouriteList.faveList.Remove(strName);
	}

	public static void Save()
	{
		string empty = string.Empty;
		if (!Directory.Exists("cfg"))
		{
			Directory.CreateDirectory("cfg");
		}
		foreach (string str in FavouriteList.faveList)
		{
			empty = string.Concat(empty, "serverfavourite.add \"", str.ToString(), "\"\r\n");
			Debug.Log(string.Concat("serverfavourite.add \"", str.ToString(), "\"\r\n"));
		}
		File.WriteAllText("cfg/favourites.cfg", empty);
		Debug.Log(empty);
	}
}