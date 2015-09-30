using System;
using UnityEngine;

public class SpawnInArea : MonoBehaviour
{
	public Texture2D SpawnMap;

	private float Offset = 10f;

	private float AboveGround = 1f;

	private bool TerrainOnly = true;

	public SpawnInArea()
	{
	}

	private void RandomPositionOnTerrain(GameObject obj)
	{
		RaycastHit raycastHit;
		Vector3 vector3 = Terrain.activeTerrain.terrainData.size;
		Vector3 offset = new Vector3();
		bool flag = false;
		while (!flag)
		{
			offset = Terrain.activeTerrain.transform.position;
			float single = UnityEngine.Random.Range(0f, vector3.x);
			float single1 = UnityEngine.Random.Range(0f, vector3.z);
			offset.x = offset.x + single;
			offset.y = offset.y + (vector3.y + this.Offset);
			offset.z = offset.z + single1;
			if (!this.SpawnMap)
			{
				flag = true;
			}
			else
			{
				int num = Mathf.RoundToInt((float)this.SpawnMap.width * single / vector3.x);
				int num1 = Mathf.RoundToInt((float)this.SpawnMap.height * single1 / vector3.z);
				float pixel = this.SpawnMap.GetPixel(num, num1).grayscale;
				flag = (pixel <= 0f || UnityEngine.Random.Range(0f, 1f) >= pixel ? false : true);
			}
			if (!flag)
			{
				continue;
			}
			if (!Physics.Raycast(offset, -Vector3.up, out raycastHit))
			{
				flag = false;
			}
			else
			{
				float single2 = raycastHit.distance;
				if (raycastHit.transform.name != "Terrain" && this.TerrainOnly)
				{
					flag = false;
				}
				offset.y = offset.y - (single2 - this.AboveGround);
			}
		}
		obj.transform.position = offset;
		base.transform.Rotate(Vector3.up * (float)UnityEngine.Random.Range(0, 360), Space.World);
	}
}