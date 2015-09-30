using MoPhoGames.USpeak.Interface;
using System;
using uLink;
using UnityEngine;

public sealed class VoiceCom : IDLocalCharacter, IVoiceCom, ISpeechDataHandler, IUSpeakTalkController
{
	private int setupData;

	[NonSerialized]
	private USpeaker _uspeaker;

	[NonSerialized]
	private bool _uspeakerChecked;

	public USpeaker uspeaker
	{
		get
		{
			if (!this._uspeakerChecked)
			{
				this._uspeaker = USpeaker.Get(this);
				this._uspeakerChecked = true;
			}
			return this._uspeaker;
		}
	}

	public VoiceCom()
	{
	}

	[RPC]
	private void clientspeak(int setupData, byte[] data)
	{
	}

	public static float GetVolume()
	{
		return 0f;
	}

	[RPC]
	private void init(int data)
	{
		this.uspeaker.InitializeSettings(data);
	}

	void MoPhoGames.USpeak.Interface.IUSpeakTalkController.OnInspectorGUI()
	{
	}

	public bool ShouldSend()
	{
		Character character = base.idMain;
		return (!character || !character.alive ? false : GameInput.GetButton("Voice").IsDown());
	}

	private void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		if (base.networkView.isMine)
		{
			this.uspeaker.SpeakerMode = SpeakerMode.Local;
		}
		else
		{
			this.uspeaker.SpeakerMode = SpeakerMode.Remote;
		}
	}

	public void USpeakInitializeSettings(int data)
	{
		this.setupData = data;
	}

	public void USpeakOnSerializeAudio(byte[] data)
	{
		base.networkView.RPC("clientspeak", uLink.RPCMode.Server, new object[] { this.setupData, data });
	}

	[RPC]
	private void voiceplay(float hearDistance, int setupData, byte[] data)
	{
		Camera camera = Camera.main;
		if (!camera)
		{
			return;
		}
		if (hearDistance <= 0f)
		{
			return;
		}
		USpeaker uSpeaker = this.uspeaker;
		if (!uSpeaker)
		{
			Debug.LogWarning(string.Concat("voiceplayback:", base.gameObject, " didn't have a USpeaker!?"));
		}
		if (!uSpeaker.HasSettings())
		{
			uSpeaker.InitializeSettings(setupData);
		}
		if (data == null)
		{
			Debug.LogWarning("voiceplayback: data was null!");
		}
		Vector3 worldPoint = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0001f));
		float single = Vector3.Distance(worldPoint, base.eyesOrigin);
		float single1 = Mathf.Clamp01(1f - single / hearDistance);
		uSpeaker.SpeakerVolume = single1;
		uSpeaker.ReceiveAudio(data);
		AudioSource audioSource = uSpeaker.audio;
		if (audioSource)
		{
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.maxDistance = hearDistance;
			audioSource.minDistance = 1f;
		}
	}
}