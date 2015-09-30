using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Terrain/Terrain Toolkit")]
[ExecuteInEditMode]
public class TerrainToolkit : MonoBehaviour
{
	public GUISkin guiSkin;

	public Texture2D createIcon;

	public Texture2D erodeIcon;

	public Texture2D textureIcon;

	public Texture2D mooreIcon;

	public Texture2D vonNeumannIcon;

	public Texture2D mountainsIcon;

	public Texture2D hillsIcon;

	public Texture2D plateausIcon;

	public Texture2D defaultTexture;

	public int toolModeInt;

	private TerrainToolkit.ErosionMode erosionMode;

	private TerrainToolkit.ErosionType erosionType;

	public int erosionTypeInt;

	private TerrainToolkit.GeneratorType generatorType;

	public int generatorTypeInt;

	public bool isBrushOn;

	public bool isBrushHidden;

	public bool isBrushPainting;

	public Vector3 brushPosition;

	public float brushSize = 50f;

	public float brushOpacity = 1f;

	public float brushSoftness = 0.5f;

	public int neighbourhoodInt;

	private TerrainToolkit.Neighbourhood neighbourhood;

	public bool useDifferenceMaps = true;

	public int thermalIterations = 25;

	public float thermalMinSlope = 1f;

	public float thermalFalloff = 0.5f;

	public int hydraulicTypeInt;

	public TerrainToolkit.HydraulicType hydraulicType;

	public int hydraulicIterations = 25;

	public float hydraulicMaxSlope = 60f;

	public float hydraulicFalloff = 0.5f;

	public float hydraulicRainfall = 0.01f;

	public float hydraulicEvaporation = 0.5f;

	public float hydraulicSedimentSolubility = 0.01f;

	public float hydraulicSedimentSaturation = 0.1f;

	public float hydraulicVelocityRainfall = 0.01f;

	public float hydraulicVelocityEvaporation = 0.5f;

	public float hydraulicVelocitySedimentSolubility = 0.01f;

	public float hydraulicVelocitySedimentSaturation = 0.1f;

	public float hydraulicVelocity = 20f;

	public float hydraulicMomentum = 1f;

	public float hydraulicEntropy;

	public float hydraulicDowncutting = 0.1f;

	public int tidalIterations = 25;

	public float tidalSeaLevel = 50f;

	public float tidalRangeAmount = 5f;

	public float tidalCliffLimit = 60f;

	public int windIterations = 25;

	public float windDirection;

	public float windForce = 0.5f;

	public float windLift = 0.01f;

	public float windGravity = 0.5f;

	public float windCapacity = 0.01f;

	public float windEntropy = 0.1f;

	public float windSmoothing = 0.25f;

	public SplatPrototype[] splatPrototypes;

	public Texture2D tempTexture;

	public float slopeBlendMinAngle = 60f;

	public float slopeBlendMaxAngle = 75f;

	public List<float> heightBlendPoints;

	public string[] gradientStyles;

	public int voronoiTypeInt;

	public TerrainToolkit.VoronoiType voronoiType;

	public int voronoiCells = 16;

	public float voronoiFeatures = 1f;

	public float voronoiScale = 1f;

	public float voronoiBlend = 1f;

	public float diamondSquareDelta = 0.5f;

	public float diamondSquareBlend = 1f;

	public int perlinFrequency = 4;

	public float perlinAmplitude = 1f;

	public int perlinOctaves = 8;

	public float perlinBlend = 1f;

	public float smoothBlend = 1f;

	public int smoothIterations;

	public float normaliseMin;

	public float normaliseMax = 1f;

	public float normaliseBlend = 1f;

	[NonSerialized]
	public bool presetsInitialised;

	[NonSerialized]
	public int voronoiPresetId;

	[NonSerialized]
	public int fractalPresetId;

	[NonSerialized]
	public int perlinPresetId;

	[NonSerialized]
	public int thermalErosionPresetId;

	[NonSerialized]
	public int fastHydraulicErosionPresetId;

	[NonSerialized]
	public int fullHydraulicErosionPresetId;

	[NonSerialized]
	public int velocityHydraulicErosionPresetId;

	[NonSerialized]
	public int tidalErosionPresetId;

	[NonSerialized]
	public int windErosionPresetId;

	public ArrayList voronoiPresets = new ArrayList();

	public ArrayList fractalPresets = new ArrayList();

	public ArrayList perlinPresets = new ArrayList();

	public ArrayList thermalErosionPresets = new ArrayList();

	public ArrayList fastHydraulicErosionPresets = new ArrayList();

	public ArrayList fullHydraulicErosionPresets = new ArrayList();

	public ArrayList velocityHydraulicErosionPresets = new ArrayList();

	public ArrayList tidalErosionPresets = new ArrayList();

	public ArrayList windErosionPresets = new ArrayList();

	public TerrainToolkit()
	{
	}

	public void addBlendPoints()
	{
		float item = 0f;
		if (this.heightBlendPoints.Count > 0)
		{
			item = (float)this.heightBlendPoints[this.heightBlendPoints.Count - 1];
		}
		float single = item + (1f - item) * 0.33f;
		this.heightBlendPoints.Add(single);
		single = item + (1f - item) * 0.66f;
		this.heightBlendPoints.Add(single);
	}

	public void addPresets()
	{
		this.presetsInitialised = true;
		this.voronoiPresets = new ArrayList();
		this.fractalPresets = new ArrayList();
		this.perlinPresets = new ArrayList();
		this.thermalErosionPresets = new ArrayList();
		this.fastHydraulicErosionPresets = new ArrayList();
		this.fullHydraulicErosionPresets = new ArrayList();
		this.velocityHydraulicErosionPresets = new ArrayList();
		this.tidalErosionPresets = new ArrayList();
		this.windErosionPresets = new ArrayList();
		this.voronoiPresets.Add(new TerrainToolkit.voronoiPresetData("Scattered Peaks", TerrainToolkit.VoronoiType.Linear, 16, 8f, 0.5f, 1f));
		this.voronoiPresets.Add(new TerrainToolkit.voronoiPresetData("Rolling Hills", TerrainToolkit.VoronoiType.Sine, 8, 8f, 0f, 1f));
		this.voronoiPresets.Add(new TerrainToolkit.voronoiPresetData("Jagged Mountains", TerrainToolkit.VoronoiType.Linear, 32, 32f, 0.5f, 1f));
		this.fractalPresets.Add(new TerrainToolkit.fractalPresetData("Rolling Plains", 0.4f, 1f));
		this.fractalPresets.Add(new TerrainToolkit.fractalPresetData("Rough Mountains", 0.5f, 1f));
		this.fractalPresets.Add(new TerrainToolkit.fractalPresetData("Add Noise", 0.75f, 0.05f));
		this.perlinPresets.Add(new TerrainToolkit.perlinPresetData("Rough Plains", 2, 0.5f, 9, 1f));
		this.perlinPresets.Add(new TerrainToolkit.perlinPresetData("Rolling Hills", 5, 0.75f, 3, 1f));
		this.perlinPresets.Add(new TerrainToolkit.perlinPresetData("Rocky Mountains", 4, 1f, 8, 1f));
		this.perlinPresets.Add(new TerrainToolkit.perlinPresetData("Hellish Landscape", 11, 1f, 7, 1f));
		this.perlinPresets.Add(new TerrainToolkit.perlinPresetData("Add Noise", 10, 1f, 8, 0.2f));
		this.thermalErosionPresets.Add(new TerrainToolkit.thermalErosionPresetData("Gradual, Weak Erosion", 25, 7.5f, 0.5f));
		this.thermalErosionPresets.Add(new TerrainToolkit.thermalErosionPresetData("Fast, Harsh Erosion", 25, 2.5f, 0.1f));
		this.thermalErosionPresets.Add(new TerrainToolkit.thermalErosionPresetData("Thermal Erosion Brush", 25, 0.1f, 0f));
		this.fastHydraulicErosionPresets.Add(new TerrainToolkit.fastHydraulicErosionPresetData("Rainswept Earth", 25, 70f, 1f));
		this.fastHydraulicErosionPresets.Add(new TerrainToolkit.fastHydraulicErosionPresetData("Terraced Slopes", 25, 30f, 0.4f));
		this.fastHydraulicErosionPresets.Add(new TerrainToolkit.fastHydraulicErosionPresetData("Hydraulic Erosion Brush", 25, 85f, 1f));
		this.fullHydraulicErosionPresets.Add(new TerrainToolkit.fullHydraulicErosionPresetData("Low Rainfall, Hard Rock", 25, 0.01f, 0.5f, 0.01f, 0.1f));
		this.fullHydraulicErosionPresets.Add(new TerrainToolkit.fullHydraulicErosionPresetData("Low Rainfall, Soft Earth", 25, 0.01f, 0.5f, 0.06f, 0.15f));
		this.fullHydraulicErosionPresets.Add(new TerrainToolkit.fullHydraulicErosionPresetData("Heavy Rainfall, Hard Rock", 25, 0.02f, 0.5f, 0.01f, 0.1f));
		this.fullHydraulicErosionPresets.Add(new TerrainToolkit.fullHydraulicErosionPresetData("Heavy Rainfall, Soft Earth", 25, 0.02f, 0.5f, 0.06f, 0.15f));
		this.velocityHydraulicErosionPresets.Add(new TerrainToolkit.velocityHydraulicErosionPresetData("Low Rainfall, Hard Rock", 25, 0.01f, 0.5f, 0.01f, 0.1f, 1f, 1f, 0.05f, 0.12f));
		this.velocityHydraulicErosionPresets.Add(new TerrainToolkit.velocityHydraulicErosionPresetData("Low Rainfall, Soft Earth", 25, 0.01f, 0.5f, 0.06f, 0.15f, 1.2f, 2.8f, 0.05f, 0.12f));
		this.velocityHydraulicErosionPresets.Add(new TerrainToolkit.velocityHydraulicErosionPresetData("Heavy Rainfall, Hard Rock", 25, 0.02f, 0.5f, 0.01f, 0.1f, 1.1f, 2.2f, 0.05f, 0.12f));
		this.velocityHydraulicErosionPresets.Add(new TerrainToolkit.velocityHydraulicErosionPresetData("Heavy Rainfall, Soft Earth", 25, 0.02f, 0.5f, 0.06f, 0.15f, 1.2f, 2.4f, 0.05f, 0.12f));
		this.velocityHydraulicErosionPresets.Add(new TerrainToolkit.velocityHydraulicErosionPresetData("Carved Stone", 25, 0.01f, 0.5f, 0.01f, 0.1f, 2f, 1.25f, 0.05f, 0.35f));
		this.tidalErosionPresets.Add(new TerrainToolkit.tidalErosionPresetData("Low Tidal Range, Calm Waves", 25, 5f, 65f));
		this.tidalErosionPresets.Add(new TerrainToolkit.tidalErosionPresetData("Low Tidal Range, Strong Waves", 25, 5f, 35f));
		this.tidalErosionPresets.Add(new TerrainToolkit.tidalErosionPresetData("High Tidal Range, Calm Water", 25, 15f, 55f));
		this.tidalErosionPresets.Add(new TerrainToolkit.tidalErosionPresetData("High Tidal Range, Strong Waves", 25, 15f, 25f));
		this.windErosionPresets.Add(new TerrainToolkit.windErosionPresetData("Default (Northerly)", 25, 180f, 0.5f, 0.01f, 0.5f, 0.01f, 0.1f, 0.25f));
		this.windErosionPresets.Add(new TerrainToolkit.windErosionPresetData("Default (Southerly)", 25, 0f, 0.5f, 0.01f, 0.5f, 0.01f, 0.1f, 0.25f));
		this.windErosionPresets.Add(new TerrainToolkit.windErosionPresetData("Default (Easterly)", 25, 270f, 0.5f, 0.01f, 0.5f, 0.01f, 0.1f, 0.25f));
		this.windErosionPresets.Add(new TerrainToolkit.windErosionPresetData("Default (Westerly)", 25, 90f, 0.5f, 0.01f, 0.5f, 0.01f, 0.1f, 0.25f));
	}

	public void addSplatPrototype(Texture2D tex, int index)
	{
		SplatPrototype[] splatPrototype = new SplatPrototype[index + 1];
		for (int i = 0; i <= index; i++)
		{
			splatPrototype[i] = new SplatPrototype();
			if (i != index)
			{
				splatPrototype[i].texture = this.splatPrototypes[i].texture;
				splatPrototype[i].tileSize = this.splatPrototypes[i].tileSize;
			}
			else
			{
				splatPrototype[i].texture = tex;
				splatPrototype[i].tileSize = new Vector2(15f, 15f);
			}
		}
		this.splatPrototypes = splatPrototype;
		if (index + 1 > 2)
		{
			this.addBlendPoints();
		}
	}

