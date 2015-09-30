using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpawner : MonoBehaviour
{
	private static float spawnStagger;

	public float radius = 40f;

	public float thinkDelay = 60f;

	public bool initialSpawn;

	[SerializeField]
	public List<GenericSpawnerSpawnList.GenericSpawnInstance> _spawnList;

	public GenericSpawnerSpawnList spawnListObjectOverride;

	static GenericSpawner()
	{
	}

	public GenericSpawner()
	{
	}

	public void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = new Color(0.3f, 0.3f, 1f, 0.5f);
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(base.transform.position, Vector3.one * 0.5f);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
	}
}