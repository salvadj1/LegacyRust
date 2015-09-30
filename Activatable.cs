using Facepunch;
using System;
using uLink;
using UnityEngine;

[InterfaceDriverComponent(typeof(IActivatable), "_implementation", "implementation", SearchRoute=InterfaceSearchRoute.GameObject, UnityType=typeof(Facepunch.MonoBehaviour), AlwaysSaveDisabled=true)]
public sealed class Activatable : UnityEngine.MonoBehaviour, IComponentInterfaceDriver<IActivatable, Facepunch.MonoBehaviour, Activatable>
{
	[SerializeField]
	private Facepunch.MonoBehaviour _implementation;

	private Facepunch.MonoBehaviour implementation;

	private IActivatable act;

	private bool canAct;

	private IActivatableToggle actToggle;

	private bool canToggle;

	private ActivatableInfo info;

	[NonSerialized]
	private bool _implemented;

	[NonSerialized]
	private bool _awoke;

	public Activatable driver
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

	public IActivatable @interface
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
			return this.act;
		}
	}

	public bool isToggle
	{
		get
		{
			return this.canToggle;
		}
	}

	public ActivationToggleState toggleState
	{
		get
		{
			return (!this.canToggle || !this.implementation ? ActivationToggleState.Unspecified : this.actToggle.ActGetToggleState());
		}
	}

	public Activatable()
	{
	}

	private ActivationResult Act(Character instigator, ulong timestamp)
	{
		ActivationResult activationResult;
		if (!this.canAct)
		{
			activationResult = ActivationResult.Error_Implementation;
		}
		else
		{
			activationResult = (!this.implementation ? ActivationResult.Error_Destroyed : this.act.ActTrigger(instigator, timestamp));
		}
		return activationResult;
	}

	private ActivationResult Act(Character instigator, ActivationToggleState state, ulong timestamp)
	{
		ActivationResult activationResult;
		if (!this.canToggle)
		{
			activationResult = ActivationResult.Error_Implementation;
		}
		else
		{
			activationResult = (!this.implementation ? ActivationResult.Error_Destroyed : this.actToggle.ActTrigger(instigator, state, timestamp));
		}
		return activationResult;
	}

	public ActivationResult Activate(ulong timestamp)
	{
		return this.Activate(null, timestamp);
	}

	public ActivationResult Activate(Character instigator, ulong timestamp)
	{
		throw new NotSupportedException("Server only");
	}

	public ActivationResult Activate()
	{
		return this.Activate(null, NetCull.timeInMillis);
	}

	public ActivationResult Activate(bool on, Character instigator, ulong timestamp)
	{
		throw new NotSupportedException("Server only");
	}

	public ActivationResult Activate(bool on, Character instigator)
	{
		return this.Activate(on, instigator, NetCull.timeInMillis);
	}

	public ActivationResult Activate(bool on, ulong timestamp)
	{
		return this.Activate(on, null, timestamp);
	}

	public ActivationResult Activate(bool on)
	{
		return this.Activate(on, null, NetCull.timeInMillis);
	}

	public ActivationResult Activate(ref uLink.NetworkMessageInfo info)
	{
		bool? nullable = null;
		return this.ActRoute(nullable, info.sender, info.timestampInMillis);
	}

	public ActivationResult Activate(bool on, ref uLink.NetworkMessageInfo info)
	{
		return this.ActRoute(new bool?(on), info.sender, info.timestampInMillis);
	}

	private ActivationResult ActRoute(bool? on, Character character, ulong timestamp)
	{
		if (!on.HasValue)
		{
			return this.Activate(character, timestamp);
		}
		return this.Activate(on.Value, character, timestamp);
	}

	private ActivationResult ActRoute(bool? on, Controllable controllable, ulong timestamp)
	{
		Character component;
		bool? nullable = on;
		if (!controllable)
		{
			component = null;
		}
		else
		{
			component = controllable.GetComponent<Character>();
		}
		return this.ActRoute(nullable, component, timestamp);
	}

	private ActivationResult ActRoute(bool? on, PlayerClient sender, ulong timestamp)
	{
		Controllable controllable;
		bool? nullable = on;
		if (!sender || !sender.controllable)
		{
			controllable = null;
		}
		else
		{
			controllable = sender.controllable;
		}
		return this.ActRoute(nullable, controllable, timestamp);
	}

	private ActivationResult ActRoute(bool? on, uLink.NetworkPlayer sender, ulong timestamp)
	{
		PlayerClient playerClient;
		ServerManagement serverManagement = ServerManagement.Get();
		if (!serverManagement)
		{
			playerClient = null;
		}
		else
		{
			serverManagement.GetPlayerClient(sender, out playerClient);
		}
		return this.ActRoute(on, playerClient, timestamp);
	}

	private void Awake()
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
	}

	private void OnDestroy()
	{
		if (this.implementation)
		{
			IActivatableFill activatableFill = this.implementation as IActivatableFill;
			if (activatableFill != null)
			{
				activatableFill.ActivatableChanged(this, false);
			}
		}
		this.implementation = null;
		this.canAct = false;
		this.canToggle = false;
		this.act = null;
		this.actToggle = null;
		this.info = new ActivatableInfo();
	}

	private void Refresh()
	{
		this.implementation = this._implementation;
		this._implementation = null;
		this.act = this.implementation as IActivatable;
		this.canAct = this.act != null;
		if (!this.canAct)
		{
			Debug.LogWarning("implementation is null or does not implement IActivatable", this);
		}
		else
		{
			this.actToggle = this.implementation as IActivatableToggle;
			this.canToggle = this.actToggle != null;
			IActivatableFill activatableFill = this.implementation as IActivatableFill;
			if (activatableFill != null)
			{
				activatableFill.ActivatableChanged(this, true);
			}
			IActivatableInfo activatableInfo = this.implementation as IActivatableInfo;
			if (activatableInfo != null)
			{
				activatableInfo.ActInfo(out this.info);
			}
		}
	}

	private void Reset()
	{
		if (!this.canAct)
		{
			Facepunch.MonoBehaviour[] components = base.GetComponents<Facepunch.MonoBehaviour>();
			int num = 0;
			while (num < (int)components.Length)
			{
				Facepunch.MonoBehaviour monoBehaviour = components[num];
				if (!(monoBehaviour != this) || !(monoBehaviour is IActivatable))
				{
					num++;
				}
				else
				{
					this._implementation = monoBehaviour;
					break;
				}
			}
		}
	}
}