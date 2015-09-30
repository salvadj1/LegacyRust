using System;

public class RustServerManagement : ServerManagement
{
	public RustServerManagement()
	{
	}

	public static new RustServerManagement Get()
	{
		return (RustServerManagement)ServerManagement.Get();
	}
}