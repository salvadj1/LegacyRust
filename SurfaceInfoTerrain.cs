using System;
using UnityEngine;

public class SurfaceInfoTerrain : SurfaceInfo
{
	public SurfaceInfoObject[] surfaces;

	public SurfaceInfoTerrain()
	{
	}

	public override SurfaceInfoObject SurfaceObj()
	{
		return this.surfaces[0];
	}

	public override SurfaceInfoObject SurfaceObj(Vector3 worldPos)
	{
		int textureIndex = TerrainTextureHelper.GetTextureIndex(worldPos);
		if (textureIndex < (int)this.surfaces.Length)
		{
			return this.surfaces[textureIndex];
		}
		Debug.Log(string.Concat("Missing surface info for splat index ", textureIndex));
		return this.surfaces[0];
	}
}