	private void convertIntVarsToEnums()
	{
		int num = this.erosionTypeInt;
		switch (num)
		{
			case 0:
			{
				this.erosionType = TerrainToolkit.ErosionType.Thermal;
				break;
			}
			case 1:
			{
				this.erosionType = TerrainToolkit.ErosionType.Hydraulic;
				break;
			}
			case 2:
			{
				this.erosionType = TerrainToolkit.ErosionType.Tidal;
				break;
			}
			case 3:
			{
				this.erosionType = TerrainToolkit.ErosionType.Wind;
				break;
			}
			case 4:
			{
				this.erosionType = TerrainToolkit.ErosionType.Glacial;
				break;
			}
		}
		num = this.hydraulicTypeInt;
		switch (num)
		{
			case 0:
			{
				this.hydraulicType = TerrainToolkit.HydraulicType.Fast;
				break;
			}
			case 1:
			{
				this.hydraulicType = TerrainToolkit.HydraulicType.Full;
				break;
			}
			case 2:
			{
				this.hydraulicType = TerrainToolkit.HydraulicType.Velocity;
				break;
			}
		}
		num = this.generatorTypeInt;
		switch (num)
		{
			case 0:
			{
				this.generatorType = TerrainToolkit.GeneratorType.Voronoi;
				break;
			}
			case 1:
			{
				this.generatorType = TerrainToolkit.GeneratorType.DiamondSquare;
				break;
			}
			case 2:
			{
				this.generatorType = TerrainToolkit.GeneratorType.Perlin;
				break;
			}
			case 3:
			{
				this.generatorType = TerrainToolkit.GeneratorType.Smooth;
				break;
			}
			case 4:
			{
				this.generatorType = TerrainToolkit.GeneratorType.Normalise;
				break;
			}
		}
		num = this.voronoiTypeInt;
		switch (num)
		{
			case 0:
			{
				this.voronoiType = TerrainToolkit.VoronoiType.Linear;
				break;
			}
			case 1:
			{
				this.voronoiType = TerrainToolkit.VoronoiType.Sine;
				break;
			}
			case 2:
			{
				this.voronoiType = TerrainToolkit.VoronoiType.Tangent;
				break;
			}
		}
		num = this.neighbourhoodInt;
		if (num == 0)
		{
			this.neighbourhood = TerrainToolkit.Neighbourhood.Moore;
		}
		else if (num == 1)
		{
			this.neighbourhood = TerrainToolkit.Neighbourhood.VonNeumann;
		}
	}

	public void deleteAllBlendPoints()
	{
		this.heightBlendPoints = new List<float>();
	}

	public void deleteAllSplatPrototypes()
	{
		this.splatPrototypes = new SplatPrototype[0];
	}

	public void deleteBlendPoints()
	{
		if (this.heightBlendPoints.Count > 0)
		{
			this.heightBlendPoints.RemoveAt(this.heightBlendPoints.Count - 1);
		}
		if (this.heightBlendPoints.Count > 0)
		{
			this.heightBlendPoints.RemoveAt(this.heightBlendPoints.Count - 1);
		}
	}

	public void deleteSplatPrototype(Texture2D tex, int index)
	{
		int length = 0;
		length = (int)this.splatPrototypes.Length;
		SplatPrototype[] splatPrototype = new SplatPrototype[length - 1];
		int num = 0;
		for (int i = 0; i < length; i++)
		{
			if (i != index)
			{
				splatPrototype[num] = new SplatPrototype();
				splatPrototype[num].texture = this.splatPrototypes[i].texture;
				splatPrototype[num].tileSize = this.splatPrototypes[i].tileSize;
				num++;
			}
		}
		this.splatPrototypes = splatPrototype;
		if (length - 1 > 1)
		{
			this.deleteBlendPoints();
		}
	}

	private void dsCalculateHeight(float[,] heightMap, Vector2 arraySize, int Tx, int Ty, Vector2[] points, float heightRange)
	{
		int num = (int)arraySize.x;
		int num1 = (int)arraySize.y;
		float single = 0f;
		for (int i = 0; i < 4; i++)
		{
			if (points[i].x < 0f)
			{
				points[i].x = points[i].x + (float)(num - 1);
			}
			else if (points[i].x > (float)num)
			{
				points[i].x = points[i].x - (float)(num - 1);
			}
			else if (points[i].y < 0f)
			{
				points[i].y = points[i].y + (float)(num1 - 1);
			}
			else if (points[i].y > (float)num1)
			{
				points[i].y = points[i].y - (float)(num1 - 1);
			}
			single = single + (float)(heightMap[(int)points[i].x, (int)points[i].y] / 4f);
		}
		single = single + (UnityEngine.Random.@value * heightRange - heightRange / 2f);
		if (single < 0f)
		{
			single = 0f;
		}
		else if (single > 1f)
		{
			single = 1f;
		}
		heightMap[Tx, Ty] = single;
		if (Tx == 0)
		{
			heightMap[num - 1, Ty] = single;
		}
		else if (Tx == num - 1)
		{
			heightMap[0, Ty] = single;
		}
		else if (Ty == 0)
		{
			heightMap[Tx, num1 - 1] = single;
		}
		else if (Ty == num1 - 1)
		{
			heightMap[Tx, 0] = single;
		}
	}

	public void dummyErosionProgress(string titleString, string displayString, int iteration, int nIterations, float percentComplete)
	{
	}

	public void dummyGeneratorProgress(string titleString, string displayString, float percentComplete)
	{
	}

	public void dummyTextureProgress(string titleString, string displayString, float percentComplete)
	{
	}

