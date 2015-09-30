using Facepunch;
using Facepunch.Movement;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

[RequireComponent(typeof(uLinkNetworkView))]
public class ItemRepresentation : IDMain, IInterpTimedEventReceiver
{
	private HeldItemDataBlock _datablock;

	private InventoryHolder _holder;

	[SerializeField]
	private GameObject[] visuals;

	internal ItemRepresentation.ItemModPairArray _itemMods;

	private Facepunch.NetworkView _parentView;

	private IDMain _parentMain;

	private uLink.NetworkViewID _parentViewID;

	private Character _characterSignalee;

	private ViewModel _lastViewModel;

	[NonSerialized]
	private string worldAnimationGroupNameOverride;

	public Socket.LocalSpace muzzle;

	public Socket.LocalSpace hand;

	private ItemModFlags _modFlags;

	private bool worldStateDisabled;

	private CharacterStateFlags? lastCharacterStateFlags;

	private readonly CharacterStateSignal stateSignalReceive;

	private bool modLock;

	[NonSerialized]
	private ItemModRepresentation destroyingRep;

	public HeldItemDataBlock datablock
	{
		get
		{
			return this._datablock;
		}
	}

	public ItemModFlags modFlags
	{
		get
		{
			return this._modFlags;
		}
	}

	public string worldAnimationGroupName
	{
		get
		{
			return this.worldAnimationGroupNameOverride ?? this.datablock.animationGroupName;
		}
	}

	public bool worldModels
	{
		get
		{
			return !this.worldStateDisabled;
		}
		set
		{
			if (this.worldStateDisabled == value)
			{
				this.worldStateDisabled = !this.worldStateDisabled;
				if (this.visuals != null)
				{
					for (int i = 0; i < (int)this.visuals.Length; i++)
					{
						if (this.visuals[i])
						{
							this.visuals[i].SetActive(value);
						}
					}
				}
				if (base.renderer)
				{
					base.renderer.enabled = value;
				}
				if (!value)
				{
					for (int j = 0; j < 5; j++)
					{
						this._itemMods.UnBindAsProxy(j, this);
					}
				}
				else
				{
					for (int k = 0; k < 5; k++)
					{
						this._itemMods.BindAsProxy(k, this);
					}
				}
			}
		}
	}

	public ItemRepresentation() : base(IDFlags.Item)
	{
		ItemRepresentation itemRepresentation = this;
		this.stateSignalReceive = new CharacterStateSignal(itemRepresentation.StateSignalReceive);
	}

	public void Action(int number, uLink.RPCMode mode)
	{
		base.networkView.RPC(ItemRepresentation.ActionRPC(number), mode, new object[0]);
	}

	public void Action<T>(int number, uLink.RPCMode mode, T argument)
	{
		base.networkView.RPC<T>(ItemRepresentation.ActionRPC(number), mode, argument);
	}

	public void Action(int number, uLink.RPCMode mode, params object[] arguments)
	{
		base.networkView.RPC(ItemRepresentation.ActionRPC(number), mode, arguments);
	}

	public void Action(int number, uLink.NetworkPlayer target)
	{
		base.networkView.RPC(ItemRepresentation.ActionRPC(number), target, new object[0]);
	}

	public void Action<T>(int number, uLink.NetworkPlayer target, T argument)
	{
		base.networkView.RPC<T>(ItemRepresentation.ActionRPC(number), target, argument);
	}

	public void Action(int number, uLink.NetworkPlayer target, params object[] arguments)
	{
		base.networkView.RPC(ItemRepresentation.ActionRPC(number), target, arguments);
	}

	public void Action(int number, IEnumerable<uLink.NetworkPlayer> targets)
	{
		base.networkView.RPC(ItemRepresentation.ActionRPC(number), targets, new object[0]);
	}

	public void Action<T>(int number, IEnumerable<uLink.NetworkPlayer> targets, T argument)
	{
		base.networkView.RPC<T>(ItemRepresentation.ActionRPC(number), targets, argument);
	}

	public void Action(int number, IEnumerable<uLink.NetworkPlayer> targets, params object[] arguments)
	{
		base.networkView.RPC(ItemRepresentation.ActionRPC(number), targets, arguments);
	}

