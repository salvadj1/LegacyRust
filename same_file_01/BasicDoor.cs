using Facepunch;
using System;
using UnityEngine;

[AddComponentMenu("")]
[NGCAutoAddScript]
public abstract class BasicDoor : NetBehaviour, IServerSaveable, IActivatable, IActivatableToggle, IContextRequestable, IContextRequestableMenu, IContextRequestableQuick, IContextRequestableStatus, IContextRequestableText, IContextRequestableSoleAccess, IContextRequestablePointText, IComponentInterface<IActivatable, Facepunch.MonoBehaviour, Activatable>, IComponentInterface<IActivatable, Facepunch.MonoBehaviour>, IComponentInterface<IActivatable>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	private const BasicDoor.RunFlags kRF_StartOpen_Mask = BasicDoor.RunFlags.OpenedForward;

	private const BasicDoor.RunFlags kRF_StartOpen_Value = BasicDoor.RunFlags.OpenedForward;

	private const BasicDoor.RunFlags kRF_DefaultReverse_Mask = BasicDoor.RunFlags.ClosedReverse | BasicDoor.RunFlags.ClosedNoReverse;

	private const BasicDoor.RunFlags kRF_DefaultReverse_Value = BasicDoor.RunFlags.ClosedReverse;

	private const BasicDoor.RunFlags kRF_DisableReverse_Mask = BasicDoor.RunFlags.ClosedNoReverse;

	private const BasicDoor.RunFlags kRF_DisableReverse_Value = BasicDoor.RunFlags.ClosedNoReverse;

	private const BasicDoor.RunFlags kRF_FixedUpdate_Mask = BasicDoor.RunFlags.FixedUpdateClosedForward;

	private const BasicDoor.RunFlags kRF_FixedUpdate_Value = BasicDoor.RunFlags.FixedUpdateClosedForward;

	private const BasicDoor.RunFlags kRF_PointText_Mask = BasicDoor.RunFlags.ClosedForwardWithPointText;

	private const BasicDoor.RunFlags kRF_PointText_Value = BasicDoor.RunFlags.ClosedForwardWithPointText;

	private const BasicDoor.RunFlags kRF_WaitsTarget_Mask = BasicDoor.RunFlags.ClosedForwardWaits;

	private const BasicDoor.RunFlags kRF_WaitsTarget_Value = BasicDoor.RunFlags.ClosedForwardWaits;

	private const float kVolume = 1f;

	private const float kMinDistance = 5f;

	private const float kMaxDistance = 20f;

	private const sbyte kOpenForward = 1;

	private const sbyte kOpenBackward = 2;

	private const sbyte kClose = 0;

	private const string kRPCName_SetOpenOrClosed = "DOo";

	private const string kRPCName_ConnectSetup = "DOc";

	[SerializeField]
	private BasicDoor.RunFlags startConfig;

	[NonSerialized]
	protected Vector3 originalLocalPosition;

	[NonSerialized]
	protected Quaternion originalLocalRotation;

	[NonSerialized]
	protected Vector3 originalLocalScale;

	[NonSerialized]
	private ulong? timeStampChanged;

	[SerializeField]
	protected float durationClose = 1f;

	[SerializeField]
	protected float durationOpen = 1f;

	[NonSerialized]
	private bool capturedOriginals;

	[SerializeField]
	protected string textOpen = "Open";

	[SerializeField]
	protected string textClose = "Close";

	[SerializeField]
	protected Vector3 pointTextPointOpened;

	[SerializeField]
	protected Vector3 pointTextPointClosed;

	[SerializeField]
	protected AudioClip openSound;

	[SerializeField]
	protected AudioClip openedSound;

	[SerializeField]
	protected AudioClip closeSound;

	[SerializeField]
	protected AudioClip closedSound;

	[SerializeField]
	protected float minimumTimeBetweenOpenClose = 1f;

	[NonSerialized]
	private ulong? serverLastTimeStamp;

	[NonSerialized]
	private BasicDoor.State state;

	[NonSerialized]
	private BasicDoor.State target;

	[NonSerialized]
	private bool openingInReverse;

	public bool canOpenReverse
	{
		get
		{
			return !this.reverseOpenDisabled;
		}
		protected set
		{
			this.reverseOpenDisabled = !value;
		}
	}

	public bool defaultReversed
	{
		get
		{
			return (this.startConfig & (BasicDoor.RunFlags.ClosedReverse | BasicDoor.RunFlags.ClosedNoReverse)) == BasicDoor.RunFlags.ClosedReverse;
		}
		protected set
		{
			if (!value)
			{
				BasicDoor basicDoor = this;
				basicDoor.startConfig = basicDoor.startConfig & (BasicDoor.RunFlags.OpenedForward | BasicDoor.RunFlags.ClosedForwardWithPointText | BasicDoor.RunFlags.OpenedForwardWithPointText | BasicDoor.RunFlags.FixedUpdateClosedForward | BasicDoor.RunFlags.FixedUpdateOpenedForward | BasicDoor.RunFlags.FixedUpdateClosedForwardWithPointText | BasicDoor.RunFlags.FixedUpdateOpenedForwardWithPointText | BasicDoor.RunFlags.ClosedNoReverse | BasicDoor.RunFlags.OpenedNoReverse | BasicDoor.RunFlags.ClosedNoReverseWithPointText | BasicDoor.RunFlags.OpenedNoReverseWithPointText | BasicDoor.RunFlags.FixedUpdateClosedNoReverse | BasicDoor.RunFlags.FixedUpdateOpenedNoReverse | BasicDoor.RunFlags.FixedUpdateClosedNoReverseWithPointText | BasicDoor.RunFlags.FixedUpdateOpenedNoReverseWithPointText | BasicDoor.RunFlags.ClosedForwardWaits | BasicDoor.RunFlags.OpenedForwardWaits | BasicDoor.RunFlags.ClosedForwardWaitsWithPointText | BasicDoor.RunFlags.OpenedForwardWaitsWithPointText | BasicDoor.RunFlags.FixedUpdateClosedForwardWaits | BasicDoor.RunFlags.FixedUpdateOpenedForwardWaits | BasicDoor.RunFlags.FixedUpdateClosedForwardWaitsWithPointText | BasicDoor.RunFlags.FixedUpdateOpenedForwardWaitsWithPointText | BasicDoor.RunFlags.ClosedNoReverseWaits | BasicDoor.RunFlags.OpenedNoReverseWaits | BasicDoor.RunFlags.ClosedNoReverseWithPointTextWaits | BasicDoor.RunFlags.OpenedNoReverseWithPointTextWaits | BasicDoor.RunFlags.FixedUpdateClosedNoReverseWaits | BasicDoor.RunFlags.FixedUpdateOpenedNoReverseWaits | BasicDoor.RunFlags.FixedUpdateClosedNoReverseWaitsWithPointText | BasicDoor.RunFlags.FixedUpdateOpenedNoReverseWaitsWithPointText);
			}
			else
			{
				BasicDoor basicDoor1 = this;
				basicDoor1.startConfig = basicDoor1.startConfig | BasicDoor.RunFlags.ClosedReverse;
			}
		}
	}

	protected double elapsed
	{
		get
		{
			if (!this.timeStampChanged.HasValue)
			{
				return Double.PositiveInfinity;
			}
			return (double)((float)(BasicDoor.time - this.timeStampChanged.Value)) / 1000;
		}
	}

	public bool fixedUpdate
	{
		get
		{
			return (this.startConfig & BasicDoor.RunFlags.FixedUpdateClosedForward) == BasicDoor.RunFlags.FixedUpdateClosedForward;
		}
		protected set
		{
			if (!value)
			{
				BasicDoor basicDoor = this;
				basicDoor.startConfig = basicDoor.startConfig & (BasicDoor.RunFlags.OpenedForward | BasicDoor.RunFlags.ClosedReverse | BasicDoor.RunFlags.OpenedReverse | BasicDoor.RunFlags.ClosedForwardWithPointText | BasicDoor.RunFlags.OpenedForwardWithPointText | BasicDoor.RunFlags.ClosedReverseWithPointText | BasicDoor.RunFlags.OpenedReverseWithPointText | BasicDoor.RunFlags.ClosedNoReverse | BasicDoor.RunFlags.OpenedNoReverse | BasicDoor.RunFlags.ClosedNoReverseWithPointText | BasicDoor.RunFlags.OpenedNoReverseWithPointText | BasicDoor.RunFlags.ClosedForwardWaits | BasicDoor.RunFlags.OpenedForwardWaits | BasicDoor.RunFlags.ClosedReverseWaits | BasicDoor.RunFlags.OpenedReverseWaits | BasicDoor.RunFlags.ClosedForwardWaitsWithPointText | BasicDoor.RunFlags.OpenedForwardWaitsWithPointText | BasicDoor.RunFlags.ClosedReverseWaitsWithPointText | BasicDoor.RunFlags.OpenedReverseWaitsWithPointText | BasicDoor.RunFlags.ClosedNoReverseWaits | BasicDoor.RunFlags.OpenedNoReverseWaits | BasicDoor.RunFlags.ClosedNoReverseWithPointTextWaits | BasicDoor.RunFlags.OpenedNoReverseWithPointTextWaits);
			}
			else
			{
				BasicDoor basicDoor1 = this;
				basicDoor1.startConfig = basicDoor1.startConfig | BasicDoor.RunFlags.FixedUpdateClosedForward;
			}
		}
	}

	private bool on
	{
		get
		{
			return (this.target == BasicDoor.State.Opened ? true : this.target == BasicDoor.State.Opening);
		}
	}

	public bool pointText
	{
		get
		{
			return (this.startConfig & BasicDoor.RunFlags.ClosedForwardWithPointText) == BasicDoor.RunFlags.ClosedForwardWithPointText;
		}
		protected set
		{
			if (!value)
			{
				BasicDoor basicDoor = this;
				basicDoor.startConfig = basicDoor.startConfig & (BasicDoor.RunFlags.OpenedForward | BasicDoor.RunFlags.ClosedReverse | BasicDoor.RunFlags.OpenedReverse | BasicDoor.RunFlags.FixedUpdateClosedForward | BasicDoor.RunFlags.FixedUpdateOpenedForward | BasicDoor.RunFlags.FixedUpdateClosedReverse | BasicDoor.RunFlags.FixedUpdateOpenedReverse | BasicDoor.RunFlags.ClosedNoReverse | BasicDoor.RunFlags.OpenedNoReverse | BasicDoor.RunFlags.FixedUpdateClosedNoReverse | BasicDoor.RunFlags.FixedUpdateOpenedNoReverse | BasicDoor.RunFlags.ClosedForwardWaits | BasicDoor.RunFlags.OpenedForwardWaits | BasicDoor.RunFlags.ClosedReverseWaits | BasicDoor.RunFlags.OpenedReverseWaits | BasicDoor.RunFlags.FixedUpdateClosedForwardWaits | BasicDoor.RunFlags.FixedUpdateOpenedForwardWaits | BasicDoor.RunFlags.FixedUpdateClosedReverseWaits | BasicDoor.RunFlags.FixedUpdateOpenedReverseWaits | BasicDoor.RunFlags.ClosedNoReverseWaits | BasicDoor.RunFlags.OpenedNoReverseWaits | BasicDoor.RunFlags.FixedUpdateClosedNoReverseWaits | BasicDoor.RunFlags.FixedUpdateOpenedNoReverseWaits);
			}
			else
			{
				BasicDoor basicDoor1 = this;
				basicDoor1.startConfig = basicDoor1.startConfig | BasicDoor.RunFlags.ClosedForwardWithPointText;
			}
		}
	}

	public bool reverseOpenDisabled
	{
		get
		{
			return (this.startConfig & BasicDoor.RunFlags.ClosedNoReverse) == BasicDoor.RunFlags.ClosedNoReverse;
		}
		protected set
		{
			if (!value)
			{
				BasicDoor basicDoor = this;
				basicDoor.startConfig = basicDoor.startConfig & (BasicDoor.RunFlags.OpenedForward | BasicDoor.RunFlags.ClosedReverse | BasicDoor.RunFlags.OpenedReverse | BasicDoor.RunFlags.ClosedForwardWithPointText | BasicDoor.RunFlags.OpenedForwardWithPointText | BasicDoor.RunFlags.ClosedReverseWithPointText | BasicDoor.RunFlags.OpenedReverseWithPointText | BasicDoor.RunFlags.FixedUpdateClosedForward | BasicDoor.RunFlags.FixedUpdateOpenedForward | BasicDoor.RunFlags.FixedUpdateClosedReverse | BasicDoor.RunFlags.FixedUpdateOpenedReverse | BasicDoor.RunFlags.FixedUpdateClosedForwardWithPointText | BasicDoor.RunFlags.FixedUpdateOpenedForwardWithPointText | BasicDoor.RunFlags.FixedUpdateClosedReverseWithPointText | BasicDoor.RunFlags.FixedUpdateOpenedReverseWithPointText | BasicDoor.RunFlags.ClosedForwardWaits | BasicDoor.RunFlags.OpenedForwardWaits | BasicDoor.RunFlags.ClosedReverseWaits | BasicDoor.RunFlags.OpenedReverseWaits | BasicDoor.RunFlags.ClosedForwardWaitsWithPointText | BasicDoor.RunFlags.OpenedForwardWaitsWithPointText | BasicDoor.RunFlags.ClosedReverseWaitsWithPointText | BasicDoor.RunFlags.OpenedReverseWaitsWithPointText | BasicDoor.RunFlags.FixedUpdateClosedForwardWaits | BasicDoor.RunFlags.FixedUpdateOpenedForwardWaits | BasicDoor.RunFlags.FixedUpdateClosedReverseWaits | BasicDoor.RunFlags.FixedUpdateOpenedReverseWaits | BasicDoor.RunFlags.FixedUpdateClosedForwardWaitsWithPointText | BasicDoor.RunFlags.FixedUpdateOpenedForwardWaitsWithPointText | BasicDoor.RunFlags.FixedUpdateClosedReverseWaitsWithPointText | BasicDoor.RunFlags.FixedUpdateOpenedReverseWaitsWithPointText);
			}
			else
			{
				BasicDoor basicDoor1 = this;
				basicDoor1.startConfig = basicDoor1.startConfig | BasicDoor.RunFlags.ClosedNoReverse;
			}
		}
	}

	public bool startsOpened
	{
		get
		{
			return (this.startConfig & BasicDoor.RunFlags.OpenedForward) == BasicDoor.RunFlags.OpenedForward;
		}
		protected set
		{
			if (!value)
			{
				BasicDoor basicDoor = this;
				basicDoor.startConfig = basicDoor.startConfig & (BasicDoor.RunFlags.ClosedReverse | BasicDoor.RunFlags.ClosedForwardWithPointText | BasicDoor.RunFlags.ClosedReverseWithPointText | BasicDoor.RunFlags.FixedUpdateClosedForward | BasicDoor.RunFlags.FixedUpdateClosedReverse | BasicDoor.RunFlags.FixedUpdateClosedForwardWithPointText | BasicDoor.RunFlags.FixedUpdateClosedReverseWithPointText | BasicDoor.RunFlags.ClosedNoReverse | BasicDoor.RunFlags.ClosedNoReverseWithPointText | BasicDoor.RunFlags.FixedUpdateClosedNoReverse | BasicDoor.RunFlags.FixedUpdateClosedNoReverseWithPointText | BasicDoor.RunFlags.ClosedForwardWaits | BasicDoor.RunFlags.ClosedReverseWaits | BasicDoor.RunFlags.ClosedForwardWaitsWithPointText | BasicDoor.RunFlags.ClosedReverseWaitsWithPointText | BasicDoor.RunFlags.FixedUpdateClosedForwardWaits | BasicDoor.RunFlags.FixedUpdateClosedReverseWaits | BasicDoor.RunFlags.FixedUpdateClosedForwardWaitsWithPointText | BasicDoor.RunFlags.FixedUpdateClosedReverseWaitsWithPointText | BasicDoor.RunFlags.ClosedNoReverseWaits | BasicDoor.RunFlags.ClosedNoReverseWithPointTextWaits | BasicDoor.RunFlags.FixedUpdateClosedNoReverseWaits | BasicDoor.RunFlags.FixedUpdateClosedNoReverseWaitsWithPointText);
			}
			else
			{
				BasicDoor basicDoor1 = this;
				basicDoor1.startConfig = basicDoor1.startConfig | BasicDoor.RunFlags.OpenedForward;
			}
		}
	}

	protected static ulong time
	{
		get
		{
			return NetCull.timeInMillis;
		}
	}

	public bool waitsTarget
	{
		get
		{
			return (this.startConfig & BasicDoor.RunFlags.ClosedForwardWaits) == BasicDoor.RunFlags.ClosedForwardWaits;
		}
		protected set
		{
			if (!value)
			{
				BasicDoor basicDoor = this;
				basicDoor.startConfig = basicDoor.startConfig & (BasicDoor.RunFlags.OpenedForward | BasicDoor.RunFlags.ClosedReverse | BasicDoor.RunFlags.OpenedReverse | BasicDoor.RunFlags.ClosedForwardWithPointText | BasicDoor.RunFlags.OpenedForwardWithPointText | BasicDoor.RunFlags.ClosedReverseWithPointText | BasicDoor.RunFlags.OpenedReverseWithPointText | BasicDoor.RunFlags.FixedUpdateClosedForward | BasicDoor.RunFlags.FixedUpdateOpenedForward | BasicDoor.RunFlags.FixedUpdateClosedReverse | BasicDoor.RunFlags.FixedUpdateOpenedReverse | BasicDoor.RunFlags.FixedUpdateClosedForwardWithPointText | BasicDoor.RunFlags.FixedUpdateOpenedForwardWithPointText | BasicDoor.RunFlags.FixedUpdateClosedReverseWithPointText | BasicDoor.RunFlags.FixedUpdateOpenedReverseWithPointText | BasicDoor.RunFlags.ClosedNoReverse | BasicDoor.RunFlags.OpenedNoReverse | BasicDoor.RunFlags.ClosedNoReverseWithPointText | BasicDoor.RunFlags.OpenedNoReverseWithPointText | BasicDoor.RunFlags.FixedUpdateClosedNoReverse | BasicDoor.RunFlags.FixedUpdateOpenedNoReverse | BasicDoor.RunFlags.FixedUpdateClosedNoReverseWithPointText | BasicDoor.RunFlags.FixedUpdateOpenedNoReverseWithPointText);
			}
			else
			{
				BasicDoor basicDoor1 = this;
				basicDoor1.startConfig = basicDoor1.startConfig | BasicDoor.RunFlags.ClosedForwardWaits;
			}
		}
	}

	protected BasicDoor()
	{
	}

	protected ActivationToggleState ActGetToggleState()
	{
		return (!this.on ? ActivationToggleState.Off : ActivationToggleState.On);
	}

	protected ActivationResult ActTrigger(Character instigator, ActivationToggleState toggleTarget, ulong timestamp)
	{
		ActivationToggleState activationToggleState = toggleTarget;
		if (activationToggleState == ActivationToggleState.On)
		{
			if (this.on)
			{
				return ActivationResult.Fail_Redundant;
			}
			this.ToggleStateServer(timestamp, instigator);
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
		this.ToggleStateServer(timestamp, instigator);
		return (!this.on ? ActivationResult.Success : ActivationResult.Fail_Busy);
	}

	protected void Awake()
	{
		BasicDoor.State state;
		this.CaptureOriginals();
		this.openingInReverse = this.defaultReversed;
		this.InitializeObstacle();
		if (!this.startsOpened)
		{
			int num = 3;
			state = (BasicDoor.State)num;
			this.state = (BasicDoor.State)num;
			this.target = state;
			this.DoDoorFraction(0);
		}
		else
		{
			int num1 = 1;
			state = (BasicDoor.State)num1;
			this.state = (BasicDoor.State)num1;
			this.target = state;
			this.DoDoorFraction(1);
		}
		base.enabled = false;
	}

	private BasicDoor.Side CalculateOpenWay()
	{
		return (this.openingInReverse || !this.canOpenReverse ? BasicDoor.Side.Forward : BasicDoor.Side.Reverse);
	}

	private BasicDoor.Side CalculateOpenWay(Vector3 worldPoint)
	{
		if (this.canOpenReverse)
		{
			BasicDoor.IdealSide idealSide = this.IdealSideForPoint(worldPoint);
			BasicDoor.IdealSide idealSide1 = idealSide;
			if ((int)idealSide != 1)
			{
				if ((int)idealSide1 != 0)
				{
					return BasicDoor.Side.Reverse;
				}
				return (!this.openingInReverse ? BasicDoor.Side.Reverse : BasicDoor.Side.Forward);
			}
		}
		return BasicDoor.Side.Forward;
	}

	private BasicDoor.Side CalculateOpenWay(Vector3? worldPoint)
	{
		return (!worldPoint.HasValue ? this.CalculateOpenWay() : this.CalculateOpenWay(worldPoint.Value));
	}

	private void CaptureOriginals()
	{
		if (!this.capturedOriginals)
		{
			this.originalLocalRotation = base.transform.localRotation;
			this.originalLocalPosition = base.transform.localPosition;
			this.originalLocalScale = base.transform.localScale;
			this.capturedOriginals = true;
		}
	}

	protected ContextStatusFlags ContextStatusPoll()
	{
		switch (this.state)
		{
			case BasicDoor.State.Opened:
			case BasicDoor.State.Closed:
			{
				return 0;
			}
			case BasicDoor.State.Closing:
			{
				return ContextStatusFlags.ObjectBusy | ContextStatusFlags.SpriteFlag0;
			}
			default:
			{
				return ContextStatusFlags.ObjectBusy | ContextStatusFlags.SpriteFlag0;
			}
		}
	}

	protected string ContextText(Controllable localControllable)
	{
		switch (this.state)
		{
			case BasicDoor.State.Opened:
			{
				return this.textClose;
			}
			case BasicDoor.State.Closing:
			{
				return null;
			}
			case BasicDoor.State.Closed:
			{
				return this.textOpen;
			}
			default:
			{
				return null;
			}
		}
	}

	protected bool ContextTextPoint(out Vector3 worldPoint)
	{
		if (this.pointText)
		{
			switch (this.state)
			{
				case BasicDoor.State.Opened:
				{
					worldPoint = base.transform.TransformPoint(this.pointTextPointOpened);
					return true;
				}
				case BasicDoor.State.Closing:
				{
					break;
				}
				case BasicDoor.State.Closed:
				{
					worldPoint = base.transform.TransformPoint(this.pointTextPointClosed);
					return true;
				}
				default:
				{
					goto case BasicDoor.State.Closing;
				}
			}
		}
		worldPoint = new Vector3();
		return false;
	}

	protected void DisableObstacle()
	{
	}

	[RPC]
	protected void DOc(sbyte open)
	{
		long num;
		BasicDoor.State state;
		this.CaptureOriginals();
		if ((int)open == 0)
		{
			int num1 = 3;
			state = (BasicDoor.State)num1;
			this.target = (BasicDoor.State)num1;
			this.state = state;
			num = (long)((double)this.durationOpen * 1000);
			this.DoDoorFraction(0);
		}
		else
		{
			int num2 = 1;
			state = (BasicDoor.State)num2;
			this.target = (BasicDoor.State)num2;
			this.state = state;
			num = (long)((double)this.durationOpen * 1000);
			this.openingInReverse = (int)open == 2;
			this.DoDoorFraction(1);
		}
		ulong num3 = BasicDoor.time;
		if (num <= num3)
		{
			this.timeStampChanged = null;
		}
		else
		{
			this.timeStampChanged = new ulong?(num3 - num);
		}
	}

	protected void DoDoorFraction(double fractionOpen)
	{
		if (!this.openingInReverse)
		{
			this.OnDoorFraction(fractionOpen);
		}
		else
		{
			this.OnDoorFraction(-fractionOpen);
		}
	}

	[RPC]
	protected void DOo(sbyte open, ulong timestamp)
	{
		this.CaptureOriginals();
		if ((int)open != 0)
		{
			this.openingInReverse = (int)open == 2;
		}
		this.StartOpeningOrClosing(open, timestamp);
	}

	private void DoorUpdate()
	{
		double num = this.elapsed;
		if (num <= 0)
		{
			return;
		}
		bool flag = this.state != this.target;
		switch (this.target)
		{
			case BasicDoor.State.Opened:
			{
				if (num < (double)this.durationOpen)
				{
					if (this.state == BasicDoor.State.Closed)
					{
						this.OnDoorStartOpen();
					}
					this.state = BasicDoor.State.Opening;
					this.DoDoorFraction(num / (double)this.durationOpen);
				}
				else
				{
					base.enabled = false;
					this.state = BasicDoor.State.Opened;
					this.DoDoorFraction(1);
					if (flag)
					{
						this.OnDoorEndOpen();
					}
				}
				return;
			}
			case BasicDoor.State.Closing:
			{
				return;
			}
			case BasicDoor.State.Closed:
			{
				if (num < (double)this.durationClose)
				{
					if (this.state == BasicDoor.State.Opened)
					{
						this.OnDoorStartClose();
					}
					this.state = BasicDoor.State.Closing;
					this.DoDoorFraction(1 - num / (double)this.durationClose);
				}
				else
				{
					base.enabled = false;
					this.state = BasicDoor.State.Closed;
					this.DoDoorFraction(0);
					if (flag)
					{
						this.OnDoorEndClose();
					}
				}
				return;
			}
			default:
			{
				return;
			}
		}
	}

	protected void EnableObstacle()
	{
	}

	protected void FixedUpdate()
	{
		if (this.fixedUpdate)
		{
			this.DoorUpdate();
		}
	}

	ActivationResult IActivatable.ActTrigger(Character instigator, ulong timestamp)
	{
		return this.ActTrigger(instigator, (!this.on ? ActivationToggleState.On : ActivationToggleState.Off), timestamp);
	}

	ActivationToggleState IActivatableToggle.ActGetToggleState()
	{
		return this.ActGetToggleState();
	}

	ActivationResult IActivatableToggle.ActTrigger(Character instigator, ActivationToggleState toggleTarget, ulong timestamp)
	{
		return this.ActTrigger(instigator, toggleTarget, timestamp);
	}

	bool IContextRequestablePointText.ContextTextPoint(out Vector3 worldPoint)
	{
		return this.ContextTextPoint(out worldPoint);
	}

	ContextStatusFlags IContextRequestableStatus.ContextStatusPoll()
	{
		return this.ContextStatusPoll();
	}

	string IContextRequestableText.ContextText(Controllable localControllable)
	{
		return this.ContextText(localControllable);
	}

	protected abstract BasicDoor.IdealSide IdealSideForPoint(Vector3 worldPoint);

	private void InitializeObstacle()
	{
		NavMeshObstacle component = base.GetComponent<NavMeshObstacle>();
		if (component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	protected void LateUpdate()
	{
		if (!this.fixedUpdate)
		{
			this.DoorUpdate();
		}
	}

	protected void OnDestroy()
	{
	}

	protected virtual void OnDoorEndClose()
	{
		this.PlaySound(this.closedSound);
		this.EnableObstacle();
	}

	protected void OnDoorEndOpen()
	{
		this.PlaySound(this.openedSound);
		this.DisableObstacle();
	}

	protected abstract void OnDoorFraction(double fractionOpen);

	protected virtual void OnDoorStartClose()
	{
		this.PlaySound(this.closeSound);
	}

	protected void OnDoorStartOpen()
	{
		this.PlaySound(this.openSound);
	}

	protected void PlayerConnected(PlayerClient player)
	{
	}

	private void PlaySound(AudioClip clip)
	{
		if (clip)
		{
			clip.Play(base.transform.position, 1f, 5f, 20f);
		}
	}

	protected void StartOpeningOrClosing(sbyte open, ulong timestamp)
	{
		BasicDoor.State state;
		long num;
		double num1;
		double num2;
		bool flag = this.openingInReverse;
		if ((int)open == 0)
		{
			state = BasicDoor.State.Closed;
			if (state == this.state || (byte)state == (byte)((byte)this.state + (byte)BasicDoor.State.Opened))
			{
				return;
			}
			double num3 = this.elapsed;
			if ((double)this.durationOpen > 0)
			{
				num1 = (num3 < (double)this.durationOpen ? num3 / (double)this.durationOpen : 1);
			}
			else
			{
				num1 = 1;
			}
			double num4 = num1;
			num = (long)((1 - num4) * (double)this.durationClose * 1000);
		}
		else
		{
			if (this.state == BasicDoor.State.Closed)
			{
				flag = (!this.canOpenReverse ? false : (int)open == 2);
			}
			state = BasicDoor.State.Opened;
			if (state == this.state || (byte)state == (byte)((byte)this.state + (byte)BasicDoor.State.Opened))
			{
				return;
			}
			double num5 = this.elapsed;
			if ((double)this.durationClose > 0)
			{
				num2 = (num5 < (double)this.durationClose ? 1 - num5 / (double)this.durationClose : 0);
			}
			else
			{
				num2 = 0;
			}
			num = (long)(num2 * (double)this.durationOpen * 1000);
		}
		if (num <= timestamp)
		{
			this.timeStampChanged = new ulong?(timestamp - num);
		}
		else
		{
			this.timeStampChanged = null;
		}
		base.enabled = true;
		this.openingInReverse = flag;
		this.target = state;
	}

	private bool ToggleStateServer(Vector3? openerPoint, ulong timestamp, bool? fallbackReverse = null)
	{
		if (!this.serverLastTimeStamp.HasValue || timestamp > this.serverLastTimeStamp.Value)
		{
			if (this.waitsTarget && (this.state == BasicDoor.State.Opening || this.state == BasicDoor.State.Closing))
			{
				return false;
			}
			this.serverLastTimeStamp = new ulong?(timestamp);
			BasicDoor.State state = this.target;
			bool flag = this.openingInReverse;
			if (this.target != BasicDoor.State.Closed)
			{
				this.StartOpeningOrClosing(0, timestamp);
			}
			else if (!openerPoint.HasValue && fallbackReverse.HasValue)
			{
				this.StartOpeningOrClosing(((!fallbackReverse.HasValue ? !this.defaultReversed : !fallbackReverse.Value) ? 1 : 2), timestamp);
			}
			else if (this.CalculateOpenWay(openerPoint) != BasicDoor.Side.Forward)
			{
				this.StartOpeningOrClosing(2, timestamp);
			}
			else
			{
				this.StartOpeningOrClosing(1, timestamp);
			}
			if (state != this.target || flag != this.openingInReverse)
			{
				return true;
			}
		}
		return false;
	}

	private bool ToggleStateServer(ulong timestamp, Character instigator)
	{
		if (instigator)
		{
			bool? nullable = null;
			return this.ToggleStateServer(new Vector3?(instigator.eyesOrigin), timestamp, nullable);
		}
		return this.ToggleStateServer(null, timestamp, null);
	}

	protected enum IdealSide : sbyte
	{
		Reverse = -1,
		Unknown = 0,
		Forward = 1
	}

	private enum RunFlags
	{
		ClosedForward = 0,
		OpenedForward = 1,
		ClosedReverse = 2,
		OpenedReverse = 3,
		ClosedForwardWithPointText = 4,
		OpenedForwardWithPointText = 5,
		ClosedReverseWithPointText = 6,
		OpenedReverseWithPointText = 7,
		FixedUpdateClosedForward = 8,
		FixedUpdateOpenedForward = 9,
		FixedUpdateClosedReverse = 10,
		FixedUpdateOpenedReverse = 11,
		FixedUpdateClosedForwardWithPointText = 12,
		FixedUpdateOpenedForwardWithPointText = 13,
		FixedUpdateClosedReverseWithPointText = 14,
		FixedUpdateOpenedReverseWithPointText = 15,
		ClosedNoReverse = 16,
		OpenedNoReverse = 17,
		ClosedNoReverseWithPointText = 20,
		OpenedNoReverseWithPointText = 21,
		FixedUpdateClosedNoReverse = 24,
		FixedUpdateOpenedNoReverse = 25,
		FixedUpdateClosedNoReverseWithPointText = 28,
		FixedUpdateOpenedNoReverseWithPointText = 29,
		ClosedForwardWaits = 32,
		OpenedForwardWaits = 33,
		ClosedReverseWaits = 34,
		OpenedReverseWaits = 35,
		ClosedForwardWaitsWithPointText = 36,
		OpenedForwardWaitsWithPointText = 37,
		ClosedReverseWaitsWithPointText = 38,
		OpenedReverseWaitsWithPointText = 39,
		FixedUpdateClosedForwardWaits = 40,
		FixedUpdateOpenedForwardWaits = 41,
		FixedUpdateClosedReverseWaits = 42,
		FixedUpdateOpenedReverseWaits = 43,
		FixedUpdateClosedForwardWaitsWithPointText = 44,
		FixedUpdateOpenedForwardWaitsWithPointText = 45,
		FixedUpdateClosedReverseWaitsWithPointText = 46,
		FixedUpdateOpenedReverseWaitsWithPointText = 47,
		ClosedNoReverseWaits = 48,
		OpenedNoReverseWaits = 49,
		ClosedNoReverseWithPointTextWaits = 52,
		OpenedNoReverseWithPointTextWaits = 53,
		FixedUpdateClosedNoReverseWaits = 56,
		FixedUpdateOpenedNoReverseWaits = 57,
		FixedUpdateClosedNoReverseWaitsWithPointText = 60,
		FixedUpdateOpenedNoReverseWaitsWithPointText = 61
	}

	private enum Side : byte
	{
		Forward,
		Reverse
	}

	private enum State : byte
	{
		Opening,
		Opened,
		Closing,
		Closed
	}
}