using System;
using System.Collections.Generic;
using UnityEngine;

public class EngineSoundLoop : ScriptableObject
{
	private const float kPitchDefault_Idle = 0.7f;

	private const float kPitchDefault_Start = 0.85f;

	private const float kPitchDefault_Low = 1.17f;

	private const float kPitchDefault_Medium = 1.25f;

	private const float kPitchDefault_High1 = 1.65f;

	private const float kPitchDefault_High2 = 1.76f;

	private const float kPitchDefault_High3 = 1.8f;

	private const float kPitchDefault_High4 = 1.86f;

	private const float kPitchDefault_Shift = 1.44f;

	private const float F_PITCH = 0.8f;

	private const float F_THROTTLE = 0.7f;

	private const float E_PITCH = 0.89f;

	private const float E_THROTTLE = 0.8f;

	private const float sD = 0.4f;

	private const float sF = 0.4f;

	private const float sE = 0.4f;

	private const float sK = 0.7f;

	private const float sL = 0.4f;

	private const float F_PITCH_DELTA = 0.199999988f;

	private const float F_THROTTLE_DELTA = 0.3f;

	private const float E_PITCH_DELTA = 0.110000014f;

	private const float E_THROTTLE_DELTA = 0.199999988f;

	[SerializeField]
	private EngineSoundLoop.Phrase _dUpper = new EngineSoundLoop.Phrase(0.565f);

	[SerializeField]
	private EngineSoundLoop.Phrase _fMidHigh = new EngineSoundLoop.Phrase(0.78f);

	[SerializeField]
	private EngineSoundLoop.Phrase _eMidLow = new EngineSoundLoop.Phrase(0.8f);

	[SerializeField]
	private EngineSoundLoop.Phrase _lLower = new EngineSoundLoop.Phrase(0.61f);

	[SerializeField]
	private EngineSoundLoop.Phrase _kPassing = new EngineSoundLoop.Phrase(0.565f);

	[SerializeField]
	private EngineSoundLoop.Gear _idleShiftUp = new EngineSoundLoop.Gear(1.17f, 1.65f);

	[SerializeField]
	private EngineSoundLoop.Gear _shiftUp = new EngineSoundLoop.Gear(1.17f, 1.76f);

	[SerializeField]
	private EngineSoundLoop.Gear[] _gears = new EngineSoundLoop.Gear[] { new EngineSoundLoop.Gear(0.7f, 1.65f), new EngineSoundLoop.Gear(0.85f, 1.76f), new EngineSoundLoop.Gear(1.17f, 1.8f), new EngineSoundLoop.Gear(1.25f, 1.86f) };

	[SerializeField]
	private EngineSoundLoop.Gear _shiftDown = new EngineSoundLoop.Gear(1.44f, 1.17f);

	[SerializeField]
	private float _shiftDuration = 0.1f;

	[SerializeField]
	private float _volumeFromPitchBase = 0.85f;

	[SerializeField]
	private float _volumeFromPitchRange = 0.909999967f;

	[SerializeField]
	private int _topGear = 4;

	[NonSerialized]
	private Dictionary<Transform, EngineSoundLoop.Instance> instances;

	private float volumeD
	{
		get
		{
			return this._dUpper.volume * 0.4f;
		}
	}

	private float volumeE
	{
		get
		{
			return this._eMidLow.volume * 0.4f;
		}
	}

	private float volumeF
	{
		get
		{
			return this._fMidHigh.volume * 0.4f;
		}
	}

	private float volumeK
	{
		get
		{
			return this._kPassing.volume * 0.7f;
		}
	}

	private float volumeL
	{
		get
		{
			return this._lLower.volume * 0.4f;
		}
	}

	public EngineSoundLoop()
	{
	}

	private static float Coserp(float start, float end, float value)
	{
		float single;
		float single1 = Mathf.Cos(value * 1.57079637f);
		if (single1 < 1f)
		{
			single = (single1 > 0f ? start * single1 + end * (1f - single1) : end);
		}
		else
		{
			single = start;
		}
		return single;
	}

