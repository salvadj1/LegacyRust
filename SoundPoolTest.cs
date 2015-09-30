using System;
using UnityEngine;

public class SoundPoolTest : MonoBehaviour
{
	public AudioClip[] clips;

	public Transform[] on;

	public float chanceOn;

	public float intervalPlayRandomClip;

	private float lastTime;

	private bool first;

	public SoundPoolTest()
	{
	}

	private void OnEnable()
	{
		this.first = true;
	}

	public void OnGUI()
	{
		if (this.clips != null)
		{
			AudioClip[] audioClipArray = this.clips;
			for (int i = 0; i < (int)audioClipArray.Length; i++)
			{
				AudioClip audioClip = audioClipArray[i];
				if (GUILayout.Button(audioClip.name, new GUILayoutOption[0]))
				{
					audioClip.Play();
				}
			}
		}
		GUI.Box(new Rect((float)(Screen.width - 256), 0f, 256f, 24f), string.Concat("Total Sound Nodes   ", SoundPool.totalCount));
		GUI.Box(new Rect((float)(Screen.width - 256), 30f, 256f, 24f), string.Concat("Playing Sound Nodes ", SoundPool.playingCount));
		GUI.Box(new Rect((float)(Screen.width - 256), 60f, 256f, 24f), string.Concat("Reserve Sound Nodes ", SoundPool.reserveCount));
		if (GUI.Button(new Rect((float)(Screen.width - 128), 90f, 128f, 24f), "Drain Reserves"))
		{
			SoundPool.DrainReserves();
		}
		if (GUI.Button(new Rect((float)(Screen.width - 128), 120f, 128f, 24f), "Drain"))
		{
			SoundPool.Drain();
		}
		if (GUI.Button(new Rect((float)(Screen.width - 128), 150f, 128f, 24f), "Stop All"))
		{
			SoundPool.Stop();
		}
	}

	private void Update()
	{
		if (this.clips == null || this.intervalPlayRandomClip <= 0f)
		{
			this.first = true;
		}
		else
		{
			float single = Mathf.Max(0.05f, this.intervalPlayRandomClip);
			if (this.first)
			{
				this.lastTime = Time.time - single;
			}
			this.first = false;
			while (Time.time - this.lastTime >= single)
			{
				AudioClip audioClip = this.clips[UnityEngine.Random.Range(0, (int)this.clips.Length)];
				if (this.on == null || (int)this.on.Length <= 0 || UnityEngine.Random.@value > this.chanceOn)
				{
					audioClip.Play();
				}
				else
				{
					audioClip.Play(this.on[UnityEngine.Random.Range(0, (int)this.on.Length)]);
				}
				SoundPoolTest soundPoolTest = this;
				soundPoolTest.lastTime = soundPoolTest.lastTime + single;
			}
		}
	}
}