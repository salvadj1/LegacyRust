using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

public sealed class DeathTransfer : IDLocalCharacterAddon, IInterpTimedEventReceiver
{
	private const IDLocalCharacterAddon.AddonFlags DeathTransferAddonFlags = 0;

	[NonSerialized]
	private QueuedShotDeathInfo deathShot;

	[NonSerialized]
	private GameObject _ragdollInstance;

	public DeathTransfer() : this(0)
	{
	}

	protected DeathTransfer(IDLocalCharacterAddon.AddonFlags addonFlags) : base(addonFlags)
	{
	}

	[RPC]
	protected void Client_OnKilled(uLink.NetworkMessageInfo info)
	{
		this.Client_OnKilledShared(false, null, ref info);
	}

	[RPC]
	protected void Client_OnKilledBy(uLink.NetworkViewID attackerViewID, uLink.NetworkMessageInfo info)
	{
		uLink.NetworkView networkView = uLink.NetworkView.Find(attackerViewID);
		if (networkView)
		{
			Character component = networkView.GetComponent<Character>();
			this.Client_OnKilledShared(component, component, ref info);
		}
		else
		{
			this.Client_OnKilledShared(false, null, ref info);
		}
	}

	private void Client_OnKilledShared(bool killedBy, Character attacker, ref uLink.NetworkMessageInfo info)
	{
		Controllable controllable;
		bool flag;
		AudioClipArray trait = base.GetTrait<CharacterSoundsTrait>().death;
		AudioClip item = trait[UnityEngine.Random.Range(0, trait.Length)];
		item.Play(base.transform.position, 1f, 1f, 10f);
		bool flag1 = base.localControlled;
		if (flag1)
		{
			base.rposLimitFlags = RPOSLimitFlags.KeepOff | RPOSLimitFlags.HideInventory | RPOSLimitFlags.HideContext | RPOSLimitFlags.HideSprites;
			DeathScreen.Show();
		}
		BaseHitBox remote = base.idMain.GetRemote<BaseHitBox>();
		if (remote)
		{
			remote.collider.enabled = false;
		}
		if (!killedBy || !attacker)
		{
			controllable = null;
			flag = false;
		}
		else
		{
			controllable = attacker.controllable;
			flag = (!controllable ? false : controllable.localPlayerControlled);
		}
		base.AdjustClientSideHealth(0f);
		if (flag1 || flag)
		{
			InterpTimedEvent.Queue(this, "ClientLocalDeath", ref info);
			if (!flag1)
			{
				InterpTimedEvent.Remove(this, true);
			}
			else
			{
				InterpTimedEvent.Clear(true);
			}
		}
		else
		{
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
			for (int i = 0; i < (int)componentsInChildren.Length; i++)
			{
				Collider collider = componentsInChildren[i];
				if (collider)
				{
					collider.enabled = false;
				}
			}
			InterpTimedEvent.Queue(this, "RAG", ref info);
			NetCull.DontDestroyWithNetwork(this);
		}
	}

	[RPC]
	protected void Client_OnKilledShot(Vector3 point, Angle2 normal, byte bodyPart, uLink.NetworkMessageInfo info)
	{
		this.Client_ShotShared(ref point, ref normal, bodyPart, ref info);
		this.Client_OnKilled(info);
	}

	[RPC]
	protected void Client_OnKilledShotBy(uLink.NetworkViewID attackerViewID, Vector3 point, Angle2 normal, byte bodyPart, uLink.NetworkMessageInfo info)
	{
		this.Client_ShotShared(ref point, ref normal, bodyPart, ref info);
		this.Client_OnKilledBy(attackerViewID, info);
	}

	private void Client_ShotShared(ref Vector3 point, ref Angle2 normal, byte bodyPart, ref uLink.NetworkMessageInfo info)
	{
		this.deathShot.Set(base.idMain.hitBoxSystem, ref point, ref normal, bodyPart, ref info);
	}

