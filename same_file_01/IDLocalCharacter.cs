using System;
using UnityEngine;

public abstract class IDLocalCharacter : IDLocal
{
	public NavMeshAgent agent
	{
		get
		{
			return this.idMain.agent;
		}
	}

	public bool aiControlled
	{
		get
		{
			return this.idMain.aiControlled;
		}
	}

	public Controllable aiControlledControllable
	{
		get
		{
			return this.idMain.aiControlledControllable;
		}
	}

	public Controller aiControlledController
	{
		get
		{
			return this.idMain.aiControlledController;
		}
	}

	public bool alive
	{
		get
		{
			return this.idMain.alive;
		}
	}

	public bool assignedControl
	{
		get
		{
			return this.idMain.assignedControl;
		}
	}

	public bool blind
	{
		get
		{
			return this.idMain.blind;
		}
		set
		{
			this.idMain.blind = value;
		}
	}

	public CCMotor ccmotor
	{
		get
		{
			return this.idMain.ccmotor;
		}
	}

	public Character character
	{
		get
		{
			return (Character)this.idMain;
		}
	}

	public int controlCount
	{
		get
		{
			return this.idMain.controlCount;
		}
	}

	public int controlDepth
	{
		get
		{
			return this.idMain.controlDepth;
		}
	}

	public Controllable controllable
	{
		get
		{
			return this.idMain.controllable;
		}
	}

	public bool controlled
	{
		get
		{
			return this.idMain.controlled;
		}
	}

	public Controllable controlledControllable
	{
		get
		{
			return this.idMain.controlledControllable;
		}
	}

	public Controller controlledController
	{
		get
		{
			return this.idMain.controlledController;
		}
	}

	public Controller controller
	{
		get
		{
			return this.idMain.controller;
		}
	}

	public string controllerClassName
	{
		get
		{
			return this.idMain.controllerClassName;
		}
	}

	public bool controlOverridden
	{
		get
		{
			return this.idMain.controlOverridden;
		}
	}

	public Crouchable crouchable
	{
		get
		{
			return this.idMain.crouchable;
		}
	}

	public bool crouched
	{
		get
		{
			return this.idMain.crouched;
		}
	}

	public bool dead
	{
		get
		{
			return this.idMain.dead;
		}
	}

	public bool deaf
	{
		get
		{
			return this.idMain.deaf;
		}
		set
		{
			this.idMain.deaf = value;
		}
	}

	public Angle2 eyesAngles
	{
		get
		{
			return this.idMain.eyesAngles;
		}
		set
		{
			this.idMain.eyesAngles = value;
		}
	}

	public Vector3 eyesOffset
	{
		get
		{
			return this.idMain.eyesOffset;
		}
		set
		{
			this.idMain.eyesOffset = value;
		}
	}

	public Vector3 eyesOrigin
	{
		get
		{
			return this.idMain.eyesOrigin;
		}
	}

	public Vector3 eyesOriginAtInitialOffset
	{
		get
		{
			return this.idMain.eyesOriginAtInitialOffset;
		}
	}

	public float eyesPitch
	{
		get
		{
			return this.idMain.eyesPitch;
		}
		set
		{
			this.idMain.eyesPitch = value;
		}
	}

	public Ray eyesRay
	{
		get
		{
			return this.idMain.eyesRay;
		}
	}

	public Quaternion eyesRotation
	{
		get
		{
			return this.idMain.eyesRotation;
		}
		set
		{
			this.idMain.eyesRotation = value;
		}
	}

	public Transform eyesTransformReadOnly
	{
		get
		{
			return this.idMain.eyesTransformReadOnly;
		}
	}

	public float eyesYaw
	{
		get
		{
			return this.idMain.eyesYaw;
		}
		set
		{
			this.idMain.eyesYaw = value;
		}
	}

	public Vector3 forward
	{
		get
		{
			return this.idMain.forward;
		}
	}

	public float health
	{
		get
		{
			return this.idMain.health;
		}
	}

	public float healthFraction
	{
		get
		{
			return this.idMain.healthFraction;
		}
	}

	public float healthLoss
	{
		get
		{
			return this.idMain.healthLoss;
		}
	}

	public float healthLossFraction
	{
		get
		{
			return this.idMain.healthLossFraction;
		}
	}

