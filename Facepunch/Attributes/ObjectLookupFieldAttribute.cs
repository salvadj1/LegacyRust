using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facepunch.Attributes
{
	public abstract class ObjectLookupFieldAttribute : FieldAttribute
	{
		public readonly PrefabLookupKinds Kinds;

		private Type minType;

		private Facepunch.Attributes.SearchMode searchMode;

		private readonly Type attributeMinimumType = typeof(UnityEngine.Object);

		private readonly Facepunch.Attributes.SearchMode searchModeDefault = Facepunch.Attributes.SearchMode.MainAsset;

		public readonly Type[] RequiredInterfaces;

		public bool AllowNull
		{
			get;
			set;
		}

		public Type MinimumType
		{
			get
			{
				return this.minType ?? this.attributeMinimumType;
			}
			set
			{
				if (value != null && !this.attributeMinimumType.IsAssignableFrom(value) && !this.CompliantMinimumType(value))
				{
					throw new ArgumentOutOfRangeException("value", value, "The type is not assignable given restrictions");
				}
				this.minType = value;
			}
		}

		public Facepunch.Attributes.SearchMode SearchMode
		{
			get
			{
				return (this.searchMode != Facepunch.Attributes.SearchMode.Default ? this.searchMode : this.searchModeDefault);
			}
			protected set
			{
				this.searchMode = value;
			}
		}

		protected ObjectLookupFieldAttribute(PrefabLookupKinds kinds, Type minimumType, Facepunch.Attributes.SearchMode searchModeDefault, Type[] interfaceTypes)
		{
			this.Kinds = kinds;
			this.MinimumType = minimumType;
			if (searchModeDefault != Facepunch.Attributes.SearchMode.Default)
			{
				this.searchModeDefault = searchModeDefault;
			}
			this.RequiredInterfaces = interfaceTypes ?? ObjectLookupFieldAttribute.Empty.TypeArray;
		}

		protected virtual bool CompliantMinimumType(Type type)
		{
			return true;
		}

		public CustomLookupResult Confirm(UnityEngine.Object obj)
		{
			bool flag;
			CustomLookupResult customLookupResult;
			CustomLookupResult customLookupResult1;
			if (this.AllowNull)
			{
				flag = !obj;
			}
			else
			{
				if (!obj)
				{
					return CustomLookupResult.FailNull;
				}
				flag = false;
			}
			if (flag)
			{
				try
				{
					customLookupResult = this.CustomConfirm(null, true, null);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					customLookupResult = CustomLookupResult.FailConfirmException;
				}
			}
			else
			{
				try
				{
					Type type = obj.GetType();
					try
					{
						customLookupResult = this.CustomConfirm(obj, false, type);
					}
					catch (Exception exception1)
					{
						Debug.LogException(exception1, obj);
						customLookupResult = CustomLookupResult.FailConfirmException;
					}
					if (customLookupResult == CustomLookupResult.Fallback)
					{
						return CustomLookupResult.AcceptConfirmed;
					}
					return customLookupResult;
				}
				catch (Exception exception2)
				{
					Debug.LogException(exception2, obj);
					customLookupResult1 = CustomLookupResult.FailNull;
				}
				return customLookupResult1;
			}
			if (customLookupResult == CustomLookupResult.Fallback)
			{
				return CustomLookupResult.AcceptConfirmed;
			}
			return customLookupResult;
		}

		protected virtual CustomLookupResult CustomConfirm(UnityEngine.Object obj, bool isNull, Type type)
		{
			return CustomLookupResult.Fallback;
		}

		protected virtual CustomLookupResult CustomLookup(object value, Type type, ref UnityEngine.Object find)
		{
			return CustomLookupResult.Fallback;
		}

		public CustomLookupResult Lookup(object value, out UnityEngine.Object find)
		{
			return this.Lookup(value, this.MinimumType, out find);
		}

		public CustomLookupResult Lookup(object value, Type type, out UnityEngine.Object find)
		{
			CustomLookupResult customLookupResult;
			CustomLookupResult customLookupResult1;
			find = null;
			if (!this.MinimumType.IsAssignableFrom(type))
			{
				return CustomLookupResult.FailCast;
			}
			Type[] requiredInterfaces = this.RequiredInterfaces;
			for (int i = 0; i < (int)requiredInterfaces.Length; i++)
			{
				if (!requiredInterfaces[i].IsAssignableFrom(type))
				{
					return CustomLookupResult.FailInterface;
				}
			}
			try
			{
				customLookupResult = this.CustomLookup(value, type, ref find);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, find);
				customLookupResult1 = CustomLookupResult.FailCustomException;
				return customLookupResult1;
			}
			if (customLookupResult == CustomLookupResult.Fallback)
			{
				customLookupResult = CustomLookupResult.Accept;
			}
			if (customLookupResult == CustomLookupResult.Accept)
			{
				try
				{
					customLookupResult = this.Confirm(find);
				}
				catch (Exception exception1)
				{
					Debug.LogException(exception1, find);
					customLookupResult1 = CustomLookupResult.FailConfirmException;
					return customLookupResult1;
				}
			}
			return customLookupResult;
		}

		public CustomLookupResult Lookup<TObj>(object value, out TObj find)
		where TObj : UnityEngine.Object
		{
			return this.Lookup<TObj>(value, typeof(TObj), out find);
		}

		public CustomLookupResult Lookup<TObj>(object value, Type type, out TObj find)
		where TObj : UnityEngine.Object
		{
			UnityEngine.Object obj;
			CustomLookupResult customLookupResult;
			CustomLookupResult customLookupResult1;
			if (!typeof(TObj).IsAssignableFrom(type))
			{
				throw new ArgumentOutOfRangeException("type", type, string.Concat("type is not assignable to the generic ", typeof(TObj)));
			}
			try
			{
				customLookupResult = this.Lookup(value, typeof(TObj), out obj);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				find = (TObj)null;
				customLookupResult1 = CustomLookupResult.FailCustomException;
				return customLookupResult1;
			}
			if (customLookupResult <= CustomLookupResult.Fallback)
			{
				try
				{
					find = (TObj)obj;
				}
				catch
				{
					find = (TObj)null;
				}
			}
			else
			{
				try
				{
					find = (TObj)obj;
				}
				catch (Exception exception1)
				{
					Debug.LogException(exception1, obj);
					find = (TObj)null;
					customLookupResult1 = CustomLookupResult.FailCast;
					return customLookupResult1;
				}
			}
			return customLookupResult;
		}

		private static class Empty
		{
			public readonly static Type[] TypeArray;

			static Empty()
			{
				ObjectLookupFieldAttribute.Empty.TypeArray = new Type[0];
			}
		}
	}
}