using Facepunch;
using System;
using System.Runtime.InteropServices;
using uLink;
using UnityEngine;

[StructLayout(LayoutKind.Explicit)]
public struct NetEntityID : IEquatable<uLink.NetworkViewID>, IEquatable<NetEntityID>, IComparable<uLink.NetworkViewID>, IComparable<NetEntityID>
{
	[FieldOffset(0)]
	private uLink.NetworkViewID _viewID;

	[FieldOffset(0)]
	private ushort p2;

	[FieldOffset(2)]
	private ushort p1;

	[FieldOffset(0)]
	private int v;

	[FieldOffset(-1)]
	private readonly static BitStreamCodec.Serializer serializer;

	[FieldOffset(-1)]
	private readonly static BitStreamCodec.Deserializer deserializer;

	public Collider collider
	{
		get
		{
			Collider collider;
			UnityEngine.MonoBehaviour monoBehaviour = this.view;
			if (!monoBehaviour)
			{
				collider = null;
			}
			else
			{
				collider = monoBehaviour.collider;
			}
			return collider;
		}
	}

	public GameObject gameObject
	{
		get
		{
			GameObject gameObject;
			UnityEngine.MonoBehaviour monoBehaviour = this.view;
			if (!monoBehaviour)
			{
				gameObject = null;
			}
			else
			{
				gameObject = monoBehaviour.gameObject;
			}
			return gameObject;
		}
	}

	public int id
	{
		get
		{
			return this.v;
		}
	}

	public IDBase idBase
	{
		get
		{
			if (this.p1 != 0)
			{
				NGCView nGCView = NGC.Find(this.v);
				if (!nGCView)
				{
					return null;
				}
				return IDBase.Get(nGCView.gameObject);
			}
			if (this.p2 == 0)
			{
				return null;
			}
			Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(this._viewID);
			if (!networkView)
			{
				return null;
			}
			return IDBase.Get(networkView);
		}
	}

	public bool isAllocated
	{
		get
		{
			if (this.p1 != 0)
			{
				return true;
			}
			return this._viewID.isAllocated;
		}
	}

	public bool isManual
	{
		get
		{
			if (this.p1 != 0)
			{
				return false;
			}
			return this._viewID.isManual;
		}
	}

	public bool isMine
	{
		get
		{
			if (this.p1 != 0)
			{
				return false;
			}
			return this._viewID.isMine;
		}
	}

	public bool isNet
	{
		get
		{
			return (this.p1 != 0 ? false : this._viewID != uLink.NetworkViewID.unassigned);
		}
	}

	public bool isNGC
	{
		get
		{
			return this.p1 != 0;
		}
	}

	public bool isUnassigned
	{
		get
		{
			return this.v == 0;
		}
	}

	public IDMain main
	{
		get
		{
			if (this.p1 != 0)
			{
				NGCView nGCView = NGC.Find(this.v);
				if (!nGCView)
				{
					return null;
				}
				return IDBase.GetMain(nGCView.gameObject);
			}
			if (this.p2 == 0)
			{
				return null;
			}
			Facepunch.NetworkView networkView = Facepunch.NetworkView.Find(this._viewID);
			if (!networkView)
			{
				return null;
			}
			IDBase dBase = IDBase.Get(networkView);
			if (!dBase)
			{
				return null;
			}
			return dBase.idMain;
		}
	}

	public Facepunch.NetworkView networkView
	{
		get
		{
			if (this.p1 != 0)
			{
				return null;
			}
			return Facepunch.NetworkView.Find(this._viewID);
		}
	}

	public NGCView ngcView
	{
		get
		{
			if (this.p1 == 0)
			{
				return null;
			}
			return NGC.Find(this.v);
		}
	}

	public uLink.NetworkPlayer owner
	{
		get
		{
			if (this.p1 != 0)
			{
				return uLink.NetworkPlayer.server;
			}
			return this._viewID.owner;
		}
	}

	public Renderer renderer
	{
		get
		{
			Renderer renderer;
			UnityEngine.MonoBehaviour monoBehaviour = this.view;
			if (!monoBehaviour)
			{
				renderer = null;
			}
			else
			{
				renderer = monoBehaviour.renderer;
			}
			return renderer;
		}
	}

	public Rigidbody rigidbody
	{
		get
		{
			Rigidbody rigidbody;
			UnityEngine.MonoBehaviour monoBehaviour = this.view;
			if (!monoBehaviour)
			{
				rigidbody = null;
			}
			else
			{
				rigidbody = monoBehaviour.rigidbody;
			}
			return rigidbody;
		}
	}