	public EngineSoundLoop.Instance Create(Transform attachTo, Vector3 localPosition)
	{
		EngineSoundLoop.Instance instance;
		if (!attachTo)
		{
			throw new MissingReferenceException("attachTo must not be null or destroyed");
		}
		if (this.instances == null)
		{
			this.instances = new Dictionary<Transform, EngineSoundLoop.Instance>();
		}
		else if (this.instances.TryGetValue(attachTo, out instance))
		{
			instance.localPosition = localPosition;
			return instance;
		}
		instance = new EngineSoundLoop.Instance(attachTo, localPosition, this);
		this.instances[attachTo] = instance;
		return instance;
	}

	public EngineSoundLoop.Instance Create(Transform attachTo)
	{
		return this.Create(attachTo, Vector3.zero);
	}

	public EngineSoundLoop.Instance CreateWorld(Transform attachTo, Vector3 worldPosition)
	{
		return this.Create(attachTo, attachTo.InverseTransformPoint(worldPosition));
	}

	private void GearLerp(byte gear, float factor, ref float pitch, ref bool pitchChanged, ref float volume, ref bool volumeChanged)
	{
		int num;
		if (this._gears != null)
		{
			int length = (int)this._gears.Length;
			int num1 = length;
			if (length != 0)
			{
				num = (this._topGear >= num1 ? num1 - 1 : this._topGear);
				if (gear <= num)
				{
					this._gears[gear].CompareLerp(factor, ref pitch, ref pitchChanged, ref volume, ref volumeChanged);
				}
				else
				{
					this._gears[num].CompareLerp(factor, ref pitch, ref pitchChanged, ref volume, ref volumeChanged);
				}
				return;
			}
		}
	}

	private static float Sinerp(float start, float end, float value)
	{
		float single;
		float single1 = Mathf.Sin(value * 1.57079637f);
		if (single1 > 0f)
		{
			single = (single1 < 1f ? end * single1 + start * (1f - single1) : end);
		}
		else
		{
			single = start;
		}
		return single;
	}

	private sbyte VolumeFactor(float pitch, out float between)
	{
		between = (pitch - this._volumeFromPitchBase) / this._volumeFromPitchRange;
		if (between >= 1f)
		{
			between = 1f;
			return 1;
		}
		if (between > 0f)
		{
			return 0;
		}
		between = 0f;
		return -1;
	}

	[Serializable]
	private class Gear
	{
		public float lowPitch;

		public float lowVolume;

		public float highPitch;

		public float highVolume;

		public Gear() : this(0.7f, 1.65f)
		{
		}

		public Gear(float lower, float upper)
		{
			float single = lower;
			float single1 = single;
			this.lowVolume = single;
			this.lowPitch = single1;
			float single2 = upper;
			single1 = single2;
			this.highVolume = single2;
			this.highPitch = single1;
		}

		public Gear(float lowerPitch, float lowerVolume, float upperPitch, float upperVolume)
		{
		}

		public void CompareLerp(float t, ref float pitch, ref bool pitchChanged, ref float volume, ref bool volumeChanged)
		{
			if (t <= 0f)
			{
				if (pitch != this.lowPitch)
				{
					pitchChanged = true;
					pitch = this.lowPitch;
				}
				if (volume != this.lowVolume)
				{
					volumeChanged = true;
					volume = this.lowVolume;
				}
			}
			else if (t < 1f)
			{
				float single = 1f - t;
				float single1 = this.lowPitch * single + this.highPitch * t;
				float single2 = this.lowVolume * single + this.highVolume * t;
				if (pitch != single1)
				{
					pitchChanged = true;
					pitch = single1;
				}
				if (volume != single2)
				{
					volumeChanged = true;
					volume = single2;
				}
			}
			else
			{
				if (pitch != this.highPitch)
				{
					pitchChanged = true;
					pitch = this.highPitch;
				}
				if (volume != this.highVolume)
				{
					volumeChanged = true;
					volume = this.highVolume;
				}
			}
		}

