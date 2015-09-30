using Facepunch;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class FPGrassProbabilityGenerator : ScriptableObject, IFPGrassAsset
{
	[NonSerialized]
	public RenderTexture probabilityTexture;

	[SerializeField]
	private Material material;

	[SerializeField]
	public float gridScale;

	[SerializeField]
	public int gridSize;

	public new string name
	{
		get
		{
			return base.name;
		}
		set
		{
			base.name = value;
			this.material.name = string.Concat(value, "(", this.material.name.Replace("(Clone)", string.Empty), ")");
		}
	}

	public FPGrassProbabilityGenerator()
	{
	}

	private void CreateRenderTexture()
	{
		this.DestroyProbabilityTexture();
		int num = Mathf.NextPowerOfTwo(this.gridSize);
		RenderTexture renderTexture = new RenderTexture(num, num, 0, FPGrass.Support.ProbabilityRenderTextureFormat4Channel)
		{
			hideFlags = HideFlags.DontSave
		};
		this.probabilityTexture = renderTexture;
		this.probabilityTexture.filterMode = FilterMode.Point;
		this.probabilityTexture.useMipMap = false;
		this.probabilityTexture.anisoLevel = 0;
		this.probabilityTexture.Create();
	}

	public void DestroyObjects()
	{
		this.DestroyProbabilityTexture();
	}

	private void DestroyProbabilityTexture()
	{
		if (this.probabilityTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.probabilityTexture, true);
			this.probabilityTexture = null;
		}
	}

	public void Initialize()
	{
		if (!this.probabilityTexture && this.gridSize > 0)
		{
			this.CreateRenderTexture();
		}
		if (!this.material)
		{
			this.material = (Material)UnityEngine.Object.Instantiate(Bundling.Load("rust/fpgrass/RenderSplatMaterial", typeof(Material)));
			this.material.SetTexture("_Noise", (Texture2D)Bundling.Load("rust/fpgrass/noise", typeof(Texture2D)));
		}
	}

	private void OnDestroy()
	{
		this.DestroyProbabilityTexture();
	}

	private void OnDisable()
	{
		this.DestroyProbabilityTexture();
	}

	private void OnEnable()
	{
		this.Initialize();
	}

	public void SetDetailProbabilities(Texture2D texture)
	{
		this.material.SetTexture("_DetailProbabilities", texture);
	}

	public void SetGridScale(float newScale)
	{
		this.gridScale = newScale;
		this.material.SetFloat("_GridScale", this.gridScale);
	}

	public void SetGridSize(int newSize)
	{
		this.gridSize = newSize;
		this.material.SetFloat("_GridSize", (float)this.gridSize);
		this.CreateRenderTexture();
	}

	public void SetSplatTexture(Texture2D texture)
	{
		this.material.SetTexture("_Splat1", texture);
	}

	public void UpdateMap(Vector3 newPosition)
	{
		if (!this.material)
		{
			Debug.Log("No Material to render splat!");
			return;
		}
		if (!this.probabilityTexture)
		{
			this.CreateRenderTexture();
		}
		float single = 1f - (float)this.gridSize / (float)this.probabilityTexture.height * 2f;
		float single1 = -1f;
		float single2 = single1 + (float)this.gridSize / (float)this.probabilityTexture.width * 2f;
		float single3 = 1f;
		if (SystemInfo.graphicsDeviceVersion.StartsWith("OpenGL", StringComparison.InvariantCultureIgnoreCase))
		{
			single3 = -1f;
			single = -1f + (float)this.gridSize / (float)this.probabilityTexture.height * 2f;
		}
		float single4 = newPosition.z - (float)Mathf.FloorToInt((float)this.gridSize * 0.5f * this.gridScale);
		float single5 = newPosition.x - (float)Mathf.FloorToInt((float)this.gridSize * 0.5f * this.gridScale);
		float single6 = single4 + (float)this.gridSize * this.gridScale;
		float single7 = single5 + (float)this.gridSize * this.gridScale;
		this.material.SetFloat("_TerrainSize", Terrain.activeTerrain.terrainData.size.x);
		this.material.SetVector("_Position", new Vector4(newPosition.x, newPosition.y, newPosition.z, 1f));
		int num = (FPGrass.Support.DetailProbabilityFilterMode != FilterMode.Point ? 1 : 0);
		RenderTexture renderTexture = RenderTexture.active;
		try
		{
			GL.PushMatrix();
			RenderTexture.active = this.probabilityTexture;
			GL.LoadPixelMatrix(0f, (float)this.probabilityTexture.width, 0f, (float)this.probabilityTexture.height);
			this.material.SetPass(num);
			GL.Begin(5);
			GL.TexCoord(new Vector3(single7, single4, 0f));
			GL.Vertex3(single2, single3, 0f);
			GL.TexCoord(new Vector3(single5, single4, 0f));
			GL.Vertex3(single1, single3, 0f);
			GL.TexCoord(new Vector3(single7, single6, 0f));
			GL.Vertex3(single2, single, 0f);
			GL.TexCoord(new Vector3(single5, single6, 0f));
			GL.Vertex3(single1, single, 0f);
			GL.End();
			GL.PopMatrix();
		}
		finally
		{
			RenderTexture.active = renderTexture;
		}
	}
}