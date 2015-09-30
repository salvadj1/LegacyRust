using Facepunch.Actor;
using System;
using uLink;
using UnityEngine;

[NGCAutoAddScript]
public class SleepingAvatar : DeployableObject, IServerSaveable
{
	private const string kPoseName = "sleep";

	protected const string ArmorConfigRPC = "SAAM";

	protected const string SettingLiveCharacterNowRPC = "SACH";

	protected const string HasDiedNowRPC = "SAKL";

	[NonSerialized]
	public ArmorDataBlock footArmor;

	[NonSerialized]
	public ArmorDataBlock legArmor;

	[NonSerialized]
	public ArmorDataBlock torsoArmor;

	[NonSerialized]
	public ArmorDataBlock headArmor;

	public MeshFilter footMeshFilter;

	public MeshFilter legMeshFilter;

	public MeshFilter torsoMeshFilter;

	public MeshFilter headMeshFilter;

	public Ragdoll ragdollPrefab;

	[NonSerialized]
	private Ragdoll ragdollInstance;

	public MeshRenderer footRenderer
	{
		get
		{
			MeshRenderer meshRenderer;
			if (!this.footMeshFilter)
			{
				meshRenderer = null;
			}
			else
			{
				meshRenderer = this.footMeshFilter.renderer as MeshRenderer;
			}
			return meshRenderer;
		}
	}

	public MeshRenderer headRenderer
	{
		get
		{
			MeshRenderer meshRenderer;
			if (!this.headMeshFilter)
			{
				meshRenderer = null;
			}
			else
			{
				meshRenderer = this.headMeshFilter.renderer as MeshRenderer;
			}
			return meshRenderer;
		}
	}

	public MeshRenderer legRenderer
	{
		get
		{
			MeshRenderer meshRenderer;
			if (!this.legMeshFilter)
			{
				meshRenderer = null;
			}
			else
			{
				meshRenderer = this.legMeshFilter.renderer as MeshRenderer;
			}
			return meshRenderer;
		}
	}

	public MeshRenderer torsoRenderer
	{
		get
		{
			MeshRenderer meshRenderer;
			if (!this.torsoMeshFilter)
			{
				meshRenderer = null;
			}
			else
			{
				meshRenderer = this.torsoMeshFilter.renderer as MeshRenderer;
			}
			return meshRenderer;
		}
	}

	public SleepingAvatar()
	{
	}

	private static bool BindArmorMap<TArmorModel>(ArmorDataBlock armor, ref ArmorModelMemberMap map)
	where TArmorModel : ArmorModel, new()
	{
		if (armor)
		{
			TArmorModel armorModel = armor.GetArmorModel<TArmorModel>();
			if (armorModel)
			{
				map.SetArmorModel<TArmorModel>(armorModel);
				return true;
			}
		}
		return false;
	}

	private static void BindRenderer<TArmorModel>(ArmorModelRenderer prefabRenderer, ArmorDataBlock armor, MeshFilter filter, MeshRenderer renderer)
	where TArmorModel : ArmorModel<TArmorModel>, new()
	{
		TArmorModel armorModel;
		Mesh mesh;
		if (!armor)
		{
			if (!prefabRenderer)
			{
				return;
			}
			armorModel = prefabRenderer.GetArmorModel<TArmorModel>();
		}
		else
		{
			armorModel = armor.GetArmorModel<TArmorModel>();
			if (!armorModel && prefabRenderer)
			{
				armorModel = prefabRenderer.GetArmorModel<TArmorModel>();
			}
		}
		if (!armorModel)
		{
			return;
		}
		if (ArmorModelRenderer.Censored && armorModel.censoredModel)
		{
			armorModel = armorModel.censoredModel;
		}
		if (armorModel && armorModel.actorMeshInfo.FindPose("sleep", out mesh))
		{
			filter.sharedMesh = mesh;
			renderer.sharedMaterials = armorModel.sharedMaterials;
		}
	}