	private void ClientLocalDeath()
	{
		Transform transforms;
		bool flag;
		Ragdoll ragdoll = this.DeathRagdoll();
		if (base.localControlled)
		{
			if (!actor.forceThirdPerson)
			{
				CameraMount componentInChildren = base.GetComponentInChildren<CameraMount>();
				if (!componentInChildren || !componentInChildren.open)
				{
					Debug.Log("No camera?");
				}
				else
				{
					RagdollTransferInfoProvider ragdollTransferInfoProvider = base.controller as RagdollTransferInfoProvider;
					if (ragdollTransferInfoProvider == null)
					{
						transforms = null;
						flag = false;
					}
					else
					{
						try
						{
							RagdollTransferInfo ragdollTransferInfo = ragdollTransferInfoProvider.RagdollTransferInfo;
							flag = ragdollTransferInfo.FindHead(ragdoll.transform, out transforms);
						}
						catch (Exception exception)
						{
							Debug.LogException(exception, this);
							transforms = null;
							flag = false;
						}
					}
					if (flag)
					{
						Vector3 vector3 = transforms.InverseTransformPoint(componentInChildren.transform.position);
						vector3.y = vector3.y + 0.08f;
						Vector3 vector31 = transforms.TransformPoint(vector3);
						Ragdoll ragdoll1 = ragdoll;
						ragdoll1.origin = ragdoll1.origin + (vector31 - transforms.position);
						CameraMount.CreateTemporaryCameraMount(componentInChildren, transforms).camera.nearClipPlane = 0.02f;
					}
					ArmorModelRenderer local = ragdoll.GetLocal<ArmorModelRenderer>();
					if (local)
					{
						local.enabled = false;
					}
				}
			}
			UnityEngine.Object.Destroy(base.GetComponent<LocalDamageDisplay>());
		}
	}

	private Ragdoll CreateRagdoll()
	{
		CharacterRagdollTrait trait = base.GetTrait<CharacterRagdollTrait>();
		if (!trait)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(trait.ragdollPrefab, base.transform.position, base.transform.rotation) as GameObject;
		Ragdoll component = gameObject.GetComponent<Ragdoll>();
		component.sourceMain = base.idMain;
		this._ragdollInstance = gameObject;
		UnityEngine.Object.Destroy(gameObject, 80f);
		this.deathShot.LinkRagdoll(base.transform, gameObject);
		ArmorModelRenderer local = base.GetLocal<ArmorModelRenderer>();
		ArmorModelRenderer armorModelRenderer = component.GetLocal<ArmorModelRenderer>();
		if (local && armorModelRenderer)
		{
			armorModelRenderer.BindArmorModels(local.GetArmorModelMemberMapCopy());
		}
		return component;
	}

	private Ragdoll DeathRagdoll()
	{
		Ragdoll ragdoll = this.CreateRagdoll();
		PlayerProxyTest component = base.GetComponent<PlayerProxyTest>();
		if (component.body)
		{
			component.body.SetActive(false);
		}
		return ragdoll;
	}

	void IInterpTimedEventReceiver.OnInterpTimedEvent()
	{
		int num;
		string tag = InterpTimedEvent.Tag;
		if (tag != null)
		{
			if (DeathTransfer.<>f__switch$map3 == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(2)
				{
					{ "ClientLocalDeath", 0 },
					{ "RAG", 1 }
				};
				DeathTransfer.<>f__switch$map3 = strs;
			}
			if (DeathTransfer.<>f__switch$map3.TryGetValue(tag, out num))
			{
				if (num == 0)
				{
					try
					{
						this.ClientLocalDeath();
					}
					finally
					{
						if (!base.localControlled)
						{
							UnityEngine.Object.Destroy(base.gameObject);
						}
					}
					return;
				}
				else
				{
					if (num != 1)
					{
						InterpTimedEvent.MarkUnhandled();
						return;
					}
					try
					{
						this.DeathRagdoll();
					}
					finally
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
					return;
				}
			}
		}
		InterpTimedEvent.MarkUnhandled();
	}
}