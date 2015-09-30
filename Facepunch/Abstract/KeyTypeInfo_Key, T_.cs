using System;

namespace Facepunch.Abstract
{
	internal static class KeyTypeInfo<Key, T>
	where Key : TraitKey
	where T : Key
	{
		internal readonly static KeyTypeInfo<Key> Info;

		static KeyTypeInfo()
		{
			if (typeof(T) == typeof(Key))
			{
				throw new KeyArgumentIsKeyTypeException("<T>", "You cannot use GetTrait<Key>. Must use a types inheriting Key");
			}
			KeyTypeInfo<Key, T>.Info = KeyTypeInfo<Key>.Registration.GetUnsafe(typeof(T));
		}
	}
}