	[RPC]
	protected void Action1(uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
		InterpTimedEvent.Queue(this, "Action1", ref info, new object[] { stream });
	}

	[RPC]
	protected void Action1B(byte[] data, uLink.NetworkMessageInfo info)
	{
		this.Action1(new uLink.BitStream(data, false), info);
	}

	[RPC]
	protected void Action2(uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
		InterpTimedEvent.Queue(this, "Action2", ref info, new object[] { stream });
	}

	[RPC]
	protected void Action2B(byte[] data, uLink.NetworkMessageInfo info)
	{
		this.Action2(new uLink.BitStream(data, false), info);
	}

	[RPC]
	protected void Action3(uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
		InterpTimedEvent.Queue(this, "Action3", ref info, new object[] { stream });
	}

	[RPC]
	protected void Action3B(byte[] data, uLink.NetworkMessageInfo info)
	{
		this.Action3(new uLink.BitStream(data, false), info);
	}

	private static string ActionRPC(int number)
	{
		switch (number)
		{
			case 1:
			{
				return "Action1";
			}
			case 2:
			{
				return "Action2";
			}
			case 3:
			{
				return "Action3";
			}
		}
		throw new ArgumentOutOfRangeException("number", (object)number, "number must be at or between 1 and 3");
	}

	private static string ActionRPCBitstream(int number)
	{
		switch (number)
		{
			case 1:
			{
				return "Action1B";
			}
			case 2:
			{
				return "Action2B";
			}
			case 3:
			{
				return "Action3B";
			}
		}
		throw new ArgumentOutOfRangeException("number", (object)number, "number must be at or between 1 and 3");
	}

	public void ActionStream(int number, IEnumerable<uLink.NetworkPlayer> targets, uLink.BitStream stream)
	{
		base.networkView.RPC<byte[]>(ItemRepresentation.ActionRPCBitstream(number), targets, stream.GetDataByteArray());
	}

	public void ActionStream(int number, uLink.NetworkPlayer target, uLink.BitStream stream)
	{
		base.networkView.RPC<byte[]>(ItemRepresentation.ActionRPCBitstream(number), target, stream.GetDataByteArray());
	}

	public void ActionStream(int number, uLink.RPCMode mode, uLink.BitStream stream)
	{
		base.networkView.RPC<byte[]>(ItemRepresentation.ActionRPCBitstream(number), mode, stream.GetDataByteArray());
	}

	protected void Awake()
	{
	}

	private void BindModAsLocal(ref ItemRepresentation.ItemModPair pair, ref ModViewModelAddArgs a)
	{
		if ((int)pair.bindState == 2)
		{
			this.UnBindModAsProxy(ref pair);
		}
		if ((int)pair.bindState == 1 || (int)pair.bindState == 3)
		{
			a.modRep = pair.representation;
			pair.dataBlock.BindAsLocal(ref a);
			pair.bindState = ItemRepresentation.BindState.Local;
		}
	}

	private void BindModAsProxy(ref ItemRepresentation.ItemModPair pair)
	{
		if ((int)pair.bindState == 1)
		{
			pair.dataBlock.BindAsProxy(pair.representation);
			pair.bindState = ItemRepresentation.BindState.World;
		}
	}

	internal void BindViewModel(ViewModel vm, IHeldItem item)
	{
		this.RunViewModelAdd(vm, item, false);
		this._lastViewModel = vm;
	}

