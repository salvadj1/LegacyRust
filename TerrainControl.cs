using Facepunch;
using System;
using UnityEngine;

[ExecuteInEditMode]
public sealed class TerrainControl : UnityEngine.MonoBehaviour
{
	[SerializeField]
	private Terrain _terrain;

	[SerializeField]
	private float _customBasemapDistance = 10000f;

	[NonSerialized]
	private bool running;

	[NonSerialized]
	private bool quitting;

	[SerializeField]
	private Material _terrainMaterialTemplate;

	private static TerrainControl activeTerrainControl;

	[SerializeField]
	private TerrainControl.TerrainSettingsHack settings;

	public bool forceCustomBasemapDistance = true;

	public string bundlePathToTerrainData = "Env/ter/rust_island_2013-2";

	public float reassignTerrainDataInterval;

	private TerrainData terrainDataFromBundle;

	[NonSerialized]
	private float timeNoticedCameraChange;

	[NonSerialized]
	private Vector3 lastCameraPosition;

	[NonSerialized]
	private Vector3 lastCameraForward;

	public float customBasemapDistance
	{
		get
		{
			return this._customBasemapDistance;
		}
		set
		{
			this._customBasemapDistance = value;
			this.BindTerrainSettings();
		}
	}

	public Terrain terrain
	{
		get
		{
			return this._terrain;
		}
	}

	public TerrainControl()
	{
	}

	private void BindTerrainSettings()
	{
		if (this.forceCustomBasemapDistance && this.terrain)
		{
			this.terrain.basemapDistance = this.customBasemapDistance;
		}
	}

	[ContextMenu("Get settings from terrain")]
	private void CopyTerrainSettings()
	{
		this.settings.CopyFrom(this.terrain);
	}

	private bool DoReassignmentOfTerrainData(bool td, bool andFlush, bool mats, bool doNotCopySettings)
	{
		if (!this.terrainDataFromBundle && !Bundling.Load<TerrainData>(this.bundlePathToTerrainData, out this.terrainDataFromBundle))
		{
			Debug.LogError(string.Concat("Bad terrain data path ", this.bundlePathToTerrainData));
			return true;
		}
		if (td)
		{
			if (!doNotCopySettings)
			{
				this.terrain.terrainData = this.terrainDataFromBundle;
				this.RestoreTerrainSettings();
			}
			else
			{
				this.terrain.terrainData = this.terrainDataFromBundle;
			}
		}
		if (mats)
		{
			this.terrain.materialTemplate = this._terrainMaterialTemplate;
		}
		if (andFlush)
		{
			this.terrain.Flush();
			if (mats)
			{
				this.terrain.materialTemplate = this._terrainMaterialTemplate;
			}
		}
		return !this.terrainDataFromBundle;
	}

	private void OnApplicationQuit()
	{
		this.quitting = true;
	}

	private void OnDisable()
	{
		if (!this.quitting && this.running)
		{
			this.running = false;
		}
	}

	private void OnEnable()
	{
		TerrainControl.activeTerrainControl = this;
		this.quitting = false;
		if (!this.running)
		{
			this.running = true;
			this.BindTerrainSettings();
		}
		if (this.reassignTerrainDataInterval > 0f)
		{
			base.Invoke("ReassignTerrainData", this.reassignTerrainDataInterval);
		}
	}

	private void ReassignTerrainData()
	{
		if (Application.isPlaying && !terrain.manual)
		{
			if (!Bundling.Load<TerrainData>(this.bundlePathToTerrainData, out this.terrainDataFromBundle))
			{
				Debug.LogError(string.Concat("Bad terrain data path ", this.bundlePathToTerrainData));
			}
			try
			{
				this.terrain.terrainData = this.terrainDataFromBundle;
				this.RestoreTerrainSettings();
			}
			catch (Exception exception)
			{
				Debug.Log(exception, this);
				base.Invoke("ReassignTerrainData", this.reassignTerrainDataInterval);
			}
		}
	}

	private void Reset()
	{
		GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Main Terrain");
		if ((int)gameObjectArray.Length > 0)
		{
			int num = 0;
			while (num < (int)gameObjectArray.Length)
			{
				this._terrain = gameObjectArray[num].GetComponent<Terrain>();
				if (!this._terrain)
				{
					num++;
				}
				else
				{
					break;
				}
			}
		}
	}

	[ContextMenu("Set settings to terrain")]
	private void RestoreTerrainSettings()
	{
		this.settings.CopyTo(this.terrain);
	}

	internal static void ter_flush()
	{
		if (TerrainControl.activeTerrainControl)
		{
			TerrainControl.activeTerrainControl.DoReassignmentOfTerrainData(false, true, false, false);
		}
	}

	internal static void ter_flushtrees()
	{
		if (TerrainControl.activeTerrainControl && TerrainControl.activeTerrainControl._terrain)
		{
			TerrainHack.RefreshTreeTextures(TerrainControl.activeTerrainControl._terrain);
		}
	}

	internal static void ter_mat()
	{
		if (TerrainControl.activeTerrainControl)
		{
			TerrainControl.activeTerrainControl.DoReassignmentOfTerrainData(false, false, true, false);
		}
	}

	internal static void ter_reassign()
	{
		if (TerrainControl.activeTerrainControl)
		{
			TerrainControl.activeTerrainControl.DoReassignmentOfTerrainData(true, false, false, false);
		}
	}

