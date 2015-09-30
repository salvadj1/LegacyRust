using Facepunch.Actor;
using System;
using System.Reflection;
using UnityEngine;

public sealed class ArmorModelRenderer : IDLocalCharacter
{
	[PrefetchComponent]
	[SerializeField]
	private BoneStructure boneStructure;

	[PrefetchChildComponent]
	[SerializeField]
	private SkinnedMeshRenderer originalRenderer;

	[NonSerialized]
	private ArmorModelMemberMap<ActorMeshRenderer> renderers;

	[NonSerialized]
	private ArmorModelMemberMap models;

	[NonSerialized]
	private bool awake;

	[NonSerialized]
	private CharacterArmorTrait armorTrait;

	private static bool censored;

	private static bool rebindingCensorship;

	public ActorRig actorRig
	{
		get
		{
			return this.boneStructure.actorRig;
		}
	}

	public static bool Censored
	{
		get
		{
			return ArmorModelRenderer.censored;
		}
		set
		{
			if (ArmorModelRenderer.censored != value)
			{
				ArmorModelRenderer.censored = value;
				try
				{
					ArmorModelRenderer.rebindingCensorship = true;
					UnityEngine.Object[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(ArmorModelRenderer));
					for (int i = 0; i < (int)objArray.Length; i++)
					{
						ArmorModelRenderer armorModelRenderer = (ArmorModelRenderer)objArray[i];
						if (armorModelRenderer)
						{
							for (ArmorModelSlot j = ArmorModelSlot.Feet; (int)j < 4; j = (ArmorModelSlot)((byte)j + (byte)ArmorModelSlot.Legs))
							{
								ArmorModel item = armorModelRenderer[j];
								if (item && item.hasCensoredModel)
								{
									if (armorModelRenderer.awake)
									{
										armorModelRenderer.BindArmorModelCheckedNonNull(item);
									}
									else
									{
										armorModelRenderer.Initialize(new ArmorModelMemberMap(), 0);
										break;
									}
								}
							}
						}
					}
					SleepingAvatar.RebindAllRenderers();
				}
				finally
				{
					ArmorModelRenderer.rebindingCensorship = false;
				}
			}
		}
	}

	public ArmorModelGroup defaultArmorModelGroup
	{
		get
		{
			CharacterArmorTrait characterArmorTrait;
			if (!this.armorTrait)
			{
				CharacterArmorTrait trait = base.GetTrait<CharacterArmorTrait>();
				CharacterArmorTrait characterArmorTrait1 = trait;
				this.armorTrait = trait;
				characterArmorTrait = characterArmorTrait1;
			}
			else
			{
				characterArmorTrait = this.armorTrait;
			}
			return characterArmorTrait.defaultGroup;
		}
	}

	public ArmorModel this[ArmorModelSlot slot]
	{
		get
		{
			if (this.awake)
			{
				return this.models[slot];
			}
			ArmorModelGroup armorModelGroups = this.defaultArmorModelGroup;
			if (!armorModelGroups)
			{
				return null;
			}
			return armorModelGroups[slot];
		}
	}

	static ArmorModelRenderer()
	{
		ArmorModelRenderer.censored = true;
	}

	public ArmorModelRenderer()
	{
	}

	public ArmorModelSlotMask BindArmorGroup(ArmorModelGroup group, ArmorModelSlotMask slotMask)
	{
		if (!this.awake)
		{
			if (!group)
			{
				return 0;
			}
			return this.Initialize(group.armorModelMemberMap, slotMask);
		}
		ArmorModelSlotMask mask = (ArmorModelSlotMask)0;
		ArmorModelSlot[] armorModelSlotArray = slotMask.EnumerateSlots();
		for (int i = 0; i < (int)armorModelSlotArray.Length; i++)
		{
			ArmorModelSlot armorModelSlot = armorModelSlotArray[i];
			ArmorModel item = group[armorModelSlot];
			if (item && this.BindArmorModelCheckedNonNull(item))
			{
				mask = mask | armorModelSlot.ToMask();
			}
		}
		return mask;
	}

	public ArmorModelSlotMask BindArmorGroup(ArmorModelGroup group)
	{
		ArmorModelSlotMask mask = (ArmorModelSlotMask)0;
		if (group)
		{
			for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
			{
				ArmorModel item = group[i];
				if (item && this.BindArmorModelCheckedNonNull(item))
				{
					mask = mask | i.ToMask();
				}
			}
		}
		return mask;
	}

