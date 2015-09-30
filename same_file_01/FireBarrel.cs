using Facepunch;
using RustProto;
using RustProto.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using uLink;
using UnityEngine;

[NGCAutoAddScript]
public class FireBarrel : LootableObject, IServerSaveable, IActivatable, IActivatableToggle, IContextRequestable, IContextRequestableMenu, IContextRequestableQuick, IContextRequestableText, IContextRequestablePointText, IComponentInterface<IActivatable, Facepunch.MonoBehaviour, Activatable>, IComponentInterface<IActivatable, Facepunch.MonoBehaviour>, IComponentInterface<IActivatable>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour, Contextual>, IComponentInterface<IContextRequestable, Facepunch.MonoBehaviour>, IComponentInterface<IContextRequestable>
{
	public Light fireLight;

	public ParticleSystem[] emitters;

	public bool isOn;

	public bool startOn;

	private Vector3 _lightPosInitial;

	private Vector3 _lightPosCurrent;

	private Vector3 _lightPosTarget;

	private float _lightFlickerTarget = 1f;

	private float _lightIntensityInitial = 1f;

	public HeatZone _heatZone;

	public static float decayResetRange;

	private DeployableObject _deployable;

	public int myTemp = 1;

	private readonly static FireBarrel.FireBarrelPrototype optionIgnite;

	private readonly static FireBarrel.FireBarrelPrototype optionExtinguish;

	private readonly static FireBarrel.FireBarrelPrototype optionOpen;

	static FireBarrel()
	{
		FireBarrel.decayResetRange = 5f;
		FireBarrel.optionIgnite = new FireBarrel.FireBarrelPrototype(FireBarrel.FireBarrelAction.Ignite);
		FireBarrel.optionExtinguish = new FireBarrel.FireBarrelPrototype(FireBarrel.FireBarrelAction.Extinguish);
		FireBarrel.optionOpen = new FireBarrel.FireBarrelPrototype(FireBarrel.FireBarrelAction.Open);
	}

	public FireBarrel()
	{
	}

	public ActivationToggleState ActGetToggleState()
	{
		return (!this.isOn ? ActivationToggleState.Off : ActivationToggleState.On);
	}

	public ActivationResult ActTrigger(Character instigator, ActivationToggleState toggleTarget, ulong timestamp)
	{
		ActivationToggleState activationToggleState = toggleTarget;
		if (activationToggleState == ActivationToggleState.On)
		{
			if (this.isOn)
			{
				return ActivationResult.Fail_Redundant;
			}
			this.PlayerUse(null);
			return (!this.isOn ? ActivationResult.Fail_Busy : ActivationResult.Success);
		}
		if (activationToggleState != ActivationToggleState.Off)
		{
			return ActivationResult.Fail_BadToggle;
		}
		if (!this.isOn)
		{
			return ActivationResult.Fail_Redundant;
		}
		this.PlayerUse(null);
		return (!this.isOn ? ActivationResult.Success : ActivationResult.Fail_Busy);
	}

	public ActivationResult ActTrigger(Character instigator, ulong timestamp)
	{
		return this.ActTrigger(instigator, (!this.isOn ? ActivationToggleState.On : ActivationToggleState.Off), timestamp);
	}

	public void Awake()
	{
		this._lightPosInitial = this.fireLight.transform.localPosition;
		this._lightPosCurrent = this._lightPosInitial;
		this._lightPosTarget = this._lightPosCurrent;
		this._lightIntensityInitial = this.fireLight.intensity;
	}

	[DebuggerHidden]
	protected IEnumerable<ContextActionPrototype> ContextQueryMenu_FireBarrel(Controllable controllable, ulong timestamp)
	{
		FireBarrel.<ContextQueryMenu_FireBarrel>c__IteratorB variable = null;
		return variable;
	}

	protected ContextResponse ContextRespond_SetFireBarrelOn(Controllable controllable, ulong timestamp, bool turnOn)
	{
		if (this.isOn == turnOn)
		{
			return ContextResponse.DoneBreak;
		}
		if (this.isOn && !this.HasFuel())
		{
			return ContextResponse.FailBreak;
		}
		this.TrySetOn(!this.isOn);
		if (this.isOn != turnOn)
		{
			return ContextResponse.DoneBreak;
		}
		return ContextResponse.FailBreak;
	}

	protected ContextResponse ContextRespondMenu_FireBarrel(Controllable controllable, FireBarrel.FireBarrelPrototype action, ulong timestamp)
	{
		bool flag = action == FireBarrel.optionIgnite;
		if (flag || action == FireBarrel.optionExtinguish)
		{
			return this.ContextRespond_SetFireBarrelOn(controllable, timestamp, flag);
		}
		if (action != FireBarrel.optionOpen)
		{
			return ContextResponse.FailBreak;
		}
		return this.ContextRespond_OpenLoot(controllable, timestamp);
	}

	protected ContextResponse ContextRespondQuick_FireBarrel(Controllable controllable, ulong timestamp)
	{
		if (this.isOn)
		{
			return this.ContextRespond_SetFireBarrelOn(controllable, timestamp, false);
		}
		if (!this.HasFuel())
		{
			return this.ContextRespond_OpenLoot(controllable, timestamp);
		}
		return this.ContextRespond_SetFireBarrelOn(controllable, timestamp, true);
	}

