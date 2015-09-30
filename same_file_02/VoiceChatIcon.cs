using System;
using UnityEngine;

public class VoiceChatIcon : MonoBehaviour
{
	private dfLabel label;

	public VoiceChatIcon()
	{
	}

	private void OnEnable()
	{
		this.label = base.GetComponent<dfLabel>();
	}

	private void Update()
	{
		if (this.label == null)
		{
			return;
		}
		float currentVolume = 0f;
		if (GameInput.GetButton("Voice").IsDown())
		{
			currentVolume = USpeaker.CurrentVolume;
		}
		this.label.Opacity = Mathf.Clamp(currentVolume * 20f, 0f, 1f);
	}
}