	protected bool CheckParent()
	{
		Vector3 vector3;
		Quaternion quaternion;
		if (this._parentView)
		{
			return true;
		}
		if (this._parentViewID != uLink.NetworkViewID.unassigned)
		{
			this._parentView = Facepunch.NetworkView.Find(this._parentViewID);
			if (this._parentView)
			{
				this._parentMain = null;
				Socket.LocalSpace component = this._parentView.GetComponent<PlayerAnimation>().itemAttachment;
				if (component != null)
				{
					if (!this.hand.parent || !(this.hand.parent != base.transform))
					{
						vector3 = this.hand.offset;
						quaternion = Quaternion.Euler(this.hand.eulerRotate);
					}
					else
					{
						vector3 = base.transform.InverseTransformPoint(this.hand.position);
						Quaternion quaternion1 = this.hand.rotation;
						Vector3 vector31 = quaternion1 * Vector3.forward;
						Vector3 vector32 = quaternion1 * Vector3.up;
						vector31 = base.transform.InverseTransformDirection(vector31);
						vector32 = base.transform.InverseTransformDirection(vector32);
						quaternion = Quaternion.LookRotation(vector31, vector32);
					}
					component.AddChildWithCoords(base.transform, vector3, quaternion);
				}
				if (base.networkView.isMine)
				{
					this.worldModels = actor.forceThirdPerson;
				}
				this.FindSignalee();
				return true;
			}
		}
		this.ClearSignals();
		return false;
	}

	private void ClearModPair(ref ItemRepresentation.ItemModPair pair)
	{
		this.KillModRep(ref pair.representation, false);
		this.EraseModDatablock(ref pair.dataBlock);
		pair = new ItemRepresentation.ItemModPair();
	}

	private bool ClearMods()
	{
		bool flag = this.modLock;
		if (this.modLock)
		{
			return false;
		}
		this._modFlags = ItemModFlags.Other;
		try
		{
			this.modLock = true;
			for (int i = 0; i < 5; i++)
			{
				this._itemMods.ClearModPair(i, this);
			}
		}
		finally
		{
			this.modLock = flag;
		}
		return true;
	}

	private void ClearSignals()
	{
		if (this._characterSignalee)
		{
			this._characterSignalee.signal_state -= this.stateSignalReceive;
		}
		if (this._holder)
		{
			this._holder.ClearItemRepresentation(this);
			this._holder = null;
		}
		this._characterSignalee = null;
	}

	private void EraseModDatablock(ref ItemModDataBlock block)
	{
		block = null;
	}

	private void FindSignalee()
	{
		this._parentMain = this._parentView.idMain;
		if (!(this._parentMain is Character))
		{
			this._holder = null;
			this.ClearSignals();
			return;
		}
		Character character = (Character)this._parentMain;
		this.SetSignalee(character);
		this._holder = character.GetLocal<InventoryHolder>();
		if (this._holder)
		{
			this._holder.SetItemRepresentation(this);
		}
	}

	protected CharacterStateFlags GetCharacterStateFlags()
	{
		if (this.CheckParent() && this._parentMain is Character)
		{
			CharacterStateFlags characterStateFlag = ((Character)this._parentMain).stateFlags;
			this.lastCharacterStateFlags = new CharacterStateFlags?(characterStateFlag);
			return characterStateFlag;
		}
		CharacterStateFlags? nullable = this.lastCharacterStateFlags;
		return (!nullable.HasValue ? new CharacterStateFlags() : nullable.Value);
	}

	void IInterpTimedEventReceiver.OnInterpTimedEvent()
	{
		this.OnInterpTimedEvent();
	}

	private void InstallMod(ref ItemRepresentation.ItemModPair to, int slot, ItemModDataBlock datablock, CharacterStateFlags flags)
	{
		to.dataBlock = datablock;
		if (to.representation)
		{
			this.KillModRep(ref to.representation, false);
		}
		if (to.dataBlock.hasModRepresentation && to.dataBlock.AddModRepresentationComponent(base.gameObject, out to.representation))
		{
			to.bindState = ItemRepresentation.BindState.None;
			to.representation.Initialize(this, slot, flags);
			if (!to.representation)
			{
				to.bindState = ItemRepresentation.BindState.Vacant;
				to.representation = null;
			}
			else if (this.worldModels)
			{
				this._itemMods.BindAsProxy(slot, this);
			}
		}
	}

	[RPC]
	protected void InterpDestroy(uLink.NetworkMessageInfo info)
	{
		if (!base.networkView || !base.networkView.isMine)
		{
			InterpTimedEvent.Queue(this, "InterpDestroy", ref info);
			NetCull.DontDestroyWithNetwork(this);
		}
		else
		{
			InterpTimedEvent.Remove(this, true);
		}
	}

