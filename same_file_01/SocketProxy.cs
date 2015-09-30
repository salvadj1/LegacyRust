using System;
using UnityEngine;

[AddComponentMenu("")]
public sealed class SocketProxy : Socket.Proxy
{
	public SocketProxy()
	{
	}

	protected override void UninitializeProxy()
	{
		base.transform.DetachChildren();
	}
}