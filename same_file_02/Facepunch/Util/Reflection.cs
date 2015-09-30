using System;
using System.Reflection;

namespace Facepunch.Util
{
	public class Reflection
	{
		public Reflection()
		{
		}

		public static bool HasAttribute(MemberInfo method, Type attribute)
		{
			return (int)method.GetCustomAttributes(attribute, true).Length > 0;
		}
	}
}