	internal static void ter_reassign_nocopy()
	{
		if (TerrainControl.activeTerrainControl)
		{
			TerrainControl.activeTerrainControl.DoReassignmentOfTerrainData(true, false, false, true);
		}
	}

	private void Update()
	{
		bool flag;
		Vector3 vector3 = new Vector3();
		float single;
		float single1;
		float single2;
		float single3 = terrain.idleinterval;
		if (single3 > 0f)
		{
			MountedCamera mountedCamera = MountedCamera.main;
			MountedCamera mountedCamera1 = mountedCamera;
			if (!mountedCamera)
			{
				flag = true;
				single = Time.realtimeSinceStartup;
				if (!flag)
				{
					single1 = Time.realtimeSinceStartup - this.timeNoticedCameraChange;
					if (single1 > single3)
					{
						single2 = (single3 <= 0f ? single : single - single1 % single3);
						this.timeNoticedCameraChange = single2;
						TerrainHack.RefreshTreeTextures(this._terrain);
					}
				}
				else
				{
					this.timeNoticedCameraChange = single;
				}
				return;
			}
			Vector3 vector31 = mountedCamera1.transform.position;
			Vector3 vector32 = mountedCamera1.transform.forward;
			vector32.Normalize();
			vector3.x = vector31.x - this.lastCameraPosition.x;
			vector3.y = vector31.y - this.lastCameraPosition.y;
			vector3.z = vector31.z - this.lastCameraPosition.z;
			if ((vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z > 5.625E-05f ? false : Vector3.Angle(vector32, this.lastCameraForward) <= 0.5f))
			{
				flag = false;
				single = Time.realtimeSinceStartup;
				if (!flag)
				{
					single1 = Time.realtimeSinceStartup - this.timeNoticedCameraChange;
					if (single1 > single3)
					{
						single2 = (single3 <= 0f ? single : single - single1 % single3);
						this.timeNoticedCameraChange = single2;
						TerrainHack.RefreshTreeTextures(this._terrain);
					}
				}
				else
				{
					this.timeNoticedCameraChange = single;
				}
				return;
			}
			else
			{
				this.lastCameraPosition = vector31;
				this.lastCameraForward = vector32;
				flag = true;
				single = Time.realtimeSinceStartup;
				if (!flag)
				{
					single1 = Time.realtimeSinceStartup - this.timeNoticedCameraChange;
					if (single1 > single3)
					{
						single2 = (single3 <= 0f ? single : single - single1 % single3);
						this.timeNoticedCameraChange = single2;
						TerrainHack.RefreshTreeTextures(this._terrain);
					}
				}
				else
				{
					this.timeNoticedCameraChange = single;
				}
				return;
			}
		}
		flag = true;
		single = Time.realtimeSinceStartup;
		if (!flag)
		{
			single1 = Time.realtimeSinceStartup - this.timeNoticedCameraChange;
			if (single1 > single3)
			{
				single2 = (single3 <= 0f ? single : single - single1 % single3);
				this.timeNoticedCameraChange = single2;
				TerrainHack.RefreshTreeTextures(this._terrain);
			}
		}
		else
		{
			this.timeNoticedCameraChange = single;
		}
	}

	[Serializable]
	private class TerrainSettingsHack
	{
		public float basemapDistance;

		public bool castShadows;

		public float detailObjectDensity;

		public float detailObjectDistance;

		public int heightmapMaximumLOD;

		public float heightmapPixelError;

		public Material materialTemplate;

		public float treeBillboardDistance;

		public float treeCrossFadeLength;

		public float treeDistance;

		public int treeMaximumFullLODCount;

		public TerrainSettingsHack()
		{
		}

		public void CopyFrom(Terrain terrain)
		{
			this.basemapDistance = terrain.basemapDistance;
			this.castShadows = terrain.castShadows;
			this.detailObjectDensity = terrain.detailObjectDensity;
			this.detailObjectDistance = terrain.detailObjectDistance;
			this.heightmapMaximumLOD = terrain.heightmapMaximumLOD;
			this.heightmapPixelError = terrain.heightmapPixelError;
			this.materialTemplate = terrain.materialTemplate;
			this.treeBillboardDistance = terrain.treeBillboardDistance;
			this.treeCrossFadeLength = terrain.treeCrossFadeLength;
			this.treeDistance = terrain.treeDistance;
			this.treeMaximumFullLODCount = terrain.treeMaximumFullLODCount;
		}

		public void CopyTo(Terrain terrain)
		{
			terrain.basemapDistance = this.basemapDistance;
			terrain.castShadows = this.castShadows;
			terrain.detailObjectDensity = this.detailObjectDensity;
			terrain.detailObjectDistance = this.detailObjectDistance;
			terrain.heightmapMaximumLOD = this.heightmapMaximumLOD;
			terrain.heightmapPixelError = this.heightmapPixelError;
			terrain.materialTemplate = this.materialTemplate;
			terrain.treeBillboardDistance = this.treeBillboardDistance;
			terrain.treeCrossFadeLength = this.treeCrossFadeLength;
			terrain.treeDistance = this.treeDistance;
			terrain.treeMaximumFullLODCount = this.treeMaximumFullLODCount;
		}
	}
}