	internal void ItemModRepresentationDestroyed(ItemModRepresentation rep)
	{
		if (this.modLock || this.destroyingRep == rep)
		{
			return;
		}
		this._itemMods.KillModForRep(rep, this, true);
	}

	private void KillModRep(ref ItemModRepresentation rep, bool fromCallback)
	{
		if (!fromCallback && rep)
		{
			ItemModRepresentation itemModRepresentation = this.destroyingRep;
			try
			{
				this.destroyingRep = rep;
				UnityEngine.Object.Destroy(rep);
			}
			finally
			{
				this.destroyingRep = itemModRepresentation;
			}
		}
		rep = null;
	}

	[RPC]
	protected void Mods(byte[] data)
	{
		this.ClearMods();
		uLink.BitStream bitStream = new uLink.BitStream(data, false);
		byte num = bitStream.ReadByte();
		if (num > 0)
		{
			CharacterStateFlags characterStateFlags = this.GetCharacterStateFlags();
			for (int i = 0; i < num; i++)
			{
				ItemModDataBlock byUniqueID = (ItemModDataBlock)DatablockDictionary.GetByUniqueID(bitStream.ReadInt32());
				this._itemMods.InstallMod(i, this, byUniqueID, characterStateFlags);
				ItemRepresentation itemRepresentation = this;
				itemRepresentation._modFlags = itemRepresentation._modFlags | byUniqueID.modFlag;
			}
		}
	}

	protected new void OnDestroy()
	{
		try
		{
			InterpTimedEvent.Remove(this, true);
			this.ClearMods();
		}
		finally
		{
			this._parentViewID = uLink.NetworkViewID.unassigned;
			this.ClearSignals();
			base.OnDestroy();
		}
	}

	protected void OnDrawGizmosSelected()
	{
		this.muzzle.DrawGizmos("muzzle");
	}

	protected virtual void OnInterpTimedEvent()
	{
		uLink.BitStream bitStream;
		int num;
		int num1 = -1;
		string tag = InterpTimedEvent.Tag;
		if (tag != null)
		{
			if (ItemRepresentation.<>f__switch$mapB == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(4)
				{
					{ "Action1", 0 },
					{ "Action2", 1 },
					{ "Action3", 2 },
					{ "InterpDestroy", 3 }
				};
				ItemRepresentation.<>f__switch$mapB = strs;
			}
			if (ItemRepresentation.<>f__switch$mapB.TryGetValue(tag, out num))
			{
				switch (num)
				{
					case 0:
					{
						num1 = 1;
						bitStream = InterpTimedEvent.Argument<uLink.BitStream>(0);
						break;
					}
					case 1:
					{
						num1 = 2;
						bitStream = InterpTimedEvent.Argument<uLink.BitStream>(0);
						break;
					}
					case 2:
					{
						num1 = 3;
						bitStream = InterpTimedEvent.Argument<uLink.BitStream>(0);
						break;
					}
					case 3:
					{
						UnityEngine.Object.Destroy(base.gameObject);
						return;
					}
					default:
					{
						InterpTimedEvent.MarkUnhandled();
						return;
					}
				}
				uLink.NetworkMessageInfo info = InterpTimedEvent.Info;
				this.RunAction(num1, bitStream, ref info);
				return;
			}
		}
		InterpTimedEvent.MarkUnhandled();
	}

	public bool OverrideAnimationGroupName(string newGroupName)
	{
		if (string.IsNullOrEmpty(newGroupName))
		{
			newGroupName = null;
		}
		if (this.worldAnimationGroupNameOverride == newGroupName)
		{
			return false;
		}
		if (!this._holder)
		{
			this.worldAnimationGroupNameOverride = newGroupName;
		}
		else
		{
			this._holder.ClearItemRepresentation(this);
			this.worldAnimationGroupNameOverride = newGroupName;
			this._holder.SetItemRepresentation(this);
		}
		return true;
	}

	public bool PlayWorldAnimation(Facepunch.Movement.GroupEvent GroupEvent, float speed, float animationTime)
	{
		if (this._characterSignalee)
		{
			PlayerAnimation component = this._characterSignalee.GetComponent<PlayerAnimation>();
			if (component)
			{
				return component.PlayAnimation(GroupEvent, speed, animationTime);
			}
		}
		return false;
	}

