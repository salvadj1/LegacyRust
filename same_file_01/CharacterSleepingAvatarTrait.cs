using System;
using UnityEngine;

public class CharacterSleepingAvatarTrait : CharacterTrait
{
	[SerializeField]
	private string _sleepingAvatarPrefab;

	[SerializeField]
	private bool _allowDroppingOfInventory;

	[SerializeField]
	private bool _grabCarrierOnCreate;

	[SerializeField]
	private Vector3 boxCenter;

	[SerializeField]
	private Vector3 boxSize;

	[NonSerialized]
	private bool? _prefabValid;

	[NonSerialized]
	private bool _hasInventory;

	[NonSerialized]
	private bool _hasTakeDamage;

	[NonSerialized]
	private Type _takeDamageType;

	public bool canDropInventories
	{
		get
		{
			return (!this._allowDroppingOfInventory ? false : this.hasInventory);
		}
	}

	public bool grabsCarrierOnCreate
	{
		get
		{
			return (!this.valid ? false : this._grabCarrierOnCreate);
		}
	}

	public bool hasInventory
	{
		get
		{
			return (!this.valid ? false : this._hasInventory);
		}
	}

	public bool hasTakeDamage
	{
		get
		{
			return (!this.valid ? false : this._hasTakeDamage);
		}
	}

	public string prefab
	{
		get
		{
			return this._sleepingAvatarPrefab ?? string.Empty;
		}
	}

	public Type takeDamageType
	{
		get
		{
			if (!this.hasTakeDamage)
			{
				throw new InvalidOperationException("You need to check hasTakeDamage before requesting this. hasTakeDamage == False");
			}
			return this._takeDamageType;
		}
	}

	public bool valid
	{
		get
		{
			bool value;
			bool? nullable = this._prefabValid;
			if (!nullable.HasValue)
			{
				bool? nullable1 = new bool?(this.ValidatePrefab());
				bool? nullable2 = nullable1;
				this._prefabValid = nullable1;
				value = nullable2.Value;
			}
			else
			{
				value = nullable.Value;
			}
			return value;
		}
	}

	public CharacterSleepingAvatarTrait()
	{
	}

	public Vector3 SolvePlacement(Vector3 origin, Quaternion rot, int iter)
	{
		return TransformHelpers.TestBoxCorners(origin, rot, this.boxCenter, this.boxSize, 1024, iter);
	}

	private bool ValidatePrefab()
	{
		GameObject gameObject;
		Type type;
		if (string.IsNullOrEmpty(this._sleepingAvatarPrefab))
		{
			return false;
		}
		NetCull.PrefabSearch prefabSearch = NetCull.LoadPrefab(this._sleepingAvatarPrefab, out gameObject);
		if ((int)prefabSearch != 1)
		{
			Debug.LogError(string.Format("sleeping avatar prefab named \"{0}\" resulted in {1} which was not {2}(required)", this.prefab, prefabSearch, NetCull.PrefabSearch.NGC));
			return false;
		}
		IDMain component = gameObject.GetComponent<IDMain>();
		if (!(component is SleepingAvatar))
		{
			Debug.LogError(string.Format("Theres no Sleeping avatar on prefab \"{0}\"", this.prefab), gameObject);
			return false;
		}
		this._hasInventory = component.GetLocal<Inventory>();
		TakeDamage local = component.GetLocal<TakeDamage>();
		this._hasTakeDamage = local;
		if (!this._hasTakeDamage)
		{
			type = null;
		}
		else
		{
			type = local.GetType();
		}
		this._takeDamageType = type;
		return true;
	}
}