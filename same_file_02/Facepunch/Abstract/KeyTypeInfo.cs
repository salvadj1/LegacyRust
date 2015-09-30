using System;

namespace Facepunch.Abstract
{
	internal static class KeyTypeInfo
	{
		public static int ForcedDifCompareValue(Type x, Type y)
		{
			int hashCode = x.GetHashCode();
			int num = hashCode.CompareTo(y.GetHashCode());
			if (num == 0)
			{
				num = x.AssemblyQualifiedName.CompareTo(y.AssemblyQualifiedName);
				if (num == 0)
				{
					long num1 = x.TypeHandle.Value.ToInt64();
					num = num1.CompareTo(y.TypeHandle.Value);
					if (num == 0)
					{
						throw new InvalidProgramException();
					}
				}
			}
			return num;
		}
	}
}