	public HitBoxSystem hitBoxSystem
	{
		get
		{
			return this.idMain.hitBoxSystem;
		}
	}

	public bool? idle
	{
		get
		{
			return this.idMain.idle;
		}
	}

	public IDLocalCharacterIdleControl idleControl
	{
		get
		{
			return this.idMain.idleControl;
		}
	}

	public new Character idMain
	{
		get
		{
			return (Character)this.idMain;
		}
	}

	public Vector3 initialEyesOffset
	{
		get
		{
			return this.idMain.initialEyesOffset;
		}
	}

	public float initialEyesOffsetX
	{
		get
		{
			return this.idMain.initialEyesOffsetX;
		}
	}

	public float initialEyesOffsetY
	{
		get
		{
			return this.idMain.initialEyesOffsetY;
		}
	}

	public float initialEyesOffsetZ
	{
		get
		{
			return this.idMain.initialEyesOffsetZ;
		}
	}

	public CharacterInterpolatorBase interpolator
	{
		get
		{
			return this.idMain.interpolator;
		}
	}

	public bool localAIControlled
	{
		get
		{
			return this.idMain.localAIControlled;
		}
	}

	public Controllable localAIControlledControllable
	{
		get
		{
			return this.idMain.localAIControlledControllable;
		}
	}

	public Controller localAIControlledController
	{
		get
		{
			return this.idMain.localAIControlledController;
		}
	}

	public bool localControlled
	{
		get
		{
			return this.idMain.localControlled;
		}
	}

	public bool localPlayerControlled
	{
		get
		{
			return this.idMain.localPlayerControlled;
		}
	}

	public Controllable localPlayerControlledControllable
	{
		get
		{
			return this.idMain.localPlayerControlledControllable;
		}
	}

	public Controller localPlayerControlledController
	{
		get
		{
			return this.idMain.localPlayerControlledController;
		}
	}

	public bool lockLook
	{
		get
		{
			return this.idMain.lockLook;
		}
		set
		{
			this.idMain.lockLook = value;
		}
	}

	public bool lockMovement
	{
		get
		{
			return this.idMain.lockMovement;
		}
		set
		{
			this.idMain.lockMovement = value;
		}
	}

	public Character masterCharacter
	{
		get
		{
			return this.idMain.masterCharacter;
		}
	}

	public Controllable masterControllable
	{
		get
		{
			return this.idMain.masterControllable;
		}
	}

	public Controller masterController
	{
		get
		{
			return this.idMain.masterController;
		}
	}

	public float maxHealth
	{
		get
		{
			return this.idMain.maxHealth;
		}
	}

	public float maxPitch
	{
		get
		{
			return this.idMain.maxPitch;
		}
	}

	public float minPitch
	{
		get
		{
			return this.idMain.minPitch;
		}
	}

	public bool mute
	{
		get
		{
			return this.idMain.mute;
		}
		set
		{
			this.idMain.mute = value;
		}
	}

	public Character nextCharacter
	{
		get
		{
			return this.idMain.nextCharacter;
		}
	}

	public Controllable nextControllable
	{
		get
		{
			return this.idMain.nextControllable;
		}
	}

	public Controller nextController
	{
		get
		{
			return this.idMain.nextController;
		}
	}

	public string npcName
	{
		get
		{
			return this.idMain.npcName;
		}
	}

	public Vector3 origin
	{
		get
		{
			return this.idMain.origin;
		}
		set
		{
			this.idMain.origin = value;
		}
	}

	public IDLocalCharacterAddon overlay
	{
		get
		{
			return this.idMain.overlay;
		}
	}

	public bool overridingControl
	{
		get
		{
			return this.idMain.overridingControl;
		}
	}

	public PlayerClient playerClient
	{
		get
		{
			return this.idMain.playerClient;
		}
	}

	public bool playerControlled
	{
		get
		{
			return this.idMain.playerControlled;
		}
	}

	public Controllable playerControlledControllable
	{
		get
		{
			return this.idMain.playerControlledControllable;
		}
	}

	public Controller playerControlledController
	{
		get
		{
			return this.idMain.playerControlledController;
		}
	}

	public Character previousCharacter
	{
		get
		{
			return this.idMain.previousCharacter;
		}
	}

	public Controllable previousControllable
	{
		get
		{
			return this.idMain.previousControllable;
		}
	}

