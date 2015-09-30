using System;
using System.Collections.Generic;
using UnityEngine;

public class TreeColliderAdd : MonoBehaviour
{
	public Terrain terrain;

	private TerrainData terrainData;

	public Vector3 lastPos;

	public GameObject treeColliderPrefab;

	private int pooledColliders = 500;

	private List<GameObject> treeColliderPool;

	private List<GameObject> usedCollidersPool;

	private Vector3[] convertedTreePositions;

	public TreeColliderAdd()
	{
	}

	private void AddNewColliders()
	{
		Vector3 vector3 = new Vector3();
		this.CleanupOldColliders();
		Vector3 vector31 = base.transform.position;
		int num = 0;
		int num1 = 0;
		int count = this.treeColliderPool.Count;
		int num2 = 0;
		int length = (int)this.convertedTreePositions.Length;
		while (num2 < length)
		{
			vector3.x = this.convertedTreePositions[num2].x - vector31.x;
			vector3.y = this.convertedTreePositions[num2].y - vector31.y;
			vector3.z = this.convertedTreePositions[num2].z - vector31.z;
			if (vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z <= 40000f)
			{
				GameObject freeTreeCollider = this.GetFreeTreeCollider();
				if (!freeTreeCollider)
				{
					return;
				}
				Vector3 vector32 = this.convertedTreePositions[num2];
				freeTreeCollider.transform.position = vector32;
				this.usedCollidersPool.Add(freeTreeCollider);
				this.convertedTreePositions[num2] = this.convertedTreePositions[num1];
				int num3 = num1;
				num1 = num3 + 1;
				this.convertedTreePositions[num3] = vector32;
				int num4 = count - 1;
				count = num4;
				if (num4 == 0)
				{
					break;
				}
			}
			num++;
			num2++;
		}
	}

	private void CleanupOldColliders()
	{
		foreach (GameObject gameObject in this.usedCollidersPool)
		{
			this.treeColliderPool.Add(gameObject);
		}
		this.usedCollidersPool.Clear();
	}

	public GameObject GetFreeTreeCollider()
	{
		if (this.treeColliderPool.Count <= 0)
		{
			return null;
		}
		GameObject item = this.treeColliderPool[0];
		this.treeColliderPool.RemoveAt(0);
		return item;
	}

	private void Start()
	{
		this.terrainData = this.terrain.terrainData;
		this.lastPos = base.transform.position;
		this.treeColliderPool = new List<GameObject>();
		this.usedCollidersPool = new List<GameObject>();
		this.convertedTreePositions = new Vector3[(int)this.terrainData.treeInstances.Length];
		int num = 0;
		TreeInstance[] treeInstanceArray = this.terrainData.treeInstances;
		for (int i = 0; i < (int)treeInstanceArray.Length; i++)
		{
			TreeInstance treeInstance = treeInstanceArray[i];
			this.convertedTreePositions[num] = Vector3.Scale(treeInstance.position, this.terrainData.size) + this.terrain.transform.position;
			num++;
		}
		Debug.Log(string.Concat("Tree instances length:", (int)this.terrainData.treeInstances.Length));
		for (int j = 0; j < this.pooledColliders; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.treeColliderPrefab, new Vector3(0f, -20000f, 0f), Quaternion.identity) as GameObject;
			this.treeColliderPool.Add(gameObject);
		}
	}

	private void Update()
	{
		Vector3 vector3 = base.transform.position;
		if (Vector3.Distance(vector3, this.lastPos) >= 100f)
		{
			this.AddNewColliders();
			this.lastPos = vector3;
		}
	}
}