	private bool CreateRagdoll()
	{
		AnimationClip animationClip;
		float single;
		if (this.ragdollPrefab)
		{
			ArmorModelRenderer local = this.ragdollPrefab.GetLocal<ArmorModelRenderer>();
			if (local)
			{
				ActorRig actorRig = local.actorRig;
				if (actorRig)
				{
					if (actorRig.FindPoseClip("sleep", out animationClip, out single))
					{
						this.ragdollInstance = UnityEngine.Object.Instantiate(this.ragdollPrefab, base.transform.position, base.transform.rotation) as Ragdoll;
						this.ragdollInstance.sourceMain = this;
						GameObject gameObject = this.ragdollInstance.gameObject;
						UnityEngine.Object.Destroy(gameObject, 80f);
						gameObject.SampleAnimation(animationClip, single);
						local = this.ragdollInstance.GetLocal<ArmorModelRenderer>();
						ArmorModelMemberMap armorModelMemberMaps = new ArmorModelMemberMap();
						if (false | SleepingAvatar.BindArmorMap<ArmorModelFeet>(this.footArmor, ref armorModelMemberMaps) | SleepingAvatar.BindArmorMap<ArmorModelLegs>(this.legArmor, ref armorModelMemberMaps) | SleepingAvatar.BindArmorMap<ArmorModelTorso>(this.torsoArmor, ref armorModelMemberMaps) | SleepingAvatar.BindArmorMap<ArmorModelHead>(this.headArmor, ref armorModelMemberMaps))
						{
							local.BindArmorModels(armorModelMemberMaps);
						}
						return true;
					}
				}
			}
		}
		return false;
	}

	public static void RebindAllRenderers()
	{
		UnityEngine.Object[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(SleepingAvatar));
		for (int i = 0; i < (int)objArray.Length; i++)
		{
			SleepingAvatar sleepingAvatar = (SleepingAvatar)objArray[i];
			if (sleepingAvatar)
			{
				sleepingAvatar.RebindRenderers();
			}
		}
	}

	private void RebindRenderers()
	{
		ArmorModelRenderer local;
		if (!this.ragdollPrefab)
		{
			local = null;
		}
		else
		{
			local = this.ragdollPrefab.GetLocal<ArmorModelRenderer>();
		}
		ArmorModelRenderer armorModelRenderer = local;
		SleepingAvatar.BindRenderer<ArmorModelFeet>(armorModelRenderer, this.footArmor, this.footMeshFilter, this.footRenderer);
		SleepingAvatar.BindRenderer<ArmorModelLegs>(armorModelRenderer, this.legArmor, this.legMeshFilter, this.legRenderer);
		SleepingAvatar.BindRenderer<ArmorModelTorso>(armorModelRenderer, this.torsoArmor, this.torsoMeshFilter, this.torsoRenderer);
		SleepingAvatar.BindRenderer<ArmorModelHead>(armorModelRenderer, this.headArmor, this.headMeshFilter, this.headRenderer);
	}

	[NGCRPC]
	protected void SAAM(int footArmorUID, int legArmorUID, int torsoArmorUID, int headArmorUID)
	{
		if (footArmorUID != 0)
		{
			this.footArmor = (ArmorDataBlock)DatablockDictionary.GetByUniqueID(footArmorUID);
		}
		else
		{
			this.footArmor = null;
		}
		if (legArmorUID != 0)
		{
			this.legArmor = (ArmorDataBlock)DatablockDictionary.GetByUniqueID(legArmorUID);
		}
		else
		{
			this.legArmor = null;
		}
		if (torsoArmorUID != 0)
		{
			this.torsoArmor = (ArmorDataBlock)DatablockDictionary.GetByUniqueID(torsoArmorUID);
		}
		else
		{
			this.torsoArmor = null;
		}
		if (headArmorUID != 0)
		{
			this.headArmor = (ArmorDataBlock)DatablockDictionary.GetByUniqueID(headArmorUID);
		}
		else
		{
			this.headArmor = null;
		}
		this.RebindRenderers();
	}

	[NGCRPC]
	protected void SACH(NetEntityID makingForCharacterIDNow, uLink.NetworkMessageInfo info)
	{
		AudioSource audioSource = base.audio;
		if (audioSource)
		{
			audioSource.Play();
		}
	}

	[NGCRPC]
	protected void SAKL(uLink.NetworkMessageInfo info)
	{
		if (this.CreateRagdoll())
		{
			if (this.footMeshFilter)
			{
				this.footMeshFilter.renderer.enabled = false;
			}
			if (this.legMeshFilter)
			{
				this.legMeshFilter.renderer.enabled = false;
			}
			if (this.torsoMeshFilter)
			{
				this.torsoMeshFilter.renderer.enabled = false;
			}
			if (this.headMeshFilter)
			{
				this.headMeshFilter.renderer.enabled = false;
			}
		}
	}
}