	public Transform transform
	{
		get
		{
			Transform transforms;
			UnityEngine.MonoBehaviour monoBehaviour = this.view;
			if (!monoBehaviour)
			{
				transforms = null;
			}
			else
			{
				transforms = monoBehaviour.transform;
			}
			return transforms;
		}
	}

	public static NetEntityID unassigned
	{
		get
		{
			return new NetEntityID();
		}
	}

	public UnityEngine.MonoBehaviour view
	{
		get
		{
			if (this.p1 != 0)
			{
				return NGC.Find(this.v);
			}
			if (this.p2 == 0)
			{
				return null;
			}
			return Facepunch.NetworkView.Find(this._viewID);
		}
	}

	static NetEntityID()
	{
		NetEntityID.serializer = new BitStreamCodec.Serializer(NetEntityID.Serializer);
		NetEntityID.deserializer = new BitStreamCodec.Deserializer(NetEntityID.Deserializer);
		BitStreamCodec.AddAndMakeArray<NetEntityID>(NetEntityID.deserializer, NetEntityID.serializer);
	}

	public NetEntityID(NGCView view)
	{
		this = new NetEntityID();
		if (view)
		{
			this.v = view.id;
		}
	}

	public NetEntityID(uLink.NetworkView view)
	{
		this = new NetEntityID();
		if (view)
		{
			this._viewID = view.viewID;
		}
	}

	public NetEntityID(uLink.NetworkViewID viewID)
	{
		this = new NetEntityID()
		{
			_viewID = viewID
		};
	}

	public int CompareTo(NetEntityID other)
	{
		return this.v.CompareTo(other.v);
	}

	public int CompareTo(uLink.NetworkViewID other)
	{
		return this.v.CompareTo(other.id);
	}

	private static object Deserializer(uLink.BitStream bs, params object[] codecOptions)
	{
		NetEntityID netEntityID = new NetEntityID()
		{
			p1 = bs.Read<ushort>(codecOptions)
		};
		if (netEntityID.p1 != 0)
		{
			netEntityID.p2 = bs.Read<ushort>(codecOptions);
		}
		else
		{
			netEntityID._viewID = bs.Read<uLink.NetworkViewID>(codecOptions);
		}
		return netEntityID;
	}

	public override bool Equals(object obj)
	{
		bool flag;
		if (!(obj is NetEntityID))
		{
			flag = (!this.isNet || !(obj is uLink.NetworkViewID) ? false : this.Equals((uLink.NetworkViewID)obj));
		}
		else
		{
			flag = this.Equals((NetEntityID)obj);
		}
		return flag;
	}

	public bool Equals(NetEntityID obj)
	{
		return this.v == obj.v;
	}

	public bool Equals(uLink.NetworkViewID obj)
	{
		return (this.p1 != 0 ? false : this._viewID == obj);
	}

	public static NetEntityID Get(GameObject entity)
	{
		return NetEntityID.Get(entity, false);
	}

