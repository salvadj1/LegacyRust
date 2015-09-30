using Facepunch;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FPGrass : UnityEngine.MonoBehaviour, IFPGrassAsset
{
	[SerializeField]
	private List<FPGrassLevel> children = new List<FPGrassLevel>();

	public Camera parentCamera;

	public int numberOfLevels = 4;

	public float baseLevelSize = 20f;

	public int gridSizePerLevel = 28;

	[SerializeField]
	private float gridSizeAtFinestLevel;

	public Material material;

	[SerializeField]
	private float scatterAmount = 1f;

	[SerializeField]
	private float normalBias = 0.7f;

	public FPGrassProbabilities grassProbabilities;

	public FPGrassAtlas grassAtlas;

	public bool followSceneCamera;

	public bool toggleWireframe;

	[SerializeField]
	private float windSpeed = 0.1f;

	[SerializeField]
	private float windSize = 1f;

	[SerializeField]
	private float windBending = 1f;

	[SerializeField]
	private Color windTint = Color.white;

	[HideInInspector]
	[SerializeField]
	private Texture2D heightMap;

	[HideInInspector]
	[SerializeField]
	private Texture2D normalMap;

	[HideInInspector]
	[SerializeField]
	private Texture2D splatMap;

	[NonSerialized]
	private bool settingsDirty;

	private static List<FPGrass> AllEnabledFPGrass;

	private static List<FPGrass> AllEnabledFPGrassInstancesSwap;

	[NonSerialized]
	private bool inList;

	public static bool castShadows;

	public static bool receiveShadows;

	public static bool anyEnabled
	{
		get
		{
			return FPGrass.AllEnabledFPGrass.Count > 0;
		}
	}

	public float NormalBias
	{
		get
		{
			return this.normalBias;
		}
		set
		{
			this.normalBias = value;
			this.material.SetFloat("_GroundNormalBias", this.normalBias);
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].levelMaterial.SetFloat("_GroundNormalBias", this.normalBias);
			}
		}
	}

	public float ScatterAmount
	{
		get
		{
			return this.scatterAmount;
		}
		set
		{
			this.scatterAmount = value;
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].levelMaterial.SetFloat("_ScatterAmount", this.scatterAmount);
			}
		}
	}

	public float WindBending
	{
		get
		{
			return this.windBending;
		}
		set
		{
			this.windBending = value;
			this.UpdateWindSettings();
		}
	}

	public float WindSize
	{
		get
		{
			return this.windSize;
		}
		set
		{
			this.windSize = value;
		}
	}

	public float WindSpeed
	{
		get
		{
			return this.windSpeed;
		}
		set
		{
			this.windSpeed = value;
			this.UpdateWindSettings();
		}
	}

	public Color WindTint
	{
		get
		{
			return this.windTint;
		}
		set
		{
			this.windTint = value;
			this.UpdateWindSettings();
		}
	}

	static FPGrass()
	{
		FPGrass.AllEnabledFPGrass = new List<FPGrass>();
		FPGrass.AllEnabledFPGrassInstancesSwap = new List<FPGrass>();
		FPGrass.castShadows = false;
		FPGrass.receiveShadows = true;
	}

	public FPGrass()
	{
	}

	private void Awake()
	{
		if (!this.material)
		{
			this.material = (Material)UnityEngine.Object.Instantiate(Bundling.Load("rust/fpgrass/grassmaterial", typeof(Material)));
		}
	}

	internal static void DrawAllGrass(ref FPGrass.RenderArguments renderArgs)
	{
		List<FPGrass> allEnabledFPGrass = FPGrass.AllEnabledFPGrass;
		FPGrass.AllEnabledFPGrassInstancesSwap.AddRange(allEnabledFPGrass);
		FPGrass.AllEnabledFPGrass = FPGrass.AllEnabledFPGrassInstancesSwap;
		FPGrass.AllEnabledFPGrassInstancesSwap = allEnabledFPGrass;
		try
		{
			foreach (FPGrass allEnabledFPGrassInstancesSwap in FPGrass.AllEnabledFPGrassInstancesSwap)
			{
				allEnabledFPGrassInstancesSwap.Render(ref renderArgs);
			}
		}
		finally
		{
			FPGrass.AllEnabledFPGrassInstancesSwap.Clear();
		}
	}

	private bool EnterList()
	{
		if (this.inList)
		{
			return false;
		}
		FPGrass.AllEnabledFPGrass.Add(this);
		this.inList = true;
		return true;
	}

	private bool ExitList()
	{
		if (!this.inList)
		{
			return false;
		}
		bool flag = FPGrass.AllEnabledFPGrass.Remove(this);
		this.inList = false;
		return flag;
	}

	private void Initialize()
	{
		if (!FPGrass.Support.Supported)
		{
			return;
		}
		if (!this.grassProbabilities)
		{
			this.grassProbabilities = ScriptableObject.CreateInstance<FPGrassProbabilities>();
			this.grassProbabilities.name = "FPGrassProbabilities";
		}
		if (!this.grassAtlas)
		{
			this.grassAtlas = ScriptableObject.CreateInstance<FPGrassAtlas>();
			this.grassAtlas.name = "FPGrassAtlas";
		}
		this.settingsDirty = true;
		this.UpdateProbabilities();
		this.UpdateGrassProperties();
	}

	private void OnDisable()
	{
		this.ExitList();
		if (!Terrain.activeTerrain)
		{
			return;
		}
		Terrain.activeTerrain.detailObjectDistance = 134.6f;
		Terrain.activeTerrain.detailObjectDensity = 1f;
	}

	private void OnEnable()
	{
		if (!FPGrass.Support.Supported)
		{
			return;
		}
		this.EnterList();
		if (!Terrain.activeTerrain)
		{
			return;
		}
		Terrain.activeTerrain.detailObjectDistance = 0f;
		Terrain.activeTerrain.detailObjectDensity = 0f;
	}

	private void OnValidate()
	{
		this.Initialize();
	}

	private void Render(ref FPGrass.RenderArguments renderArgs)
	{
		if (base.enabled)
		{
			foreach (FPGrassLevel child in this.children)
			{
				if (!child.enabled)
				{
					continue;
				}
				if (!renderArgs.immediate)
				{
					child.UpdateLevel(renderArgs.center, renderArgs.terrain);
				}
				if (!child.enabled)
				{
					continue;
				}
				child.Render(ref renderArgs);
			}
		}
	}

	private void Reset()
	{
		for (int i = base.transform.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.DestroyImmediate(base.transform.GetChild(i).gameObject);
		}
		if (this.grassAtlas)
		{
			UnityEngine.Object.DestroyImmediate(this.grassAtlas);
		}
		if (this.grassProbabilities)
		{
			UnityEngine.Object.DestroyImmediate(this.grassProbabilities);
		}
		this.Initialize();
	}

	private void Start()
	{
		this.Initialize();
	}

	private void Update()
	{
		if (!FPGrass.Support.Supported)
		{
			return;
		}
		if (!grass.on)
		{
			this.ExitList();
			return;
		}
		if (!this.EnterList())
		{
			this.settingsDirty = true;
			if (this.settingsDirty)
			{
				this.UpdateProbabilities();
				this.UpdateGrassProperties();
				this.settingsDirty = false;
			}
		}
		else
		{
			this.Initialize();
		}
		if (Application.isPlaying && this.parentCamera)
		{
			this.UpdateLevels(this.parentCamera.transform.position);
		}
	}

	public void UpdateGrassProperties()
	{
		if (!this.grassAtlas || !this.material)
		{
			return;
		}
		for (int i = 0; i < 16; i++)
		{
			FPGrassProperty item = this.grassAtlas.properties[i];
			this.material.SetColor(string.Concat("_GrassColorsOne", i), item.Color1);
			this.material.SetColor(string.Concat("_GrassColorsTwo", i), item.Color2);
			this.material.SetVector(string.Concat("_GrassSizes", i), new Vector4(item.MinWidth, item.MaxWidth, item.MinHeight, item.MaxHeight));
		}
		for (int j = 0; j < this.children.Count; j++)
		{
			for (int k = 0; k < 16; k++)
			{
				FPGrassProperty fPGrassProperty = this.grassAtlas.properties[k];
				this.children[j].levelMaterial.SetColor(string.Concat("_GrassColorsOne", k), fPGrassProperty.Color1);
				this.children[j].levelMaterial.SetColor(string.Concat("_GrassColorsTwo", k), fPGrassProperty.Color2);
				this.children[j].levelMaterial.SetVector(string.Concat("_GrassSizes", k), new Vector4(fPGrassProperty.MinWidth, fPGrassProperty.MaxWidth, fPGrassProperty.MinHeight, fPGrassProperty.MaxHeight));
			}
		}
	}

	public void UpdateLevels(Vector3 position)
	{
		base.transform.position = Vector3.zero;
		Terrain terrain = Terrain.activeTerrain;
		if (terrain)
		{
			foreach (FPGrassLevel child in this.children)
			{
				child.UpdateLevel(position, terrain);
			}
		}
	}

	public void UpdateProbabilities()
	{
		foreach (FPGrassLevel child in this.children)
		{
			child.probabilityGenerator.SetDetailProbabilities(this.grassProbabilities.GetTexture());
		}
	}

	public void UpdateWindSettings()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].levelMaterial.SetVector("_WaveAndDistance", new Vector4(this.windSpeed, this.windSize, this.windBending, 0f));
			this.children[i].levelMaterial.SetColor("_WavingTint", this.windTint);
		}
	}

	internal struct RenderArguments
	{
		public Plane[] frustum;

		public Camera camera;

		public Terrain terrain;

		public Vector3 center;

		public bool immediate;
	}

	public static class Support
	{
		public readonly static bool Supported;

		public readonly static bool DisplacementExpensive;

		public static FilterMode DetailProbabilityFilterMode;

		public readonly static RenderTextureFormat ProbabilityRenderTextureFormat4Channel;

		public readonly static RenderTextureFormat ProbabilityRenderTextureFormat1Channel;

		static Support()
		{
			bool flag;
			bool flag1;
			if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
			{
				FPGrass.Support.ProbabilityRenderTextureFormat4Channel = RenderTextureFormat.Default;
				flag = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Default);
			}
			else
			{
				FPGrass.Support.ProbabilityRenderTextureFormat4Channel = RenderTextureFormat.ARGB32;
				flag = true;
			}
			if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8))
			{
				flag1 = false;
				FPGrass.Support.ProbabilityRenderTextureFormat1Channel = RenderTextureFormat.Default;
			}
			else
			{
				FPGrass.Support.ProbabilityRenderTextureFormat1Channel = RenderTextureFormat.R8;
				flag1 = true;
			}
			if (!flag || flag1)
			{
				FPGrass.Support.DisplacementExpensive = false;
			}
			else
			{
				FPGrass.Support.DisplacementExpensive = true;
				FPGrass.Support.ProbabilityRenderTextureFormat1Channel = FPGrass.Support.ProbabilityRenderTextureFormat4Channel;
			}
			FPGrass.Support.Supported = (flag1 ? true : flag);
			if (SystemInfo.supportsComputeShaders)
			{
				FPGrass.Support.DetailProbabilityFilterMode = FilterMode.Point;
			}
			else
			{
				FPGrass.Support.DetailProbabilityFilterMode = FilterMode.Bilinear;
			}
		}
	}
}