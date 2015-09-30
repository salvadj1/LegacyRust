using System;
using UnityEngine;

[ExecuteInEditMode]
public class FPGrassLayer : MonoBehaviour
{
	[NonSerialized]
	private bool _enabled;

	[NonSerialized]
	private Plane[] _frustum;

	public FPGrassLayer()
	{
	}

	private void OnDisable()
	{
		this._enabled = false;
	}

	private void OnEnable()
	{
		this._enabled = true;
	}

	private void OnPreCull()
	{
		FPGrass.RenderArguments renderArgument = new FPGrass.RenderArguments();
		if (!Terrain.activeTerrain || !Terrain.activeTerrain.terrainData)
		{
			return;
		}
		if (this._enabled && grass.on && FPGrass.anyEnabled)
		{
			Terrain terrain = Terrain.activeTerrain;
			this.UpdateDisplacement(grass.displacement);
			if (terrain)
			{
				Camera camera = base.camera;
				this._frustum = GeometryUtility.CalculateFrustumPlanes(camera);
				renderArgument.frustum = this._frustum;
				renderArgument.camera = camera;
				renderArgument.immediate = false;
				renderArgument.terrain = terrain;
				renderArgument.center = camera.transform.position;
				FPGrass.DrawAllGrass(ref renderArgument);
			}
		}
	}

	private void UpdateDisplacement(bool on)
	{
		Vector3 vector3 = new Vector3();
		Vector2 vector2 = new Vector2();
		Vector2 vector21 = new Vector2();
		Camera camera;
		if (!on)
		{
			Shader.SetGlobalVector("_DisplacementWorldMin", Vector2.zero);
			Shader.SetGlobalVector("_DisplacementWorldMax", Vector2.zero);
			return;
		}
		FPGrassDisplacementCamera fPGrassDisplacementCamera = FPGrassDisplacementCamera.Get();
		if (fPGrassDisplacementCamera == null)
		{
			camera = null;
		}
		else
		{
			camera = fPGrassDisplacementCamera.camera;
		}
		Camera vector31 = camera;
		if (vector31 == null)
		{
			return;
		}
		float single = vector31.orthographicSize;
		float single1 = single / (float)vector31.targetTexture.width;
		Vector3 vector32 = base.camera.transform.position;
		if (TransformHelpers.Dist2D(vector32, vector31.transform.position) > 5f)
		{
			vector3.x = Mathf.Round(vector32.x / single1) * single1;
			vector3.y = Mathf.Round(vector32.y / single1) * single1;
			vector3.z = Mathf.Round(vector32.z / single1) * single1;
			vector31.transform.position = vector3 + new Vector3(0f, 50f, 0f);
		}
		Vector3 vector33 = vector31.transform.position;
		vector2.x = vector33.x - single;
		vector2.y = vector33.z - single;
		vector21.x = vector33.x + single;
		vector21.y = vector33.z + single;
		Shader.SetGlobalVector("_DisplacementWorldMin", vector2);
		Shader.SetGlobalVector("_DisplacementWorldMax", vector21);
		vector31.Render();
	}
}