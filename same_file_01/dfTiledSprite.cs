using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Sprite/Tiled")]
[ExecuteInEditMode]
[Serializable]
public class dfTiledSprite : dfSprite
{
	private static int[] quadTriangles;

	private static Vector2[] quadUV;

	[SerializeField]
	protected Vector2 tileScale = Vector2.one;

	[SerializeField]
	protected Vector2 tileScroll = Vector2.zero;

	public Vector2 TileScale
	{
		get
		{
			return this.tileScale;
		}
		set
		{
			if (Vector2.Distance(value, this.tileScale) > 1.401298E-45f)
			{
				this.tileScale = Vector2.Max(Vector2.one * 0.1f, value);
				this.Invalidate();
			}
		}
	}

	public Vector2 TileScroll
	{
		get
		{
			return this.tileScroll;
		}
		set
		{
			if (Vector2.Distance(value, this.tileScroll) > 1.401298E-45f)
			{
				this.tileScroll = value;
				this.Invalidate();
			}
		}
	}

	static dfTiledSprite()
	{
		dfTiledSprite.quadTriangles = new int[] { 0, 1, 3, 3, 1, 2 };
		dfTiledSprite.quadUV = new Vector2[4];
	}

	public dfTiledSprite()
	{
	}

	private void addQuadColors(dfList<Color32> colors)
	{
		colors.EnsureCapacity(colors.Count + 4);
		Color32 color32 = base.ApplyOpacity((!base.IsEnabled ? this.disabledColor : this.color));
		for (int i = 0; i < 4; i++)
		{
			colors.Add(color32);
		}
	}

	private void addQuadTriangles(dfList<int> triangles, int baseIndex)
	{
		for (int i = 0; i < (int)dfTiledSprite.quadTriangles.Length; i++)
		{
			triangles.Add(dfTiledSprite.quadTriangles[i] + baseIndex);
		}
	}

	private void addQuadUV(dfList<Vector2> uv, Vector2[] spriteUV)
	{
		uv.AddRange(spriteUV);
	}

	private Vector2[] buildQuadUV()
	{
		Rect spriteInfo = base.SpriteInfo.region;
		dfTiledSprite.quadUV[0] = new Vector2(spriteInfo.x, spriteInfo.yMax);
		dfTiledSprite.quadUV[1] = new Vector2(spriteInfo.xMax, spriteInfo.yMax);
		dfTiledSprite.quadUV[2] = new Vector2(spriteInfo.xMax, spriteInfo.y);
		dfTiledSprite.quadUV[3] = new Vector2(spriteInfo.x, spriteInfo.y);
		Vector2 vector2 = Vector2.zero;
		if (this.flip.IsSet(dfSpriteFlip.FlipHorizontal))
		{
			vector2 = dfTiledSprite.quadUV[1];
			dfTiledSprite.quadUV[1] = dfTiledSprite.quadUV[0];
			dfTiledSprite.quadUV[0] = vector2;
			vector2 = dfTiledSprite.quadUV[3];
			dfTiledSprite.quadUV[3] = dfTiledSprite.quadUV[2];
			dfTiledSprite.quadUV[2] = vector2;
		}
		if (this.flip.IsSet(dfSpriteFlip.FlipVertical))
		{
			vector2 = dfTiledSprite.quadUV[0];
			dfTiledSprite.quadUV[0] = dfTiledSprite.quadUV[3];
			dfTiledSprite.quadUV[3] = vector2;
			vector2 = dfTiledSprite.quadUV[1];
			dfTiledSprite.quadUV[1] = dfTiledSprite.quadUV[2];
			dfTiledSprite.quadUV[2] = vector2;
		}
		return dfTiledSprite.quadUV;
	}

