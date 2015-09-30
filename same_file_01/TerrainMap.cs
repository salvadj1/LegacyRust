using System;
using System.Reflection;
using UnityEngine;

public class TerrainMap : ScriptableObject
{
	[SerializeField]
	private string[] _guids;

	[SerializeField]
	private int _width;

	[SerializeField]
	private int _height;

	public float baseHeight;

	public Vector3 scale;

	public Terrain copyFrom;

	public TerrainData root;

	public int count
	{
		get
		{
			return this._width * this._height;
		}
	}

	public int height
	{
		get
		{
			return this._height;
		}
	}

	public string this[int i]
	{
		get
		{
			return this._guids[i];
		}
		set
		{
			this._guids[i] = value;
		}
	}

	public string this[int x, int y]
	{
		get
		{
			return this[y * this._width + x];
		}
		set
		{
			this[y * this._width + x] = value;
		}
	}

	public int width
	{
		get
		{
			return this._width;
		}
	}

	public TerrainMap()
	{
	}

	public void ResizeGUIDS(int width, int height)
	{
		int num = this._width;
		int num1 = this._height;
		if (num != width || num1 != height)
		{
			string[] strArrays = this._guids;
			this._guids = new string[width * height];
			this._width = width;
			this._height = height;
			int num2 = Mathf.Min(num, width);
			int num3 = Mathf.Min(num1, height);
			for (int i = 0; i < num3; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					this._guids[i * this._width + j] = strArrays[i * num + j];
				}
			}
		}
	}
}