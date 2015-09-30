using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpawnerSpawnList : ScriptableObject
{
	[SerializeField]
	public List<GenericSpawnerSpawnList.GenericSpawnInstance> _spawnList;

	public GenericSpawnerSpawnList()
	{
	}

	public List<GenericSpawnerSpawnList.GenericSpawnInstance> GetCopy()
	{
		List<GenericSpawnerSpawnList.GenericSpawnInstance> genericSpawnInstances = new List<GenericSpawnerSpawnList.GenericSpawnInstance>(this._spawnList.Count);
		foreach (GenericSpawnerSpawnList.GenericSpawnInstance genericSpawnInstance in this._spawnList)
		{
			genericSpawnInstances.Add(genericSpawnInstance.Clone());
		}
		return genericSpawnInstances;
	}

	[Serializable]
	public class GenericSpawnInstance
	{
		public string prefabName;

		public int targetPopulation;

		public int numToSpawnPerTick;

		public bool forceStaticInstantiate;

		public bool useNavmeshSample;

		public List<GameObject> spawned;

		public GenericSpawnInstance()
		{
		}

		public GenericSpawnerSpawnList.GenericSpawnInstance Clone()
		{
			GenericSpawnerSpawnList.GenericSpawnInstance genericSpawnInstance = new GenericSpawnerSpawnList.GenericSpawnInstance()
			{
				prefabName = this.prefabName,
				targetPopulation = this.targetPopulation,
				numToSpawnPerTick = this.numToSpawnPerTick,
				forceStaticInstantiate = this.forceStaticInstantiate,
				spawned = new List<GameObject>()
			};
			return genericSpawnInstance;
		}

		public int GetNumActive()
		{
			return this.spawned.Count;
		}
	}
}