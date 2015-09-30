using System;
using System.Collections.Generic;

public static class ODBCachedEnumerator
{
	public static IEnumerator<T> Cache<TEnumerator, T>(ref TEnumerator enumerator)
	where TEnumerator : struct, ODBEnumerator<T>
	where T : Object
	{
		return ODBCachedEnumerator<T>.Cache<TEnumerator>(ref enumerator);
	}
}