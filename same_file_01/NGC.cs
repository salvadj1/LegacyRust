using Facepunch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

[AddComponentMenu("")]
public sealed class NGC : UnityEngine.MonoBehaviour
{
	private const string kAddRPC = "A";

	private const string kDeleteRPC = "D";

	private const string kCallRPC = "C";

	private const string kPrefabIdentifier = "!Ng";

	[NonSerialized]
	private bool added;

	[NonSerialized]
	internal ushort groupNumber;

	[NonSerialized]
	private uLink.NetworkMessageInfo creation;

	[NonSerialized]
	internal NGCInternalView networkView;

	[NonSerialized]
	internal uLink.NetworkViewID networkViewID;

	[NonSerialized]
	private readonly Dictionary<ushort, NGCView> views = new Dictionary<ushort, NGCView>();

	private static bool log_nonexistant_ngc_errors;

	static NGC()
	{
	}

	public NGC()
	{
	}

	[RPC]
	internal void A(byte[] data, uLink.NetworkMessageInfo info)
	{
		NGCView nGCView = this.Add(data, 0, (int)data.Length, info);
		this.views[nGCView.innerID] = nGCView;
		try
		{
			nGCView.PostInstantiate();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	private NGCView Add(byte[] data, int offset, int length, uLink.NetworkMessageInfo info)
	{
		Vector3 single = new Vector3();
		Vector3 vector3 = new Vector3();
		NGC.Prefab prefab;
		int num = offset;
		int num1 = BitConverter.ToInt32(data, num);
		num = num + 4;
		ushort num2 = BitConverter.ToUInt16(data, num);
		num = num + 2;
		single.x = BitConverter.ToSingle(data, num);
		num = num + 4;
		single.y = BitConverter.ToSingle(data, num);
		num = num + 4;
		single.z = BitConverter.ToSingle(data, num);
		num = num + 4;
		vector3.x = BitConverter.ToSingle(data, num);
		num = num + 4;
		vector3.y = BitConverter.ToSingle(data, num);
		num = num + 4;
		vector3.z = BitConverter.ToSingle(data, num);
		num = num + 4;
		Quaternion quaternion = Quaternion.Euler(vector3);
		NGC.Prefab.Register.Find(num1, out prefab);
		NGCView bitStream = (NGCView)UnityEngine.Object.Instantiate(prefab.prefab, single, quaternion);
		bitStream.creation = info;
		bitStream.innerID = num2;
		bitStream.prefab = prefab;
		bitStream.outer = this;
		bitStream.spawnPosition = single;
		bitStream.spawnRotation = quaternion;
		int num3 = offset + length;
		if (num3 != num)
		{
			byte[] numArray = new byte[num3 - num];
			int num4 = 0;
			do
			{
				int num5 = num4;
				num4 = num5 + 1;
				int num6 = num;
				num = num6 + 1;
				numArray[num5] = data[num6];
			}
			while (num < num3);
			bitStream.initialData = new uLink.BitStream(numArray, false);
		}
		else
		{
			bitStream.initialData = null;
		}
		bitStream.install = new NGC.Prefab.Installation.Instance(prefab.installation);
		return bitStream;
	}

	public static NGC.callf<P0>.Block BlockArgs<P0>(P0 p0)
	{
		NGC.callf<P0>.Block block = new NGC.callf<P0>.Block();
		block.p0 = p0;
		return block;
	}

	public static NGC.callf<P0, P1>.Block BlockArgs<P0, P1>(P0 p0, P1 p1)
	{
		NGC.callf<P0, P1>.Block block = new NGC.callf<P0, P1>.Block();
		block.p0 = p0;
		block.p1 = p1;
		return block;
	}

	public static NGC.callf<P0, P1, P2>.Block BlockArgs<P0, P1, P2>(P0 p0, P1 p1, P2 p2)
	{
		NGC.callf<P0, P1, P2>.Block block = new NGC.callf<P0, P1, P2>.Block();
		block.p0 = p0;
		block.p1 = p1;
		block.p2 = p2;
		return block;
	}

	public static NGC.callf<P0, P1, P2, P3>.Block BlockArgs<P0, P1, P2, P3>(P0 p0, P1 p1, P2 p2, P3 p3)
	{
		NGC.callf<P0, P1, P2, P3>.Block block = new NGC.callf<P0, P1, P2, P3>.Block();
		block.p0 = p0;
		block.p1 = p1;
		block.p2 = p2;
		block.p3 = p3;
		return block;
	}

	public static NGC.callf<P0, P1, P2, P3, P4>.Block BlockArgs<P0, P1, P2, P3, P4>(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4)
	{
		NGC.callf<P0, P1, P2, P3, P4>.Block block = new NGC.callf<P0, P1, P2, P3, P4>.Block();
		block.p0 = p0;
		block.p1 = p1;
		block.p2 = p2;
		block.p3 = p3;
		block.p4 = p4;
		return block;
	}

	public static NGC.callf<P0, P1, P2, P3, P4, P5>.Block BlockArgs<P0, P1, P2, P3, P4, P5>(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
	{
		NGC.callf<P0, P1, P2, P3, P4, P5>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5>.Block();
		block.p0 = p0;
		block.p1 = p1;
		block.p2 = p2;
		block.p3 = p3;
		block.p4 = p4;
		block.p5 = p5;
		return block;
	}

	public static NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Block BlockArgs<P0, P1, P2, P3, P4, P5, P6>(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
	{
		NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Block();
		block.p0 = p0;
		block.p1 = p1;
		block.p2 = p2;
		block.p3 = p3;
		block.p4 = p4;
		block.p5 = p5;
		block.p6 = p6;
		return block;
	}

	public static NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Block BlockArgs<P0, P1, P2, P3, P4, P5, P6, P7>(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
	{
		NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Block();
		block.p0 = p0;
		block.p1 = p1;
		block.p2 = p2;
		block.p3 = p3;
		block.p4 = p4;
		block.p5 = p5;
		block.p6 = p6;
		block.p7 = p7;
		return block;
	}

	public static NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Block BlockArgs<P0, P1, P2, P3, P4, P5, P6, P7, P8>(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
	{
		NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Block();
		block.p0 = p0;
		block.p1 = p1;
		block.p2 = p2;
		block.p3 = p3;
		block.p4 = p4;
		block.p5 = p5;
		block.p6 = p6;
		block.p7 = p7;
		block.p8 = p8;
		return block;
	}

	public static NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Block BlockArgs<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
	{
		NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Block();
		block.p0 = p0;
		block.p1 = p1;
		block.p2 = p2;
		block.p3 = p3;
		block.p4 = p4;
		block.p5 = p5;
		block.p6 = p6;
		block.p7 = p7;
		block.p8 = p8;
		block.p9 = p9;
		return block;
	}

	public static NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Block BlockArgs<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
	{
		NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Block();
		block.p0 = p0;
		block.p1 = p1;
		block.p2 = p2;
		block.p3 = p3;
		block.p4 = p4;
		block.p5 = p5;
		block.p6 = p6;
		block.p7 = p7;
		block.p8 = p8;
		block.p9 = p9;
		block.p10 = p10;
		return block;
	}

	public static NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Block BlockArgs<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11)
	{
		NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Block();
		block.p0 = p0;
		block.p1 = p1;
		block.p2 = p2;
		block.p3 = p3;
		block.p4 = p4;
		block.p5 = p5;
		block.p6 = p6;
		block.p7 = p7;
		block.p8 = p8;
		block.p9 = p9;
		block.p10 = p10;
		block.p11 = p11;
		return block;
	}

	[RPC]
	internal void C(byte[] data, uLink.NetworkMessageInfo info)
	{
		NGC.Procedure procedure = this.Message(data, 0, (int)data.Length, info);
		if (!procedure.Call())
		{
			if (procedure.view)
			{
				Debug.LogWarning(string.Format("Did not call rpc \"{0}\" for view \"{1}\" (entid:{2},msg:{3})", new object[] { procedure.view.prefab.installation.methods[procedure.message].method.Name, procedure.view.name, procedure.view.id, procedure.message }), this);
			}
			else if (NGC.log_nonexistant_ngc_errors)
			{
				Debug.LogWarning(string.Format("Did not call rpc to non existant view# {0}. ( message id was {1} )", procedure.target, procedure.message), this);
			}
		}
	}

	private static NGC.RPCName CallRPCName(NetworkFlags? flags, NGCView view, int messageID)
	{
		return new NGC.RPCName(view, messageID, "C", (!flags.HasValue ? view.prefab.DefaultNetworkFlags(messageID) : flags.Value));
	}

	private static NGC.RPCName CallRPCName(NetworkFlags? flags, NGCView view, int messageID, ref uLink.NetworkPlayer target)
	{
		return NGC.CallRPCName(flags, view, messageID);
	}

	private static NGC.RPCName CallRPCName(NetworkFlags? flags, NGCView view, int messageID, ref IEnumerable<uLink.NetworkPlayer> targets)
	{
		return NGC.CallRPCName(flags, view, messageID);
	}

	private static NGC.RPCName CallRPCName(NetworkFlags? flags, NGCView view, int messageID, ref uLink.RPCMode mode)
	{
		return NGC.CallRPCName(flags, view, messageID);
	}

	[RPC]
	internal void D(ushort id, uLink.NetworkMessageInfo info)
	{
		this.DestroyView(this.Delete(id, info), true, true);
	}

	private NGCView Delete(ushort id, uLink.NetworkMessageInfo info)
	{
		NGCView item = this.views[id];
		this.DestroyView(item, false, false);
		this.views.Remove(id);
		return item;
	}

	private static void DestroyNGC_NetworkView(uLink.NetworkView view)
	{
		NGC component = view.GetComponent<NGC>();
		component.PreDestroy();
		UnityEngine.Object.Destroy(component);
		NetworkInstantiator.defaultDestroyer(view);
	}

	private void DestroyView(NGCView view, bool andGameObject, bool skipPreDestroy)
	{
		if (!view)
		{
			return;
		}
		if (andGameObject)
		{
			GameObject gameObject = view.gameObject;
			if (!skipPreDestroy)
			{
				this.DestroyView(view, false, false);
			}
			UnityEngine.Object.Destroy(gameObject);
		}
		else if (!skipPreDestroy)
		{
			try
			{
				view.PreDestroy();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}

	public static NGCView Find(int id)
	{
		ushort num;
		ushort num1;
		NGC nGC;
		NGCView nGCView;
		if (!NGC.UnpackID(id, out num, out num1))
		{
			return null;
		}
		if (!NGC.Global.byGroup.TryGetValue(num, out nGC))
		{
			return null;
		}
		nGC.views.TryGetValue(num1, out nGCView);
		return nGCView;
	}

	private static NGC.IExecuter FindExecuter(Type[] argumentTypes)
	{
		switch ((int)argumentTypes.Length)
		{
			case 0:
			{
				return typeof(NGC.callf).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 1:
			{
				return typeof(NGC.callf<>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 2:
			{
				return typeof(NGC.callf<,>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 3:
			{
				return typeof(NGC.callf<,,>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 4:
			{
				return typeof(NGC.callf<,,,>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 5:
			{
				return typeof(NGC.callf<,,,,>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 6:
			{
				return typeof(NGC.callf<,,,,,>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 7:
			{
				return typeof(NGC.callf<,,,,,,>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 8:
			{
				return typeof(NGC.callf<,,,,,,,>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 9:
			{
				return typeof(NGC.callf<,,,,,,,,>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 10:
			{
				return typeof(NGC.callf<,,,,,,,,,>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 11:
			{
				return typeof(NGC.callf<,,,,,,,,,,>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
			case 12:
			{
				return typeof(NGC.callf<,,,,,,,,,,,>).MakeGenericType(argumentTypes).GetProperty("Exec", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as NGC.IExecuter;
			}
		}
		throw new ArgumentOutOfRangeException("argumentTypes.Length > {0}");
	}

	internal Dictionary<ushort, NGCView>.ValueCollection GetViews()
	{
		return this.views.Values;
	}

	[Obsolete("NO, Use net cull making sure the prefab name string you must use starts with ;", true)]
	public static new UnityEngine.Object Instantiate(UnityEngine.Object obj)
	{
		return UnityEngine.Object.Instantiate(obj);
	}

	[Obsolete("NO, Use net cull making sure the prefab name string you must use starts with ;", true)]
	public static new UnityEngine.Object Instantiate(UnityEngine.Object obj, Vector3 position, Quaternion rotation)
	{
		return UnityEngine.Object.Instantiate(obj, position, rotation);
	}

	private NGC.Procedure Message(int id, int msg, byte[] args, int argByteSize, uLink.NetworkMessageInfo info)
	{
		NGC.Procedure procedure = new NGC.Procedure()
		{
			outer = this,
			target = id,
			message = msg,
			data = args,
			dataLength = argByteSize,
			info = info
		};
		return procedure;
	}

	private NGC.Procedure Message(int id_msg, byte[] args, int argByteSize, uLink.NetworkMessageInfo info)
	{
		return this.Message(id_msg >> 16 & 65535, id_msg & 65535, args, argByteSize, info);
	}

	private NGC.Procedure Message(byte[] data, int offset, int length, uLink.NetworkMessageInfo info)
	{
		int num;
		byte[] numArray;
		int num1 = offset;
		int num2 = BitConverter.ToInt32(data, num1);
		num1 = num1 + 4;
		int num3 = offset + length;
		if (num1 != num3)
		{
			num = num3 - num1;
			numArray = new byte[num];
			int num4 = 0;
			do
			{
				int num5 = num4;
				num4 = num5 + 1;
				int num6 = num1;
				num1 = num6 + 1;
				numArray[num5] = data[num6];
			}
			while (num1 < num3);
		}
		else
		{
			numArray = null;
			num = 0;
		}
		return this.Message(num2, numArray, num, info);
	}

	internal void NGCViewRPC(NetworkFlags flags, uLink.RPCMode mode, NGCView view, int messageID, byte[] arguments, int argumentsOffset, int argumentsLength)
	{
		this.ShootRPC(NGC.CallRPCName(new NetworkFlags?(flags), view, messageID, ref mode), mode, this.RPCData((int)view.innerID, messageID, arguments, argumentsOffset, argumentsLength));
	}

	internal void NGCViewRPC(NetworkFlags flags, uLink.NetworkPlayer target, NGCView view, int messageID, byte[] arguments, int argumentsOffset, int argumentsLength)
	{
		this.ShootRPC(NGC.CallRPCName(new NetworkFlags?(flags), view, messageID, ref target), target, this.RPCData((int)view.innerID, messageID, arguments, argumentsOffset, argumentsLength));
	}

	internal void NGCViewRPC(NetworkFlags flags, IEnumerable<uLink.NetworkPlayer> targets, NGCView view, int messageID, byte[] arguments, int argumentsOffset, int argumentsLength)
	{
		this.ShootRPC(NGC.CallRPCName(new NetworkFlags?(flags), view, messageID, ref targets), targets, this.RPCData((int)view.innerID, messageID, arguments, argumentsOffset, argumentsLength));
	}

	internal void NGCViewRPC(uLink.RPCMode mode, NGCView view, int messageID, byte[] arguments, int argumentsOffset, int argumentsLength)
	{
		NetworkFlags? nullable = null;
		this.ShootRPC(NGC.CallRPCName(nullable, view, messageID, ref mode), mode, this.RPCData((int)view.innerID, messageID, arguments, argumentsOffset, argumentsLength));
	}

	internal void NGCViewRPC(uLink.NetworkPlayer target, NGCView view, int messageID, byte[] arguments, int argumentsOffset, int argumentsLength)
	{
		NetworkFlags? nullable = null;
		this.ShootRPC(NGC.CallRPCName(nullable, view, messageID, ref target), target, this.RPCData((int)view.innerID, messageID, arguments, argumentsOffset, argumentsLength));
	}

	internal void NGCViewRPC(IEnumerable<uLink.NetworkPlayer> targets, NGCView view, int messageID, byte[] arguments, int argumentsOffset, int argumentsLength)
	{
		NetworkFlags? nullable = null;
		this.ShootRPC(NGC.CallRPCName(nullable, view, messageID, ref targets), targets, this.RPCData((int)view.innerID, messageID, arguments, argumentsOffset, argumentsLength));
	}

	private void OnDestroy()
	{
		this.Release();
	}

	internal static int PackID(int groupNumber, int innerID)
	{
		if (groupNumber < 0 || innerID <= 0)
		{
			return 0;
		}
		return (groupNumber & 65535) << 16 | innerID;
	}

	private void PreDestroy()
	{
		List<NGCView> nGCViews = new List<NGCView>(this.views.Values);
		foreach (NGCView nGCView in nGCViews)
		{
			this.DestroyView(nGCView, false, false);
		}
		foreach (NGCView nGCView1 in nGCViews)
		{
			this.DestroyView(nGCView1, true, true);
		}
	}

	public static void Register(NGCConfiguration configuration)
	{
		NetworkInstantiator.Add("!Ng", new NetworkInstantiator.Creator(NGC.SpawnNGC_NetworkView), new NetworkInstantiator.Destroyer(NGC.DestroyNGC_NetworkView));
		configuration.Install();
	}

	private void Release()
	{
		if (this.added)
		{
			if (NGC.Global.all.Remove(this))
			{
				NGC.Global.byGroup.Remove(this.groupNumber);
			}
			this.added = false;
		}
	}

	private byte[] RPCData(int viewID, int messageID, byte[] arguments, int argumentsOffset, int argumentsLength)
	{
		byte[] bytes = BitConverter.GetBytes(viewID << 16 | messageID & 65535);
		byte[] numArray = new byte[(int)bytes.Length + argumentsLength];
		int num = 0;
		for (int i = 0; i < (int)bytes.Length; i++)
		{
			int num1 = num;
			num = num1 + 1;
			numArray[num1] = bytes[i];
		}
		int num2 = argumentsOffset;
		int num3 = 0;
		while (num3 < argumentsLength)
		{
			int num4 = num;
			num = num4 + 1;
			numArray[num4] = arguments[num2];
			num3++;
			num2++;
		}
		return numArray;
	}

	private void ShootRPC(NGC.RPCName rpc, uLink.RPCMode mode, byte[] data)
	{
		if (rpc.flags != NetworkFlags.Normal)
		{
			this.networkView.RPC<byte[]>(rpc.flags, rpc.name, mode, data);
		}
		else
		{
			this.networkView.RPC<byte[]>(rpc.name, mode, data);
		}
	}

	private void ShootRPC(NGC.RPCName rpc, uLink.NetworkPlayer target, byte[] data)
	{
		if (rpc.flags != NetworkFlags.Normal)
		{
			this.networkView.RPC<byte[]>(rpc.flags, rpc.name, target, data);
		}
		else
		{
			this.networkView.RPC<byte[]>(rpc.name, target, data);
		}
	}

	private void ShootRPC(NGC.RPCName rpc, IEnumerable<uLink.NetworkPlayer> targets, byte[] data)
	{
		if (rpc.flags != NetworkFlags.Normal)
		{
			this.networkView.RPC<byte[]>(rpc.flags, rpc.name, targets, data);
		}
		else
		{
			this.networkView.RPC<byte[]>(rpc.name, targets, data);
		}
	}

	private static uLink.NetworkView SpawnNGC_NetworkView(string prefabName, NetworkInstantiateArgs args, uLink.NetworkMessageInfo info)
	{
		NetworkInstantiatorUtility.AutoSetupNetworkViewOnAwake(args);
		GameObject gameObject = new GameObject(string.Format("__NGC-{0:X}", args.@group), new Type[] { typeof(NGC), typeof(NGCInternalView) })
		{
			hideFlags = HideFlags.HideInHierarchy
		};
		NetworkInstantiatorUtility.ClearAutoSetupNetworkViewOnAwake();
		uLinkNetworkView component = gameObject.GetComponent<uLinkNetworkView>();
		NGC nGC = gameObject.GetComponent<NGC>();
		component.observed = nGC;
		component.rpcReceiver = RPCReceiver.OnlyObservedComponent;
		component.stateSynchronization = uLink.NetworkStateSynchronization.Off;
		nGC.uLink_OnNetworkInstantiate(new uLink.NetworkMessageInfo(info, component));
		return component;
	}

	private void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		NGC nGC;
		if (NGC.Global.byGroup.TryGetValue(this.groupNumber, out nGC))
		{
			if (nGC == this)
			{
				return;
			}
			if (nGC)
			{
				nGC.Release();
			}
		}
		NGC.Global.all.Add(this);
		this.groupNumber = (ushort)this.networkView.@group.id;
		NGC.Global.byGroup[this.groupNumber] = this;
		this.added = true;
		this.creation = info;
	}

	internal static bool UnpackID(int packed, out ushort groupNumber, out ushort innerID)
	{
		if (packed == 0)
		{
			groupNumber = 0;
			innerID = 0;
			return false;
		}
		groupNumber = (ushort)(packed >> 16 & 65535);
		innerID = (ushort)(packed & 65535);
		return true;
	}

	public static class callf
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf.Executer.Singleton;
			}
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf.Call), instance, method, true);
			}
			((NGC.callf.Call)d)();
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf.InfoCall), instance, method, true);
			}
			((NGC.callf.InfoCall)d)(info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf.InfoRoutine), instance, method, true);
			}
			return ((NGC.callf.InfoRoutine)d)(info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf.Routine), instance, method, true);
			}
			return ((NGC.callf.Routine)d)();
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf.StreamCall), instance, method, true);
			}
			((NGC.callf.StreamCall)d)(stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf.StreamInfoCall), instance, method, true);
			}
			((NGC.callf.StreamInfoCall)d)(info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf.StreamInfoRoutine), instance, method, true);
			}
			return ((NGC.callf.StreamInfoRoutine)d)(info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf.StreamRoutine), instance, method, true);
			}
			return ((NGC.callf.StreamRoutine)d)(stream);
		}

		public delegate void Call();

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf.Executer.Singleton = new NGC.callf.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine();

		public delegate void StreamCall(uLink.BitStream stream);

		public delegate void StreamInfoCall(uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(uLink.BitStream stream);
	}

	public static class callf<P0>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0>.Block block = new NGC.callf<P0>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<>.Call<P0>), instance, method, true);
			}
			((NGC.callf<P0>.Call)d)(p0);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<>.InfoCall<P0>), instance, method, true);
			}
			((NGC.callf<P0>.InfoCall)d)(p0, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<>.InfoRoutine<P0>), instance, method, true);
			}
			return ((NGC.callf<P0>.InfoRoutine)d)(p0, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<>.Routine<P0>), instance, method, true);
			}
			return ((NGC.callf<P0>.Routine)d)(p0);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<>.StreamCall<P0>), instance, method, true);
			}
			((NGC.callf<P0>.StreamCall)d)(p0, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<>.StreamInfoCall<P0>), instance, method, true);
			}
			((NGC.callf<P0>.StreamInfoCall)d)(p0, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<>.StreamInfoRoutine<P0>), instance, method, true);
			}
			return ((NGC.callf<P0>.StreamInfoRoutine)d)(p0, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<>.StreamRoutine<P0>), instance, method, true);
			}
			return ((NGC.callf<P0>.StreamRoutine)d)(p0, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			stream.Write<P0>(((NGC.callf<P0>.Block)value).p0, codecOptions);
		}

		public struct Block
		{
			public P0 p0;
		}

		public delegate void Call(P0 p0);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0>.Executer.Singleton = new NGC.callf<P0>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0);

		public delegate void StreamCall(P0 p0, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, uLink.BitStream stream);
	}

	public static class callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			block.p1 = stream.Read<P1>(codecOptions);
			block.p2 = stream.Read<P2>(codecOptions);
			block.p3 = stream.Read<P3>(codecOptions);
			block.p4 = stream.Read<P4>(codecOptions);
			block.p5 = stream.Read<P5>(codecOptions);
			block.p6 = stream.Read<P6>(codecOptions);
			block.p7 = stream.Read<P7>(codecOptions);
			block.p8 = stream.Read<P8>(codecOptions);
			block.p9 = stream.Read<P9>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,>.Call<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Call)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,>.InfoCall<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.InfoCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,>.InfoRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.InfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,>.Routine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Routine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,>.StreamCall<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.StreamCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,>.StreamInfoCall<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.StreamInfoCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,>.StreamInfoRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.StreamInfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,>.StreamRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.StreamRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Block block = (NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Block)value;
			stream.Write<P0>(block.p0, codecOptions);
			stream.Write<P1>(block.p1, codecOptions);
			stream.Write<P2>(block.p2, codecOptions);
			stream.Write<P3>(block.p3, codecOptions);
			stream.Write<P4>(block.p4, codecOptions);
			stream.Write<P5>(block.p5, codecOptions);
			stream.Write<P6>(block.p6, codecOptions);
			stream.Write<P7>(block.p7, codecOptions);
			stream.Write<P8>(block.p8, codecOptions);
			stream.Write<P9>(block.p9, codecOptions);
		}

		public struct Block
		{
			public P0 p0;

			public P1 p1;

			public P2 p2;

			public P3 p3;

			public P4 p4;

			public P5 p5;

			public P6 p6;

			public P7 p7;

			public P8 p8;

			public P9 p9;
		}

		public delegate void Call(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Executer.Singleton = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9);

		public delegate void StreamCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, uLink.BitStream stream);
	}

	public static class callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			block.p1 = stream.Read<P1>(codecOptions);
			block.p2 = stream.Read<P2>(codecOptions);
			block.p3 = stream.Read<P3>(codecOptions);
			block.p4 = stream.Read<P4>(codecOptions);
			block.p5 = stream.Read<P5>(codecOptions);
			block.p6 = stream.Read<P6>(codecOptions);
			block.p7 = stream.Read<P7>(codecOptions);
			block.p8 = stream.Read<P8>(codecOptions);
			block.p9 = stream.Read<P9>(codecOptions);
			block.p10 = stream.Read<P10>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,>.Call<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Call)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,>.InfoCall<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.InfoCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,>.InfoRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.InfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,>.Routine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Routine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,>.StreamCall<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.StreamCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,>.StreamInfoCall<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.StreamInfoCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,>.StreamInfoRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.StreamInfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,>.StreamRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.StreamRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Block block = (NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Block)value;
			stream.Write<P0>(block.p0, codecOptions);
			stream.Write<P1>(block.p1, codecOptions);
			stream.Write<P2>(block.p2, codecOptions);
			stream.Write<P3>(block.p3, codecOptions);
			stream.Write<P4>(block.p4, codecOptions);
			stream.Write<P5>(block.p5, codecOptions);
			stream.Write<P6>(block.p6, codecOptions);
			stream.Write<P7>(block.p7, codecOptions);
			stream.Write<P8>(block.p8, codecOptions);
			stream.Write<P9>(block.p9, codecOptions);
			stream.Write<P10>(block.p10, codecOptions);
		}

		public struct Block
		{
			public P0 p0;

			public P1 p1;

			public P2 p2;

			public P3 p3;

			public P4 p4;

			public P5 p5;

			public P6 p6;

			public P7 p7;

			public P8 p8;

			public P9 p9;

			public P10 p10;
		}

		public delegate void Call(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Executer.Singleton = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10);

		public delegate void StreamCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, uLink.BitStream stream);
	}

	public static class callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			block.p1 = stream.Read<P1>(codecOptions);
			block.p2 = stream.Read<P2>(codecOptions);
			block.p3 = stream.Read<P3>(codecOptions);
			block.p4 = stream.Read<P4>(codecOptions);
			block.p5 = stream.Read<P5>(codecOptions);
			block.p6 = stream.Read<P6>(codecOptions);
			block.p7 = stream.Read<P7>(codecOptions);
			block.p8 = stream.Read<P8>(codecOptions);
			block.p9 = stream.Read<P9>(codecOptions);
			block.p10 = stream.Read<P10>(codecOptions);
			block.p11 = stream.Read<P11>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			P11 p11 = stream.Read<P11>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,,>.Call<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Call)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			P11 p11 = stream.Read<P11>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,,>.InfoCall<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.InfoCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			P11 p11 = stream.Read<P11>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,,>.InfoRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.InfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			P11 p11 = stream.Read<P11>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,,>.Routine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Routine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			P11 p11 = stream.Read<P11>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,,>.StreamCall<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.StreamCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			P11 p11 = stream.Read<P11>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,,>.StreamInfoCall<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.StreamInfoCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			P11 p11 = stream.Read<P11>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,,>.StreamInfoRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.StreamInfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			P9 p9 = stream.Read<P9>(new object[0]);
			P10 p10 = stream.Read<P10>(new object[0]);
			P11 p11 = stream.Read<P11>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,,,,>.StreamRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.StreamRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Block block = (NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Block)value;
			stream.Write<P0>(block.p0, codecOptions);
			stream.Write<P1>(block.p1, codecOptions);
			stream.Write<P2>(block.p2, codecOptions);
			stream.Write<P3>(block.p3, codecOptions);
			stream.Write<P4>(block.p4, codecOptions);
			stream.Write<P5>(block.p5, codecOptions);
			stream.Write<P6>(block.p6, codecOptions);
			stream.Write<P7>(block.p7, codecOptions);
			stream.Write<P8>(block.p8, codecOptions);
			stream.Write<P9>(block.p9, codecOptions);
			stream.Write<P10>(block.p10, codecOptions);
			stream.Write<P11>(block.p11, codecOptions);
		}

		public struct Block
		{
			public P0 p0;

			public P1 p1;

			public P2 p2;

			public P3 p3;

			public P4 p4;

			public P5 p5;

			public P6 p6;

			public P7 p7;

			public P8 p8;

			public P9 p9;

			public P10 p10;

			public P11 p11;
		}

		public delegate void Call(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Executer.Singleton = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11);

		public delegate void StreamCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10, P11 p11, uLink.BitStream stream);
	}

	public static class callf<P0, P1>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0, P1>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0, P1>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0, P1>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0, P1>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0, P1>.Block block = new NGC.callf<P0, P1>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			block.p1 = stream.Read<P1>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,>.Call<P0, P1>), instance, method, true);
			}
			((NGC.callf<P0, P1>.Call)d)(p0, p1);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,>.InfoCall<P0, P1>), instance, method, true);
			}
			((NGC.callf<P0, P1>.InfoCall)d)(p0, p1, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,>.InfoRoutine<P0, P1>), instance, method, true);
			}
			return ((NGC.callf<P0, P1>.InfoRoutine)d)(p0, p1, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,>.Routine<P0, P1>), instance, method, true);
			}
			return ((NGC.callf<P0, P1>.Routine)d)(p0, p1);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,>.StreamCall<P0, P1>), instance, method, true);
			}
			((NGC.callf<P0, P1>.StreamCall)d)(p0, p1, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,>.StreamInfoCall<P0, P1>), instance, method, true);
			}
			((NGC.callf<P0, P1>.StreamInfoCall)d)(p0, p1, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,>.StreamInfoRoutine<P0, P1>), instance, method, true);
			}
			return ((NGC.callf<P0, P1>.StreamInfoRoutine)d)(p0, p1, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,>.StreamRoutine<P0, P1>), instance, method, true);
			}
			return ((NGC.callf<P0, P1>.StreamRoutine)d)(p0, p1, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			NGC.callf<P0, P1>.Block block = (NGC.callf<P0, P1>.Block)value;
			stream.Write<P0>(block.p0, codecOptions);
			stream.Write<P1>(block.p1, codecOptions);
		}

		public struct Block
		{
			public P0 p0;

			public P1 p1;
		}

		public delegate void Call(P0 p0, P1 p1);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0, P1>.Executer.Singleton = new NGC.callf<P0, P1>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, P1 p1, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, P1 p1, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0, P1 p1);

		public delegate void StreamCall(P0 p0, P1 p1, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, P1 p1, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, P1 p1, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, P1 p1, uLink.BitStream stream);
	}

	public static class callf<P0, P1, P2>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0, P1, P2>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0, P1, P2>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0, P1, P2>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0, P1, P2>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2>.Block block = new NGC.callf<P0, P1, P2>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			block.p1 = stream.Read<P1>(codecOptions);
			block.p2 = stream.Read<P2>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,>.Call<P0, P1, P2>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2>.Call)d)(p0, p1, p2);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,>.InfoCall<P0, P1, P2>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2>.InfoCall)d)(p0, p1, p2, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,>.InfoRoutine<P0, P1, P2>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2>.InfoRoutine)d)(p0, p1, p2, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,>.Routine<P0, P1, P2>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2>.Routine)d)(p0, p1, p2);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,>.StreamCall<P0, P1, P2>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2>.StreamCall)d)(p0, p1, p2, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,>.StreamInfoCall<P0, P1, P2>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2>.StreamInfoCall)d)(p0, p1, p2, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,>.StreamInfoRoutine<P0, P1, P2>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2>.StreamInfoRoutine)d)(p0, p1, p2, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,>.StreamRoutine<P0, P1, P2>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2>.StreamRoutine)d)(p0, p1, p2, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2>.Block block = (NGC.callf<P0, P1, P2>.Block)value;
			stream.Write<P0>(block.p0, codecOptions);
			stream.Write<P1>(block.p1, codecOptions);
			stream.Write<P2>(block.p2, codecOptions);
		}

		public struct Block
		{
			public P0 p0;

			public P1 p1;

			public P2 p2;
		}

		public delegate void Call(P0 p0, P1 p1, P2 p2);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0, P1, P2>.Executer.Singleton = new NGC.callf<P0, P1, P2>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, P1 p1, P2 p2, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, P1 p1, P2 p2, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0, P1 p1, P2 p2);

		public delegate void StreamCall(P0 p0, P1 p1, P2 p2, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, P1 p1, P2 p2, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, P1 p1, P2 p2, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, P1 p1, P2 p2, uLink.BitStream stream);
	}

	public static class callf<P0, P1, P2, P3>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0, P1, P2, P3>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0, P1, P2, P3>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0, P1, P2, P3>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0, P1, P2, P3>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3>.Block block = new NGC.callf<P0, P1, P2, P3>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			block.p1 = stream.Read<P1>(codecOptions);
			block.p2 = stream.Read<P2>(codecOptions);
			block.p3 = stream.Read<P3>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,>.Call<P0, P1, P2, P3>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3>.Call)d)(p0, p1, p2, p3);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,>.InfoCall<P0, P1, P2, P3>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3>.InfoCall)d)(p0, p1, p2, p3, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,>.InfoRoutine<P0, P1, P2, P3>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3>.InfoRoutine)d)(p0, p1, p2, p3, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,>.Routine<P0, P1, P2, P3>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3>.Routine)d)(p0, p1, p2, p3);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,>.StreamCall<P0, P1, P2, P3>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3>.StreamCall)d)(p0, p1, p2, p3, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,>.StreamInfoCall<P0, P1, P2, P3>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3>.StreamInfoCall)d)(p0, p1, p2, p3, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,>.StreamInfoRoutine<P0, P1, P2, P3>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3>.StreamInfoRoutine)d)(p0, p1, p2, p3, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,>.StreamRoutine<P0, P1, P2, P3>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3>.StreamRoutine)d)(p0, p1, p2, p3, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3>.Block block = (NGC.callf<P0, P1, P2, P3>.Block)value;
			stream.Write<P0>(block.p0, codecOptions);
			stream.Write<P1>(block.p1, codecOptions);
			stream.Write<P2>(block.p2, codecOptions);
			stream.Write<P3>(block.p3, codecOptions);
		}

		public struct Block
		{
			public P0 p0;

			public P1 p1;

			public P2 p2;

			public P3 p3;
		}

		public delegate void Call(P0 p0, P1 p1, P2 p2, P3 p3);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0, P1, P2, P3>.Executer.Singleton = new NGC.callf<P0, P1, P2, P3>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, P1 p1, P2 p2, P3 p3, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0, P1 p1, P2 p2, P3 p3);

		public delegate void StreamCall(P0 p0, P1 p1, P2 p2, P3 p3, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, P1 p1, P2 p2, P3 p3, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, P1 p1, P2 p2, P3 p3, uLink.BitStream stream);
	}

	public static class callf<P0, P1, P2, P3, P4>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0, P1, P2, P3, P4>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0, P1, P2, P3, P4>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0, P1, P2, P3, P4>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0, P1, P2, P3, P4>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4>.Block block = new NGC.callf<P0, P1, P2, P3, P4>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			block.p1 = stream.Read<P1>(codecOptions);
			block.p2 = stream.Read<P2>(codecOptions);
			block.p3 = stream.Read<P3>(codecOptions);
			block.p4 = stream.Read<P4>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,>.Call<P0, P1, P2, P3, P4>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4>.Call)d)(p0, p1, p2, p3, p4);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,>.InfoCall<P0, P1, P2, P3, P4>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4>.InfoCall)d)(p0, p1, p2, p3, p4, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,>.InfoRoutine<P0, P1, P2, P3, P4>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4>.InfoRoutine)d)(p0, p1, p2, p3, p4, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,>.Routine<P0, P1, P2, P3, P4>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4>.Routine)d)(p0, p1, p2, p3, p4);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,>.StreamCall<P0, P1, P2, P3, P4>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4>.StreamCall)d)(p0, p1, p2, p3, p4, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,>.StreamInfoCall<P0, P1, P2, P3, P4>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4>.StreamInfoCall)d)(p0, p1, p2, p3, p4, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,>.StreamInfoRoutine<P0, P1, P2, P3, P4>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4>.StreamInfoRoutine)d)(p0, p1, p2, p3, p4, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,>.StreamRoutine<P0, P1, P2, P3, P4>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4>.StreamRoutine)d)(p0, p1, p2, p3, p4, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4>.Block block = (NGC.callf<P0, P1, P2, P3, P4>.Block)value;
			stream.Write<P0>(block.p0, codecOptions);
			stream.Write<P1>(block.p1, codecOptions);
			stream.Write<P2>(block.p2, codecOptions);
			stream.Write<P3>(block.p3, codecOptions);
			stream.Write<P4>(block.p4, codecOptions);
		}

		public struct Block
		{
			public P0 p0;

			public P1 p1;

			public P2 p2;

			public P3 p3;

			public P4 p4;
		}

		public delegate void Call(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0, P1, P2, P3, P4>.Executer.Singleton = new NGC.callf<P0, P1, P2, P3, P4>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4);

		public delegate void StreamCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, uLink.BitStream stream);
	}

	public static class callf<P0, P1, P2, P3, P4, P5>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0, P1, P2, P3, P4, P5>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0, P1, P2, P3, P4, P5>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0, P1, P2, P3, P4, P5>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			block.p1 = stream.Read<P1>(codecOptions);
			block.p2 = stream.Read<P2>(codecOptions);
			block.p3 = stream.Read<P3>(codecOptions);
			block.p4 = stream.Read<P4>(codecOptions);
			block.p5 = stream.Read<P5>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,>.Call<P0, P1, P2, P3, P4, P5>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5>.Call)d)(p0, p1, p2, p3, p4, p5);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,>.InfoCall<P0, P1, P2, P3, P4, P5>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5>.InfoCall)d)(p0, p1, p2, p3, p4, p5, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,>.InfoRoutine<P0, P1, P2, P3, P4, P5>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5>.InfoRoutine)d)(p0, p1, p2, p3, p4, p5, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,>.Routine<P0, P1, P2, P3, P4, P5>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5>.Routine)d)(p0, p1, p2, p3, p4, p5);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,>.StreamCall<P0, P1, P2, P3, P4, P5>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5>.StreamCall)d)(p0, p1, p2, p3, p4, p5, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,>.StreamInfoCall<P0, P1, P2, P3, P4, P5>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5>.StreamInfoCall)d)(p0, p1, p2, p3, p4, p5, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,>.StreamInfoRoutine<P0, P1, P2, P3, P4, P5>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5>.StreamInfoRoutine)d)(p0, p1, p2, p3, p4, p5, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,>.StreamRoutine<P0, P1, P2, P3, P4, P5>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5>.StreamRoutine)d)(p0, p1, p2, p3, p4, p5, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5>.Block block = (NGC.callf<P0, P1, P2, P3, P4, P5>.Block)value;
			stream.Write<P0>(block.p0, codecOptions);
			stream.Write<P1>(block.p1, codecOptions);
			stream.Write<P2>(block.p2, codecOptions);
			stream.Write<P3>(block.p3, codecOptions);
			stream.Write<P4>(block.p4, codecOptions);
			stream.Write<P5>(block.p5, codecOptions);
		}

		public struct Block
		{
			public P0 p0;

			public P1 p1;

			public P2 p2;

			public P3 p3;

			public P4 p4;

			public P5 p5;
		}

		public delegate void Call(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0, P1, P2, P3, P4, P5>.Executer.Singleton = new NGC.callf<P0, P1, P2, P3, P4, P5>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);

		public delegate void StreamCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, uLink.BitStream stream);
	}

	public static class callf<P0, P1, P2, P3, P4, P5, P6>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			block.p1 = stream.Read<P1>(codecOptions);
			block.p2 = stream.Read<P2>(codecOptions);
			block.p3 = stream.Read<P3>(codecOptions);
			block.p4 = stream.Read<P4>(codecOptions);
			block.p5 = stream.Read<P5>(codecOptions);
			block.p6 = stream.Read<P6>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,>.Call<P0, P1, P2, P3, P4, P5, P6>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Call)d)(p0, p1, p2, p3, p4, p5, p6);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,>.InfoCall<P0, P1, P2, P3, P4, P5, P6>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6>.InfoCall)d)(p0, p1, p2, p3, p4, p5, p6, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,>.InfoRoutine<P0, P1, P2, P3, P4, P5, P6>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6>.InfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,>.Routine<P0, P1, P2, P3, P4, P5, P6>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Routine)d)(p0, p1, p2, p3, p4, p5, p6);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,>.StreamCall<P0, P1, P2, P3, P4, P5, P6>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6>.StreamCall)d)(p0, p1, p2, p3, p4, p5, p6, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,>.StreamInfoCall<P0, P1, P2, P3, P4, P5, P6>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6>.StreamInfoCall)d)(p0, p1, p2, p3, p4, p5, p6, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,>.StreamInfoRoutine<P0, P1, P2, P3, P4, P5, P6>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6>.StreamInfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,>.StreamRoutine<P0, P1, P2, P3, P4, P5, P6>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6>.StreamRoutine)d)(p0, p1, p2, p3, p4, p5, p6, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Block block = (NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Block)value;
			stream.Write<P0>(block.p0, codecOptions);
			stream.Write<P1>(block.p1, codecOptions);
			stream.Write<P2>(block.p2, codecOptions);
			stream.Write<P3>(block.p3, codecOptions);
			stream.Write<P4>(block.p4, codecOptions);
			stream.Write<P5>(block.p5, codecOptions);
			stream.Write<P6>(block.p6, codecOptions);
		}

		public struct Block
		{
			public P0 p0;

			public P1 p1;

			public P2 p2;

			public P3 p3;

			public P4 p4;

			public P5 p5;

			public P6 p6;
		}

		public delegate void Call(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Executer.Singleton = new NGC.callf<P0, P1, P2, P3, P4, P5, P6>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6);

		public delegate void StreamCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, uLink.BitStream stream);
	}

	public static class callf<P0, P1, P2, P3, P4, P5, P6, P7>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			block.p1 = stream.Read<P1>(codecOptions);
			block.p2 = stream.Read<P2>(codecOptions);
			block.p3 = stream.Read<P3>(codecOptions);
			block.p4 = stream.Read<P4>(codecOptions);
			block.p5 = stream.Read<P5>(codecOptions);
			block.p6 = stream.Read<P6>(codecOptions);
			block.p7 = stream.Read<P7>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,>.Call<P0, P1, P2, P3, P4, P5, P6, P7>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Call)d)(p0, p1, p2, p3, p4, p5, p6, p7);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,>.InfoCall<P0, P1, P2, P3, P4, P5, P6, P7>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.InfoCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,>.InfoRoutine<P0, P1, P2, P3, P4, P5, P6, P7>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.InfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,>.Routine<P0, P1, P2, P3, P4, P5, P6, P7>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Routine)d)(p0, p1, p2, p3, p4, p5, p6, p7);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,>.StreamCall<P0, P1, P2, P3, P4, P5, P6, P7>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.StreamCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,>.StreamInfoCall<P0, P1, P2, P3, P4, P5, P6, P7>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.StreamInfoCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,>.StreamInfoRoutine<P0, P1, P2, P3, P4, P5, P6, P7>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.StreamInfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,>.StreamRoutine<P0, P1, P2, P3, P4, P5, P6, P7>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.StreamRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Block block = (NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Block)value;
			stream.Write<P0>(block.p0, codecOptions);
			stream.Write<P1>(block.p1, codecOptions);
			stream.Write<P2>(block.p2, codecOptions);
			stream.Write<P3>(block.p3, codecOptions);
			stream.Write<P4>(block.p4, codecOptions);
			stream.Write<P5>(block.p5, codecOptions);
			stream.Write<P6>(block.p6, codecOptions);
			stream.Write<P7>(block.p7, codecOptions);
		}

		public struct Block
		{
			public P0 p0;

			public P1 p1;

			public P2 p2;

			public P3 p3;

			public P4 p4;

			public P5 p5;

			public P6 p6;

			public P7 p7;
		}

		public delegate void Call(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Executer.Singleton = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7);

		public delegate void StreamCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, uLink.BitStream stream);
	}

	public static class callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>
	{
		public static NGC.IExecuter Exec
		{
			get
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Executer.Singleton;
			}
		}

		static callf()
		{
			BitStreamCodec.Add<NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Block>(new BitStreamCodec.Deserializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Deserializer), new BitStreamCodec.Serializer(NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Serializer));
		}

		private static object Deserializer(uLink.BitStream stream, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Block block = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Block();
			block.p0 = stream.Read<P0>(codecOptions);
			block.p1 = stream.Read<P1>(codecOptions);
			block.p2 = stream.Read<P2>(codecOptions);
			block.p3 = stream.Read<P3>(codecOptions);
			block.p4 = stream.Read<P4>(codecOptions);
			block.p5 = stream.Read<P5>(codecOptions);
			block.p6 = stream.Read<P6>(codecOptions);
			block.p7 = stream.Read<P7>(codecOptions);
			block.p8 = stream.Read<P8>(codecOptions);
			return block;
		}

		public static void InvokeCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,>.Call<P0, P1, P2, P3, P4, P5, P6, P7, P8>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Call)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8);
		}

		public static void InvokeInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,>.InfoCall<P0, P1, P2, P3, P4, P5, P6, P7, P8>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.InfoCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, info);
		}

		public static IEnumerator InvokeInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,>.InfoRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.InfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, info);
		}

		public static IEnumerator InvokeRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,>.Routine<P0, P1, P2, P3, P4, P5, P6, P7, P8>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Routine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8);
		}

		public static void InvokeStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,>.StreamCall<P0, P1, P2, P3, P4, P5, P6, P7, P8>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.StreamCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, stream);
		}

		public static void InvokeStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,>.StreamInfoCall<P0, P1, P2, P3, P4, P5, P6, P7, P8>), instance, method, true);
			}
			((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.StreamInfoCall)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, info, stream);
		}

		public static IEnumerator InvokeStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,>.StreamInfoRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.StreamInfoRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, info, stream);
		}

		public static IEnumerator InvokeStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
		{
			P0 p0 = stream.Read<P0>(new object[0]);
			P1 p1 = stream.Read<P1>(new object[0]);
			P2 p2 = stream.Read<P2>(new object[0]);
			P3 p3 = stream.Read<P3>(new object[0]);
			P4 p4 = stream.Read<P4>(new object[0]);
			P5 p5 = stream.Read<P5>(new object[0]);
			P6 p6 = stream.Read<P6>(new object[0]);
			P7 p7 = stream.Read<P7>(new object[0]);
			P8 p8 = stream.Read<P8>(new object[0]);
			if (d == null)
			{
				d = Delegate.CreateDelegate(typeof(NGC.callf<,,,,,,,,>.StreamRoutine<P0, P1, P2, P3, P4, P5, P6, P7, P8>), instance, method, true);
			}
			return ((NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.StreamRoutine)d)(p0, p1, p2, p3, p4, p5, p6, p7, p8, stream);
		}

		private static void Serializer(uLink.BitStream stream, object value, params object[] codecOptions)
		{
			NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Block block = (NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Block)value;
			stream.Write<P0>(block.p0, codecOptions);
			stream.Write<P1>(block.p1, codecOptions);
			stream.Write<P2>(block.p2, codecOptions);
			stream.Write<P3>(block.p3, codecOptions);
			stream.Write<P4>(block.p4, codecOptions);
			stream.Write<P5>(block.p5, codecOptions);
			stream.Write<P6>(block.p6, codecOptions);
			stream.Write<P7>(block.p7, codecOptions);
			stream.Write<P8>(block.p8, codecOptions);
		}

		public struct Block
		{
			public P0 p0;

			public P1 p1;

			public P2 p2;

			public P3 p3;

			public P4 p4;

			public P5 p5;

			public P6 p6;

			public P7 p7;

			public P8 p8;
		}

		public delegate void Call(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8);

		private sealed class Executer : NGC.IExecuter
		{
			public readonly static NGC.IExecuter Singleton;

			static Executer()
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Executer.Singleton = new NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.Executer();
			}

			public Executer()
			{
			}

			public void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.InvokeCall(stream, ref d, method, instance);
			}

			public void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.InvokeInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.InvokeInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.InvokeRoutine(stream, ref d, method, instance);
			}

			public void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.InvokeStreamCall(stream, ref d, method, instance);
			}

			public void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.InvokeStreamInfoCall(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.InvokeStreamInfoRoutine(stream, ref d, method, instance, info);
			}

			public IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance)
			{
				return NGC.callf<P0, P1, P2, P3, P4, P5, P6, P7, P8>.InvokeStreamRoutine(stream, ref d, method, instance);
			}
		}

		public delegate void InfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, uLink.NetworkMessageInfo info);

		public delegate IEnumerator InfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, uLink.NetworkMessageInfo info);

		public delegate IEnumerator Routine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8);

		public delegate void StreamCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, uLink.BitStream stream);

		public delegate void StreamInfoCall(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamInfoRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, uLink.NetworkMessageInfo info, uLink.BitStream stream);

		public delegate IEnumerator StreamRoutine(P0 p0, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, uLink.BitStream stream);
	}

	public delegate void EventCallback(NGCView view);

	private static class Global
	{
		public readonly static Dictionary<ushort, NGC> byGroup;

		public readonly static List<NGC> all;

		static Global()
		{
			NGC.Global.byGroup = new Dictionary<ushort, NGC>();
			NGC.Global.all = new List<NGC>();
		}
	}

	public interface IExecuter
	{
		void ExecuteCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance);

		void ExecuteInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info);

		IEnumerator ExecuteInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info);

		IEnumerator ExecuteRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance);

		void ExecuteStreamCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance);

		void ExecuteStreamInfoCall(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info);

		IEnumerator ExecuteStreamInfoRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance, uLink.NetworkMessageInfo info);

		IEnumerator ExecuteStreamRoutine(uLink.BitStream stream, ref Delegate d, MethodInfo method, UnityEngine.MonoBehaviour instance);
	}

	public sealed class Prefab
	{
		[NonSerialized]
		public readonly string contentPath;

		[NonSerialized]
		public readonly int key;

		[NonSerialized]
		public readonly string handle;

		[NonSerialized]
		private NGC.Prefab.Installation _installation;

		private Dictionary<string, int> cachedMessageIndices;

		private WeakReference weakReference;

		public NGC.Prefab.Installation installation
		{
			get
			{
				if (this._installation == null && !this.prefab)
				{
					throw new InvalidOperationException("Could not get installation because prefab could not load");
				}
				return this._installation;
			}
		}

		public NGCView prefab
		{
			get
			{
				NGCView nGCView;
				if (this.weakReference != null)
				{
					NGCView target = (NGCView)this.weakReference.Target;
					nGCView = target;
					if (target && this.weakReference.IsAlive)
					{
						return nGCView;
					}
				}
				if (!Bundling.Load<NGCView>(this.contentPath, typeof(NGCView), out nGCView))
				{
					throw new MissingReferenceException(string.Concat("Could not load NGCView at ", this.contentPath));
				}
				if (this._installation == null)
				{
					this._installation = NGC.Prefab.Installation.Create(nGCView);
				}
				this.weakReference = new WeakReference(nGCView);
				return nGCView;
			}
		}

		private Prefab(string contentPath, int key, string handle)
		{
			this.contentPath = contentPath;
			this.key = key;
			this.handle = handle;
		}

		internal NetworkFlags DefaultNetworkFlags(int messageIndex)
		{
			return this.installation.methods[messageIndex].defaultNetworkFlags;
		}

		public int MessageIndex(string message)
		{
			int num;
			if (this.cachedMessageIndices != null && this.cachedMessageIndices.TryGetValue(message, out num))
			{
				return num;
			}
			int num1 = this.MessageIndexFind(message);
			if (num1 == -1)
			{
				throw new ArgumentException(message, "message");
			}
			if (this.cachedMessageIndices == null)
			{
				this.cachedMessageIndices = new Dictionary<string, int>();
			}
			this.cachedMessageIndices[message] = num1;
			return num1;
		}

		private int MessageIndexFind(string message)
		{
			int num = message.LastIndexOf(':');
			if (num != -1)
			{
				for (int i = 0; i < (int)this._installation.methods.Length; i++)
				{
					if (string.Compare(this._installation.methods[i].method.Name, 0, message, num + 1, message.Length - (num + 1)) == 0 && string.Compare(this._installation.methods[i].method.DeclaringType.FullName, 0, message, 0, num) == 0)
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = 0; j < (int)this._installation.methods.Length; j++)
				{
					if (this._installation.methods[j].method.Name == message)
					{
						return j;
					}
				}
			}
			return -1;
		}

		public sealed class Installation
		{
			public readonly NGC.Prefab.Installation.Method[] methods;

			public readonly ushort[] methodScriptIndices;

			private readonly static Dictionary<Type, NGC.Prefab.Installation.Method[]> methodsForType;

			static Installation()
			{
				NGC.Prefab.Installation.methodsForType = new Dictionary<Type, NGC.Prefab.Installation.Method[]>();
			}

			private Installation(NGC.Prefab.Installation.Method[] methods, ushort[] methodScriptIndices)
			{
				this.methods = methods;
				this.methodScriptIndices = methodScriptIndices;
			}

			public static NGC.Prefab.Installation Create(NGCView view)
			{
				NGC.Prefab.Installation.Method[] array;
				int length = 0;
				List<NGC.Prefab.Installation.Method[]> methodArrays = new List<NGC.Prefab.Installation.Method[]>();
				UnityEngine.MonoBehaviour[] monoBehaviourArray = view.scripts;
				for (int i = 0; i < (int)monoBehaviourArray.Length; i++)
				{
					Type type = monoBehaviourArray[i].GetType();
					if (!NGC.Prefab.Installation.methodsForType.TryGetValue(type, out array))
					{
						List<NGC.Prefab.Installation.Method> methods = new List<NGC.Prefab.Installation.Method>();
						MethodInfo[] methodInfoArray = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						for (int j = 0; j < (int)methodInfoArray.Length; j++)
						{
							MethodInfo methodInfo = methodInfoArray[j];
							bool flag = false;
							if (methodInfo.IsDefined(typeof(RPC), true))
							{
								if (!methodInfo.IsDefined(typeof(NGCRPCSkipAttribute), false) || methodInfo.IsDefined(typeof(NGCRPCAttribute), true))
								{
									flag = true;
								}
							}
							else if (methodInfo.IsDefined(typeof(NGCRPCAttribute), true))
							{
								flag = true;
							}
							if (flag)
							{
								methods.Add(NGC.Prefab.Installation.Method.Create(methodInfo));
							}
						}
						methods.Sort((NGC.Prefab.Installation.Method x, NGC.Prefab.Installation.Method y) => x.method.Name.CompareTo(y.method.Name));
						array = methods.ToArray();
						NGC.Prefab.Installation.methodsForType[type] = array;
					}
					length = length + (int)array.Length;
					methodArrays.Add(array);
				}
				NGC.Prefab.Installation.Method[] methodArray = new NGC.Prefab.Installation.Method[length];
				ushort[] numArray = new ushort[length];
				int num = 0;
				ushort num1 = 0;
				foreach (NGC.Prefab.Installation.Method[] methodArray1 in methodArrays)
				{
					NGC.Prefab.Installation.Method[] methodArray2 = methodArray1;
					for (int k = 0; k < (int)methodArray2.Length; k++)
					{
						NGC.Prefab.Installation.Method method = methodArray2[k];
						methodArray[num] = method;
						numArray[num] = num1;
						num++;
					}
					num1 = (ushort)(num1 + 1);
				}
				return new NGC.Prefab.Installation(methodArray, numArray);
			}

			public sealed class Instance
			{
				public readonly Delegate[] delegates;

				public Instance(NGC.Prefab.Installation installation)
				{
					this.delegates = new Delegate[(int)installation.methods.Length];
				}

				public void Invoke(NGC.Procedure procedure)
				{
					procedure.view.prefab.installation.methods[procedure.message].Invoke(ref this.delegates[procedure.message], procedure, procedure.view.prefab.installation.methodScriptIndices[procedure.message]);
				}
			}

			public struct Method
			{
				private const byte FLAG_INFO = 1;

				private const byte FLAG_STREAM = 2;

				private const byte FLAG_ENUMERATOR = 4;

				private const byte FLAG_FORCE_UNBUFFERED = 8;

				private const byte FLAG_FORCE_INSECURE = 16;

				private const byte FLAG_FORCE_NO_TIMESTAMP = 32;

				private const byte FLAG_FORCE_UNRELIABLE = 64;

				private const byte FLAG_FORCE_TYPE_UNSAFE = 128;

				private const byte INVOKE_FLAGS = 7;

				public readonly MethodInfo method;

				public readonly byte flags;

				private readonly NGC.IExecuter executer;

				public NetworkFlags defaultNetworkFlags
				{
					get
					{
						NetworkFlags networkFlag = NetworkFlags.Normal;
						if ((this.flags & 33) != 1)
						{
							networkFlag = (NetworkFlags)((byte)(networkFlag | NetworkFlags.NoTimestamp));
						}
						if ((this.flags & 128) == 128)
						{
							networkFlag = (NetworkFlags)((byte)(networkFlag | NetworkFlags.TypeUnsafe));
						}
						if ((this.flags & 64) == 16)
						{
							networkFlag = (NetworkFlags)((byte)(networkFlag | NetworkFlags.Unreliable | NetworkFlags.Unbuffered));
						}
						else if ((this.flags & 8) == 8)
						{
							networkFlag = (NetworkFlags)((byte)(networkFlag | NetworkFlags.Unbuffered));
						}
						if ((this.flags & 16) == 16)
						{
							networkFlag = (NetworkFlags)((byte)(networkFlag | NetworkFlags.Unencrypted));
						}
						return networkFlag;
					}
				}

				private Method(MethodInfo method, byte flags, NGC.IExecuter executer)
				{
					this.method = method;
					this.flags = flags;
					this.executer = executer;
				}

				public static NGC.Prefab.Installation.Method Create(MethodInfo info)
				{
					byte num;
					Type returnType = info.ReturnType;
					if (returnType != typeof(void))
					{
						if (returnType != typeof(IEnumerator))
						{
							throw new InvalidOperationException(string.Format("RPC {0} returns a unhandled type: {1}", info, returnType));
						}
						num = 4;
					}
					else
					{
						num = 0;
					}
					ParameterInfo[] parameters = info.GetParameters();
					for (int i = 0; i < (int)parameters.Length; i++)
					{
						if (parameters[i].IsOut || parameters[i].IsIn)
						{
							throw new InvalidOperationException(string.Format("RPC method {0} has a illegal parameter {1}", info, parameters[i]));
						}
					}
					int length = (int)parameters.Length;
					if (length > 0)
					{
						Type parameterType = parameters[(int)parameters.Length - 1].ParameterType;
						Type type = parameterType;
						if (parameterType == typeof(uLink.NetworkMessageInfo))
						{
							length--;
							if (length > 0)
							{
								Type parameterType1 = parameters[length - 1].ParameterType;
								type = parameterType1;
								if (parameterType1 != typeof(uLink.BitStream))
								{
									goto Label1;
								}
								length--;
								num = (byte)(num | 3);
								goto Label0;
							}
						Label1:
							num = (byte)(num | 1);
						Label0:
						}
						else if (type == typeof(uLink.BitStream))
						{
							length--;
							num = (byte)(num | 2);
						}
					}
					Type[] typeArray = new Type[length];
					for (int j = 0; j < length; j++)
					{
						typeArray[j] = parameters[j].ParameterType;
					}
					NGC.IExecuter executer = NGC.FindExecuter(typeArray);
					if (executer == null)
					{
						throw new InvalidProgramException();
					}
					return new NGC.Prefab.Installation.Method(info, num, executer);
				}

				public void Invoke(ref Delegate d, NGC.Procedure procedure, ushort scriptIndex)
				{
					IEnumerator enumerator;
					UnityEngine.MonoBehaviour monoBehaviour = procedure.view.scripts[scriptIndex];
					switch (this.flags & 7)
					{
						case 0:
						{
							this.executer.ExecuteCall(procedure.CreateBitStream(), ref d, this.method, monoBehaviour);
							return;
						}
						case 1:
						{
							this.executer.ExecuteInfoCall(procedure.CreateBitStream(), ref d, this.method, monoBehaviour, procedure.info);
							return;
						}
						case 2:
						{
							this.executer.ExecuteStreamCall(procedure.CreateBitStream(), ref d, this.method, monoBehaviour);
							return;
						}
						case 3:
						{
							this.executer.ExecuteStreamInfoCall(procedure.CreateBitStream(), ref d, this.method, monoBehaviour, procedure.info);
							return;
						}
						case 4:
						{
							enumerator = this.executer.ExecuteRoutine(procedure.CreateBitStream(), ref d, this.method, monoBehaviour);
							break;
						}
						case 5:
						{
							enumerator = this.executer.ExecuteInfoRoutine(procedure.CreateBitStream(), ref d, this.method, monoBehaviour, procedure.info);
							break;
						}
						case 6:
						{
							enumerator = this.executer.ExecuteStreamRoutine(procedure.CreateBitStream(), ref d, this.method, monoBehaviour);
							break;
						}
						case 7:
						{
							enumerator = this.executer.ExecuteStreamInfoRoutine(procedure.CreateBitStream(), ref d, this.method, monoBehaviour, procedure.info);
							break;
						}
						default:
						{
							throw new NotImplementedException((this.flags & 7).ToString());
						}
					}
					if (enumerator == null)
					{
						return;
					}
					try
					{
						monoBehaviour.StartCoroutine(enumerator);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception, monoBehaviour);
					}
				}
			}
		}

		public static class Register
		{
			private readonly static Dictionary<int, NGC.Prefab> PrefabByIndex;

			private readonly static Dictionary<string, NGC.Prefab> PrefabByName;

			private readonly static List<NGC.Prefab> Prefabs;

			static Register()
			{
				NGC.Prefab.Register.PrefabByIndex = new Dictionary<int, NGC.Prefab>();
				NGC.Prefab.Register.PrefabByName = new Dictionary<string, NGC.Prefab>();
				NGC.Prefab.Register.Prefabs = new List<NGC.Prefab>();
			}

			public static bool Add(string contentPath, int index, string handle)
			{
				bool flag;
				try
				{
					NGC.Prefab prefab = new NGC.Prefab(contentPath, index, handle);
					NGC.Prefab.Register.PrefabByIndex.Add(index, prefab);
					try
					{
						NGC.Prefab.Register.PrefabByName.Add(handle, prefab);
					}
					catch
					{
						NGC.Prefab.Register.PrefabByIndex.Remove(index);
						throw;
					}
					NGC.Prefab.Register.Prefabs.Add(prefab);
					flag = true;
				}
				catch
				{
					flag = false;
				}
				return flag;
			}

			public static bool Find(int index, out NGC.Prefab prefab)
			{
				return NGC.Prefab.Register.PrefabByIndex.TryGetValue(index, out prefab);
			}

			public static bool Find(string handle, out NGC.Prefab prefab)
			{
				return NGC.Prefab.Register.PrefabByName.TryGetValue(handle, out prefab);
			}

			public static string FindName(int iIndex)
			{
				return NGC.Prefab.Register.PrefabByIndex[iIndex].handle;
			}
		}
	}

	public sealed class Procedure
	{
		public NGC outer;

		public int target;

		public int message;

		public byte[] data;

		public int dataLength;

		public uLink.NetworkMessageInfo info;

		public NGCView view;

		public Procedure()
		{
		}

		public bool Call()
		{
			bool flag;
			if (!this.view)
			{
				try
				{
					this.view = this.outer.views[(ushort)this.target];
					try
					{
						this.view.install.Invoke(this);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception, this.view);
					}
					return true;
				}
				catch (KeyNotFoundException keyNotFoundException)
				{
					flag = false;
				}
				return flag;
			}
			try
			{
				this.view.install.Invoke(this);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this.view);
			}
			return true;
		}

		public uLink.BitStream CreateBitStream()
		{
			if (this.dataLength == 0)
			{
				return new uLink.BitStream(false);
			}
			return new uLink.BitStream(this.data, false);
		}
	}

	private struct RPCName
	{
		public readonly string name;

		public readonly NetworkFlags flags;

		public RPCName(NGCView view, int message, string name, NetworkFlags flags)
		{
			this.name = name;
			this.flags = flags;
		}
	}
}