	public void erodeAllTerrain(TerrainToolkit.ErosionProgressDelegate erosionProgressDelegate)
	{
		int num;
		this.erosionMode = TerrainToolkit.ErosionMode.Filter;
		this.convertIntVarsToEnums();
		Terrain component = (Terrain)base.GetComponent(typeof(Terrain));
		if (component == null)
		{
			return;
		}
		try
		{
			TerrainData terrainDatum = component.terrainData;
			int num1 = terrainDatum.heightmapWidth;
			int num2 = terrainDatum.heightmapHeight;
			float[,] heights = terrainDatum.GetHeights(0, 0, num1, num2);
			switch (this.erosionType)
			{
				case TerrainToolkit.ErosionType.Thermal:
				{
					num = this.thermalIterations;
					heights = this.fastErosion(heights, new Vector2((float)num1, (float)num2), num, erosionProgressDelegate);
					break;
				}
				case TerrainToolkit.ErosionType.Hydraulic:
				{
					num = this.hydraulicIterations;
					switch (this.hydraulicType)
					{
						case TerrainToolkit.HydraulicType.Fast:
						{
							heights = this.fastErosion(heights, new Vector2((float)num1, (float)num2), num, erosionProgressDelegate);
							break;
						}
						case TerrainToolkit.HydraulicType.Full:
						{
							heights = this.fullHydraulicErosion(heights, new Vector2((float)num1, (float)num2), num, erosionProgressDelegate);
							break;
						}
						case TerrainToolkit.HydraulicType.Velocity:
						{
							heights = this.velocityHydraulicErosion(heights, new Vector2((float)num1, (float)num2), num, erosionProgressDelegate);
							break;
						}
					}
					break;
				}
				case TerrainToolkit.ErosionType.Tidal:
				{
					Vector3 vector3 = terrainDatum.size;
					if (this.tidalSeaLevel < base.transform.position.y || this.tidalSeaLevel > base.transform.position.y + vector3.y)
					{
						Debug.LogError("Sea level does not intersect terrain object. Erosion operation failed.");
					}
					else
					{
						num = this.tidalIterations;
						heights = this.fastErosion(heights, new Vector2((float)num1, (float)num2), num, erosionProgressDelegate);
					}
					break;
				}
				case TerrainToolkit.ErosionType.Wind:
				{
					num = this.windIterations;
					heights = this.windErosion(heights, new Vector2((float)num1, (float)num2), num, erosionProgressDelegate);
					break;
				}
				default:
				{
					return;
				}
			}
			terrainDatum.SetHeights(0, 0, heights);
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("An error occurred: ", exception));
		}
	}

	private void erodeTerrainWithBrush()
	{
		this.erosionMode = TerrainToolkit.ErosionMode.Brush;
		Terrain component = (Terrain)base.GetComponent(typeof(Terrain));
		if (component == null)
		{
			return;
		}
		int num = 0;
		int num1 = 0;
		try
		{
			TerrainData terrainDatum = component.terrainData;
			int num2 = terrainDatum.heightmapWidth;
			int num3 = terrainDatum.heightmapHeight;
			Vector3 vector3 = terrainDatum.size;
			int length = (int)Mathf.Floor((float)num2 / vector3.x * this.brushSize);
			int length1 = (int)Mathf.Floor((float)num3 / vector3.z * this.brushSize);
			Vector3 vector31 = base.transform.InverseTransformPoint(this.brushPosition);
			num = (int)Mathf.Round(vector31.x / vector3.x * (float)num2 - (float)(length / 2));
			num1 = (int)Mathf.Round(vector31.z / vector3.z * (float)num3 - (float)(length1 / 2));
			if (num < 0)
			{
				length = length + num;
				num = 0;
			}
			if (num1 < 0)
			{
				length1 = length1 + num1;
				num1 = 0;
			}
			if (num + length > num2)
			{
				length = num2 - num;
			}
			if (num1 + length1 > num3)
			{
				length1 = num3 - num1;
			}
			float[,] heights = terrainDatum.GetHeights(num, num1, length, length1);
			length = heights.GetLength(1);
			length1 = heights.GetLength(0);
			float[,] singleArray = (float[,])heights.Clone();
			TerrainToolkit.ErosionProgressDelegate erosionProgressDelegate = new TerrainToolkit.ErosionProgressDelegate(this.dummyErosionProgress);
			singleArray = this.fastErosion(singleArray, new Vector2((float)length, (float)length1), 1, erosionProgressDelegate);
			float single = (float)length / 2f;
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length1; j++)
				{
					float single1 = heights[j, i];
					float single2 = singleArray[j, i];
					float single3 = Vector2.Distance(new Vector2((float)j, (float)i), new Vector2(single, single));
					float single4 = 1f - (single3 - (single - single * this.brushSoftness)) / (single * this.brushSoftness);
					if (single4 < 0f)
					{
						single4 = 0f;
					}
					else if (single4 > 1f)
					{
						single4 = 1f;
					}
					single4 = single4 * this.brushOpacity;
					float single5 = single2 * single4 + single1 * (1f - single4);
					heights[j, i] = single5;
				}
			}
			terrainDatum.SetHeights(num, num1, heights);
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("A brush error occurred: ", exception));
		}
	}

	private float[,] fastErosion(float[,] heightMap, Vector2 arraySize, int iterations, TerrainToolkit.ErosionProgressDelegate erosionProgressDelegate)
	{
		int num;
		int num1;
		int num2;
		int num3;
		int num4;
		int num5;
		int k;
		int j;
		int l;
		int m;
		float single;
		float single1;
		int num6 = (int)arraySize.y;
		int num7 = (int)arraySize.x;
		float[,] singleArray = new float[num6, num7];
		Vector3 component = ((Terrain)base.GetComponent(typeof(Terrain))).terrainData.size;
		float single2 = 0f;
		float single3 = 0f;
		float single4 = 0f;
		float single5 = 0f;
		float single6 = 0f;
		float single7 = 0f;
		float single8 = 0f;
		float single9 = 0f;
		float single10 = 0f;
		switch (this.erosionType)
		{
			case TerrainToolkit.ErosionType.Thermal:
			{
				single2 = component.x / (float)num6 * Mathf.Tan(this.thermalMinSlope * 0.0174532924f) / component.y;
				if (single2 > 1f)
				{
					single2 = 1f;
				}
				if (this.thermalFalloff == 1f)
				{
					this.thermalFalloff = 0.999f;
				}
				float single11 = this.thermalMinSlope + (90f - this.thermalMinSlope) * this.thermalFalloff;
				single3 = component.x / (float)num6 * Mathf.Tan(single11 * 0.0174532924f) / component.y;
				if (single3 > 1f)
				{
					single3 = 1f;
				}
				break;
			}
			case TerrainToolkit.ErosionType.Hydraulic:
			{
				single5 = component.x / (float)num6 * Mathf.Tan(this.hydraulicMaxSlope * 0.0174532924f) / component.y;
				if (this.hydraulicFalloff == 0f)
				{
					this.hydraulicFalloff = 0.001f;
				}
				float single12 = this.hydraulicMaxSlope * (1f - this.hydraulicFalloff);
				single4 = component.x / (float)num6 * Mathf.Tan(single12 * 0.0174532924f) / component.y;
				break;
			}
			case TerrainToolkit.ErosionType.Tidal:
			{
				float single13 = this.tidalSeaLevel - base.transform.position.y;
				Vector3 vector3 = base.transform.position;
				single6 = single13 / (vector3.y + component.y);
				float single14 = this.tidalSeaLevel;
				Vector3 vector31 = base.transform.position;
				float single15 = single14 - vector31.y - this.tidalRangeAmount;
				Vector3 vector32 = base.transform.position;
				single7 = single15 / (vector32.y + component.y);
				float single16 = this.tidalSeaLevel;
				Vector3 vector33 = base.transform.position;
				float single17 = single16 - vector33.y + this.tidalRangeAmount;
				Vector3 vector34 = base.transform.position;
				single8 = single17 / (vector34.y + component.y);
				single9 = single8 - single6;
				single10 = component.x / (float)num6 * Mathf.Tan(this.tidalCliffLimit * 0.0174532924f) / component.y;
				break;
			}
			default:
			{
				return heightMap;
			}
		}
		for (int i = 0; i < iterations; i++)
		{
			for (j = 0; j < num7; j++)
			{
				if (j == 0)
				{
					num1 = 2;
					num3 = 0;
					num5 = 0;
				}
				else if (j != num7 - 1)
				{
					num1 = 3;
					num3 = -1;
					num5 = 1;
				}
				else
				{
					num1 = 2;
					num3 = -1;
					num5 = 1;
				}
				for (k = 0; k < num6; k++)
				{
					if (k == 0)
					{
						num = 2;
						num2 = 0;
						num4 = 0;
					}
					else if (k != num6 - 1)
					{
						num = 3;
						num2 = -1;
						num4 = 1;
					}
					else
					{
						num = 2;
						num2 = -1;
						num4 = 1;
					}
					float single18 = 1f;
					float single19 = 0f;
					float single20 = 0f;
					float single21 = heightMap[k + num4 + num2, j + num5 + num3];
					float single22 = single21;
					int num8 = 0;
					for (l = 0; l < num1; l++)
					{
						for (m = 0; m < num; m++)
						{
							if ((m != num4 || l != num5) && (this.neighbourhood == TerrainToolkit.Neighbourhood.Moore || this.neighbourhood == TerrainToolkit.Neighbourhood.VonNeumann && (m == num4 || l == num5)))
							{
								float single23 = heightMap[k + m + num2, j + l + num3];
								single22 = single22 + single23;
								single = single21 - single23;
								if (single > 0f)
								{
									single20 = single20 + single;
									if (single < single18)
									{
										single18 = single;
									}
									if (single > single19)
									{
										single19 = single;
									}
								}
								num8++;
							}
						}
					}
					float single24 = single20 / (float)num8;
					bool flag = false;
					switch (this.erosionType)
					{
						case TerrainToolkit.ErosionType.Thermal:
						{
							if (single24 >= single2)
							{
								flag = true;
							}
							break;
						}
						case TerrainToolkit.ErosionType.Hydraulic:
						{
							if (single24 > 0f && single24 <= single5)
							{
								flag = true;
							}
							break;
						}
						case TerrainToolkit.ErosionType.Tidal:
						{
							if (single24 > 0f && single24 <= single10 && single21 < single8 && single21 > single7)
							{
								flag = true;
							}
							break;
						}
						default:
						{
							return heightMap;
						}
					}
					if (flag)
					{
						if (this.erosionType != TerrainToolkit.ErosionType.Tidal)
						{
							if (this.erosionType == TerrainToolkit.ErosionType.Thermal)
							{
								single1 = (single24 <= single3 ? (single24 - single2) / (single3 - single2) : 1f);
							}
							else if (single24 >= single4)
							{
								float single25 = single5 - single4;
								single1 = 1f - (single24 - single4) / single25;
							}
							else
							{
								single1 = 1f;
							}
							float single26 = single18 / 2f * single1;
							float single27 = heightMap[k + num4 + num2, j + num5 + num3];
							if (this.erosionMode == TerrainToolkit.ErosionMode.Filter || this.erosionMode == TerrainToolkit.ErosionMode.Brush && this.useDifferenceMaps)
							{
								float single28 = singleArray[k + num4 + num2, j + num5 + num3];
								float single29 = single28 - single26;
								singleArray[k + num4 + num2, j + num5 + num3] = single29;
							}
							else
							{
								float single30 = single27 - single26;
								if (single30 < 0f)
								{
									single30 = 0f;
								}
								heightMap[k + num4 + num2, j + num5 + num3] = single30;
							}
							for (l = 0; l < num1; l++)
							{
								for (m = 0; m < num; m++)
								{
									if ((m != num4 || l != num5) && (this.neighbourhood == TerrainToolkit.Neighbourhood.Moore || this.neighbourhood == TerrainToolkit.Neighbourhood.VonNeumann && (m == num4 || l == num5)))
									{
										float single31 = heightMap[k + m + num2, j + l + num3];
										single = single27 - single31;
										if (single > 0f)
										{
											float single32 = single26 * (single / single20);
											if (this.erosionMode == TerrainToolkit.ErosionMode.Filter || this.erosionMode == TerrainToolkit.ErosionMode.Brush && this.useDifferenceMaps)
											{
												float single33 = singleArray[k + m + num2, j + l + num3];
												float single34 = single33 + single32;
												singleArray[k + m + num2, j + l + num3] = single34;
											}
											else
											{
												single31 = single31 + single32;
												if (single31 < 0f)
												{
													single31 = 0f;
												}
												heightMap[k + m + num2, j + l + num3] = single31;
											}
										}
									}
								}
							}
						}
						else
						{
							float single35 = single22 / (float)(num8 + 1);
							float single36 = Mathf.Abs(single6 - single21);
							single1 = single36 / single9;
							float single37 = single21 * single1 + single35 * (1f - single1);
							float single38 = Mathf.Pow(single36, 3f);
							heightMap[k + num4 + num2, j + num5 + num3] = single6 * single38 + single37 * (1f - single38);
						}
					}
				}
			}
			if ((this.erosionMode == TerrainToolkit.ErosionMode.Filter || this.erosionMode == TerrainToolkit.ErosionMode.Brush && this.useDifferenceMaps) && this.erosionType != TerrainToolkit.ErosionType.Tidal)
			{
				for (j = 0; j < num7; j++)
				{
					for (k = 0; k < num6; k++)
					{
						float single39 = heightMap[k, j] + singleArray[k, j];
						if (single39 > 1f)
						{
							single39 = 1f;
						}
						else if (single39 < 0f)
						{
							single39 = 0f;
						}
						heightMap[k, j] = single39;
						singleArray[k, j] = 0f;
					}
				}
			}
			if (this.erosionMode == TerrainToolkit.ErosionMode.Filter)
			{
				string empty = string.Empty;
				string str = string.Empty;
				switch (this.erosionType)
				{
					case TerrainToolkit.ErosionType.Thermal:
					{
						empty = "Applying Thermal Erosion";
						str = "Applying thermal erosion.";
						break;
					}
					case TerrainToolkit.ErosionType.Hydraulic:
					{
						empty = "Applying Hydraulic Erosion";
						str = "Applying hydraulic erosion.";
						break;
					}
					case TerrainToolkit.ErosionType.Tidal:
					{
						empty = "Applying Tidal Erosion";
						str = "Applying tidal erosion.";
						break;
					}
					default:
					{
						return heightMap;
					}
				}
				float single40 = (float)i / (float)iterations;
				erosionProgressDelegate(empty, str, i, iterations, single40);
			}
		}
		return heightMap;
	}

	public void FastHydraulicErosion(int iterations, float maxSlope, float blendAmount)
	{
		this.erosionTypeInt = 1;
		this.erosionType = TerrainToolkit.ErosionType.Hydraulic;
		this.hydraulicTypeInt = 0;
		this.hydraulicType = TerrainToolkit.HydraulicType.Fast;
		this.hydraulicIterations = iterations;
		this.hydraulicMaxSlope = maxSlope;
		this.hydraulicFalloff = blendAmount;
		this.neighbourhood = TerrainToolkit.Neighbourhood.Moore;
		this.erodeAllTerrain(new TerrainToolkit.ErosionProgressDelegate(this.dummyErosionProgress));
	}

	public void FastThermalErosion(int iterations, float minSlope, float blendAmount)
	{
		this.erosionTypeInt = 0;
		this.erosionType = TerrainToolkit.ErosionType.Thermal;
		this.thermalIterations = iterations;
		this.thermalMinSlope = minSlope;
		this.thermalFalloff = blendAmount;
		this.neighbourhood = TerrainToolkit.Neighbourhood.Moore;
		this.erodeAllTerrain(new TerrainToolkit.ErosionProgressDelegate(this.dummyErosionProgress));
	}

	public void FractalGenerator(float fractalDelta, float blend)
	{
		this.generatorTypeInt = 1;
		this.generatorType = TerrainToolkit.GeneratorType.DiamondSquare;
		this.diamondSquareDelta = fractalDelta;
		this.diamondSquareBlend = blend;
		this.generateTerrain(new TerrainToolkit.GeneratorProgressDelegate(this.dummyGeneratorProgress));
	}

	private float[,] fullHydraulicErosion(float[,] heightMap, Vector2 arraySize, int iterations, TerrainToolkit.ErosionProgressDelegate erosionProgressDelegate)
	{
		int j;
		int i;
		int num;
		int num1;
		int num2;
		int num3;
		int num4;
		int num5;
		int m;
		int l;
		float single;
		float single1;
		float single2;
		float single3;
		float single4;
		float single5;
		int num6 = (int)arraySize.x;
		int num7 = (int)arraySize.y;
		float[,] singleArray = new float[num6, num7];
		float[,] singleArray1 = new float[num6, num7];
		float[,] singleArray2 = new float[num6, num7];
		float[,] singleArray3 = new float[num6, num7];
		for (i = 0; i < num7; i++)
		{
			for (j = 0; j < num6; j++)
			{
				singleArray[j, i] = 0f;
				singleArray1[j, i] = 0f;
				singleArray2[j, i] = 0f;
				singleArray3[j, i] = 0f;
			}
		}
		for (int k = 0; k < iterations; k++)
		{
			for (i = 0; i < num7; i++)
			{
				for (j = 0; j < num6; j++)
				{
					float single6 = singleArray[j, i] + this.hydraulicRainfall;
					if (single6 > 1f)
					{
						single6 = 1f;
					}
					singleArray[j, i] = single6;
				}
			}
			for (i = 0; i < num7; i++)
			{
				for (j = 0; j < num6; j++)
				{
					float single7 = singleArray2[j, i];
					single3 = singleArray[j, i] * this.hydraulicSedimentSaturation;
					if (single7 < single3)
					{
						float single8 = singleArray[j, i] * this.hydraulicSedimentSolubility;
						if (single7 + single8 > single3)
						{
							single8 = single3 - single7;
						}
						single1 = heightMap[j, i];
						if (single8 > single1)
						{
							single8 = single1;
						}
						singleArray2[j, i] = single7 + single8;
						heightMap[j, i] = single1 - single8;
					}
				}
			}
			for (i = 0; i < num7; i++)
			{
				if (i == 0)
				{
					num1 = 2;
					num3 = 0;
					num5 = 0;
				}
				else if (i != num7 - 1)
				{
					num1 = 3;
					num3 = -1;
					num5 = 1;
				}
				else
				{
					num1 = 2;
					num3 = -1;
					num5 = 1;
				}
				for (j = 0; j < num6; j++)
				{
					if (j == 0)
					{
						num = 2;
						num2 = 0;
						num4 = 0;
					}
					else if (j != num6 - 1)
					{
						num = 3;
						num2 = -1;
						num4 = 1;
					}
					else
					{
						num = 2;
						num2 = -1;
						num4 = 1;
					}
					float single9 = 0f;
					float single10 = 0f;
					single1 = heightMap[j + num4 + num2, i + num5 + num3];
					float single11 = singleArray[j + num4 + num2, i + num5 + num3];
					float single12 = single1;
					int num8 = 0;
					for (l = 0; l < num1; l++)
					{
						for (m = 0; m < num; m++)
						{
							if ((m != num4 || l != num5) && (this.neighbourhood == TerrainToolkit.Neighbourhood.Moore || this.neighbourhood == TerrainToolkit.Neighbourhood.VonNeumann && (m == num4 || l == num5)))
							{
								single2 = heightMap[j + m + num2, i + l + num3];
								single5 = singleArray[j + m + num2, i + l + num3];
								single = single1 + single11 - (single2 + single5);
								if (single > 0f)
								{
									single9 = single9 + single;
									single12 = single12 + (single2 + single5);
									num8++;
									if (single > single10)
									{
										single = single10;
									}
								}
							}
						}
					}
					float single13 = single12 / (float)(num8 + 1);
					float single14 = single1 + single11 - single13;
					float single15 = Mathf.Min(single11, single14);
					float single16 = singleArray1[j + num4 + num2, i + num5 + num3];
					float single17 = single16 - single15;
					singleArray1[j + num4 + num2, i + num5 + num3] = single17;
					float single18 = singleArray2[j + num4 + num2, i + num5 + num3];
					float single19 = single18 * (single15 / single11);
					float single20 = singleArray3[j + num4 + num2, i + num5 + num3];
					float single21 = single20 - single19;
					singleArray3[j + num4 + num2, i + num5 + num3] = single21;
					for (l = 0; l < num1; l++)
					{
						for (m = 0; m < num; m++)
						{
							if ((m != num4 || l != num5) && (this.neighbourhood == TerrainToolkit.Neighbourhood.Moore || this.neighbourhood == TerrainToolkit.Neighbourhood.VonNeumann && (m == num4 || l == num5)))
							{
								single2 = heightMap[j + m + num2, i + l + num3];
								single5 = singleArray[j + m + num2, i + l + num3];
								single = single1 + single11 - (single2 + single5);
								if (single > 0f)
								{
									float single22 = singleArray1[j + m + num2, i + l + num3];
									float single23 = single22 + single15 * (single / single9);
									singleArray1[j + m + num2, i + l + num3] = single23;
									float single24 = singleArray3[j + m + num2, i + l + num3];
									float single25 = single24 + single19 * (single / single9);
									singleArray3[j + m + num2, i + l + num3] = single25;
								}
							}
						}
					}
				}
			}
			for (i = 0; i < num7; i++)
			{
				for (j = 0; j < num6; j++)
				{
					float single26 = singleArray[j, i] + singleArray1[j, i];
					single26 = single26 - single26 * this.hydraulicEvaporation;
					if (single26 < 0f)
					{
						single26 = 0f;
					}
					singleArray[j, i] = single26;
					singleArray1[j, i] = 0f;
				}
			}
			for (i = 0; i < num7; i++)
			{
				for (j = 0; j < num6; j++)
				{
					single4 = singleArray2[j, i] + singleArray3[j, i];
					if (single4 > 1f)
					{
						single4 = 1f;
					}
					else if (single4 < 0f)
					{
						single4 = 0f;
					}
					singleArray2[j, i] = single4;
					singleArray3[j, i] = 0f;
				}
			}
			for (i = 0; i < num7; i++)
			{
				for (j = 0; j < num6; j++)
				{
					single3 = singleArray[j, i] * this.hydraulicSedimentSaturation;
					single4 = singleArray2[j, i];
					if (single4 > single3)
					{
						float single27 = single4 - single3;
						singleArray2[j, i] = single3;
						float single28 = heightMap[j, i];
						heightMap[j, i] = single28 + single27;
					}
				}
			}
			float single29 = (float)k / (float)iterations;
			erosionProgressDelegate("Applying Hydraulic Erosion", "Applying hydraulic erosion.", k, iterations, single29);
		}
		return heightMap;
	}

	public void FullHydraulicErosion(int iterations, float rainfall, float evaporation, float solubility, float saturation)
	{
		this.erosionTypeInt = 1;
		this.erosionType = TerrainToolkit.ErosionType.Hydraulic;
		this.hydraulicTypeInt = 1;
		this.hydraulicType = TerrainToolkit.HydraulicType.Full;
		this.hydraulicIterations = iterations;
		this.hydraulicRainfall = rainfall;
		this.hydraulicEvaporation = evaporation;
		this.hydraulicSedimentSolubility = solubility;
		this.hydraulicSedimentSaturation = saturation;
		this.neighbourhood = TerrainToolkit.Neighbourhood.Moore;
		this.erodeAllTerrain(new TerrainToolkit.ErosionProgressDelegate(this.dummyErosionProgress));
	}

	private float[,] generateDiamondSquare(float[,] heightMap, Vector2 arraySize, TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate)
	{
		int num = (int)arraySize.x;
		int num1 = (int)arraySize.y;
		float single = 1f;
		int num2 = num - 1;
		heightMap[0, 0] = 0.5f;
		heightMap[num - 1, 0] = 0.5f;
		heightMap[0, num1 - 1] = 0.5f;
		heightMap[num - 1, num1 - 1] = 0.5f;
		generatorProgressDelegate("Fractal Generator", "Generating height map. Please wait.", 0f);
		while (num2 > 1)
		{
			for (int i = 0; i < num - 1; i = i + num2)
			{
				for (int j = 0; j < num1 - 1; j = j + num2)
				{
					int num3 = i + (num2 >> 1);
					int num4 = j + (num2 >> 1);
					Vector2[] vector2 = new Vector2[] { new Vector2((float)i, (float)j), new Vector2((float)(i + num2), (float)j), new Vector2((float)i, (float)(j + num2)), new Vector2((float)(i + num2), (float)(j + num2)) };
					this.dsCalculateHeight(heightMap, arraySize, num3, num4, vector2, single);
				}
			}
			for (int k = 0; k < num - 1; k = k + num2)
			{
				for (int l = 0; l < num1 - 1; l = l + num2)
				{
					int num5 = num2 >> 1;
					int num6 = k + num5;
					int num7 = l;
					int num8 = k;
					int num9 = l + num5;
					Vector2[] vector2Array = new Vector2[] { new Vector2((float)(num6 - num5), (float)num7), new Vector2((float)num6, (float)(num7 - num5)), new Vector2((float)(num6 + num5), (float)num7), new Vector2((float)num6, (float)(num7 + num5)) };
					Vector2[] vector21 = new Vector2[] { new Vector2((float)(num8 - num5), (float)num9), new Vector2((float)num8, (float)(num9 - num5)), new Vector2((float)(num8 + num5), (float)num9), new Vector2((float)num8, (float)(num9 + num5)) };
					this.dsCalculateHeight(heightMap, arraySize, num6, num7, vector2Array, single);
					this.dsCalculateHeight(heightMap, arraySize, num8, num9, vector21, single);
				}
			}
			single = single * this.diamondSquareDelta;
			num2 = num2 >> 1;
		}
		generatorProgressDelegate("Fractal Generator", "Generating height map. Please wait.", 1f);
		return heightMap;
	}

	private float[,] generatePerlin(float[,] heightMap, Vector2 arraySize, TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate)
	{
		int k;
		int num = (int)arraySize.x;
		int num1 = (int)arraySize.y;
		for (int i = 0; i < num1; i++)
		{
			for (int j = 0; j < num; j++)
			{
				heightMap[j, i] = 0f;
			}
		}
		TerrainToolkit.PerlinNoise2D[] perlinNoise2D = new TerrainToolkit.PerlinNoise2D[this.perlinOctaves];
		int num2 = this.perlinFrequency;
		float single = 1f;
		for (k = 0; k < this.perlinOctaves; k++)
		{
			perlinNoise2D[k] = new TerrainToolkit.PerlinNoise2D(num2, single);
			num2 = num2 * 2;
			single = single / 2f;
		}
		for (k = 0; k < this.perlinOctaves; k++)
		{
			double frequency = (double)((float)num / (float)perlinNoise2D[k].Frequency);
			double frequency1 = (double)((float)num1 / (float)perlinNoise2D[k].Frequency);
			for (int l = 0; l < num; l++)
			{
				for (int m = 0; m < num1; m++)
				{
					int num3 = (int)((double)l / frequency);
					int num4 = num3 + 1;
					int num5 = (int)((double)m / frequency1);
					int num6 = num5 + 1;
					double interpolatedPoint = perlinNoise2D[k].getInterpolatedPoint(num3, num4, num5, num6, (double)l / frequency - (double)num3, (double)m / frequency1 - (double)num5);
					heightMap[l, m] = heightMap[l, m] + (float)(interpolatedPoint * (double)perlinNoise2D[k].Amplitude);
				}
			}
			float single1 = (float)((k + 1) / this.perlinOctaves);
			generatorProgressDelegate("Perlin Generator", "Generating height map. Please wait.", single1);
		}
		TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate1 = new TerrainToolkit.GeneratorProgressDelegate(this.dummyGeneratorProgress);
		float single2 = this.normaliseMin;
		float single3 = this.normaliseMax;
		float single4 = this.normaliseBlend;
		this.normaliseMin = 0f;
		this.normaliseMax = 1f;
		this.normaliseBlend = 1f;
		heightMap = this.normalise(heightMap, arraySize, generatorProgressDelegate1);
		this.normaliseMin = single2;
		this.normaliseMax = single3;
		this.normaliseBlend = single4;
		for (int n = 0; n < num; n++)
		{
			for (int o = 0; o < num1; o++)
			{
				heightMap[n, o] = heightMap[n, o] * this.perlinAmplitude;
			}
		}
		for (k = 0; k < this.perlinOctaves; k++)
		{
			perlinNoise2D[k] = null;
		}
		perlinNoise2D = null;
		return heightMap;
	}

	public void generateTerrain(TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate)
	{
		this.convertIntVarsToEnums();
		Terrain component = (Terrain)base.GetComponent(typeof(Terrain));
		if (component == null)
		{
			return;
		}
		TerrainData terrainDatum = component.terrainData;
		int num = terrainDatum.heightmapWidth;
		int num1 = terrainDatum.heightmapHeight;
		float[,] heights = terrainDatum.GetHeights(0, 0, num, num1);
		float[,] singleArray = (float[,])heights.Clone();
		switch (this.generatorType)
		{
			case TerrainToolkit.GeneratorType.Voronoi:
			{
				singleArray = this.generateVoronoi(singleArray, new Vector2((float)num, (float)num1), generatorProgressDelegate);
				break;
			}
			case TerrainToolkit.GeneratorType.DiamondSquare:
			{
				singleArray = this.generateDiamondSquare(singleArray, new Vector2((float)num, (float)num1), generatorProgressDelegate);
				break;
			}
			case TerrainToolkit.GeneratorType.Perlin:
			{
				singleArray = this.generatePerlin(singleArray, new Vector2((float)num, (float)num1), generatorProgressDelegate);
				break;
			}
			case TerrainToolkit.GeneratorType.Smooth:
			{
				singleArray = this.smooth(singleArray, new Vector2((float)num, (float)num1), generatorProgressDelegate);
				break;
			}
			case TerrainToolkit.GeneratorType.Normalise:
			{
				singleArray = this.normalise(singleArray, new Vector2((float)num, (float)num1), generatorProgressDelegate);
				break;
			}
			default:
			{
				return;
			}
		}
		for (int i = 0; i < num1; i++)
		{
			for (int j = 0; j < num; j++)
			{
				float single = heights[j, i];
				float single1 = singleArray[j, i];
				float single2 = 0f;
				switch (this.generatorType)
				{
					case TerrainToolkit.GeneratorType.Voronoi:
					{
						single2 = single1 * this.voronoiBlend + single * (1f - this.voronoiBlend);
						break;
					}
					case TerrainToolkit.GeneratorType.DiamondSquare:
					{
						single2 = single1 * this.diamondSquareBlend + single * (1f - this.diamondSquareBlend);
						break;
					}
					case TerrainToolkit.GeneratorType.Perlin:
					{
						single2 = single1 * this.perlinBlend + single * (1f - this.perlinBlend);
						break;
					}
					case TerrainToolkit.GeneratorType.Smooth:
					{
						single2 = single1 * this.smoothBlend + single * (1f - this.smoothBlend);
						break;
					}
					case TerrainToolkit.GeneratorType.Normalise:
					{
						single2 = single1 * this.normaliseBlend + single * (1f - this.normaliseBlend);
						break;
					}
				}
				heights[j, i] = single2;
			}
		}
		terrainDatum.SetHeights(0, 0, heights);
	}

	private float[,] generateVoronoi(float[,] heightMap, Vector2 arraySize, TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate)
	{
		int i;
		int k;
		int j;
		float single;
		int num = (int)arraySize.x;
		int num1 = (int)arraySize.y;
		ArrayList arrayLists = new ArrayList();
		for (i = 0; i < this.voronoiCells; i++)
		{
			TerrainToolkit.Peak vector2 = new TerrainToolkit.Peak();
			int num2 = (int)Mathf.Floor(UnityEngine.Random.@value * (float)num);
			int num3 = (int)Mathf.Floor(UnityEngine.Random.@value * (float)num1);
			float single1 = UnityEngine.Random.@value;
			if (UnityEngine.Random.@value > this.voronoiFeatures)
			{
				single1 = 0f;
			}
			vector2.peakPoint = new Vector2((float)num2, (float)num3);
			vector2.peakHeight = single1;
			arrayLists.Add(vector2);
		}
		float single2 = 0f;
		for (j = 0; j < num1; j++)
		{
			for (k = 0; k < num; k++)
			{
				ArrayList arrayLists1 = new ArrayList();
				for (i = 0; i < this.voronoiCells; i++)
				{
					Vector2 item = ((TerrainToolkit.Peak)arrayLists[i]).peakPoint;
					float single3 = Vector2.Distance(item, new Vector2((float)k, (float)j));
					TerrainToolkit.PeakDistance peakDistance = new TerrainToolkit.PeakDistance()
					{
						id = i,
						dist = single3
					};
					arrayLists1.Add(peakDistance);
				}
				arrayLists1.Sort();
				TerrainToolkit.PeakDistance item1 = (TerrainToolkit.PeakDistance)arrayLists1[0];
				TerrainToolkit.PeakDistance peakDistance1 = (TerrainToolkit.PeakDistance)arrayLists1[1];
				int num4 = item1.id;
				float single4 = item1.dist;
				float single5 = peakDistance1.dist;
				float single6 = Mathf.Abs(single4 - single5) / ((float)(num + num1) / Mathf.Sqrt((float)this.voronoiCells));
				float item2 = (float)((TerrainToolkit.Peak)arrayLists[num4]).peakHeight;
				float single7 = item2 - Mathf.Abs(single4 / single5) * item2;
				switch (this.voronoiType)
				{
					case TerrainToolkit.VoronoiType.Sine:
					{
						single = single7 * 3.14159274f - 1.57079637f;
						single7 = 0.5f + Mathf.Sin(single) / 2f;
						break;
					}
					case TerrainToolkit.VoronoiType.Tangent:
					{
						single = single7 * 3.14159274f / 2f;
						single7 = 0.5f + Mathf.Tan(single) / 2f;
						break;
					}
				}
				single7 = single7 * single6 * this.voronoiScale + single7 * (1f - this.voronoiScale);
				if (single7 < 0f)
				{
					single7 = 0f;
				}
				else if (single7 > 1f)
				{
					single7 = 1f;
				}
				heightMap[k, j] = single7;
				if (single7 > single2)
				{
					single2 = single7;
				}
			}
			float single8 = (float)(j * num1);
			float single9 = (float)(num * num1);
			generatorProgressDelegate("Voronoi Generator", "Generating height map. Please wait.", single8 / single9);
		}
		for (j = 0; j < num1; j++)
		{
			for (k = 0; k < num; k++)
			{
				float single10 = heightMap[k, j] * (1f / single2);
				heightMap[k, j] = single10;
			}
		}
		return heightMap;
	}

	private float[,] normalise(float[,] heightMap, Vector2 arraySize, TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate)
	{
		int j;
		int i;
		int num = (int)arraySize.x;
		int num1 = (int)arraySize.y;
		float single = 0f;
		float single1 = 1f;
		generatorProgressDelegate("Normalise Filter", "Normalising height map. Please wait.", 0f);
		for (i = 0; i < num1; i++)
		{
			for (j = 0; j < num; j++)
			{
				float single2 = heightMap[j, i];
				if (single2 < single1)
				{
					single1 = single2;
				}
				else if (single2 > single)
				{
					single = single2;
				}
			}
		}
		generatorProgressDelegate("Normalise Filter", "Normalising height map. Please wait.", 0.5f);
		float single3 = single - single1;
		float single4 = this.normaliseMax - this.normaliseMin;
		for (i = 0; i < num1; i++)
		{
			for (j = 0; j < num; j++)
			{
				float single5 = (heightMap[j, i] - single1) / single3 * single4;
				heightMap[j, i] = this.normaliseMin + single5;
			}
		}
		generatorProgressDelegate("Normalise Filter", "Normalising height map. Please wait.", 1f);
		return heightMap;
	}

	public void NormaliseTerrain(float minHeight, float maxHeight, float blend)
	{
		this.generatorTypeInt = 4;
		this.generatorType = TerrainToolkit.GeneratorType.Normalise;
		this.normaliseMin = minHeight;
		this.normaliseMax = maxHeight;
		this.normaliseBlend = blend;
		this.generateTerrain(new TerrainToolkit.GeneratorProgressDelegate(this.dummyGeneratorProgress));
	}

	public void NormalizeTerrain(float minHeight, float maxHeight, float blend)
	{
		this.NormaliseTerrain(minHeight, maxHeight, blend);
	}

	public void OnDrawGizmos()
	{
		Terrain component = (Terrain)base.GetComponent(typeof(Terrain));
		if (component == null)
		{
			return;
		}
		if (this.isBrushOn && !this.isBrushHidden)
		{
			if (!this.isBrushPainting)
			{
				Gizmos.color = Color.white;
			}
			else
			{
				Gizmos.color = Color.red;
			}
			float single = this.brushSize / 4f;
			Gizmos.DrawLine(this.brushPosition + new Vector3(-single, 0f, 0f), this.brushPosition + new Vector3(single, 0f, 0f));
			Gizmos.DrawLine(this.brushPosition + new Vector3(0f, -single, 0f), this.brushPosition + new Vector3(0f, single, 0f));
			Gizmos.DrawLine(this.brushPosition + new Vector3(0f, 0f, -single), this.brushPosition + new Vector3(0f, 0f, single));
			Gizmos.DrawWireCube(this.brushPosition, new Vector3(this.brushSize, 0f, this.brushSize));
			Gizmos.DrawWireSphere(this.brushPosition, this.brushSize / 2f);
		}
		Vector3 vector3 = component.terrainData.size;
		if (this.toolModeInt == 1 && this.erosionTypeInt == 2)
		{
			Gizmos.color = Color.blue;
			Vector3 vector31 = base.transform.position;
			float single1 = vector31.x + vector3.x / 2f;
			float single2 = this.tidalSeaLevel;
			Vector3 vector32 = base.transform.position;
			Gizmos.DrawWireCube(new Vector3(single1, single2, vector32.z + vector3.z / 2f), new Vector3(vector3.x, 0f, vector3.z));
			Gizmos.color = Color.white;
			Vector3 vector33 = base.transform.position;
			float single3 = vector33.x + vector3.x / 2f;
			float single4 = this.tidalSeaLevel;
			Vector3 vector34 = base.transform.position;
			Gizmos.DrawWireCube(new Vector3(single3, single4, vector34.z + vector3.z / 2f), new Vector3(vector3.x, this.tidalRangeAmount * 2f, vector3.z));
		}
		if (this.toolModeInt == 1 && this.erosionTypeInt == 3)
		{
			Gizmos.color = Color.blue;
			Quaternion quaternion = Quaternion.Euler(0f, this.windDirection, 0f);
			Vector3 vector35 = quaternion * Vector3.forward;
			Vector3 vector36 = base.transform.position;
			float single5 = vector36.x + vector3.x / 2f;
			float single6 = base.transform.position.y + vector3.y;
			Vector3 vector37 = base.transform.position;
			Vector3 vector38 = new Vector3(single5, single6, vector37.z + vector3.z / 2f);
			Vector3 vector39 = vector38 + (vector35 * (vector3.x / 4f));
			Vector3 vector310 = vector38 + (vector35 * (vector3.x / 6f));
			Gizmos.DrawLine(vector38, vector39);
			Gizmos.DrawLine(vector39, vector310 + new Vector3(0f, vector3.x / 16f, 0f));
			Gizmos.DrawLine(vector39, vector310 - new Vector3(0f, vector3.x / 16f, 0f));
		}
	}

	public void paint()
	{
		this.convertIntVarsToEnums();
		this.erodeTerrainWithBrush();
	}

	public void PerlinGenerator(int frequency, float amplitude, int octaves, float blend)
	{
		this.generatorTypeInt = 2;
		this.generatorType = TerrainToolkit.GeneratorType.Perlin;
		this.perlinFrequency = frequency;
		this.perlinAmplitude = amplitude;
		this.perlinOctaves = octaves;
		this.perlinBlend = blend;
		this.generateTerrain(new TerrainToolkit.GeneratorProgressDelegate(this.dummyGeneratorProgress));
	}

	public void setFastHydraulicErosionPreset(TerrainToolkit.fastHydraulicErosionPresetData preset)
	{
		this.erosionTypeInt = 1;
		this.erosionType = TerrainToolkit.ErosionType.Hydraulic;
		this.hydraulicTypeInt = 0;
		this.hydraulicType = TerrainToolkit.HydraulicType.Fast;
		this.hydraulicIterations = preset.hydraulicIterations;
		this.hydraulicMaxSlope = preset.hydraulicMaxSlope;
		this.hydraulicFalloff = preset.hydraulicFalloff;
	}

	public void setFractalPreset(TerrainToolkit.fractalPresetData preset)
	{
		this.generatorTypeInt = 1;
		this.generatorType = TerrainToolkit.GeneratorType.DiamondSquare;
		this.diamondSquareDelta = preset.diamondSquareDelta;
		this.diamondSquareBlend = preset.diamondSquareBlend;
	}

	public void setFullHydraulicErosionPreset(TerrainToolkit.fullHydraulicErosionPresetData preset)
	{
		this.erosionTypeInt = 1;
		this.erosionType = TerrainToolkit.ErosionType.Hydraulic;
		this.hydraulicTypeInt = 1;
		this.hydraulicType = TerrainToolkit.HydraulicType.Full;
		this.hydraulicIterations = preset.hydraulicIterations;
		this.hydraulicRainfall = preset.hydraulicRainfall;
		this.hydraulicEvaporation = preset.hydraulicEvaporation;
		this.hydraulicSedimentSolubility = preset.hydraulicSedimentSolubility;
		this.hydraulicSedimentSaturation = preset.hydraulicSedimentSaturation;
	}

	public void setPerlinPreset(TerrainToolkit.perlinPresetData preset)
	{
		this.generatorTypeInt = 2;
		this.generatorType = TerrainToolkit.GeneratorType.Perlin;
		this.perlinFrequency = preset.perlinFrequency;
		this.perlinAmplitude = preset.perlinAmplitude;
		this.perlinOctaves = preset.perlinOctaves;
		this.perlinBlend = preset.perlinBlend;
	}

	public void setThermalErosionPreset(TerrainToolkit.thermalErosionPresetData preset)
	{
		this.erosionTypeInt = 0;
		this.erosionType = TerrainToolkit.ErosionType.Thermal;
		this.thermalIterations = preset.thermalIterations;
		this.thermalMinSlope = preset.thermalMinSlope;
		this.thermalFalloff = preset.thermalFalloff;
	}

	public void setTidalErosionPreset(TerrainToolkit.tidalErosionPresetData preset)
	{
		this.erosionTypeInt = 2;
		this.erosionType = TerrainToolkit.ErosionType.Tidal;
		this.tidalIterations = preset.tidalIterations;
		this.tidalRangeAmount = preset.tidalRangeAmount;
		this.tidalCliffLimit = preset.tidalCliffLimit;
	}

	public void setVelocityHydraulicErosionPreset(TerrainToolkit.velocityHydraulicErosionPresetData preset)
	{
		this.erosionTypeInt = 1;
		this.erosionType = TerrainToolkit.ErosionType.Hydraulic;
		this.hydraulicTypeInt = 2;
		this.hydraulicType = TerrainToolkit.HydraulicType.Velocity;
		this.hydraulicIterations = preset.hydraulicIterations;
		this.hydraulicVelocityRainfall = preset.hydraulicVelocityRainfall;
		this.hydraulicVelocityEvaporation = preset.hydraulicVelocityEvaporation;
		this.hydraulicVelocitySedimentSolubility = preset.hydraulicVelocitySedimentSolubility;
		this.hydraulicVelocitySedimentSaturation = preset.hydraulicVelocitySedimentSaturation;
		this.hydraulicVelocity = preset.hydraulicVelocity;
		this.hydraulicMomentum = preset.hydraulicMomentum;
		this.hydraulicEntropy = preset.hydraulicEntropy;
		this.hydraulicDowncutting = preset.hydraulicDowncutting;
	}

	public void setVoronoiPreset(TerrainToolkit.voronoiPresetData preset)
	{
		this.generatorTypeInt = 0;
		this.generatorType = TerrainToolkit.GeneratorType.Voronoi;
		this.voronoiTypeInt = (int)preset.voronoiType;
		this.voronoiType = preset.voronoiType;
		this.voronoiCells = preset.voronoiCells;
		this.voronoiFeatures = preset.voronoiFeatures;
		this.voronoiScale = preset.voronoiScale;
		this.voronoiBlend = preset.voronoiBlend;
	}

	public void setWindErosionPreset(TerrainToolkit.windErosionPresetData preset)
	{
		this.erosionTypeInt = 3;
		this.erosionType = TerrainToolkit.ErosionType.Wind;
		this.windIterations = preset.windIterations;
		this.windDirection = preset.windDirection;
		this.windForce = preset.windForce;
		this.windLift = preset.windLift;
		this.windGravity = preset.windGravity;
		this.windCapacity = preset.windCapacity;
		this.windEntropy = preset.windEntropy;
		this.windSmoothing = preset.windSmoothing;
	}

	private float[,] smooth(float[,] heightMap, Vector2 arraySize, TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate)
	{
		int num;
		int num1;
		int num2;
		int num3;
		int num4;
		int num5;
		int num6 = (int)arraySize.x;
		int num7 = (int)arraySize.y;
		for (int i = 0; i < this.smoothIterations; i++)
		{
			for (int j = 0; j < num7; j++)
			{
				if (j == 0)
				{
					num1 = 2;
					num3 = 0;
					num5 = 0;
				}
				else if (j != num7 - 1)
				{
					num1 = 3;
					num3 = -1;
					num5 = 1;
				}
				else
				{
					num1 = 2;
					num3 = -1;
					num5 = 1;
				}
				for (int k = 0; k < num6; k++)
				{
					if (k == 0)
					{
						num = 2;
						num2 = 0;
						num4 = 0;
					}
					else if (k != num6 - 1)
					{
						num = 3;
						num2 = -1;
						num4 = 1;
					}
					else
					{
						num = 2;
						num2 = -1;
						num4 = 1;
					}
					float single = 0f;
					int num8 = 0;
					for (int l = 0; l < num1; l++)
					{
						for (int m = 0; m < num; m++)
						{
							if (this.neighbourhood == TerrainToolkit.Neighbourhood.Moore || this.neighbourhood == TerrainToolkit.Neighbourhood.VonNeumann && (m == num4 || l == num5))
							{
								float single1 = heightMap[k + m + num2, j + l + num3];
								single = single + single1;
								num8++;
							}
						}
					}
					float single2 = single / (float)num8;
					heightMap[k + num4 + num2, j + num5 + num3] = single2;
				}
			}
			float single3 = (float)((i + 1) / this.smoothIterations);
			generatorProgressDelegate("Smoothing Filter", "Smoothing height map. Please wait.", single3);
		}
		return heightMap;
	}

	public void SmoothTerrain(int iterations, float blend)
	{
		this.generatorTypeInt = 3;
		this.generatorType = TerrainToolkit.GeneratorType.Smooth;
		this.smoothIterations = iterations;
		this.smoothBlend = blend;
		this.generateTerrain(new TerrainToolkit.GeneratorProgressDelegate(this.dummyGeneratorProgress));
	}

	public void textureTerrain(TerrainToolkit.TextureProgressDelegate textureProgressDelegate)
	{
		int num;
		int num1;
		int num2;
		int num3;
		int num4;
		int num5;
		int n;
		int m;
		Terrain component = (Terrain)base.GetComponent(typeof(Terrain));
		if (component == null)
		{
			return;
		}
		TerrainData terrainDatum = component.terrainData;
		this.splatPrototypes = terrainDatum.splatPrototypes;
		int length = (int)this.splatPrototypes.Length;
		if (length < 2)
		{
			Debug.LogError("Error: You must assign at least 2 textures.");
			return;
		}
		textureProgressDelegate("Procedural Terrain Texture", "Generating height and slope maps. Please wait.", 0.1f);
		int num6 = terrainDatum.heightmapWidth - 1;
		int num7 = terrainDatum.heightmapHeight - 1;
		float[,] singleArray = new float[num6, num7];
		float[,] singleArray1 = new float[num6, num7];
		terrainDatum.alphamapResolution = num6;
		float[,,] alphamaps = terrainDatum.GetAlphamaps(0, 0, num6, num6);
		Vector3 vector3 = terrainDatum.size;
		float single = vector3.x / (float)num6 * Mathf.Tan(this.slopeBlendMinAngle * 0.0174532924f) / vector3.y;
		float single1 = vector3.x / (float)num6 * Mathf.Tan(this.slopeBlendMaxAngle * 0.0174532924f) / vector3.y;
		try
		{
			float single2 = 0f;
			float[,] heights = terrainDatum.GetHeights(0, 0, num6, num7);
			for (int i = 0; i < num7; i++)
			{
				if (i == 0)
				{
					num1 = 2;
					num3 = 0;
					num5 = 0;
				}
				else if (i != num7 - 1)
				{
					num1 = 3;
					num3 = -1;
					num5 = 1;
				}
				else
				{
					num1 = 2;
					num3 = -1;
					num5 = 1;
				}
				for (int j = 0; j < num6; j++)
				{
					if (j == 0)
					{
						num = 2;
						num2 = 0;
						num4 = 0;
					}
					else if (j != num6 - 1)
					{
						num = 3;
						num2 = -1;
						num4 = 1;
					}
					else
					{
						num = 2;
						num2 = -1;
						num4 = 1;
					}
					float single3 = heights[j + num4 + num2, i + num5 + num3];
					if (single3 > single2)
					{
						single2 = single3;
					}
					singleArray[j, i] = single3;
					float single4 = 0f;
					float single5 = (float)(num * num1 - 1);
					for (int k = 0; k < num1; k++)
					{
						for (int l = 0; l < num; l++)
						{
							if (l != num4 || k != num5)
							{
								float single6 = Mathf.Abs(single3 - heights[j + l + num2, i + k + num3]);
								single4 = single4 + single6;
							}
						}
					}
					singleArray1[j, i] = single4 / single5;
				}
			}
			for (m = 0; m < num7; m++)
			{
				for (n = 0; n < num6; n++)
				{
					float single7 = singleArray1[n, m];
					if (single7 < single)
					{
						single7 = 0f;
					}
					else if (single7 < single1)
					{
						single7 = (single7 - single) / (single1 - single);
					}
					else if (single7 > single1)
					{
						single7 = 1f;
					}
					singleArray1[n, m] = single7;
					alphamaps[n, m, 0] = single7;
				}
			}
			for (int o = 1; o < length; o++)
			{
				for (m = 0; m < num7; m++)
				{
					for (n = 0; n < num6; n++)
					{
						float item = 0f;
						float item1 = 0f;
						float item2 = 1f;
						float item3 = 1f;
						float single8 = 0f;
						if (o > 1)
						{
							item = (float)this.heightBlendPoints[o * 2 - 4];
							item1 = (float)this.heightBlendPoints[o * 2 - 3];
						}
						if (o < length - 1)
						{
							item2 = (float)this.heightBlendPoints[o * 2 - 2];
							item3 = (float)this.heightBlendPoints[o * 2 - 1];
						}
						float single9 = singleArray[n, m];
						if (single9 >= item1 && single9 <= item2)
						{
							single8 = 1f;
						}
						else if (single9 >= item && single9 < item1)
						{
							single8 = (single9 - item) / (item1 - item);
						}
						else if (single9 > item2 && single9 <= item3)
						{
							single8 = 1f - (single9 - item2) / (item3 - item2);
						}
						single8 = single8 - singleArray1[n, m];
						if (single8 < 0f)
						{
							single8 = 0f;
						}
						alphamaps[n, m, o] = single8;
					}
				}
			}
			textureProgressDelegate("Procedural Terrain Texture", "Generating splat map. Please wait.", 0.9f);
			terrainDatum.SetAlphamaps(0, 0, alphamaps);
			singleArray = null;
			singleArray1 = null;
			alphamaps = null;
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			singleArray = null;
			singleArray1 = null;
			alphamaps = null;
			Debug.LogError(string.Concat("An error occurred: ", exception));
		}
	}

	public void TextureTerrain(float[] slopeStops, float[] heightStops, Texture2D[] textures)
	{
		if ((int)slopeStops.Length != 2)
		{
			Debug.LogError("Error: slopeStops must have 2 values");
			return;
		}
		if ((int)heightStops.Length > 8)
		{
			Debug.LogError("Error: heightStops must have no more than 8 values");
			return;
		}
		if ((int)heightStops.Length % 2 != 0)
		{
			Debug.LogError("Error: heightStops must have an even number of values");
			return;
		}
		if ((int)textures.Length != (int)heightStops.Length / 2 + 2)
		{
			Debug.LogError("Error: heightStops contains an incorrect number of values");
			return;
		}
		float[] singleArray = slopeStops;
		for (int i = 0; i < (int)singleArray.Length; i++)
		{
			float single = (float)singleArray[i];
			if (single < 0f || single > 90f)
			{
				Debug.LogError("Error: The value of all slopeStops must be in the range 0.0 to 90.0");
				return;
			}
		}
		float[] singleArray1 = heightStops;
		for (int j = 0; j < (int)singleArray1.Length; j++)
		{
			float single1 = (float)singleArray1[j];
			if (single1 < 0f || single1 > 1f)
			{
				Debug.LogError("Error: The value of all heightStops must be in the range 0.0 to 1.0");
				return;
			}
		}
		TerrainData component = ((Terrain)base.GetComponent(typeof(Terrain))).terrainData;
		this.splatPrototypes = component.splatPrototypes;
		this.deleteAllSplatPrototypes();
		int num = 0;
		Texture2D[] texture2DArray = textures;
		for (int k = 0; k < (int)texture2DArray.Length; k++)
		{
			this.addSplatPrototype(texture2DArray[k], num);
			num++;
		}
		this.slopeBlendMinAngle = slopeStops[0];
		this.slopeBlendMaxAngle = slopeStops[1];
		num = 0;
		float[] singleArray2 = heightStops;
		for (int l = 0; l < (int)singleArray2.Length; l++)
		{
			float single2 = (float)singleArray2[l];
			this.heightBlendPoints[num] = single2;
			num++;
		}
		component.splatPrototypes = this.splatPrototypes;
		this.textureTerrain(new TerrainToolkit.TextureProgressDelegate(this.dummyTextureProgress));
	}

	public void TidalErosion(int iterations, float seaLevel, float tidalRange, float cliffLimit)
	{
		this.erosionTypeInt = 2;
		this.erosionType = TerrainToolkit.ErosionType.Tidal;
		this.tidalIterations = iterations;
		this.tidalSeaLevel = seaLevel;
		this.tidalRangeAmount = tidalRange;
		this.tidalCliffLimit = cliffLimit;
		this.neighbourhood = TerrainToolkit.Neighbourhood.Moore;
		this.erodeAllTerrain(new TerrainToolkit.ErosionProgressDelegate(this.dummyErosionProgress));
	}

	public void Update()
	{
		if (this.isBrushOn && (this.toolModeInt != 1 || this.erosionTypeInt > 2 || this.erosionTypeInt == 1 && this.hydraulicTypeInt > 0))
		{
			this.isBrushOn = false;
		}
	}

	private float[,] velocityHydraulicErosion(float[,] heightMap, Vector2 arraySize, int iterations, TerrainToolkit.ErosionProgressDelegate erosionProgressDelegate)
	{
		int j;
		int i;
		int num;
		int num1;
		int num2;
		int num3;
		int num4;
		int num5;
		float single;
		float single1;
		int l;
		int k;
		float single2;
		float single3;
		float single4;
		int num6;
		float single5;
		float single6;
		float single7;
		float single8;
		int num7 = (int)arraySize.x;
		int num8 = (int)arraySize.y;
		float[,] singleArray = new float[num7, num8];
		float[,] singleArray1 = new float[num7, num8];
		float[,] singleArray2 = new float[num7, num8];
		float[,] singleArray3 = new float[num7, num8];
		float[,] singleArray4 = new float[num7, num8];
		float[,] singleArray5 = new float[num7, num8];
		float[,] singleArray6 = new float[num7, num8];
		float[,] singleArray7 = new float[num7, num8];
		for (i = 0; i < num8; i++)
		{
			for (j = 0; j < num7; j++)
			{
				singleArray2[j, i] = 0f;
				singleArray3[j, i] = 0f;
				singleArray4[j, i] = 0f;
				singleArray5[j, i] = 0f;
				singleArray6[j, i] = 0f;
				singleArray7[j, i] = 0f;
			}
		}
		for (i = 0; i < num8; i++)
		{
			for (j = 0; j < num7; j++)
			{
				singleArray[j, i] = heightMap[j, i];
			}
		}
		for (i = 0; i < num8; i++)
		{
			if (i == 0)
			{
				num1 = 2;
				num3 = 0;
				num5 = 0;
			}
			else if (i != num8 - 1)
			{
				num1 = 3;
				num3 = -1;
				num5 = 1;
			}
			else
			{
				num1 = 2;
				num3 = -1;
				num5 = 1;
			}
			for (j = 0; j < num7; j++)
			{
				if (j == 0)
				{
					num = 2;
					num2 = 0;
					num4 = 0;
				}
				else if (j != num7 - 1)
				{
					num = 3;
					num2 = -1;
					num4 = 1;
				}
				else
				{
					num = 2;
					num2 = -1;
					num4 = 1;
				}
				single1 = 0f;
				single3 = heightMap[j + num4 + num2, i + num5 + num3];
				num6 = 0;
				for (k = 0; k < num1; k++)
				{
					for (l = 0; l < num; l++)
					{
						if ((l != num4 || k != num5) && (this.neighbourhood == TerrainToolkit.Neighbourhood.Moore || this.neighbourhood == TerrainToolkit.Neighbourhood.VonNeumann && (l == num4 || k == num5)))
						{
							single4 = heightMap[j + l + num2, i + k + num3];
							single2 = Mathf.Abs(single3 - single4);
							single1 = single1 + single2;
							num6++;
						}
					}
				}
				float single9 = single1 / (float)num6;
				singleArray1[j + num4 + num2, i + num5 + num3] = single9;
			}
		}
		for (int m = 0; m < iterations; m++)
		{
			for (i = 0; i < num8; i++)
			{
				for (j = 0; j < num7; j++)
				{
					float single10 = singleArray2[j, i] + singleArray[j, i] * this.hydraulicVelocityRainfall;
					if (single10 > 1f)
					{
						single10 = 1f;
					}
					singleArray2[j, i] = single10;
				}
			}
			for (i = 0; i < num8; i++)
			{
				for (j = 0; j < num7; j++)
				{
					float single11 = singleArray6[j, i];
					single = singleArray2[j, i] * this.hydraulicVelocitySedimentSaturation;
					if (single11 < single)
					{
						float single12 = singleArray2[j, i] * singleArray4[j, i] * this.hydraulicVelocitySedimentSolubility;
						if (single11 + single12 > single)
						{
							single12 = single - single11;
						}
						single3 = heightMap[j, i];
						if (single12 > single3)
						{
							single12 = single3;
						}
						singleArray6[j, i] = single11 + single12;
						heightMap[j, i] = single3 - single12;
					}
				}
			}
			for (i = 0; i < num8; i++)
			{
				if (i == 0)
				{
					num1 = 2;
					num3 = 0;
					num5 = 0;
				}
				else if (i != num8 - 1)
				{
					num1 = 3;
					num3 = -1;
					num5 = 1;
				}
				else
				{
					num1 = 2;
					num3 = -1;
					num5 = 1;
				}
				for (j = 0; j < num7; j++)
				{
					if (j == 0)
					{
						num = 2;
						num2 = 0;
						num4 = 0;
					}
					else if (j != num7 - 1)
					{
						num = 3;
						num2 = -1;
						num4 = 1;
					}
					else
					{
						num = 2;
						num2 = -1;
						num4 = 1;
					}
					single1 = 0f;
					single3 = heightMap[j, i];
					float single13 = single3;
					float single14 = singleArray2[j, i];
					num6 = 0;
					for (k = 0; k < num1; k++)
					{
						for (l = 0; l < num; l++)
						{
							if ((l != num4 || k != num5) && (this.neighbourhood == TerrainToolkit.Neighbourhood.Moore || this.neighbourhood == TerrainToolkit.Neighbourhood.VonNeumann && (l == num4 || k == num5)))
							{
								single4 = heightMap[j + l + num2, i + k + num3];
								single5 = singleArray2[j + l + num2, i + k + num3];
								single2 = single3 + single14 - (single4 + single5);
								if (single2 > 0f)
								{
									single1 = single1 + single2;
									single13 = single13 + (single3 + single14);
									num6++;
								}
							}
						}
					}
					float single15 = singleArray4[j, i];
					float single16 = singleArray1[j, i];
					float single17 = singleArray6[j, i];
					float single18 = single15 + this.hydraulicVelocity * single16;
					float single19 = single13 / (float)(num6 + 1);
					float single20 = single3 + single14 - single19;
					float single21 = Mathf.Min(single14, single20 * (1f + single15));
					float single22 = singleArray3[j, i];
					singleArray3[j, i] = single22 - single21;
					float single23 = single18 * (single21 / single14);
					float single24 = single17 * (single21 / single14);
					for (k = 0; k < num1; k++)
					{
						for (l = 0; l < num; l++)
						{
							if ((l != num4 || k != num5) && (this.neighbourhood == TerrainToolkit.Neighbourhood.Moore || this.neighbourhood == TerrainToolkit.Neighbourhood.VonNeumann && (l == num4 || k == num5)))
							{
								single4 = heightMap[j + l + num2, i + k + num3];
								single5 = singleArray2[j + l + num2, i + k + num3];
								single2 = single3 + single14 - (single4 + single5);
								if (single2 > 0f)
								{
									float single25 = singleArray3[j + l + num2, i + k + num3];
									float single26 = single25 + single21 * (single2 / single1);
									singleArray3[j + l + num2, i + k + num3] = single26;
									float single27 = singleArray5[j + l + num2, i + k + num3];
									float single28 = single23 * this.hydraulicMomentum * (single2 / single1);
									float single29 = single27 + single28;
									singleArray5[j + l + num2, i + k + num3] = single29;
									float single30 = singleArray7[j + l + num2, i + k + num3];
									float single31 = single24 * this.hydraulicMomentum * (single2 / single1);
									float single32 = single30 + single31;
									singleArray7[j + l + num2, i + k + num3] = single32;
								}
							}
						}
					}
					float single33 = singleArray5[j, i];
					singleArray5[j, i] = single33 - single23;
				}
			}
			for (i = 0; i < num8; i++)
			{
				for (j = 0; j < num7; j++)
				{
					single6 = singleArray4[j, i] + singleArray5[j, i];
					single6 = single6 * (1f - this.hydraulicEntropy);
					if (single6 > 1f)
					{
						single6 = 1f;
					}
					else if (single6 < 0f)
					{
						single6 = 0f;
					}
					singleArray4[j, i] = single6;
					singleArray5[j, i] = 0f;
				}
			}
			for (i = 0; i < num8; i++)
			{
				for (j = 0; j < num7; j++)
				{
					float single34 = singleArray2[j, i] + singleArray3[j, i];
					single34 = single34 - single34 * this.hydraulicVelocityEvaporation;
					if (single34 > 1f)
					{
						single34 = 1f;
					}
					else if (single34 < 0f)
					{
						single34 = 0f;
					}
					singleArray2[j, i] = single34;
					singleArray3[j, i] = 0f;
				}
			}
			for (i = 0; i < num8; i++)
			{
				for (j = 0; j < num7; j++)
				{
					single7 = singleArray6[j, i] + singleArray7[j, i];
					if (single7 > 1f)
					{
						single7 = 1f;
					}
					else if (single7 < 0f)
					{
						single7 = 0f;
					}
					singleArray6[j, i] = single7;
					singleArray7[j, i] = 0f;
				}
			}
			for (i = 0; i < num8; i++)
			{
				for (j = 0; j < num7; j++)
				{
					single = singleArray2[j, i] * this.hydraulicVelocitySedimentSaturation;
					single7 = singleArray6[j, i];
					if (single7 > single)
					{
						float single35 = single7 - single;
						singleArray6[j, i] = single;
						single8 = heightMap[j, i];
						heightMap[j, i] = single8 + single35;
					}
				}
			}
			for (i = 0; i < num8; i++)
			{
				for (j = 0; j < num7; j++)
				{
					single6 = singleArray2[j, i];
					single8 = heightMap[j, i];
					float single36 = 1f - Mathf.Abs(0.5f - single8) * 2f;
					single8 = single8 - this.hydraulicDowncutting * single6 * single36;
					heightMap[j, i] = single8;
				}
			}
			float single37 = (float)m / (float)iterations;
			erosionProgressDelegate("Applying Hydraulic Erosion", "Applying hydraulic erosion.", m, iterations, single37);
		}
		return heightMap;
	}

	public void VelocityHydraulicErosion(int iterations, float rainfall, float evaporation, float solubility, float saturation, float velocity, float momentum, float entropy, float downcutting)
	{
		this.erosionTypeInt = 1;
		this.erosionType = TerrainToolkit.ErosionType.Hydraulic;
		this.hydraulicTypeInt = 2;
		this.hydraulicType = TerrainToolkit.HydraulicType.Velocity;
		this.hydraulicIterations = iterations;
		this.hydraulicVelocityRainfall = rainfall;
		this.hydraulicVelocityEvaporation = evaporation;
		this.hydraulicVelocitySedimentSolubility = solubility;
		this.hydraulicVelocitySedimentSaturation = saturation;
		this.hydraulicVelocity = velocity;
		this.hydraulicMomentum = momentum;
		this.hydraulicEntropy = entropy;
		this.hydraulicDowncutting = downcutting;
		this.neighbourhood = TerrainToolkit.Neighbourhood.Moore;
		this.erodeAllTerrain(new TerrainToolkit.ErosionProgressDelegate(this.dummyErosionProgress));
	}

	public void VoronoiGenerator(TerrainToolkit.FeatureType featureType, int cells, float features, float scale, float blend)
	{
		this.generatorTypeInt = 0;
		this.generatorType = TerrainToolkit.GeneratorType.Voronoi;
		switch (featureType)
		{
			case TerrainToolkit.FeatureType.Mountains:
			{
				this.voronoiTypeInt = 0;
				this.voronoiType = TerrainToolkit.VoronoiType.Linear;
				break;
			}
			case TerrainToolkit.FeatureType.Hills:
			{
				this.voronoiTypeInt = 1;
				this.voronoiType = TerrainToolkit.VoronoiType.Sine;
				break;
			}
			case TerrainToolkit.FeatureType.Plateaus:
			{
				this.voronoiTypeInt = 2;
				this.voronoiType = TerrainToolkit.VoronoiType.Tangent;
				break;
			}
		}
		this.voronoiCells = cells;
		this.voronoiFeatures = features;
		this.voronoiScale = scale;
		this.voronoiBlend = blend;
		this.generateTerrain(new TerrainToolkit.GeneratorProgressDelegate(this.dummyGeneratorProgress));
	}

	private float[,] windErosion(float[,] heightMap, Vector2 arraySize, int iterations, TerrainToolkit.ErosionProgressDelegate erosionProgressDelegate)
	{
		int num;
		int num1;
		int num2;
		int num3;
		int num4;
		int num5;
		int j;
		int i;
		float single;
		float single1;
		TerrainData component = ((Terrain)base.GetComponent(typeof(Terrain))).terrainData;
		Quaternion quaternion = Quaternion.Euler(0f, this.windDirection + 180f, 0f);
		Vector3 vector3 = quaternion * Vector3.forward;
		int num6 = (int)arraySize.x;
		int num7 = (int)arraySize.y;
		float[,] singleArray = new float[num6, num7];
		float[,] singleArray1 = new float[num6, num7];
		float[,] singleArray2 = new float[num6, num7];
		float[,] singleArray3 = new float[num6, num7];
		float[,] singleArray4 = new float[num6, num7];
		float[,] singleArray5 = new float[num6, num7];
		float[,] singleArray6 = new float[num6, num7];
		for (i = 0; i < num7; i++)
		{
			for (j = 0; j < num6; j++)
			{
				singleArray[j, i] = 0f;
				singleArray1[j, i] = 0f;
				singleArray2[j, i] = 0f;
				singleArray3[j, i] = 0f;
				singleArray4[j, i] = 0f;
				singleArray5[j, i] = 0f;
				singleArray6[j, i] = 0f;
			}
		}
		for (int k = 0; k < iterations; k++)
		{
			for (i = 0; i < num7; i++)
			{
				for (j = 0; j < num6; j++)
				{
					single1 = singleArray2[j, i];
					float single2 = heightMap[j, i];
					float single3 = singleArray4[j, i];
					float single4 = single3 * this.windGravity;
					singleArray4[j, i] = single3 - single4;
					heightMap[j, i] = single2 + single4;
				}
			}
			for (i = 0; i < num7; i++)
			{
				for (j = 0; j < num6; j++)
				{
					float single5 = heightMap[j, i];
					Vector3 interpolatedNormal = component.GetInterpolatedNormal((float)j / (float)num6, (float)i / (float)num7);
					float single6 = (Vector3.Angle(interpolatedNormal, vector3) - 90f) / 90f;
					if (single6 < 0f)
					{
						single6 = 0f;
					}
					singleArray[j, i] = single6 * single5;
					float single7 = 1f - Mathf.Abs(Vector3.Angle(interpolatedNormal, vector3) - 90f) / 90f;
					singleArray1[j, i] = single7 * single5;
					float single8 = single7 * single5 * this.windForce;
					float single9 = singleArray2[j, i] + single8;
					singleArray2[j, i] = single9;
					single = singleArray4[j, i];
					float single10 = this.windLift * single9;
					if (single + single10 > this.windCapacity)
					{
						single10 = this.windCapacity - single;
					}
					singleArray4[j, i] = single + single10;
					heightMap[j, i] = single5 - single10;
				}
			}
			for (i = 0; i < num7; i++)
			{
				if (i == 0)
				{
					num1 = 2;
					num3 = 0;
					num5 = 0;
				}
				else if (i != num7 - 1)
				{
					num1 = 3;
					num3 = -1;
					num5 = 1;
				}
				else
				{
					num1 = 2;
					num3 = -1;
					num5 = 1;
				}
				for (j = 0; j < num6; j++)
				{
					if (j == 0)
					{
						num = 2;
						num2 = 0;
						num4 = 0;
					}
					else if (j != num6 - 1)
					{
						num = 3;
						num2 = -1;
						num4 = 1;
					}
					else
					{
						num = 2;
						num2 = -1;
						num4 = 1;
					}
					float single11 = singleArray1[j, i];
					float single12 = singleArray[j, i];
					single = singleArray4[j, i];
					for (int l = 0; l < num1; l++)
					{
						for (int m = 0; m < num; m++)
						{
							if ((m != num4 || l != num5) && (this.neighbourhood == TerrainToolkit.Neighbourhood.Moore || this.neighbourhood == TerrainToolkit.Neighbourhood.VonNeumann && (m == num4 || l == num5)))
							{
								Vector3 vector31 = new Vector3((float)(m + num2), 0f, (float)(-1 * (l + num3)));
								float single13 = (90f - Vector3.Angle(vector31, vector3)) / 90f;
								if (single13 < 0f)
								{
									single13 = 0f;
								}
								float single14 = singleArray3[j + m + num2, i + l + num3];
								float single15 = single13 * (single11 - single12) * 0.1f;
								if (single15 < 0f)
								{
									single15 = 0f;
								}
								float single16 = single14 + single15;
								singleArray3[j + m + num2, i + l + num3] = single16;
								float single17 = singleArray3[j, i];
								singleArray3[j, i] = single17 - single15;
								float single18 = singleArray5[j + m + num2, i + l + num3];
								float single19 = single * single15;
								float single20 = single18 + single19;
								singleArray5[j + m + num2, i + l + num3] = single20;
								float single21 = singleArray5[j, i];
								singleArray5[j, i] = single21 - single19;
							}
						}
					}
				}
			}
			for (i = 0; i < num7; i++)
			{
				for (j = 0; j < num6; j++)
				{
					float single22 = singleArray4[j, i] + singleArray5[j, i];
					if (single22 > 1f)
					{
						single22 = 1f;
					}
					else if (single22 < 0f)
					{
						single22 = 0f;
					}
					singleArray4[j, i] = single22;
					singleArray5[j, i] = 0f;
				}
			}
			for (i = 0; i < num7; i++)
			{
				for (j = 0; j < num6; j++)
				{
					single1 = singleArray2[j, i] + singleArray3[j, i];
					single1 = single1 * (1f - this.windEntropy);
					if (single1 > 1f)
					{
						single1 = 1f;
					}
					else if (single1 < 0f)
					{
						single1 = 0f;
					}
					singleArray2[j, i] = single1;
					singleArray3[j, i] = 0f;
				}
			}
			this.smoothIterations = 1;
			this.smoothBlend = 0.25f;
			float[,] singleArray7 = (float[,])heightMap.Clone();
			TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate = new TerrainToolkit.GeneratorProgressDelegate(this.dummyGeneratorProgress);
			singleArray7 = this.smooth(singleArray7, arraySize, generatorProgressDelegate);
			for (i = 0; i < num7; i++)
			{
				for (j = 0; j < num6; j++)
				{
					float single23 = heightMap[j, i];
					float single24 = singleArray7[j, i];
					float single25 = singleArray[j, i] * this.windSmoothing;
					float single26 = single24 * single25 + single23 * (1f - single25);
					heightMap[j, i] = single26;
				}
			}
			float single27 = (float)k / (float)iterations;
			erosionProgressDelegate("Applying Wind Erosion", "Applying wind erosion.", k, iterations, single27);
		}
		return heightMap;
	}

	public void WindErosion(int iterations, float direction, float force, float lift, float gravity, float capacity, float entropy, float smoothing)
	{
		this.erosionTypeInt = 3;
		this.erosionType = TerrainToolkit.ErosionType.Wind;
		this.windIterations = iterations;
		this.windDirection = direction;
		this.windForce = force;
		this.windLift = lift;
		this.windGravity = gravity;
		this.windCapacity = capacity;
		this.windEntropy = entropy;
		this.windSmoothing = smoothing;
		this.neighbourhood = TerrainToolkit.Neighbourhood.Moore;
		this.erodeAllTerrain(new TerrainToolkit.ErosionProgressDelegate(this.dummyErosionProgress));
	}

	public enum ErosionMode
	{
		Filter,
		Brush
	}

	public delegate void ErosionProgressDelegate(string titleString, string displayString, int iteration, int nIterations, float percentComplete);

	public enum ErosionType
	{
		Thermal,
		Hydraulic,
		Tidal,
		Wind,
		Glacial
	}

	public class fastHydraulicErosionPresetData
	{
		public string presetName;

		public int hydraulicIterations;

		public float hydraulicMaxSlope;

		public float hydraulicFalloff;

		public fastHydraulicErosionPresetData(string pn, int hi, float hms, float hba)
		{
			this.presetName = pn;
			this.hydraulicIterations = hi;
			this.hydraulicMaxSlope = hms;
			this.hydraulicFalloff = hba;
		}
	}

	public enum FeatureType
	{
		Mountains,
		Hills,
		Plateaus
	}

	public class fractalPresetData
	{
		public string presetName;

		public float diamondSquareDelta;

		public float diamondSquareBlend;

		public fractalPresetData(string pn, float dsd, float dsb)
		{
			this.presetName = pn;
			this.diamondSquareDelta = dsd;
			this.diamondSquareBlend = dsb;
		}
	}

	public class fullHydraulicErosionPresetData
	{
		public string presetName;

		public int hydraulicIterations;

		public float hydraulicRainfall;

		public float hydraulicEvaporation;

		public float hydraulicSedimentSolubility;

		public float hydraulicSedimentSaturation;

		public fullHydraulicErosionPresetData(string pn, int hi, float hr, float he, float hso, float hsa)
		{
			this.presetName = pn;
			this.hydraulicIterations = hi;
			this.hydraulicRainfall = hr;
			this.hydraulicEvaporation = he;
			this.hydraulicSedimentSolubility = hso;
			this.hydraulicSedimentSaturation = hsa;
		}
	}

	public delegate void GeneratorProgressDelegate(string titleString, string displayString, float percentComplete);

	public enum GeneratorType
	{
		Voronoi,
		DiamondSquare,
		Perlin,
		Smooth,
		Normalise
	}

	public enum HydraulicType
	{
		Fast,
		Full,
		Velocity
	}

	public enum Neighbourhood
	{
		Moore,
		VonNeumann
	}

	public struct Peak
	{
		public Vector2 peakPoint;

		public float peakHeight;
	}

	public class PeakDistance : IComparable
	{
		public int id;

		public float dist;

		public PeakDistance()
		{
		}

		public int CompareTo(object obj)
		{
			TerrainToolkit.PeakDistance peakDistance = (TerrainToolkit.PeakDistance)obj;
			int num = this.dist.CompareTo(peakDistance.dist);
			if (num == 0)
			{
				num = this.dist.CompareTo(peakDistance.dist);
			}
			return num;
		}
	}

	public class PerlinNoise2D
	{
		private double[,] noiseValues;

		private float amplitude;

		private int frequency;

		public float Amplitude
		{
			get
			{
				return this.amplitude;
			}
		}

		public int Frequency
		{
			get
			{
				return this.frequency;
			}
		}

		public PerlinNoise2D(int freq, float _amp)
		{
			System.Random random = new System.Random(Environment.TickCount);
			this.noiseValues = new double[freq, freq];
			this.amplitude = _amp;
			this.frequency = freq;
			for (int i = 0; i < freq; i++)
			{
				for (int j = 0; j < freq; j++)
				{
					this.noiseValues[i, j] = random.NextDouble();
				}
			}
		}

		public double getInterpolatedPoint(int _xa, int _xb, int _ya, int _yb, double Px, double Py)
		{
			double num = this.interpolate(this.noiseValues[_xa % this.Frequency, _ya % this.frequency], this.noiseValues[_xb % this.Frequency, _ya % this.frequency], Px);
			double num1 = this.interpolate(this.noiseValues[_xa % this.Frequency, _yb % this.frequency], this.noiseValues[_xb % this.Frequency, _yb % this.frequency], Px);
			return this.interpolate(num, num1, Py);
		}

		private double interpolate(double Pa, double Pb, double Px)
		{
			double px = Px * 3.14159274101257;
			double num = (double)(1f - Mathf.Cos((float)px)) * 0.5;
			return Pa * (1 - num) + Pb * num;
		}
	}

	public class perlinPresetData
	{
		public string presetName;

		public int perlinFrequency;

		public float perlinAmplitude;

		public int perlinOctaves;

		public float perlinBlend;

		public perlinPresetData(string pn, int pf, float pa, int po, float pb)
		{
			this.presetName = pn;
			this.perlinFrequency = pf;
			this.perlinAmplitude = pa;
			this.perlinOctaves = po;
			this.perlinBlend = pb;
		}
	}

	public delegate void TextureProgressDelegate(string titleString, string displayString, float percentComplete);

	public class thermalErosionPresetData
	{
		public string presetName;

		public int thermalIterations;

		public float thermalMinSlope;

		public float thermalFalloff;

		public thermalErosionPresetData(string pn, int ti, float tms, float tba)
		{
			this.presetName = pn;
			this.thermalIterations = ti;
			this.thermalMinSlope = tms;
			this.thermalFalloff = tba;
		}
	}

	public class tidalErosionPresetData
	{
		public string presetName;

		public int tidalIterations;

		public float tidalRangeAmount;

		public float tidalCliffLimit;

		public tidalErosionPresetData(string pn, int ti, float tra, float tcl)
		{
			this.presetName = pn;
			this.tidalIterations = ti;
			this.tidalRangeAmount = tra;
			this.tidalCliffLimit = tcl;
		}
	}

	public enum ToolMode
	{
		Create,
		Erode,
		Texture
	}

	public class velocityHydraulicErosionPresetData
	{
		public string presetName;

		public int hydraulicIterations;

		public float hydraulicVelocityRainfall;

		public float hydraulicVelocityEvaporation;

		public float hydraulicVelocitySedimentSolubility;

		public float hydraulicVelocitySedimentSaturation;

		public float hydraulicVelocity;

		public float hydraulicMomentum;

		public float hydraulicEntropy;

		public float hydraulicDowncutting;

		public velocityHydraulicErosionPresetData(string pn, int hi, float hvr, float hve, float hso, float hsa, float hv, float hm, float he, float hd)
		{
			this.presetName = pn;
			this.hydraulicIterations = hi;
			this.hydraulicVelocityRainfall = hvr;
			this.hydraulicVelocityEvaporation = hve;
			this.hydraulicVelocitySedimentSolubility = hso;
			this.hydraulicVelocitySedimentSaturation = hsa;
			this.hydraulicVelocity = hv;
			this.hydraulicMomentum = hm;
			this.hydraulicEntropy = he;
			this.hydraulicDowncutting = hd;
		}
	}

	public class voronoiPresetData
	{
		public string presetName;

		public TerrainToolkit.VoronoiType voronoiType;

		public int voronoiCells;

		public float voronoiFeatures;

		public float voronoiScale;

		public float voronoiBlend;

		public voronoiPresetData(string pn, TerrainToolkit.VoronoiType vt, int c, float vf, float vs, float vb)
		{
			this.presetName = pn;
			this.voronoiType = vt;
			this.voronoiCells = c;
			this.voronoiFeatures = vf;
			this.voronoiScale = vs;
			this.voronoiBlend = vb;
		}
	}

	public enum VoronoiType
	{
		Linear,
		Sine,
		Tangent
	}

	public class windErosionPresetData
	{
		public string presetName;

		public int windIterations;

		public float windDirection;

		public float windForce;

		public float windLift;

		public float windGravity;

		public float windCapacity;

		public float windEntropy;

		public float windSmoothing;

		public windErosionPresetData(string pn, int wi, float wd, float wf, float wl, float wg, float wc, float we, float ws)
		{
			this.presetName = pn;
			this.windIterations = wi;
			this.windDirection = wd;
			this.windForce = wf;
			this.windLift = wl;
			this.windGravity = wg;
			this.windCapacity = wc;
			this.windEntropy = we;
			this.windSmoothing = ws;
		}
	}
}