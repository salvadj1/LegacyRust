using System;

public class serverfavourite : ConsoleSystem
{
	public serverfavourite()
	{
	}

	[Client]
	[Help("Adds a server to favourites", "")]
	public static void @add(ref ConsoleSystem.Arg arg)
	{
		FavouriteList.Add(arg.GetString(0, string.Empty));
	}

	[Client]
	[Help("Load fave list", "")]
	public static void load(ref ConsoleSystem.Arg arg)
	{
		FavouriteList.Load();
	}

	[Client]
	[Help("Removes a server to favourites", "")]
	public static void @remove(ref ConsoleSystem.Arg arg)
	{
		string str = arg.GetString(0, string.Empty);
		FavouriteList.Remove(str);
	}

	[Client]
	[Help("Save fave list", "")]
	public static void save(ref ConsoleSystem.Arg arg)
	{
		FavouriteList.Save();
	}
}