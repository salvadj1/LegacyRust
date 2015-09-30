using System;
using System.Collections.Generic;
using UnityEngine;

public static class ODBCachedEnumerator<T>
where T : UnityEngine.Object
{
	public static IEnumerator<T> Cache<TEnumerator>(ref TEnumerator enumerator)
	where TEnumerator : struct, ODBEnumerator<T>
	{
		return ODBCachedEnumerator<T, TEnumerator>.Cache(ref enumerator);
	}
}