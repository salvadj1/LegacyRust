using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ODBEnumerable<T, TEnumerator> : IEnumerable, IEnumerable<T>
where T : UnityEngine.Object
where TEnumerator : struct, ODBEnumerator<T>
{
	TEnumerator GetEnumerator();

	IEnumerable<T> ToGeneric();
}