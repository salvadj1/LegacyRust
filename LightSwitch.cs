using Facepunch;
using System;
using System.IO;
using uLink;
using UnityEngine;

[NGCAutoAddScript]
public class LightSwitch : NetBehaviour, IActivatable, IActivatableToggle, IContextRequestable, IContextRequestableQuick, IContextRequestableText, IContextRequestablePointText, IComponentInterface<IActivatable, Facepunch.MonoBehaviour, Activatable>, IComponentInterface<IActivatable, Facepunch.MonoBehaviour>, IComponentInterface<IActivatable>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	[SerializeField]
	protected LightStylist[] stylists;

	private LightSwitch.StylistCTX[] stylistCTX;

	private double lastChangeTime;

	[SerializeField]
	protected LightStyle[] randOn;

	[SerializeField]
	protected LightStyle[] randOff;

	[SerializeField]
	private int _randSeed;

	[SerializeField]
	protected float minOnFadeDuration;

	[SerializeField]
	protected float maxOnFadeDuration;

	[SerializeField]
	protected float minOffFadeDuration;

	[SerializeField]
	protected float maxOffFadeDuration;

	[SerializeField]
	private bool _startsOn;

	private sbyte lastPickedOn;

	private sbyte lastPickedOff;

	private SeededRandom rand;

	private bool on;

	[SerializeField]
	protected string textTurnOn = "Flick Up";

	[SerializeField]
	protected string textTurnOff = "Flick Down";

	private bool registeredConnectCallback;

	protected int randSeed
	{
		get
		{
			return this._randSeed;
		}
	}

	protected bool startsOn
	{
		get
		{
			return this._startsOn;
		}
		private set
		{
			this._startsOn = value;
		}
	}

	private int StreamSize
	{
		get
		{
			return 17 + (int)this.stylistCTX.Length * 2;
		}
	}

	public LightSwitch()
	{
	}

	public ActivationToggleState ActGetToggleState()
	{
		return (!this.on ? ActivationToggleState.Off : ActivationToggleState.On);
	}

	public ActivationResult ActTrigger(Character instigator, ActivationToggleState toggleTarget, ulong timestamp)
	{
		ActivationToggleState activationToggleState = toggleTarget;
		if (activationToggleState == ActivationToggleState.On)
		{
			if (this.on)
			{
				return ActivationResult.Fail_Redundant;
			}
			this.ServerToggle(timestamp);
			return (!this.on ? ActivationResult.Fail_Busy : ActivationResult.Success);
		}
		if (activationToggleState != ActivationToggleState.Off)
		{
			return ActivationResult.Fail_BadToggle;
		}
		if (!this.on)
		{
			return ActivationResult.Fail_Redundant;
		}
		this.ServerToggle(timestamp);
		return (!this.on ? ActivationResult.Success : ActivationResult.Fail_Busy);
	}

	public ActivationResult ActTrigger(Character instigator, ulong timestamp)
	{
		return this.ActTrigger(instigator, (!this.on ? ActivationToggleState.On : ActivationToggleState.Off), timestamp);
	}

	private void Awake()
	{
		this.rand = new SeededRandom(this.randSeed);
		LightSwitch.MakeCTX(ref this.stylists, ref this.stylistCTX);
		if (this.stylists != null)
		{
			for (int i = 0; i < (int)this.stylists.Length; i++)
			{
				if (this.stylists[i])
				{
					this.stylists[i] = this.stylists[i].ensuredAwake;
				}
			}
		}
	}

	[RPC]
	private void ConnectSetup(byte[] data)
	{
		using (MemoryStream memoryStream = new MemoryStream(data))
		{
			using (BinaryReader binaryReader = new BinaryReader(memoryStream))
			{
				this.Read(binaryReader);
			}
		}
	}

	public ContextExecution ContextQuery(Controllable controllable, ulong timestamp)
	{
		return ContextExecution.Quick;
	}

	public ContextResponse ContextRespondQuick(Controllable controllable, ulong timestamp)
	{
		this.ServerToggle(timestamp);
		return ContextResponse.DoneBreak;
	}

	public string ContextText(Controllable localControllable)
	{
		if (this.on)
		{
			return this.textTurnOff;
		}
		return this.textTurnOn;
	}

	public bool ContextTextPoint(out Vector3 worldPoint)
	{
		worldPoint = new Vector3();
		return false;
	}

	private static void DefaultArray(string test, ref LightStyle[] array)
	{
		if (array == null)
		{
			LightStyle lightStyle = test;
			if (!lightStyle)
			{
				array = new LightStyle[0];
			}
			else
			{
				array = new LightStyle[] { lightStyle };
			}
		}
		else if ((int)array.Length == 0)
		{
			LightStyle lightStyle1 = test;
			if (lightStyle1)
			{
				array = new LightStyle[] { lightStyle1 };
			}
		}
	}

	private void JumpUpdate()
	{
		double num = NetCull.time - this.lastChangeTime;
		if (!this.on)
		{
			int num1 = 0;
			int num2 = (this.randOff != null ? (int)this.randOff.Length : 0);
			while (num1 < (int)this.stylistCTX.Length)
			{
				if (!this.stylists[num1] || (int)this.stylistCTX[num1].lastOffStyle < 0 || (int)this.stylistCTX[num1].lastOffStyle >= num2 || !this.randOff[(int)this.stylistCTX[num1].lastOffStyle])
				{
					Debug.Log(string.Concat("Did not set off ", num1), this);
				}
				else
				{
					this.stylists[num1].Play(this.randOff[(int)this.stylistCTX[num1].lastOffStyle], num);
				}
				num1++;
			}
		}
		else
		{
			int num3 = 0;
			int num4 = (this.randOn != null ? (int)this.randOn.Length : 0);
			while (num3 < (int)this.stylistCTX.Length)
			{
				if (!this.stylists[num3] || (int)this.stylistCTX[num3].lastOnStyle < 0 || (int)this.stylistCTX[num3].lastOnStyle >= num4 || !this.randOn[(int)this.stylistCTX[num3].lastOnStyle])
				{
					Debug.Log(string.Concat("Did not set on ", num3), this);
				}
				else
				{
					this.stylists[num3].Play(this.randOn[(int)this.stylistCTX[num3].lastOnStyle], num);
				}
				num3++;
			}
		}
	}

	private static bool MakeCTX(ref LightStylist[] stylists, ref LightSwitch.StylistCTX[] ctx)
	{
		int num;
		num = (stylists != null ? (int)stylists.Length : 0);
		Array.Resize<LightSwitch.StylistCTX>(ref ctx, num);
		return num > 0;
	}

	private void OnDestroy()
	{
		if (this.registeredConnectCallback)
		{
			GameEvent.PlayerConnected -= new GameEvent.OnPlayerConnectedHandler(this.PlayerConnected);
			this.registeredConnectCallback = false;
		}
	}

	public void PlayerConnected(PlayerClient player)
	{
		byte[] numArray = new byte[this.StreamSize];
		using (MemoryStream memoryStream = new MemoryStream(numArray))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				this.Write(binaryWriter);
			}
		}
		NetCull.RPC<byte[]>(this, "ConnectSetup", player.netPlayer, numArray);
	}

	private void Read(BinaryReader reader)
	{
		this.lastChangeTime = reader.ReadDouble();
		this.on = this.lastChangeTime > 0;
		if (!this.on)
		{
			this.lastChangeTime = -this.lastChangeTime;
		}
		int num = reader.ReadInt32();
		uint num1 = reader.ReadUInt32();
		byte num2 = reader.ReadByte();
		Array.Resize<LightSwitch.StylistCTX>(ref this.stylistCTX, (int)num2);
		Array.Resize<LightStylist>(ref this.stylists, (int)num2);
		for (int i = 0; i < num2; i++)
		{
			this.stylistCTX[i].Read(reader);
		}
		if (num != this.rand.Seed)
		{
			this._randSeed = num;
			this.rand = new SeededRandom(num);
		}
		this.rand.PositionData = num1;
		this.JumpUpdate();
	}

	[RPC]
	protected void ReadState(bool on, uLink.NetworkMessageInfo info)
	{
		this.lastChangeTime = (double)((float)info.timestampInMillis);
		this.on = on;
		if (!on)
		{
			this.TurnOff();
		}
		else
		{
			this.TurnOn();
		}
	}

	private void Reset()
	{
		this._randSeed = UnityEngine.Random.Range(0, 2147483647);
		if (this.stylists == null)
		{
			this.stylists = new LightStylist[0];
		}
		LightSwitch.DefaultArray("on", ref this.randOn);
		LightSwitch.DefaultArray("off", ref this.randOff);
	}

	private void ServerToggle(ulong timestamp)
	{
		this.on = !this.on;
		this.lastChangeTime = (double)((float)timestamp) / 1000;
		if (!this.on)
		{
			this.TurnOff();
		}
		else
		{
			this.TurnOn();
		}
		NetCull.RPC<bool>(this, "ReadState", uLink.RPCMode.Others, this.on);
	}

	private void TurnOff()
	{
		if (this.randOff == null || (int)this.randOff.Length == 0)
		{
			Debug.LogError("Theres no light styles in randOn", this);
		}
		else
		{
			int length = (int)this.randOff.Length;
			for (int i = 0; i < (int)this.stylistCTX.Length; i++)
			{
				this.stylistCTX[i].lastOffStyle = (sbyte)this.rand.RandomIndex(length);
				if (this.stylists[i])
				{
					this.stylists[i].CrossFade(this.randOff[(int)this.stylistCTX[i].lastOffStyle], UnityEngine.Random.Range(this.minOffFadeDuration, this.maxOffFadeDuration));
				}
			}
		}
	}

	private void TurnOn()
	{
		if (this.randOn == null || (int)this.randOn.Length == 0)
		{
			Debug.LogError("Theres no light styles in randOn", this);
		}
		else
		{
			int length = (int)this.randOn.Length;
			for (int i = 0; i < (int)this.stylistCTX.Length; i++)
			{
				this.stylistCTX[i].lastOnStyle = (sbyte)this.rand.RandomIndex(length);
				if (this.stylists[i])
				{
					this.stylists[i].CrossFade(this.randOn[(int)this.stylistCTX[i].lastOnStyle], UnityEngine.Random.Range(this.minOnFadeDuration, this.maxOnFadeDuration));
				}
			}
		}
	}

	private void Write(BinaryWriter writer)
	{
		writer.Write((!this.on ? -this.lastChangeTime : this.lastChangeTime));
		writer.Write(this.rand.Seed);
		writer.Write(this.rand.PositionData);
		writer.Write((byte)((int)this.stylistCTX.Length));
		for (int i = 0; i < (int)this.stylistCTX.Length; i++)
		{
			this.stylistCTX[i].Write(writer);
		}
	}

	private struct StylistCTX
	{
		public const int SIZE = 2;

		public sbyte lastOnStyle;

		public sbyte lastOffStyle;

		public void Read(BinaryReader reader)
		{
			this.lastOnStyle = reader.ReadSByte();
			this.lastOffStyle = reader.ReadSByte();
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(this.lastOnStyle);
			writer.Write(this.lastOffStyle);
		}
	}
}