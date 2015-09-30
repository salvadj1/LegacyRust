using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AuthorSuiteCreationAttribute : Attribute
{
	public string Description
	{
		get;
		set;
	}

	public Type OutputType
	{
		get;
		set;
	}

	public bool Ready
	{
		get;
		set;
	}

	public string Scripter
	{
		get;
		set;
	}

	public string Title
	{
		get;
		set;
	}

	public AuthorSuiteCreationAttribute()
	{
	}
}