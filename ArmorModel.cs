using Facepunch.Actor;
using System;
using UnityEngine;

public abstract class ArmorModel : ScriptableObject
{
	[NonSerialized]
	public readonly ArmorModelSlot slot;

	[SerializeField]
	private ActorMeshInfo _actorMeshInfo;

	[SerializeField]
	private Material[] _materials;

	protected abstract ArmorModel _censored
	{
		get;
	}

	public ActorMeshInfo actorMeshInfo
	{
		get
		{
			return this._actorMeshInfo;
		}
	}

	public ActorRig actorRig
	{
		get
		{
			ActorRig actorRig;
			if (!this._actorMeshInfo)
			{
				actorRig = null;
			}
			else
			{
				actorRig = this._actorMeshInfo.actorRig;
			}
			return actorRig;
		}
	}

	public ArmorModel censoredModel
	{
		get
		{
			return this._censored;
		}
	}

	public bool hasCensoredModel
	{
		get
		{
			return this._censored;
		}
	}

	public Material[] sharedMaterials
	{
		get
		{
			return this._materials;
		}
	}

	public Mesh sharedMesh
	{
		get
		{
			Mesh mesh;
			if (!this._actorMeshInfo)
			{
				mesh = null;
			}
			else
			{
				mesh = this._actorMeshInfo.sharedMesh;
			}
			return mesh;
		}
	}

	public ArmorModelSlotMask slotMask
	{
		get
		{
			return this.slot.ToMask();
		}
	}

	internal ArmorModel(ArmorModelSlot slot)
	{
		this.slot = slot;
	}
}