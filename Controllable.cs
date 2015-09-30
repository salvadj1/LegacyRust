using Facepunch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using uLink;
using UnityEngine;

public sealed class Controllable : IDLocalCharacter
{
	private const int RT_ENTERED = 1;

	private const int RT_PROMOTED = 3;

	private const int RT_ENTER_LOCK = 8;

	private const int RT_PROMO_LOCK = 16;

	private const int RT_DESTROY_LOCK = 32;

	private const int RT_ENTERED_ONCE = 64;

	private const int RT_PROMOTED_ONCE = 128;

	private const int RT_DEMOTED_ONCE = 256;

	private const int RT_EXITED_ONCE = 512;

	private const int RT_WILL_DESTROY = 1024;

	private const int RT_IS_DESTROYED = 2048;

	private const int RT_RPC_CONTROL_0 = 4096;

	private const int RT_RPC_CONTROL_1 = 8192;

	private const int RT_RPC_CONTROL_2 = 12288;

	private const int RT_STATE = 3;

	private const int RT_ONCE = 960;

	private const int RT_DESTROY_STATE = 3072;

	private const int RT_RPC_CONTROL = 12288;

	private const Controllable.ControlFlags PERSISTANT_FLAGS = Controllable.ControlFlags.Root | Controllable.ControlFlags.Strong;

	private const Controllable.ControlFlags MUTABLE_FLAGS = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player | Controllable.ControlFlags.Initialized;

	private const Controllable.ControlFlags TRANSFERED_FLAGS = Controllable.ControlFlags.Local | Controllable.ControlFlags.Player;

	private const Controllable.ControlFlags CONTROLLER_NPC = 0;

	private const Controllable.ControlFlags CONTROLLER_CLIENT = Controllable.ControlFlags.Player;

	private const Controllable.ControlFlags NETWORK_MINE = Controllable.ControlFlags.Local;

	private const Controllable.ControlFlags NETWORK_PROXY = 0;

	private const Controllable.ControlFlags ACTIVE_OCCUPIED = Controllable.ControlFlags.Owned;

	private const Controllable.ControlFlags ACTIVE_VACANT = 0;

	private const Controllable.ControlFlags TREE_TRUNK = Controllable.ControlFlags.Root;

	private const Controllable.ControlFlags TREE_BRANCH = 0;

	private const Controllable.ControlFlags SETUP_INITIALIZED = Controllable.ControlFlags.Initialized;

	private const Controllable.ControlFlags SETUP_UNINITIALIZED = 0;

	private const Controllable.ControlFlags BINDING_STRONG = Controllable.ControlFlags.Strong;

	private const Controllable.ControlFlags BINDING_WEAK = 0;

	private const Controllable.ControlFlags CONTROLLER_MASK = Controllable.ControlFlags.Player;

	private const Controllable.ControlFlags NETWORK_MASK = Controllable.ControlFlags.Local;

	private const Controllable.ControlFlags ACTIVE_MASK = Controllable.ControlFlags.Owned;

	private const Controllable.ControlFlags TREE_MASK = Controllable.ControlFlags.Root;

	private const Controllable.ControlFlags SETUP_MASK = Controllable.ControlFlags.Initialized;

	private const Controllable.ControlFlags BINDING_MASK = Controllable.ControlFlags.Strong;

	private const Controllable.ControlFlags MASK = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player | Controllable.ControlFlags.Root | Controllable.ControlFlags.Initialized | Controllable.ControlFlags.Strong;

	private const Controllable.ControlFlags OWNER_MASK = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player;

	private const Controllable.ControlFlags OWNER_NPC = Controllable.ControlFlags.Owned;

	private const Controllable.ControlFlags OWNER_CLIENT = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player;

	private const Controllable.ControlFlags OWNER_NET_MASK = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player;

	private const Controllable.ControlFlags OWNER_NET_NPC_MINE = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local;

	private const Controllable.ControlFlags OWNER_NET_NPC_PROXY = Controllable.ControlFlags.Owned;

	private const Controllable.ControlFlags OWNER_NET_CLIENT_MINE = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player;

	private const Controllable.ControlFlags OWNER_NET_CLIENT_PROXY = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player;

	private const Controllable.ControlFlags OWNER_NET_TREE_MASK = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player | Controllable.ControlFlags.Root;

	private const Controllable.ControlFlags OWNER_NET_TREE_NPC_MINE_TRUNK = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Root;

	private const Controllable.ControlFlags OWNER_NET_TREE_NPC_PROXY_TRUNK = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Root;

	private const Controllable.ControlFlags OWNER_NET_TREE_CLIENT_MINE_TRUNK = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player | Controllable.ControlFlags.Root;

	private const Controllable.ControlFlags OWNER_NET_TREE_CLIENT_PROXY_TRUNK = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player | Controllable.ControlFlags.Root;

	private const Controllable.ControlFlags OWNER_NET_TREE_NPC_MINE_BRANCH = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local;

	private const Controllable.ControlFlags OWNER_NET_TREE_NPC_PROXY_BRANCH = Controllable.ControlFlags.Owned;

	private const Controllable.ControlFlags OWNER_NET_TREE_CLIENT_MINE_BRANCH = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player;

	private const Controllable.ControlFlags OWNER_NET_TREE_CLIENT_PROXY_BRANCH = Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player;

	private const string kControllableRPCPrefix = "Controllable:";

	private const string kClientDeleteRPCName = "Controllable:CLD";

	private const string kClearFromChainRPCName = "Controllable:CLR";

	private const string kIdleOnRPCName = "Controllable:ID1";

	private const string kOverrideControlOfRPCName1 = "Controllable:OC1";

	private const string kOverrideControlOfRPCName2 = "Controllable:OC2";

	private const string kClientRefreshRPCName = "Controllable:RFH";

	private const uLink.RPCMode kClientDeleteRPCMode = uLink.RPCMode.Server;

	private const uLink.RPCMode kClearFromChainRPCMode = uLink.RPCMode.All;

	private const uLink.RPCMode kClearFromChainRPCMode_POST = uLink.RPCMode.Others;

	private const uLink.RPCMode kOverrideControlOfRPCMode = uLink.RPCMode.AllBuffered;

	private const uLink.RPCMode kIdleOnRPCMode = uLink.RPCMode.AllBuffered;

	private const uLink.RPCMode kClientSideRootNumberRPCMode = uLink.RPCMode.OthersBuffered;

	private const uLink.RPCMode kClientRefreshRPCMode = uLink.RPCMode.OthersBuffered;

	private const string kRPCCall = "RPC call only. Do not call through script";

	private const bool kRPCCallError = false;

	[NonSerialized]
	private Controllable.CL_Binder _binder;

	[NonSerialized]
	private List<ulong> _rootCountTimeStamps;

	[NonSerialized]
	private int _pendingControlCount;

	[NonSerialized]
	private int _refreshedControlCount;

	[SerializeField]
	private ControllerClass @class;

	[NonSerialized]
	private PlayerClient _playerClient;

	[NonSerialized]
	private Controller _controller;

	[NonSerialized]
	private Controllable.ControlFlags F;

	[NonSerialized]
	private Controllable.Chain ch;

	[NonSerialized]
	private int RT;

	[NonSerialized]
	private uLink.NetworkViewID __controllerDriverViewID;

	[NonSerialized]
	private uLink.NetworkMessageInfo __controllerCreateMessageInfo;

	[NonSerialized]
	private uLink.NetworkView __networkViewForControllable;

	[NonSerialized]
	private bool lateFinding;

	[NonSerialized]
	public bool isInContextQuery;

	private static int localPlayerControllableCount;