	public bool PlayWorldAnimation(Facepunch.Movement.GroupEvent GroupEvent, float speed)
	{
		if (this._characterSignalee)
		{
			PlayerAnimation component = this._characterSignalee.GetComponent<PlayerAnimation>();
			if (component)
			{
				return component.PlayAnimation(GroupEvent, speed);
			}
		}
		return false;
	}

	public bool PlayWorldAnimation(Facepunch.Movement.GroupEvent GroupEvent)
	{
		if (this._characterSignalee)
		{
			PlayerAnimation component = this._characterSignalee.GetComponent<PlayerAnimation>();
			if (component)
			{
				return component.PlayAnimation(GroupEvent);
			}
		}
		return false;
	}

	internal void PrepareViewModel(ViewModel vm, IHeldItem item)
	{
		this.RunViewModelAdd(vm, item, true);
		this._lastViewModel = vm;
	}

	private void RunAction(int number, uLink.BitStream stream, ref uLink.NetworkMessageInfo info)
	{
		switch (number)
		{
			case 1:
			{
				this.datablock.DoAction1(stream, this, ref info);
				break;
			}
			case 2:
			{
				this.datablock.DoAction2(stream, this, ref info);
				break;
			}
			case 3:
			{
				this.datablock.DoAction3(stream, this, ref info);
				break;
			}
		}
	}

	private void RunViewModelAdd(ViewModel vm, IHeldItem item, bool doMeshes)
	{
		ModViewModelAddArgs modViewModelAddArg = new ModViewModelAddArgs(vm, item, doMeshes);
		for (int i = 0; i < 5; i++)
		{
			this._itemMods.BindAsLocal(i, ref modViewModelAddArg, this);
		}
	}

	[Obsolete("This is dumb. The datablock shouldnt change")]
	internal void SetDataBlockFromHeldItem<T>(HeldItem<T> item)
	where T : HeldItemDataBlock
	{
		this._datablock = (T)item.datablock;
	}

	public virtual void SetParent(GameObject parentGameObject)
	{
		Transform transforms = parentGameObject.transform;
		if (!base.transform.IsChildOf(transforms))
		{
			base.transform.parent = transforms;
		}
	}

	private void SetSignalee(Character signalee)
	{
		if (signalee)
		{
			if (this._characterSignalee && this._characterSignalee == signalee)
			{
				return;
			}
			signalee.signal_state += this.stateSignalReceive;
			this._characterSignalee = signalee;
		}
		else
		{
			this.ClearSignals();
		}
	}

	protected virtual void StateSignalReceive(Character character, bool treatedAsFirst)
	{
		CharacterStateFlags characterStateFlag = character.stateFlags;
		if (this.lastCharacterStateFlags.HasValue && this.lastCharacterStateFlags.Value.Equals(characterStateFlag))
		{
			return;
		}
		this.lastCharacterStateFlags = new CharacterStateFlags?(characterStateFlag);
		for (int i = 0; i < 5; i++)
		{
			if (this._itemMods[i].representation)
			{
				ItemRepresentation.ItemModPair item = this._itemMods[i];
				item.representation.HandleChangedStateFlags(characterStateFlag, !treatedAsFirst);
			}
		}
	}

