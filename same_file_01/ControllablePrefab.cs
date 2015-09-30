using Facepunch;
using System;
using System.Collections.Generic;
using uLink;

public class ControllablePrefab : CharacterPrefab
{
	private const byte kVesselFlag_Vessel = 1;

	private const byte kVesselFlag_Vessel_Standalone = 3;

	private const byte kVesselFlag_Vessel_Dependant = 5;

	private const byte kVesselFlag_Vessel_Free = 7;

	private const byte kVesselFlag_PlayerCanControl = 8;

	private const byte kVesselFlag_AICanControl = 16;

	private const byte kVesselFlag_StaticGroup = 32;

	private const byte kVesselFlag_Missing = 64;

	private const byte kVesselKindMask = 7;

	private readonly static Type[] minimalRequiredIDLocals;

	private static Dictionary<string, byte> playerRootCompatibilityCache;

	private static Dictionary<string, sbyte> aiRootCompatibilityCache;

	private static Dictionary<string, byte> vesselCompatibilityCache;

	private bool aiRootComapatable
	{
		get
		{
			Controllable controllable = ((Character)base.serverPrefab).controllable;
			if (!controllable)
			{
				return false;
			}
			if (!controllable.classFlagsRootControllable)
			{
				return false;
			}
			if (!controllable.classFlagsAISupport)
			{
				return false;
			}
			controllable = ((Character)base.proxyPrefab).controllable;
			if (!controllable)
			{
				return false;
			}
			if (!controllable.classFlagsRootControllable)
			{
				return false;
			}
			if (!controllable.classFlagsAISupport)
			{
				return false;
			}
			return true;
		}
	}

	private ControllerClass.Merge mergedClasses
	{
		get
		{
			ControllerClass.Merge merge = new ControllerClass.Merge();
			Controllable.MergeClasses(base.serverPrefab, ref merge);
			Controllable.MergeClasses(base.proxyPrefab, ref merge);
			Controllable.MergeClasses(base.localPrefab, ref merge);
			return merge;
		}
	}

	private bool playerRootComapatable
	{
		get
		{
			Controllable controllable = ((Character)base.serverPrefab).controllable;
			if (!controllable)
			{
				return false;
			}
			if (!controllable.classFlagsRootControllable)
			{
				return false;
			}
			if (!controllable.classFlagsPlayerSupport)
			{
				return false;
			}
			controllable = ((Character)base.proxyPrefab).controllable;
			if (!controllable)
			{
				return false;
			}
			if (!controllable.classFlagsRootControllable)
			{
				return false;
			}
			if (!controllable.classFlagsPlayerSupport)
			{
				return false;
			}
			return true;
		}
	}

	private byte vesselCompatibility
	{
		get
		{
			byte num;
			ControllerClass.Merge merge = this.mergedClasses;
			if (!merge.any)
			{
				return (byte)0;
			}
			if (!merge.vessel)
			{
				return (byte)64;
			}
			if (merge.vesselFree)
			{
				num = 7;
			}
			else if (!merge.vesselDependant)
			{
				if (!merge.vesselStandalone)
				{
					throw new NotImplementedException();
				}
				num = 3;
			}
			else
			{
				num = 5;
			}
			if (merge[true])
			{
				num = (byte)(num | 8);
			}
			if (merge[false])
			{
				num = (byte)(num | 16);
			}
			if (merge.staticGroup)
			{
				num = (byte)(num | 32);
			}
			return num;
		}
	}

	static ControllablePrefab()
	{
		ControllablePrefab.minimalRequiredIDLocals = new Type[] { typeof(Controllable) };
		ControllablePrefab.playerRootCompatibilityCache = new Dictionary<string, byte>();
		ControllablePrefab.aiRootCompatibilityCache = new Dictionary<string, sbyte>();
		ControllablePrefab.vesselCompatibilityCache = new Dictionary<string, byte>();
	}

	public ControllablePrefab() : this(typeof(Character), false, ControllablePrefab.minimalRequiredIDLocals, false)
	{
	}

	protected ControllablePrefab(Type characterType, params Type[] idlocalRequired) : this(characterType, true, idlocalRequired, (idlocalRequired == null ? false : (int)idlocalRequired.Length > 0))
	{
	}

	protected ControllablePrefab(Type characterType) : this(characterType, true, null, false)
	{
	}

	private ControllablePrefab(Type characterType, bool typeCheck, Type[] requiredIDLocalTypes, bool mergeTypes) : base(characterType, (!mergeTypes ? ControllablePrefab.minimalRequiredIDLocals : CharacterPrefab.TypeArrayAppend(ControllablePrefab.minimalRequiredIDLocals, requiredIDLocalTypes)))
	{
	}

	public static void EnsurePrefabIsAIRootCompatible(string name, out bool staticGroup)
	{
		sbyte num;
		NetMainPrefab.EnsurePrefabName(name);
		if (!ControllablePrefab.aiRootCompatibilityCache.TryGetValue(name, out num))
		{
			ControllablePrefab controllablePrefab = NetMainPrefab.Lookup<ControllablePrefab>(name);
			if (!controllablePrefab)
			{
				num = 0;
			}
			else if (controllablePrefab.aiRootComapatable)
			{
				num = (!((Character)controllablePrefab.serverPrefab).controllable.classFlagsStaticGroup ? 1 : -1);
			}
			else
			{
				num = 2;
			}
			ControllablePrefab.aiRootCompatibilityCache[name] = num;
		}
		switch (num)
		{
			case -1:
			{
				staticGroup = true;
				break;
			}
			case 0:
			{
				throw new NonControllableException(name);
			}
			case 1:
			{
				staticGroup = false;
				break;
			}
			case 2:
			{
				throw new NonAIRootControllableException(name);
			}
			default:
			{
				throw new NonControllableException(name);
			}
		}
	}

