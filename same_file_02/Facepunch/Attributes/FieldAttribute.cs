using System;

namespace Facepunch.Attributes
{
	[AttributeUsage(AttributeTargets.Field, Inherited=true)]
	public abstract class FieldAttribute : Attribute
	{
		protected FieldAttribute()
		{
		}
	}
}