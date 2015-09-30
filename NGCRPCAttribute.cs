using System;

[AttributeUsage(AttributeTargets.Method, Inherited=true)]
public sealed class NGCRPCAttribute : Attribute
{
	public NGCRPCAttribute()
	{
	}
}