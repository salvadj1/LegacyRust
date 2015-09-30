using System;
using UnityEngine;

public class MegaTerrain : MonoBehaviour
{
	public TerrainData _rootTerrainData;

	public Terrain[] _terrains;

	public string name_base = "rust_terrain";

	public MegaTerrain()
	{
	}

	public Terrain FindTerrain(int x, int y)
	{
		Terrain component;
		string str = string.Concat(new object[] { this.name_base, "_x", x, "_y", y });
		if (GameObject.Find(str) == null)
		{
			component = null;
		}
		else
		{
			component = GameObject.Find(str).GetComponent<Terrain>();
		}
		return component;
	}

	[ContextMenu("Generate")]
	private void Generate()
	{
		for (int i = 0; i < 16; i++)
		{
			int num = 0;
			while (num < 16)
			{
				num++;
			}
		}
	}

	private void Start()
	{
	}

	[ContextMenu("Stitch")]
	private void Stitch()
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				Terrain terrain = this.FindTerrain(i, j);
				if (!terrain)
				{
					Debug.Log(string.Concat(new object[] { "couldnt find terrain :", this.name_base, "_x", i, "_y", j }));
				}
				else
				{
					Debug.Log("found terrain");
					Terrain terrain1 = this.FindTerrain(i - 1, j);
					Terrain terrain2 = this.FindTerrain(i + 1, j);
					Terrain terrain3 = this.FindTerrain(i, j + 1);
					Terrain terrain4 = this.FindTerrain(i, j - 1);
					terrain.SetNeighbors(terrain1, terrain3, terrain2, terrain4);
					!terrain1;
				}
			}
		}
	}

	private void Update()
	{
	}
}