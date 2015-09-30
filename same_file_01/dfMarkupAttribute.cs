using System;
using System.Runtime.CompilerServices;

public class dfMarkupAttribute
{
	public string Name
	{
		get;
		set;
	}

	public string Value
	{
		get;
		set;
	}

	public dfMarkupAttribute(string name, string value)
	{
		this.Name = name;
		this.Value = value;
	}

	public override string ToString()
	{
		return string.Format("{0}='{1}'", this.Name, this.Value);
	}
}