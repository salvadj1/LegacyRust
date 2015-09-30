using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using uLink;
using UnityEngine;

public abstract class Controller : IDLocalCharacterAddon
{
	[NonSerialized]
	private Controllable _controllable;

	[NonSerialized]
	private readonly Controller.ControllerFlags controllerFlags;

	[NonSerialized]
	private RPOSLimitFlags _rposLimitFlags;

	[NonSerialized]
	private bool wasSetup;

	[NonSerialized]
	private bool _forwardsPlayerClientInput;

	[NonSerialized]
	private bool _doesNotSave;

	[NonSerialized]
	protected Controller.Commandment commandment;

	public new bool aiControlled
	{
		get
		{
			return this._controllable.aiControlled;
		}
	}

	public new bool assignedControl
	{
		get
		{
			return this._controllable.assignedControl;
		}
	}

	public new int controlCount
	{
		get
		{
			return this._controllable.controlCount;
		}
	}

	public new int controlDepth
	{
		get
		{
			return this._controllable.controlDepth;
		}
	}

	public new Controllable controllable
	{
		get
		{
			return this._controllable;
		}
	}

	public new bool controlled
	{
		get
		{
			return this._controllable.controlled;
		}
	}

	public new Controllable controlledControllable
	{
		get
		{
			Controllable controllable;
			if (!this._controllable.controlled)
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

	public new Controller controlledController
	{
		get
		{
			Controller controller;
			if (!this._controllable.controlled)
			{
				controller = null;
			}
			else
			{
				controller = this;
			}
			return controller;
		}
	}

	public new Controller controller
	{
		get
		{
			return this;
		}
	}

	public new string controllerClassName
	{
		get
		{
			return this._controllable.controllerClassName;
		}
	}

	public new bool controlOverridden
	{
		get
		{
			return this._controllable.controlOverridden;
		}
	}

	public bool doesNotSave
	{
		get
		{
			return this._doesNotSave;
		}
		protected set
		{
			this._doesNotSave = value;
		}
	}

	public bool forwardsPlayerClientInput
	{
		get
		{
			return this._forwardsPlayerClientInput;
		}
		protected set
		{
			this._forwardsPlayerClientInput = value;
		}
	}

	public new bool localAIControlled
	{
		get
		{
			return this._controllable.localAIControlled;
		}
	}

	public new Controllable localAIControlledControllable
	{
		get
		{
			return this._controllable.localAIControlledControllable;
		}
	}

	public new Controller localAIControlledController
	{
		get
		{
			return this._controllable.localAIControlledController;
		}
	}

	public new bool localControlled
	{
		get
		{
			return this._controllable.localControlled;
		}
	}

	public new bool localPlayerControlled
	{
		get
		{
			return this._controllable.localPlayerControlled;
		}
	}

	public new Controllable localPlayerControlledControllable
	{
		get
		{
			return this._controllable.localPlayerControlledControllable;
		}
	}

	public new Controller localPlayerControlledController
	{
		get
		{
			return this._controllable.localPlayerControlledController;
		}
	}

	public new Character masterCharacter
	{
		get
		{
			return this._controllable.masterCharacter;
		}
	}

	public new Controllable masterControllable
	{
		get
		{
			return this._controllable.masterControllable;
		}
	}

	public new Controller masterController
	{
		get
		{
			return this._controllable.masterController;
		}
	}

	public new Character nextCharacter
	{
		get
		{
			return this._controllable.nextCharacter;
		}
	}

	public new Controllable nextControllable
	{
		get
		{
			return this._controllable.nextControllable;
		}
	}

	public new Controller nextController
	{
		get
		{
			return this._controllable.nextController;
		}
	}

	public new string npcName
	{
		get
		{
			return this._controllable.npcName;
		}
	}

	public new bool overridingControl
	{
		get
		{
			return this._controllable.overridingControl;
		}
	}

	public new PlayerClient playerClient
	{
		get
		{
			return this._controllable.playerClient;
		}
	}

	public new bool playerControlled
	{
		get
		{
			return this._controllable.playerControlled;
		}
	}

	public static IEnumerable<Controller> PlayerCurrentControllers
	{
		get
		{
			Controller.<>c__IteratorF variable = null;
			return variable;
		}
	}

	public static IEnumerable<Controller> PlayerRootControllers
	{
		get
		{
			Controller.<>c__IteratorE variable = null;
			return variable;
		}
	}

	public new Character previousCharacter
	{
		get
		{
			return this._controllable.previousCharacter;
		}
	}

	public new Controllable previousControllable
	{
		get
		{
			return this._controllable.previousControllable;
		}
	}

	public new Controller previousController
	{
		get
		{
			return this._controllable.previousController;
		}
	}

	public new bool remoteAIControlled
	{
		get
		{
			return this._controllable.remoteAIControlled;
		}
	}

	public new Controllable remoteAIControlledControllable
	{
		get
		{
			return this._controllable.remoteAIControlledControllable;
		}
	}

	public new Controller remoteAIControlledController
	{
		get
		{
			return this._controllable.remoteAIControlledController;
		}
	}

	public new bool remoteControlled
	{
		get
		{
			return this._controllable.remoteControlled;
		}
	}

	public new bool remotePlayerControlled
	{
		get
		{
			return this._controllable.remotePlayerControlled;
		}
	}

	public new Controllable remotePlayerControlledControllable
	{
		get
		{
			return this._controllable.remotePlayerControlledControllable;
		}
	}

	public new Controller remotePlayerControlledController
	{
		get
		{
			return this._controllable.remotePlayerControlledController;
		}
	}

	public new Character rootCharacter
	{
		get
		{
			return this._controllable.rootCharacter;
		}
	}

	public new Controllable rootControllable
	{
		get
		{
			return this._controllable.rootControllable;
		}
	}

	public new Controller rootController
	{
		get
		{
			return this._controllable.rootController;
		}
	}

	public new RPOSLimitFlags rposLimitFlags
	{
		get
		{
			return this._rposLimitFlags;
		}
		protected internal set
		{
			this._rposLimitFlags = value;
		}
	}

	protected Controller(Controller.ControllerFlags controllerFlags) : this(controllerFlags, 0)
	{
	}

	protected Controller(Controller.ControllerFlags controllerFlags, IDLocalCharacterAddon.AddonFlags addonFlags) : base(addonFlags)
	{
		this.controllerFlags = controllerFlags;
	}

	public new bool AssignedControlOf(Controllable controllable)
	{
		return this._controllable.AssignedControlOf(controllable);
	}

	public new bool AssignedControlOf(Controller controller)
	{
		return this._controllable.AssignedControlOf(controller);
	}

	public new bool AssignedControlOf(IDMain character)
	{
		return this._controllable.AssignedControlOf(character);
	}

	public new bool AssignedControlOf(IDBase idBase)
	{
		return this._controllable.AssignedControlOf(idBase);
	}

	[Obsolete("Used only by Controllable")]
	internal void ControlCease(int cmd)
	{
		Controller.Commandment commandment = this.commandment;
		this.commandment = new Controller.Commandment(cmd & 32767 | 98304);
		try
		{
			this.OnControlCease();
		}
		finally
		{
			this.commandment = commandment;
		}
	}

	[Obsolete("Used only by Controllable")]
	internal void ControlEngauge(int cmd)
	{
		Controller.Commandment commandment = this.commandment;
		this.commandment = new Controller.Commandment(cmd & 32767 | 65536);
		try
		{
			this.OnControlEngauge();
		}
		finally
		{
			this.commandment = commandment;
		}
	}

	[Obsolete("Used only by Controllable")]
	internal void ControlEnter(int cmd)
	{
		Controller.Commandment commandment = this.commandment;
		this.commandment = new Controller.Commandment(cmd & 32767 | 32768);
		try
		{
			this.OnControlEnter();
		}
		finally
		{
			this.commandment = commandment;
		}
	}

	[Obsolete("Used only by Controllable")]
	internal void ControlExit(int cmd)
	{
		Controller.Commandment commandment = this.commandment;
		this.commandment = new Controller.Commandment(cmd & 32767 | 131072);
		try
		{
			this.OnControlExit();
		}
		finally
		{
			this.commandment = commandment;
		}
	}

	internal void ControllerSetup(Controllable controllable, uLink.NetworkView view, ref uLink.NetworkMessageInfo info)
	{
		bool flag;
		Controller.ControllerFlags controllerFlag;
		Controller.ControllerFlags controllerFlag1;
		if (this.wasSetup)
		{
			throw new InvalidOperationException("Already was setup");
		}
		this.wasSetup = true;
		Controller.ControllerFlags controllerFlag2 = this.controllerFlags & Controller.ControllerFlags.DontMessWithEnabled;
		if (controllerFlag2 == Controller.ControllerFlags.AlwaysSavedAsDisabled)
		{
			flag = false;
			if (base.enabled)
			{
				base.enabled = false;
				UnityEngine.Debug.LogError("this was not saved as enabled", this);
			}
		}
		else if (controllerFlag2 == Controller.ControllerFlags.AlwaysSavedAsEnabled)
		{
			flag = false;
			if (!base.enabled)
			{
				base.enabled = true;
				UnityEngine.Debug.LogError("this was not saved as disabled", this);
			}
		}
		else
		{
			flag = (controllerFlag2 == Controller.ControllerFlags.DontMessWithEnabled ? true : false);
		}
		this._controllable = controllable;
		if (this.playerControlled)
		{
			if (this.localPlayerControlled)
			{
				if ((this.controllerFlags & Controller.ControllerFlags.IncompatibleAsLocalPlayer) == Controller.ControllerFlags.IncompatibleAsLocalPlayer)
				{
					throw new NotSupportedException();
				}
			}
			else if ((this.controllerFlags & Controller.ControllerFlags.IncompatibleAsRemotePlayer) == Controller.ControllerFlags.IncompatibleAsRemotePlayer)
			{
				throw new NotSupportedException();
			}
		}
		else if (this.localAIControlled)
		{
			if ((this.controllerFlags & Controller.ControllerFlags.IncompatibleAsLocalAI) == Controller.ControllerFlags.IncompatibleAsLocalAI)
			{
				throw new NotSupportedException();
			}
		}
		else if ((this.controllerFlags & Controller.ControllerFlags.IncompatibleAsRemoteAI) == Controller.ControllerFlags.IncompatibleAsRemoteAI)
		{
			throw new NotSupportedException();
		}
		this.OnControllerSetup(view, ref info);
		if (!flag)
		{
			if (this.playerControlled)
			{
				if (!this.localPlayerControlled)
				{
					controllerFlag = Controller.ControllerFlags.EnableWhenRemotePlayer;
					controllerFlag1 = Controller.ControllerFlags.DisableWhenRemotePlayer;
				}
				else
				{
					controllerFlag = Controller.ControllerFlags.EnableWhenLocalPlayer;
					controllerFlag1 = Controller.ControllerFlags.DisableWhenLocalPlayer;
				}
			}
			else if (!this.localAIControlled)
			{
				controllerFlag = Controller.ControllerFlags.EnableWhenRemoteAI;
				controllerFlag1 = Controller.ControllerFlags.DisableWhenRemoteAI;
			}
			else
			{
				controllerFlag = Controller.ControllerFlags.EnableWhenLocalAI;
				controllerFlag1 = Controller.ControllerFlags.DisableWhenLocalAI;
			}
			if ((this.controllerFlags & controllerFlag) == controllerFlag)
			{
				if ((this.controllerFlags & controllerFlag1) != controllerFlag1)
				{
					base.enabled = true;
				}
				else
				{
					base.enabled = !base.enabled;
				}
			}
			else if ((this.controllerFlags & controllerFlag1) == controllerFlag1)
			{
				base.enabled = false;
			}
		}
	}

	public new bool ControlOverriddenBy(Controllable controllable)
	{
		return this._controllable.ControlOverriddenBy(controllable);
	}

	public new bool ControlOverriddenBy(Controller controller)
	{
		return this._controllable.ControlOverriddenBy(controller);
	}

	public new bool ControlOverriddenBy(Character character)
	{
		return this._controllable.ControlOverriddenBy(character);
	}

	public new bool ControlOverriddenBy(IDMain main)
	{
		return this._controllable.ControlOverriddenBy(main);
	}

	public new bool ControlOverriddenBy(IDBase idBase)
	{
		return this._controllable.ControlOverriddenBy(idBase);
	}

	public new bool ControlOverriddenBy(IDLocalCharacter idLocal)
	{
		return this._controllable.ControlOverriddenBy(idLocal);
	}

	[DebuggerHidden]
	public static IEnumerable<Controller> CurrentControllers(IEnumerable<PlayerClient> playerClients)
	{
		Controller.<CurrentControllers>c__Iterator11 variable = null;
		return variable;
	}

	[DebuggerHidden]
	public static IEnumerable<TController> CurrentControllers<TController>(IEnumerable<PlayerClient> playerClients)
	where TController : Controller
	{
		Controller.<CurrentControllers>c__Iterator13<TController> variable = null;
		return variable;
	}

	protected virtual void OnControlCease()
	{
	}

	protected virtual void OnControlEngauge()
	{
	}

	protected virtual void OnControlEnter()
	{
	}

	protected virtual void OnControlExit()
	{
	}

	protected virtual void OnControllerSetup(uLink.NetworkView networkView, ref uLink.NetworkMessageInfo info)
	{
	}

	protected virtual void OnLocalPlayerInputFrame()
	{
	}

	protected virtual void OnLocalPlayerPreRender()
	{
	}

	public new bool OverridingControlOf(Controllable controllable)
	{
		return this._controllable.OverridingControlOf(controllable);
	}

	public new bool OverridingControlOf(Controller controller)
	{
		return this._controllable.OverridingControlOf(controller);
	}

	public new bool OverridingControlOf(Character character)
	{
		return this._controllable.OverridingControlOf(character);
	}

	public new bool OverridingControlOf(IDMain main)
	{
		return this._controllable.OverridingControlOf(main);
	}

	public new bool OverridingControlOf(IDBase idBase)
	{
		return this._controllable.OverridingControlOf(idBase);
	}

	public new bool OverridingControlOf(IDLocalCharacter idLocal)
	{
		return this._controllable.OverridingControlOf(idLocal);
	}

	internal void ProcessLocalPlayerInput()
	{
		this.OnLocalPlayerInputFrame();
	}

	internal void ProcessLocalPlayerPreRender()
	{
		this.OnLocalPlayerPreRender();
	}

	public new RelativeControl RelativeControlFrom(Controllable controllable)
	{
		return this._controllable.RelativeControlFrom(controllable);
	}

	public new RelativeControl RelativeControlFrom(Controller controller)
	{
		return this._controllable.RelativeControlFrom(controller);
	}

	public new RelativeControl RelativeControlFrom(Character character)
	{
		return this._controllable.RelativeControlFrom(character);
	}

	public new RelativeControl RelativeControlFrom(IDMain main)
	{
		return this._controllable.RelativeControlFrom(main);
	}

	public new RelativeControl RelativeControlFrom(IDLocalCharacter idLocal)
	{
		return this._controllable.RelativeControlFrom(idLocal);
	}

	public new RelativeControl RelativeControlFrom(IDBase idBase)
	{
		return this._controllable.RelativeControlFrom(idBase);
	}

	public new RelativeControl RelativeControlTo(Controllable controllable)
	{
		return this._controllable.RelativeControlTo(controllable);
	}

	public new RelativeControl RelativeControlTo(Controller controller)
	{
		return this._controllable.RelativeControlTo(controller);
	}

	public new RelativeControl RelativeControlTo(Character character)
	{
		return this._controllable.RelativeControlTo(character);
	}

	public new RelativeControl RelativeControlTo(IDMain main)
	{
		return this._controllable.RelativeControlTo(main);
	}

	public new RelativeControl RelativeControlTo(IDLocalCharacter idLocal)
	{
		return this._controllable.RelativeControlTo(idLocal);
	}

	public new RelativeControl RelativeControlTo(IDBase idBase)
	{
		return this._controllable.RelativeControlTo(idBase);
	}

	[DebuggerHidden]
	public static IEnumerable<Controller> RootControllers(IEnumerable<PlayerClient> playerClients)
	{
		Controller.<RootControllers>c__Iterator10 variable = null;
		return variable;
	}

	[DebuggerHidden]
	public static IEnumerable<TController> RootControllers<TController>(IEnumerable<PlayerClient> playerClients)
	where TController : Controller
	{
		Controller.<RootControllers>c__Iterator12<TController> variable = null;
		return variable;
	}

	protected internal struct Commandment
	{
		private const int B = 1;

		internal const int THIS_TO_BASE = 1;

		internal const int THIS_TO_ROOT = 2;

		internal const int ALL = 32767;

		internal const int ALL_THIS = 145;

		internal const int ALL_BASE = 290;

		internal const int ALL_ROOT = 580;

		private readonly int f;

		public bool baseDestroying
		{
			get
			{
				return (this.f & 2) == 2;
			}
		}

		public bool baseExit
		{
			get
			{
				return (this.f & 32) == 32;
			}
		}

		public bool bindStrong
		{
			get
			{
				return (this.f & 4096) == 4096;
			}
		}

		public bool bindWeak
		{
			get
			{
				return (this.f & 4096) == 0;
			}
		}

		public bool callAgain
		{
			get
			{
				return (this.f & 1024) == 1024;
			}
		}

		public bool callFirst
		{
			get
			{
				return (this.f & 1024) == 0;
			}
		}

		public bool kindRoot
		{
			get
			{
				return (this.f & 2048) == 2048;
			}
		}

		public bool kindVessel
		{
			get
			{
				return (this.f & 2048) == 0;
			}
		}

		public bool networkInvalid
		{
			get
			{
				return (this.f & 8) == 8;
			}
		}

		public bool networkValid
		{
			get
			{
				return (this.f & 8) == 0;
			}
		}

		public bool overrideBase
		{
			get
			{
				return (this.f & 256) == 256;
			}
		}

		public bool overrideRoot
		{
			get
			{
				return (this.f & 512) == 512;
			}
		}

		public bool overrideThis
		{
			get
			{
				return (this.f & 128) == 128;
			}
		}

		public bool ownerClient
		{
			get
			{
				return (this.f & 8192) == 8192;
			}
		}

		public bool ownerServer
		{
			get
			{
				return (this.f & 8192) == 0;
			}
		}

		public bool rootDestroying
		{
			get
			{
				return (this.f & 4) == 4;
			}
		}

		public bool rootExit
		{
			get
			{
				return (this.f & 64) == 64;
			}
		}

		public bool runningLocally
		{
			get
			{
				return (this.f & 16384) == 16384;
			}
		}

		public bool runningRemotely
		{
			get
			{
				return (this.f & 16384) == 0;
			}
		}

		public bool thisDestroying
		{
			get
			{
				return (this.f & 1) == 1;
			}
		}

		public bool thisExit
		{
			get
			{
				return (this.f & 16) == 16;
			}
		}

		internal Commandment(int f)
		{
			this.f = f & 262143;
		}

		public override string ToString()
		{
			if ((this.f & 229376) == 0)
			{
				return "INVALID";
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = this.f & 112;
			switch (num)
			{
				case 16:
				{
					stringBuilder.Append("exit[THIS]");
					break;
				}
				case 18:
				{
					stringBuilder.Append("exit[THIS,BASE]");
					break;
				}
				case 20:
				{
					stringBuilder.Append("exit[THIS,ROOT]");
					break;
				}
				default:
				{
					if (num == 32)
					{
						stringBuilder.Append("exit[BASE]");
						break;
					}
					else if (num == 36)
					{
						stringBuilder.Append("exit[BASE,ROOT]");
						break;
					}
					else if (num == 64)
					{
						stringBuilder.Append("exit[ROOT]");
						break;
					}
					else if (num == 112)
					{
						stringBuilder.Append("exit[ALL]");
						break;
					}
					else
					{
						break;
					}
				}
			}
			num = this.f & 896;
			switch (num)
			{
				case 128:
				{
					stringBuilder.Append("override[THIS]");
					break;
				}
				case 130:
				{
					stringBuilder.Append("override[THIS,BASE]");
					break;
				}
				case 132:
				{
					stringBuilder.Append("override[THIS,ROOT]");
					break;
				}
				default:
				{
					if (num == 256)
					{
						stringBuilder.Append("override[BASE]");
						break;
					}
					else if (num == 260)
					{
						stringBuilder.Append("override[BASE,ROOT]");
						break;
					}
					else if (num == 512)
					{
						stringBuilder.Append("override[ROOT]");
						break;
					}
					else if (num == 896)
					{
						stringBuilder.Append("override[ALL]");
						break;
					}
					else
					{
						break;
					}
				}
			}
			num = this.f & 2048;
			if (num == 0)
			{
				stringBuilder.Append("kind[VESL]");
			}
			else if (num == 2048)
			{
				stringBuilder.Append("kind[ROOT]");
			}
			num = this.f & 4096;
			if (num == 0)
			{
				stringBuilder.Append("bind[WEAK]");
			}
			else if (num == 4096)
			{
				stringBuilder.Append("bind[STRONG]");
			}
			num = this.f & 8192;
			if (num == 0)
			{
				stringBuilder.Append("server[");
			}
			else if (num == 8192)
			{
				stringBuilder.Append("client[");
			}
			num = this.f & 16384;
			if (num == 0)
			{
				stringBuilder.Append("RMOTE]");
			}
			else if (num == 16384)
			{
				stringBuilder.Append("LOCAL]");
			}
			num = this.f & 8;
			if (num == 0)
			{
				stringBuilder.Append("net[YES]");
			}
			else if (num == 8)
			{
				stringBuilder.Append("net[NOO]");
			}
			num = this.f & 7;
			switch (num)
			{
				case 1:
				{
					stringBuilder.Append("destroy[THIS]");
					break;
				}
				case 2:
				{
					stringBuilder.Append("destroy[BASE]");
					break;
				}
				case 3:
				{
					stringBuilder.Append("destroy[THIS,BASE]");
					break;
				}
				case 4:
				{
					stringBuilder.Append("destroy[ROOT]");
					break;
				}
				case 5:
				{
					stringBuilder.Append("destroy[THIS,ROOT]");
					break;
				}
				case 6:
				{
					stringBuilder.Append("destroy[BASE,ROOT]");
					break;
				}
				case 7:
				{
					stringBuilder.Append("destroy[ALL]");
					break;
				}
			}
			num = this.f & 229376;
			if (num == 32768)
			{
				stringBuilder.Append("->ENTR");
			}
			else if (num == 65536)
			{
				stringBuilder.Append("->PRMO");
			}
			else if (num == 98304)
			{
				stringBuilder.Append("->DEMO");
			}
			else if (num == 131072)
			{
				stringBuilder.Append("->EXIT");
			}
			if ((this.f & 1024) == 0)
			{
				stringBuilder.Append("(first)");
			}
			return stringBuilder.ToString();
		}

		internal static class BINDING
		{
			public const int STRONG = 4096;

			public const int WEAK = 0;

			public const int ALL = 4096;
		}

		internal static class DESTROY
		{
			public const int THIS = 1;

			public const int BASE = 2;

			public const int ROOT = 4;

			public const int NONE = 0;

			public const int ALL = 7;
		}

		internal static class EVENT
		{
			public const int NONE = 0;

			public const int ENTER = 32768;

			public const int ENGAUGE = 65536;

			public const int CEASE = 98304;

			public const int EXIT = 131072;

			public const int ALL = 229376;
		}

		internal static class EXIT
		{
			public const int THIS = 16;

			public const int BASE = 32;

			public const int ROOT = 64;

			public const int NONE = 0;

			public const int ALL = 112;
		}

		internal static class KIND
		{
			public const int ROOT = 2048;

			public const int VESSEL = 0;

			public const int ALL = 2048;
		}

		internal static class NETWORK
		{
			public const int VALID = 0;

			public const int INVALID = 8;

			public const int ALL = 8;
		}

		internal static class ONCE
		{
			public const int TRUE = 1024;

			public const int FALSE = 0;

			public const int ALL = 1024;
		}

		internal static class OVERRIDE
		{
			public const int THIS = 128;

			public const int BASE = 256;

			public const int ROOT = 512;

			public const int NONE = 0;

			public const int ALL = 896;
		}

		internal static class OWNER
		{
			public const int CLIENT = 8192;

			public const int SERVER = 0;

			public const int ALL = 8192;
		}

		internal static class PLACE
		{
			public const int HERE = 16384;

			public const int ELSEWHERE = 0;

			public const int ALL = 16384;
		}
	}

	protected enum ControllerFlags
	{
		EnableWhenLocalPlayer = 1,
		EnableWhenLocalAI = 2,
		EnableWhenRemotePlayer = 4,
		EnableWhenRemoteAI = 8,
		DisableWhenLocalPlayer = 16,
		ToggleEnableWhenLocalPlayer = 17,
		DisableWhenLocalAI = 32,
		ToggleEnableLocalAI = 34,
		DisableWhenRemotePlayer = 64,
		ToggleEnableRemotePlayer = 68,
		DisableWhenRemoteAI = 128,
		ToggleEnableRemoteAI = 136,
		AlwaysSavedAsDisabled = 256,
		AlwaysSavedAsEnabled = 512,
		DontMessWithEnabled = 768,
		IncompatibleAsRemoteAI = 1024,
		IncompatibleAsRemotePlayer = 2048,
		IncompatibleAsLocalPlayer = 4096,
		IncompatibleAsLocalAI = 8192
	}
}