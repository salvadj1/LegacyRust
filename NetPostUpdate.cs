using System;
using UnityEngine;

internal class NetPostUpdate : MonoBehaviour
{
	public NetPostUpdate()
	{
	}

	private void Awake()
	{
		NetCull.Callbacks.BindUpdater(this);
	}

	protected void LateUpdate()
	{
		if (Application.isPlaying)
		{
			NetCull.Callbacks.FirePostUpdate(this);
		}
	}

	private void OnDestroy()
	{
		NetCull.Callbacks.ResignUpdater(this);
	}
}