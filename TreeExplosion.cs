using System;
using System.Collections;
using UnityEngine;

public class TreeExplosion : MonoBehaviour
{
	public float BlastRange = 30f;

	public float BlastForce = 30000f;

	public GameObject DeadReplace;

	public GameObject Explosion;

	public TreeExplosion()
	{
	}

	private void Explode()
	{
		UnityEngine.Object.Instantiate(this.Explosion, base.transform.position, Quaternion.identity);
		TerrainData array = Terrain.activeTerrain.terrainData;
		ArrayList arrayLists = new ArrayList();
		TreeInstance[] treeInstanceArray = array.treeInstances;
		for (int i = 0; i < (int)treeInstanceArray.Length; i++)
		{
			TreeInstance treeInstance = treeInstanceArray[i];
			if (Vector3.Distance(Vector3.Scale(treeInstance.position, array.size) + Terrain.activeTerrain.transform.position, base.transform.position) >= this.BlastRange)
			{
				arrayLists.Add(treeInstance);
			}
			else
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(this.DeadReplace, Vector3.Scale(treeInstance.position, array.size) + Terrain.activeTerrain.transform.position, Quaternion.identity) as GameObject;
				gameObject.rigidbody.maxAngularVelocity = 1f;
				gameObject.rigidbody.AddExplosionForce(this.BlastForce, base.transform.position, 20f + this.BlastRange * 5f, -20f);
			}
		}
		array.treeInstances = (TreeInstance[])arrayLists.ToArray(typeof(TreeInstance));
	}

	private void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			this.Explode();
		}
	}
}