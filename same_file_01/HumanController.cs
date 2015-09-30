using Facepunch;
using Facepunch.Clocks.Counters;
using Facepunch.Cursor;
using System;
using uLink;
using UnityEngine;

public class HumanController : Controller, RagdollTransferInfoProvider
{
	private const string kHeadPath = "RustPlayer_Pelvis/RustPlayer_Spine/RustPlayer_Spine1/RustPlayer_Spine2/RustPlayer_Spine4/RustPlayer_Neck1/RustPlayer_Head1";

	private const long clearOnDisableCharacterStateFlags = 335L;

	private const ushort doNotClearOnDisableCharacterStateFlags = 7856;

	protected const Controller.ControllerFlags kControllerFlags = Controller.ControllerFlags.EnableWhenLocalPlayer | Controller.ControllerFlags.DisableWhenRemotePlayer | Controller.ControllerFlags.DisableWhenRemoteAI | Controller.ControllerFlags.IncompatibleAsLocalAI;

	private const bool stepMotorHere = true;

	[NonSerialized]
	private bool crouch_was_blocked;

	[NonSerialized]
	private Crouchable.Smoothing crouch_smoothing;

	private Vector3 lastFrameVelocity;

	private Vector3 midairStartPos;

	[NonSerialized]
	private ContextProbe contextProbe;

	[NonSerialized]
	private LocalRadiationEffect localRadiation;

	[NonSerialized]
	private CacheRef<Inventory> __inventory;

	[NonSerialized]
	private ClientVitalsSync clientVitalsSync;

	[NonSerialized]
	private DeathTransfer deathTransfer;

	[NonSerialized]
	protected int badPacketCount;

	[NonSerialized]
	private bool firstState = true;

	[NonSerialized]
	private HumanControlConfiguration _controlConfig;

	[NonSerialized]
	private bool _didControlConfigTest;

	[NonSerialized]
	private bool? thatsRightPatWeDontNeedComments;

	[NonSerialized]
	private float sprintInMulTime = 1f;

	[NonSerialized]
	private float crouchInMulTime = 1f;

	[NonSerialized]
	private Vector3 server_last_pos = Vector3.zero;

	[NonSerialized]
	private bool server_was_grounded = true;

	[NonSerialized]
	private float server_next_fall_damage_time;

	[NonSerialized]
	private float magnitudeAir;

	[NonSerialized]
	private bool wasInAir;

	[NonSerialized]
	private bool onceEngaged;

	[NonSerialized]
	private float landingSpeedPenaltyTime = Single.MaxValue;

	[NonSerialized]
	private bool onceClock;

	[NonSerialized]
	private SystemTimestamp clock;

	[NonSerialized]
	private Transform _headBone;

	[NonSerialized]
	private bool sprinting;

	[NonSerialized]
	private bool exitingSprint;

	[NonSerialized]
	private bool crouching;

	[NonSerialized]
	private bool exitingCrouch;

	[NonSerialized]
	private bool wasSprinting;

	[NonSerialized]
	private float sprintTime;

	[NonSerialized]
	private float crouchTime;

	[NonSerialized]
	private PlayerProxyTest proxyTest;

	[NonSerialized]
	private PlayerClient instantiatedPlayerClient;

	private PlayerInventory _inventory
	{
		get
		{
			return this.inventory as PlayerInventory;
		}
	}

	public bool bleeding
	{
		get
		{
			return (!this.clientVitalsSync ? base.stateFlags.bleeding : this.clientVitalsSync.bleeding);
		}
	}

	protected HumanControlConfiguration controlConfig
	{
		get
		{
			if (!this._didControlConfigTest)
			{
				this._controlConfig = base.GetTrait<HumanControlConfiguration>();
				this._didControlConfigTest = true;
			}
			return this._controlConfig;
		}
	}

	private Transform headBone
	{
		get
		{
			if (!this._headBone)
			{
				this._headBone = base.transform.FindChild("RustPlayer_Pelvis/RustPlayer_Spine/RustPlayer_Spine1/RustPlayer_Spine2/RustPlayer_Spine4/RustPlayer_Neck1/RustPlayer_Head1");
				if (!this._headBone)
				{
					this._headBone = base.transform.FindChild("RustPlayer_Pelvis/RustPlayer_Spine/RustPlayer_Spine1/RustPlayer_Spine2/RustPlayer_Spine4/RustPlayer_Neck1/RustPlayer_Head1");
					if (!this._headBone)
					{
						Character character = base.idMain;
						if (!character || !character.eyesTransformReadOnly)
						{
							this._headBone = base.transform;
						}
						else
						{
							this._headBone = character.eyesTransformReadOnly;
						}
					}
				}
			}
			return this._headBone;
		}
	}

