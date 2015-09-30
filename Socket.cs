using Facepunch.Intersect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class Socket
{
	public Transform parent;

	public Vector3 offset;

	public Vector3 eulerRotate;

	private readonly bool is_vm;

	private Vector3 rotate_last;

	private Quaternion quat_last;

	private bool got_last;

	public Transform attachParent
	{
		get
		{
			if (!this.is_vm)
			{
				return this.parent;
			}
			return ((Socket.CameraSpace)this).attachParent;
		}
	}

	public Vector3 localPosition
	{
		get
		{
			return this.offset;
		}
	}

	public Quaternion localRotation
	{
		get
		{
			return this.rotate;
		}
	}

	public Vector3 position
	{
		get
		{
			if (this.is_vm)
			{
				return ((Socket.CameraSpace)this).position;
			}
			return ((Socket.LocalSpace)this).position;
		}
	}

	public Quaternion rotate
	{
		get
		{
			if (!this.got_last || this.rotate_last != this.eulerRotate)
			{
				this.rotate_last = this.eulerRotate;
				this.quat_last = Quaternion.Euler(this.eulerRotate);
				this.got_last = true;
			}
			return this.quat_last;
		}
	}

	public Quaternion rotation
	{
		get
		{
			if (this.is_vm)
			{
				return ((Socket.CameraSpace)this).rotation;
			}
			return ((Socket.LocalSpace)this).rotation;
		}
	}

	protected Socket(bool is_vm)
	{
		this.is_vm = is_vm;
	}

	public bool AddChild(Transform transform, bool snap)
	{
		if (this.is_vm)
		{
			return ((Socket.CameraSpace)this).AddChild(transform, snap);
		}
		return ((Socket.LocalSpace)this).AddChild(transform, snap);
	}

	public bool AddChildWithCoords(Transform transform, Vector3 offsetFromThisSocket)
	{
		if (this.is_vm)
		{
			return ((Socket.CameraSpace)this).AddChildWithCoords(transform, offsetFromThisSocket);
		}
		return ((Socket.LocalSpace)this).AddChildWithCoords(transform, offsetFromThisSocket);
	}

	public bool AddChildWithCoords(Transform transform, Vector3 offsetFromThisSocket, Vector3 eulerOffsetFromThisSocket)
	{
		if (this.is_vm)
		{
			return ((Socket.CameraSpace)this).AddChildWithCoords(transform, offsetFromThisSocket, eulerOffsetFromThisSocket);
		}
		return ((Socket.LocalSpace)this).AddChildWithCoords(transform, offsetFromThisSocket, eulerOffsetFromThisSocket);
	}

	public bool AddChildWithCoords(Transform transform, Vector3 offsetFromThisSocket, Quaternion rotationalOffsetFromThisSocket)
	{
		if (this.is_vm)
		{
			return ((Socket.CameraSpace)this).AddChildWithCoords(transform, offsetFromThisSocket, rotationalOffsetFromThisSocket);
		}
		return ((Socket.LocalSpace)this).AddChildWithCoords(transform, offsetFromThisSocket, rotationalOffsetFromThisSocket);
	}

	private void AddInstanceChild(Transform tr, bool snap)
	{
		if (!this.AddChild(tr, snap))
		{
			Debug.LogWarning("Could not add child!", tr);
		}
	}

	public void DrawGizmos(string icon)
	{
		Matrix4x4 matrix4x4 = Gizmos.matrix;
		if (this.parent)
		{
			Gizmos.matrix = this.parent.localToWorldMatrix;
		}
		Gizmos.matrix = Gizmos.matrix * Matrix4x4.TRS(this.offset, this.rotate, Vector3.one);
		Color color = Gizmos.color;
		Gizmos.color = color * Color.red;
		Gizmos.DrawLine(Vector3.zero, Vector3.right * 0.1f);
		if (icon != null)
		{
			Gizmos.DrawIcon(Vector3.left, icon);
		}
		Gizmos.color = color * Color.green;
		Gizmos.DrawLine(Vector3.zero, Vector3.up * 0.1f);
		if (icon != null)
		{
			Gizmos.DrawIcon(Vector3.down, icon);
		}
		Gizmos.color = color * Color.blue;
		Gizmos.DrawLine(Vector3.zero, Vector3.forward * 0.1f);
		Gizmos.matrix = matrix4x4;
		Gizmos.color = color;
	}

	public TObject Instantiate<TObject>(TObject prefab)
	where TObject : UnityEngine.Object
	{
		return (TObject)UnityEngine.Object.Instantiate(prefab, this.position, this.rotation);
	}

	public Transform InstantiateAsChild(Transform prefab, bool snap)
	{
		Transform transforms = (Transform)UnityEngine.Object.Instantiate(prefab, this.position, this.rotation);
		this.AddInstanceChild(transforms, snap);
		return transforms;
	}

	public GameObject InstantiateAsChild(GameObject prefab, bool snap)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(prefab, this.position, this.rotation);
		this.AddInstanceChild(gameObject.transform, snap);
		return gameObject;
	}

	public TComponent InstantiateAsChild<TComponent>(TComponent prefab, bool snap)
	where TComponent : Component
	{
		TComponent tComponent = (TComponent)UnityEngine.Object.Instantiate(prefab, this.position, this.rotation);
		this.AddInstanceChild(tComponent.transform, snap);
		return tComponent;
	}

	public void Rotate(Quaternion rotation)
	{
		if (!this.is_vm)
		{
			((Socket.LocalSpace)this).Rotate(rotation);
		}
		else
		{
			((Socket.CameraSpace)this).Rotate(rotation);
		}
	}

	public void Snap()
	{
		if (this.is_vm)
		{
			((Socket.CameraSpace)this).Snap();
		}
	}

	public void UnRotate(Quaternion rotation)
	{
		if (!this.is_vm)
		{
			((Socket.LocalSpace)this).UnRotate(rotation);
		}
		else
		{
			((Socket.CameraSpace)this).UnRotate(rotation);
		}
	}

	public struct CameraConversion : IEquatable<Socket.CameraConversion>
	{
		public readonly Transform Eye;

		public readonly Transform Shelf;

		public readonly bool Provided;

		public static Socket.CameraConversion None
		{
			get
			{
				return new Socket.CameraConversion();
			}
		}

		public bool Valid
		{
			get
			{
				bool shelf;
				if (!this.Provided || !this.Eye)
				{
					shelf = false;
				}
				else
				{
					shelf = this.Shelf;
				}
				return shelf;
			}
		}

		public CameraConversion(Transform World, Transform Camera)
		{
			bool camera;
			this.Eye = World;
			this.Shelf = Camera;
			if (!(World != Camera) || !World)
			{
				camera = false;
			}
			else
			{
				camera = Camera;
			}
			this.Provided = camera;
		}

		public bool Equals(Socket.CameraConversion other)
		{
			bool provided;
			if (!this.Provided)
			{
				provided = !other.Provided;
			}
			else
			{
				provided = (!other.Provided || !(this.Eye == other.Eye) ? false : this.Shelf == other.Shelf);
			}
			return provided;
		}

		public override bool Equals(object obj)
		{
			return (!(obj is Socket.CameraConversion) ? false : this.Equals((Socket.CameraConversion)obj));
		}

		public override int GetHashCode()
		{
			return (!this.Provided ? 0 : this.Eye.GetHashCode() ^ this.Shelf.GetHashCode());
		}

		public static bool operator @false(Socket.CameraConversion cc)
		{
			return !cc.Valid;
		}

		public static implicit operator Boolean(Socket.CameraConversion cc)
		{
			return cc.Valid;
		}

		public static bool operator @true(Socket.CameraConversion cc)
		{
			return cc.Valid;
		}

		public override string ToString()
		{
			string str;
			if (!this.Valid)
			{
				str = (!this.Provided ? "[CameraConversion:NotProvided]" : "[CameraConversion:Invalid]");
			}
			else
			{
				str = "[CameraConversion:Valid]";
			}
			return str;
		}
	}

	[Serializable]
	public sealed class CameraSpace : Socket
	{
		[NonSerialized]
		public Transform eye;

		[NonSerialized]
		public Transform root;

		public bool proxy;

		[NonSerialized]
		internal Transform proxyTransform;

		public new Transform attachParent
		{
			get
			{
				if (this.proxy)
				{
					return this.proxyTransform;
				}
				return this.eye;
			}
		}

		public new Vector3 position
		{
			get
			{
				Vector3 vector3;
				if (!this.root)
				{
					vector3 = (!this.parent ? this.offset : this.parent.TransformPoint(this.offset));
				}
				else
				{
					vector3 = (!this.parent || !(this.parent != this.root) ? this.offset : this.root.InverseTransformPoint(this.parent.TransformPoint(this.offset)));
				}
				return (!this.eye ? vector3 : this.eye.TransformPoint(vector3));
			}
		}

		public Vector3 preEyePosition
		{
			get
			{
				return (!this.parent ? this.offset : this.parent.TransformPoint(this.offset));
			}
		}

		public Quaternion preEyeRotation
		{
			get
			{
				return (!this.parent ? base.rotate : this.parent.rotation * base.rotate);
			}
		}

		public new Quaternion rotation
		{
			get
			{
				Quaternion quaternion;
				if (!this.root)
				{
					quaternion = (!this.parent ? base.rotate : base.rotate * this.parent.rotation);
				}
				else
				{
					quaternion = (!this.parent || !(this.parent != this.root) ? base.rotate : Quaternion.Inverse(this.root.rotation) * base.rotate * this.parent.rotation);
				}
				if (!this.eye)
				{
					return quaternion;
				}
				return this.eye.rotation * quaternion;
			}
		}

		public CameraSpace() : base(true)
		{
		}

		public new bool AddChild(Transform transform, bool snap)
		{
			if (!this.proxy || !this.proxyTransform)
			{
				return false;
			}
			if (!snap)
			{
				Vector3 vector3 = transform.position;
				Vector3 vector31 = transform.forward;
				Vector3 vector32 = transform.up;
				if (this.eye)
				{
					vector3 = this.eye.InverseTransformPoint(vector3);
					vector32 = this.eye.InverseTransformDirection(vector32);
					vector31 = this.eye.InverseTransformDirection(vector31);
				}
				if (this.root)
				{
					vector3 = this.root.TransformPoint(vector3);
					vector32 = this.root.TransformDirection(vector32);
					vector31 = this.root.TransformDirection(vector31);
				}
				if (this.parent)
				{
					vector3 = this.parent.InverseTransformPoint(vector3);
					vector32 = this.parent.InverseTransformDirection(vector32);
					vector31 = this.parent.InverseTransformDirection(vector31);
				}
				transform.parent = this.proxyTransform;
				transform.localPosition = vector3;
				transform.localRotation = Quaternion.LookRotation(vector31, vector32);
			}
			else
			{
				transform.parent = this.proxyTransform;
				transform.localPosition = this.offset;
				transform.localEulerAngles = this.eulerRotate;
			}
			return true;
		}

		public new bool AddChildWithCoords(Transform transform, Vector3 offsetFromThisSocket)
		{
			if (!this.AddChild(transform, false))
			{
				return false;
			}
			transform.localPosition = this.offset + (base.rotate * offsetFromThisSocket);
			return true;
		}

		public new bool AddChildWithCoords(Transform transform, Vector3 offsetFromThisSocket, Vector3 eulerOffsetFromThisSocket)
		{
			if (!this.AddChild(transform, false))
			{
				return false;
			}
			Quaternion quaternion = base.rotate;
			transform.localPosition = this.offset + (quaternion * offsetFromThisSocket);
			transform.localRotation = quaternion * Quaternion.Euler(eulerOffsetFromThisSocket);
			return true;
		}

		public new bool AddChildWithCoords(Transform transform, Vector3 offsetFromThisSocket, Quaternion rotationOffsetFromThisSocket)
		{
			if (!this.AddChild(transform, false))
			{
				return false;
			}
			Quaternion quaternion = base.rotate;
			transform.localPosition = this.offset + (quaternion * offsetFromThisSocket);
			transform.localRotation = quaternion * rotationOffsetFromThisSocket;
			return true;
		}

		public new void Rotate(Quaternion rotation)
		{
			Vector3 vector3;
			float single;
			rotation.ToAngleAxis(out single, out vector3);
			vector3 = this.parent.TransformDirection(vector3);
			this.parent.RotateAround(this.preEyePosition, vector3, single);
		}

		public new void Snap()
		{
			if (this.proxy && this.proxyTransform && this.root && this.eye)
			{
				Socket.CameraSpace.UpdateProxy(this.parent, this.proxyTransform, this.root, this.eye);
			}
		}

		public new void UnRotate(Quaternion rotation)
		{
			Vector3 vector3;
			float single;
			rotation.ToAngleAxis(out single, out vector3);
			vector3 = this.parent.TransformDirection(vector3);
			this.parent.RotateAround(this.preEyePosition, -vector3, single);
		}

		public static void UpdateProxy(Transform key, Transform value, Transform shelf, Transform eye)
		{
			value.position = eye.TransformPoint(shelf.InverseTransformPoint(key.position));
			Vector3 vector3 = eye.TransformDirection(shelf.InverseTransformDirection(key.forward));
			Vector3 vector31 = eye.TransformDirection(shelf.InverseTransformDirection(key.up));
			value.rotation = Quaternion.LookRotation(vector3, vector31);
		}
	}

	[Serializable]
	public sealed class ConfigBodyPart
	{
		public BodyPart parent;

		public Vector3 offset;

		public Vector3 eulerRotate;

		public ConfigBodyPart()
		{
		}

		public static Socket.ConfigBodyPart Create(BodyPart parent, Vector3 offset, Vector3 eulerRotate)
		{
			Socket.ConfigBodyPart configBodyPart = new Socket.ConfigBodyPart()
			{
				parent = parent,
				offset = offset,
				eulerRotate = eulerRotate
			};
			return configBodyPart;
		}

		public bool Extract(ref Socket.LocalSpace space, HitBoxSystem system)
		{
			Transform transforms;
			if (!this.Find(system, out transforms))
			{
				return false;
			}
			if (space == null)
			{
				Socket.LocalSpace localSpace = new Socket.LocalSpace()
				{
					parent = transforms,
					eulerRotate = this.eulerRotate,
					offset = this.offset
				};
				space = localSpace;
			}
			else if (space.parent != transforms)
			{
				space.parent = transforms;
				space.eulerRotate = this.eulerRotate;
				space.offset = this.offset;
			}
			return true;
		}

		public bool Extract(ref Socket.CameraSpace space, HitBoxSystem system)
		{
			Transform transforms;
			if (!this.Find(system, out transforms))
			{
				return false;
			}
			if (space == null)
			{
				Socket.CameraSpace cameraSpace = new Socket.CameraSpace()
				{
					parent = transforms,
					eulerRotate = this.eulerRotate,
					offset = this.offset
				};
				space = cameraSpace;
			}
			else if (space.parent != transforms)
			{
				space.parent = transforms;
				space.eulerRotate = this.eulerRotate;
				space.offset = this.offset;
			}
			return true;
		}

		private bool Find(HitBoxSystem system, out Transform parent)
		{
			IDRemoteBodyPart dRemoteBodyPart;
			if (!system)
			{
				parent = null;
				return false;
			}
			if (!system.bodyParts.TryGetValue(this.parent, out dRemoteBodyPart))
			{
				parent = null;
				return false;
			}
			parent = dRemoteBodyPart.transform;
			return true;
		}
	}

	[Serializable]
	public sealed class LocalSpace : Socket
	{
		public new Vector3 position
		{
			get
			{
				return (!this.parent ? this.offset : this.parent.TransformPoint(this.offset));
			}
		}

		public new Quaternion rotation
		{
			get
			{
				return (!this.parent ? base.rotate : this.parent.rotation * base.rotate);
			}
		}

		public LocalSpace() : base(false)
		{
		}

		public new bool AddChild(Transform transform, bool snap)
		{
			if (!transform)
			{
				return false;
			}
			transform.parent = this.parent;
			if (snap)
			{
				transform.localPosition = this.offset;
				transform.localEulerAngles = this.eulerRotate;
			}
			return true;
		}

		public new bool AddChildWithCoords(Transform transform, Vector3 offsetFromThisSocket)
		{
			if (!this.AddChild(transform, false))
			{
				return false;
			}
			transform.localPosition = this.offset + (base.rotate * offsetFromThisSocket);
			return true;
		}

		public new bool AddChildWithCoords(Transform transform, Vector3 offsetFromThisSocket, Vector3 eulerOffsetFromThisSocket)
		{
			if (!this.AddChild(transform, false))
			{
				return false;
			}
			Quaternion quaternion = base.rotate;
			transform.localPosition = this.offset + (quaternion * offsetFromThisSocket);
			transform.localRotation = quaternion * Quaternion.Euler(eulerOffsetFromThisSocket);
			return true;
		}

		public new bool AddChildWithCoords(Transform transform, Vector3 offsetFromThisSocket, Quaternion rotationOffsetFromThisSocket)
		{
			if (!this.AddChild(transform, false))
			{
				return false;
			}
			Quaternion quaternion = base.rotate;
			transform.localPosition = this.offset + (quaternion * offsetFromThisSocket);
			transform.localRotation = quaternion * rotationOffsetFromThisSocket;
			return true;
		}

		public new void Rotate(Quaternion rotation)
		{
			Vector3 vector3;
			float single;
			rotation.ToAngleAxis(out single, out vector3);
			vector3 = this.parent.TransformDirection(vector3);
			this.parent.RotateAround(this.position, vector3, single);
		}

		public new void Snap()
		{
		}

		public new void UnRotate(Quaternion rotation)
		{
			Vector3 vector3;
			float single;
			rotation.ToAngleAxis(out single, out vector3);
			vector3 = this.parent.TransformDirection(vector3);
			this.parent.RotateAround(this.position, -vector3, single);
		}
	}

	public sealed class Map : Socket.Mapped
	{
		[NonSerialized]
		private readonly UnityEngine.Object script;

		[NonSerialized]
		private readonly Socket.Source source;

		[NonSerialized]
		private Dictionary<string, int> dict;

		[NonSerialized]
		private bool initialized;

		[NonSerialized]
		private bool checkTransforms;

		[NonSerialized]
		private bool securing;

		[NonSerialized]
		private bool forceUpdate;

		[NonSerialized]
		private bool deleted;

		[NonSerialized]
		private Socket.Map.Element[] array;

		[NonSerialized]
		private int version;

		[NonSerialized]
		private Socket.CameraConversion cameraSpace;

		public Socket.CameraConversion cameraConversion
		{
			get
			{
				Socket.CameraConversion cameraConversion;
				this.GetCameraSpace(out cameraConversion);
				return cameraConversion;
			}
		}

		public Socket.Slot this[int index]
		{
			get
			{
				if (index < 0 || !this.EnsureState() || index >= (int)this.array.Length)
				{
					throw new IndexOutOfRangeException();
				}
				return new Socket.Slot(this, index);
			}
		}

		public Socket.Slot this[string name]
		{
			get
			{
				if (!this.EnsureState())
				{
					throw new KeyNotFoundException(name);
				}
				return new Socket.Slot(this, this.dict[name]);
			}
		}

		private static Socket.Map NullMap
		{
			get
			{
				return null;
			}
		}

		Socket.Map Socket.Mapped.socketMap
		{
			get
			{
				return this.EnsureMap();
			}
		}

		public int socketCount
		{
			get
			{
				return (!this.EnsureState() ? 0 : (int)this.array.Length);
			}
		}

		private Map(Socket.Source source, UnityEngine.Object script)
		{
			this.source = source;
			this.script = script;
		}

		private void CheckProxyIndex(int index, out Socket.Map.ProxyCheck o)
		{
			o = new Socket.Map.ProxyCheck();
			bool flag;
			Socket.CameraSpace cameraSpace = this.array[index].socket as Socket.CameraSpace;
			Socket.CameraSpace cameraSpace1 = cameraSpace;
			o.cameraSpace = cameraSpace;
			bool flag1 = !object.ReferenceEquals(cameraSpace1, null);
			bool flag2 = flag1;
			o.isCameraSpace = flag1;
			flag = (!flag2 ? false : o.cameraSpace.proxy);
			flag2 = flag;
			o.isProxy = flag;
			if (!flag2)
			{
				o.proxyLink = null;
				o.parentOrProxy = this.array[index].socket.parent;
			}
			else
			{
				if (this.array[index].madeLink)
				{
					o.proxyLink = this.array[index].link;
				}
				else
				{
					Socket.ProxyLink proxyLink = this.MakeProxy(o.cameraSpace, index);
					Socket.ProxyLink proxyLink1 = proxyLink;
					this.array[index].link = proxyLink;
					o.proxyLink = proxyLink1;
					this.array[index].madeLink = true;
				}
				o.parentOrProxy = o.proxyLink.proxy.transform;
			}
			o.index = index;
		}

		private void CleanTransforms()
		{
			this.checkTransforms = true;
			this.cameraSpace = Socket.CameraConversion.None;
		}

		private void Delete()
		{
			if (!this.initialized || this.deleted)
			{
				return;
			}
			this.deleted = true;
			for (int i = (int)this.array.Length - 1; i >= 0; i--)
			{
				if (this.array[i].madeLink)
				{
					this.DestroyProxyLink(this.array[i].link);
				}
			}
		}

		private void DestroyProxyLink(Socket.ProxyLink link)
		{
			if (link.linked)
			{
				link.linked = false;
				if (link.scriptAlive && link.proxy)
				{
					UnityEngine.Object.Destroy(link.proxy);
				}
				link.proxy = null;
				if (link.gameObject)
				{
					UnityEngine.Object.Destroy(link.gameObject);
				}
				link.gameObject = null;
				link.proxy = null;
			}
		}

		private void ElementRemove(ref Socket.Map.Element element, ref Socket.Map.RemoveList<Socket.ProxyLink> removeList)
		{
			if (element.madeLink)
			{
				if (element.link.scriptAlive)
				{
					removeList.Add(element.link);
				}
				element.link = null;
				element.madeLink = false;
			}
			this.dict.Remove(element.name);
		}

		private void ElementUpdate(int srcIndex, ref Socket.Map.Element src, int dstIndex, ref Socket.Map.Element dst, Socket newSocket)
		{
			if (srcIndex != dstIndex)
			{
				dst.name = src.name;
				dst.link = src.link;
				dst.socket = src.socket;
				dst.madeLink = src.madeLink;
				if (dst.madeLink)
				{
					dst.link.index = dstIndex;
				}
				this.dict[dst.name] = dstIndex;
			}
			this.SocketUpdate(ref dst.socket, newSocket);
		}

		private Socket.Map EnsureMap()
		{
			return (!this.EnsureState() ? Socket.Map.NullMap : this);
		}

		private bool EnsureState()
		{
			if (!this.script || this.deleted)
			{
				return false;
			}
			if (this.securing)
			{
				return true;
			}
			try
			{
				this.securing = true;
				this.SecureState();
			}
			finally
			{
				this.securing = false;
			}
			return true;
		}

		private static Socket.Map Get<TSource>(TSource source, ref Socket.Map member)
		where TSource : UnityEngine.Object, Socket.Source
		{
			if (object.ReferenceEquals(source, null))
			{
				throw new ArgumentNullException("source");
			}
			if (!source)
			{
				return Socket.Map.NullMap;
			}
			Socket.Map map = member;
			if (object.ReferenceEquals(map, null))
			{
				map = new Socket.Map((object)source, (object)source);
			}
			Socket.Map map1 = map.EnsureMap();
			Socket.Map map2 = map1;
			member = map1;
			return map2;
		}

		private bool GetCameraSpace(out Socket.CameraConversion cameraSpace)
		{
			if (!this.EnsureState())
			{
				this.checkTransforms = false;
				this.cameraSpace = Socket.CameraConversion.None;
			}
			else if (this.checkTransforms)
			{
				this.checkTransforms = false;
				this.cameraSpace = this.source.CameraSpaceSetup();
			}
			cameraSpace = this.cameraSpace;
			return cameraSpace.Valid;
		}

		private void Initialize()
		{
			ICollection<string> strs;
			IEnumerable<string> socketNames = this.source.SocketNames;
			if (!object.ReferenceEquals(socketNames, null))
			{
				strs = socketNames as ICollection<string>;
				if (strs == null)
				{
					strs = new HashSet<string>(socketNames, StringComparer.InvariantCultureIgnoreCase);
				}
			}
			else
			{
				strs = new string[0];
			}
			int count = strs.Count;
			this.array = new Socket.Map.Element[count];
			this.dict = new Dictionary<string, int>(count, StringComparer.InvariantCultureIgnoreCase);
			int num = 0;
			IEnumerator<string> enumerator = strs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					if (!this.source.GetSocket(current, out this.array[num].socket))
					{
						continue;
					}
					try
					{
						string str = current;
						string str1 = str;
						this.array[num].name = str;
						this.dict.Add(str1, num);
					}
					catch (ArgumentException argumentException)
					{
						Debug.LogException(argumentException, this.script);
						Debug.Log(current);
						continue;
					}
					num++;
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			Array.Resize<Socket.Map.Element>(ref this.array, num);
			this.version = this.source.SocketsVersion;
		}

		private Socket.ProxyLink MakeProxy(Socket.CameraSpace socket, int index)
		{
			Socket.CameraConversion cameraConversion;
			Type type = this.source.ProxyScriptType(this.array[index].name);
			if (object.ReferenceEquals(type, null))
			{
				return null;
			}
			if (!typeof(Socket.Proxy).IsAssignableFrom(type))
			{
				throw new InvalidProgramException("SocketSource returned a type that did not extend SocketMap.Proxy");
			}
			Socket.ProxyLink proxyLink = new Socket.ProxyLink()
			{
				map = this,
				index = index
			};
			Socket.ProxyLink.Push(proxyLink);
			Vector3 vector3 = Vector3.zero;
			Quaternion quaternion = Quaternion.identity;
			if (!this.GetCameraSpace(out cameraConversion))
			{
				socket.eye = null;
				socket.root = null;
			}
			else
			{
				socket.root = cameraConversion.Shelf;
				socket.eye = cameraConversion.Eye;
			}
			try
			{
				vector3 = socket.position;
				quaternion = socket.rotation;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this.script);
			}
			try
			{
				try
				{
					GameObject gameObject = new GameObject(this.array[index].name, new Type[] { type });
					gameObject.transform.position = vector3;
					gameObject.transform.rotation = quaternion;
					proxyLink.gameObject = gameObject;
				}
				catch
				{
					proxyLink.linked = false;
					if (proxyLink.gameObject)
					{
						UnityEngine.Object.Destroy(proxyLink.gameObject);
					}
					throw;
				}
			}
			finally
			{
				Socket.ProxyLink.EnsurePopped(proxyLink);
			}
			proxyLink.linked = true;
			socket.proxyTransform = proxyLink.proxy.transform;
			return proxyLink;
		}

		private static bool Of(ref Socket.Map member, out Socket.Map value)
		{
			if (object.ReferenceEquals(member, null))
			{
				value = null;
				return false;
			}
			Socket.Map map = member.EnsureMap();
			member = map;
			value = map;
			return !object.ReferenceEquals(map, null);
		}

		internal static Socket.Map Of(ref Socket.Map member)
		{
			Socket.Map map;
			Socket.Map.Of(ref member, out map);
			return map;
		}

		internal void OnProxyDestroyed(object link)
		{
			this.DestroyProxyLink((Socket.ProxyLink)link);
		}

		private void OnState(Socket.Map.Result State)
		{
			Socket.Map.ProxyCheck eye;
			bool flag = false;
			bool cameraSpace = false;
			Socket.CameraConversion cameraConversion = new Socket.CameraConversion();
			for (int i = 0; i < (int)this.array.Length; i++)
			{
				this.CheckProxyIndex(i, out eye);
				if (eye.isCameraSpace)
				{
					if (!flag)
					{
						cameraSpace = this.GetCameraSpace(out cameraConversion);
						flag = true;
					}
					eye.cameraSpace.eye = cameraConversion.Eye;
					eye.cameraSpace.root = cameraConversion.Shelf;
					eye.cameraSpace.proxyTransform = eye.proxyTransform;
				}
			}
		}

		private Socket.Map.Result PollState()
		{
			Socket.Map.Result result;
			if (!this.initialized)
			{
				this.Initialize();
				return Socket.Map.Result.Initialized;
			}
			int socketsVersion = this.source.SocketsVersion;
			if (this.version == socketsVersion)
			{
				if (!this.forceUpdate)
				{
					return Socket.Map.Result.Nothing;
				}
				result = Socket.Map.Result.Forced;
			}
			else
			{
				this.version = socketsVersion;
				result = Socket.Map.Result.Version;
			}
			this.forceUpdate = false;
			this.Update(result);
			return result;
		}

		public bool ReplaceSocket(string name, Socket value)
		{
			int num;
			return (!this.EnsureState() || !this.dict.TryGetValue(name, out num) ? false : this.ValidSlotReplace(num, value));
		}

		public bool ReplaceSocket(int index, Socket value)
		{
			if (index < 0)
			{
				return false;
			}
			return (!this.EnsureState() || index >= (int)this.array.Length ? false : this.ValidSlotReplace(index, value));
		}

		public bool ReplaceSocket(Socket.Slot slot, Socket value)
		{
			if (slot.index < 0)
			{
				return false;
			}
			return (!slot.BelongsTo(this) || slot.index >= (int)this.array.Length ? false : this.ValidSlotReplace(slot.index, value));
		}

		private Socket.Map.Result SecureState()
		{
			Socket.Map.Result result = this.PollState();
			switch (result)
			{
				case Socket.Map.Result.Initialized:
				{
					this.initialized = true;
					this.CleanTransforms();
					break;
				}
				case Socket.Map.Result.Version:
				{
					this.CleanTransforms();
					break;
				}
				case Socket.Map.Result.Forced:
				{
					break;
				}
				default:
				{
					return result;
				}
			}
			this.OnState(result);
			return result;
		}

		public void SnapProxies()
		{
			Socket.CameraConversion cameraConversion;
			if (this.EnsureState())
			{
				bool cameraSpace = this.GetCameraSpace(out cameraConversion);
				for (int i = 0; i < (int)this.array.Length; i++)
				{
					if (this.array[i].madeLink && this.array[i].link.scriptAlive && this.array[i].link.linked)
					{
						try
						{
							Socket.CameraSpace eye = (Socket.CameraSpace)this.array[i].socket;
							eye.proxyTransform = this.array[i].link.proxy.transform;
							eye.eye = cameraConversion.Eye;
							eye.root = cameraConversion.Shelf;
							if (cameraSpace)
							{
								eye.Snap();
							}
						}
						catch (Exception exception1)
						{
							Exception exception = exception1;
							Debug.LogException(exception, this.array[i].link.proxy);
						}
					}
				}
			}
		}

		private void SocketUpdate(ref Socket socket, Socket newSocket)
		{
			Socket socket1 = socket;
			if (!object.ReferenceEquals(socket1, newSocket))
			{
				socket = newSocket;
				if (socket1 is Socket.CameraSpace && newSocket is Socket.CameraSpace)
				{
					Socket.CameraSpace cameraSpace = (Socket.CameraSpace)socket1;
					Socket.CameraSpace cameraSpace1 = (Socket.CameraSpace)newSocket;
					cameraSpace1.root = cameraSpace.root;
					cameraSpace1.eye = cameraSpace.eye;
					cameraSpace1.proxyTransform = cameraSpace.proxyTransform;
				}
			}
		}

		private void Update(Socket.Map.Result Because)
		{
			Socket socket;
			if (Because == Socket.Map.Result.Version)
			{
				this.CleanTransforms();
			}
			int num = 0;
			Socket.Map.RemoveList<Socket.ProxyLink> removeList = new Socket.Map.RemoveList<Socket.ProxyLink>();
			for (int i = 0; i < (int)this.array.Length; i++)
			{
				if (!this.source.GetSocket(this.array[i].name, out socket))
				{
					this.ElementRemove(ref this.array[i], ref removeList);
				}
				else
				{
					int num1 = num;
					num = num1 + 1;
					int num2 = num1;
					this.ElementUpdate(i, ref this.array[i], num2, ref this.array[num2], socket);
				}
			}
			Array.Resize<Socket.Map.Element>(ref this.array, num);
		}

		private bool ValidSlotReplace(int index, Socket value)
		{
			Socket socket = this.array[index].socket;
			if (object.ReferenceEquals(value, socket))
			{
				return true;
			}
			if (!object.ReferenceEquals(value, null) && value.GetType() != socket.GetType() || !this.source.ReplaceSocket(this.array[index].name, value))
			{
				return false;
			}
			this.forceUpdate = true;
			return this.EnsureState();
		}

		private struct Element
		{
			public Socket socket;

			public string name;

			public Socket.ProxyLink link;

			public bool madeLink;
		}

		public struct Member
		{
			private Socket.Map reference;

			private bool deleted;

			public bool DeleteBy<T>(T outerInstance)
			where T : UnityEngine.Object, Socket.Source
			{
				if (this.deleted)
				{
					return false;
				}
				if (!object.ReferenceEquals(this.reference, null))
				{
					if (!object.ReferenceEquals(outerInstance, this.reference.source))
					{
						throw new ArgumentException("instance did not match that of which created the map", "outerInstance");
					}
					this.deleted = true;
					try
					{
						try
						{
							this.reference.Delete();
						}
						catch (Exception exception)
						{
							Debug.LogException(exception, outerInstance);
						}
					}
					finally
					{
						this.reference = null;
					}
				}
				else
				{
					this.deleted = true;
				}
				return true;
			}

			public Socket.Map Get<T>(T outerInstance)
			where T : UnityEngine.Object, Socket.Source
			{
				if (this.deleted)
				{
					return null;
				}
				return Socket.Map.Get<T>(outerInstance, ref this.reference);
			}

			public bool Get<T>(T outerInstance, out Socket.Map map)
			where T : UnityEngine.Object, Socket.Source
			{
				map = this.Get<T>(outerInstance);
				return !object.ReferenceEquals(map, null);
			}
		}

		private struct ProxyCheck
		{
			public Transform parentOrProxy;

			public Socket.CameraSpace cameraSpace;

			public Socket.ProxyLink proxyLink;

			public int index;

			public bool isCameraSpace;

			public bool isProxy;

			public Transform parent
			{
				get
				{
					return (!this.isProxy ? this.parentOrProxy : this.cameraSpace.parent);
				}
			}

			public Transform proxyTransform
			{
				get
				{
					Transform transforms;
					if (!this.isProxy)
					{
						transforms = null;
					}
					else
					{
						transforms = this.parentOrProxy;
					}
					return transforms;
				}
			}
		}

		internal struct Reference
		{
			private Socket.Map reference;

			public bool Exists
			{
				get
				{
					Socket.Map map;
					return Socket.Map.Of(ref this.reference, out map);
				}
			}

			public Socket.Map Map
			{
				get
				{
					return Socket.Map.Of(ref this.reference);
				}
			}

			private Reference(Socket.Map reference)
			{
				this.reference = reference;
			}

			private bool ByIndex(int index, out Socket.Map map)
			{
				if (index < 0)
				{
					map = null;
				}
				else if (this.Try(out map) && index < (int)map.array.Length)
				{
					return true;
				}
				return false;
			}

			private bool ByKey(string name, out Socket.Map map, out int index)
			{
				if (object.ReferenceEquals(name, null))
				{
					map = null;
				}
				else if (this.Try(out map))
				{
					return map.dict.TryGetValue(name, out index);
				}
				index = -1;
				return false;
			}

			public bool Is(Socket.Map map)
			{
				return object.ReferenceEquals(this.Map, map);
			}

			private static bool Name(bool valid, int index, Socket.Map map, out string name)
			{
				if (!valid)
				{
					name = null;
					return false;
				}
				name = map.array[index].name;
				return true;
			}

			public bool Name(int index, out string name)
			{
				Socket.Map map;
				return Socket.Map.Reference.Name(this.ByIndex(index, out map), index, map, out name);
			}

			public string Name(int index)
			{
				return this.Map.array[index].name;
			}

			public bool Name(string key, out string name)
			{
				Socket.Map map;
				int num;
				return Socket.Map.Reference.Name(this.ByKey(key, out map, out num), num, map, out name);
			}

			public string Name(string key)
			{
				Socket.Map map = this.Map;
				return map.array[map.dict[key]].name;
			}

			public static implicit operator Reference(Socket.Map reference)
			{
				return new Socket.Map.Reference(reference);
			}

			private static bool Proxy(bool valid, int index, Socket.Map map, out Socket.ProxyLink proxyLink)
			{
				if (!valid)
				{
					proxyLink = null;
					return false;
				}
				proxyLink = map.array[index].link;
				return map.array[index].madeLink;
			}

			public bool Proxy(int index, out Socket.ProxyLink link)
			{
				Socket.Map map;
				return Socket.Map.Reference.Proxy(this.ByIndex(index, out map), index, map, out link);
			}

			internal Socket.ProxyLink Proxy(int index)
			{
				return this.Map.array[index].link;
			}

			internal bool Proxy(string key, out Socket.ProxyLink link)
			{
				Socket.Map map;
				int num;
				return Socket.Map.Reference.Proxy(this.ByKey(key, out map, out num), num, map, out link);
			}

			internal Socket.ProxyLink Proxy(string key)
			{
				Socket.Map map = this.Map;
				return map.array[map.dict[key]].link;
			}

			public bool RefEquals(Socket.Map map)
			{
				return object.ReferenceEquals(this.reference, map);
			}

			private static bool Socket(bool valid, int index, Socket.Map map, out Socket socket)
			{
				if (!valid)
				{
					socket = null;
					return false;
				}
				socket = map.array[index].socket;
				return true;
			}

			public bool Socket(int index, out Socket socket)
			{
				Socket.Map map;
				return Socket.Map.Reference.Socket(this.ByIndex(index, out map), index, map, out socket);
			}

			public Socket Socket(int index)
			{
				return this.Map.array[index].socket;
			}

			public bool Socket(string key, out Socket socket)
			{
				Socket.Map map;
				int num;
				return Socket.Map.Reference.Socket(this.ByKey(key, out map, out num), num, map, out socket);
			}

			public Socket Socket(string key)
			{
				Socket.Map map = this.Map;
				return map.array[map.dict[key]].socket;
			}

			public bool Socket<TSocket>(int index, out TSocket socket)
			where TSocket : Socket, new()
			{
				Socket socket1;
				bool flag = this.Socket(index, out socket1);
				socket = (!flag ? (TSocket)null : (TSocket)(socket1 as TSocket));
				return (!flag ? false : socket1 != null);
			}

			public bool Socket<TSocket>(string name, out TSocket socket)
			where TSocket : Socket, new()
			{
				Socket socket1;
				bool flag = this.Socket(name, out socket1);
				socket = (!flag ? (TSocket)null : (TSocket)(socket1 as TSocket));
				return (!flag ? false : socket1 != null);
			}

			public TSocket Socket<TSocket>(int index)
			where TSocket : Socket, new()
			{
				return (TSocket)this.Socket(index);
			}

			public TSocket Socket<TSocket>(string name)
			where TSocket : Socket, new()
			{
				return (TSocket)this.Socket(name);
			}

			public bool SocketIndex(string name, out int index)
			{
				Socket.Map map;
				if (!this.Try(out map))
				{
					index = -1;
					return false;
				}
				return map.dict.TryGetValue(name, out index);
			}

			public int SocketIndex(string name)
			{
				return this.Map.dict[name];
			}

			public bool Try(out Socket.Map map)
			{
				return Socket.Map.Of(ref this.reference, out map);
			}
		}

		private struct RemoveList<T>
		{
			public bool exists;

			public List<T> list;

			public void Add(T item)
			{
				if (!this.exists)
				{
					this.list = new List<T>();
				}
				this.list.Add(item);
			}
		}

		private enum Result
		{
			Nothing,
			Initialized,
			Version,
			Forced
		}
	}

	public interface Mapped
	{
		Socket.Map socketMap
		{
			get;
		}
	}

	public interface Provider : Socket.Source, Socket.Mapped
	{

	}

	public abstract class Proxy : MonoBehaviour, Socket.Mapped
	{
		[NonSerialized]
		private readonly Socket.ProxyLink link;

		[NonSerialized]
		private Transform _transform;

		public Socket.CameraSpace socket
		{
			get
			{
				Socket.CameraSpace cameraSpace;
				Socket.CameraSpace cameraSpace1;
				if (!this.link.linked || !this.link.map.Socket<Socket.CameraSpace>(this.link.index, out cameraSpace))
				{
					cameraSpace1 = null;
				}
				else
				{
					cameraSpace1 = cameraSpace;
				}
				return cameraSpace1;
			}
		}

		public bool socketExists
		{
			get
			{
				return (!this.link.linked ? false : this.link.map.Exists);
			}
		}

		public int socketIndex
		{
			get
			{
				return (!this.link.linked || !this.link.map.Exists ? -1 : this.link.index);
			}
		}

		public Socket.Map socketMap
		{
			get
			{
				return this.link.map.Map;
			}
		}

		public string socketName
		{
			get
			{
				string str;
				string str1;
				if (!this.link.linked || !this.link.map.Name(this.link.index, out str))
				{
					str1 = null;
				}
				else
				{
					str1 = str;
				}
				return str1;
			}
		}

		public new Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		public Proxy()
		{
			this.link = Socket.ProxyLink.Pop();
			this.link.proxy = this;
		}

		protected void Awake()
		{
			this._transform = base.transform;
			this.link.scriptAlive = true;
			try
			{
				this.InitializeProxy();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
		}

		public bool GetSocketMap(out Socket.Map map)
		{
			return this.link.map.Try(out map);
		}

		protected virtual void InitializeProxy()
		{
		}

		protected void OnDestroy()
		{
			Socket.Map map;
			if (this.link.scriptAlive)
			{
				this.link.scriptAlive = false;
				try
				{
					this.UninitializeProxy();
				}
				finally
				{
					if (this.GetSocketMap(out map))
					{
						map.OnProxyDestroyed(this.link);
					}
					this.link.proxy = null;
				}
			}
		}

		protected virtual void UninitializeProxy()
		{
		}
	}

	internal sealed class ProxyLink
	{
		[NonSerialized]
		public Socket.Map.Reference map;

		[NonSerialized]
		public Socket.Proxy proxy;

		[NonSerialized]
		public GameObject gameObject;

		[NonSerialized]
		public bool scriptAlive;

		[NonSerialized]
		public bool linked;

		[NonSerialized]
		public int index;

		public ProxyLink()
		{
		}

		public static void EnsurePopped(Socket.ProxyLink top)
		{
			if (Socket.ProxyLink.Usage.Stack.Count > 0 && Socket.ProxyLink.Usage.Stack.Peek() == top)
			{
				Socket.ProxyLink.Usage.Stack.Pop();
			}
		}

		public static Socket.ProxyLink Pop()
		{
			return Socket.ProxyLink.Usage.Stack.Pop();
		}

		public static void Push(Socket.ProxyLink top)
		{
			Socket.ProxyLink.Usage.Stack.Push(top);
		}

		private static class Usage
		{
			public readonly static Stack<Socket.ProxyLink> Stack;

			static Usage()
			{
				Socket.ProxyLink.Usage.Stack = new Stack<Socket.ProxyLink>();
			}
		}
	}

	public struct Slot
	{
		private Socket.Map.Reference m;

		public readonly int index;

		public string name
		{
			get
			{
				return this.m.Name(this.index);
			}
		}

		public Transform proxy
		{
			get
			{
				Socket.ProxyLink proxyLink;
				Transform transforms;
				if (!this.m.Proxy(this.index, out proxyLink) || !proxyLink.proxy)
				{
					transforms = null;
				}
				else
				{
					transforms = proxyLink.proxy.transform;
				}
				return transforms;
			}
		}

		public Socket socket
		{
			get
			{
				return this.m.Socket(this.index);
			}
			set
			{
				if (!this.ReplaceSocket(value))
				{
					throw new InvalidOperationException("could not replace socket");
				}
			}
		}

		internal Slot(Socket.Map.Reference map, int index)
		{
			this.m = map;
			this.index = index;
		}

		public bool BelongsTo(Socket.Map map)
		{
			return this.m.Is(map);
		}

		public bool ReplaceSocket(Socket newSocketValue)
		{
			Socket.Map map;
			return (!this.m.Try(out map) ? false : map.ReplaceSocket(this.index, newSocketValue));
		}
	}

	public interface Source
	{
		IEnumerable<string> SocketNames
		{
			get;
		}

		int SocketsVersion
		{
			get;
		}

		Socket.CameraConversion CameraSpaceSetup();

		bool GetSocket(string name, out Socket socket);

		Type ProxyScriptType(string name);

		bool ReplaceSocket(string name, Socket newValue);
	}
}