	private bool BindArmorModel<TArmorModel>(TArmorModel model)
	where TArmorModel : ArmorModel, new()
	{
		if (model)
		{
			return this.BindArmorModelCheckedNonNull(model);
		}
		ArmorModel item = this.defaultArmorModelGroup[ArmorModelSlotUtility.GetArmorModelSlotForClass<TArmorModel>()];
		if (!item)
		{
			return false;
		}
		return this.BindArmorModelCheckedNonNull(item);
	}

	private bool BindArmorModel(ArmorModel model, ArmorModelSlot slot)
	{
		if (model)
		{
			if (model.slot == slot)
			{
				return this.BindArmorModelCheckedNonNull(model);
			}
			Debug.LogError(string.Concat("model.slot != ", slot), model);
			return false;
		}
		ArmorModel item = this.defaultArmorModelGroup[slot];
		return (!item ? false : this.BindArmorModelCheckedNonNull(item));
	}

	private bool BindArmorModelCheckedNonNull(ArmorModel model)
	{
		ArmorModel armorModel;
		ArmorModelSlot armorModelSlot = model.slot;
		if (!ArmorModelRenderer.rebindingCensorship && this.models[armorModelSlot] == model)
		{
			return false;
		}
		ActorMeshRenderer item = this.renderers[armorModelSlot];
		if (!ArmorModelRenderer.censored)
		{
			armorModel = model;
		}
		else
		{
			armorModel = model.censoredModel;
			if (!armorModel)
			{
				armorModel = model;
			}
		}
		if (item.actorRig != armorModel.actorRig)
		{
			return false;
		}
		if (!base.enabled)
		{
			item.renderer.enabled = true;
		}
		item.Bind(armorModel.actorMeshInfo, armorModel.sharedMaterials);
		if (!base.enabled)
		{
			item.renderer.enabled = false;
		}
		this.models[armorModelSlot] = model;
		return true;
	}