	public Inventory inventory
	{
		get
		{
			if (!this.__inventory.cached)
			{
				this.__inventory = base.GetLocal<Inventory>();
			}
			return this.__inventory.@value;
		}
	}

	public InventoryHolder inventoryHolder
	{
		get
		{
			InventoryHolder inventoryHolder;
			Inventory inventory = this.inventory;
			if (!inventory)
			{
				inventoryHolder = null;
			}
			else
			{
				inventoryHolder = inventory.inventoryHolder;
			}
			return inventoryHolder;
		}
	}

	RagdollTransferInfo RagdollTransferInfoProvider.RagdollTransferInfo
	{
		get
		{
			return "RustPlayer_Pelvis/RustPlayer_Spine/RustPlayer_Spine1/RustPlayer_Spine2/RustPlayer_Spine4/RustPlayer_Neck1/RustPlayer_Head1";
		}
	}

	public HumanController() : this(Controller.ControllerFlags.EnableWhenLocalPlayer | Controller.ControllerFlags.DisableWhenRemotePlayer | Controller.ControllerFlags.DisableWhenRemoteAI | Controller.ControllerFlags.IncompatibleAsLocalAI)
	{
	}

	protected HumanController(Controller.ControllerFlags controllerFlags) : base(controllerFlags)
	{
	}

	private void CheckBeltUsage()
	{
		if (UIUnityEvents.shouldBlockButtonInput)
		{
			return;
		}
		if (!base.enabled)
		{
			return;
		}
		if (ConsoleWindow.IsVisible())
		{
			return;
		}
		Inventory inventory = this.inventory;
		if (!inventory)
		{
			return;
		}
		InventoryHolder inventoryHolder = inventory.inventoryHolder;
		if (!inventoryHolder)
		{
			return;
		}
		int num = HumanController.InputSample.PollItemButtons();
		if (num != -1)
		{
			inventoryHolder.BeltUse(num);
		}
	}

	[RPC]
	private void GetClientMove(Vector3 origin, int encoded, ushort stateFlags, uLink.NetworkMessageInfo info)
	{
	}

	protected override void OnControlCease()
	{
		if (base.localControlled)
		{
			CameraMount componentInChildren = base.GetComponentInChildren<CameraMount>();
			if (componentInChildren)
			{
				componentInChildren.open = false;
			}
		}
		base.RemoveAddon<ContextProbe>(ref this.contextProbe);
		base.RemoveAddon<LocalRadiationEffect>(ref this.localRadiation);
		base.enabled = false;
		if (base.localControlled)
		{
			if (this.proxyTest)
			{
				this.proxyTest.treatAsProxy = true;
			}
			if (this._inventory)
			{
				this._inventory.DeactivateItem();
			}
		}
		base.OnControlCease();
	}

	protected override void OnControlEngauge()
	{
		base.OnControlEngauge();
		if (base.localControlled)
		{
			CameraMount componentInChildren = base.GetComponentInChildren<CameraMount>();
			if (componentInChildren)
			{
				componentInChildren.open = true;
			}
			this.contextProbe = (!this.contextProbe ? base.AddAddon<ContextProbe>() : this.contextProbe);
			this.localRadiation = (!this.localRadiation ? base.AddAddon<LocalRadiationEffect>() : this.localRadiation);
			if (!this.onceEngaged)
			{
				this.proxyTest = base.GetComponent<PlayerProxyTest>();
				this.onceEngaged = true;
			}
			else if (this.proxyTest)
			{
				this.proxyTest.treatAsProxy = false;
			}
			base.enabled = true;
		}
	}

