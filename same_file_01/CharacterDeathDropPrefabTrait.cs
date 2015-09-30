using System;
using UnityEngine;

public class CharacterDeathDropPrefabTrait : CharacterTrait
{
	[SerializeField]
	private string _prefabName;

	[NonSerialized]
	private GameObject _loadedPrefab;

	[NonSerialized]
	private bool _loaded;

	[NonSerialized]
	private bool _loadFail;

	public bool hasPrefab
	{
		get
		{
			bool flag;
			if (!this._loaded)
			{
				flag = this.prefab;
			}
			else
			{
				flag = !this._loadFail;
			}
			return flag;
		}
	}

	public string instantiateString
	{
		get
		{
			if (!this.prefab)
			{
				return null;
			}
			return this._prefabName;
		}
	}

	private GameObject prefab
	{
		get
		{
			if (!this._loaded)
			{
				this._loaded = true;
				this._loadFail = (int)NetCull.LoadPrefab(this._prefabName, out this._loadedPrefab) == 0;
			}
			return this._loadedPrefab;
		}
	}

	public Transform prefabTransform
	{
		get
		{
			Transform transforms;
			GameObject gameObject = this.prefab;
			if (!gameObject)
			{
				transforms = null;
			}
			else
			{
				transforms = gameObject.transform;
			}
			return transforms;
		}
	}

	public CharacterDeathDropPrefabTrait()
	{
	}
}