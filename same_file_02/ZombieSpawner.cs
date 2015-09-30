using System;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
	public string[] zombiePrefabs;

	public int targetPopulation = 10;

	public float radius = 40f;

	public float thinkDelay = 60f;

	[NonSerialized]
	private int exaustCount;

	private ZombieSpawner()
	{
		this.zombiePrefabs = new string[] { "npc_zombie" };
	}

	private void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(base.transform.position, Vector3.one * 0.5f);
	}
}