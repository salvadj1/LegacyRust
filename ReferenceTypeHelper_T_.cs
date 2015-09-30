using System;

public static class ReferenceTypeHelper<T>
{
	public readonly static bool TreatAsReferenceHolder;

	static ReferenceTypeHelper()
	{
		ReferenceTypeHelper<T>.TreatAsReferenceHolder = ReferenceTypeHelper.TreatAsReferenceHolder(typeof(T));
	}
}