	protected void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		this._parentViewID = info.networkView.initialData.ReadNetworkViewID();
		int num = info.networkView.initialData.ReadInt32();
		this._datablock = (HeldItemDataBlock)DatablockDictionary.GetByUniqueID(num);
		if (!this.CheckParent())
		{
			Debug.Log("No parent for item rep (yet)", this);
		}
	}

	private void UnBindModAsLocal(ref ItemRepresentation.ItemModPair pair, ref ModViewModelRemoveArgs a)
	{
		if ((int)pair.bindState == 3)
		{
			a.modRep = pair.representation;
			pair.dataBlock.UnBindAsLocal(ref a);
			pair.bindState = ItemRepresentation.BindState.None;
		}
	}

	private void UnBindModAsProxy(ref ItemRepresentation.ItemModPair pair)
	{
		if ((int)pair.bindState == 2)
		{
			pair.dataBlock.UnBindAsProxy(pair.representation);
			pair.bindState = ItemRepresentation.BindState.None;
		}
	}

	internal void UnBindViewModel(ViewModel vm, IHeldItem item)
	{
		ModViewModelRemoveArgs modViewModelRemoveArg = new ModViewModelRemoveArgs(vm, item);
		for (int i = 0; i < 5; i++)
		{
			this._itemMods.UnBindAsLocal(i, ref modViewModelRemoveArg, this);
		}
		if (this._lastViewModel == vm)
		{
			this._lastViewModel = null;
		}
	}

	internal enum BindState : sbyte
	{
		Vacant,
		None,
		World,
		Local
	}

	internal struct ItemModPair
	{
		public ItemModDataBlock dataBlock;

		public ItemModRepresentation representation;

		public ItemRepresentation.BindState bindState;
	}

	internal struct ItemModPairArray
	{
		private const int internalPairCount = 5;

		private ItemRepresentation.ItemModPair a;

		private ItemRepresentation.ItemModPair b;

		private ItemRepresentation.ItemModPair c;

		private ItemRepresentation.ItemModPair d;

		private ItemRepresentation.ItemModPair e;

		public ItemRepresentation.ItemModPair this[int slotNumber]
		{
			get
			{
				switch (slotNumber)
				{
					case 0:
					{
						return this.a;
					}
					case 1:
					{
						return this.b;
					}
					case 2:
					{
						return this.c;
					}
					case 3:
					{
						return this.d;
					}
					case 4:
					{
						return this.e;
					}
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				switch (slotNumber)
				{
					case 0:
					{
						this.a = value;
						break;
					}
					case 1:
					{
						this.b = value;
						break;
					}
					case 2:
					{
						this.c = value;
						break;
					}
					case 3:
					{
						this.d = value;
						break;
					}
					case 4:
					{
						this.e = value;
						break;
					}
					default:
					{
						throw new IndexOutOfRangeException();
					}
				}
			}
		}

		static ItemModPairArray()
		{
		}

		public void BindAsLocal(int slotNumber, ref ModViewModelAddArgs args, ItemRepresentation itemRep)
		{
			switch (slotNumber)
			{
				case 0:
				{
					itemRep.BindModAsLocal(ref this.a, ref args);
					break;
				}
				case 1:
				{
					itemRep.BindModAsLocal(ref this.b, ref args);
					break;
				}
				case 2:
				{
					itemRep.BindModAsLocal(ref this.c, ref args);
					break;
				}
				case 3:
				{
					itemRep.BindModAsLocal(ref this.d, ref args);
					break;
				}
				case 4:
				{
					itemRep.BindModAsLocal(ref this.e, ref args);
					break;
				}
				default:
				{
					throw new IndexOutOfRangeException();
				}
			}
		}

		public void BindAsProxy(int slotNumber, ItemRepresentation itemRep)
		{
			switch (slotNumber)
			{
				case 0:
				{
					itemRep.BindModAsProxy(ref this.a);
					break;
				}
				case 1:
				{
					itemRep.BindModAsProxy(ref this.b);
					break;
				}
				case 2:
				{
					itemRep.BindModAsProxy(ref this.c);
					break;
				}
				case 3:
				{
					itemRep.BindModAsProxy(ref this.d);
					break;
				}
				case 4:
				{
					itemRep.BindModAsProxy(ref this.e);
					break;
				}
				default:
				{
					throw new IndexOutOfRangeException();
				}
			}
		}

		public void ClearModPair(int slotNumber, ItemRepresentation owner)
		{
			switch (slotNumber)
			{
				case 0:
				{
					owner.ClearModPair(ref this.a);
					break;
				}
				case 1:
				{
					owner.ClearModPair(ref this.b);
					break;
				}
				case 2:
				{
					owner.ClearModPair(ref this.c);
					break;
				}
				case 3:
				{
					owner.ClearModPair(ref this.d);
					break;
				}
				case 4:
				{
					owner.ClearModPair(ref this.e);
					break;
				}
				default:
				{
					throw new IndexOutOfRangeException();
				}
			}
		}

		public void InstallMod(int slotNumber, ItemRepresentation owner, ItemModDataBlock datablock, CharacterStateFlags flags)
		{
			switch (slotNumber)
			{
				case 0:
				{
					owner.InstallMod(ref this.a, 0, datablock, flags);
					break;
				}
				case 1:
				{
					owner.InstallMod(ref this.b, 1, datablock, flags);
					break;
				}
				case 2:
				{
					owner.InstallMod(ref this.c, 2, datablock, flags);
					break;
				}
				case 3:
				{
					owner.InstallMod(ref this.d, 3, datablock, flags);
					break;
				}
				case 4:
				{
					owner.InstallMod(ref this.e, 4, datablock, flags);
					break;
				}
				default:
				{
					throw new IndexOutOfRangeException();
				}
			}
		}

		public ItemModDataBlock ItemModDataBlock(int slotNumber)
		{
			switch (slotNumber)
			{
				case 0:
				{
					return this.a.dataBlock;
				}
				case 1:
				{
					return this.b.dataBlock;
				}
				case 2:
				{
					return this.c.dataBlock;
				}
				case 3:
				{
					return this.d.dataBlock;
				}
				case 4:
				{
					return this.e.dataBlock;
				}
			}
			throw new IndexOutOfRangeException();
		}

		private static bool KillModForRep(ref ItemRepresentation.ItemModPair pair, ItemModRepresentation modRep, ItemRepresentation owner, bool fromCallback)
		{
			if (pair.representation != modRep)
			{
				return true;
			}
			owner.KillModRep(ref pair.representation, fromCallback);
			return true;
		}

		public bool KillModForRep(ItemModRepresentation modRep, ItemRepresentation owner, bool fromCallback)
		{
			switch (modRep.modSlot)
			{
				case 0:
				{
					return ItemRepresentation.ItemModPairArray.KillModForRep(ref this.a, modRep, owner, fromCallback);
				}
				case 1:
				{
					return ItemRepresentation.ItemModPairArray.KillModForRep(ref this.b, modRep, owner, fromCallback);
				}
				case 2:
				{
					return ItemRepresentation.ItemModPairArray.KillModForRep(ref this.c, modRep, owner, fromCallback);
				}
				case 3:
				{
					return ItemRepresentation.ItemModPairArray.KillModForRep(ref this.d, modRep, owner, fromCallback);
				}
				case 4:
				{
					return ItemRepresentation.ItemModPairArray.KillModForRep(ref this.e, modRep, owner, fromCallback);
				}
			}
			throw new IndexOutOfRangeException();
		}

		public void UnBindAsLocal(int slotNumber, ref ModViewModelRemoveArgs args, ItemRepresentation itemRep)
		{
			switch (slotNumber)
			{
				case 0:
				{
					itemRep.UnBindModAsLocal(ref this.a, ref args);
					break;
				}
				case 1:
				{
					itemRep.UnBindModAsLocal(ref this.b, ref args);
					break;
				}
				case 2:
				{
					itemRep.UnBindModAsLocal(ref this.c, ref args);
					break;
				}
				case 3:
				{
					itemRep.UnBindModAsLocal(ref this.d, ref args);
					break;
				}
				case 4:
				{
					itemRep.UnBindModAsLocal(ref this.e, ref args);
					break;
				}
				default:
				{
					throw new IndexOutOfRangeException();
				}
			}
		}

		public void UnBindAsProxy(int slotNumber, ItemRepresentation itemRep)
		{
			switch (slotNumber)
			{
				case 0:
				{
					itemRep.UnBindModAsProxy(ref this.a);
					break;
				}
				case 1:
				{
					itemRep.UnBindModAsProxy(ref this.b);
					break;
				}
				case 2:
				{
					itemRep.UnBindModAsProxy(ref this.c);
					break;
				}
				case 3:
				{
					itemRep.UnBindModAsProxy(ref this.d);
					break;
				}
				case 4:
				{
					itemRep.UnBindModAsProxy(ref this.e);
					break;
				}
				default:
				{
					throw new IndexOutOfRangeException();
				}
			}
		}
	}
}