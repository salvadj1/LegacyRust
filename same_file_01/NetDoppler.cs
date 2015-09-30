using System;
using UnityEngine;

public class NetDoppler : MonoBehaviour
{
	public float minPitch = 0.5f;

	private float? lastVolume;

	public NetDoppler()
	{
	}

	public void Update()
	{
		MountedCamera mountedCamera = MountedCamera.main;
		if (!mountedCamera)
		{
			this.lastVolume = new float?(base.audio.volume);
		}
		else
		{
			float single = Vector3.Distance(base.transform.position, mountedCamera.transform.position);
			float single1 = 1f - Mathf.Clamp01(single / base.audio.maxDistance);
			float single2 = 1f + this.minPitch * single1;
			base.audio.pitch = single2;
			if (this.lastVolume.HasValue)
			{
				base.audio.volume = this.lastVolume.Value;
				this.lastVolume = null;
			}
		}
	}
}