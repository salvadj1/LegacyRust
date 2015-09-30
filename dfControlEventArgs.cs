using System;
using System.Runtime.CompilerServices;

public class dfControlEventArgs
{
	public dfControl Source
	{
		get;
		private set;
	}

	public bool Used
	{
		get;
		private set;
	}

	internal dfControlEventArgs(dfControl Target)
	{
		this.Source = Target;
	}

	public void Use()
	{
		this.Used = true;
	}
}