	public Controller previousController
	{
		get
		{
			return this.idMain.previousController;
		}
	}

	public RecoilSimulation recoilSimulation
	{
		get
		{
			return this.idMain.recoilSimulation;
		}
	}

	public bool remoteAIControlled
	{
		get
		{
			return this.idMain.remoteAIControlled;
		}
	}

	public Controllable remoteAIControlledControllable
	{
		get
		{
			return this.idMain.remoteAIControlledControllable;
		}
	}

	public Controller remoteAIControlledController
	{
		get
		{
			return this.idMain.remoteAIControlledController;
		}
	}

	public bool remoteControlled
	{
		get
		{
			return this.idMain.remoteControlled;
		}
	}

	public bool remotePlayerControlled
	{
		get
		{
			return this.idMain.remotePlayerControlled;
		}
	}

	public Controllable remotePlayerControlledControllable
	{
		get
		{
			return this.idMain.remotePlayerControlledControllable;
		}
	}

	public Controller remotePlayerControlledController
	{
		get
		{
			return this.idMain.remotePlayerControlledController;
		}
	}

	public Vector3 right
	{
		get
		{
			return this.idMain.right;
		}
	}

	public Character rootCharacter
	{
		get
		{
			return this.idMain.rootCharacter;
		}
	}

	public Controllable rootControllable
	{
		get
		{
			return this.idMain.rootControllable;
		}
	}

	public Controller rootController
	{
		get
		{
			return this.idMain.rootController;
		}
	}

	public Quaternion rotation
	{
		get
		{
			return this.idMain.rotation;
		}
		set
		{
			this.idMain.rotation = value;
		}
	}

	protected RPOSLimitFlags rposLimitFlags
	{
		get
		{
			Controller controller = this.controller;
			return (!controller ? RPOSLimitFlags.KeepOff | RPOSLimitFlags.HideInventory | RPOSLimitFlags.HideContext | RPOSLimitFlags.HideSprites : controller.rposLimitFlags);
		}
		set
		{
			Controller controller = this.controller;
			if (controller)
			{
				controller.rposLimitFlags = value;
			}
		}
	}

	public bool signaledDeath
	{
		get
		{
			return this.idMain.signaledDeath;
		}
	}

	public CharacterStateFlags stateFlags
	{
		get
		{
			return this.idMain.stateFlags;
		}
		set
		{
			this.idMain.stateFlags = value;
		}
	}

	public TakeDamage takeDamage
	{
		get
		{
			return this.idMain.takeDamage;
		}
	}

	public Vis.Mask traitMask
	{
		get
		{
			return this.idMain.traitMask;
		}
		set
		{
			this.idMain.traitMask = value;
		}
	}

	public Vector3 up
	{
		get
		{
			return this.idMain.up;
		}
	}

	public Vis.Mask viewMask
	{
		get
		{
			return this.idMain.viewMask;
		}
		set
		{
			this.idMain.viewMask = value;
		}
	}

	public VisNode visNode
	{
		get
		{
			return this.idMain.visNode;
		}
	}

	protected IDLocalCharacter()
	{
	}

	public T AddAddon<T>()
	where T : IDLocalCharacterAddon, new()
	{
		return this.idMain.AddAddon<T>();
	}

	public TBase AddAddon<TBase, T>()
	where TBase : IDLocalCharacterAddon
	where T : TBase, new()
	{
		return this.idMain.AddAddon<TBase, T>();
	}

	public IDLocalCharacterAddon AddAddon(Type addonType)
	{
		return this.idMain.AddAddon(addonType);
	}

	public IDLocalCharacterAddon AddAddon(Type addonType, Type minimumType)
	{
		return this.idMain.AddAddon(addonType, minimumType);
	}

	public TBase AddAddon<TBase>(Type addonType)
	where TBase : IDLocalCharacterAddon
	{
		return this.idMain.AddAddon<TBase>(addonType);
	}

	public IDLocalCharacterAddon AddAddon(string addonTypeName)
	{
		return this.idMain.AddAddon(addonTypeName);
	}

	public IDLocalCharacterAddon AddAddon(string addonTypeName, Type minimumType)
	{
		return this.idMain.AddAddon(addonTypeName, minimumType);
	}