	public static NetEntityID Get(GameObject entity, bool throwIfNotFound)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entity, out netEntityID) != 0)
		{
			return netEntityID;
		}
		if (throwIfNotFound)
		{
			throw new InvalidOperationException("no recognizable net entity id");
		}
		return NetEntityID.unassigned;
	}

	public static NetEntityID Get(Component entityComponent)
	{
		return NetEntityID.Get(entityComponent, false);
	}

	public static NetEntityID Get(Component entityComponent, bool throwIfNotFound)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityComponent, out netEntityID) != 0)
		{
			return netEntityID;
		}
		if (throwIfNotFound)
		{
			throw new InvalidOperationException("no recognizable net entity id");
		}
		return NetEntityID.unassigned;
	}

	public static NetEntityID Get(UnityEngine.MonoBehaviour entityScript)
	{
		return NetEntityID.Get(entityScript, false);
	}

	public static NetEntityID Get(UnityEngine.MonoBehaviour entityScript, bool throwIfNotFound)
	{
		NetEntityID netEntityID;
		if ((int)NetEntityID.Of(entityScript, out netEntityID) != 0)
		{
			return netEntityID;
		}
		if (throwIfNotFound)
		{
			throw new InvalidOperationException("no recognizable net entity id");
		}
		return NetEntityID.unassigned;
	}

	public static NetEntityID Get(uLink.NetworkViewID id)
	{
		return new NetEntityID(id);
	}

	public TComponent GetComponent<TComponent>()
	where TComponent : Component
	{
		UnityEngine.MonoBehaviour monoBehaviour = this.view;
		return (!monoBehaviour ? (TComponent)null : monoBehaviour.GetComponent<TComponent>());
	}

	public bool GetComponent<TComponent>(out TComponent component)
	where TComponent : Component
	{
		UnityEngine.MonoBehaviour monoBehaviour = this.view;
		if (!monoBehaviour)
		{
			component = (TComponent)null;
			return false;
		}
		if (monoBehaviour is TComponent)
		{
			component = (TComponent)monoBehaviour;
			return true;
		}
		TComponent tComponent = monoBehaviour.GetComponent<TComponent>();
		TComponent tComponent1 = tComponent;
		component = tComponent;
		return tComponent1;
	}

	public override int GetHashCode()
	{
		return (this.p1 != 0 ? this.v ^ -65536 : this.p2.GetHashCode());
	}

	public static NetEntityID.Kind Of(Component component, out NetEntityID entID, out UnityEngine.MonoBehaviour view)
	{
		if (component is UnityEngine.MonoBehaviour)
		{
			return NetEntityID.Of((UnityEngine.MonoBehaviour)component, out entID, out view);
		}
		if (component)
		{
			return NetEntityID.Of(component.gameObject, out entID, out view);
		}
		entID = NetEntityID.unassigned;
		view = null;
		return NetEntityID.Kind.Missing;
	}

	public static NetEntityID.Kind Of(Component component, out NetEntityID entID)
	{
		UnityEngine.MonoBehaviour monoBehaviour;
		return NetEntityID.Of(component, out entID, out monoBehaviour);
	}

	public static NetEntityID.Kind Of(Component component, out UnityEngine.MonoBehaviour view)
	{
		NetEntityID netEntityID;
		return NetEntityID.Of(component, out netEntityID, out view);
	}

	public static NetEntityID.Kind Of(Component component)
	{
		NetEntityID netEntityID;
		UnityEngine.MonoBehaviour monoBehaviour;
		return NetEntityID.Of(component, out netEntityID, out monoBehaviour);
	}

	public static NetEntityID.Kind Of(UnityEngine.MonoBehaviour script, out NetEntityID entID, out UnityEngine.MonoBehaviour view)
	{
		if (!script)
		{
			entID = NetEntityID.unassigned;
			view = null;
			return NetEntityID.Kind.Missing;
		}
		if (script is uLink.NetworkView)
		{
			view = script;
			entID = ((uLink.NetworkView)script).viewID;
			return NetEntityID.Kind.Net;
		}
		if (!(script is NGCView))
		{
			return NetEntityID.Of(script.gameObject, out entID, out view);
		}
		view = script;
		entID = new NetEntityID((NGCView)script);
		return NetEntityID.Kind.NGC | NetEntityID.Kind.Net;
	}

	public static NetEntityID.Kind Of(UnityEngine.MonoBehaviour script, out NetEntityID entID)
	{
		UnityEngine.MonoBehaviour monoBehaviour;
		return NetEntityID.Of(script, out entID, out monoBehaviour);
	}

	public static NetEntityID.Kind Of(UnityEngine.MonoBehaviour script, out UnityEngine.MonoBehaviour view)
	{
		NetEntityID netEntityID;
		return NetEntityID.Of(script, out netEntityID, out view);
	}

	public static NetEntityID.Kind Of(UnityEngine.MonoBehaviour script)
	{
		NetEntityID netEntityID;
		UnityEngine.MonoBehaviour monoBehaviour;
		return NetEntityID.Of(script, out netEntityID, out monoBehaviour);
	}

	public static NetEntityID.Kind Of(GameObject entity)
	{
		NetEntityID netEntityID;
		UnityEngine.MonoBehaviour monoBehaviour;
		return NetEntityID.Of(entity, out netEntityID, out monoBehaviour);
	}

	public static NetEntityID.Kind Of(GameObject entity, out UnityEngine.MonoBehaviour view)
	{
		NetEntityID netEntityID;
		return NetEntityID.Of(entity, out netEntityID, out view);
	}

	public static NetEntityID.Kind Of(GameObject entity, out NetEntityID entID)
	{
		UnityEngine.MonoBehaviour monoBehaviour;
		return NetEntityID.Of(entity, out entID, out monoBehaviour);
	}

	public static NetEntityID.Kind Of(GameObject entity, out NetEntityID entID, out UnityEngine.MonoBehaviour view)
	{
		if (!entity)
		{
			entID = NetEntityID.unassigned;
			view = null;
			return NetEntityID.Kind.Missing;
		}
		uLink.NetworkView component = entity.GetComponent<uLink.NetworkView>();
		if (component)
		{
			entID = new NetEntityID(component.viewID);
			view = component;
			return NetEntityID.Kind.Net;
		}
		NGCView nGCView = entity.GetComponent<NGCView>();
		if (nGCView)
		{
			entID = new NetEntityID(nGCView);
			view = nGCView;
			return NetEntityID.Kind.NGC | NetEntityID.Kind.Net;
		}
		entID = NetEntityID.unassigned;
		view = null;
		return NetEntityID.Kind.Missing;
	}

	public static bool operator ==(NetEntityID lhs, NetEntityID rhs)
	{
		return lhs.v == rhs.v;
	}

	public static bool operator ==(NetEntityID lhs, uLink.NetworkViewID rhs)
	{
		return (lhs.p1 != 0 ? false : lhs.p2 == rhs.id);
	}

	public static bool operator ==(uLink.NetworkViewID lhs, NetEntityID rhs)
	{
		return (rhs.p1 != 0 ? false : rhs.p2 == lhs.id);
	}

	public static explicit operator NetworkViewID(NetEntityID viewID)
	{
		if (viewID.p1 != 0)
		{
			throw new InvalidCastException("The NetEntityID did not represet a NetworkViewID");
		}
		return viewID._viewID;
	}

	public static bool operator @false(NetEntityID id)
	{
		return id.v == 0;
	}

	public static bool operator >(NetEntityID lhs, NetEntityID rhs)
	{
		return lhs.v > rhs.v;
	}

	public static bool operator >(NetEntityID lhs, uLink.NetworkViewID rhs)
	{
		return lhs.v > rhs.id;
	}

	public static bool operator >(uLink.NetworkViewID lhs, NetEntityID rhs)
	{
		return lhs.id > rhs.v;
	}

	public static bool operator >=(NetEntityID lhs, NetEntityID rhs)
	{
		return lhs.v >= rhs.v;
	}

	public static bool operator >=(NetEntityID lhs, uLink.NetworkViewID rhs)
	{
		return lhs.v >= rhs.id;
	}

	public static bool operator >=(uLink.NetworkViewID lhs, NetEntityID rhs)
	{
		return lhs.id >= rhs.v;
	}

	public static implicit operator NetEntityID(uLink.NetworkViewID viewID)
	{
		return new NetEntityID()
		{
			_viewID = viewID
		};
	}

	public static bool operator !=(NetEntityID lhs, NetEntityID rhs)
	{
		return lhs.v != rhs.v;
	}

	public static bool operator !=(NetEntityID lhs, uLink.NetworkViewID rhs)
	{
		return (lhs.p1 != 0 ? true : lhs.p2 != rhs.id);
	}

	public static bool operator !=(uLink.NetworkViewID lhs, NetEntityID rhs)
	{
		return (rhs.p1 != 0 ? true : rhs.p2 != lhs.id);
	}

	public static bool operator <(NetEntityID lhs, NetEntityID rhs)
	{
		return lhs.v < rhs.v;
	}

	public static bool operator <(NetEntityID lhs, uLink.NetworkViewID rhs)
	{
		return lhs.v < rhs.id;
	}

	public static bool operator <(uLink.NetworkViewID lhs, NetEntityID rhs)
	{
		return lhs.id < rhs.v;
	}

	public static bool operator <=(NetEntityID lhs, NetEntityID rhs)
	{
		return lhs.v <= rhs.v;
	}

	public static bool operator <=(NetEntityID lhs, uLink.NetworkViewID rhs)
	{
		return lhs.v <= rhs.id;
	}

	public static bool operator <=(uLink.NetworkViewID lhs, NetEntityID rhs)
	{
		return lhs.id <= rhs.v;
	}

	public static bool operator @true(NetEntityID id)
	{
		return id.v != 0;
	}

	private static void Serializer(uLink.BitStream bs, object value, params object[] codecOptions)
	{
		NetEntityID netEntityID = (NetEntityID)value;
		bs.Write<ushort>(netEntityID.p1, codecOptions);
		if (netEntityID.p1 != 0)
		{
			bs.Write<ushort>(netEntityID.p2, new object[0]);
		}
		else
		{
			bs.Write<uLink.NetworkViewID>(netEntityID._viewID, codecOptions);
		}
	}

	public override string ToString()
	{
		if (this.v == 0)
		{
			return "Unassigned";
		}
		if (this.p1 == 0)
		{
			return this._viewID.ToString();
		}
		return string.Format("NGC ViewID {0} ({1}:{2})", this.v, this.p1, this.p2 + 1);
	}

	public enum Kind : sbyte
	{
		NGC = -1,
		Missing = 0,
		Net = 1
	}
}