	protected override void OnControlEnter()
	{
		base.OnControlEnter();
		if (base.localControlled)
		{
			this.clientVitalsSync = base.AddAddon<ClientVitalsSync>();
			ImageEffectManager.GetInstance<GameFullscreen>().fadeColor = Color.black;
			ImageEffectManager.GetInstance<GameFullscreen>().tintColor = Color.white;
			RPOS.DoFade(2f, 2.5f, Color.clear);
			RPOS.SetCurrentFade(Color.black);
			RPOS.HealthUpdate(base.health);
			RPOS.ObservedPlayer = base.controllable;
		}
	}

	protected override void OnControlExit()
	{
		base.RemoveAddon<ClientVitalsSync>(ref this.clientVitalsSync);
		base.OnControlExit();
	}

	protected void OnDisable()
	{
		if (Application.isPlaying)
		{
			Character character = base.idMain;
			if (character)
			{
				character.stateFlags.flags = (ushort)(character.stateFlags.flags & 7856);
			}
			this.SetLocalOnlyComponentsEnabled(false);
		}
		this.sprinting = false;
		this.exitingSprint = true;
	}

	protected void OnEnable()
	{
		this.SetLocalOnlyComponentsEnabled(true);
		LockCursorManager.IsLocked(true);
		this.onceClock = false;
		this.clock = SystemTimestamp.Restart;
	}

	protected override void OnLocalPlayerPreRender()
	{
		InventoryHolder inventoryHolder = this.inventoryHolder;
		if (inventoryHolder)
		{
			inventoryHolder.InvokeInputItemPreRender();
		}
	}

