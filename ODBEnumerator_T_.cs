using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ODBEnumerator<T> : IDisposable, IEnumerator, IEnumerator<T>
where T : UnityEngine.Object
{
	T ExplicitCurrent
	{
		get;
	}

	IEnumerator<T> ToGeneric();
}