	public static void EnsurePrefabIsPlayerRootCompatible(string name)
	{
		byte num;
		NetMainPrefab.EnsurePrefabName(name);
		if (!ControllablePrefab.playerRootCompatibilityCache.TryGetValue(name, out num))
		{
			ControllablePrefab controllablePrefab = NetMainPrefab.Lookup<ControllablePrefab>(name);
			if (controllablePrefab)
			{
				num = (byte)((controllablePrefab.playerRootComapatable ? 1 : 2));
			}
			else
			{
				num = 0;
			}
			ControllablePrefab.playerRootCompatibilityCache[name] = num;
		}
		if (num == 0)
		{
			throw new NonControllableException(name);
		}
		if (num == 2)
		{
			throw new NonPlayerRootControllableException(name);
		}
	}

	public static void EnsurePrefabIsVessel(string name, out ControllablePrefab.VesselInfo vi)
	{
		byte vesselCompatibility = ControllablePrefab.GetVesselCompatibility(name);
		if ((vesselCompatibility & 1) != 1)
		{
			if ((vesselCompatibility & 64) != 64)
			{
				throw new NonControllableException(name);
			}
			throw new NonVesselControllableException(name);
		}
		if ((vesselCompatibility & 24) == 0)
		{
			throw new NonControllableException("The vessel has not been marked for either ai and/or player control. not bothering to spawn it.");
		}
		vi = new ControllablePrefab.VesselInfo(vesselCompatibility);
	}

	public static void EnsurePrefabIsVessel(string name, Controllable forControllable, out ControllablePrefab.VesselInfo vi)
	{
		ControllablePrefab.EnsurePrefabIsVessel(name, out vi);
		if (forControllable && forControllable.controlled)
		{
			if (forControllable.aiControlled)
			{
				if (!vi.supportsAI)
				{
					throw new NonAIVesselControllableException(name);
				}
			}
			else if (forControllable.playerControlled && !vi.supportsPlayer)
			{
				throw new NonPlayerVesselControllableException(name);
			}
		}
	}

	private static byte GetVesselCompatibility(string name)
	{
		byte num;
		NetMainPrefab.EnsurePrefabName(name);
		if (ControllablePrefab.vesselCompatibilityCache.TryGetValue(name, out num))
		{
			return num;
		}
		ControllablePrefab controllablePrefab = NetMainPrefab.Lookup<ControllablePrefab>(name);
		if (controllablePrefab)
		{
			num = controllablePrefab.vesselCompatibility;
		}
		else
		{
			num = 0;
		}
		ControllablePrefab.vesselCompatibilityCache[name] = num;
		return num;
	}

	protected override void StandardInitialization(bool didAppend, IDRemote appended, NetInstance instance, Facepunch.NetworkView view, ref uLink.NetworkMessageInfo info)
	{
		Controllable controllable = ((Character)instance.idMain).controllable;
		controllable.PrepareInstantiate(view, ref info);
		base.StandardInitialization(false, appended, instance, view, ref info);
		if (didAppend)
		{
			NetMainPrefab.IssueLocallyAppended(appended, instance.idMain);
		}
		controllable.OnInstantiated();
	}

	public struct VesselInfo
	{
		private byte data;

		public bool bindless
		{
			get
			{
				switch (this.data & 7)
				{
					case 0:
					{
						return false;
					}
					case 1:
					case 2:
					case 4:
					case 6:
					{
						throw new NotImplementedException();
					}
					case 3:
					{
						return true;
					}
					case 5:
					{
						return false;
					}
					case 7:
					{
						return true;
					}
					default:
					{
						throw new NotImplementedException();
					}
				}
			}
		}

		public bool canBind
		{
			get
			{
				switch (this.data & 7)
				{
					case 0:
					{
						return false;
					}
					case 1:
					case 2:
					case 4:
					case 6:
					{
						throw new NotImplementedException();
					}
					case 3:
					{
						return false;
					}
					case 5:
					{
						return true;
					}
					case 7:
					{
						return true;
					}
					default:
					{
						throw new NotImplementedException();
					}
				}
			}
		}

		public bool mustBind
		{
			get
			{
				switch (this.data & 7)
				{
					case 0:
					{
						return false;
					}
					case 1:
					case 2:
					case 4:
					case 6:
					{
						throw new NotImplementedException();
					}
					case 3:
					{
						return false;
					}
					case 5:
					{
						return true;
					}
					case 7:
					{
						return false;
					}
					default:
					{
						throw new NotImplementedException();
					}
				}
			}
		}

		public bool staticGroup
		{
			get
			{
				return (this.data & 32) == 32;
			}
		}

		public bool supportsAI
		{
			get
			{
				return (this.data & 16) == 16;
			}
		}

		public bool supportsPlayer
		{
			get
			{
				return (this.data & 8) == 8;
			}
		}

		internal VesselInfo(byte data)
		{
			this.data = data;
		}
	}
}