	public TBase AddAddon<TBase>(string addonTypeName)
	where TBase : IDLocalCharacterAddon
	{
		return this.idMain.AddAddon<TBase>(addonTypeName);
	}

	public void AdjustClientSideHealth(float newHealthValue)
	{
		this.idMain.AdjustClientSideHealth(newHealthValue);
	}

	public void ApplyAdditiveEyeAngles(Angle2 angles)
	{
		this.idMain.ApplyAdditiveEyeAngles(angles);
	}

	public bool AssignedControlOf(Controllable controllable)
	{
		return this.idMain.AssignedControlOf(controllable);
	}

	public bool AssignedControlOf(Controller controller)
	{
		return this.idMain.AssignedControlOf(controller);
	}

	public bool AssignedControlOf(IDMain character)
	{
		return this.idMain.AssignedControlOf(character);
	}

	public bool AssignedControlOf(IDBase idBase)
	{
		return this.idMain.AssignedControlOf(idBase);
	}

	public bool AttentionMessage(string message)
	{
		return this.idMain.AttentionMessage(message);
	}

	public bool AttentionMessage(string message, object arg)
	{
		return this.idMain.AttentionMessage(message, arg);
	}

	public bool AudibleMessage(Vector3 point, float hearRadius, string message, object arg)
	{
		return this.idMain.AudibleMessage(point, hearRadius, message, arg);
	}

	public bool AudibleMessage(Vector3 point, float hearRadius, string message)
	{
		return this.idMain.AudibleMessage(point, hearRadius, message);
	}

	public bool AudibleMessage(float hearRadius, string message, object arg)
	{
		return this.idMain.AudibleMessage(hearRadius, message, arg);
	}

	public bool AudibleMessage(float hearRadius, string message)
	{
		return this.idMain.AudibleMessage(hearRadius, message);
	}

	public bool CanSee(VisNode other)
	{
		return this.idMain.CanSee(other);
	}

	public bool CanSee(Character other)
	{
		return this.idMain.CanSee(other);
	}

	public bool CanSee(IDMain other)
	{
		return this.idMain.CanSee(other);
	}

	public bool CanSee(VisNode other, bool unobstructed)
	{
		return this.idMain.CanSee(other, unobstructed);
	}

	public bool CanSee(Character other, bool unobstructed)
	{
		return this.idMain.CanSee(other, unobstructed);
	}

	public bool CanSee(IDMain other, bool unobstructed)
	{
		return this.idMain.CanSee(other, unobstructed);
	}

	public bool CanSeeUnobstructed(VisNode other)
	{
		return this.idMain.CanSeeUnobstructed(other);
	}

	public bool CanSeeUnobstructed(Character other)
	{
		return this.idMain.CanSeeUnobstructed(other);
	}

	public bool CanSeeUnobstructed(IDMain other)
	{
		return this.idMain.CanSeeUnobstructed(other);
	}

	public float ClampPitch(float v)
	{
		return this.idMain.ClampPitch(v);
	}

	public void ClampPitch(ref float v)
	{
		this.idMain.ClampPitch(ref v);
	}

	public Angle2 ClampPitch(Angle2 v)
	{
		return this.idMain.ClampPitch(v);
	}

	public void ClampPitch(ref Angle2 v)
	{
		this.idMain.ClampPitch(ref v);
	}

	public bool ContactMessage(string message)
	{
		return this.idMain.ContactMessage(message);
	}

	public bool ContactMessage(string message, object arg)
	{
		return this.idMain.ContactMessage(message, arg);
	}

	public bool ControlOverriddenBy(Controllable controllable)
	{
		return this.idMain.ControlOverriddenBy(controllable);
	}

	public bool ControlOverriddenBy(Controller controller)
	{
		return this.idMain.ControlOverriddenBy(controller);
	}

	public bool ControlOverriddenBy(Character character)
	{
		return this.idMain.ControlOverriddenBy(character);
	}

	public bool ControlOverriddenBy(IDMain main)
	{
		return this.idMain.ControlOverriddenBy(main);
	}

	public bool ControlOverriddenBy(IDBase idBase)
	{
		return this.idMain.ControlOverriddenBy(idBase);
	}

	public bool ControlOverriddenBy(IDLocalCharacter idLocal)
	{
		return this.idMain.ControlOverriddenBy(idLocal);
	}

