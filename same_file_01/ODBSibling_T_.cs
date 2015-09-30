using System;
using UnityEngine;

public struct ODBSibling<T>
where T : UnityEngine.Object
{
	public ODBNode<T> item;

	public bool has;
}