	private void ProcessInput(ref HumanController.InputSample sample)
	{
		bool flag;
		bool flag1;
		CCMotor.InputFrame movementScale = new CCMotor.InputFrame();
		float single;
		float single1;
		CCMotor cCMotor = base.ccmotor;
		if (!cCMotor)
		{
			flag1 = false;
			flag = true;
		}
		else
		{
			flag = cCMotor.isGrounded;
			flag1 = cCMotor.isSliding;
			if (!flag && !flag1)
			{
				sample.sprint = false;
				sample.crouch = false;
				sample.aim = false;
				sample.info__crouchBlocked = false;
				if (!this.wasInAir)
				{
					this.wasInAir = true;
					this.magnitudeAir = cCMotor.input.moveDirection.magnitude;
					this.midairStartPos = base.transform.position;
				}
				this.lastFrameVelocity = cCMotor.velocity;
			}
			else if (this.wasInAir)
			{
				this.wasInAir = false;
				this.magnitudeAir = 1f;
				this.landingSpeedPenaltyTime = 0f;
				if (base.transform.position.y < this.midairStartPos.y && Mathf.Abs(base.transform.position.y - this.midairStartPos.y) > 2f)
				{
					base.idMain.GetLocal<FallDamage>().SendFallImpact(this.lastFrameVelocity);
				}
				this.lastFrameVelocity = Vector3.zero;
				this.midairStartPos = Vector3.zero;
			}
			bool flag2 = (sample.crouch ? true : sample.info__crouchBlocked);
			movementScale.jump = sample.jump;
			movementScale.moveDirection.x = sample.strafe;
			movementScale.moveDirection.y = 0f;
			movementScale.moveDirection.z = sample.walk;
			movementScale.crouchSpeed = (!sample.crouch ? 1f : -1f);
			if (movementScale.moveDirection == Vector3.zero)
			{
				this.sprinting = false;
				this.exitingSprint = false;
				this.sprintTime = 0f;
				this.crouchTime = (!sample.crouch ? 0f : this.controlConfig.curveCrouchMulSpeedByTime.GetEndTime());
				this.magnitudeAir = 1f;
			}
			else
			{
				float single2 = movementScale.moveDirection.magnitude;
				if (single2 < 1f)
				{
					movementScale.moveDirection = movementScale.moveDirection / single2;
					single2 = single2 * single2;
					movementScale.moveDirection = movementScale.moveDirection * single2;
				}
				else if (single2 > 1f)
				{
					movementScale.moveDirection = movementScale.moveDirection / single2;
				}
				if (HumanController.InputSample.MovementScale < 1f)
				{
					if (HumanController.InputSample.MovementScale <= 0f)
					{
						movementScale.moveDirection = Vector3.zero;
					}
					else
					{
						movementScale.moveDirection = movementScale.moveDirection * HumanController.InputSample.MovementScale;
					}
				}
				Vector3 vector3 = movementScale.moveDirection;
				vector3.x = vector3.x * this.controlConfig.sprintScaleX;
				vector3.z = vector3.z * this.controlConfig.sprintScaleY;
				if (!sample.sprint || flag2 || sample.aim)
				{
					sample.sprint = false;
					single = -Time.deltaTime;
				}
				else
				{
					single = Time.deltaTime * this.sprintInMulTime;
				}
				movementScale.moveDirection = movementScale.moveDirection + (vector3 * this.controlConfig.curveSprintAddSpeedByTime.EvaluateClampedTime(ref this.sprintTime, single));
				single1 = (!flag2 ? -Time.deltaTime : Time.deltaTime * this.crouchInMulTime);
				movementScale.moveDirection = movementScale.moveDirection * this.controlConfig.curveCrouchMulSpeedByTime.EvaluateClampedTime(ref this.crouchTime, single1);
				movementScale.moveDirection = base.transform.TransformDirection(movementScale.moveDirection);
				if (!this.wasInAir)
				{
					movementScale.moveDirection = movementScale.moveDirection * this.controlConfig.curveLandingSpeedPenalty.EvaluateClampedTime(ref this.landingSpeedPenaltyTime, Time.deltaTime);
				}
				else
				{
					float single3 = movementScale.moveDirection.magnitude;
					if (!Mathf.Approximately(single3, this.magnitudeAir))
					{
						movementScale.moveDirection = movementScale.moveDirection / single3;
						movementScale.moveDirection = movementScale.moveDirection * this.magnitudeAir;
					}
				}
			}
			if (DebugInput.GetKey(KeyCode.H))
			{
				movementScale.moveDirection = movementScale.moveDirection * 100f;
			}
			cCMotor.input = movementScale;
			if (cCMotor.stepMode == CCMotor.StepMode.Elsewhere)
			{
				cCMotor.Step();
			}
		}
		Character character = base.idMain;
		Crouchable crouchable = character.crouchable;
		if (character)
		{
			Angle2 angle2 = base.eyesAngles;
			Angle2 angle21 = base.eyesAngles;
			angle2.yaw = Mathf.DeltaAngle(0f, angle21.yaw + sample.yaw);
			angle2.pitch = base.ClampPitch(angle2.pitch + sample.pitch);
			base.eyesAngles = angle2;
			ushort num = character.stateFlags.flags;
			if (crouchable)
			{
				this.crouch_smoothing.AddSeconds((double)Time.deltaTime);
				crouchable.LocalPlayerUpdateCrouchState(cCMotor, ref sample.crouch, ref sample.info__crouchBlocked, ref this.crouch_smoothing);
			}
			int num1 = (!sample.aim ? 0 : 4) | (!sample.sprint ? 0 : 2) | (!sample.attack ? 0 : 8) | (!sample.attack2 ? 0 : 256) | (!sample.crouch ? 0 : 1) | (sample.strafe != 0f || sample.walk != 0f ? 64 : 0) | (!LockCursorManager.IsLocked() ? 128 : 0) | (!flag ? 16 : 0) | (!flag1 ? 0 : 32) | (!this.bleeding ? 0 : 512) | (!sample.lamp ? 0 : 2048) | (!sample.laser ? 0 : 4096) | (!sample.info__crouchBlocked ? 0 : 1024);
			character.stateFlags = num1;
			if (num != num1)
			{
				character.Signal_State_FlagsChanged(false);
			}
		}
		this.crouch_was_blocked = sample.info__crouchBlocked;
		if (sample.inventory)
		{
			RPOS.Toggle();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			RPOS.Hide();
		}
	}

	[RPC]
	private void ReadClientMove(Vector3 origin, int encoded, ushort stateFlags, float timeAfterServerReceived, uLink.NetworkMessageInfo info)
	{
		Angle2 angle2 = new Angle2()
		{
			encoded = encoded
		};
		this.UpdateStateNew(origin, angle2, stateFlags, info.timestamp);
	}