	public new bool aiControlled
	{
		get
		{
			return (this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player)) == Controllable.ControlFlags.Owned;
		}
	}

	public new Controllable aiControlledControllable
	{
		get
		{
			Controllable controllable;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player)) != Controllable.ControlFlags.Owned)
			{
				controllable = null;
			}
			else
			{
				controllable = this;
			}
			return controllable;
		}
	}

	public new Controller aiControlledController
	{
		get
		{
			Controller controller;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player)) != Controllable.ControlFlags.Owned)
			{
				controller = null;
			}
			else
			{
				controller = this._controller;
			}
			return controller;
		}
	}

	public new bool assignedControl
	{
		get
		{
			return this.ch.vl;
		}
	}

	internal bool classAssigned
	{
		get
		{
			return this.@class;
		}
	}

	internal bool classFlagsAISupport
	{
		get
		{
			return (!this.@class ? false : this.@class.DefinesClass(false));
		}
	}

	internal bool classFlagsDependantVessel
	{
		get
		{
			return (!this.@class ? false : this.@class.vesselDependant);
		}
	}

	internal bool classFlagsFreeVessel
	{
		get
		{
			return (!this.@class ? false : this.@class.vesselFree);
		}
	}

	internal bool classFlagsPlayerSupport
	{
		get
		{
			return (!this.@class ? false : this.@class.DefinesClass(true));
		}
	}

	internal bool classFlagsRootControllable
	{
		get
		{
			return (!this.@class ? false : this.@class.root);
		}
	}

	internal bool classFlagsStandaloneVessel
	{
		get
		{
			return (!this.@class ? false : this.@class.vesselStandalone);
		}
	}

	internal bool classFlagsStaticGroup
	{
		get
		{
			return (!this.@class ? false : this.@class.staticGroup);
		}
	}

	internal bool classFlagsVessel
	{
		get
		{
			return (!this.@class ? false : this.@class.vessel);
		}
	}

	public new int controlCount
	{
		get
		{
			return this.ch.su;
		}
	}

	public new int controlDepth
	{
		get
		{
			return this.ch.id;
		}
	}

	public new Controllable controllable
	{
		get
		{
			return this;
		}
	}

	public new bool controlled
	{
		get
		{
			return (this.F & Controllable.ControlFlags.Owned) == Controllable.ControlFlags.Owned;
		}
	}

	public new Controllable controlledControllable
	{
		get
		{
			Controllable controllable;
			if ((this.F & Controllable.ControlFlags.Owned) != Controllable.ControlFlags.Owned)
			{
				controllable = null;
			}
			else
			{
				controllable = this;
			}
			return controllable;
		}
	}

	public new Controller controlledController
	{
		get
		{
			Controller controller;
			if ((this.F & Controllable.ControlFlags.Owned) != Controllable.ControlFlags.Owned)
			{
				controller = null;
			}
			else
			{
				controller = this._controller;
			}
			return controller;
		}
	}

	public new Controller controller
	{
		get
		{
			return this._controller;
		}
	}

	public new string controllerClassName
	{
		get
		{
			string className;
			if (!this.@class)
			{
				className = null;
			}
			else
			{
				className = this.@class.GetClassName((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player)) == (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player), (this.F & Controllable.ControlFlags.Local) == Controllable.ControlFlags.Local);
			}
			return className;
		}
	}

	public new bool controlOverridden
	{
		get
		{
			return (!this.ch.vl ? false : this.ch.ln > 0);
		}
	}

	public bool core
	{
		get
		{
			return (this.F & Controllable.ControlFlags.Root) == Controllable.ControlFlags.Root;
		}
	}

	public bool doesNotSave
	{
		get
		{
			return (!this._controller ? true : this._controller.doesNotSave);
		}
	}

	public bool forwardsPlayerClientInput
	{
		get
		{
			return (!this._controller ? false : this._controller.forwardsPlayerClientInput);
		}
	}

	public new bool localAIControlled
	{
		get
		{
			return (this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) == (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local);
		}
	}

	public new Controllable localAIControlledControllable
	{
		get
		{
			Controllable controllable;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) != (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local))
			{
				controllable = null;
			}
			else
			{
				controllable = this;
			}
			return controllable;
		}
	}

	public new Controller localAIControlledController
	{
		get
		{
			Controller controller;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) != (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local))
			{
				controller = null;
			}
			else
			{
				controller = this._controller;
			}
			return controller;
		}
	}

	public new bool localControlled
	{
		get
		{
			return (this.F & Controllable.ControlFlags.Local) == Controllable.ControlFlags.Local;
		}
	}

	public static Controllable localPlayerControllable
	{
		get
		{
			int num = Controllable.localPlayerControllableCount;
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return Controllable.LocalOnly.rootLocalPlayerControllables[0];
			}
			return Controllable.LocalOnly.rootLocalPlayerControllables[Controllable.localPlayerControllableCount - 1];
		}
	}

	public static bool localPlayerControllableExists
	{
		get
		{
			return Controllable.localPlayerControllableCount > 0;
		}
	}

	public new bool localPlayerControlled
	{
		get
		{
			return (this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) == (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player);
		}
	}

	public new Controllable localPlayerControlledControllable
	{
		get
		{
			Controllable controllable;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) != (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player))
			{
				controllable = null;
			}
			else
			{
				controllable = this;
			}
			return controllable;
		}
	}

	public new Controller localPlayerControlledController
	{
		get
		{
			Controller controller;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) != (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player))
			{
				controller = null;
			}
			else
			{
				controller = this._controller;
			}
			return controller;
		}
	}

	public new Character masterCharacter
	{
		get
		{
			Character character;
			if (!this.ch.vl)
			{
				character = null;
			}
			else
			{
				character = this.ch.tp.idMain;
			}
			return character;
		}
	}

	public new Controllable masterControllable
	{
		get
		{
			Controllable controllable;
			if (!this.ch.vl)
			{
				controllable = null;
			}
			else
			{
				controllable = this.ch.tp;
			}
			return controllable;
		}
	}

	public new Controller masterController
	{
		get
		{
			Controller controller;
			if (!this.ch.vl)
			{
				controller = null;
			}
			else
			{
				controller = this.ch.tp._controller;
			}
			return controller;
		}
	}

	public uLink.NetworkPlayer netPlayer
	{
		get
		{
			return (!this._playerClient ? uLink.NetworkPlayer.unassigned : this._playerClient.netPlayer);
		}
	}

	public new Character nextCharacter
	{
		get
		{
			Character character;
			if (!this.ch.vl || !this.ch.up.vl)
			{
				character = null;
			}
			else
			{
				character = this.ch.up.it.idMain;
			}
			return character;
		}
	}

	public new Controllable nextControllable
	{
		get
		{
			Controllable controllable;
			if (!this.ch.vl || !this.ch.up.vl)
			{
				controllable = null;
			}
			else
			{
				controllable = this.ch.up.it;
			}
			return controllable;
		}
	}

	public new Controller nextController
	{
		get
		{
			Controller controller;
			if (!this.ch.vl || !this.ch.up.vl)
			{
				controller = null;
			}
			else
			{
				controller = this.ch.up.it._controller;
			}
			return controller;
		}
	}

	public new string npcName
	{
		get
		{
			string str;
			if (!this.@class)
			{
				str = null;
			}
			else
			{
				str = this.@class.npcName;
			}
			return str;
		}
	}

	public new bool overridingControl
	{
		get
		{
			return (!this.ch.vl ? false : this.ch.nm > 0);
		}
	}

	public new PlayerClient playerClient
	{
		get
		{
			return this._playerClient;
		}
	}

	public new bool playerControlled
	{
		get
		{
			return (this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player)) == (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player);
		}
	}

	public new Controllable playerControlledControllable
	{
		get
		{
			Controllable controllable;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player)) != (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player))
			{
				controllable = null;
			}
			else
			{
				controllable = this;
			}
			return controllable;
		}
	}

	public new Controller playerControlledController
	{
		get
		{
			Controller controller;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player)) != (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player))
			{
				controller = null;
			}
			else
			{
				controller = this._controller;
			}
			return controller;
		}
	}

	public static IEnumerable<Controllable> PlayerCurrentControllables
	{
		get
		{
			Controllable.<>c__Iterator20 variable = null;
			return variable;
		}
	}

	public static IEnumerable<Controllable> PlayerRootControllables
	{
		get
		{
			Controllable.<>c__Iterator1F variable = null;
			return variable;
		}
	}

	public new Character previousCharacter
	{
		get
		{
			Character character;
			if (!this.ch.vl || !this.ch.dn.vl)
			{
				character = null;
			}
			else
			{
				character = this.ch.dn.it.idMain;
			}
			return character;
		}
	}

	public new Controllable previousControllable
	{
		get
		{
			Controllable controllable;
			if (!this.ch.vl || !this.ch.dn.vl)
			{
				controllable = null;
			}
			else
			{
				controllable = this.ch.dn.it;
			}
			return controllable;
		}
	}

	public new Controller previousController
	{
		get
		{
			Controller controller;
			if (!this.ch.vl || !this.ch.dn.vl)
			{
				controller = null;
			}
			else
			{
				controller = this.ch.dn.it._controller;
			}
			return controller;
		}
	}

	public new bool remoteAIControlled
	{
		get
		{
			return (this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) == Controllable.ControlFlags.Owned;
		}
	}

	public new Controllable remoteAIControlledControllable
	{
		get
		{
			Controllable controllable;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) != Controllable.ControlFlags.Owned)
			{
				controllable = null;
			}
			else
			{
				controllable = this;
			}
			return controllable;
		}
	}

	public new Controller remoteAIControlledController
	{
		get
		{
			Controller controller;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) != Controllable.ControlFlags.Owned)
			{
				controller = null;
			}
			else
			{
				controller = this._controller;
			}
			return controller;
		}
	}

	public new bool remoteControlled
	{
		get
		{
			return (int)(this.F & Controllable.ControlFlags.Local) == 0;
		}
	}

	public new bool remotePlayerControlled
	{
		get
		{
			return (this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) == (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player);
		}
	}

	public new Controllable remotePlayerControlledControllable
	{
		get
		{
			Controllable controllable;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) != (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player))
			{
				controllable = null;
			}
			else
			{
				controllable = this;
			}
			return controllable;
		}
	}

	public new Controller remotePlayerControlledController
	{
		get
		{
			Controller controller;
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Local | Controllable.ControlFlags.Player)) != (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player))
			{
				controller = null;
			}
			else
			{
				controller = this._controller;
			}
			return controller;
		}
	}

	public new Character rootCharacter
	{
		get
		{
			Character character;
			if (!this.ch.vl)
			{
				character = null;
			}
			else
			{
				character = this.ch.bt.idMain;
			}
			return character;
		}
	}

	public new Controllable rootControllable
	{
		get
		{
			Controllable controllable;
			if (!this.ch.vl)
			{
				controllable = null;
			}
			else
			{
				controllable = this.ch.bt;
			}
			return controllable;
		}
	}

	public new Controller rootController
	{
		get
		{
			Controller controller;
			if (!this.ch.vl)
			{
				controller = null;
			}
			else
			{
				controller = this.ch.bt._controller;
			}
			return controller;
		}
	}

	public new RPOSLimitFlags rposLimitFlags
	{
		get
		{
			return (!this._controller ? RPOSLimitFlags.KeepOff | RPOSLimitFlags.HideInventory | RPOSLimitFlags.HideContext | RPOSLimitFlags.HideSprites : this._controller.rposLimitFlags);
		}
	}

	public bool vessel
	{
		get
		{
			return (int)(this.F & Controllable.ControlFlags.Root) == 0;
		}
	}

	public Controllable()
	{
	}

	public new bool AssignedControlOf(Controllable controllable)
	{
		return (!this.ch.vl ? false : this == controllable);
	}

	public new bool AssignedControlOf(Controller controller)
	{
		bool flag;
		if (!this.ch.vl || !(this._controller == controller))
		{
			flag = false;
		}
		else
		{
			flag = this._controller;
		}
		return flag;
	}

	public new bool AssignedControlOf(IDMain character)
	{
		return (!this.ch.vl ? false : this.idMain == character);
	}

	public new bool AssignedControlOf(IDBase idBase)
	{
		return (!this.ch.vl || !idBase ? false : this.idMain == idBase.idMain);
	}

	private static int CAP_DEMOTE(int cmd, int RT, Controllable.ControlFlags F)
	{
		cmd = Controllable.CAP_THIS(cmd, RT, F);
		if ((RT & 256) != 256)
		{
			cmd = cmd & -1025 | 0;
		}
		else
		{
			cmd = cmd & -1025 | 1024;
		}
		return cmd;
	}

	private static int CAP_ENTER(int cmd, int RT, Controllable.ControlFlags F)
	{
		cmd = Controllable.CAP_THIS(cmd, RT, F);
		if ((RT & 64) != 64)
		{
			cmd = cmd | cmd & -1025 | 0;
		}
		else
		{
			cmd = cmd | cmd & -1025 | 1024;
		}
		return cmd;
	}

	private static int CAP_EXIT(int cmd, int RT, Controllable.ControlFlags F)
	{
		if ((RT & 512) != 512)
		{
			cmd = cmd | cmd & -1025 | 0;
		}
		else
		{
			cmd = cmd | cmd & -1025 | 1024;
		}
		return cmd;
	}

	private static int CAP_PROMOTE(int cmd, int RT, Controllable.ControlFlags F)
	{
		cmd = Controllable.CAP_THIS(cmd, RT, F);
		if ((RT & 128) != 128)
		{
			cmd = cmd | cmd & -1025 | 0;
		}
		else
		{
			cmd = cmd | cmd & -1025 | 1024;
		}
		return cmd;
	}

	private static int CAP_THIS(int cmd, int RT, Controllable.ControlFlags F)
	{
		cmd = cmd & -30721;
		if ((int)(F & Controllable.ControlFlags.Strong) == 0)
		{
			cmd = cmd | 0;
		}
		else if ((cmd & 32) != 32)
		{
			cmd = cmd | 4096;
		}
		else
		{
			cmd = cmd | 4097;
		}
		if ((F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player)) == Controllable.ControlFlags.Owned)
		{
			cmd = cmd | 0;
		}
		else if ((F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player)) == (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player))
		{
			cmd = cmd | 8192;
		}
		if ((F & Controllable.ControlFlags.Local) != Controllable.ControlFlags.Local)
		{
			cmd = cmd | 0;
		}
		else
		{
			cmd = cmd | 16384;
		}
		if ((F & Controllable.ControlFlags.Root) != Controllable.ControlFlags.Root)
		{
			cmd = cmd | 0;
		}
		else
		{
			cmd = cmd | 2048;
		}
		if ((RT & 3072) != 0 || (cmd & 4128) == 4128)
		{
			cmd = cmd | 1;
		}
		return cmd;
	}

	private void CL_Clear()
	{
		this.ClearBinder();
	}

	private void CL_OverideControlOf(uLink.NetworkViewID rootViewID, uLink.NetworkViewID parentViewID, ref uLink.NetworkMessageInfo info)
	{
		this.ClearBinder();
		this._binder = new Controllable.CL_Binder(this, rootViewID, parentViewID, ref info);
		if (this._binder.CanLink())
		{
			this._binder.Link();
		}
	}

	private void CL_Refresh(int top)
	{
		this._refreshedControlCount = top;
		if (this._pendingControlCount > this._refreshedControlCount)
		{
			if (this._rootCountTimeStamps != null)
			{
				this._rootCountTimeStamps.RemoveRange(top, this._rootCountTimeStamps.Count - top);
			}
			this._pendingControlCount = top;
		}
		if (this.ch.su != top)
		{
			Controllable.CL_Binder.StaticLink(this);
		}
		else
		{
			this.ch.RefreshEngauge();
		}
	}

	private void CL_RootControlCountSet(int count, ref uLink.NetworkMessageInfo info)
	{
		if (this._rootCountTimeStamps == null)
		{
			this._rootCountTimeStamps = new List<ulong>();
		}
		int num = this._rootCountTimeStamps.Count;
		if (num >= count)
		{
			if (num > count)
			{
				this._rootCountTimeStamps.RemoveRange(count, num - count);
			}
			this._rootCountTimeStamps[count - 1] = info.timestampInMillis;
		}
		else
		{
			while (true)
			{
				int num1 = num;
				num = num1 + 1;
				if (num1 >= count - 1)
				{
					break;
				}
				this._rootCountTimeStamps.Add((ulong)-1);
			}
			this._rootCountTimeStamps.Add(info.timestampInMillis);
		}
		this._pendingControlCount = count;
	}

	[Obsolete("RPC call only. Do not call through script", false)]
	[RPC]
	private void CLD(uLink.NetworkMessageInfo info)
	{
	}

	private void ClearBinder()
	{
		if (this._binder != null)
		{
			this._binder.Dispose();
		}
	}

	public void ClientExit()
	{
		if (!this.ch.vl)
		{
			return;
		}
		if (this.ch.vl && this.ch.bt == this.ch.it)
		{
			UnityEngine.Debug.LogWarning("You cannot exit the root controllable", this);
			return;
		}
		if (!this.localControlled)
		{
			throw new InvalidOperationException("Cannot exit other owned controllables");
		}
		base.networkView.RPC("Controllable:CLD", uLink.RPCMode.Server, new object[0]);
	}

	[Obsolete("RPC call only. Do not call through script", false)]
	[RPC]
	private void CLR(uLink.NetworkMessageInfo info)
	{
		this.ch.Delete();
		this.SharedPostCLR();
	}

	private void ControlCease(int cmd)
	{
		this._controller.ControlCease(cmd);
	}

	private void ControlEngauge(int cmd)
	{
		this._controller.ControlEngauge(cmd);
	}

	private void ControlEnter(int cmd)
	{
		try
		{
			this._controller.ControlEnter(cmd);
		}
		finally
		{
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player | Controllable.ControlFlags.Root)) == (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player | Controllable.ControlFlags.Root))
			{
				try
				{
					this._playerClient.OnRootControllableEntered(this);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(exception, this);
				}
				if ((this.F & Controllable.ControlFlags.Local) == Controllable.ControlFlags.Local)
				{
					Controllable.localPlayerControllableCount = Controllable.localPlayerControllableCount + 1;
					Controllable.LocalOnly.rootLocalPlayerControllables.Add(this);
				}
			}
		}
	}

	private void ControlExit(int cmd)
	{
		try
		{
			this._controller.ControlExit(cmd);
		}
		finally
		{
			if ((this.F & (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player | Controllable.ControlFlags.Root)) == (Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player | Controllable.ControlFlags.Root))
			{
				if (this._playerClient)
				{
					try
					{
						this._playerClient.OnRootControllableExited(this);
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogError(exception, this);
					}
				}
				if ((this.F & Controllable.ControlFlags.Local) == Controllable.ControlFlags.Local)
				{
					Controllable.localPlayerControllableCount = Controllable.localPlayerControllableCount - 1;
					Controllable.LocalOnly.rootLocalPlayerControllables.Remove(this);
				}
			}
		}
	}

	public new bool ControlOverriddenBy(Controllable controllable)
	{
		return (!this.ch.vl || this.ch.ln <= 0 || !controllable || !controllable.ch.vl || this.ch.ln <= controllable.ch.ln ? false : this.ch.bt == controllable.ch.bt);
	}

	public new bool ControlOverriddenBy(Controller controller)
	{
		bool flag;
		if (this.ch.vl && this.ch.ln > 0 && controller)
		{
			Controllable controllable = controller.controllable;
			Controllable controllable1 = controllable;
			if (!controllable || !controllable1.ch.vl || this.ch.ln <= controllable1.ch.ln)
			{
				flag = false;
				return flag;
			}
			flag = this.ch.bt == controllable1.ch.bt;
			return flag;
		}
		flag = false;
		return flag;
	}

	public new bool ControlOverriddenBy(Character character)
	{
		bool flag;
		if (this.ch.vl && this.ch.ln > 0 && character)
		{
			Controllable controllable = character.controllable;
			Controllable controllable1 = controllable;
			if (!controllable || !controllable1.ch.vl || this.ch.ln <= controllable1.ch.ln)
			{
				flag = false;
				return flag;
			}
			flag = this.ch.bt == controllable1.ch.bt;
			return flag;
		}
		flag = false;
		return flag;
	}

	public new bool ControlOverriddenBy(IDMain main)
	{
		if (!this.ch.vl || this.ch.ln == 0 || !(main is Character))
		{
			return false;
		}
		return this.ControlOverriddenBy((Character)main);
	}

	public new bool ControlOverriddenBy(IDBase idBase)
	{
		if (!this.ch.vl || this.ch.ln == 0 || !idBase)
		{
			return false;
		}
		return this.ControlOverriddenBy(idBase.idMain);
	}

	public new bool ControlOverriddenBy(IDLocalCharacter idLocal)
	{
		if (!this.ch.vl || this.ch.ln == 0 || !idLocal)
		{
			return false;
		}
		return this.ControlOverriddenBy(idLocal.idMain);
	}

	[DebuggerHidden]
	public static IEnumerable<Controllable> CurrentControllers(IEnumerable<PlayerClient> playerClients)
	{
		Controllable.<CurrentControllers>c__Iterator22 variable = null;
		return variable;
	}

	private static void DO_DEMOTE(int cmd, Controllable citr)
	{
		if ((citr.RT & 16) == 16)
		{
			return;
		}
		Controllable rT = citr;
		rT.RT = rT.RT | 16;
		citr.ControlCease(cmd);
		citr.RT = citr.RT & -20 | 257;
	}

	private static void DO_ENTER(int cmd, Controllable citr)
	{
		if ((citr.RT & 8) == 8)
		{
			return;
		}
		Controllable rT = citr;
		rT.RT = rT.RT | 8;
		citr.ControlEnter(cmd);
		citr.RT = citr.RT & -12 | 65;
	}

	private static void DO_EXIT(int cmd, Controllable citr)
	{
		if ((citr.RT & 8) == 8)
		{
			return;
		}
		Controllable rT = citr;
		rT.RT = rT.RT | 8;
		citr.ControlExit(cmd);
		citr.RT = citr.RT & -12 | 512;
	}

	private static void DO_PROMOTE(int cmd, Controllable citr)
	{
		if ((citr.RT & 16) == 16)
		{
			return;
		}
		Controllable rT = citr;
		rT.RT = rT.RT | 16;
		citr.ControlEngauge(cmd);
		citr.RT = citr.RT & -20 | 131;
	}

	private void DoDestroy()
	{
		this.CL_Clear();
		try
		{
			Controllable rT = this;
			rT.RT = rT.RT | 32;
			if ((this.RT & 3) != 0)
			{
				this.ch.Delete();
			}
		}
		finally
		{
			Controllable controllable = this;
			controllable.RT = controllable.RT & -33;
		}
	}

	private bool EnsureControllee(uLink.NetworkPlayer player)
	{
		if (!this.controlled)
		{
			return false;
		}
		if (player.isClient)
		{
			if (!this.playerControlled || this.playerClient && this.playerClient.netPlayer != player)
			{
				UnityEngine.Debug.LogWarning("player was not the controllee of this player controlled controlable", this);
				return false;
			}
		}
		else if (this.playerControlled)
		{
			UnityEngine.Debug.LogWarning("this player controlled controlable is not server owned", this);
			return false;
		}
		return true;
	}

	internal void FreshInitializeController()
	{
		if (this.__controllerDriverViewID != uLink.NetworkViewID.unassigned)
		{
			Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(this.__controllerDriverViewID);
			Controllable f = this;
			f.F = (Controllable.ControlFlags)((int)f.F | 0);
			this.InitializeController_OnFoundOverriding(networkView);
		}
		else
		{
			if ((this.F & Controllable.ControlFlags.Initialized) == Controllable.ControlFlags.Initialized)
			{
				throw new InvalidOperationException("Was already intialized.");
			}
			Controllable.Chain.ROOT(this);
			this.F = Controllable.ControlFlags.Root;
			this.InitializeController_OnFoundOverriding(null);
		}
	}

	[Conditional("LOG_CONTROL_CHANGE")]
	private static void GuardState(string state, Controllable self)
	{
	}

	[Obsolete("RPC call only. Do not call through script", false)]
	[RPC]
	private void ID1()
	{
		this.SetIdle(true);
	}

	private void InitializeController_OnFoundOverriding(Facepunch.NetworkView driverView)
	{
		Controllable.ControlFlags controlFlag;
		if ((int)(this.F & Controllable.ControlFlags.Root) != 0)
		{
			Controllable controllable = this;
			Controllable.ControlFlags f = controllable.F;
			if (!this.__networkViewForControllable.isMine)
			{
				controlFlag = (Controllable.ControlFlags)0;
			}
			else
			{
				controlFlag = Controllable.ControlFlags.Local;
			}
			controllable.F = f | controlFlag;
			Controllable f1 = this;
			f1.F = f1.F | (!PlayerClient.Find(this.__networkViewForControllable.owner, out this._playerClient) ? Controllable.ControlFlags.Owned : Controllable.ControlFlags.Owned | Controllable.ControlFlags.Player);
		}
		else
		{
			Controllable controllable1 = (driverView.idMain as Character).controllable;
			this.F = this.F & (Controllable.ControlFlags.Root | Controllable.ControlFlags.Strong) | controllable1.F & (Controllable.ControlFlags.Local | Controllable.ControlFlags.Player);
			this._playerClient = controllable1.playerClient;
			controllable1.ch.Add(this);
		}
		Controllable f2 = this;
		f2.F = f2.F | Controllable.ControlFlags.Owned;
		string str = this.controllerClassName;
		if (string.IsNullOrEmpty(str))
		{
			Controllable.ControlFlags controlFlag1 = this.F;
			this.F = (Controllable.ControlFlags)0;
			throw new ArgumentOutOfRangeException("@class", (object)controlFlag1, "The ControllerClass did not support given flags");
		}
		Controller controller = null;
		try
		{
			controller = base.AddAddon<Controller>(str);
			if (!controller)
			{
				throw new ArgumentOutOfRangeException("className", str, "classname as not a Controller!");
			}
			this._controller = controller;
			Controller controller1 = this._controller;
			try
			{
				try
				{
					this._controller.ControllerSetup(this, this.__networkViewForControllable, ref this.__controllerCreateMessageInfo);
				}
				catch
				{
					this._controller = controller1;
					throw;
				}
			}
			catch
			{
				throw;
			}
			Controllable controllable2 = this;
			controllable2.F = controllable2.F | Controllable.ControlFlags.Initialized;
		}
		catch
		{
			if (controller)
			{
				UnityEngine.Object.Destroy(controller);
			}
			this.ch.Delete();
			throw;
		}
	}

	[Conditional("LOG_CONTROL_CHANGE")]
	private static void LogState(bool guard, string state, Controllable controllable)
	{
		UnityEngine.Debug.Log(string.Format("{2}{0}::{1}", controllable.GetType().Name, state, (!guard ? "-" : "+")), controllable);
	}

	internal bool MergeClasses(ref ControllerClass.Merge merge)
	{
		return (!this.@class ? false : merge.Add(this.controllable.@class));
	}

	internal static bool MergeClasses(IDMain character, ref ControllerClass.Merge merge)
	{
		bool flag;
		if (character)
		{
			Controllable component = character.GetComponent<Controllable>();
			Controllable controllable = component;
			if (!component)
			{
				flag = false;
				return flag;
			}
			flag = controllable.MergeClasses(ref merge);
			return flag;
		}
		flag = false;
		return flag;
	}

	private void Net_Shutdown_Exit()
	{
	}

	[RPC]
	private void OC1(uLink.NetworkViewID rootViewID, uLink.NetworkMessageInfo info)
	{
		this.OverrideControlOfHandleRPC(rootViewID, rootViewID, ref info);
	}

	[RPC]
	private void OC2(uLink.NetworkViewID rootViewID, uLink.NetworkViewID parentViewID, uLink.NetworkMessageInfo info)
	{
		this.OverrideControlOfHandleRPC(rootViewID, parentViewID, ref info);
	}

	private void OCO_FOUND(uLink.NetworkViewID viewID, ref uLink.NetworkMessageInfo info)
	{
		this.SetIdle(false);
		this.__networkViewForControllable = base.networkView;
		this.__controllerDriverViewID = viewID;
		this.__controllerCreateMessageInfo = info;
		this.FreshInitializeController();
	}

	private void ON_CHAIN_ABOLISHED()
	{
	}

	private void ON_CHAIN_ERASE(int cmd)
	{
	}

	private void ON_CHAIN_RENEW()
	{
	}

	private void ON_CHAIN_SUBSCRIBE()
	{
	}

	private void OnDestroy()
	{
		this.CL_Clear();
		if (this.isInContextQuery)
		{
			try
			{
				try
				{
					if (Controllable.onDestroyInContextQuery != null)
					{
						Controllable.onDestroyInContextQuery(this);
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogError(exception, this);
				}
			}
			finally
			{
				this.isInContextQuery = false;
			}
		}
		Controllable rT = this;
		rT.RT = rT.RT | 2048;
		if ((this.RT & 1056) == 0)
		{
			this.DoDestroy();
		}
	}

	internal void OnInstantiated()
	{
		if ((this.F & Controllable.ControlFlags.Root) == Controllable.ControlFlags.Root)
		{
			this.ch.RefreshEngauge();
		}
	}

	private void OverrideControlOfHandleRPC(uLink.NetworkViewID rootViewID, uLink.NetworkViewID parentViewID, ref uLink.NetworkMessageInfo info)
	{
		this.CL_OverideControlOf(rootViewID, parentViewID, ref info);
	}

	public new bool OverridingControlOf(Controllable controllable)
	{
		return (!this.ch.vl || this.ch.nm <= 0 || !controllable || !controllable.ch.vl || this.ch.nm <= controllable.ch.nm ? false : this.ch.bt == controllable.ch.bt);
	}

	public new bool OverridingControlOf(Controller controller)
	{
		bool flag;
		if (this.ch.vl && this.ch.nm > 0 && controller)
		{
			Controllable controllable = controller.controllable;
			Controllable controllable1 = controllable;
			if (!controllable || !controllable1.ch.vl || this.ch.nm <= controllable1.ch.nm)
			{
				flag = false;
				return flag;
			}
			flag = this.ch.bt == controllable1.ch.bt;
			return flag;
		}
		flag = false;
		return flag;
	}

	public new bool OverridingControlOf(Character character)
	{
		bool flag;
		if (this.ch.vl && this.ch.nm > 0 && character)
		{
			Controllable controllable = character.controllable;
			Controllable controllable1 = controllable;
			if (!controllable || !controllable1.ch.vl || this.ch.nm <= controllable1.ch.nm)
			{
				flag = false;
				return flag;
			}
			flag = this.ch.bt == controllable1.ch.bt;
			return flag;
		}
		flag = false;
		return flag;
	}

	public new bool OverridingControlOf(IDMain main)
	{
		if (!this.ch.vl || this.ch.nm == 0 || !(main is Character))
		{
			return false;
		}
		return this.OverridingControlOf((Character)main);
	}

	public new bool OverridingControlOf(IDBase idBase)
	{
		if (!this.ch.vl || this.ch.nm == 0 || !idBase)
		{
			return false;
		}
		return this.OverridingControlOf(idBase.idMain);
	}

	public new bool OverridingControlOf(IDLocalCharacter idLocal)
	{
		if (!this.ch.vl || this.ch.nm == 0 || !idLocal)
		{
			return false;
		}
		return this.OverridingControlOf(idLocal.idMain);
	}

	internal void PrepareInstantiate(Facepunch.NetworkView view, ref uLink.NetworkMessageInfo info)
	{
		PlayerClient playerClient;
		this.__controllerCreateMessageInfo = info;
		this.__networkViewForControllable = view;
		if (this.classFlagsRootControllable || this.classFlagsStandaloneVessel)
		{
			this.__controllerDriverViewID = uLink.NetworkViewID.unassigned;
			if (this.classFlagsStandaloneVessel)
			{
				return;
			}
		}
		else if (this.classFlagsDependantVessel || this.classFlagsFreeVessel)
		{
			if (!PlayerClient.Find(view.owner, out playerClient))
			{
				this.__controllerDriverViewID = uLink.NetworkViewID.unassigned;
			}
			else
			{
				this.__controllerDriverViewID = playerClient.topControllable.networkViewID;
			}
			if (this.classFlagsFreeVessel)
			{
				return;
			}
			if (this.__controllerDriverViewID == uLink.NetworkViewID.unassigned)
			{
				UnityEngine.Debug.LogError("NOT RIGHT");
				return;
			}
		}
		this.FreshInitializeController();
	}

	internal void ProcessLocalPlayerPreRender()
	{
		this._controller.ProcessLocalPlayerPreRender();
	}

	public new RelativeControl RelativeControlFrom(Controllable controllable)
	{
		if (!this.ch.vl || !controllable || !controllable.ch.vl || controllable.ch.bt != this.ch.bt)
		{
			return RelativeControl.Incompatible;
		}
		if (this.ch.ln > controllable.ch.ln)
		{
			return RelativeControl.IsOverriding | RelativeControl.Assigned | RelativeControl.OverriddenBy;
		}
		if (this.ch.ln < controllable.ch.ln)
		{
			return RelativeControl.OverriddenBy;
		}
		return RelativeControl.Assigned | RelativeControl.OverriddenBy;
	}

	public new RelativeControl RelativeControlFrom(Controller controller)
	{
		if (this.ch.vl && controller)
		{
			Controllable controllable = controller.controllable;
			Controllable controllable1 = controllable;
			if (controllable && !controllable1.ch.vl && !(controllable1.ch.bt != this.ch.bt))
			{
				if (this.ch.ln > controllable1.ch.ln)
				{
					return RelativeControl.IsOverriding | RelativeControl.Assigned | RelativeControl.OverriddenBy;
				}
				if (this.ch.ln < controllable1.ch.ln)
				{
					return RelativeControl.OverriddenBy;
				}
				return RelativeControl.Assigned | RelativeControl.OverriddenBy;
			}
		}
		return RelativeControl.Incompatible;
	}

	public new RelativeControl RelativeControlFrom(Character character)
	{
		if (!character)
		{
			return RelativeControl.Incompatible;
		}
		return this.RelativeControlFrom(character.controllable);
	}

	public new RelativeControl RelativeControlFrom(IDMain idMain)
	{
		if (!(idMain is Character))
		{
			return RelativeControl.Incompatible;
		}
		return this.RelativeControlFrom((Character)idMain);
	}

	public new RelativeControl RelativeControlFrom(IDLocalCharacter idLocal)
	{
		if (!idLocal)
		{
			return RelativeControl.Incompatible;
		}
		return this.RelativeControlFrom(idLocal.idMain.controllable);
	}

	public new RelativeControl RelativeControlFrom(IDBase idBase)
	{
		if (!idBase)
		{
			return RelativeControl.Incompatible;
		}
		return this.RelativeControlFrom(idBase.idMain as Character);
	}

	public new RelativeControl RelativeControlTo(Controllable controllable)
	{
		if (!this.ch.vl || !controllable || !controllable.ch.vl || controllable.ch.bt != this.ch.bt)
		{
			return RelativeControl.Incompatible;
		}
		if (this.ch.ln > controllable.ch.ln)
		{
			return RelativeControl.OverriddenBy;
		}
		if (this.ch.ln < controllable.ch.ln)
		{
			return RelativeControl.IsOverriding | RelativeControl.Assigned | RelativeControl.OverriddenBy;
		}
		return RelativeControl.Assigned | RelativeControl.OverriddenBy;
	}

	public new RelativeControl RelativeControlTo(Controller controller)
	{
		if (this.ch.vl && controller)
		{
			Controllable controllable = controller.controllable;
			Controllable controllable1 = controllable;
			if (controllable && !controllable1.ch.vl && !(controllable1.ch.bt != this.ch.bt))
			{
				if (this.ch.ln > controllable1.ch.ln)
				{
					return RelativeControl.OverriddenBy;
				}
				if (this.ch.ln < controllable1.ch.ln)
				{
					return RelativeControl.IsOverriding | RelativeControl.Assigned | RelativeControl.OverriddenBy;
				}
				return RelativeControl.Assigned | RelativeControl.OverriddenBy;
			}
		}
		return RelativeControl.Incompatible;
	}

	public new RelativeControl RelativeControlTo(Character character)
	{
		if (!character)
		{
			return RelativeControl.Incompatible;
		}
		return this.RelativeControlTo(character.controllable);
	}

	public new RelativeControl RelativeControlTo(IDMain idMain)
	{
		if (!(idMain is Character))
		{
			return RelativeControl.Incompatible;
		}
		return this.RelativeControlTo((Character)idMain);
	}

	public new RelativeControl RelativeControlTo(IDLocalCharacter idLocal)
	{
		if (!idLocal)
		{
			return RelativeControl.Incompatible;
		}
		return this.RelativeControlTo(idLocal.idMain.controllable);
	}

	public new RelativeControl RelativeControlTo(IDBase idBase)
	{
		if (!idBase)
		{
			return RelativeControl.Incompatible;
		}
		return this.RelativeControlTo(idBase.idMain as Character);
	}

	[RPC]
	private void RFH(byte top)
	{
		this.CL_Refresh((int)top);
	}

	private void RN(int n, ref uLink.NetworkMessageInfo info)
	{
		this.CL_RootControlCountSet(n, ref info);
	}

	[RPC]
	private void RN0(uLink.NetworkMessageInfo info)
	{
		this.RN(0, ref info);
	}

	[RPC]
	private void RN1(uLink.NetworkMessageInfo info)
	{
		this.RN(1, ref info);
	}

	[RPC]
	private void RN2(uLink.NetworkMessageInfo info)
	{
		this.RN(2, ref info);
	}

	[RPC]
	private void RN3(uLink.NetworkMessageInfo info)
	{
		this.RN(3, ref info);
	}

	[RPC]
	private void RN4(uLink.NetworkMessageInfo info)
	{
		this.RN(4, ref info);
	}

	[RPC]
	private void RN5(uLink.NetworkMessageInfo info)
	{
		this.RN(5, ref info);
	}

	[RPC]
	private void RN6(uLink.NetworkMessageInfo info)
	{
		this.RN(6, ref info);
	}

	[RPC]
	private void RN7(uLink.NetworkMessageInfo info)
	{
		this.RN(7, ref info);
	}

	[DebuggerHidden]
	public static IEnumerable<Controllable> RootControllers(IEnumerable<PlayerClient> playerClients)
	{
		Controllable.<RootControllers>c__Iterator21 variable = null;
		return variable;
	}

	private bool SetIdle(bool idle)
	{
		bool flag;
		IDLocalCharacterIdleControl dLocalCharacterIdleControl = base.idMain.idleControl;
		if (!dLocalCharacterIdleControl)
		{
			return false;
		}
		try
		{
			flag = dLocalCharacterIdleControl.SetIdle(idle);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(exception, dLocalCharacterIdleControl);
			flag = true;
		}
		return flag;
	}

	[Obsolete("Used only by PlayerClient")]
	internal void SetRootPlayer(PlayerClient rootPlayer)
	{
	}

	private void SharedPostCLR()
	{
		if (this._controller)
		{
			UnityEngine.Object.Destroy(this._controller);
		}
		Controllable f = this;
		f.F = f.F & (Controllable.ControlFlags.Root | Controllable.ControlFlags.Strong);
		this.RT = 0;
		this._playerClient = null;
		this._controller = null;
		this.SetIdle(true);
	}

	[Conditional("LOG_CONTROL_CHANGE")]
	private static void UnguardState(string state, Controllable self)
	{
	}

	public static event Controllable.DestroyInContextQuery onDestroyInContextQuery;

	private struct Chain
	{
		public Controllable it;

		public Controllable bt;

		public Controllable tp;

		public Controllable.Link dn;

		public Controllable.Link up;

		public byte nm;

		public byte ln;

		public bool vl;

		public bool iv;

		public int id
		{
			get
			{
				return (!this.vl ? -1 : (int)this.nm);
			}
		}

		public int su
		{
			get
			{
				return (!this.vl ? -1 : 1 + this.nm + this.ln);
			}
		}

		private bool Add(ref Controllable.Chain nw, Controllable ct)
		{
			if (!this.vl || nw.vl)
			{
				return false;
			}
			nw.it = ct;
			nw.it.ON_CHAIN_RENEW();
			this.tp.ch.up.vl = true;
			this.tp.ch.up.it = nw.it;
			nw.dn.vl = true;
			nw.dn.it = this.tp;
			nw.nm = this.tp.ch.nm;
			nw.nm = (byte)(nw.nm + 1);
			nw.ln = 0;
			nw.up.vl = false;
			nw.up.it = null;
			nw.tp = nw.it;
			nw.bt = this.tp.ch.bt;
			nw.vl = true;
			Controllable.Link link = nw.dn;
			nw.iv = true;
			do
			{
				link.it.ch.tp = nw.tp;
				link.it.ch.ln = (byte)(link.it.ch.ln + 1);
				link.it.ch.iv = true;
				link = link.it.ch.dn;
			}
			while (link.vl);
			nw.it.ON_CHAIN_SUBSCRIBE();
			return true;
		}

		public bool Add(Controllable vessel)
		{
			return (!vessel ? false : this.Add(ref vessel.ch, vessel));
		}

		public void Delete()
		{
			int num;
			int num1;
			if (!this.vl)
			{
				return;
			}
			int num2 = Controllable.CAP_THIS(16, this.it.RT, this.it.F);
			if (this.up.vl)
			{
				int num3 = this.ln;
				int num4 = (num2 & 145) << 1;
				if (!this.dn.vl)
				{
					num4 = num4 | (num2 & 145) << 2;
				}
				do
				{
					Controllable controllable = this.tp.ch.dn.it;
					Controllable controllable1 = this.tp;
					switch (controllable1.RT & 3)
					{
						case 1:
						{
							int num5 = Controllable.CAP_EXIT(num4, controllable1.RT, controllable1.F);
							num = num5;
							Controllable.DO_EXIT(num5, controllable1);
							break;
						}
						case 2:
						{
							num = Controllable.CAP_THIS(num4, controllable1.RT, controllable1.F);
							break;
						}
						case 3:
						{
							num = Controllable.CAP_EXIT(num4, controllable1.RT, controllable1.F);
							Controllable.DO_DEMOTE(Controllable.CAP_DEMOTE(num, controllable1.RT, controllable1.F), controllable1);
							Controllable.DO_EXIT(num, controllable1);
							break;
						}
						default:
						{
							goto case 2;
						}
					}
					controllable1.ON_CHAIN_ERASE(num);
					controllable1.ch = new Controllable.Chain();
					controllable1.ON_CHAIN_ABOLISHED();
					this.tp = controllable;
					Controllable.Link link = new Controllable.Link();
					this.tp.ch.up = link;
					this.tp.ch.ln = (byte)(this.tp.ch.ln - 1);
					this.tp.ch.tp = this.tp;
					Controllable.Link link1 = this.tp.ch.dn;
					byte num6 = this.tp.ch.ln;
					while (link1.vl)
					{
						Controllable controllable2 = link1.it;
						link1 = controllable2.ch.dn;
						controllable2.ch.tp = this.tp;
						byte num7 = (byte)(num6 - 1);
						num6 = num7;
						controllable2.ch.ln = num7;
					}
					num1 = num3 - 1;
					num3 = num1;
				}
				while (num1 > 0);
			}
			switch (this.it.RT & 3)
			{
				case 1:
				{
					Controllable.DO_EXIT(Controllable.CAP_EXIT(num2, this.it.RT, this.it.F), this.it);
					break;
				}
				case 2:
				{
					break;
				}
				case 3:
				{
					Controllable.DO_DEMOTE(Controllable.CAP_DEMOTE(num2, this.it.RT, this.it.F), this.it);
					Controllable.DO_EXIT(Controllable.CAP_EXIT(num2, this.it.RT, this.it.F), this.it);
					break;
				}
				default:
				{
					goto case 2;
				}
			}
			Controllable controllable3 = this.it;
			controllable3.ON_CHAIN_ERASE(num2);
			Controllable.Link link2 = this.dn;
			Controllable.Chain chain = new Controllable.Chain();
			Controllable.Chain chain1 = chain;
			this = chain;
			controllable3.ch = chain1;
			if (link2.vl)
			{
				Controllable controllable4 = link2.it;
				Controllable.Link link3 = new Controllable.Link();
				controllable4.ch.up = link3;
				int num8 = 0;
				do
				{
					Controllable controllable5 = link2.it;
					link2 = controllable5.ch.dn;
					controllable5.ch.iv = true;
					controllable5.ch.tp = controllable4;
					int num9 = num8;
					num8 = num9 + 1;
					controllable5.ch.ln = (byte)num9;
				}
				while (link2.vl);
			}
			controllable3.ON_CHAIN_ABOLISHED();
		}

		public bool RefreshEngauge()
		{
			int num;
			if (!this.vl)
			{
				return false;
			}
			if (this.tp.ch.iv)
			{
				if (!this.bt.ch.up.vl)
				{
					num = 0;
				}
				else
				{
					Controllable controllable = this.bt;
					num = 128;
					while (true)
					{
						controllable.ch.iv = false;
						switch (controllable.RT & 3)
						{
							case 0:
							{
								Controllable.DO_ENTER(Controllable.CAP_ENTER(num, controllable.RT, controllable.F), controllable);
								goto case 2;
							}
							case 2:
							{
							Label0:
								num = num | 768;
								if (!controllable.ch.up.vl)
								{
									goto Label1;
								}
								controllable = controllable.ch.up.it;
								continue;
							}
							case 3:
							{
								Controllable.DO_DEMOTE(Controllable.CAP_DEMOTE(num, controllable.RT, controllable.F), controllable);
								goto case 2;
							}
							default:
							{
								goto case 2;
							}
						}
					}
				Label1:
				}
				this.tp.ch.iv = false;
				switch (this.tp.RT & 3)
				{
					case 0:
					{
						Controllable.DO_ENTER(Controllable.CAP_ENTER(num & -129, this.tp.RT, this.tp.F), this.tp);
						Controllable.DO_PROMOTE(Controllable.CAP_PROMOTE(num & -129, this.tp.RT, this.tp.F), this.tp);
						break;
					}
					case 1:
					{
						Controllable.DO_PROMOTE(Controllable.CAP_PROMOTE(num & -129, this.tp.RT, this.tp.F), this.tp);
						break;
					}
					case 3:
					{
						break;
					}
				}
			}
			return true;
		}

		public bool RefreshEnter()
		{
			int num;
			if (!this.vl)
			{
				return false;
			}
			if (this.tp.ch.iv)
			{
				if (!this.bt.ch.up.vl)
				{
					num = 0;
				}
				else
				{
					Controllable controllable = this.bt;
					num = 128;
					while (true)
					{
						switch (controllable.RT & 3)
						{
							case 0:
							{
								Controllable.DO_ENTER(Controllable.CAP_ENTER(num, controllable.RT, controllable.F), controllable);
								goto case 2;
							}
							case 2:
							{
							Label0:
								num = num | 768;
								if (!controllable.ch.up.vl)
								{
									goto Label1;
								}
								controllable = controllable.ch.up.it;
								continue;
							}
							case 3:
							{
								Controllable.DO_DEMOTE(Controllable.CAP_DEMOTE(num, controllable.RT, controllable.F), controllable);
								goto case 2;
							}
							default:
							{
								goto case 2;
							}
						}
					}
				Label1:
				}
				switch (this.tp.RT & 3)
				{
					case 0:
					{
						Controllable.DO_ENTER(Controllable.CAP_ENTER(num, this.tp.RT, this.tp.F), this.tp);
						break;
					}
					case 2:
					{
						break;
					}
					case 3:
					{
						break;
					}
				}
			}
			return true;
		}

		public static void ROOT(Controllable root)
		{
			Controllable controllable = root;
			Controllable controllable1 = controllable;
			root.ch.tp = controllable;
			Controllable controllable2 = controllable1;
			controllable1 = controllable2;
			root.ch.bt = controllable2;
			root.ch.it = controllable1;
			root.ch.vl = true;
			int num = 0;
			bool flag = (bool)num;
			root.ch.up.vl = (bool)num;
			root.ch.dn.vl = flag;
			object obj = null;
			controllable1 = (Controllable)obj;
			root.ch.up.it = (Controllable)obj;
			root.ch.dn.it = controllable1;
			int num1 = 0;
			byte num2 = (byte)num1;
			root.ch.ln = (byte)num1;
			root.ch.nm = num2;
			root.ch.iv = true;
		}

		public override string ToString()
		{
			Controllable controllable = null;
			if (!this.vl)
			{
				return "invalid";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (Controllable i = this.bt; i; i = controllable)
			{
				if (i != this.it)
				{
					stringBuilder.Append("   ");
				}
				else
				{
					stringBuilder.Append("-->");
				}
				stringBuilder.AppendLine(i.name);
				if (!i.ch.up.vl)
				{
					controllable = null;
				}
				else
				{
					controllable = i.ch.up.it;
				}
			}
			return stringBuilder.ToString();
		}
	}

	private class CL_Binder : IDisposable
	{
		private static Controllable.CL_Binder first;

		private static Controllable.CL_Binder last;

		private static int binderCount;

		private Controllable.CL_Binder.Search _root;

		private Controllable.CL_Binder.Search _parent;

		private readonly bool sameSearch;

		private uLink.NetworkMessageInfo _info;

		private readonly Controllable owner;

		private bool disposed;

		private Controllable.CL_Binder next;

		private Controllable.CL_Binder prev;

		public CL_Binder(Controllable owner, uLink.NetworkViewID rootID, uLink.NetworkViewID parentID, ref uLink.NetworkMessageInfo info)
		{
			this._root.id = rootID;
			this._parent.id = parentID;
			this._info = info;
			this.owner = owner;
			this.sameSearch = this._root.id == this._parent.id;
			int num = Controllable.CL_Binder.binderCount;
			Controllable.CL_Binder.binderCount = num + 1;
			if (num != 0)
			{
				this.prev = Controllable.CL_Binder.last;
				this.prev.next = this;
				Controllable.CL_Binder.last = this;
			}
			else
			{
				Controllable.CL_Binder cLBinder = this;
				Controllable.CL_Binder.last = cLBinder;
				Controllable.CL_Binder.first = cLBinder;
			}
		}

		public bool CanLink()
		{
			if (!this._root.Find() || this._root.controllable._rootCountTimeStamps == null)
			{
				return false;
			}
			int num = this.CountValidate(this._root.controllable._rootCountTimeStamps, this._root.controllable._rootCountTimeStamps.Count);
			return num == this._root.controllable._pendingControlCount;
		}

		protected int CountValidate(List<ulong> ts, int tsCount)
		{
			if (this.Find())
			{
				Controllable controllable = (!this.sameSearch ? this._parent.controllable : this._root.controllable);
				if (this.sameSearch)
				{
					if (tsCount > 1 && ts[1] <= this._info.timestampInMillis)
					{
						return 2;
					}
					return -1;
				}
				if (controllable._binder != null)
				{
					int num = controllable._binder.CountValidate(ts, tsCount);
					if (tsCount > num && ts[num] <= this._info.timestampInMillis)
					{
						return num + 1;
					}
				}
			}
			return -1;
		}

		public void Dispose()
		{
			Controllable.CL_Binder cLBinder;
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			if (this.owner && this.owner._binder == this)
			{
				this.owner._binder = null;
			}
			int num = Controllable.CL_Binder.binderCount - 1;
			Controllable.CL_Binder.binderCount = num;
			if (num != 0)
			{
				if (Controllable.CL_Binder.first == this)
				{
					Controllable.CL_Binder.first = this.next;
					this.next.prev = null;
				}
				else if (Controllable.CL_Binder.last != this)
				{
					this.next.prev = this.prev;
					this.prev.next = this.next;
				}
				else
				{
					Controllable.CL_Binder.last = this.prev;
					this.prev.next = null;
				}
				object obj = null;
				cLBinder = (Controllable.CL_Binder)obj;
				this.prev = (Controllable.CL_Binder)obj;
				this.next = cLBinder;
			}
			else
			{
				object obj1 = null;
				cLBinder = (Controllable.CL_Binder)obj1;
				this.prev = (Controllable.CL_Binder)obj1;
				Controllable.CL_Binder cLBinder1 = cLBinder;
				cLBinder = cLBinder1;
				this.next = cLBinder1;
				Controllable.CL_Binder cLBinder2 = cLBinder;
				Controllable.CL_Binder.last = cLBinder2;
				Controllable.CL_Binder.first = cLBinder2;
			}
		}

		public bool Find()
		{
			bool flag;
			if (!this._root.Find())
			{
				flag = false;
			}
			else
			{
				flag = (this.sameSearch ? true : this._parent.Find());
			}
			return flag;
		}

		public void Link()
		{
			this.PreLink();
			if (this._root.controllable._pendingControlCount != this._root.controllable._refreshedControlCount)
			{
				this._root.controllable.ch.RefreshEnter();
			}
			else
			{
				this._root.controllable.ch.RefreshEngauge();
			}
		}

		private void PreLink()
		{
			Controllable controllable = (!this.sameSearch ? this._parent.controllable : this._root.controllable);
			if ((int)(controllable.F & Controllable.ControlFlags.Root) == 0)
			{
				controllable._binder.PreLink();
			}
			if ((int)(this.owner.F & Controllable.ControlFlags.Initialized) == 0)
			{
				this.owner.OCO_FOUND(controllable.networkViewID, ref this._info);
			}
		}

		public static void StaticLink(Controllable root)
		{
			Controllable.CL_Binder cLBinder = Controllable.CL_Binder.last;
			for (int i = Controllable.CL_Binder.binderCount - 1; i >= 0; i--)
			{
				Controllable.CL_Binder cLBinder1 = cLBinder;
				cLBinder = cLBinder.prev;
				if (cLBinder1.Find() && cLBinder1._root.controllable == root && cLBinder1.CountValidate(root._rootCountTimeStamps, root._rootCountTimeStamps.Count) == root._refreshedControlCount)
				{
					cLBinder1.Link();
					return;
				}
			}
		}

		private struct Search
		{
			private uLink.NetworkViewID _id;

			private Facepunch.NetworkView _view;

			private Controllable _controllable;

			public Controllable controllable
			{
				get
				{
					return this._controllable;
				}
			}

			public uLink.NetworkViewID id
			{
				get
				{
					return this._id;
				}
				set
				{
					this._id = value;
					this._view = null;
					this._controllable = null;
				}
			}

			public Facepunch.NetworkView view
			{
				get
				{
					return this._view;
				}
			}

			public bool Find()
			{
				if (this._controllable)
				{
					return true;
				}
				if (!this._view)
				{
					this._view = Facepunch.NetworkView.Find(this._id);
					if (!this._view)
					{
						return false;
					}
				}
				Character character = this._view.idMain as Character;
				if (!character)
				{
					return false;
				}
				Controllable controllable = character.controllable;
				Controllable controllable1 = controllable;
				this._controllable = controllable;
				return controllable1;
			}
		}
	}

	[Flags]
	private enum ControlFlags
	{
		Owned = 1,
		Local = 2,
		Player = 4,
		Root = 8,
		Initialized = 16,
		Strong = 32
	}

	public delegate void DestroyInContextQuery(Controllable controllable);

	private struct Link
	{
		public Controllable it;

		public bool vl;
	}

	private static class LocalOnly
	{
		public readonly static List<Controllable> rootLocalPlayerControllables;

		static LocalOnly()
		{
			Controllable.LocalOnly.rootLocalPlayerControllables = new List<Controllable>();
		}
	}
}