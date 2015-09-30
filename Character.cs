using Facepunch.Prefetch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class Character : IDMain
{
	[PrefetchChildComponent(NameMask="*Eyes")]
	[SerializeField]
	private Transform _eyesTransform;

	private Angle2 _eyesAngles;

	private Vector3 _eyesOffset;

	private Vector3 _initialEyesOffset;

	[PrefetchComponent]
	[SerializeField]
	private Controllable _controllable;

	[PrefetchChildComponent]
	[SerializeField]
	private HitBoxSystem _hitBoxSystem;

	[PrefetchComponent]
	[SerializeField]
	private TakeDamage _takeDamage;

	[PrefetchComponent]
	[SerializeField]
	private RecoilSimulation _recoilSimulation;

	[PrefetchComponent]
	[SerializeField]
	private VisNode _visNode;

	[PrefetchComponent]
	[SerializeField]
	private Crouchable _crouchable;

	[PrefetchComponent]
	[SerializeField]
	private IDLocalCharacterIdleControl _idleControl;

	[NonSerialized]
	private IDLocalCharacterAddon _overlay;

	[NonSerialized]
	private CCMotor _ccmotor;

	[NonSerialized]
	private NavMeshAgent _agent;

	[NonSerialized]
	private CharacterInterpolatorBase _interpolator;

	[SerializeField]
	private string _traitMapName = "Default";

	[NonSerialized]
	private bool _attemptedTraitMapLoad;

	[NonSerialized]
	private bool _traitMapLoaded;

	[NonSerialized]
	private CharacterTraitMap _traitMap;

	[NonSerialized]
	private bool _signaledDeath;

	[NonSerialized]
	public bool lockMovement;

	[NonSerialized]
	public bool lockLook;

	[NonSerialized]
	private bool _eyesSetup;

	[NonSerialized]
	private bool _originSetup;

	[NonSerialized]
	private bool didHitBoxSystemTest;

	[NonSerialized]
	private bool didTakeDamageTest;

	[NonSerialized]
	private bool didControllableTest;

	[NonSerialized]
	private bool didRecoilSimulationTest;

	[NonSerialized]
	private bool didVisNodeTest;

	[NonSerialized]
	private bool didCrouchableTest;

	[NonSerialized]
	private bool didIdleControlTest;

	[SerializeField]
	private float _maxPitch = 89.9f;

	[SerializeField]
	private float _minPitch = -89.9f;

	[NonSerialized]
	private CharacterDeathSignal signals_death;

	[NonSerialized]
	private CharacterStateSignal signals_state;

	[NonSerialized]
	public CharacterStateFlags stateFlags;

	public NavMeshAgent agent
	{
		get
		{
			return this._agent;
		}
	}

	public bool aiControlled
	{
		get
		{
			return (!this.controllable ? false : this._controllable.aiControlled);
		}
	}

	public Controllable aiControlledControllable
	{
		get
		{
			Controllable controllable;
			if (!this.controllable || !this._controllable.aiControlled)
			{
				controllable = null;
			}
			else
			{
				controllable = this._controllable;
			}
			return controllable;
		}
	}

	public Controller aiControlledController
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.aiControlledController;
			}
			return controller;
		}
	}

	public bool alive
	{
		get
		{
			return (!this.takeDamage ? true : this._takeDamage.alive);
		}
	}

	public bool assignedControl
	{
		get
		{
			return (!this.controllable ? false : this._controllable.assignedControl);
		}
	}

	public bool blind
	{
		get
		{
			return (!this.visNode ? true : this._visNode.blind);
		}
		set
		{
			if (this.visNode)
			{
				this._visNode.blind = value;
			}
			else if (!value)
			{
				UnityEngine.Debug.LogError("no visnode", this);
			}
		}
	}

	public CCMotor ccmotor
	{
		get
		{
			return this._ccmotor;
		}
	}

	[Obsolete("this is the character")]
	public Character character
	{
		get
		{
			return this;
		}
	}

	public int controlCount
	{
		get
		{
			return (!this.controllable ? 0 : this._controllable.controlCount);
		}
	}

	public int controlDepth
	{
		get
		{
			return (!this.controllable ? -1 : this._controllable.controlDepth);
		}
	}

	public Controllable controllable
	{
		get
		{
			if (!this.didControllableTest)
			{
				Character.SeekComponentInChildren<Character, Controllable>(this, ref this._controllable);
				this.didControllableTest = true;
			}
			return this._controllable;
		}
	}

	public bool controlled
	{
		get
		{
			return (!this.controllable ? false : this._controllable.controlled);
		}
	}

	public Controllable controlledControllable
	{
		get
		{
			Controllable controllable;
			if (!this.controllable || !this._controllable.controlled)
			{
				controllable = null;
			}
			else
			{
				controllable = this._controllable;
			}
			return controllable;
		}
	}

	public Controller controlledController
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.controlledController;
			}
			return controller;
		}
	}

	public Controller controller
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.controller;
			}
			return controller;
		}
	}

	public string controllerClassName
	{
		get
		{
			string str;
			if (!this.controllable)
			{
				str = null;
			}
			else
			{
				str = this._controllable.controllerClassName;
			}
			return str;
		}
	}

	public bool controlOverridden
	{
		get
		{
			return (!this.controllable ? false : this._controllable.controlOverridden);
		}
	}

	public bool core
	{
		get
		{
			return (!this.controllable ? false : this._controllable.core);
		}
	}

	public Crouchable crouchable
	{
		get
		{
			if (!this.didCrouchableTest)
			{
				Character.SeekIDLocalComponentInChildren<Character, Crouchable>(this, ref this._crouchable);
				this.didCrouchableTest = true;
			}
			return this._crouchable;
		}
	}

	public bool crouched
	{
		get
		{
			return (!this.crouchable ? false : this.crouchable.crouched);
		}
	}

	public bool dead
	{
		get
		{
			return (!this.takeDamage ? false : this._takeDamage.dead);
		}
	}

	public bool deaf
	{
		get
		{
			return (!this.visNode ? true : this._visNode.deaf);
		}
		set
		{
			if (this.visNode)
			{
				this._visNode.deaf = value;
			}
			else if (!value)
			{
				UnityEngine.Debug.LogError("no visnode", this);
			}
		}
	}

	public Angle2 eyesAngles
	{
		get
		{
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			return this._eyesAngles;
		}
		set
		{
			if (this.lockLook)
			{
				return;
			}
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			if (this._eyesAngles.x != value.x || this._eyesAngles.y != value.y)
			{
				this._eyesAngles = value;
				this.InvalidateEyesAngles();
			}
		}
	}

	public Vector3 eyesOffset
	{
		get
		{
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			return this._eyesOffset;
		}
		set
		{
			if (this.lockLook)
			{
				return;
			}
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			if (this._eyesOffset != value)
			{
				this._eyesOffset = value;
				this.InvalidateEyesOffset();
			}
		}
	}

	public Vector3 eyesOrigin
	{
		get
		{
			return this._eyesTransform.position;
		}
	}

	public Vector3 eyesOriginAtInitialOffset
	{
		get
		{
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			return base.transform.TransformPoint(this._initialEyesOffset);
		}
	}

	public float eyesPitch
	{
		get
		{
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			return this._eyesAngles.pitch;
		}
		set
		{
			if (this.lockLook)
			{
				return;
			}
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			if (this._eyesAngles.pitch != value)
			{
				this._eyesAngles.pitch = value;
				this.InvalidateEyesAngles();
			}
		}
	}

	public Ray eyesRay
	{
		get
		{
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			return new Ray(this._eyesTransform.position, this._eyesTransform.forward);
		}
	}

	public Quaternion eyesRotation
	{
		get
		{
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			return this._eyesAngles.quat;
		}
		set
		{
			this.rotation = value;
			Quaternion quaternion = Quaternion.Euler(0f, this._eyesAngles.yaw, 0f);
			Vector3 vector3 = (value * Quaternion.Inverse(quaternion)) * Vector3.forward;
			vector3.Normalize();
			if (vector3.y >= 0f)
			{
				this.eyesPitch = Vector3.Angle(vector3, Vector3.forward);
			}
			else
			{
				this.eyesPitch = -Vector3.Angle(vector3, Vector3.forward);
			}
		}
	}

	public Transform eyesTransformReadOnly
	{
		get
		{
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			return this._eyesTransform;
		}
	}

	public float eyesYaw
	{
		get
		{
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			return this._eyesAngles.yaw;
		}
		set
		{
			if (this.lockLook)
			{
				return;
			}
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			if (this._eyesAngles.yaw != value)
			{
				this._eyesAngles.yaw = value;
				this.InvalidateEyesAngles();
			}
		}
	}

	public Vector3 forward
	{
		get
		{
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			return Quaternion.Euler(0f, this._eyesAngles.yaw, 0f) * Vector3.forward;
		}
	}

	public float health
	{
		get
		{
			return (!this.takeDamage ? Single.PositiveInfinity : this._takeDamage.health);
		}
	}

	public float healthFraction
	{
		get
		{
			return (!this.takeDamage ? 1f : this._takeDamage.healthFraction);
		}
	}

	public float healthLoss
	{
		get
		{
			return (!this.takeDamage ? 0f : this._takeDamage.healthLoss);
		}
	}

	public float healthLossFraction
	{
		get
		{
			return (!this.takeDamage ? 0f : this._takeDamage.healthLossFraction);
		}
	}

	public HitBoxSystem hitBoxSystem
	{
		get
		{
			if (!this.didHitBoxSystemTest)
			{
				Character.SeekIDRemoteComponentInChildren<Character, HitBoxSystem>(this, ref this._hitBoxSystem);
				this.didHitBoxSystemTest = true;
			}
			return this._hitBoxSystem;
		}
	}

	public bool? idle
	{
		get
		{
			if (!this.idleControl)
			{
				return null;
			}
			return new bool?(this._idleControl);
		}
	}

	public IDLocalCharacterIdleControl idleControl
	{
		get
		{
			if (!this.didIdleControlTest)
			{
				Character.SeekIDLocalComponentInChildren<Character, IDLocalCharacterIdleControl>(this, ref this._idleControl);
				this.didIdleControlTest = true;
			}
			return this._idleControl;
		}
	}

	public Vector3 initialEyesOffset
	{
		get
		{
			return this._initialEyesOffset;
		}
	}

	public float initialEyesOffsetX
	{
		get
		{
			return this._initialEyesOffset.x;
		}
	}

	public float initialEyesOffsetY
	{
		get
		{
			return this._initialEyesOffset.y;
		}
	}

	public float initialEyesOffsetZ
	{
		get
		{
			return this._initialEyesOffset.z;
		}
	}

	public CharacterInterpolatorBase interpolator
	{
		get
		{
			return this._interpolator;
		}
	}

	public bool localAIControlled
	{
		get
		{
			return (!this.controllable ? false : this._controllable.localAIControlled);
		}
	}

	public Controllable localAIControlledControllable
	{
		get
		{
			Controllable controllable;
			if (!this.controllable || !this._controllable.localAIControlled)
			{
				controllable = null;
			}
			else
			{
				controllable = this._controllable;
			}
			return controllable;
		}
	}

	public Controller localAIControlledController
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.localAIControlledController;
			}
			return controller;
		}
	}

	public bool localControlled
	{
		get
		{
			return (!this.controllable ? false : this._controllable.localControlled);
		}
	}

	public bool localPlayerControlled
	{
		get
		{
			return (!this.controllable ? false : this._controllable.localPlayerControlled);
		}
	}

	public Controllable localPlayerControlledControllable
	{
		get
		{
			Controllable controllable;
			if (!this.controllable || !this._controllable.localPlayerControlled)
			{
				controllable = null;
			}
			else
			{
				controllable = this._controllable;
			}
			return controllable;
		}
	}

	public Controller localPlayerControlledController
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.localPlayerControlledController;
			}
			return controller;
		}
	}

	public Character masterCharacter
	{
		get
		{
			Character character;
			if (!this.controllable)
			{
				character = null;
			}
			else
			{
				character = this._controllable.masterCharacter;
			}
			return character;
		}
	}

	public Controllable masterControllable
	{
		get
		{
			Controllable controllable;
			if (!this.controllable)
			{
				controllable = null;
			}
			else
			{
				controllable = this._controllable.masterControllable;
			}
			return controllable;
		}
	}

	public Controller masterController
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.masterController;
			}
			return controller;
		}
	}

	public float maxHealth
	{
		get
		{
			return (!this.takeDamage ? Single.PositiveInfinity : this._takeDamage.maxHealth);
		}
	}

	public float maxPitch
	{
		get
		{
			return this._maxPitch;
		}
	}

	public float minPitch
	{
		get
		{
			return this._minPitch;
		}
	}

	public bool mute
	{
		get
		{
			return (!this.visNode ? true : this._visNode.mute);
		}
		set
		{
			if (this.visNode)
			{
				this._visNode.mute = value;
			}
			else if (!value)
			{
				UnityEngine.Debug.LogError("no visnode", this);
			}
		}
	}

	public Character nextCharacter
	{
		get
		{
			Character character;
			if (!this.controllable)
			{
				character = null;
			}
			else
			{
				character = this._controllable.nextCharacter;
			}
			return character;
		}
	}

	public Controllable nextControllable
	{
		get
		{
			Controllable controllable;
			if (!this.controllable)
			{
				controllable = null;
			}
			else
			{
				controllable = this._controllable.nextControllable;
			}
			return controllable;
		}
	}

	public Controller nextController
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.nextController;
			}
			return controller;
		}
	}

	public string npcName
	{
		get
		{
			string str;
			if (!this.controllable)
			{
				str = null;
			}
			else
			{
				str = this._controllable.npcName;
			}
			return str;
		}
	}

	public Vector3 origin
	{
		get
		{
			return base.transform.localPosition;
		}
		set
		{
			if (this.lockMovement)
			{
				return;
			}
			base.transform.localPosition = value;
		}
	}

	public IDLocalCharacterAddon overlay
	{
		get
		{
			return this._overlay;
		}
	}

	public bool overridingControl
	{
		get
		{
			return (!this.controllable ? false : this._controllable.overridingControl);
		}
	}

	public PlayerClient playerClient
	{
		get
		{
			PlayerClient playerClient;
			if (!this._controllable)
			{
				playerClient = null;
			}
			else
			{
				playerClient = this._controllable.playerClient;
			}
			return playerClient;
		}
	}

	public bool playerControlled
	{
		get
		{
			return (!this.controllable ? false : this._controllable.playerControlled);
		}
	}

	public Controllable playerControlledControllable
	{
		get
		{
			Controllable controllable;
			if (!this.controllable || !this._controllable.playerControlled)
			{
				controllable = null;
			}
			else
			{
				controllable = this._controllable;
			}
			return controllable;
		}
	}

	public Controller playerControlledController
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.playerControlledController;
			}
			return controller;
		}
	}

	public static IEnumerable<Character> PlayerCurrentCharacters
	{
		get
		{
			Character.<>c__Iterator1A variable = null;
			return variable;
		}
	}

	public static IEnumerable<Character> PlayerRootCharacters
	{
		get
		{
			Character.<>c__Iterator19 variable = null;
			return variable;
		}
	}

	public Character previousCharacter
	{
		get
		{
			Character character;
			if (!this.controllable)
			{
				character = null;
			}
			else
			{
				character = this._controllable.previousCharacter;
			}
			return character;
		}
	}

	public Controllable previousControllable
	{
		get
		{
			Controllable controllable;
			if (!this.controllable)
			{
				controllable = null;
			}
			else
			{
				controllable = this._controllable.previousControllable;
			}
			return controllable;
		}
	}

	public Controller previousController
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.previousController;
			}
			return controller;
		}
	}

	public RecoilSimulation recoilSimulation
	{
		get
		{
			if (!this.didRecoilSimulationTest)
			{
				Character.SeekIDLocalComponentInChildren<Character, RecoilSimulation>(this, ref this._recoilSimulation);
				this.didRecoilSimulationTest = true;
			}
			return this._recoilSimulation;
		}
	}

	public bool remoteAIControlled
	{
		get
		{
			return (!this.controllable ? false : this._controllable.remoteAIControlled);
		}
	}

	public Controllable remoteAIControlledControllable
	{
		get
		{
			Controllable controllable;
			if (!this.controllable || !this._controllable.remoteAIControlled)
			{
				controllable = null;
			}
			else
			{
				controllable = this._controllable;
			}
			return controllable;
		}
	}

	public Controller remoteAIControlledController
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.remoteAIControlledController;
			}
			return controller;
		}
	}

	public bool remoteControlled
	{
		get
		{
			return (!this.controllable ? false : this._controllable.remoteControlled);
		}
	}

	public bool remotePlayerControlled
	{
		get
		{
			return (!this.controllable ? false : this._controllable.remotePlayerControlled);
		}
	}

	public Controllable remotePlayerControlledControllable
	{
		get
		{
			Controllable controllable;
			if (!this.controllable || !this._controllable.remotePlayerControlled)
			{
				controllable = null;
			}
			else
			{
				controllable = this._controllable;
			}
			return controllable;
		}
	}

	public Controller remotePlayerControlledController
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.remotePlayerControlledController;
			}
			return controller;
		}
	}

	public Vector3 right
	{
		get
		{
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			return Quaternion.Euler(0f, this._eyesAngles.yaw, 0f) * Vector3.right;
		}
	}

	public Character rootCharacter
	{
		get
		{
			Character character;
			if (!this.controllable)
			{
				character = null;
			}
			else
			{
				character = this._controllable.rootCharacter;
			}
			return character;
		}
	}

	public Controllable rootControllable
	{
		get
		{
			Controllable controllable;
			if (!this.controllable)
			{
				controllable = null;
			}
			else
			{
				controllable = this._controllable.rootControllable;
			}
			return controllable;
		}
	}

	public Controller rootController
	{
		get
		{
			Controller controller;
			if (!this.controllable)
			{
				controller = null;
			}
			else
			{
				controller = this._controllable.rootController;
			}
			return controller;
		}
	}

	public Quaternion rotation
	{
		get
		{
			if (!this._eyesSetup)
			{
				this.EyesSetup();
			}
			return Quaternion.Euler(0f, this._eyesAngles.yaw, 0f);
		}
		set
		{
			Vector2 vector2 = new Vector2();
			Vector3 vector3 = value * Vector3.forward;
			vector2.x = vector3.x;
			vector2.y = vector3.z;
			if (Mathf.Approximately(vector2.x, 0f) && Mathf.Approximately(vector2.y, 0f))
			{
				vector3 = value * Vector3.right;
				vector2.x = -vector3.z;
				vector2.y = vector3.x;
				if (Mathf.Approximately(vector2.x, 0f) && Mathf.Approximately(vector2.y, 0f))
				{
					return;
				}
			}
			this.eyesYaw = Mathf.Atan2(-vector2.x, vector2.y) * -57.29578f;
		}
	}

	public bool signaledDeath
	{
		get
		{
			return this._signaledDeath;
		}
	}

	public TakeDamage takeDamage
	{
		get
		{
			if (!this.didTakeDamageTest)
			{
				Character.SeekIDLocalComponentInChildren<Character, TakeDamage>(this, ref this._takeDamage);
				this.didTakeDamageTest = true;
			}
			return this._takeDamage;
		}
	}

	private CharacterTraitMap traitMap
	{
		get
		{
			if (!this._attemptedTraitMapLoad)
			{
				this.LoadTraitMap();
			}
			return this._traitMap;
		}
	}

	private bool traitMapLoaded
	{
		get
		{
			if (!this._attemptedTraitMapLoad)
			{
				this.LoadTraitMap();
			}
			return this._traitMapLoaded;
		}
	}

	public Vis.Mask traitMask
	{
		get
		{
			if (this.visNode)
			{
				return this._visNode.traitMask;
			}
			return new Vis.Mask();
		}
		set
		{
			if (this.visNode)
			{
				this._visNode.traitMask = value;
			}
			else if (value.data != 0)
			{
				UnityEngine.Debug.Log("no visnode", this);
			}
		}
	}

	public Vector3 up
	{
		get
		{
			return Vector3.up;
		}
	}

	public bool vessel
	{
		get
		{
			return (!this.controllable ? false : this._controllable.vessel);
		}
	}

	public Vis.Mask viewMask
	{
		get
		{
			if (this.visNode)
			{
				return this._visNode.viewMask;
			}
			return new Vis.Mask();
		}
		set
		{
			if (this.visNode)
			{
				this._visNode.viewMask = value;
			}
			else if (value.data != 0)
			{
				UnityEngine.Debug.Log("no visnode", this);
			}
		}
	}

	public VisNode visNode
	{
		get
		{
			if (!this.didVisNodeTest)
			{
				Character.SeekIDLocalComponentInChildren<Character, VisNode>(this, ref this._visNode);
				this.didVisNodeTest = true;
			}
			return this._visNode;
		}
	}

	public Character() : this(IDFlags.Character)
	{
	}

	protected Character(IDFlags flags) : base(flags)
	{
	}

	public T AddAddon<T>()
	where T : IDLocalCharacterAddon, new()
	{
		if (!Character.AddonRegistry<T>.valid)
		{
			throw new ArgumentOutOfRangeException("T");
		}
		T local = base.GetLocal<T>();
		if (!local)
		{
			local = base.gameObject.AddComponent<T>();
		}
		return (!this.InitAddon(local) ? (T)null : local);
	}

	public TBase AddAddon<TBase, T>()
	where TBase : IDLocalCharacterAddon
	where T : TBase, new()
	{
		return (TBase)(object)this.AddAddon<T>();
	}

	public IDLocalCharacterAddon AddAddon(Type addonType)
	{
		IDLocalCharacterAddon dLocalCharacterAddon;
		if (!Character.AddonRegistry.Validate(addonType))
		{
			throw new ArgumentOutOfRangeException("addonType", Convert.ToString(addonType));
		}
		IDLocalCharacterAddon component = (IDLocalCharacterAddon)base.GetComponent(addonType);
		if (!component)
		{
			component = (IDLocalCharacterAddon)base.gameObject.AddComponent(addonType);
		}
		if (!this.InitAddon(component))
		{
			dLocalCharacterAddon = null;
		}
		else
		{
			dLocalCharacterAddon = component;
		}
		return dLocalCharacterAddon;
	}

	public TBase AddAddon<TBase>(Type addonType)
	where TBase : IDLocalCharacterAddon
	{
		if (!typeof(TBase).IsAssignableFrom(addonType))
		{
			throw new ArgumentOutOfRangeException("addonType", Convert.ToString(addonType));
		}
		if (!Character.AddonRegistry.Validate(addonType))
		{
			throw new ArgumentOutOfRangeException("addonType", Convert.ToString(addonType));
		}
		TBase component = base.GetComponent<TBase>();
		if (!component)
		{
			component = (TBase)base.gameObject.AddComponent(addonType);
		}
		else if (!addonType.IsAssignableFrom(component.GetType()))
		{
			throw new InvalidOperationException("The existing TBase component was not assignable to addonType");
		}
		return (!this.InitAddon(component) ? (TBase)null : component);
	}

	public IDLocalCharacterAddon AddAddon(Type addonType, Type minimumType)
	{
		if (!minimumType.IsAssignableFrom(addonType))
		{
			throw new ArgumentOutOfRangeException("minimumType", Convert.ToString(addonType));
		}
		return this.AddAddon(addonType);
	}

	public IDLocalCharacterAddon AddAddon(string addonTypeName)
	{
		IDLocalCharacterAddon dLocalCharacterAddon;
		if (!Character.AddonStringRegistry.Validate(addonTypeName))
		{
			throw new ArgumentOutOfRangeException("addonTypeName", addonTypeName);
		}
		IDLocalCharacterAddon component = (IDLocalCharacterAddon)base.GetComponent(addonTypeName);
		if (!component)
		{
			component = (IDLocalCharacterAddon)base.gameObject.AddComponent(addonTypeName);
		}
		if (!this.InitAddon(component))
		{
			dLocalCharacterAddon = null;
		}
		else
		{
			dLocalCharacterAddon = component;
		}
		return dLocalCharacterAddon;
	}

	public IDLocalCharacterAddon AddAddon(string addonTypeName, Type minimumType)
	{
		IDLocalCharacterAddon dLocalCharacterAddon;
		if (!Character.AddonStringRegistry.Validate(addonTypeName, minimumType))
		{
			throw new ArgumentOutOfRangeException("addonTypeName", addonTypeName);
		}
		IDLocalCharacterAddon component = (IDLocalCharacterAddon)base.GetComponent(addonTypeName);
		if (!component)
		{
			component = (IDLocalCharacterAddon)base.gameObject.AddComponent(addonTypeName);
		}
		if (!this.InitAddon(component))
		{
			dLocalCharacterAddon = null;
		}
		else
		{
			dLocalCharacterAddon = component;
		}
		return dLocalCharacterAddon;
	}

	public TBase AddAddon<TBase>(string addonTypeName)
	where TBase : IDLocalCharacterAddon
	{
		Type type;
		if (!Character.AddonStringRegistry.Validate<TBase>(addonTypeName, out type))
		{
			throw new ArgumentOutOfRangeException("TBase", addonTypeName);
		}
		TBase local = base.GetLocal<TBase>();
		if (!local)
		{
			local = (TBase)base.gameObject.AddComponent(addonTypeName);
		}
		else if (!type.IsAssignableFrom(local.GetType()))
		{
			throw new InvalidOperationException("The existing TBase component was not assignable to addonType");
		}
		return (!this.InitAddon(local) ? (TBase)null : local);
	}

	public void AdjustClientSideHealth(float newHealthValue)
	{
		if (this.takeDamage)
		{
			this._takeDamage.health = newHealthValue;
		}
	}

	protected virtual void AlterEyesLocalOrigin(ref Vector3 localPosition)
	{
		if (this.crouchable)
		{
			this._crouchable.ApplyCrouch(ref localPosition);
		}
	}

	public void ApplyAdditiveEyeAngles(Angle2 angles)
	{
		float single = this._eyesAngles.pitch + angles.pitch;
		this.ClampPitch(ref single);
		if (angles.yaw != 0f)
		{
			this._eyesAngles.yaw = Mathf.DeltaAngle(0f, this._eyesAngles.yaw + angles.yaw);
			this._eyesAngles.pitch = single;
			this.InvalidateEyesAngles();
		}
		else if (single != angles.pitch)
		{
			this._eyesAngles.pitch = single;
			this.InvalidateEyesAngles();
		}
	}

	public bool AssignedControlOf(Controllable controllable)
	{
		return (!this.controllable ? false : this._controllable.AssignedControlOf(controllable));
	}

	public bool AssignedControlOf(Controller controller)
	{
		return (!this.controllable ? false : this._controllable.AssignedControlOf(controller));
	}

	public bool AssignedControlOf(IDMain character)
	{
		return (!this.controllable ? false : this._controllable.AssignedControlOf(character));
	}

	public bool AssignedControlOf(IDBase idBase)
	{
		return (!this.controllable ? false : this._controllable.AssignedControlOf(idBase));
	}

	public bool AttentionMessage(string message)
	{
		return VisNode.AttentionMessage(this.visNode, message, null);
	}

	public bool AttentionMessage(string message, object arg)
	{
		return VisNode.AttentionMessage(this.visNode, message, arg);
	}

	public bool AudibleMessage(Vector3 point, float hearRadius, string message, object arg)
	{
		return VisNode.AudibleMessage(this._visNode, point, hearRadius, message, arg);
	}

	public bool AudibleMessage(Vector3 point, float hearRadius, string message)
	{
		return VisNode.AudibleMessage(this.visNode, point, hearRadius, message);
	}

	public bool AudibleMessage(float hearRadius, string message, object arg)
	{
		return VisNode.AudibleMessage(this.visNode, hearRadius, message, arg);
	}

	public bool AudibleMessage(float hearRadius, string message)
	{
		return VisNode.AudibleMessage(this.visNode, hearRadius, message);
	}

	protected void Awake()
	{
		if (!this._originSetup)
		{
			this.OriginSetup();
		}
		if (!this._eyesSetup)
		{
			this.EyesSetup();
		}
	}

	public bool CanSee(VisNode other)
	{
		return (!this.visNode ? false : this._visNode.CanSee(other));
	}

	public bool CanSee(Character other)
	{
		return (!this.visNode || !other || !other.visNode ? false : this._visNode.CanSee(other._visNode));
	}

	public bool CanSee(IDMain other)
	{
		if (other is Character)
		{
			return this.CanSee((Character)other);
		}
		return (!other ? false : this.CanSee(other.GetLocal<VisNode>()));
	}

	public bool CanSee(VisNode other, bool unobstructed)
	{
		return (!unobstructed ? this.CanSee(other) : this.CanSeeUnobstructed(other));
	}

	public bool CanSee(Character other, bool unobstructed)
	{
		return (!unobstructed ? this.CanSee(other) : this.CanSeeUnobstructed(other));
	}

	public bool CanSee(IDMain other, bool unobstructed)
	{
		return (!unobstructed ? this.CanSee(other) : this.CanSeeUnobstructed(other));
	}

	public bool CanSeeUnobstructed(VisNode other)
	{
		return (!this.visNode ? false : this._visNode.CanSeeUnobstructed(other));
	}

	public bool CanSeeUnobstructed(Character other)
	{
		return (!this.visNode || !other || !other.visNode ? false : this._visNode.CanSeeUnobstructed(other._visNode));
	}

	public bool CanSeeUnobstructed(IDMain other)
	{
		if (other is Character)
		{
			return this.CanSeeUnobstructed((Character)other);
		}
		return (!other ? false : this.CanSeeUnobstructed(other.GetLocal<VisNode>()));
	}

	public float ClampPitch(float v)
	{
		float single;
		if (v >= this._minPitch)
		{
			single = (v <= this._maxPitch ? v : this._maxPitch);
		}
		else
		{
			single = this._minPitch;
		}
		return single;
	}

	public void ClampPitch(ref float v)
	{
		if (v < this._minPitch)
		{
			v = this._minPitch;
		}
		else if (v > this._maxPitch)
		{
			v = this._maxPitch;
		}
	}

	public Angle2 ClampPitch(Angle2 v)
	{
		this.ClampPitch(ref v.pitch);
		return v;
	}

	public void ClampPitch(ref Angle2 v)
	{
		this.ClampPitch(ref v.pitch);
	}

	public bool ContactMessage(string message)
	{
		return VisNode.ContactMessage(this.visNode, message, null);
	}

	public bool ContactMessage(string message, object arg)
	{
		return VisNode.ContactMessage(this.visNode, message, arg);
	}

	public bool ControlOverriddenBy(Controllable controllable)
	{
		return (!this.controllable ? false : this._controllable.ControlOverriddenBy(controllable));
	}

	public bool ControlOverriddenBy(Controller controller)
	{
		return (!this.controllable ? false : this._controllable.ControlOverriddenBy(controller));
	}

	public bool ControlOverriddenBy(Character character)
	{
		return (!this.controllable ? false : this._controllable.ControlOverriddenBy(character));
	}

	public bool ControlOverriddenBy(IDMain main)
	{
		return (!this.controllable ? false : this._controllable.ControlOverriddenBy(main));
	}

	public bool ControlOverriddenBy(IDBase idBase)
	{
		return (!this.controllable ? false : this._controllable.ControlOverriddenBy(idBase));
	}

	public bool ControlOverriddenBy(IDLocalCharacter idLocal)
	{
		return (!this.controllable ? false : this._controllable.ControlOverriddenBy(idLocal));
	}

	public bool CreateCCMotor()
	{
		if (this._ccmotor)
		{
			return true;
		}
		CharacterCCMotorTrait trait = this.GetTrait<CharacterCCMotorTrait>();
		CCTotemPole cCTotemPole = (CCTotemPole)UnityEngine.Object.Instantiate(trait.prefab, this.origin, Quaternion.identity);
		this._ccmotor = cCTotemPole.GetComponent<CCMotor>();
		if (!this._ccmotor)
		{
			this._ccmotor = cCTotemPole.gameObject.AddComponent<CCMotor>();
			if (!this._ccmotor)
			{
				return false;
			}
		}
		this._ccmotor.InitializeSetup(this, cCTotemPole, trait);
		return this._ccmotor;
	}

	public bool CreateInterpolator()
	{
		if (this._interpolator)
		{
			return true;
		}
		CharacterInterpolatorTrait trait = this.GetTrait<CharacterInterpolatorTrait>();
		if (!trait)
		{
			return false;
		}
		this._interpolator = this.AddAddon<CharacterInterpolatorBase>(trait.interpolatorComponentTypeName);
		return this._interpolator;
	}

	public bool CreateNavMeshAgent()
	{
		if (this._agent)
		{
			return true;
		}
		CharacterNavAgentTrait trait = this.GetTrait<CharacterNavAgentTrait>();
		if (!trait)
		{
			return false;
		}
		this._agent = base.GetComponent<NavMeshAgent>();
		if (!this._agent)
		{
			this._agent = base.gameObject.AddComponent<NavMeshAgent>();
		}
		trait.CopyTo(this._agent);
		return true;
	}

	public bool CreateOverlay()
	{
		if (this._overlay)
		{
			return true;
		}
		CharacterOverlayTrait trait = this.GetTrait<CharacterOverlayTrait>();
		if (!trait || string.IsNullOrEmpty(trait.overlayComponentName))
		{
			return false;
		}
		this._overlay = this.AddAddon(trait.overlayComponentName);
		return this._overlay;
	}

	[DebuggerHidden]
	public static IEnumerable<Character> CurrentCharacters(IEnumerable<PlayerClient> playerClients)
	{
		Character.<CurrentCharacters>c__Iterator1C variable = null;
		return variable;
	}

	[DebuggerHidden]
	public static IEnumerable<TCharacter> CurrentCharacters<TCharacter>(IEnumerable<PlayerClient> playerClients)
	where TCharacter : Character
	{
		Character.<CurrentCharacters>c__Iterator1E<TCharacter> variable = null;
		return variable;
	}

	public void DestroyCCMotor()
	{
	}

	public void DestroyInterpolator()
	{
		this.RemoveAddon<CharacterInterpolatorBase>(ref this._interpolator);
	}

	public void DestroyNavMeshAgent()
	{
		UnityEngine.Object.Destroy(this._agent);
		this._agent = null;
	}

	public void DestroyOverlay()
	{
		this.RemoveAddon<IDLocalCharacterAddon>(ref this._overlay);
	}

	protected virtual void DoDestroy()
	{
	}

	private void EyesSetup()
	{
		if (!this._originSetup)
		{
			this.OriginSetup();
		}
		if (this._eyesTransform == null || this._eyesTransform.parent != base.transform)
		{
			UnityEngine.Debug.LogError("eyes Transform is null or it is not a direct child of our transform.", this);
			return;
		}
		Vector3 vector3 = this._eyesTransform.localPosition;
		Vector3 vector31 = vector3;
		this._eyesOffset = vector3;
		this._initialEyesOffset = vector31;
		vector31 = this._eyesTransform.localEulerAngles;
		this._eyesAngles.x = -vector31.x;
		Vector3 vector32 = base.transform.localEulerAngles;
		this._eyesAngles.y = vector32.y;
		this._eyesTransform.localEulerAngles = this._eyesAngles.pitchEulerAngles;
		this._eyesSetup = true;
	}

	public bool GestureMessage(string message)
	{
		return VisNode.GestureMessage(this.visNode, message, null);
	}

	public bool GestureMessage(string message, object arg)
	{
		return VisNode.GestureMessage(this.visNode, message, arg);
	}

	public CharacterTrait GetTrait(Type characterTraitType)
	{
		CharacterTrait trait;
		if (!this.traitMapLoaded)
		{
			trait = null;
		}
		else
		{
			trait = this._traitMap.GetTrait(characterTraitType);
		}
		return trait;
	}

	public TCharacterTrait GetTrait<TCharacterTrait>()
	where TCharacterTrait : CharacterTrait
	{
		return (!this.traitMapLoaded ? (TCharacterTrait)null : this._traitMap.GetTrait<TCharacterTrait>());
	}

	private bool InitAddon(IDLocalCharacterAddon addon)
	{
		byte num = addon.InitializeAddon(this);
		if ((num & 8) == 8)
		{
			return false;
		}
		if ((num & 2) == 2)
		{
			addon.PostInitializeAddon();
		}
		return true;
	}

	protected void InvalidateEyesAngles()
	{
		base.transform.localEulerAngles = this._eyesAngles.yawEulerAngles;
		this._eyesTransform.localEulerAngles = this._eyesAngles.pitchEulerAngles;
	}

	protected internal void InvalidateEyesOffset()
	{
		Vector3 vector3 = this._eyesOffset;
		this.AlterEyesLocalOrigin(ref vector3);
		this._eyesTransform.localPosition = vector3;
	}

	private void LoadTraitMap()
	{
		this._traitMapLoaded = TraitMap<CharacterTrait, CharacterTraitMap>.ByName(this._traitMapName, out this._traitMap);
		this._attemptedTraitMapLoad = true;
	}

	protected void LoadTraitMapNonNetworked()
	{
		if (!this._traitMapLoaded)
		{
			this.LoadTraitMap();
		}
	}

	public bool ObliviousMessage(string message)
	{
		return VisNode.ObliviousMessage(this.visNode, message, null);
	}

	public bool ObliviousMessage(string message, object arg)
	{
		return VisNode.ObliviousMessage(this.visNode, message, arg);
	}

	private new void OnDestroy()
	{
		try
		{
			this.DoDestroy();
		}
		finally
		{
			if (this.signals_state != null)
			{
				this.signals_state = null;
			}
			base.OnDestroy();
		}
	}

	private void OriginSetup()
	{
		this._originSetup = true;
	}

	public bool OverridingControlOf(Controllable controllable)
	{
		return (!this.controllable ? false : this._controllable.OverridingControlOf(controllable));
	}

	public bool OverridingControlOf(Controller controller)
	{
		return (!this.controllable ? false : this._controllable.OverridingControlOf(controller));
	}

	public bool OverridingControlOf(Character character)
	{
		return (!this.controllable ? false : this._controllable.OverridingControlOf(character));
	}

	public bool OverridingControlOf(IDMain main)
	{
		return (!this.controllable ? false : this._controllable.OverridingControlOf(main));
	}

	public bool OverridingControlOf(IDBase idBase)
	{
		return (!this.controllable ? false : this._controllable.OverridingControlOf(idBase));
	}

	public bool OverridingControlOf(IDLocalCharacter idLocal)
	{
		return (!this.controllable ? false : this._controllable.OverridingControlOf(idLocal));
	}

	public bool PreyMessage(string message)
	{
		return VisNode.PreyMessage(this.visNode, message, null);
	}

	public bool PreyMessage(string message, object arg)
	{
		return VisNode.PreyMessage(this.visNode, message, arg);
	}

	public RelativeControl RelativeControlFrom(Controllable controllable)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlFrom(controllable));
	}

	public RelativeControl RelativeControlFrom(Controller controller)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlFrom(controller));
	}

	public RelativeControl RelativeControlFrom(Character character)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlFrom(character));
	}

	public RelativeControl RelativeControlFrom(IDMain main)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlFrom(main));
	}

	public RelativeControl RelativeControlFrom(IDLocalCharacter idLocal)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlFrom(idLocal));
	}

	public RelativeControl RelativeControlFrom(IDBase idBase)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlFrom(idBase));
	}

	public RelativeControl RelativeControlTo(Controllable controllable)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlTo(controllable));
	}

	public RelativeControl RelativeControlTo(Controller controller)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlTo(controller));
	}

	public RelativeControl RelativeControlTo(Character character)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlTo(character));
	}

	public RelativeControl RelativeControlTo(IDMain main)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlTo(main));
	}

	public RelativeControl RelativeControlTo(IDLocalCharacter idLocal)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlTo(idLocal));
	}

	public RelativeControl RelativeControlTo(IDBase idBase)
	{
		return (!this.controllable ? RelativeControl.Incompatible : this._controllable.RelativeControlTo(idBase));
	}

	public void RemoveAddon(IDLocalCharacterAddon addon)
	{
		if (addon)
		{
			addon.RemoveAddon();
		}
	}

	public void RemoveAddon<T>(ref T addon)
	where T : IDLocalCharacterAddon
	{
		this.RemoveAddon(addon);
		addon = (T)null;
	}

	[DebuggerHidden]
	public static IEnumerable<Character> RootCharacters(IEnumerable<PlayerClient> playerClients)
	{
		Character.<RootCharacters>c__Iterator1B variable = null;
		return variable;
	}

	[DebuggerHidden]
	public static IEnumerable<TCharacter> RootCharacters<TCharacter>(IEnumerable<PlayerClient> playerClients)
	where TCharacter : Character
	{
		Character.<RootCharacters>c__Iterator1D<TCharacter> variable = null;
		return variable;
	}

	protected static bool SeekComponentInChildren<M, T>(M main, ref T component)
	where M : IDMain
	where T : Component
	{
		if (component)
		{
			return true;
		}
		component = main.GetComponent<T>();
		return component;
	}

	protected static bool SeekIDLocalComponentInChildren<M, T>(M main, ref T component)
	where M : IDMain
	where T : IDLocal
	{
		if (component)
		{
			if (component.idMain == main)
			{
				return true;
			}
			if (!component.idMain)
			{
				return true;
			}
		}
		component = main.GetComponent<T>();
		if (component)
		{
			if (component.idMain == main)
			{
				return true;
			}
			if (!component.idMain)
			{
				return true;
			}
			T[] components = main.GetComponents<T>();
			if ((int)components.Length <= 1)
			{
				component = (T)null;
				return false;
			}
			T[] tArray = components;
			for (int i = 0; i < (int)tArray.Length; i++)
			{
				T t = tArray[i];
				if (t.idMain == main)
				{
					component = t;
					return true;
				}
			}
			component = (T)null;
		}
		return false;
	}

	protected static bool SeekIDRemoteComponentInChildren<M, T>(M main, ref T component)
	where M : IDMain
	where T : IDRemote
	{
		if (component)
		{
			if (component.idMain == main)
			{
				return true;
			}
			if (!component.idMain)
			{
				return true;
			}
		}
		component = main.GetComponentInChildren<T>();
		if (component)
		{
			if (component.idMain == main)
			{
				return true;
			}
			if (!component.idMain)
			{
				return true;
			}
			T[] componentsInChildren = main.GetComponentsInChildren<T>();
			if ((int)componentsInChildren.Length <= 1)
			{
				component = (T)null;
				return false;
			}
			T[] tArray = componentsInChildren;
			for (int i = 0; i < (int)tArray.Length; i++)
			{
				T t = tArray[i];
				if (t.idMain == main)
				{
					component = t;
					return true;
				}
			}
			component = (T)null;
		}
		return false;
	}

	private void signal_death_now(CharacterDeathSignalReason reason)
	{
	}

	public void Signal_ServerCharacterDeath()
	{
		this.signal_death_now(CharacterDeathSignalReason.Died);
	}

	public void Signal_ServerCharacterDeathReset()
	{
	}

	public void Signal_State_FlagsChanged(bool asFirst)
	{
		if (this.signals_state != null)
		{
			try
			{
				this.signals_state(this, asFirst);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError(exception, this);
			}
		}
	}

	public bool StealthMessage(string message)
	{
		return VisNode.StealthMessage(this.visNode, message, null);
	}

	public bool StealthMessage(string message, object arg)
	{
		return VisNode.StealthMessage(this.visNode, message, arg);
	}

	public event CharacterDeathSignal signal_death
	{
		add
		{
		}
		remove
		{
			if (!this._signaledDeath)
			{
				this.signals_death -= value;
			}
		}
	}

	public event CharacterStateSignal signal_state
	{
		add
		{
			if (!this._signaledDeath)
			{
				this.signals_state += value;
			}
		}
		remove
		{
			if (!this._signaledDeath)
			{
				this.signals_state -= value;
			}
		}
	}

	private static class AddonRegistry
	{
		private readonly static Dictionary<Type, bool> validatedCache;

		static AddonRegistry()
		{
			Character.AddonRegistry.validatedCache = new Dictionary<Type, bool>();
		}

		public static bool Validate(Type type)
		{
			bool flag;
			if (type == null)
			{
				return false;
			}
			if (!Character.AddonRegistry.validatedCache.TryGetValue(type, out flag))
			{
				if (!typeof(IDLocalCharacterAddon).IsAssignableFrom(type))
				{
					UnityEngine.Debug.LogError(string.Format("Type {0} is not a valid IDLocalCharacterAddon type", type));
				}
				else if (type.IsAbstract)
				{
					UnityEngine.Debug.LogError(string.Format("Type {0} is abstract, thus not a valid IDLocalCharacterAddon type", type));
				}
				else if (!Attribute.IsDefined(type, typeof(RequireComponent), false))
				{
					flag = true;
				}
				else
				{
					UnityEngine.Debug.LogWarning(string.Format("Type {0} uses the RequireComponent attribute which could be dangerous with addons", type));
					flag = true;
				}
				Character.AddonRegistry.validatedCache[type] = flag;
			}
			return flag;
		}
	}

	private static class AddonRegistry<T>
	where T : IDLocalCharacterAddon, new()
	{
		public readonly static bool valid;

		static AddonRegistry()
		{
			Character.AddonRegistry<T>.valid = Character.AddonRegistry.Validate(typeof(T));
		}
	}

	private static class AddonStringRegistry
	{
		private readonly static Dictionary<string, Character.AddonStringRegistry.TypePair> validatedCache;

		private readonly static string[] assemblyStrings;

		static AddonStringRegistry()
		{
			Character.AddonStringRegistry.validatedCache = new Dictionary<string, Character.AddonStringRegistry.TypePair>();
			Character.AddonStringRegistry.assemblyStrings = new string[] { ", Assembly-CSharp-firstpass", ", Assembly-CSharp" };
		}

		private static bool Validate(string typeName, out Type type)
		{
			Character.AddonStringRegistry.TypePair typePair;
			if (string.IsNullOrEmpty(typeName))
			{
				type = null;
				return false;
			}
			if (Character.AddonStringRegistry.validatedCache.TryGetValue(typeName, out typePair))
			{
				type = typePair.type;
				return typePair.valid;
			}
			bool flag = (!TypeUtility.TryParse(typeName, out type) ? false : Character.AddonRegistry.Validate(type));
			if (!flag)
			{
				string[] strArrays = Character.AddonStringRegistry.assemblyStrings;
				int num = 0;
				while (num < (int)strArrays.Length)
				{
					if (!TypeUtility.TryParse(string.Concat(typeName, strArrays[num]), out type) || !Character.AddonRegistry.Validate(type))
					{
						num++;
					}
					else
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					type = null;
					UnityEngine.Debug.LogError(string.Format("Couldnt associate string \"{0}\" with any valid addon type", typeName));
				}
			}
			Character.AddonStringRegistry.validatedCache[typeName] = new Character.AddonStringRegistry.TypePair(type, flag);
			return flag;
		}

		public static bool Validate(string typeName)
		{
			Type type;
			return Character.AddonStringRegistry.Validate(typeName, out type);
		}

		public static bool Validate<TBase>(string typeName)
		{
			Type type;
			return (!Character.AddonStringRegistry.Validate(typeName, out type) ? false : typeof(TBase).IsAssignableFrom(type));
		}

		public static bool Validate<TBase>(string typeName, out Type type)
		{
			return (!Character.AddonStringRegistry.Validate(typeName, out type) ? false : typeof(TBase).IsAssignableFrom(type));
		}

		public static bool Validate(string typeName, Type minimumType)
		{
			Type type;
			return (!Character.AddonStringRegistry.Validate(typeName, out type) ? false : minimumType.IsAssignableFrom(type));
		}

		private struct TypePair
		{
			public readonly Type type;

			public readonly bool valid;

			public TypePair(Type type, bool valid)
			{
				this.type = type;
				this.valid = valid;
			}
		}
	}
}