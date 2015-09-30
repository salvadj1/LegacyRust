using System;
using System.Collections.Generic;
using UnityEngine;

public class FPGrassLevel : MonoBehaviour, IFPGrassAsset
{
	public int levelNumber;

	public Material levelMaterial;

	public FPGrass parent;

	[SerializeField]
	private List<FPGrassPatch> children = new List<FPGrassPatch>();

	[SerializeField]
	private float gridSpacingAtLevel;

	[SerializeField]
	private float levelSize;

	[SerializeField]
	private int gridSize;

	[SerializeField]
	private int gridSizeAtLevel;

	private Vector3 lastPosition;

	public FPGrassProbabilityGenerator probabilityGenerator;

	[NonSerialized]
	private bool probabilityUpdateQueued;

	public FPGrassLevel()
	{
	}

	internal void Draw(FPGrassPatch patch, Mesh mesh, ref Vector3 renderPosition, ref FPGrass.RenderArguments renderArgs)
	{
		if (this.probabilityUpdateQueued || grass.forceredraw)
		{
			this.UpdateMapsNow(this.lastPosition);
			this.probabilityUpdateQueued = false;
		}
		if (grass.displacement)
		{
			Graphics.Blit(FPGrassDisplacementCamera.GetRT(), this.probabilityGenerator.probabilityTexture, FPGrassDisplacementCamera.GetBlitMat());
		}
		if (!renderArgs.immediate)
		{
			Graphics.DrawMesh(mesh, renderPosition, FPGrassLevel.Constant.rotation, this.levelMaterial, base.gameObject.layer, base.camera, 0, null, FPGrass.castShadows, FPGrass.receiveShadows);
		}
		else
		{
			GL.PushMatrix();
			this.levelMaterial.SetPass(0);
			Graphics.DrawMeshNow(mesh, renderPosition, FPGrassLevel.Constant.rotation, 0);
			GL.PopMatrix();
		}
	}

	private void OnDestroy()
	{
		this.probabilityGenerator.DestroyObjects();
	}

	internal void Render(ref FPGrass.RenderArguments renderArgs)
	{
		FPGrass.RenderArguments renderArgument = renderArgs;
		renderArgument.center = this.lastPosition;
		foreach (FPGrassPatch child in this.children)
		{
			if (!child.enabled)
			{
				continue;
			}
			child.Render(ref renderArgument);
		}
	}

	public void UpdateLevel(Vector3 position, Terrain terrain)
	{
		int num = Mathf.FloorToInt(position.x / this.gridSpacingAtLevel);
		int num1 = Mathf.FloorToInt(position.z / this.gridSpacingAtLevel);
		Vector3 vector3 = Vector3.zero;
		vector3.x = (float)num * this.gridSpacingAtLevel;
		vector3.z = (float)num1 * this.gridSpacingAtLevel;
		if (vector3 != this.lastPosition && !this.probabilityUpdateQueued)
		{
			if (!Application.isPlaying)
			{
				this.UpdateMapsNow(vector3);
			}
			else
			{
				this.probabilityUpdateQueued = true;
			}
		}
		this.lastPosition = vector3;
	}

	private void UpdateMapsNow(Vector3 gridPosition)
	{
		Terrain terrain = Terrain.activeTerrain;
		if (terrain)
		{
			this.probabilityGenerator.UpdateMap(terrain.transform.InverseTransformPoint(gridPosition));
			this.levelMaterial.SetTexture("_TextureIndexTex", this.probabilityGenerator.probabilityTexture);
			this.levelMaterial.SetVector("_TerrainPosition", terrain.transform.position);
		}
	}

	private static class Constant
	{
		public readonly static Quaternion rotation;

		static Constant()
		{
			FPGrassLevel.Constant.rotation = Quaternion.identity;
		}
	}
}