	[Obsolete("Make sure the only thing calling this is Update!")]
	protected void SendToServer()
	{
		Character character = base.idMain;
		int num = character.stateFlags.flags & -24577;
		if (Time.timeScale != 1f)
		{
			num = num | 24576;
		}
		else if (!this.thatsRightPatWeDontNeedComments.HasValue)
		{
			this.thatsRightPatWeDontNeedComments = new bool?((base.playerClient.userName.GetHashCode() & 1) == 1);
		}
		else
		{
			num = num | (!this.thatsRightPatWeDontNeedComments.Value ? 16384 : 8192);
			this.thatsRightPatWeDontNeedComments = new bool?(!this.thatsRightPatWeDontNeedComments.Value);
		}
		Facepunch.NetworkView networkView = base.networkView;
		uLink.NetworkPlayer networkPlayer = uLink.NetworkPlayer.server;
		object[] objArray = new object[] { character.origin, null, null };
		objArray[1] = character.eyesAngles.encoded;
		objArray[2] = (ushort)num;
		networkView.RPC("GetClientMove", networkPlayer, objArray);
	}

	private void SetLocalOnlyComponentsEnabled(bool enable)
	{
		CCMotor component = base.GetComponent<CCMotor>();
		if (component)
		{
			component.enabled = enable;
			CharacterController characterController = base.collider as CharacterController;
			if (characterController)
			{
				characterController.enabled = enable;
			}
		}
		CameraMount componentInChildren = base.GetComponentInChildren<CameraMount>();
		if (componentInChildren)
		{
			componentInChildren.open = enable;
			HeadBob headBob = componentInChildren.GetComponent<HeadBob>();
			if (headBob)
			{
				headBob.enabled = enable;
			}
			LazyCam lazyCam = componentInChildren.GetComponent<LazyCam>();
			if (lazyCam)
			{
				lazyCam.enabled = enable;
			}
		}
		LocalDamageDisplay localDamageDisplay = base.GetComponent<LocalDamageDisplay>();
		if (localDamageDisplay)
		{
			localDamageDisplay.enabled = enable;
		}
	}

	protected void SprintingStarted()
	{
		this.wasSprinting = true;
	}

	protected void SprintingStopped()
	{
		this.wasSprinting = false;
	}