		public void Lerp(float t, out float pitch, out float volume)
		{
			if (t <= 0f)
			{
				pitch = this.lowPitch;
				volume = this.lowVolume;
			}
			else if (t < 1f)
			{
				float single = 1f - t;
				pitch = this.lowPitch * single + this.highPitch * t;
				volume = this.lowVolume * single + this.highVolume * t;
			}
			else
			{
				pitch = this.highPitch;
				volume = this.highVolume;
			}
		}
	}

	public class Instance : IDisposable
	{
		private const ushort kD = 1;

		private const ushort kF = 2;

		private const ushort kE = 4;

		private const ushort kL = 8;

		private const ushort kK = 16;

		private const ushort kDisposed = 32;

		private const ushort kPlaying = 64;

		private const ushort kPaused = 128;

		private const ushort kShifting = 256;

		private const ushort kShiftingDown = 256;

		private const ushort kShiftingUp = 768;

		private const ushort kFlagOnceUpdate = 1024;

		private const ushort FLAGS_MASK = 65535;

		private const ushort nD = 65534;

		private const ushort nF = 65533;

		private const ushort nE = 65531;

		private const ushort nL = 65527;

		private const ushort nK = 65519;

		private const ushort nDisposed = 65503;

		private const ushort nPlaying = 65471;

		private const ushort nPaused = 65407;

		private const ushort nShifting = 65279;

		private const ushort nShiftingDown = 65279;

		private const ushort nShiftingUp = 64767;

		private const ushort kPlayingOrPaused = 192;

		private const ushort kShiftingUpOrDown = 768;

		private const ushort nPlayingOrPaused = 65343;

		private const ushort nShiftingUpOrDown = 64767;

		[NonSerialized]
		private EngineSoundLoop loop;

		[NonSerialized]
		private EngineSoundLoopPlayer player;

		[NonSerialized]
		private Transform parent;

		[NonSerialized]
		private AudioSource D;

		[NonSerialized]
		private AudioSource E;

		[NonSerialized]
		private AudioSource F;

		[NonSerialized]
		private AudioSource L;

		[NonSerialized]
		private AudioSource K;

		[NonSerialized]
		private float _volume;

		[NonSerialized]
		private float _pitch;

		[NonSerialized]
		private float _masterVolume;

		[NonSerialized]
		private float _speedFactor;

		[NonSerialized]
		private float _shiftTime;

		[NonSerialized]
		private float _throttle;

		[NonSerialized]
		private float _lastPitchFactor;

		[NonSerialized]
		private float _lastSinerp;

		[NonSerialized]
		private float _lastVolumeFactor;

		[NonSerialized]
		private float _lastClampedThrottle;

		[NonSerialized]
		private float _dVol;

		[NonSerialized]
		private float _fVol;

		[NonSerialized]
		private float _eVol;

		[NonSerialized]
		private float _kVol;

		[NonSerialized]
		private ushort flags;

		[NonSerialized]
		private byte _gear;

		[NonSerialized]
		private sbyte _lastVolumeFactorClamp;

		public bool anySounds
		{
			get
			{
				return ((this.flags & 31) == 0 ? false : (this.flags & 32) == 0);
			}
		}

		public bool disposed
		{
			get
			{
				return (this.flags & 32) == 32;
			}
		}

		public bool hasUpdated
		{
			get
			{
				return (this.flags & 1024) == 1024;
			}
		}

		internal Vector3 localPosition
		{
			set
			{
				if ((this.flags & 32) == 32)
				{
					return;
				}
				this.player.transform.localPosition = value;
			}
		}

		public bool paused
		{
			get
			{
				return (this.flags & 160) == 128;
			}
			set
			{
				if (value)
				{
					if ((this.flags & 224) == 64)
					{
						this.PAUSE();
					}
				}
				else if ((this.flags & 224) == 128)
				{
					this.PLAY();
				}
			}
		}

