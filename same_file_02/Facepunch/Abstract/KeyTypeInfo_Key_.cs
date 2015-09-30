using System;
using System.Collections.Generic;

namespace Facepunch.Abstract
{
	internal class KeyTypeInfo<Key>
	where Key : TraitKey
	{
		public readonly System.Type Type;

		public readonly KeyTypeInfo<Key> Base;

		public readonly KeyTypeInfo<Key> Root;

		public readonly int TraitDepth;

		public readonly HashSet<KeyTypeInfo<Key>> AssignableTo;

		public bool IsBaseTrait
		{
			get
			{
				return this.TraitDepth == 0;
			}
		}

		private KeyTypeInfo(System.Type Type, KeyTypeInfo<Key> Base, KeyTypeInfo<Key> Root, int TraitDepth)
		{
			this.Type = Type;
			this.Base = Base;
			object root = Root;
			if (root == null)
			{
				root = this;
			}
			this.Root = (KeyTypeInfo<Key>)root;
			this.TraitDepth = TraitDepth;
			if (this.Root != this)
			{
				this.AssignableTo = new HashSet<KeyTypeInfo<Key>>(this.Base.AssignableTo);
			}
			else
			{
				this.AssignableTo = new HashSet<KeyTypeInfo<Key>>();
			}
			this.AssignableTo.Add(this);
			KeyTypeInfo<Key>.Registration.Add(this);
		}

		public static KeyTypeInfo<Key> Find(System.Type traitType)
		{
			if (!typeof(Key).IsAssignableFrom(traitType))
			{
				throw new ArgumentOutOfRangeException("traitType", "Must be a type assignable to Key");
			}
			if (traitType == typeof(Key))
			{
				throw new KeyArgumentIsKeyTypeException("You cannot use GetTrait(typeof(Key). Must use a types inheriting Key");
			}
			return KeyTypeInfo<Key>.Registration.GetUnsafe(traitType);
		}

		public static bool Find(System.Type traitType, out KeyTypeInfo<Key> info)
		{
			if (typeof(Key).IsAssignableFrom(traitType) && traitType != typeof(Key))
			{
				info = null;
				return false;
			}
			info = KeyTypeInfo<Key>.Registration.GetUnsafe(traitType);
			return true;
		}

		public static KeyTypeInfo<Key> Find<T>()
		where T : Key
		{
			return KeyTypeInfo<Key, T>.Info;
		}

		public static bool Find<T>(out KeyTypeInfo<Key> info)
		where T : Key
		{
			bool flag;
			try
			{
				info = KeyTypeInfo<Key, T>.Info;
				flag = true;
			}
			catch (KeyArgumentIsKeyTypeException keyArgumentIsKeyTypeException)
			{
				info = null;
				flag = false;
			}
			return flag;
		}

		public bool IsAssignableFrom(KeyTypeInfo<Key> info)
		{
			return (info.Root != this.Root || info.TraitDepth < this.TraitDepth ? false : info.AssignableTo.Contains(this));
		}

		public static class Comparison
		{
			public static IComparer<KeyTypeInfo<Key>> Comparer
			{
				get
				{
					return KeyTypeInfo<Key>.Comparison.HierarchyComparer.Singleton.Instance;
				}
			}

			public static IEqualityComparer<KeyTypeInfo<Key>> EqualityComparer
			{
				get
				{
					return KeyTypeInfo<Key>.Comparison.RootEqualityComparer.Singleton.Instance;
				}
			}

			internal class HierarchyComparer : Comparer<KeyTypeInfo<Key>>
			{
				public HierarchyComparer()
				{
				}

				private static int BaseCompare(KeyTypeInfo<Key> x, KeyTypeInfo<Key> y)
				{
					if (x.TraitDepth == 0 || x == y)
					{
						return 0;
					}
					int num = KeyTypeInfo<Key>.Comparison.HierarchyComparer.BaseCompare(x.Base, y.Base);
					if (num == 0)
					{
						num = KeyTypeInfo.ForcedDifCompareValue(x.Type, y.Type);
					}
					return num;
				}

				public override int Compare(KeyTypeInfo<Key> x, KeyTypeInfo<Key> y)
				{
					return -this.CompareForward(x, y);
				}

				private int CompareForward(KeyTypeInfo<Key> x, KeyTypeInfo<Key> y)
				{
					if (x.Root != y.Root)
					{
						return KeyTypeInfo.ForcedDifCompareValue(x.Root.Type, y.Root.Type);
					}
					if (x.TraitDepth == y.TraitDepth)
					{
						return KeyTypeInfo<Key>.Comparison.HierarchyComparer.BaseCompare(x, y);
					}
					return x.TraitDepth.CompareTo(y.TraitDepth);
				}

				public static class Singleton
				{
					public readonly static IComparer<KeyTypeInfo<Key>> Instance;

					static Singleton()
					{
						KeyTypeInfo<Key>.Comparison.HierarchyComparer.Singleton.Instance = new KeyTypeInfo<Key>.Comparison.HierarchyComparer();
					}
				}
			}

			private class RootEqualityComparer : EqualityComparer<KeyTypeInfo<Key>>
			{
				private RootEqualityComparer()
				{
				}

				public override bool Equals(KeyTypeInfo<Key> x, KeyTypeInfo<Key> y)
				{
					return x.Root == y.Root;
				}

				public override int GetHashCode(KeyTypeInfo<Key> obj)
				{
					return obj.Root.Type.GetHashCode();
				}

				public static class Singleton
				{
					public readonly static IEqualityComparer<KeyTypeInfo<Key>> Instance;

					static Singleton()
					{
						KeyTypeInfo<Key>.Comparison.RootEqualityComparer.Singleton.Instance = new KeyTypeInfo<Key>.Comparison.RootEqualityComparer();
					}
				}
			}
		}

		internal static class Registration
		{
			private readonly static Dictionary<System.Type, KeyTypeInfo<Key>> dict;

			static Registration()
			{
				KeyTypeInfo<Key>.Registration.dict = new Dictionary<System.Type, KeyTypeInfo<Key>>();
			}

			public static void Add(KeyTypeInfo<Key> info)
			{
				KeyTypeInfo<Key>.Registration.dict.Add(info.Type, info);
			}

			public static KeyTypeInfo<Key> GetUnsafe(System.Type type)
			{
				KeyTypeInfo<Key> keyTypeInfo;
				if (KeyTypeInfo<Key>.Registration.dict.TryGetValue(type, out keyTypeInfo))
				{
					return keyTypeInfo;
				}
				System.Type baseType = type.BaseType;
				if (typeof(Key) == baseType)
				{
					return new KeyTypeInfo<Key>(type, null, null, 0);
				}
				KeyTypeInfo<Key> @unsafe = KeyTypeInfo<Key>.Registration.GetUnsafe(baseType);
				return new KeyTypeInfo<Key>(type, @unsafe, @unsafe.Root, @unsafe.TraitDepth + 1);
			}
		}

		internal class TraitDictionary
		{
			[NonSerialized]
			private readonly Dictionary<KeyTypeInfo<Key>, Key> rootToKey;

			public TraitDictionary(Key[] traitKeys)
			{
				if (traitKeys == null || (int)traitKeys.Length == 0)
				{
					this.rootToKey = new Dictionary<KeyTypeInfo<Key>, Key>(0);
				}
				else
				{
					this.rootToKey = new Dictionary<KeyTypeInfo<Key>, Key>((int)traitKeys.Length, KeyTypeInfo<Key>.Comparison.EqualityComparer);
					Key[] keyArray = traitKeys;
					for (int i = 0; i < (int)keyArray.Length; i++)
					{
						Key key = keyArray[i];
						if (key)
						{
							this.rootToKey.Add(KeyTypeInfo<Key>.Find(key.GetType()), key);
						}
					}
				}
			}

			private Key Get(KeyTypeInfo<Key> info)
			{
				return this.rootToKey[info];
			}

			public Key Get<T>()
			where T : Key
			{
				return this.Get(KeyTypeInfo<Key, T>.Info);
			}

			public Key Get(System.Type type)
			{
				return this.Get(KeyTypeInfo<Key>.Find(type));
			}

			private T GetHardCast<T>(KeyTypeInfo<Key> info)
			where T : Key
			{
				return (T)(object)this.Get(info);
			}

			public T GetHardCast<T>()
			where T : Key
			{
				return this.GetHardCast<T>(KeyTypeInfo<Key, T>.Info);
			}

			public T GetHardCast<T>(System.Type type)
			where T : Key
			{
				return this.GetHardCast<T>(KeyTypeInfo<Key>.Find(type));
			}

			private T GetSoftCast<T>(KeyTypeInfo<Key> info)
			where T : Key
			{
				return (T)((object)this.Get(info) as T);
			}

			public T GetSoftCast<T>()
			where T : Key
			{
				return this.GetSoftCast<T>(KeyTypeInfo<Key, T>.Info);
			}

			public T GetSoftCast<T>(System.Type type)
			where T : Key
			{
				return this.GetSoftCast<T>(KeyTypeInfo<Key>.Find(type));
			}

			public void MergeUpon(KeyTypeInfo<Key>.TraitDictionary fillGaps)
			{
				foreach (KeyValuePair<KeyTypeInfo<Key>, Key> keyValuePair in this.rootToKey)
				{
					if (fillGaps.rootToKey.ContainsKey(keyValuePair.Key.Root))
					{
						continue;
					}
					fillGaps.rootToKey.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}

			private bool TryGet(KeyTypeInfo<Key> info, out Key key)
			{
				return this.rootToKey.TryGetValue(info, out key);
			}

			public bool TryGet<T>(out Key key)
			where T : Key
			{
				return this.TryGet(KeyTypeInfo<Key, T>.Info, out key);
			}

			public bool TryGet(System.Type traitType, out Key key)
			{
				return this.TryGet(KeyTypeInfo<Key>.Find(traitType), out key);
			}

			public Key TryGet<T>()
			where T : Key
			{
				Key key;
				this.TryGet<T>(out key);
				return key;
			}

			public Key TryGet(System.Type type)
			{
				Key key;
				this.TryGet(type, out key);
				return key;
			}

			private bool TryGetHardCast<T>(KeyTypeInfo<Key> info, out T tkey)
			where T : Key
			{
				Key key;
				if (!this.TryGet(info, out key))
				{
					tkey = (T)null;
					return false;
				}
				tkey = (T)(object)key;
				return true;
			}

			public bool TryGetHardCast<T>(out T key)
			where T : Key
			{
				return this.TryGetHardCast<T>(KeyTypeInfo<Key, T>.Info, out key);
			}

			public bool TryGetHardCast<T>(System.Type traitType, out T key)
			where T : Key
			{
				return this.TryGetHardCast<T>(KeyTypeInfo<Key>.Find(traitType), out key);
			}

			public T TryGetHardCast<T>()
			where T : Key
			{
				T t;
				this.TryGetHardCast<T>(out t);
				return t;
			}

			public T TryGetHardCast<T>(System.Type type)
			where T : Key
			{
				T t;
				this.TryGetHardCast<T>(type, out t);
				return t;
			}

			private bool TryGetSoftCast<T>(KeyTypeInfo<Key> info, out T tkey)
			where T : Key
			{
				Key key;
				if (!this.TryGet(info, out key))
				{
					tkey = (T)null;
					return false;
				}
				tkey = (T)((object)key as T);
				return true;
			}

			public bool TryGetSoftCast<T>(out T key)
			where T : Key
			{
				return this.TryGetSoftCast<T>(KeyTypeInfo<Key, T>.Info, out key);
			}

			public bool TryGetSoftCast<T>(System.Type traitType, out T key)
			where T : Key
			{
				return this.TryGetSoftCast<T>(KeyTypeInfo<Key>.Find(traitType), out key);
			}

			public T TryGetSoftCast<T>()
			where T : Key
			{
				T t;
				this.TryGetSoftCast<T>(out t);
				return t;
			}

			public T TryGetSoftCast<T>(System.Type type)
			where T : Key
			{
				T t;
				this.TryGetSoftCast<T>(type, out t);
				return t;
			}
		}
	}
}