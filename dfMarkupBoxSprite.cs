using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class dfMarkupBoxSprite : dfMarkupBox
{
	private static int[] TRIANGLE_INDICES;

	private dfRenderData renderData = new dfRenderData(32);

	public dfAtlas Atlas
	{
		get;
		set;
	}

	public string Source
	{
		get;
		set;
	}

	static dfMarkupBoxSprite()
	{
		dfMarkupBoxSprite.TRIANGLE_INDICES = new int[] { 0, 1, 2, 0, 2, 3 };
	}

	public dfMarkupBoxSprite(dfMarkupElement element, dfMarkupDisplayType display, dfMarkupStyle style) : base(element, display, style)
	{
	}

	private static void addTriangleIndices(dfList<Vector3> verts, dfList<int> triangles)
	{
		int count = verts.Count;
		int[] tRIANGLEINDICES = dfMarkupBoxSprite.TRIANGLE_INDICES;
		for (int i = 0; i < (int)tRIANGLEINDICES.Length; i++)
		{
			triangles.Add(count + tRIANGLEINDICES[i]);
		}
	}

	internal void LoadImage(dfAtlas atlas, string source)
	{
		dfAtlas.ItemInfo item = atlas[source];
		if (item == null)
		{
			throw new InvalidOperationException(string.Concat("Sprite does not exist in atlas: ", source));
		}
		this.Atlas = atlas;
		this.Source = source;
		this.Size = item.sizeInPixels;
		this.Baseline = (int)this.Size.y;
	}

	protected override dfRenderData OnRebuildRenderData()
	{
		this.renderData.Clear();
		if (this.Atlas != null && this.Atlas[this.Source] != null)
		{
			dfSprite.RenderOptions renderOption = new dfSprite.RenderOptions();
			dfSprite.RenderOptions atlas = renderOption;
			atlas.atlas = this.Atlas;
			atlas.spriteInfo = this.Atlas[this.Source];
			atlas.pixelsToUnits = 1f;
			atlas.size = this.Size;
			atlas.color = this.Style.Color;
			atlas.fillAmount = 1f;
			renderOption = atlas;
			dfSlicedSprite.renderSprite(this.renderData, renderOption);
			this.renderData.Material = this.Atlas.Material;
			this.renderData.Transform = Matrix4x4.identity;
		}
		return this.renderData;
	}
}