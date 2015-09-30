using System;
using UnityEngine;

public class LootableObjectSpawner : MonoBehaviour
{
	public LootableObjectSpawner.ChancePick[] _lootableChances;

	public bool spawnOnStart = true;

	public float spawnTimeMin = 5f;

	public float spawnTimeMax = 10f;

	public LootableObjectSpawner()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(base.transform.position, base.transform.position + (base.transform.forward * 1f));
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(base.transform.position, base.transform.position + (base.transform.forward * 1f));
	}

	[Serializable]
	public class ChancePick
	{
		public LootableObject obj;

		public float weight;

		public ChancePick()
		{
		}
	}
}