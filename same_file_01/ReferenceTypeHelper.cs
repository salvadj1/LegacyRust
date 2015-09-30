using System;
using System.Collections.Generic;
using System.Reflection;

public static class ReferenceTypeHelper
{
	private readonly static Dictionary<Type, bool> cache;

	static ReferenceTypeHelper()
	{
		ReferenceTypeHelper.cache = new Dictionary<Type, bool>();
	}

	public static bool TreatAsReferenceHolder(Type type)
	{
		bool flag;
		if (!ReferenceTypeHelper.cache.TryGetValue(type, out flag))
		{
			if (type.IsByRef)
			{
				flag = true;
			}
			else if (!type.IsEnum)
			{
				FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				int num = 0;
				while (num < (int)fields.Length)
				{
					Type fieldType = fields[num].FieldType;
					if (fieldType.IsByRef || !ReferenceTypeHelper.TreatAsReferenceHolder(fieldType))
					{
						flag = false;
						break;
					}
					else
					{
						num++;
					}
				}
			}
			else
			{
				flag = false;
			}
			ReferenceTypeHelper.cache[type] = flag;
		}
		return flag;
	}
}