	public bool CreateCCMotor()
	{
		return this.idMain.CreateCCMotor();
	}

	public bool CreateInterpolator()
	{
		return this.idMain.CreateInterpolator();
	}

	public bool CreateNavMeshAgent()
	{
		return this.idMain.CreateNavMeshAgent();
	}

	public bool CreateOverlay()
	{
		return this.idMain.CreateOverlay();
	}

	public bool GestureMessage(string message)
	{
		return this.idMain.GestureMessage(message);
	}

	public bool GestureMessage(string message, object arg)
	{
		return this.idMain.GestureMessage(message, arg);
	}

	public CharacterTrait GetTrait(Type characterTraitType)
	{
		return this.idMain.GetTrait(characterTraitType);
	}

	public TCharacterTrait GetTrait<TCharacterTrait>()
	where TCharacterTrait : CharacterTrait
	{
		return this.idMain.GetTrait<TCharacterTrait>();
	}

	public bool ObliviousMessage(string message)
	{
		return this.idMain.ObliviousMessage(message);
	}

	public bool ObliviousMessage(string message, object arg)
	{
		return this.idMain.ObliviousMessage(message, arg);
	}

	public bool OverridingControlOf(Controllable controllable)
	{
		return this.idMain.OverridingControlOf(controllable);
	}

	public bool OverridingControlOf(Controller controller)
	{
		return this.idMain.OverridingControlOf(controller);
	}

	public bool OverridingControlOf(Character character)
	{
		return this.idMain.OverridingControlOf(character);
	}

	public bool OverridingControlOf(IDMain main)
	{
		return this.idMain.OverridingControlOf(main);
	}

	public bool OverridingControlOf(IDBase idBase)
	{
		return this.idMain.OverridingControlOf(idBase);
	}

	public bool OverridingControlOf(IDLocalCharacter idLocal)
	{
		return this.idMain.OverridingControlOf(idLocal);
	}

	public bool PreyMessage(string message)
	{
		return this.idMain.PreyMessage(message);
	}

	public bool PreyMessage(string message, object arg)
	{
		return this.idMain.PreyMessage(message, arg);
	}

	public RelativeControl RelativeControlFrom(Controllable controllable)
	{
		return this.idMain.RelativeControlFrom(controllable);
	}

	public RelativeControl RelativeControlFrom(Controller controller)
	{
		return this.idMain.RelativeControlFrom(controller);
	}

	public RelativeControl RelativeControlFrom(Character character)
	{
		return this.idMain.RelativeControlFrom(character);
	}

	public RelativeControl RelativeControlFrom(IDMain main)
	{
		return this.idMain.RelativeControlFrom(main);
	}

	public RelativeControl RelativeControlFrom(IDLocalCharacter idLocal)
	{
		return this.idMain.RelativeControlFrom(idLocal);
	}

	public RelativeControl RelativeControlFrom(IDBase idBase)
	{
		return this.idMain.RelativeControlFrom(idBase);
	}

	public RelativeControl RelativeControlTo(Controllable controllable)
	{
		return this.idMain.RelativeControlTo(controllable);
	}

	public RelativeControl RelativeControlTo(Controller controller)
	{
		return this.idMain.RelativeControlTo(controller);
	}

	public RelativeControl RelativeControlTo(Character character)
	{
		return this.idMain.RelativeControlTo(character);
	}

	public RelativeControl RelativeControlTo(IDMain main)
	{
		return this.idMain.RelativeControlTo(main);
	}

	public RelativeControl RelativeControlTo(IDLocalCharacter idLocal)
	{
		return this.idMain.RelativeControlTo(idLocal);
	}

	public RelativeControl RelativeControlTo(IDBase idBase)
	{
		return this.idMain.RelativeControlTo(idBase);
	}

	public void RemoveAddon(IDLocalCharacterAddon addon)
	{
		this.idMain.RemoveAddon(addon);
	}

	public void RemoveAddon<T>(ref T addon)
	where T : IDLocalCharacterAddon
	{
		this.idMain.RemoveAddon<T>(ref addon);
	}

	public bool StealthMessage(string message)
	{
		return this.idMain.StealthMessage(message);
	}

	public bool StealthMessage(string message, object arg)
	{
		return this.idMain.StealthMessage(message, arg);
	}
}