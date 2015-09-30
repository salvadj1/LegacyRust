using System;
using UnityEngine;

internal class NetPreUpdate : MonoBehaviour
{
	[NonSerialized]
	private float lastfpslog = -1f;

	[NonSerialized]
	private float lastfpslogtime;

	public NetPreUpdate()
	{
	}

	private void Awake()
	{
		NetCull.Callbacks.BindUpdater(this);
	}

	private void LateUpdate()
	{
		if (global.fpslog >= 0f)
		{
			if (this.lastfpslog != global.fpslog)
			{
				this.lastfpslog = global.fpslog;
				this.lastfpslogtime = Time.time - this.lastfpslog;
			}
			float single = Time.time;
			if (this.lastfpslog == 0f || single - this.lastfpslogtime >= this.lastfpslog)
			{
				this.lastfpslogtime = single;
				MonoBehaviour.print(string.Concat(new object[] { DateTime.Now, ": frame #", Time.frameCount, ", fps ", 1f / Time.smoothDeltaTime }));
			}
		}
		if (Application.isPlaying)
		{
			NetCull.Callbacks.FirePreUpdate(this);
		}
	}

	private void OnDestroy()
	{
		NetCull.Callbacks.ResignUpdater(this);
	}
}