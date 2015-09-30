using Facepunch;
using System;
using uLink;
using UnityEngine;

[InterfaceDriverComponent(typeof(IContextRequestable), "_implementation", "implementation", SearchRoute=InterfaceSearchRoute.GameObject, UnityType=typeof(Facepunch.MonoBehaviour), AlwaysSaveDisabled=true)]
public sealed class Contextual : UnityEngine.MonoBehaviour, IComponentInterfaceDriver<IContextRequestable, Facepunch.MonoBehaviour, Contextual>
{
	[SerializeField]
	private Facepunch.MonoBehaviour _implementation;

	[NonSerialized]
	private Facepunch.MonoBehaviour implementation;

	[NonSerialized]
	private IContextRequestable _requestable;

	[NonSerialized]
	private bool _implemented;

	[NonSerialized]
	private bool _awoke;

	[NonSerialized]
	private bool? _isSoleAccess;

	[NonSerialized]
	private bool? _isMenu;

	[NonSerialized]
	private bool? _isQuick;

	public Contextual driver
	{
		get
		{
			return this;
		}
	}

	public bool exists
	{
		get
		{
			bool flag;
			if (!this._awoke)
			{
				try
				{
					this.Refresh();
				}
				finally
				{
					this._awoke = true;
				}
			}
			if (!this._implemented)
			{
				flag = false;
			}
			else
			{
				bool flag1 = this.implementation;
				bool flag2 = flag1;
				this._implemented = flag1;
				flag = flag2;
			}
			return flag;
		}
	}

	public Facepunch.MonoBehaviour implementor
	{
		get
		{
			if (!this._awoke)
			{
				try
				{
					this.Refresh();
				}
				finally
				{
					this._awoke = true;
				}
			}
			return this.implementation;
		}
	}

	public IContextRequestable @interface
	{
		get
		{
			if (!this._awoke)
			{
				try
				{
					this.Refresh();
				}
				finally
				{
					this._awoke = true;
				}
			}
			return this._requestable;
		}
	}

	public bool isMenu
	{
		get
		{
			bool value;
			bool? nullable = this._isMenu;
			if (!nullable.HasValue)
			{
				bool? nullable1 = new bool?(this.@interface is IContextRequestableMenu);
				bool? nullable2 = nullable1;
				this._isMenu = nullable1;
				value = nullable2.Value;
			}
			else
			{
				value = nullable.Value;
			}
			return value;
		}
	}

	public bool isQuick
	{
		get
		{
			bool value;
			bool? nullable = this._isQuick;
			if (!nullable.HasValue)
			{
				bool? nullable1 = new bool?(this.@interface is IContextRequestableQuick);
				bool? nullable2 = nullable1;
				this._isQuick = nullable1;
				value = nullable2.Value;
			}
			else
			{
				value = nullable.Value;
			}
			return value;
		}
	}

	public bool isSoleAccess
	{
		get
		{
			bool value;
			bool? nullable = this._isSoleAccess;
			if (!nullable.HasValue)
			{
				bool? nullable1 = new bool?(this.@interface is IContextRequestableSoleAccess);
				bool? nullable2 = nullable1;
				this._isSoleAccess = nullable1;
				value = nullable2.Value;
			}
			else
			{
				value = nullable.Value;
			}
			return value;
		}
	}

	public Contextual()
	{
	}

	public bool AsMenu(out IContextRequestableMenu menu)
	{
		if (!this.isMenu)
		{
			menu = null;
			return false;
		}
		menu = this.@interface as IContextRequestableMenu;
		return this.implementor;
	}

	public bool AsMenu<IContextRequestableMenuType>(out IContextRequestableMenuType menu)
	where IContextRequestableMenuType : class, IContextRequestableMenu
	{
		IContextRequestableMenu contextRequestableMenu;
		if (!this.AsMenu(out contextRequestableMenu))
		{
			menu = (IContextRequestableMenuType)null;
			return false;
		}
		IContextRequestableMenuType contextRequestableMenuType = (IContextRequestableMenuType)(contextRequestableMenu as IContextRequestableMenuType);
		IContextRequestableMenuType contextRequestableMenuType1 = contextRequestableMenuType;
		menu = contextRequestableMenuType;
		return !object.ReferenceEquals(contextRequestableMenuType1, null);
	}

	public bool AsQuick(out IContextRequestableQuick quick)
	{
		if (!this.isQuick)
		{
			quick = null;
			return false;
		}
		quick = this.@interface as IContextRequestableQuick;
		return this.implementor;
	}

	public bool AsQuick<IContextRequestableQuickType>(out IContextRequestableQuickType quick)
	where IContextRequestableQuickType : class, IContextRequestableQuick
	{
		IContextRequestableQuick contextRequestableQuick;
		if (!this.AsQuick(out contextRequestableQuick))
		{
			quick = (IContextRequestableQuickType)null;
			return false;
		}
		IContextRequestableQuickType contextRequestableQuickType = (IContextRequestableQuickType)(contextRequestableQuick as IContextRequestableQuickType);
		IContextRequestableQuickType contextRequestableQuickType1 = contextRequestableQuickType;
		quick = contextRequestableQuickType;
		return !object.ReferenceEquals(contextRequestableQuickType1, null);
	}

	public static bool ContextOf(Facepunch.NetworkView networkView, out Contextual contextual)
	{
		return Contextual.GetMB(networkView, out contextual);
	}

	public static bool ContextOf(NGCView networkView, out Contextual contextual)
	{
		return Contextual.GetMB(networkView, out contextual);
	}

	public static bool ContextOf(uLink.NetworkViewID networkViewID, out Contextual contextual)
	{
		return Contextual.GetMB(Facepunch.NetworkView.Find(networkViewID), out contextual);
	}

	public static bool ContextOf(NetEntityID entityID, out Contextual contextual)
	{
		return Contextual.GetMB(entityID.view, out contextual);
	}

	public static bool ContextOf(GameObject gameObject, out Contextual contextual)
	{
		UnityEngine.MonoBehaviour monoBehaviour;
		if ((int)NetEntityID.Of(gameObject, out monoBehaviour) != 0)
		{
			return Contextual.GetMB(monoBehaviour, out contextual);
		}
		contextual = null;
		return false;
	}

	public static bool ContextOf(Component component, out Contextual contextual)
	{
		UnityEngine.MonoBehaviour monoBehaviour;
		if ((int)NetEntityID.Of(component, out monoBehaviour) != 0)
		{
			return Contextual.GetMB(monoBehaviour, out contextual);
		}
		contextual = null;
		return false;
	}

	public static bool FindUp(Transform transform, out Contextual contextual)
	{
		while (transform)
		{
			Contextual component = transform.GetComponent<Contextual>();
			Contextual contextual1 = component;
			contextual = component;
			if (contextual1)
			{
				return true;
			}
			transform = transform.parent;
		}
		contextual = null;
		return false;
	}

	private static bool GetMB(UnityEngine.MonoBehaviour networkView, out Contextual contextual)
	{
		if (networkView)
		{
			Contextual component = networkView.GetComponent<Contextual>();
			Contextual contextual1 = component;
			contextual = component;
			if (contextual1)
			{
				return contextual.exists;
			}
		}
		contextual = null;
		return false;
	}

	private void Refresh()
	{
		this.implementation = this._implementation;
		this._implementation = null;
		this._requestable = this.implementation as IContextRequestable;
		this._implemented = this._requestable != null;
		if (!this._implemented)
		{
			Debug.LogWarning("implementation is null or does not implement IContextRequestable", this);
		}
	}
}