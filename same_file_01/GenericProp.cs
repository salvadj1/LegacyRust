using System;

public class GenericProp : IDMain
{
	public GenericProp() : this(IDFlags.Unknown)
	{
	}

	protected GenericProp(IDFlags idFlags) : base(idFlags)
	{
	}
}