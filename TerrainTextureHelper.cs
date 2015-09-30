using System;
using UnityEngine;

public class TerrainTextureHelper
{
	public static byte[,] textures;

	public static Terrain cachedTerrain;

	static TerrainTextureHelper()
	{
	}

	public TerrainTextureHelper()
	{
	}

	public static void CacheTextures()
	{
		Debug.Log("Caching Terrain splatmap lookups, please wait...");
		Terrain terrain = Terrain.activeTerrain;
		TerrainData terrainDatum = terrain.terrainData;
		Vector3 vector3 = terrain.transform.position;
		float[,,] alphamaps = terrainDatum.GetAlphamaps(0, 0, terrainDatum.alphamapWidth, terrainDatum.alphamapHeight);
		TerrainTextureHelper.textures = new byte[alphamaps.GetUpperBound(0) + 1, alphamaps.GetUpperBound(1) + 1];
		for (int i = 0; i < terrainDatum.alphamapWidth; i++)
		{
			for (int j = 0; j < terrainDatum.alphamapHeight; j++)
			{
				float single = 0f;
				int num = 0;
				for (int k = 0; k < alphamaps.GetUpperBound(2) + 1; k++)
				{
					if (alphamaps[i, j, k] >= single)
					{
						num = k;
						single = alphamaps[i, j, k];
					}
				}
				TerrainTextureHelper.textures[i, j] = (byte)num;
			}
		}
		GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
	}

	public static void EnsureInit()
	{
		if (TerrainTextureHelper.cachedTerrain == Terrain.activeTerrain)
		{
			return;
		}
		TerrainTextureHelper.CacheTextures();
		TerrainTextureHelper.cachedTerrain = Terrain.activeTerrain;
	}

	public static float[] GetTextureAmounts(Vector3 worldPos)
	{
		return TerrainTextureHelper.OLD_GetTextureMix(worldPos);
	}

	public static int GetTextureIndex(Vector3 worldPos)
	{
		return TerrainTextureHelper.OLD_GetMainTexture(worldPos);
	}

	public static int OLD_GetMainTexture(Vector3 worldPos)
	{
		float[] singleArray = TerrainTextureHelper.OLD_GetTextureMix(worldPos);
		float single = 0f;
		int num = 0;
		for (int i = 0; i < (int)singleArray.Length; i++)
		{
			if (singleArray[i] > single)
			{
				num = i;
				single = singleArray[i];
			}
		}
		return num;
	}

	public static float[] OLD_GetTextureMix(Vector3 worldPos)
	{
		Terrain terrain = Terrain.activeTerrain;
		TerrainData terrainDatum = terrain.terrainData;
		Vector3 vector3 = terrain.transform.position;
		float single = worldPos.x - vector3.x;
		Vector3 vector31 = terrainDatum.size;
		int num = (int)(single / vector31.x * (float)terrainDatum.alphamapWidth);
		float single1 = worldPos.z - vector3.z;
		Vector3 vector32 = terrainDatum.size;
		int num1 = (int)(single1 / vector32.z * (float)terrainDatum.alphamapHeight);
		float[,,] alphamaps = terrainDatum.GetAlphamaps(num, num1, 1, 1);
		float[] singleArray = new float[alphamaps.GetUpperBound(2) + 1];
		for (int i = 0; i < (int)singleArray.Length; i++)
		{
			singleArray[i] = alphamaps[0, 0, i];
		}
		return singleArray;
	}
}