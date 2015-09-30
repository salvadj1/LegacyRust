using System;
using System.Collections.Generic;

public class CharacterPrefab : NetMainPrefab
{
	public CharacterPrefab() : this(typeof(Character), false, null, false)
	{
	}

	protected CharacterPrefab(Type characterType) : this(characterType, true, null, false)
	{
	}

	protected CharacterPrefab(Type characterType, params Type[] requiredIDLocalComponents) : this(characterType, true, requiredIDLocalComponents, (requiredIDLocalComponents == null ? false : (int)requiredIDLocalComponents.Length > 0))
	{
	}

	private CharacterPrefab(Type characterType, bool typeCheck, Type[] requiredIDLocalComponents, bool anyRequiredIDLocalComponents) : base(characterType)
	{
		if (typeCheck && !typeof(Character).IsAssignableFrom(characterType))
		{
			throw new ArgumentOutOfRangeException("type", "type must be assignable to Character");
		}
	}

	protected static Type[] TypeArrayAppend(Type[] mustInclude, Type[] given)
	{
		if (mustInclude == null || (int)mustInclude.Length == 0)
		{
			return given;
		}
		if (given == null || (int)given.Length == 0)
		{
			return mustInclude;
		}
		List<Type> types = new List<Type>(given);
		for (int i = 0; i < (int)mustInclude.Length; i++)
		{
			bool flag = false;
			int num = 0;
			while (num < (int)given.Length)
			{
				if (!mustInclude[i].IsAssignableFrom(given[num]))
				{
					num++;
				}
				else
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				types.Add(mustInclude[i]);
			}
		}
		return types.ToArray();
	}
}