	private void clipQuads(dfList<Vector3> verts, dfList<Vector2> uv)
	{
		float single = 0f;
		float single1 = this.size.x;
		float single2 = -this.size.y;
		float single3 = 0f;
		if (this.fillAmount < 1f)
		{
			if (this.fillDirection == dfFillDirection.Horizontal)
			{
				if (this.invertFill)
				{
					single = this.size.x - this.size.x * this.fillAmount;
				}
				else
				{
					single1 = this.size.x * this.fillAmount;
				}
			}
			else if (this.invertFill)
			{
				single3 = -this.size.y * (1f - this.fillAmount);
			}
			else
			{
				single2 = -this.size.y * this.fillAmount;
			}
		}
		for (int i = 0; i < verts.Count; i = i + 4)
		{
			Vector3 item = verts[i];
			Vector3 vector3 = verts[i + 1];
			Vector3 item1 = verts[i + 2];
			Vector3 vector31 = verts[i + 3];
			float single4 = vector3.x - item.x;
			float single5 = item.y - vector31.y;
			if (item.x < single)
			{
				float single6 = (single - item.x) / single4;
				item = new Vector3(Mathf.Max(single, item.x), item.y, item.z);
				verts[i] = item;
				vector3 = new Vector3(Mathf.Max(single, vector3.x), vector3.y, vector3.z);
				verts[i + 1] = vector3;
				item1 = new Vector3(Mathf.Max(single, item1.x), item1.y, item1.z);
				verts[i + 2] = item1;
				vector31 = new Vector3(Mathf.Max(single, vector31.x), vector31.y, vector31.z);
				verts[i + 3] = vector31;
				float item2 = uv[i].x;
				Vector2 vector2 = uv[i + 1];
				float single7 = Mathf.Lerp(item2, vector2.x, single6);
				Vector2 vector21 = uv[i];
				uv[i] = new Vector2(single7, vector21.y);
				Vector2 vector22 = uv[i + 3];
				uv[i + 3] = new Vector2(single7, vector22.y);
				single4 = vector3.x - item.x;
			}
			if (vector3.x > single1)
			{
				float single8 = 1f - (single1 - vector3.x + single4) / single4;
				item = new Vector3(Mathf.Min(item.x, single1), item.y, item.z);
				verts[i] = item;
				vector3 = new Vector3(Mathf.Min(vector3.x, single1), vector3.y, vector3.z);
				verts[i + 1] = vector3;
				item1 = new Vector3(Mathf.Min(item1.x, single1), item1.y, item1.z);
				verts[i + 2] = item1;
				vector31 = new Vector3(Mathf.Min(vector31.x, single1), vector31.y, vector31.z);
				verts[i + 3] = vector31;
				float item3 = uv[i + 1].x;
				Vector2 vector23 = uv[i];
				float single9 = Mathf.Lerp(item3, vector23.x, single8);
				Vector2 item4 = uv[i + 1];
				uv[i + 1] = new Vector2(single9, item4.y);
				Vector2 vector24 = uv[i + 2];
				uv[i + 2] = new Vector2(single9, vector24.y);
				single4 = vector3.x - item.x;
			}
			if (vector31.y < single2)
			{
				float single10 = 1f - Mathf.Abs(-single2 + item.y) / single5;
				item = new Vector3(item.x, Mathf.Max(item.y, single2), vector3.z);
				verts[i] = item;
				vector3 = new Vector3(vector3.x, Mathf.Max(vector3.y, single2), vector3.z);
				verts[i + 1] = vector3;
				item1 = new Vector3(item1.x, Mathf.Max(item1.y, single2), item1.z);
				verts[i + 2] = item1;
				vector31 = new Vector3(vector31.x, Mathf.Max(vector31.y, single2), vector31.z);
				verts[i + 3] = vector31;
				float item5 = uv[i + 3].y;
				Vector2 vector25 = uv[i];
				float single11 = Mathf.Lerp(item5, vector25.y, single10);
				Vector2 item6 = uv[i + 3];
				uv[i + 3] = new Vector2(item6.x, single11);
				Vector2 vector26 = uv[i + 2];
				uv[i + 2] = new Vector2(vector26.x, single11);
				single5 = Mathf.Abs(vector31.y - item.y);
			}
			if (item.y > single3)
			{
				float single12 = Mathf.Abs(single3 - item.y) / single5;
				item = new Vector3(item.x, Mathf.Min(single3, item.y), item.z);
				verts[i] = item;
				vector3 = new Vector3(vector3.x, Mathf.Min(single3, vector3.y), vector3.z);
				verts[i + 1] = vector3;
				item1 = new Vector3(item1.x, Mathf.Min(single3, item1.y), item1.z);
				verts[i + 2] = item1;
				vector31 = new Vector3(vector31.x, Mathf.Min(single3, vector31.y), vector31.z);
				verts[i + 3] = vector31;
				float item7 = uv[i].y;
				Vector2 vector27 = uv[i + 3];
				float single13 = Mathf.Lerp(item7, vector27.y, single12);
				Vector2 item8 = uv[i];
				uv[i] = new Vector2(item8.x, single13);
				Vector2 vector28 = uv[i + 1];
				uv[i + 1] = new Vector2(vector28.x, single13);
			}
		}
	}

	protected override void OnRebuildRenderData()
	{
		if (base.Atlas == null)
		{
			return;
		}
		dfAtlas.ItemInfo spriteInfo = base.SpriteInfo;
		if (spriteInfo == null)
		{
			return;
		}
		this.renderData.Material = base.Atlas.Material;
		dfList<Vector3> vertices = this.renderData.Vertices;
		dfList<Vector2> uV = this.renderData.UV;
		dfList<Color32> colors = this.renderData.Colors;
		dfList<int> triangles = this.renderData.Triangles;
		Vector2[] vector2Array = this.buildQuadUV();
		Vector2 vector2 = Vector2.Scale(spriteInfo.sizeInPixels, this.tileScale);
		Vector2 vector21 = new Vector2(this.tileScroll.x % 1f, this.tileScroll.y % 1f);
		for (float i = -Mathf.Abs(vector21.y * vector2.y); i < this.size.y; i = i + vector2.y)
		{
			for (float j = -Mathf.Abs(vector21.x * vector2.x); j < this.size.x; j = j + vector2.x)
			{
				int count = vertices.Count;
				vertices.Add(new Vector3(j, -i));
				vertices.Add(new Vector3(j + vector2.x, -i));
				vertices.Add(new Vector3(j + vector2.x, -i + -vector2.y));
				vertices.Add(new Vector3(j, -i + -vector2.y));
				this.addQuadTriangles(triangles, count);
				this.addQuadUV(uV, vector2Array);
				this.addQuadColors(colors);
			}
		}
		this.clipQuads(vertices, uV);
		float units = base.PixelsToUnits();
		Vector3 upperLeft = this.pivot.TransformToUpperLeft(this.size);
		for (int k = 0; k < vertices.Count; k++)
		{
			vertices[k] = (vertices[k] + upperLeft) * units;
		}
	}
}