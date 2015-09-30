using Facepunch;
using System;
using UnityEngine;

[InterfaceDriverComponent(typeof(IUseable), "_implementation", "implementation", SearchRoute=InterfaceSearchRoute.GameObject, UnityType=typeof(Facepunch.MonoBehaviour), AlwaysSaveDisabled=true)]
public sealed class Useable : UnityEngine.MonoBehaviour, IComponentInterfaceDriver<IUseable, Facepunch.MonoBehaviour, Useable>
{
	[SerializeField]
	private Facepunch.MonoBehaviour _implementation;

	[NonSerialized]
	private Facepunch.MonoBehaviour implementation;

	[NonSerialized]
	private IUseable use;

	[NonSerialized]
	private IUseableChecked useCheck;

	[NonSerialized]
	private IUseableNotifyDecline useDecline;

	[NonSerialized]
	private IUseableUpdated useUpdate;

	[NonSerialized]
	private bool canUse;

	[NonSerialized]
	private bool canCheck;

	[NonSerialized]
	private bool wantDeclines;

	[NonSerialized]
	private bool canUpdate;

	[NonSerialized]
	private bool inKillCallback;

	[NonSerialized]
	private bool inDestroy;

	[NonSerialized]
	private bool _implemented;

	[NonSerialized]
	private bool _awoke;

	[NonSerialized]
	private UseUpdateFlags updateFlags;

	[NonSerialized]
	private Character _user;

	[NonSerialized]
	private CharacterDeathSignal onDeathCallback;

	[NonSerialized]
	private Useable.FunctionCallState callState;

	private static bool hasException;

	private static Exception lastException;

	private Useable.UseExitCallback onUseExited;

	public Useable driver
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