	public ArmorModelSlotMask BindArmorModels(ArmorModelMemberMap map)
	{
		if (!this.awake)
		{
			return this.Initialize(map, ArmorModelSlotMask.Feet | ArmorModelSlotMask.Legs | ArmorModelSlotMask.Torso | ArmorModelSlotMask.Head);
		}
		ArmorModelSlotMask mask = (ArmorModelSlotMask)0;
		for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			if (this.BindArmorModel(map[i], i))
			{
				mask = mask | i.ToMask();
			}
		}
		return mask;
	}

	public ArmorModelSlotMask BindArmorModels(ArmorModelMemberMap map, ArmorModelSlotMask slotMask)
	{
		if (!this.awake)
		{
			return this.Initialize(map, slotMask);
		}
		ArmorModelSlotMask mask = (ArmorModelSlotMask)0;
		ArmorModelSlot[] armorModelSlotArray = slotMask.EnumerateSlots();
		for (int i = 0; i < (int)armorModelSlotArray.Length; i++)
		{
			ArmorModelSlot armorModelSlot = armorModelSlotArray[i];
			if (this.BindArmorModel(map[armorModelSlot], armorModelSlot))
			{
				mask = mask | armorModelSlot.ToMask();
			}
		}
		return mask;
	}

	public ArmorModelSlotMask BindDefaultArmorGroup()
	{
		if (!this.defaultArmorModelGroup)
		{
			return 0;
		}
		return this.BindArmorGroup(this.defaultArmorModelGroup);
	}

	public bool Contains(ArmorModel model)
	{
		if (!model)
		{
			return false;
		}
		if (this.awake)
		{
			return this.models[model.slot] == model;
		}
		ArmorModelGroup armorModelGroups = this.defaultArmorModelGroup;
		return (!armorModelGroups ? false : armorModelGroups[model.slot] == model);
	}

	public bool Contains<TArmorModel>(TArmorModel model)
	where TArmorModel : ArmorModel, new()
	{
		if (!model)
		{
			return false;
		}
		if (this.awake)
		{
			return this.models.GetArmorModel<TArmorModel>() == model;
		}
		ArmorModelGroup armorModelGroups = this.defaultArmorModelGroup;
		return (!armorModelGroups ? false : armorModelGroups.GetArmorModel<TArmorModel>() == model);
	}

	public T GetArmorModel<T>()
	where T : ArmorModel, new()
	{
		if (this.awake)
		{
			return this.models.GetArmorModel<T>();
		}
		ArmorModelGroup armorModelGroups = this.defaultArmorModelGroup;
		if (armorModelGroups)
		{
			return armorModelGroups.GetArmorModel<T>();
		}
		return (T)null;
	}

	public ArmorModelMemberMap GetArmorModelMemberMapCopy()
	{
		if (this.awake)
		{
			return this.models;
		}
		ArmorModelGroup armorModelGroups = this.defaultArmorModelGroup;
		if (armorModelGroups)
		{
			return armorModelGroups.armorModelMemberMap;
		}
		return new ArmorModelMemberMap();
	}

	private ArmorModelSlotMask Initialize(ArmorModelMemberMap memberMap, ArmorModelSlotMask memberMask)
	{
		ActorMeshRenderer actorMeshRenderer;
		this.awake = true;
		string rendererName = ArmorModelSlot.Head.GetRendererName();
		ActorRig item = this.defaultArmorModelGroup[ArmorModelSlot.Head].actorRig;
		actorMeshRenderer = (!this.originalRenderer ? ActorMeshRenderer.CreateOn(base.transform, rendererName, item, this.boneStructure.rigOrderedTransformArray, base.gameObject.layer) : ActorMeshRenderer.Replace(this.originalRenderer, item, this.boneStructure.rigOrderedTransformArray, rendererName));
		this.renderers[ArmorModelSlot.Head] = actorMeshRenderer;
		for (ArmorModelSlot i = ArmorModelSlot.Feet; i < ArmorModelSlot.Head; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
		{
			this.renderers[i] = actorMeshRenderer.CloneBlank(i.GetRendererName());
		}
		for (ArmorModelSlot j = ArmorModelSlot.Feet; j < ArmorModelSlot.Head; j = (ArmorModelSlot)((byte)j + (byte)ArmorModelSlot.Legs))
		{
			ActorMeshRenderer item1 = this.renderers[j];
			if (item1)
			{
				item1.renderer.enabled = base.enabled;
			}
		}
		ArmorModelSlotMask mask = (ArmorModelSlotMask)0;
		ArmorModelGroup armorModelGroups = this.defaultArmorModelGroup;
		if (!armorModelGroups)
		{
			ArmorModelSlot[] armorModelSlotArray = memberMask.EnumerateSlots();
			for (int k = 0; k < (int)armorModelSlotArray.Length; k++)
			{
				ArmorModelSlot armorModelSlot = armorModelSlotArray[k];
				ArmorModel armorModel = memberMap.GetArmorModel(armorModelSlot);
				if (armorModel && this.BindArmorModelCheckedNonNull(armorModel))
				{
					mask = mask | armorModelSlot.ToMask();
				}
			}
		}
		else
		{
			for (ArmorModelSlot l = ArmorModelSlot.Feet; (int)l < 4; l = (ArmorModelSlot)((byte)l + (byte)ArmorModelSlot.Legs))
			{
				if (memberMask.Contains(l))
				{
					ArmorModel armorModel1 = memberMap.GetArmorModel(l);
					if (!armorModel1 || !this.BindArmorModelCheckedNonNull(armorModel1))
					{
						goto Label1;
					}
					mask = mask | l.ToMask();
					goto Label0;
				}
			Label1:
				ArmorModel item2 = armorModelGroups[l];
				if (item2)
				{
					this.BindArmorModelCheckedNonNull(item2);
				}
			Label0:
			}
		}
		return mask;
	}

	private void OnDestroy()
	{
		if (this.awake)
		{
			for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
			{
				ActorMeshRenderer item = this.renderers[i];
				if (item)
				{
					UnityEngine.Object.Destroy(item.gameObject);
				}
			}
		}
		else
		{
			this.awake = true;
		}
	}

	private void OnDisable()
	{
		if (this.awake)
		{
			for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
			{
				ActorMeshRenderer item = this.renderers[i];
				if (item)
				{
					item.renderer.enabled = false;
				}
			}
		}
		else if (this.originalRenderer)
		{
			this.originalRenderer.enabled = false;
		}
	}

	private void OnEnable()
	{
		if (this.awake)
		{
			for (ArmorModelSlot i = ArmorModelSlot.Feet; (int)i < 4; i = (ArmorModelSlot)((byte)i + (byte)ArmorModelSlot.Legs))
			{
				ActorMeshRenderer item = this.renderers[i];
				if (item)
				{
					item.renderer.enabled = true;
				}
			}
		}
		else if (this.originalRenderer)
		{
			this.originalRenderer.enabled = true;
		}
	}

	private void Start()
	{
		if (!this.awake)
		{
			this.Initialize(new ArmorModelMemberMap(), 0);
		}
	}
}