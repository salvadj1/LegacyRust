using System;

[AttributeUsage(AttributeTargets.Method, Inherited=false)]
public sealed class NGCRPCSkipAttribute : Attribute
{
	public NGCRPCSkipAttribute()
	{
	}
}