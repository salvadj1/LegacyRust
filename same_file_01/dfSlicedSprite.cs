using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Sprite/Sliced")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfSlicedSprite : dfSprite
{
	private static int[] triangleIndices;

	private static int[][] horzFill;

	private static int[][] vertFill;

	private static int[][] fillIndices;

	private static Vector3[] verts;

	private static Vector2[] uv;

	static dfSlicedSprite()
	{
		dfSlicedSprite.triangleIndices = new int[] { 0, 1, 2, 2, 3, 0, 4, 5, 6, 6, 7, 4, 8, 9, 10, 10, 11, 8, 12, 13, 14, 14, 15, 12, 1, 4, 7, 7, 2, 1, 9, 12, 15, 15, 10, 9, 3, 2, 9, 9, 8, 3, 7, 6, 13, 13, 12, 7, 2, 7, 12, 12, 9, 2 };
		dfSlicedSprite.horzFill = new int[][] { new int[] { 0, 1, 4, 5 }, new int[] { 3, 2, 7, 6 }, new int[] { 8, 9, 12, 13 }, new int[] { 11, 10, 15, 14 } };
		dfSlicedSprite.vertFill = new int[][] { new int[] { 11, 8, 3, 0 }, new int[] { 10, 9, 2, 1 }, new int[] { 15, 12, 7, 4 }, new int[] { 14, 13, 6, 5 } };
		dfSlicedSprite.fillIndices = new int[][] { new int[4], new int[4], new int[4], new int[4] };
		dfSlicedSprite.verts = new Vector3[16];
		dfSlicedSprite.uv = new Vector2[16];
	}

	public dfSlicedSprite()
	{
	}

	private static void doFill(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		int num = options.baseIndex;
		dfList<Vector3> vertices = renderData.Vertices;
		dfList<Vector2> uV = renderData.UV;
		int[][] fillIndices = dfSlicedSprite.getFillIndices(options.fillDirection, num);
		bool flag = options.invertFill;
		if (options.fillDirection == dfFillDirection.Vertical)
		{
			flag = !flag;
		}
		if (flag)
		{
			for (int i = 0; i < (int)fillIndices.Length; i++)
			{
				Array.Reverse(fillIndices[i]);
			}
		}
		int num1 = (options.fillDirection != dfFillDirection.Horizontal ? 1 : 0);
		float item = vertices[fillIndices[0][(flag ? 3 : 0)]][num1];
		float single = vertices[fillIndices[0][(flag ? 0 : 3)]][num1];
		float single1 = Mathf.Abs(single - item);
		float single2 = (flag ? single - options.fillAmount * single1 : item + options.fillAmount * single1);
		for (int j = 0; j < (int)fillIndices.Length; j++)
		{
			if (flag)
			{
				for (int k = 1; k < 4; k++)
				{
					Vector3 vector3 = vertices[fillIndices[j][k]];
					float item1 = vector3[num1];
					if (item1 <= single2)
					{
						Vector3 vector31 = vertices[fillIndices[j][k]];
						vector31[num1] = single2;
						vertices[fillIndices[j][k]] = vector31;
						Vector3 item2 = vertices[fillIndices[j][k - 1]];
						float item3 = item2[num1];
						if (item3 >= single2)
						{
							float single3 = (single2 - item3) / (item1 - item3);
							Vector2 vector2 = uV[fillIndices[j][k]];
							float item4 = vector2[num1];
							Vector2 vector21 = uV[fillIndices[j][k - 1]];
							float single4 = vector21[num1];
							Vector2 vector22 = uV[fillIndices[j][k]];
							vector22[num1] = Mathf.Lerp(single4, item4, single3);
							uV[fillIndices[j][k]] = vector22;
						}
					}
				}
			}
			else
			{
				for (int l = 3; l > 0; l--)
				{
					Vector3 vector32 = vertices[fillIndices[j][l]];
					float item5 = vector32[num1];
					if (item5 >= single2)
					{
						Vector3 vector33 = vertices[fillIndices[j][l]];
						vector33[num1] = single2;
						vertices[fillIndices[j][l]] = vector33;
						Vector3 vector34 = vertices[fillIndices[j][l - 1]];
						float single5 = vector34[num1];
						if (single5 <= single2)
						{
							float single6 = (single2 - single5) / (item5 - single5);
							Vector2 vector23 = uV[fillIndices[j][l]];
							float item6 = vector23[num1];
							Vector2 vector24 = uV[fillIndices[j][l - 1]];
							float item7 = vector24[num1];
							Vector2 vector25 = uV[fillIndices[j][l]];
							vector25[num1] = Mathf.Lerp(item7, item6, single6);
							uV[fillIndices[j][l]] = vector25;
						}
					}
				}
			}
		}
	}

	private static int[][] getFillIndices(dfFillDirection fillDirection, int baseIndex)
	{
		int[][] numArray = (fillDirection != dfFillDirection.Horizontal ? dfSlicedSprite.vertFill : dfSlicedSprite.horzFill);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				dfSlicedSprite.fillIndices[i][j] = baseIndex + numArray[i][j];
			}
		}
		return dfSlicedSprite.fillIndices;
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
		if (spriteInfo.border.horizontal == 0 && spriteInfo.border.vertical == 0)
		{
			base.OnRebuildRenderData();
			return;
		}
		Color32 color32 = base.ApplyOpacity((!base.IsEnabled ? this.disabledColor : this.color));
		dfSprite.RenderOptions renderOption = new dfSprite.RenderOptions();
		dfSprite.RenderOptions upperLeft = renderOption;
		upperLeft.atlas = this.atlas;
		upperLeft.color = color32;
		upperLeft.fillAmount = this.fillAmount;
		upperLeft.fillDirection = this.fillDirection;
		upperLeft.flip = this.flip;
		upperLeft.invertFill = this.invertFill;
		upperLeft.offset = this.pivot.TransformToUpperLeft(base.Size);
		upperLeft.pixelsToUnits = base.PixelsToUnits();
		upperLeft.size = base.Size;
		upperLeft.spriteInfo = base.SpriteInfo;
		renderOption = upperLeft;
		dfSlicedSprite.renderSprite(this.renderData, renderOption);
	}

	private static void rebuildColors(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		for (int i = 0; i < 16; i++)
		{
			renderData.Colors.Add(options.color);
		}
	}

	private static void rebuildTriangles(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		int num = options.baseIndex;
		dfList<int> triangles = renderData.Triangles;
		for (int i = 0; i < (int)dfSlicedSprite.triangleIndices.Length; i++)
		{
			triangles.Add(num + dfSlicedSprite.triangleIndices[i]);
		}
	}

	private static void rebuildUV(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		dfAtlas dfAtla = options.atlas;
		Vector2 vector2 = new Vector2((float)dfAtla.Texture.width, (float)dfAtla.Texture.height);
		dfAtlas.ItemInfo itemInfo = options.spriteInfo;
		float single = (float)itemInfo.border.top / vector2.y;
		float single1 = (float)itemInfo.border.bottom / vector2.y;
		float single2 = (float)itemInfo.border.left / vector2.x;
		float single3 = (float)itemInfo.border.right / vector2.x;
		Rect rect = itemInfo.region;
		dfSlicedSprite.uv[0] = new Vector2(rect.x, rect.yMax);
		dfSlicedSprite.uv[1] = new Vector2(rect.x + single2, rect.yMax);
		dfSlicedSprite.uv[2] = new Vector2(rect.x + single2, rect.yMax - single);
		dfSlicedSprite.uv[3] = new Vector2(rect.x, rect.yMax - single);
		dfSlicedSprite.uv[4] = new Vector2(rect.xMax - single3, rect.yMax);
		dfSlicedSprite.uv[5] = new Vector2(rect.xMax, rect.yMax);
		dfSlicedSprite.uv[6] = new Vector2(rect.xMax, rect.yMax - single);
		dfSlicedSprite.uv[7] = new Vector2(rect.xMax - single3, rect.yMax - single);
		dfSlicedSprite.uv[8] = new Vector2(rect.x, rect.y + single1);
		dfSlicedSprite.uv[9] = new Vector2(rect.x + single2, rect.y + single1);
		dfSlicedSprite.uv[10] = new Vector2(rect.x + single2, rect.y);
		dfSlicedSprite.uv[11] = new Vector2(rect.x, rect.y);
		dfSlicedSprite.uv[12] = new Vector2(rect.xMax - single3, rect.y + single1);
		dfSlicedSprite.uv[13] = new Vector2(rect.xMax, rect.y + single1);
		dfSlicedSprite.uv[14] = new Vector2(rect.xMax, rect.y);
		dfSlicedSprite.uv[15] = new Vector2(rect.xMax - single3, rect.y);
		if (options.flip != dfSpriteFlip.None)
		{
			for (int i = 0; i < (int)dfSlicedSprite.uv.Length; i = i + 4)
			{
				Vector2 vector21 = Vector2.zero;
				if (options.flip.IsSet(dfSpriteFlip.FlipHorizontal))
				{
					vector21 = dfSlicedSprite.uv[i];
					dfSlicedSprite.uv[i] = dfSlicedSprite.uv[i + 1];
					dfSlicedSprite.uv[i + 1] = vector21;
					vector21 = dfSlicedSprite.uv[i + 2];
					dfSlicedSprite.uv[i + 2] = dfSlicedSprite.uv[i + 3];
					dfSlicedSprite.uv[i + 3] = vector21;
				}
				if (options.flip.IsSet(dfSpriteFlip.FlipVertical))
				{
					vector21 = dfSlicedSprite.uv[i];
					dfSlicedSprite.uv[i] = dfSlicedSprite.uv[i + 3];
					dfSlicedSprite.uv[i + 3] = vector21;
					vector21 = dfSlicedSprite.uv[i + 1];
					dfSlicedSprite.uv[i + 1] = dfSlicedSprite.uv[i + 2];
					dfSlicedSprite.uv[i + 2] = vector21;
				}
			}
			if (options.flip.IsSet(dfSpriteFlip.FlipHorizontal))
			{
				Vector2[] vector2Array = new Vector2[(int)dfSlicedSprite.uv.Length];
				Array.Copy(dfSlicedSprite.uv, vector2Array, (int)dfSlicedSprite.uv.Length);
				Array.Copy(dfSlicedSprite.uv, 0, dfSlicedSprite.uv, 4, 4);
				Array.Copy(vector2Array, 4, dfSlicedSprite.uv, 0, 4);
				Array.Copy(dfSlicedSprite.uv, 8, dfSlicedSprite.uv, 12, 4);
				Array.Copy(vector2Array, 12, dfSlicedSprite.uv, 8, 4);
			}
			if (options.flip.IsSet(dfSpriteFlip.FlipVertical))
			{
				Vector2[] vector2Array1 = new Vector2[(int)dfSlicedSprite.uv.Length];
				Array.Copy(dfSlicedSprite.uv, vector2Array1, (int)dfSlicedSprite.uv.Length);
				Array.Copy(dfSlicedSprite.uv, 0, dfSlicedSprite.uv, 8, 4);
				Array.Copy(vector2Array1, 8, dfSlicedSprite.uv, 0, 4);
				Array.Copy(dfSlicedSprite.uv, 4, dfSlicedSprite.uv, 12, 4);
				Array.Copy(vector2Array1, 12, dfSlicedSprite.uv, 4, 4);
			}
		}
		for (int j = 0; j < (int)dfSlicedSprite.uv.Length; j++)
		{
			renderData.UV.Add(dfSlicedSprite.uv[j]);
		}
	}

	private static void rebuildVertices(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		float single = 0f;
		float single1 = 0f;
		float single2 = Mathf.Ceil(options.size.x);
		float single3 = Mathf.Ceil(-options.size.y);
		dfAtlas.ItemInfo itemInfo = options.spriteInfo;
		float single4 = (float)itemInfo.border.left;
		float single5 = (float)itemInfo.border.top;
		float single6 = (float)itemInfo.border.right;
		float single7 = (float)itemInfo.border.bottom;
		if (options.flip.IsSet(dfSpriteFlip.FlipHorizontal))
		{
			float single8 = single6;
			single6 = single4;
			single4 = single8;
		}
		if (options.flip.IsSet(dfSpriteFlip.FlipVertical))
		{
			float single9 = single7;
			single7 = single5;
			single5 = single9;
		}
		dfSlicedSprite.verts[0] = new Vector3(single, single1, 0f) + options.offset;
		dfSlicedSprite.verts[1] = dfSlicedSprite.verts[0] + new Vector3(single4, 0f, 0f);
		dfSlicedSprite.verts[2] = dfSlicedSprite.verts[0] + new Vector3(single4, -single5, 0f);
		dfSlicedSprite.verts[3] = dfSlicedSprite.verts[0] + new Vector3(0f, -single5, 0f);
		dfSlicedSprite.verts[4] = new Vector3(single2 - single6, single1, 0f) + options.offset;
		dfSlicedSprite.verts[5] = dfSlicedSprite.verts[4] + new Vector3(single6, 0f, 0f);
		dfSlicedSprite.verts[6] = dfSlicedSprite.verts[4] + new Vector3(single6, -single5, 0f);
		dfSlicedSprite.verts[7] = dfSlicedSprite.verts[4] + new Vector3(0f, -single5, 0f);
		dfSlicedSprite.verts[8] = new Vector3(single, single3 + single7, 0f) + options.offset;
		dfSlicedSprite.verts[9] = dfSlicedSprite.verts[8] + new Vector3(single4, 0f, 0f);
		dfSlicedSprite.verts[10] = dfSlicedSprite.verts[8] + new Vector3(single4, -single7, 0f);
		dfSlicedSprite.verts[11] = dfSlicedSprite.verts[8] + new Vector3(0f, -single7, 0f);
		dfSlicedSprite.verts[12] = new Vector3(single2 - single6, single3 + single7, 0f) + options.offset;
		dfSlicedSprite.verts[13] = dfSlicedSprite.verts[12] + new Vector3(single6, 0f, 0f);
		dfSlicedSprite.verts[14] = dfSlicedSprite.verts[12] + new Vector3(single6, -single7, 0f);
		dfSlicedSprite.verts[15] = dfSlicedSprite.verts[12] + new Vector3(0f, -single7, 0f);
		for (int i = 0; i < (int)dfSlicedSprite.verts.Length; i++)
		{
			renderData.Vertices.Add((dfSlicedSprite.verts[i] * options.pixelsToUnits).Quantize(options.pixelsToUnits));
		}
	}

	internal static new void renderSprite(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		options.baseIndex = renderData.Vertices.Count;
		dfSlicedSprite.rebuildTriangles(renderData, options);
		dfSlicedSprite.rebuildVertices(renderData, options);
		dfSlicedSprite.rebuildUV(renderData, options);
		dfSlicedSprite.rebuildColors(renderData, options);
		if (options.fillAmount < 1f)
		{
			dfSlicedSprite.doFill(renderData, options);
		}
	}
}