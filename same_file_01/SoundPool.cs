using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public static class SoundPool
{
	private const sbyte SelectRoot_Attach = 2;

	private const sbyte SelectRoot_Camera = 1;

	private const sbyte SelectRoot_Default = 0;

	private const sbyte SelectRoot_Camera_WorldOffset = 5;

	private const sbyte SelectRoot_Attach_WorldOffset = 6;

	private const string goName = "zzz-soundpoolnode";

	private const float logarithmicMaxScale = 2f;

	private static bool _enabled;

	private static bool _quitting;

	private readonly static SoundPool.Root playingAttached;

	private readonly static SoundPool.Root playingCamera;

	private readonly static SoundPool.Root playing;

	private readonly static SoundPool.Root reserved;

	private static bool firstLeak;

	private static bool hadFirstLeak;

	private readonly static Type[] goTypes;

	private readonly static SoundPool.Settings DEF;

	internal static bool enabled
	{
		get
		{
			return SoundPool._enabled;
		}
		set
		{
			if (!value)
			{
				SoundPool._enabled = false;
			}
			else
			{
				SoundPool._enabled = !SoundPool._quitting;
			}
		}
	}

	public static int playingCount
	{
		get
		{
			return SoundPool.playingCamera.count + SoundPool.playingAttached.count + SoundPool.playing.count;
		}
	}

	internal static bool quitting
	{
		set
		{
			if (!SoundPool._quitting && value)
			{
				SoundPool._quitting = true;
				SoundPool._enabled = false;
				SoundPool.Drain();
			}
		}
	}

	public static int reserveCount
	{
		get
		{
			return SoundPool.reserved.count;
		}
	}

	public static int totalCount
	{
		get
		{
			return SoundPool.playingCamera.count + SoundPool.playingAttached.count + SoundPool.playing.count + SoundPool.reserved.count;
		}
	}

	static SoundPool()
	{
		SoundPool.playingAttached = new SoundPool.Root(SoundPool.RootID.PLAYING_ATTACHED | SoundPool.RootID.RESERVED);
		SoundPool.playingCamera = new SoundPool.Root(SoundPool.RootID.PLAYING_CAMERA | SoundPool.RootID.DISPOSED);
		SoundPool.playing = new SoundPool.Root(SoundPool.RootID.PLAYING_ATTACHED | SoundPool.RootID.PLAYING_CAMERA | SoundPool.RootID.PLAYING | SoundPool.RootID.RESERVED | SoundPool.RootID.DISPOSED);
		SoundPool.reserved = new SoundPool.Root(SoundPool.RootID.RESERVED);
		SoundPool.firstLeak = false;
		SoundPool.goTypes = new Type[] { typeof(AudioSource) };
		SoundPool.Settings setting = new SoundPool.Settings()
		{
			volume = 1f,
			pitch = 1f,
			mode = AudioRolloffMode.Linear,
			min = 1f,
			max = 500f,
			panLevel = 1f,
			doppler = 1f,
			priority = 128,
			localRotation = Quaternion.identity
		};
		SoundPool.DEF = setting;
	}

	private static SoundPool.Node CreateNode()
	{
		if (!SoundPool.reserved.first.has)
		{
			return SoundPool.NewNode();
		}
		SoundPool.Node node = SoundPool.reserved.first.node;
		node.EnterLimbo();
		return node;
	}

	public static void Drain()
	{
		SoundPool.Node node;
		SoundPool.Dir dir = SoundPool.playing.first;
		while (dir.has)
		{
			node = dir.node;
			dir = dir.node.way.next;
			node.Dispose();
		}
		dir = SoundPool.playingAttached.first;
		while (dir.has)
		{
			node = dir.node;
			dir = dir.node.way.next;
			node.Dispose();
		}
		dir = SoundPool.playingCamera.first;
		while (dir.has)
		{
			node = dir.node;
			dir = dir.node.way.next;
			node.Dispose();
		}
		dir = SoundPool.reserved.first;
		while (dir.has)
		{
			node = dir.node;
			dir = dir.node.way.next;
			node.Dispose();
		}
	}

	public static void DrainReserves()
	{
		SoundPool.Dir dir = SoundPool.reserved.first;
		while (dir.has)
		{
			SoundPool.Node node = dir.node;
			dir = dir.node.way.next;
			node.Dispose();
		}
	}

	private static SoundPool.Node NewNode()
	{
		SoundPool.Node node = new SoundPool.Node();
		GameObject gameObject = new GameObject("zzz-soundpoolnode", SoundPool.goTypes)
		{
			hideFlags = HideFlags.NotEditable
		};
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		node.audio = gameObject.audio;
		node.transform = gameObject.transform;
		node.audio.playOnAwake = false;
		node.audio.enabled = false;
		return node;
	}

	private static void Play(ref SoundPool.Settings settings)
	{
		SoundPool.Root root;
		SoundPool.RootID rootID;
		bool flag;
		Vector3 vector3;
		Vector3 vector31;
		Quaternion quaternion;
		Quaternion quaternion1;
		SoundPool.RootID rootID1;
		if (!SoundPool._enabled || settings.volume <= 0f || settings.pitch == 0f || !settings.clip)
		{
			return;
		}
		Transform transforms = null;
		switch (settings.SelectRoot)
		{
			case 0:
			case 3:
			case 4:
			{
				root = SoundPool.playing;
				rootID = SoundPool.RootID.PLAYING_ATTACHED | SoundPool.RootID.PLAYING_CAMERA | SoundPool.RootID.PLAYING | SoundPool.RootID.RESERVED | SoundPool.RootID.DISPOSED;
				flag = false;
				break;
			}
			case 1:
			{
				if (!Camera.main)
				{
					return;
				}
				transforms = Camera.main.transform;
				root = SoundPool.playingCamera;
				rootID = SoundPool.RootID.PLAYING_CAMERA | SoundPool.RootID.DISPOSED;
				flag = false;
				break;
			}
			case 2:
			{
				if (!settings.parent)
				{
					return;
				}
				root = SoundPool.playingAttached;
				rootID = SoundPool.RootID.PLAYING_ATTACHED | SoundPool.RootID.RESERVED;
				flag = false;
				break;
			}
			case 5:
			{
				if (!Camera.main)
				{
					return;
				}
				transforms = Camera.main.transform;
				root = SoundPool.playingCamera;
				rootID = SoundPool.RootID.PLAYING_CAMERA | SoundPool.RootID.DISPOSED;
				flag = true;
				break;
			}
			case 6:
			{
				if (!settings.parent)
				{
					return;
				}
				root = SoundPool.playingAttached;
				rootID = SoundPool.RootID.PLAYING_ATTACHED | SoundPool.RootID.RESERVED;
				flag = true;
				break;
			}
			default:
			{
				goto case 4;
			}
		}
		if (!flag)
		{
			vector3 = settings.localPosition;
			quaternion = settings.localRotation;
			rootID1 = rootID;
			switch (rootID1)
			{
				case SoundPool.RootID.PLAYING_ATTACHED | SoundPool.RootID.RESERVED:
				{
					vector31 = settings.parent.TransformPoint(vector3);
					quaternion1 = settings.parent.rotation * quaternion;
					goto Label1;
				}
				case SoundPool.RootID.PLAYING_CAMERA | SoundPool.RootID.DISPOSED:
				{
					vector31 = transforms.TransformPoint(vector3);
					quaternion1 = transforms.rotation * quaternion;
					goto Label1;
				}
				case SoundPool.RootID.PLAYING_ATTACHED | SoundPool.RootID.PLAYING_CAMERA | SoundPool.RootID.PLAYING | SoundPool.RootID.RESERVED | SoundPool.RootID.DISPOSED:
				{
					vector31 = vector3;
					quaternion1 = quaternion;
					goto Label1;
				}
			}
			return;
		}
		else
		{
			rootID1 = rootID;
			if (rootID1 == (SoundPool.RootID.PLAYING_ATTACHED | SoundPool.RootID.RESERVED))
			{
				vector3 = settings.parent.InverseTransformPoint(settings.localPosition);
				quaternion = settings.localRotation * Quaternion.Inverse(settings.parent.rotation);
			}
			else
			{
				if (rootID1 != (SoundPool.RootID.PLAYING_CAMERA | SoundPool.RootID.DISPOSED))
				{
					return;
				}
				vector3 = transforms.InverseTransformPoint(settings.localPosition);
				quaternion = settings.localRotation * Quaternion.Inverse(transforms.rotation);
			}
			vector31 = settings.localPosition;
			quaternion1 = settings.localRotation;
		}
	Label1:
		if (!transforms)
		{
			Camera camera = Camera.main;
			if (!camera)
			{
				return;
			}
			transforms = camera.transform;
			float single = Vector3.Distance(vector31, transforms.position);
			switch (settings.mode)
			{
				case AudioRolloffMode.Logarithmic:
				{
					if (single > settings.max * 2f)
					{
						return;
					}
					break;
				}
				case AudioRolloffMode.Linear:
				case AudioRolloffMode.Custom:
				{
					if (single > settings.max)
					{
						return;
					}
					break;
				}
			}
		}
		SoundPool.Node node = SoundPool.CreateNode();
		if ((int)node.rootID != 0)
		{
			Debug.LogWarning(string.Concat("Wasn't Limbo ", node.rootID));
		}
		node.root = root;
		node.rootID = rootID;
		node.audio.pan = settings.pan;
		node.audio.panLevel = settings.panLevel;
		node.audio.volume = settings.volume;
		node.audio.dopplerLevel = settings.doppler;
		node.audio.pitch = settings.pitch;
		node.audio.rolloffMode = settings.mode;
		node.audio.minDistance = settings.min;
		node.audio.maxDistance = settings.max;
		node.audio.spread = settings.spread;
		node.audio.bypassEffects = settings.bypassEffects;
		node.audio.priority = settings.priority;
		node.parent = settings.parent;
		node.transform.position = vector31;
		node.transform.rotation = quaternion1;
		node.translation = vector3;
		node.rotation = quaternion;
		node.audio.clip = settings.clip;
		node.Bind();
		node.audio.enabled = true;
		node.audio.Play();
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation, float volume)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation, float volume, float pitch)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation, float volume, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation, float volume, AudioRolloffMode rolloffMode, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, Quaternion rotation, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, float pitch)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, float minDistance, float maxDistance, int priority)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.priority = priority;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, float pitch, float minDistance, float maxDistance, int priority)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.priority = priority;
		dEF.pitch = pitch;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, AudioRolloffMode rolloffMode, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, float pitch, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Vector3 position, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 1;
		dEF.clip = clip;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, float volume)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 1;
		dEF.clip = clip;
		dEF.volume = volume;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, float volume, float pan)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 1;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pan = pan;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, float volume, float pan, float pitch)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 1;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pan = pan;
		dEF.pitch = pitch;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, float volume, float pan, Vector3 worldPosition)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 5;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pan = pan;
		dEF.localPosition = worldPosition;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, float volume, float pan, Vector3 worldPosition, Quaternion worldRotation)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 5;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pan = pan;
		dEF.localPosition = worldPosition;
		dEF.localRotation = worldRotation;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.clip = clip;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, float volume)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.clip = clip;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, AudioRolloffMode rolloffMode, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, float volume)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, float volume, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, float volume, AudioRolloffMode rolloffMode, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void Play(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 6;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, float volume, float pan, Vector3 worldPosition)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 1;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pan = pan;
		dEF.localPosition = worldPosition;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, float volume, float pan, Vector3 worldPosition, Quaternion worldRotation)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 1;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pan = pan;
		dEF.localPosition = worldPosition;
		dEF.localRotation = worldRotation;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, AudioRolloffMode rolloffMode, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, Quaternion rotation, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.localRotation = rotation;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, int priority)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.priority = priority;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, AudioRolloffMode rolloffMode, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, float minDistance, float maxDistance)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, float minDistance, float maxDistance, int priority)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.priority = priority;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.mode = rolloffMode;
		dEF.spread = spread;
		SoundPool.Play(ref dEF);
	}

	public static void PlayLocal(this AudioClip clip, Transform on, Vector3 position, float volume, float pitch, AudioRolloffMode rolloffMode, float minDistance, float maxDistance, float dopplerLevel, float spread, bool bypassEffects)
	{
		SoundPool.Settings dEF = SoundPool.DEF;
		dEF.SelectRoot = 2;
		dEF.parent = on;
		dEF.localPosition = position;
		dEF.clip = clip;
		dEF.volume = volume;
		dEF.pitch = pitch;
		dEF.doppler = dopplerLevel;
		dEF.min = minDistance;
		dEF.max = maxDistance;
		dEF.spread = spread;
		dEF.bypassEffects = bypassEffects;
		dEF.mode = rolloffMode;
		SoundPool.Play(ref dEF);
	}

	public static void Pump()
	{
		if (SoundPool.firstLeak)
		{
			if (!SoundPool.hadFirstLeak)
			{
				Debug.LogWarning("SoundPool node leaked for the first time. Though performance should still be good, from now on until application exit there will be extra processing in Pump to clean up game objects of leaked/gc'd nodes. [ie. a mutex is now being locked and unlocked]");
				SoundPool.hadFirstLeak = true;
			}
			SoundPool.NodeGC.JOIN();
		}
		SoundPool.Dir dir = SoundPool.playingCamera.first;
		if (dir.has)
		{
			Camera camera = Camera.main;
			if (!camera)
			{
				do
				{
					SoundPool.Node node = dir.node;
					dir = dir.node.way.next;
					node.Reserve();
				}
				while (dir.has);
			}
			else
			{
				Transform transforms = camera.transform;
				Quaternion quaternion = transforms.rotation;
				do
				{
					SoundPool.Node node1 = dir.node;
					dir = dir.node.way.next;
					if (!node1.audio.isPlaying)
					{
						node1.Reserve();
					}
					else
					{
						node1.transform.position = transforms.TransformPoint(node1.translation);
						node1.transform.rotation = quaternion * node1.rotation;
					}
				}
				while (dir.has);
			}
		}
		dir = SoundPool.playingAttached.first;
		while (dir.has)
		{
			SoundPool.Node node2 = dir.node;
			dir = dir.node.way.next;
			if (!node2.audio.isPlaying || !node2.parent)
			{
				node2.Reserve();
			}
			else
			{
				node2.transform.position = node2.parent.TransformPoint(node2.translation);
				node2.transform.rotation = node2.parent.rotation * node2.rotation;
			}
		}
		dir = SoundPool.playing.first;
		while (dir.has)
		{
			SoundPool.Node node3 = dir.node;
			dir = dir.node.way.next;
			if (node3.audio.isPlaying)
			{
				continue;
			}
			node3.Reserve();
		}
	}

	public static void Stop()
	{
		SoundPool.Node node;
		SoundPool.Dir dir = SoundPool.playing.first;
		while (dir.has)
		{
			node = dir.node;
			dir = dir.node.way.next;
			node.Reserve();
		}
		dir = SoundPool.playingAttached.first;
		while (dir.has)
		{
			node = dir.node;
			dir = dir.node.way.next;
			node.Reserve();
		}
		dir = SoundPool.playingCamera.first;
		while (dir.has)
		{
			node = dir.node;
			dir = dir.node.way.next;
			node.Reserve();
		}
	}

	private static UnityEngine.Object TARG(ref SoundPool.Settings settings)
	{
		UnityEngine.Object obj;
		if (!settings.parent)
		{
			obj = settings.clip;
		}
		else
		{
			obj = settings.parent;
		}
		return obj;
	}

	private struct Dir
	{
		public SoundPool.Node node;

		public bool has;
	}

	private sealed class Node : IDisposable
	{
		public AudioSource audio;

		public Transform transform;

		public SoundPool.Way way;

		public SoundPool.RootID rootID;

		public SoundPool.Root root;

		public Vector3 translation;

		public Quaternion rotation;

		public Transform parent;

		public Node()
		{
		}

		public void Bind()
		{
			SoundPool.Dir dir = new SoundPool.Dir();
			this.way.prev = dir;
			this.way.next = this.root.first;
			this.root.first.has = true;
			this.root.first.node = this;
			if (this.way.next.has)
			{
				this.way.next.node.way.prev.has = true;
				this.way.next.node.way.prev.node = this;
			}
			SoundPool.Root root = this.root;
			root.count = root.count + 1;
		}

		public void Dispose()
		{
			switch (this.rootID)
			{
				case SoundPool.RootID.LIMBO:
				{
					break;
				}
				case SoundPool.RootID.RESERVED:
				{
					this.EnterLimbo();
					break;
				}
				case SoundPool.RootID.DISPOSED:
				{
					return;
				}
				default:
				{
					goto case SoundPool.RootID.RESERVED;
				}
			}
			UnityEngine.Object.Destroy(this.transform.gameObject);
			this.transform = null;
			this.audio = null;
			this.rootID = SoundPool.RootID.DISPOSED;
			GC.SuppressFinalize(this);
			GC.KeepAlive(this);
		}

		public void EnterLimbo()
		{
			switch (this.rootID)
			{
				case SoundPool.RootID.LIMBO:
				case SoundPool.RootID.DISPOSED:
				{
					return;
				}
				case SoundPool.RootID.RESERVED:
				{
					break;
				}
				default:
				{
					this.audio.Stop();
					this.audio.enabled = false;
					this.audio.clip = null;
					this.parent = null;
					break;
				}
			}
			if (!this.way.prev.has)
			{
				this.root.first = this.way.next;
			}
			else
			{
				this.way.prev.node.way.next = this.way.next;
			}
			if (this.way.next.has)
			{
				this.way.next.node.way.prev = this.way.prev;
			}
			SoundPool.Root root = this.root;
			root.count = root.count - 1;
			this.way = new SoundPool.Way();
			this.root = null;
			this.rootID = SoundPool.RootID.LIMBO;
		}

		~Node()
		{
			if ((int)this.rootID != 2)
			{
				SoundPool.NodeGC.LEAK(this.transform);
			}
			this.transform = null;
			this.audio = null;
		}

		public void Reserve()
		{
			switch (this.rootID)
			{
				case SoundPool.RootID.LIMBO:
				{
					break;
				}
				case SoundPool.RootID.RESERVED:
				case SoundPool.RootID.DISPOSED:
				{
					return;
				}
				default:
				{
					this.audio.Stop();
					this.audio.enabled = false;
					this.audio.clip = null;
					this.parent = null;
					if (this.way.next.has)
					{
						this.way.next.node.way.prev = this.way.prev;
					}
					if (!this.way.prev.has)
					{
						this.root.first = this.way.next;
					}
					else
					{
						this.way.prev.node.way.next = this.way.next;
					}
					SoundPool.Root root = this.root;
					root.count = root.count - 1;
					this.way = new SoundPool.Way();
					break;
				}
			}
			this.root = SoundPool.reserved;
			this.rootID = SoundPool.RootID.RESERVED;
			this.way.next = SoundPool.reserved.first;
			if (this.way.next.has)
			{
				this.way.next.node.way.prev.has = true;
				this.way.next.node.way.prev.node = this;
			}
			SoundPool.reserved.first.has = true;
			SoundPool.reserved.first.node = this;
			SoundPool.Root root1 = SoundPool.reserved;
			root1.count = root1.count + 1;
		}
	}

	private static class NodeGC
	{
		public static void JOIN()
		{
			Transform[] array = null;
			bool flag = false;
			object obj = SoundPool.NodeGC.GCDAT.destroyNextPumpLock;
			Monitor.Enter(obj);
			try
			{
				if (SoundPool.NodeGC.GCDAT.destroyNextQueued)
				{
					flag = true;
					array = SoundPool.NodeGC.GCDAT.destroyTheseNextPump.ToArray();
					SoundPool.NodeGC.GCDAT.destroyTheseNextPump.Clear();
					SoundPool.NodeGC.GCDAT.destroyNextQueued = false;
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
			if (flag)
			{
				Transform[] transformArrays = array;
				for (int i = 0; i < (int)transformArrays.Length; i++)
				{
					Transform transforms = transformArrays[i];
					if (transforms)
					{
						UnityEngine.Object.Destroy(transforms.gameObject);
					}
				}
				Debug.LogWarning(string.Concat("There were ", (int)array.Length, " SoundPool nodes leaked!. Cleaned them up."));
			}
		}

		public static void LEAK(Transform transform)
		{
			object obj = SoundPool.NodeGC.GCDAT.destroyNextPumpLock;
			Monitor.Enter(obj);
			try
			{
				SoundPool.NodeGC.GCDAT.destroyNextQueued = true;
				SoundPool.NodeGC.GCDAT.destroyTheseNextPump.Add(transform);
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}

		private static class GCDAT
		{
			public readonly static List<Transform> destroyTheseNextPump;

			public readonly static object destroyNextPumpLock;

			public static bool destroyNextQueued;

			static GCDAT()
			{
				SoundPool.NodeGC.GCDAT.destroyTheseNextPump = new List<Transform>();
				SoundPool.NodeGC.GCDAT.destroyNextPumpLock = new object();
				SoundPool.firstLeak = true;
			}
		}
	}

	public struct Player2D
	{
		public readonly static SoundPool.Player2D Default;

		public SoundPool.PlayerShared super;

		public float pan;

		public AudioClip clip
		{
			get
			{
				return this.super.clip;
			}
			set
			{
				this.super.clip = value;
			}
		}

		public float pitch
		{
			get
			{
				return this.super.pitch;
			}
			set
			{
				this.super.pitch = value;
			}
		}

		public int priority
		{
			get
			{
				return this.super.priority;
			}
			set
			{
				this.super.priority = value;
			}
		}

		public float volume
		{
			get
			{
				return this.super.volume;
			}
			set
			{
				this.super.volume = value;
			}
		}

		static Player2D()
		{
			SoundPool.Player2D player2D = new SoundPool.Player2D()
			{
				super = SoundPool.PlayerShared.Default,
				pan = SoundPool.DEF.pan
			};
			SoundPool.Player2D.Default = player2D;
		}

		public Player2D(AudioClip clip)
		{
			this.super = new SoundPool.PlayerShared(clip);
			this.pan = SoundPool.DEF.pan;
		}

		public void Play()
		{
			this.Play(this.clip);
		}

		public void Play(AudioClip clip)
		{
			if (!clip)
			{
				return;
			}
		}
	}

	public struct Player3D
	{
		public readonly static SoundPool.Player3D Default;

		public SoundPool.PlayerShared super;

		public float minDistance;

		public float maxDistance;

		public float spread;

		public float dopplerLevel;

		public float panLevel;

		public AudioRolloffMode rolloffMode;

		public bool cameraSticky;

		public bool bypassEffects;

		public AudioClip clip
		{
			get
			{
				return this.super.clip;
			}
			set
			{
				this.super.clip = value;
			}
		}

		public float pitch
		{
			get
			{
				return this.super.pitch;
			}
			set
			{
				this.super.pitch = value;
			}
		}

		public int priority
		{
			get
			{
				return this.super.priority;
			}
			set
			{
				this.super.priority = value;
			}
		}

		public float volume
		{
			get
			{
				return this.super.volume;
			}
			set
			{
				this.super.volume = value;
			}
		}

		static Player3D()
		{
			SoundPool.Player3D player3D = new SoundPool.Player3D()
			{
				super = SoundPool.PlayerShared.Default,
				minDistance = SoundPool.DEF.min,
				maxDistance = SoundPool.DEF.max,
				rolloffMode = SoundPool.DEF.mode,
				spread = SoundPool.DEF.spread,
				dopplerLevel = SoundPool.DEF.doppler,
				bypassEffects = SoundPool.DEF.bypassEffects,
				panLevel = SoundPool.DEF.panLevel
			};
			SoundPool.Player3D.Default = player3D;
		}

		public Player3D(AudioClip clip)
		{
			this.super = new SoundPool.PlayerShared(clip);
			this.minDistance = SoundPool.DEF.min;
			this.maxDistance = SoundPool.DEF.max;
			this.spread = SoundPool.DEF.spread;
			this.dopplerLevel = SoundPool.DEF.doppler;
			this.panLevel = SoundPool.DEF.panLevel;
			this.rolloffMode = SoundPool.DEF.mode;
			this.bypassEffects = SoundPool.DEF.bypassEffects;
			this.cameraSticky = false;
		}
	}

	public struct PlayerChild
	{
		public readonly static SoundPool.PlayerChild Default;

		public SoundPool.PlayerLocal super;

		public bool unglue;

		public Transform parent;

		public bool bypassEffects
		{
			get
			{
				return this.super.bypassEffects;
			}
			set
			{
				this.super.bypassEffects = value;
			}
		}

		public bool cameraSticky
		{
			get
			{
				return this.super.cameraSticky;
			}
			set
			{
				this.super.cameraSticky = value;
			}
		}

		public AudioClip clip
		{
			get
			{
				return this.super.clip;
			}
			set
			{
				this.super.clip = value;
			}
		}

		public float dopplerLevel
		{
			get
			{
				return this.super.dopplerLevel;
			}
			set
			{
				this.super.dopplerLevel = value;
			}
		}

		public Vector3 localPosition
		{
			get
			{
				return this.super.localPosition;
			}
			set
			{
				this.super.localPosition = value;
			}
		}

		public Quaternion localRotation
		{
			get
			{
				return this.super.localRotation;
			}
			set
			{
				this.super.localRotation = value;
			}
		}

		public float maxDistance
		{
			get
			{
				return this.super.maxDistance;
			}
			set
			{
				this.super.maxDistance = value;
			}
		}

		public float minDistance
		{
			get
			{
				return this.super.minDistance;
			}
			set
			{
				this.super.minDistance = value;
			}
		}

		public float pitch
		{
			get
			{
				return this.super.pitch;
			}
			set
			{
				this.super.pitch = value;
			}
		}

		public int priority
		{
			get
			{
				return this.super.priority;
			}
			set
			{
				this.super.priority = value;
			}
		}

		public AudioRolloffMode rolloffMode
		{
			get
			{
				return this.super.rolloffMode;
			}
			set
			{
				this.super.rolloffMode = value;
			}
		}

		public float spread
		{
			get
			{
				return this.super.spread;
			}
			set
			{
				this.super.spread = value;
			}
		}

		public float volume
		{
			get
			{
				return this.super.volume;
			}
			set
			{
				this.super.volume = value;
			}
		}

		static PlayerChild()
		{
			SoundPool.PlayerChild.Default = new SoundPool.PlayerChild()
			{
				super = SoundPool.PlayerLocal.Default
			};
		}

		public PlayerChild(AudioClip clip)
		{
			this.super = new SoundPool.PlayerLocal(clip);
			this.parent = null;
			this.unglue = false;
		}
	}

	public struct PlayerLocal
	{
		public readonly static SoundPool.PlayerLocal Default;

		public SoundPool.Player3D super;

		public Vector3 localPosition;

		public Quaternion localRotation;

		public bool bypassEffects
		{
			get
			{
				return this.super.bypassEffects;
			}
			set
			{
				this.super.bypassEffects = value;
			}
		}

		public bool cameraSticky
		{
			get
			{
				return this.super.cameraSticky;
			}
			set
			{
				this.super.cameraSticky = value;
			}
		}

		public AudioClip clip
		{
			get
			{
				return this.super.clip;
			}
			set
			{
				this.super.clip = value;
			}
		}

		public float dopplerLevel
		{
			get
			{
				return this.super.dopplerLevel;
			}
			set
			{
				this.super.dopplerLevel = value;
			}
		}

		public float maxDistance
		{
			get
			{
				return this.super.maxDistance;
			}
			set
			{
				this.super.maxDistance = value;
			}
		}

		public float minDistance
		{
			get
			{
				return this.super.minDistance;
			}
			set
			{
				this.super.minDistance = value;
			}
		}

		public float panLevel
		{
			get
			{
				return this.super.panLevel;
			}
			set
			{
				this.super.panLevel = value;
			}
		}

		public float pitch
		{
			get
			{
				return this.super.pitch;
			}
			set
			{
				this.super.pitch = value;
			}
		}

		public int priority
		{
			get
			{
				return this.super.priority;
			}
			set
			{
				this.super.priority = value;
			}
		}

		public AudioRolloffMode rolloffMode
		{
			get
			{
				return this.super.rolloffMode;
			}
			set
			{
				this.super.rolloffMode = value;
			}
		}

		public float spread
		{
			get
			{
				return this.super.spread;
			}
			set
			{
				this.super.spread = value;
			}
		}

		public float volume
		{
			get
			{
				return this.super.volume;
			}
			set
			{
				this.super.volume = value;
			}
		}

		static PlayerLocal()
		{
			SoundPool.PlayerLocal playerLocal = new SoundPool.PlayerLocal()
			{
				super = SoundPool.Player3D.Default,
				localPosition = new Vector3(),
				localRotation = Quaternion.identity
			};
			SoundPool.PlayerLocal.Default = playerLocal;
		}

		public PlayerLocal(AudioClip clip)
		{
			this.super = new SoundPool.Player3D(clip);
			this.localPosition = new Vector3();
			this.localRotation = Quaternion.identity;
		}
	}

	public struct PlayerShared
	{
		public readonly static SoundPool.PlayerShared Default;

		public AudioClip clip;

		public float volume;

		public float pitch;

		public int priority;

		static PlayerShared()
		{
			SoundPool.PlayerShared playerShared = new SoundPool.PlayerShared()
			{
				volume = SoundPool.DEF.volume,
				pitch = SoundPool.DEF.pitch,
				priority = SoundPool.DEF.priority
			};
			SoundPool.PlayerShared.Default = playerShared;
		}

		public PlayerShared(AudioClip clip)
		{
			this.clip = clip;
			this.volume = SoundPool.DEF.volume;
			this.pitch = SoundPool.DEF.pitch;
			this.priority = SoundPool.DEF.priority;
		}
	}

	public struct PlayerWorld
	{
		public readonly static SoundPool.PlayerWorld Default;

		public SoundPool.Player3D super;

		public Vector3 position;

		public Quaternion rotation;

		public bool bypassEffects
		{
			get
			{
				return this.super.bypassEffects;
			}
			set
			{
				this.super.bypassEffects = value;
			}
		}

		public bool cameraSticky
		{
			get
			{
				return this.super.cameraSticky;
			}
			set
			{
				this.super.cameraSticky = value;
			}
		}

		public AudioClip clip
		{
			get
			{
				return this.super.clip;
			}
			set
			{
				this.super.clip = value;
			}
		}

		public float dopplerLevel
		{
			get
			{
				return this.super.dopplerLevel;
			}
			set
			{
				this.super.dopplerLevel = value;
			}
		}

		public float maxDistance
		{
			get
			{
				return this.super.maxDistance;
			}
			set
			{
				this.super.maxDistance = value;
			}
		}

		public float minDistance
		{
			get
			{
				return this.super.minDistance;
			}
			set
			{
				this.super.minDistance = value;
			}
		}

		public float panLevel
		{
			get
			{
				return this.super.panLevel;
			}
			set
			{
				this.super.panLevel = value;
			}
		}

		public float pitch
		{
			get
			{
				return this.super.pitch;
			}
			set
			{
				this.super.pitch = value;
			}
		}

		public int priority
		{
			get
			{
				return this.super.priority;
			}
			set
			{
				this.super.priority = value;
			}
		}

		public AudioRolloffMode rolloffMode
		{
			get
			{
				return this.super.rolloffMode;
			}
			set
			{
				this.super.rolloffMode = value;
			}
		}

		public float spread
		{
			get
			{
				return this.super.spread;
			}
			set
			{
				this.super.spread = value;
			}
		}

		public float volume
		{
			get
			{
				return this.super.volume;
			}
			set
			{
				this.super.volume = value;
			}
		}

		static PlayerWorld()
		{
			SoundPool.PlayerWorld playerWorld = new SoundPool.PlayerWorld()
			{
				super = SoundPool.Player3D.Default,
				position = new Vector3(),
				rotation = Quaternion.identity
			};
			SoundPool.PlayerWorld.Default = playerWorld;
		}

		public PlayerWorld(AudioClip clip)
		{
			this.super = new SoundPool.Player3D(clip);
			this.position = new Vector3();
			this.rotation = Quaternion.identity;
		}
	}

	private class Root
	{
		public int count;

		public SoundPool.Dir first;

		public readonly SoundPool.RootID id;

		public Root(SoundPool.RootID id)
		{
			this.id = id;
		}
	}

	private enum RootID : sbyte
	{
		PLAYING_ATTACHED = -3,
		PLAYING_CAMERA = -2,
		PLAYING = -1,
		LIMBO = 0,
		RESERVED = 1,
		DISPOSED = 2
	}

	private struct Settings
	{
		public AudioClip clip;

		public Transform parent;

		public Quaternion localRotation;

		public Vector3 localPosition;

		public float volume;

		public float pitch;

		public float pan;

		public float panLevel;

		public float min;

		public float max;

		public float doppler;

		public float spread;

		public int priority;

		public AudioRolloffMode mode;

		public sbyte SelectRoot;

		public bool bypassEffects;

		public static explicit operator Settings(SoundPool.PlayerShared player)
		{
			SoundPool.Settings dEF = SoundPool.DEF;
			dEF.clip = player.clip;
			dEF.volume = player.volume;
			dEF.pitch = player.pitch;
			dEF.priority = player.priority;
			return dEF;
		}

		public static explicit operator Settings(SoundPool.Player3D player)
		{
			SoundPool.Settings setting = (SoundPool.Settings)player.super;
			setting.doppler = player.dopplerLevel;
			setting.min = player.minDistance;
			setting.max = player.maxDistance;
			setting.panLevel = player.panLevel;
			setting.spread = player.spread;
			setting.mode = player.rolloffMode;
			setting.bypassEffects = player.bypassEffects;
			setting.SelectRoot = (!player.cameraSticky ? 0 : 5);
			return setting;
		}

		public static explicit operator Settings(SoundPool.PlayerLocal player)
		{
			SoundPool.Settings setting = (SoundPool.Settings)player.super;
			setting.localPosition = player.localPosition;
			setting.localRotation = player.localRotation;
			return setting;
		}

		public static explicit operator Settings(SoundPool.Player2D player)
		{
			SoundPool.Settings setting = (SoundPool.Settings)player.super;
			setting.pan = player.pan;
			return setting;
		}
	}

	private struct Way
	{
		public SoundPool.Dir prev;

		public SoundPool.Dir next;
	}
}