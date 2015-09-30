using Facepunch;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public sealed class ClientVitalsSync : IDLocalCharacterAddon, IInterpTimedEventReceiver
{
	private const IDLocalCharacterAddon.AddonFlags ClientVitalsSyncAddonFlags = IDLocalCharacterAddon.AddonFlags.FireOnAddonPostAwake | IDLocalCharacterAddon.AddonFlags.PrerequisitCheck;

	[NonSerialized]
	private HumanBodyTakeDamage humanBodyTakeDamage;

	private static HUDDirectionalDamage hudDamagePrefab;

	public bool bleeding
	{
		get
		{
			return (!this.humanBodyTakeDamage ? false : this.humanBodyTakeDamage.IsBleeding());
		}
	}

	public ClientVitalsSync() : this(IDLocalCharacterAddon.AddonFlags.FireOnAddonPostAwake | IDLocalCharacterAddon.AddonFlags.PrerequisitCheck)
	{
	}

	protected ClientVitalsSync(IDLocalCharacterAddon.AddonFlags addonFlags) : base(addonFlags)
	{
	}

	protected override bool CheckPrerequesits()
	{
		this.humanBodyTakeDamage = base.takeDamage as HumanBodyTakeDamage;
		return (!this.humanBodyTakeDamage ? false : base.networkViewOwner.isClient);
	}

	public void ClientHealthChange(float amount, GameObject attacker)
	{
		Vector3 vector3;
		Character character;
		float single = base.health;
		base.AdjustClientSideHealth(amount);
		float single1 = Mathf.Abs(amount - single);
		bool flag = amount < single;
		float single2 = base.healthFraction;
		if (base.localControlled && single1 >= 1f)
		{
			base.GetComponent<LocalDamageDisplay>().SetNewHealthPercent(single2, attacker);
		}
		if (attacker && flag && single1 >= 1f && (ClientVitalsSync.hudDamagePrefab || Bundling.Load<HUDDirectionalDamage>("content/hud/DirectionalDamage", out ClientVitalsSync.hudDamagePrefab)))
		{
			vector3 = (!IDBase.GetMain<Character>(attacker, out character) ? base.origin - attacker.transform.position : base.eyesOrigin - character.eyesOrigin);
			HUDDirectionalDamage.CreateIndicator(vector3, (double)amount, NetCull.time, 1.60000002384186, ClientVitalsSync.hudDamagePrefab);
		}
		RPOS.HealthUpdate(amount);
	}

	void IInterpTimedEventReceiver.OnInterpTimedEvent()
	{
		int num;
		string tag = InterpTimedEvent.Tag;
		if (tag != null)
		{
			if (ClientVitalsSync.<>f__switch$map2 == null)
			{
				ClientVitalsSync.<>f__switch$map2 = new Dictionary<string, int>(1)
				{
					{ "DMG", 0 }
				};
			}
			if (ClientVitalsSync.<>f__switch$map2.TryGetValue(tag, out num))
			{
				if (num != 0)
				{
					InterpTimedEvent.MarkUnhandled();
					return;
				}
				this.ClientHealthChange(InterpTimedEvent.Argument<float>(0), InterpTimedEvent.Argument<GameObject>(1));
				return;
			}
		}
		InterpTimedEvent.MarkUnhandled();
	}

	[RPC]
	public void Local_BleedChange(float amount)
	{
		if (this.humanBodyTakeDamage)
		{
			this.humanBodyTakeDamage._bleedingLevel = amount;
		}
		if (base.localControlled)
		{
			RPOS.SetPlaqueActive("PlaqueBleeding", this.humanBodyTakeDamage._bleedingLevel > 0f);
		}
	}

	[RPC]
	public void Local_HealthChange(float amount, uLink.NetworkViewID attackerID, uLink.NetworkMessageInfo info)
	{
		GameObject gameObject;
		object[] objArray;
		bool flag;
		if (attackerID != uLink.NetworkViewID.unassigned)
		{
			uLink.NetworkView networkView = uLink.NetworkView.Find(attackerID);
			uLink.NetworkView networkView1 = networkView;
			if (!networkView)
			{
				gameObject = null;
				objArray = new object[] { amount, gameObject };
				flag = InterpTimedEvent.Queue(this, "DMG", ref info, objArray);
				return;
			}
			gameObject = networkView1.gameObject;
			objArray = new object[] { amount, gameObject };
			flag = InterpTimedEvent.Queue(this, "DMG", ref info, objArray);
			return;
		}
		gameObject = null;
		objArray = new object[] { amount, gameObject };
		flag = InterpTimedEvent.Queue(this, "DMG", ref info, objArray);
	}

	protected override void OnAddonPostAwake()
	{
	}
}