	public override string ContextText(Controllable localControllable)
	{
		PlayerClient playerClient;
		if (this._currentlyUsingPlayer == uLink.NetworkPlayer.unassigned)
		{
			return "Use";
		}
		if (this.occupierText == null)
		{
			if (PlayerClient.Find(this._currentlyUsingPlayer, out playerClient))
			{
				this.occupierText = string.Format("Occupied by {0}", playerClient.userName);
			}
			else
			{
				this.occupierText = "Occupied";
			}
		}
		return this.occupierText;
	}

	public override bool ContextTextPoint(out Vector3 worldPoint)
	{
		ContextRequestable.PointUtil.SpriteOrOrigin(this, out worldPoint);
		return true;
	}

	public IFlammableItem FindFuel()
	{
		IFlammableItem flammableItem;
		IEnumerator<IFlammableItem> enumerator = this._inventory.FindItems<IFlammableItem>().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IFlammableItem current = enumerator.Current;
				if (!current.flammable)
				{
					continue;
				}
				flammableItem = current;
				return flammableItem;
			}
			return null;
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		return flammableItem;
	}

	public void FuelRemoveCheck()
	{
		if (!this.HasFuel())
		{
			this.SetOn(false);
		}
	}

	protected virtual float GetCookDuration()
	{
		return 60f;
	}

	public virtual bool HasFuel()
	{
		return this.FindFuel() != null;
	}

	public void InvItemAdded()
	{
	}

	public void InvItemRemoved()
	{
	}

	private void NewFlickerTarget()
	{
		this._lightFlickerTarget = this._lightIntensityInitial * UnityEngine.Random.Range(0.75f, 1.25f);
	}

	protected new void OnDestroy()
	{
		this.TurnOff();
		base.OnDestroy();
	}

	private void PlayerUse(Controllable looter)
	{
		this.TrySetOn(!this.isOn);
	}

	public void ReadObjectSave(ref SavedObject saveobj)
	{
		if (!saveobj.HasFireBarrel)
		{
			return;
		}
		this.SetOn(saveobj.FireBarrel.OnFire);
	}

	[RPC]
	protected void ReceiveNetState(bool on)
	{
		this.SetOn(on);
	}

	public void SetOn(bool on)
	{
		this.isOn = on;
		if (!this.isOn)
		{
			this.TurnOff();
		}
		else
		{
			this.TurnOn();
		}
	}

	public virtual void TrySetOn(bool on)
	{
		this.SetOn(on);
	}

	private void TurnOff()
	{
		if (base.audio)
		{
			base.audio.Stop();
		}
		ParticleSystem[] particleSystemArray = this.emitters;
		for (int i = 0; i < (int)particleSystemArray.Length; i++)
		{
			particleSystemArray[i].Stop();
		}
		if (this.fireLight)
		{
			this.fireLight.enabled = false;
			this.fireLight.intensity = 0f;
		}
	}

	private void TurnOn()
	{
		this.fireLight.enabled = true;
		ParticleSystem[] particleSystemArray = this.emitters;
		for (int i = 0; i < (int)particleSystemArray.Length; i++)
		{
			particleSystemArray[i].Play();
		}
		base.audio.Play();
		this.NewFlickerTarget();
	}

	public void Update()
	{
		if (this.isOn)
		{
			if (this.fireLight.transform.localPosition == this._lightPosTarget)
			{
				this._lightPosTarget = this._lightPosInitial + (UnityEngine.Random.insideUnitSphere * 10f);
				this._lightPosCurrent = this.fireLight.transform.localPosition;
			}
			this.fireLight.intensity = Mathf.Lerp(this.fireLight.intensity, this._lightFlickerTarget, Time.deltaTime * 10f);
			if ((double)Mathf.Abs(this.fireLight.intensity - this._lightFlickerTarget) < 0.05)
			{
				this.NewFlickerTarget();
			}
		}
	}

	public void WriteObjectSave(ref SavedObject.Builder saveobj)
	{
		using (Recycler<objectFireBarrel, objectFireBarrel.Builder> recycler = objectFireBarrel.Recycler())
		{
			objectFireBarrel.Builder builder = recycler.OpenBuilder();
			builder.SetOnFire(this.isOn);
			saveobj.SetFireBarrel(builder);
		}
	}

	public static class DefaultItems
	{
		public static Datablock.Ident fuel;

		public static Datablock.Ident byProduct;

		static DefaultItems()
		{
			FireBarrel.DefaultItems.fuel = "Wood";
			FireBarrel.DefaultItems.byProduct = "Charcoal";
		}
	}

	protected enum FireBarrelAction
	{
		Ignite,
		Extinguish,
		Open
	}

	protected class FireBarrelPrototype : ContextActionPrototype
	{
		public FireBarrel.FireBarrelAction action;

		public FireBarrelPrototype(FireBarrel.FireBarrelAction action)
		{
			this.name = (int)action;
			this.text = action.ToString();
			this.action = action;
		}
	}
}