	public IUseable @interface
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
			return this.use;
		}
	}

	public bool occupied
	{
		get
		{
			return this._user;
		}
	}

	public Character user
	{
		get
		{
			return this._user;
		}
	}

	public Useable()
	{
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

	private static void ClearException(bool got)
	{
		if (!got)
		{
			Debug.LogWarning(string.Concat("Nothing got previous now clearing exception \r\n", Useable.lastException));
		}
		Useable.lastException = null;
		Useable.hasException = false;
	}

	public bool Eject()
	{
		UseExitReason useExitReason;
		bool flag;
		UseExitReason useExitReason1;
		Useable.EnsureServer();
		if ((int)this.callState == 0)
		{
			if (!this.inDestroy)
			{
				useExitReason1 = (!this.inKillCallback ? UseExitReason.Forced : UseExitReason.Killed);
			}
			else
			{
				useExitReason1 = UseExitReason.Destroy;
			}
			useExitReason = useExitReason1;
		}
		else
		{
			if ((int)this.callState != 4)
			{
				Debug.LogWarning(string.Concat("Some how Eject got called from a call stack originating with ", this.callState, " fix your script to not do this."), this);
				return false;
			}
			useExitReason = UseExitReason.Manual;
		}
		if (!this._user)
		{
			return false;
		}
		try
		{
			if (!this.implementation)
			{
				Debug.LogError(string.Concat("The IUseable has been destroyed with a user on it. IUseable should ALWAYS call UseableUtility.OnDestroy within the script's OnDestroy message first thing! ", base.gameObject), this);
			}
			else
			{
				try
				{
					this.callState = Useable.FunctionCallState.Eject;
					this.use.OnUseExit(this, useExitReason);
				}
				finally
				{
					try
					{
						this.InvokeUseExitCallback();
					}
					finally
					{
						this.callState = Useable.FunctionCallState.None;
					}
				}
			}
			flag = true;
		}
		finally
		{
			this.UnlatchUse();
			this._user = null;
		}
		return flag;
	}

	private static void EnsureServer()
	{
		throw new InvalidOperationException("A function ( Enter, Exit or Eject ) in Useable was called client side. Should have only been called server side.");
	}

	private UseResponse Enter(Character attempt, UseEnterRequest request)
	{
		UseResponse useResponse;
		UseResponse useResponse1;
		if (!this.canUse)
		{
			return UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_Checked_BadConfiguration | UseResponse.Fail_CheckException | UseResponse.Fail_UserDead | UseResponse.Fail_NotIUseable;
		}
		Useable.EnsureServer();
		if ((int)this.callState != 0)
		{
			Debug.LogWarning(string.Concat("Some how Enter got called from a call stack originating with ", this.callState, " fix your script to not do this."), this);
			return UseResponse.Pass_Checked | UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_Checked_UserIncompatible | UseResponse.Fail_Checked_BadConfiguration | UseResponse.Fail_Checked_BadResult | UseResponse.Fail_CheckException | UseResponse.Fail_EnterException | UseResponse.Fail_UserDead | UseResponse.Fail_Destroyed | UseResponse.Fail_NotIUseable | UseResponse.Fail_InvalidOperation;
		}
		if (Useable.hasException)
		{
			Useable.ClearException(false);
		}
		if (!attempt)
		{
			return UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_CheckException | UseResponse.Fail_UserDead | UseResponse.Fail_NullOrMissingUser;
		}
		if (attempt.signaledDeath)
		{
			return UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_CheckException | UseResponse.Fail_UserDead;
		}
		if (this._user)
		{
			if (this._user == attempt)
			{
				if (this.wantDeclines && this.implementation)
				{
					try
					{
						this.useDecline.OnUseDeclined(attempt, UseResponse.Pass_Checked | UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_Checked_UserIncompatible | UseResponse.Fail_Checked_BadConfiguration | UseResponse.Fail_Checked_BadResult | UseResponse.Fail_CheckException | UseResponse.Fail_EnterException | UseResponse.Fail_Vacancy | UseResponse.Fail_Redundant, request);
					}
					catch (Exception exception)
					{
						Debug.LogError(string.Concat("Caught exception in OnUseDeclined \r\n (response was Fail_Redundant)", exception), this.implementation);
					}
				}
				return UseResponse.Pass_Checked | UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_Checked_UserIncompatible | UseResponse.Fail_Checked_BadConfiguration | UseResponse.Fail_Checked_BadResult | UseResponse.Fail_CheckException | UseResponse.Fail_EnterException | UseResponse.Fail_Vacancy | UseResponse.Fail_Redundant;
			}
			if (this.wantDeclines && this.implementation)
			{
				try
				{
					this.useDecline.OnUseDeclined(attempt, UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_Checked_BadConfiguration | UseResponse.Fail_CheckException | UseResponse.Fail_Vacancy, request);
				}
				catch (Exception exception1)
				{
					Debug.LogError(string.Concat("Caught exception in OnUseDeclined \r\n (response was Fail_Vacancy)", exception1), this.implementation);
				}
			}
			return UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_Checked_BadConfiguration | UseResponse.Fail_CheckException | UseResponse.Fail_Vacancy;
		}
		if (!this.implementation)
		{
			return UseResponse.Pass_Checked | UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_Checked_UserIncompatible | UseResponse.Fail_CheckException | UseResponse.Fail_EnterException | UseResponse.Fail_UserDead | UseResponse.Fail_Destroyed;
		}
		try
		{
			this.callState = Useable.FunctionCallState.Enter;
			if (!this.canCheck)
			{
				useResponse = UseResponse.Pass_Unchecked;
			}
			else
			{
				try
				{
					useResponse = (UseResponse)this.useCheck.CanUse(attempt, request);
				}
				catch (Exception exception2)
				{
					Useable.lastException = exception2;
					useResponse1 = UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_CheckException;
					return useResponse1;
				}
				if ((int)useResponse != 1)
				{
					if (!useResponse.Succeeded())
					{
						if (this.wantDeclines)
						{
							try
							{
								this.useDecline.OnUseDeclined(attempt, useResponse, request);
							}
							catch (Exception exception4)
							{
								Exception exception3 = exception4;
								Debug.LogError(string.Concat(new object[] { "Caught exception in OnUseDeclined \r\n (response was ", useResponse, ")", exception3 }), this.implementation);
							}
						}
						useResponse1 = useResponse;
						return useResponse1;
					}
					else
					{
						Debug.LogError(string.Concat("A IUseableChecked return a invalid value that should have cause success [", useResponse, "], but it was not UseCheck.Success! fix your script."), this.implementation);
						useResponse1 = UseResponse.Pass_Checked | UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_Checked_UserIncompatible | UseResponse.Fail_Checked_BadConfiguration | UseResponse.Fail_Checked_BadResult;
						return useResponse1;
					}
				}
			}
			try
			{
				this._user = attempt;
				this.use.OnUseEnter(this);
			}
			catch (Exception exception6)
			{
				Exception exception5 = exception6;
				this._user = null;
				Debug.LogError(string.Concat("Exception thrown during Useable.Enter. Object not set as used!\r\n", exception5), attempt);
				Useable.lastException = exception5;
				useResponse1 = UseResponse.Pass_Checked | UseResponse.Fail_Checked_OutOfOrder | UseResponse.Fail_Checked_UserIncompatible | UseResponse.Fail_CheckException | UseResponse.Fail_EnterException;
				return useResponse1;
			}
			if (useResponse.Succeeded())
			{
				this.LatchUse();
			}
			useResponse1 = useResponse;
		}
		finally
		{
			this.callState = Useable.FunctionCallState.None;
		}
		return useResponse1;
	}

	public UseResponse EnterFromContext(Character attempt)
	{
		return this.Enter(attempt, UseEnterRequest.Context);
	}

	public UseResponse EnterFromElsewhere(Character attempt)
	{
		return this.Enter(attempt, UseEnterRequest.Elsewhere);
	}

	public bool Exit(Character attempt)
	{
		bool flag;
		Useable.EnsureServer();
		if ((int)this.callState != 0)
		{
			Debug.LogWarning(string.Concat("Some how Exit got called from a call stack originating with ", this.callState, " fix your script to not do this."), this);
			return false;
		}
		if (!(attempt == this._user) || !attempt)
		{
			return false;
		}
		try
		{
			if (this.implementation)
			{
				try
				{
					this.callState = Useable.FunctionCallState.Exit;
					this.use.OnUseExit(this, UseExitReason.Manual);
				}
				finally
				{
					this.InvokeUseExitCallback();
					this.callState = Useable.FunctionCallState.None;
				}
			}
			flag = true;
		}
		finally
		{
			this._user = null;
			this.UnlatchUse();
		}
		return flag;
	}

	public static bool GetLastException<E>(out E exception, bool doNotClear)
	where E : Exception
	{
		if (!Useable.hasException || !(Useable.lastException is E))
		{
			exception = (E)null;
			return false;
		}
		exception = (E)Useable.lastException;
		if (!doNotClear)
		{
			Useable.ClearException(true);
		}
		return true;
	}

	public static bool GetLastException<E>(out E exception)
	where E : Exception
	{
		return Useable.GetLastException<E>(out exception, false);
	}

	public static bool GetLastException(out Exception exception, bool doNotClear)
	{
		if (!Useable.hasException)
		{
			exception = null;
			return true;
		}
		exception = Useable.lastException;
		if (!doNotClear)
		{
			Useable.ClearException(true);
		}
		return true;
	}

	public static bool GetLastException(out Exception exception)
	{
		return Useable.GetLastException(out exception, false);
	}

	private void InvokeUseExitCallback()
	{
		if (this.onUseExited != null)
		{
			this.onUseExited(this, (int)this.callState == 3);
		}
	}

	private void KilledCallback(Character user, CharacterDeathSignalReason reason)
	{
		if (!user)
		{
			Debug.LogError("Somehow KilledCallback got a null", this);
		}
		if (user == this._user)
		{
			try
			{
				try
				{
					this.inKillCallback = true;
					if (!this.Eject())
					{
						Debug.LogWarning("Failure to eject??", this);
					}
				}
				catch (Exception exception)
				{
					Debug.LogError(string.Concat("Exception in Eject while inside a killed callback\r\n", exception), user);
				}
			}
			finally
			{
				this.inKillCallback = false;
			}
		}
		else
		{
			Debug.LogError("Some callback invoked kill callback on the Useable but it was not being used by it", user);
		}
	}

	private void LatchUse()
	{
		this._user.signal_death += this.onDeathCallback;
		base.enabled = (this.updateFlags & UseUpdateFlags.UpdateWithUser) == UseUpdateFlags.UpdateWithUser;
	}

	private void OnDestroy()
	{
		this.inDestroy = true;
		if (this._user)
		{
			this.Eject();
		}
		this.canCheck = false;
		this.canUpdate = false;
		this.canUse = false;
		this.wantDeclines = false;
		this.use = null;
		this.useUpdate = null;
		this.useCheck = null;
		this.useDecline = null;
	}

	private void OnEnable()
	{
		Debug.LogError("Something is trying to enable useable on client.", this);
		base.enabled = false;
	}

	private void Refresh()
	{
		this.implementation = this._implementation;
		this._implementation = null;
		this.use = this.implementation as IUseable;
		this.canUse = this.use != null;
		if (!this.canUse)
		{
			Debug.LogWarning("implementation is null or does not implement IUseable", this);
		}
		else
		{
			base.enabled = false;
			this.useDecline = null;
			this.useCheck = null;
			this.updateFlags = UseUpdateFlags.None;
			IUseableAwake useableAwake = this.implementation as IUseableAwake;
			if (useableAwake != null)
			{
				useableAwake.OnUseableAwake(this);
			}
		}
	}

	private void Reset()
	{
		Facepunch.MonoBehaviour[] components = base.GetComponents<Facepunch.MonoBehaviour>();
		for (int i = 0; i < (int)components.Length; i++)
		{
			Facepunch.MonoBehaviour monoBehaviour = components[i];
			if (monoBehaviour is IUseable)
			{
				this._implementation = monoBehaviour;
			}
		}
	}

	private void RunUpdate()
	{
		Useable.FunctionCallState functionCallState = this.callState;
		try
		{
			try
			{
				this.callState = Useable.FunctionCallState.OnUseUpdate;
				this.useUpdate.OnUseUpdate(this);
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Inside OnUseUpdate\r\n", exception), this.implementation);
			}
		}
		finally
		{
			this.callState = functionCallState;
		}
	}

	private void UnlatchUse()
	{
		try
		{
			try
			{
				if (this._user)
				{
					this._user.signal_death -= this.onDeathCallback;
				}
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception caught during unlatch\r\n", exception), this);
			}
		}
		finally
		{
			this._user = null;
		}
	}

	private void Update()
	{
		if (this._user)
		{
			if (!this.implementation)
			{
				base.enabled = false;
			}
			else
			{
				this.RunUpdate();
			}
		}
		else if ((this.updateFlags & UseUpdateFlags.UpdateWithoutUser) != UseUpdateFlags.UpdateWithoutUser)
		{
			Debug.LogWarning("Most likely the user was destroyed without being set up properly!", this);
			base.enabled = false;
		}
		else if (!this.implementation)
		{
			base.enabled = false;
		}
		else
		{
			this.RunUpdate();
		}
	}

	public event Useable.UseExitCallback onUseExited
	{
		add
		{
			this.onUseExited += value;
		}
		remove
		{
			this.onUseExited -= value;
		}
	}

	private enum FunctionCallState : sbyte
	{
		None,
		Enter,
		Exit,
		Eject,
		OnUseUpdate
	}

	public delegate void UseExitCallback(Useable useable, bool wasEjected);
}