		public bool playing
		{
			get
			{
				return (this.flags & 96) == 64;
			}
			set
			{
				if (value)
				{
					if ((this.flags & 96) == 0)
					{
						this.PLAY();
					}
				}
				else if ((this.flags & 96) == 64)
				{
					this.PAUSE();
				}
			}
		}

		public bool playingOrPaused
		{
			get
			{
				return ((this.flags & 96) == 64 ? true : (this.flags & 160) == 128);
			}
		}

		public float speedFactor
		{
			get
			{
				return this._speedFactor;
			}
		}

		public bool stopped
		{
			get
			{
				return (this.flags & 192) == 0;
			}
		}

		public float volume
		{
			get
			{
				return this._masterVolume;
			}
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}
				if (this._masterVolume != value)
				{
					this._masterVolume = value;
					if ((this.flags & 32) == 0)
					{
						this.UPDATE_MASTER_VOLUME();
					}
				}
			}
		}

		internal Instance(Transform parent, Vector3 offset, EngineSoundLoop loop)
		{
			this.parent = parent;
			this.loop = loop;
			GameObject gameObject = new GameObject("_EnginePlayer", new Type[] { typeof(EngineSoundLoopPlayer) });
			this.player = gameObject.GetComponent<EngineSoundLoopPlayer>();
			this.player.instance = this;
			EngineSoundLoop.Instance.Setup(gameObject, ref this.D, ref this.flags, 1, loop._dUpper, 1f);
			EngineSoundLoop.Instance.Setup(gameObject, ref this.F, ref this.flags, 2, loop._fMidHigh, 1f);
			EngineSoundLoop.Instance.Setup(gameObject, ref this.E, ref this.flags, 4, loop._eMidLow, 1f);
			EngineSoundLoop.Instance.Setup(gameObject, ref this.L, ref this.flags, 8, loop._lLower, 1f);
			EngineSoundLoop.Instance.Setup(gameObject, ref this.K, ref this.flags, 16, loop._kPassing, 0f);
			float single = Single.NegativeInfinity;
			float single1 = single;
			this._lastPitchFactor = single;
			float single2 = single1;
			single1 = single2;
			this._lastSinerp = single2;
			float single3 = single1;
			single1 = single3;
			this._lastClampedThrottle = single3;
			this._lastVolumeFactor = single1;
			this._lastVolumeFactorClamp = -128;
			this._masterVolume = 1f;
			this._pitch = loop._idleShiftUp.lowVolume;
			this._shiftTime = -3000f;
			float single4 = 0f;
			single1 = single4;
			this._throttle = single4;
			float single5 = single1;
			single1 = single5;
			this._kVol = single5;
			float single6 = single1;
			single1 = single6;
			this._eVol = single6;
			float single7 = single1;
			single1 = single7;
			this._fVol = single7;
			float single8 = single1;
			single1 = single8;
			this._dVol = single8;
			this._speedFactor = single1;
			this._gear = 0;
			Transform transforms = gameObject.transform;
			transforms.parent = parent;
			transforms.localPosition = offset;
			transforms.localRotation = Quaternion.identity;
		}

		public void Dispose()
		{
			this.Dispose(false);
		}

		internal void Dispose(bool fromPlayer)
		{
			if ((this.flags & 32) == 32)
			{
				return;
			}
			if (this.loop && this.loop.instances != null)
			{
				this.loop.instances.Remove(this.parent);
			}
			object obj = null;
			AudioSource audioSource = (AudioSource)obj;
			this.K = (AudioSource)obj;
			AudioSource audioSource1 = audioSource;
			audioSource = audioSource1;
			this.L = audioSource1;
			AudioSource audioSource2 = audioSource;
			audioSource = audioSource2;
			this.F = audioSource2;
			AudioSource audioSource3 = audioSource;
			audioSource = audioSource3;
			this.E = audioSource3;
			this.D = audioSource;
			if (!fromPlayer && this.player)
			{
				try
				{
					this.player.instance = null;
					UnityEngine.Object.Destroy(this.player.gameObject);
				}
				catch (Exception exception)
				{
					Debug.LogError(exception, this.player);
				}
			}
			this.player = null;
			this.flags = 32;
		}

		public void Pause()
		{
			if ((this.flags & 224) == 64)
			{
				this.PAUSE();
			}
		}

		private void PAUSE()
		{
			if ((this.flags & 1) == 1)
			{
				this.D.Pause();
			}
			if ((this.flags & 2) == 2)
			{
				this.F.Pause();
			}
			if ((this.flags & 4) == 4)
			{
				this.E.Pause();
			}
			if ((this.flags & 8) == 8)
			{
				this.L.Pause();
			}
			if ((this.flags & 16) == 16)
			{
				this.K.Pause();
			}
			EngineSoundLoop.Instance instance = this;
			instance.flags = (ushort)(instance.flags | 128);
			EngineSoundLoop.Instance instance1 = this;
			instance1.flags = (ushort)(instance1.flags & 65471);
		}

		public void Play()
		{
			if ((this.flags & 96) == 0)
			{
				this.PLAY();
			}
		}

		private void PLAY()
		{
			if ((this.flags & 1024) == 1024)
			{
				if ((this.flags & 1) == 1)
				{
					this.D.Play();
				}
				if ((this.flags & 2) == 2)
				{
					this.F.Play();
				}
				if ((this.flags & 4) == 4)
				{
					this.E.Play();
				}
				if ((this.flags & 8) == 8)
				{
					this.L.Play();
				}
				if ((this.flags & 16) == 16)
				{
					this.K.Play();
				}
			}
			EngineSoundLoop.Instance instance = this;
			instance.flags = (ushort)(instance.flags | 64);
			EngineSoundLoop.Instance instance1 = this;
			instance1.flags = (ushort)(instance1.flags & 65407);
		}

		private static void Setup(GameObject go, ref AudioSource source, ref ushort flags, ushort flag, EngineSoundLoop.Phrase phrase, float volumeScalar)
		{
			if (phrase == null || !phrase.clip)
			{
				return;
			}
			source = go.AddComponent<AudioSource>();
			source.playOnAwake = false;
			source.loop = true;
			source.clip = phrase.clip;
			source.volume = phrase.volume * volumeScalar;
			source.dopplerLevel = 0f;
			flags = (ushort)(flags | flag);
		}

		public void Stop()
		{
			if ((this.flags & 32) == 0 && (this.flags & 192) != 0)
			{
				this.STOP();
			}
		}

		private void STOP()
		{
			if ((this.flags & 1) == 1)
			{
				this.D.Stop();
			}
			if ((this.flags & 2) == 2)
			{
				this.F.Stop();
			}
			if ((this.flags & 4) == 4)
			{
				this.E.Stop();
			}
			if ((this.flags & 8) == 8)
			{
				this.L.Stop();
			}
			if ((this.flags & 16) == 16)
			{
				this.K.Stop();
			}
			EngineSoundLoop.Instance instance = this;
			instance.flags = (ushort)(instance.flags & 65343);
		}

		public void Update(float speedFactor, float throttle)
		{
			int num = this.flags & 1056;
			if (num == 32)
			{
				this._speedFactor = speedFactor;
				this._throttle = throttle;
			}
			else if (num == 1024)
			{
				this.UPDATE(speedFactor, throttle);
			}
			else
			{
				EngineSoundLoop.Instance instance = this;
				instance.flags = (ushort)(instance.flags | 1024);
				this.UPDATE(speedFactor, throttle);
				if ((this.flags & 192) == 64)
				{
					this.PLAY();
				}
			}
		}

		private void UPDATE(float speedFactor, float throttle)
		{
			bool flag;
			bool flag1;
			bool flag2;
			byte num;
			bool flag3;
			float single;
			if (throttle == this._throttle)
			{
				flag = false;
			}
			else
			{
				this._throttle = throttle;
				flag = true;
			}
			float single1 = this._pitch;
			float single2 = this._volume;
			float single3 = this._speedFactor;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = speedFactor != single3;
			if (flag6)
			{
				this._speedFactor = speedFactor;
			}
			if ((this.flags & 256) != 256)
			{
				flag1 = true;
				flag2 = false;
			}
			else
			{
				bool flag7 = this.UPDATE_SHIFTING(ref flag4, ref flag5);
				flag2 = flag7;
				flag1 = flag7;
			}
			if (flag1)
			{
				while (true)
				{
					if (flag6 || flag2)
					{
						int num1 = this.loop._topGear;
						this._lastSinerp = EngineSoundLoop.Sinerp(0f, (float)num1, speedFactor);
						int num2 = (int)this._lastSinerp;
						if (num2 == this._gear)
						{
							flag3 = false;
							num = (num2 != num1 ? this._gear : (byte)(num1 - 1));
						}
						else if (num2 < this._gear)
						{
							if (this._gear <= 0)
							{
								flag3 = false;
								num = this._gear;
							}
							else if (this._gear != num1)
							{
								flag3 = true;
								num = (byte)(this._gear - 1);
							}
							else
							{
								EngineSoundLoop.Instance instance = this;
								instance._gear = (byte)(instance._gear - 1);
								flag3 = false;
								num = this._gear;
							}
						}
						else if (this._gear >= 255 || this._gear >= num1)
						{
							flag3 = false;
							num = this._gear;
						}
						else if (this._gear >= num1 - 1)
						{
							flag3 = false;
							num = this._gear;
							EngineSoundLoop.Instance instance1 = this;
							instance1._gear = (byte)(instance1._gear + 1);
						}
						else
						{
							flag3 = true;
							num = (byte)(this._gear + 1);
						}
					}
					else
					{
						flag3 = false;
						num = (this._gear != this.loop._topGear ? this._gear : (byte)(this._gear - 1));
					}
					float single4 = this._lastSinerp - (float)num;
					if (single4 == 0f)
					{
						single = 0f;
					}
					else if (throttle < 0.5f)
					{
						single = (throttle > 0f ? single4 * (0.3f + throttle * 0.7f) : single4 * 0.3f);
					}
					else
					{
						single = single4;
					}
					if (!flag3)
					{
						break;
					}
					if (num <= this._gear)
					{
						EngineSoundLoop.Instance instance2 = this;
						instance2.flags = (ushort)(instance2.flags | 256);
					}
					else
					{
						EngineSoundLoop.Instance instance3 = this;
						instance3.flags = (ushort)(instance3.flags | 768);
					}
					this._lastPitchFactor = single;
					this._shiftTime = Time.time;
					bool flag8 = this.UPDATE_SHIFTING(ref flag4, ref flag5);
					flag2 = flag8;
					if (!flag8)
					{
						if (flag5 && this._volume != single2)
						{
							this.UPDATE_PITCH_AND_OR_THROTTLE_VOLUME();
						}
						else if (flag)
						{
							this.UPDATE_THROTTLE_VOLUME();
						}
						if (flag6)
						{
							this.UPDATE_PASSING_VOLUME();
						}
						if (flag4 && this._pitch != single1)
						{
							this.UPDATE_RATES();
						}
						return;
					}
				}
				if (single != this._lastPitchFactor || flag2)
				{
					this._lastPitchFactor = single;
					this.loop.GearLerp(num, single, ref this._pitch, ref flag4, ref this._volume, ref flag5);
				}
			}
			if (flag5 && this._volume != single2)
			{
				this.UPDATE_PITCH_AND_OR_THROTTLE_VOLUME();
			}
			else if (flag)
			{
				this.UPDATE_THROTTLE_VOLUME();
			}
			if (flag6)
			{
				this.UPDATE_PASSING_VOLUME();
			}
			if (flag4 && this._pitch != single1)
			{
				this.UPDATE_RATES();
			}
		}

		private void UPDATE_MASTER_VOLUME()
		{
			if ((this.flags & 1) == 1)
			{
				this.D.volume = this._dVol * this._masterVolume;
			}
			if ((this.flags & 2) == 2)
			{
				this.F.volume = this._fVol * this._masterVolume;
			}
			if ((this.flags & 4) == 4)
			{
				this.E.volume = this._eVol * this._masterVolume;
			}
			if ((this.flags & 8) == 8)
			{
				this.L.volume = this.loop.volumeL * this._masterVolume;
			}
			if ((this.flags & 16) == 16)
			{
				this.K.volume = this._kVol * this._masterVolume;
			}
		}

		private void UPDATE_PASSING_VOLUME()
		{
			if ((this.flags & 16) == 16)
			{
				AudioSource k = this.K;
				float single = this.loop.volumeK * this._speedFactor;
				float single1 = single;
				this._kVol = single;
				k.volume = single1 * this._masterVolume;
			}
		}

		private void UPDATE_PITCH_AND_OR_THROTTLE_VOLUME()
		{
			float single;
			bool flag;
			float single1;
			float single2;
			float single3;
			ushort num = this.flags;
			num = (ushort)(num & 7);
			if (num != 0)
			{
				sbyte num1 = this.loop.VolumeFactor(this._volume, out single);
				if ((int)this._lastVolumeFactorClamp != (int)num1 || this._lastVolumeFactor != single)
				{
					flag = true;
					this._lastVolumeFactor = single;
					this._lastVolumeFactorClamp = num1;
					if ((num & 1) == 1)
					{
						AudioSource d = this.D;
						if ((int)num1 != -1)
						{
							float single4 = this._masterVolume;
							single2 = ((int)num1 != 1 ? this.loop.volumeD * single : this.loop.volumeD);
							single1 = single2;
							this._dVol = single2;
							single3 = single4 * single1;
						}
						else
						{
							float single5 = 0f;
							single1 = single5;
							this._dVol = single5;
							single3 = single1;
						}
						d.volume = single3;
					}
				}
				else
				{
					flag = false;
				}
				num = (ushort)(num & 65534);
				if (num != 0)
				{
					float single6 = Mathf.Clamp01(this._throttle);
					if (single6 != this._lastClampedThrottle)
					{
						this._lastClampedThrottle = single6;
						flag = true;
					}
					if (flag)
					{
						switch (num1)
						{
							case -1:
							{
								if ((num & 2) == 2)
								{
									float single7 = this.loop.volumeF * 0.8f * (0.7f + 0.3f * single6);
									single1 = single7;
									this._fVol = single7;
									this.F.volume = single1 * this._masterVolume;
								}
								if ((num & 4) == 4)
								{
									float single8 = this.loop.volumeE * 0.89f * (0.8f + 0.199999988f * single6);
									single1 = single8;
									this._eVol = single8;
									this.E.volume = single1 * this._masterVolume;
								}
								break;
							}
							case 0:
							{
								if ((num & 2) == 2)
								{
									float single9 = this.loop.volumeF * (0.8f + 0.199999988f * single) * (0.7f + 0.3f * single6);
									single1 = single9;
									this._fVol = single9;
									this.F.volume = single1 * this._masterVolume;
								}
								if ((num & 4) == 4)
								{
									float single10 = this.loop.volumeE * (0.89f + 0.110000014f * single) * (0.8f + 0.199999988f * single6);
									single1 = single10;
									this._eVol = single10;
									this.E.volume = single1 * this._masterVolume;
								}
								break;
							}
							case 1:
							{
								if ((num & 2) == 2)
								{
									float single11 = this.loop.volumeF * (0.7f + 0.3f * single6);
									single1 = single11;
									this._fVol = single11;
									this.F.volume = single1 * this._masterVolume;
								}
								if ((num & 4) == 4)
								{
									float single12 = this.loop.volumeE * (0.8f + 0.199999988f * single6);
									single1 = single12;
									this._eVol = single12;
									this.E.volume = single1 * this._masterVolume;
								}
								break;
							}
							default:
							{
								goto case 0;
							}
						}
					}
				}
			}
		}

		private void UPDATE_RATES()
		{
			if ((this.flags & 1) == 1)
			{
				this.D.pitch = this._pitch;
			}
			if ((this.flags & 2) == 2)
			{
				this.F.pitch = this._pitch;
			}
			if ((this.flags & 4) == 4)
			{
				this.E.pitch = this._pitch;
			}
			if ((this.flags & 8) == 8)
			{
				this.L.pitch = this._pitch;
			}
		}

		private bool UPDATE_SHIFTING(ref bool doPitchAdjust, ref bool doVolumeAdjust)
		{
			float single;
			EngineSoundLoop.Gear gear;
			float single1 = Time.time - this._shiftTime;
			if (single1 >= this.loop._shiftDuration)
			{
				if ((this.flags & 768) == 768)
				{
					EngineSoundLoop.Instance instance = this;
					instance._gear = (byte)(instance._gear + 1);
				}
				else if (this._gear > 0)
				{
					EngineSoundLoop.Instance instance1 = this;
					instance1._gear = (byte)(instance1._gear - 1);
				}
				EngineSoundLoop.Instance instance2 = this;
				instance2.flags = (ushort)(instance2.flags & 64767);
				return true;
			}
			float single2 = single1 / this.loop._shiftDuration;
			if ((this.flags & 768) != 768)
			{
				single = single2;
				gear = this.loop._shiftDown;
			}
			else
			{
				single = this._lastPitchFactor * single2;
				gear = (this._gear != 0 ? this.loop._shiftUp : this.loop._idleShiftUp);
			}
			gear.CompareLerp(single, ref this._pitch, ref doPitchAdjust, ref this._volume, ref doVolumeAdjust);
			return false;
		}

		private void UPDATE_THROTTLE_VOLUME()
		{
			float single;
			ushort num = this.flags;
			num = (ushort)(num & 6);
			if (num != 0)
			{
				float single1 = Mathf.Clamp01(this._throttle);
				if (single1 != this._lastClampedThrottle)
				{
					float single2 = this._lastVolumeFactor;
					this._lastClampedThrottle = single1;
					switch (this._lastVolumeFactorClamp)
					{
						case -1:
						{
							if ((num & 2) == 2)
							{
								float single3 = this.loop.volumeF * 0.8f * (0.7f + 0.3f * single1);
								single = single3;
								this._fVol = single3;
								this.F.volume = single * this._masterVolume;
							}
							if ((num & 4) == 4)
							{
								float single4 = this.loop.volumeE * 0.89f * (0.8f + 0.199999988f * single1);
								single = single4;
								this._eVol = single4;
								this.E.volume = single * this._masterVolume;
							}
							break;
						}
						case 0:
						{
							if ((num & 2) == 2)
							{
								float single5 = this.loop.volumeF * (0.8f + 0.199999988f * single2) * (0.7f + 0.3f * single1);
								single = single5;
								this._fVol = single5;
								this.F.volume = single * this._masterVolume;
							}
							if ((num & 4) == 4)
							{
								float single6 = this.loop.volumeE * (0.89f + 0.110000014f * single2) * (0.8f + 0.199999988f * single1);
								single = single6;
								this._eVol = single6;
								this.E.volume = single * this._masterVolume;
							}
							break;
						}
						case 1:
						{
							if ((num & 2) == 2)
							{
								float single7 = this.loop.volumeF * (0.7f + 0.3f * single1);
								single = single7;
								this._fVol = single7;
								this.F.volume = single * this._masterVolume;
							}
							if ((num & 4) == 4)
							{
								float single8 = this.loop.volumeE * (0.8f + 0.199999988f * single1);
								single = single8;
								this._eVol = single8;
								this.E.volume = single * this._masterVolume;
							}
							break;
						}
						default:
						{
							goto case 0;
						}
					}
				}
			}
		}
	}

	[Serializable]
	private class Phrase
	{
		public AudioClip clip;

		public float volume;

		public Phrase()
		{
			this.volume = 1f;
		}

		public Phrase(float volume)
		{
			this.volume = volume;
		}
	}
}