	protected void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		this.instantiatedPlayerClient = base.playerClient;
		if (this.instantiatedPlayerClient)
		{
			base.name = string.Format("{0}{1}", this.instantiatedPlayerClient.name, info.networkView.localPrefab);
		}
		try
		{
			this.deathTransfer = base.AddAddon<DeathTransfer>();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
		}
		if (!base.networkView.isMine)
		{
			if (base.CreateInterpolator())
			{
				base.interpolator.running = true;
			}
			UnityEngine.Object.Destroy(base.GetComponent<LocalDamageDisplay>());
		}
		else
		{
			CameraMount.ClearTemporaryCameraMount();
			UnityEngine.Object.Destroy(base.GetComponent<ApplyCrouch>());
			base.CreateCCMotor();
			base.CreateOverlay();
		}
	}

	protected void Update()
	{
		if (base.dead)
		{
			return;
		}
		try
		{
			this.UpdateInput();
		}
		finally
		{
			if ((!this.onceClock || this.clock.ElapsedSeconds > NetCull.sendInterval) && !base.dead)
			{
				this.onceClock = true;
				this.SendToServer();
				this.clock = SystemTimestamp.Restart;
			}
		}
	}

	protected void UpdateInput()
	{
		bool flag;
		bool flag1;
		InventoryHolder inventoryHolder = this.inventoryHolder;
		PlayerClient.InputFunction(base.gameObject);
		if (!inventoryHolder)
		{
			int num = 1;
			flag1 = (bool)num;
			flag = (bool)num;
		}
		else
		{
			ItemModFlags itemModFlag = inventoryHolder.modFlags;
			flag = (itemModFlag & ItemModFlags.Lamp) == ItemModFlags.Other;
			flag1 = (itemModFlag & ItemModFlags.Laser) == ItemModFlags.Other;
		}
		HumanController.InputSample crouchWasBlocked = HumanController.InputSample.Poll(flag, flag1);
		crouchWasBlocked.info__crouchBlocked = this.crouch_was_blocked;
		bool legInjury = base.GetLocal<FallDamage>().GetLegInjury() > 0f;
		if (legInjury)
		{
			crouchWasBlocked.crouch = true;
			crouchWasBlocked.jump = false;
		}
		if (crouchWasBlocked.walk <= 0f || Mathf.Abs(crouchWasBlocked.strafe) >= 0.05f || crouchWasBlocked.attack2 || this._inventory.isCrafting || legInjury)
		{
			crouchWasBlocked.sprint = false;
		}
		float single = 1f;
		if (this._inventory.isCrafting)
		{
			single = single * 0.5f;
		}
		if (legInjury)
		{
			single = single * 0.5f;
		}
		HumanController.InputSample.MovementScale = single;
		if (!inventoryHolder)
		{
			this.ProcessInput(ref crouchWasBlocked);
		}
		else
		{
			object obj = inventoryHolder.InvokeInputItemPreFrame(ref crouchWasBlocked);
			this.ProcessInput(ref crouchWasBlocked);
			inventoryHolder.InvokeInputItemPostFrame(obj, ref crouchWasBlocked);
		}
		this.CheckBeltUsage();
		if (this.wasSprinting && !crouchWasBlocked.sprint)
		{
			this.SprintingStopped();
		}
		else if (!this.wasSprinting && crouchWasBlocked.sprint)
		{
			this.SprintingStarted();
		}
	}

	private void UpdateStateNew(Vector3 origin, Angle2 eyesAngles, ushort stateFlags, double timestamp)
	{
		CharacterStateInterpolatorData characterStateInterpolatorDatum = new CharacterStateInterpolatorData();
		Character character = base.idMain;
		if (this.firstState)
		{
			this.firstState = false;
			character.origin = origin;
			character.eyesAngles = eyesAngles;
			character.stateFlags.flags = stateFlags;
			return;
		}
		if (!base.networkView.isMine)
		{
			CharacterInterpolatorBase characterInterpolatorBase = base.interpolator;
			if (characterInterpolatorBase)
			{
				IStateInterpolator<CharacterStateInterpolatorData> stateInterpolator = characterInterpolatorBase as IStateInterpolator<CharacterStateInterpolatorData>;
				if (stateInterpolator == null)
				{
					character.stateFlags.flags = stateFlags;
					characterInterpolatorBase.SetGoals(origin, eyesAngles.quat, timestamp);
				}
				else
				{
					characterStateInterpolatorDatum.origin = origin;
					characterStateInterpolatorDatum.state.flags = stateFlags;
					characterStateInterpolatorDatum.eyesAngles = eyesAngles;
					stateInterpolator.SetGoals(ref characterStateInterpolatorDatum, ref timestamp);
				}
			}
		}
		else
		{
			character.origin = origin;
			character.eyesAngles = eyesAngles;
			character.stateFlags.flags = stateFlags;
			CCMotor cCMotor = base.ccmotor;
			if (cCMotor)
			{
				cCMotor.Teleport(origin);
			}
		}
	}

	public struct InputSample
	{
		public const string kButtonAim = "Aim";

		public const string kRawYaw = "Mouse X";

		public const string kRawPitch = "Mouse Y";

		public const string kYaw = "Yaw";

		public const string kPitch = "Pitch";

		public const string kButtonUse = "WorldUse";

		public static float MovementScale;

		public float walk;

		public float strafe;

		public float yaw;

		public float pitch;

		public bool jump;

		public bool crouch;

		public bool sprint;

		public bool aim;

		public bool attack;

		public bool attack2;

		public bool reload;

		public bool inventory;

		public bool lamp;

		public bool laser;

		public bool info__crouchBlocked;

		private static float yawSensitivityJoy;

		private static float pitchSensitivityJoy;

		private readonly static string[] kUseButtons;

		public bool is_sprinting
		{
			get
			{
				return (!this.sprint || this.aim ? false : this.walk != 0f);
			}
		}

		static InputSample()
		{
			HumanController.InputSample.MovementScale = 1f;
			HumanController.InputSample.yawSensitivityJoy = 30f;
			HumanController.InputSample.pitchSensitivityJoy = 30f;
			HumanController.InputSample.kUseButtons = new string[] { "UseItem1", "UseItem2", "UseItem3", "UseItem4", "UseItem5", "UseItem6" };
		}

		public static HumanController.InputSample Poll()
		{
			return HumanController.InputSample.Poll(false, false);
		}

		public static HumanController.InputSample Poll(bool noLamp, bool noLaser)
		{
			HumanController.InputSample inputSample = new HumanController.InputSample();
			if (ConsoleWindow.IsVisible())
			{
				return new HumanController.InputSample();
			}
			if (MainMenu.IsVisible())
			{
				return new HumanController.InputSample();
			}
			if (ChatUI.IsVisible())
			{
				return new HumanController.InputSample();
			}
			if (LockEntry.IsVisible())
			{
				return new HumanController.InputSample();
			}
			if (LockCursorManager.IsLocked(true))
			{
				float single = Time.deltaTime;
				inputSample.info__crouchBlocked = false;
				inputSample.walk = 0f;
				if (GameInput.GetButton("Up").IsDown())
				{
					inputSample.walk = inputSample.walk + 1f;
				}
				if (GameInput.GetButton("Down").IsDown())
				{
					inputSample.walk = inputSample.walk - 1f;
				}
				inputSample.strafe = 0f;
				if (GameInput.GetButton("Right").IsDown())
				{
					inputSample.strafe = inputSample.strafe + 1f;
				}
				if (GameInput.GetButton("Left").IsDown())
				{
					inputSample.strafe = inputSample.strafe - 1f;
				}
				inputSample.yaw = GameInput.mouseDeltaX + HumanController.InputSample.yawSensitivityJoy * Input.GetAxis("Yaw") * single;
				inputSample.pitch = GameInput.mouseDeltaY + HumanController.InputSample.pitchSensitivityJoy * Input.GetAxis("Pitch") * single;
				if (input.flipy)
				{
					inputSample.pitch = inputSample.pitch * -1f;
				}
				inputSample.jump = GameInput.GetButton("Jump").IsDown();
				inputSample.crouch = GameInput.GetButton("Duck").IsDown();
				inputSample.sprint = GameInput.GetButton("Sprint").IsDown();
				inputSample.aim = false;
				inputSample.attack = GameInput.GetButton("Fire").IsDown();
				inputSample.attack2 = GameInput.GetButton("AltFire").IsDown();
				inputSample.reload = GameInput.GetButton("Reload").IsDown();
				inputSample.inventory = GameInput.GetButton("Inventory").IsPressed();
				inputSample.lamp = (!noLamp ? HumanController.InputSample.saved.GetLamp(GameInput.GetButton("Flashlight").IsPressed()) : HumanController.InputSample.saved.lamp);
				inputSample.laser = (!noLaser ? HumanController.InputSample.saved.GetLaser(GameInput.GetButton("Laser").IsPressed()) : HumanController.InputSample.saved.laser);
			}
			else
			{
				inputSample = new HumanController.InputSample();
				if (!UIUnityEvents.shouldBlockButtonInput)
				{
					inputSample.inventory = GameInput.GetButton("Inventory").IsPressed();
				}
				inputSample.lamp = HumanController.InputSample.saved.lamp;
				inputSample.laser = HumanController.InputSample.saved.laser;
			}
			if (GameInput.GetButton("Chat").IsPressed())
			{
				ChatUI.Open();
			}
			return inputSample;
		}

		public static int PollItemButtons()
		{
			if (LockCursorManager.keySubsetEnabled)
			{
				for (int i = 0; i < (int)HumanController.InputSample.kUseButtons.Length; i++)
				{
					if (Input.GetButtonDown(HumanController.InputSample.kUseButtons[i]))
					{
						return i;
					}
				}
			}
			return -1;
		}

		private static class saved
		{
			public static bool lamp;

			public static bool laser;

			static saved()
			{
				HumanController.InputSample.saved.lamp = PlayerPrefs.GetInt("LAMP", 1) != 0;
				HumanController.InputSample.saved.laser = PlayerPrefs.GetInt("LASER", 1) != 0;
			}

			public static bool GetLamp(bool pressed)
			{
				if (pressed)
				{
					HumanController.InputSample.saved.lamp = !HumanController.InputSample.saved.lamp;
					PlayerPrefs.SetInt("LAMP", (!HumanController.InputSample.saved.lamp ? 0 : 1));
				}
				return HumanController.InputSample.saved.lamp;
			}

			public static bool GetLaser(bool pressed)
			{
				if (pressed)
				{
					HumanController.InputSample.saved.laser = !HumanController.InputSample.saved.laser;
					PlayerPrefs.SetInt("LASER", (!HumanController.InputSample.saved.laser ? 0 : 1));
				}
				return HumanController.InputSample.saved.laser;
			}
		}
	}
}