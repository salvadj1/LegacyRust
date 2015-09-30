using System;

namespace Facepunch.Attributes
{
	public abstract class ObjectLookupFieldFixedTypeAttribute : ObjectLookupFieldAttribute
	{
		public readonly Type[] RequiredComponents;

		public new Type MinimumType
		{
			get
			{
				return base.MinimumType;
			}
		}

		protected ObjectLookupFieldFixedTypeAttribute(PrefabLookupKinds kinds, Type minimalType, Facepunch.Attributes.SearchMode defaultSearchMode, Type[] interfacesRequired) : base(kinds, minimalType, defaultSearchMode, interfacesRequired)
		{
		}
	}
}