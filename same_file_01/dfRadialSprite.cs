using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Sprite/Radial")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfRadialSprite : dfSprite
{
	private static Vector3[] baseVerts;

	[SerializeField]
	protected dfPivotPoint fillOrigin = dfPivotPoint.MiddleCenter;

	public dfPivotPoint FillOrigin
	{
		get
		{
			return this.fillOrigin;
		}
		set
		{
			if (value != this.fillOrigin)
			{
				this.fillOrigin = value;
				this.Invalidate();
			}
		}
	}

	static dfRadialSprite()
	{
		dfRadialSprite.baseVerts = new Vector3[] { new Vector3(0f, 0.5f, 0f), new Vector3(0.5f, 0.5f, 0f), new Vector3(0.5f, 0f, 0f), new Vector3(0.5f, -0.5f, 0f), new Vector3(0f, -0.5f, 0f), new Vector3(-0.5f, -0.5f, 0f), new Vector3(-0.5f, 0f, 0f), new Vector3(-0.5f, 0.5f, 0f) };
	}

	public dfRadialSprite()
	{
	}

	private Color32[] buildColors(int vertCount)
	{
		Color32 color32 = base.ApplyOpacity((!base.IsEnabled ? this.disabledColor : this.color));
		Color32[] color32Array = new Color32[vertCount];
		for (int i = 0; i < (int)color32Array.Length; i++)
		{
			color32Array[i] = color32;
		}
		return color32Array;
	}

	private void buildMeshData(ref List<Vector3> verts, ref List<int> indices, ref List<Vector2> uv)
	{
		List<Vector3> vector3s = new List<Vector3>();
		List<Vector3> vector3s1 = vector3s;
		verts = vector3s;
		List<Vector3> vector3s2 = vector3s1;
		verts.AddRange(dfRadialSprite.baseVerts);
		int num = 8;
		int num1 = -1;
		switch (this.fillOrigin)
		{
			case dfPivotPoint.TopLeft:
			{
				num = 4;
				num1 = 5;
				vector3s2.RemoveAt(6);
				vector3s2.RemoveAt(0);
				break;
			}
			case dfPivotPoint.TopCenter:
			{
				num = 6;
				num1 = 0;
				break;
			}
			case dfPivotPoint.TopRight:
			{
				num = 4;
				num1 = 0;
				vector3s2.RemoveAt(2);
				vector3s2.RemoveAt(0);
				break;
			}
			case dfPivotPoint.MiddleLeft:
			{
				num = 6;
				num1 = 6;
				break;
			}
			case dfPivotPoint.MiddleCenter:
			{
				num = 8;
				vector3s2.Add(vector3s2[0]);
				vector3s2.Insert(0, Vector3.zero);
				num1 = 0;
				break;
			}
			case dfPivotPoint.MiddleRight:
			{
				num = 6;
				num1 = 2;
				break;
			}
			case dfPivotPoint.BottomLeft:
			{
				num = 4;
				num1 = 4;
				vector3s2.RemoveAt(6);
				vector3s2.RemoveAt(4);
				break;
			}
			case dfPivotPoint.BottomCenter:
			{
				num = 6;
				num1 = 4;
				break;
			}
			case dfPivotPoint.BottomRight:
			{
				num = 4;
				num1 = 2;
				vector3s2.RemoveAt(4);
				vector3s2.RemoveAt(2);
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
		}
		this.makeFirst(vector3s2, num1);
		List<int> nums = this.buildTriangles(vector3s2);
		List<int> nums1 = nums;
		indices = nums;
		List<int> nums2 = nums1;
		float single = 1f / (float)num;
		float single1 = this.fillAmount.Quantize(single);
		for (int i = Mathf.CeilToInt(single1 / single) + 1; i < num; i++)
		{
			if (!this.invertFill)
			{
				vector3s2.RemoveAt(vector3s2.Count - 1);
				nums2.RemoveRange(nums2.Count - 3, 3);
			}
			else
			{
				nums2.RemoveRange(0, 3);
			}
		}
		if (this.fillAmount < 1f)
		{
			int item = nums2[(!this.invertFill ? nums2.Count - 2 : 2)];
			int item1 = nums2[(!this.invertFill ? nums2.Count - 1 : 1)];
			float fillAmount = (base.FillAmount - single1) / single;
			vector3s2[item1] = Vector3.Lerp(vector3s2[item], vector3s2[item1], fillAmount);
		}
		uv = this.buildUV(vector3s2);
		float units = base.PixelsToUnits();
		Vector2 vector2 = units * this.size;
		Vector3 center = this.pivot.TransformToCenter(this.size) * units;
		for (int j = 0; j < vector3s2.Count; j++)
		{
			vector3s2[j] = Vector3.Scale(vector3s2[j], vector2) + center;
		}
	}

	private List<int> buildTriangles(List<Vector3> verts)
	{
		List<int> nums = new List<int>();
		int count = verts.Count;
		for (int i = 1; i < count - 1; i++)
		{
			nums.Add(0);
			nums.Add(i);
			nums.Add(i + 1);
		}
		return nums;
	}

	private List<Vector2> buildUV(List<Vector3> verts)
	{
		dfAtlas.ItemInfo spriteInfo = base.SpriteInfo;
		if (spriteInfo == null)
		{
			return null;
		}
		Rect rect = spriteInfo.region;
		if (this.flip.IsSet(dfSpriteFlip.FlipHorizontal))
		{
			rect = new Rect(rect.xMax, rect.y, -rect.width, rect.height);
		}
		if (this.flip.IsSet(dfSpriteFlip.FlipVertical))
		{
			rect = new Rect(rect.x, rect.yMax, rect.width, -rect.height);
		}
		Vector2 vector2 = new Vector2(rect.x, rect.y);
		Vector2 vector21 = new Vector2(0.5f, 0.5f);
		Vector2 vector22 = new Vector2(rect.width, rect.height);
		List<Vector2> vector2s = new List<Vector2>(verts.Count);
		for (int i = 0; i < verts.Count; i++)
		{
			Vector2 item = verts[i] + vector21;
			vector2s.Add(Vector2.Scale(item, vector22) + vector2);
		}
		return vector2s;
	}

	private void makeFirst(List<Vector3> list, int index)
	{
		if (index == 0)
		{
			return;
		}
		List<Vector3> range = list.GetRange(index, list.Count - index);
		list.RemoveRange(index, list.Count - index);
		list.InsertRange(0, range);
	}

	protected override void OnRebuildRenderData()
	{
		if (base.Atlas == null)
		{
			return;
		}
		if (base.SpriteInfo == null)
		{
			return;
		}
		this.renderData.Material = base.Atlas.Material;
		List<Vector3> vector3s = null;
		List<int> nums = null;
		List<Vector2> vector2s = null;
		this.buildMeshData(ref vector3s, ref nums, ref vector2s);
		Color32[] color32Array = this.buildColors(vector3s.Count);
		this.renderData.Vertices.AddRange(vector3s);
		this.renderData.Triangles.AddRange(nums);
		this.renderData.UV.AddRange(vector2s);
		this.renderData.Colors.AddRange(color32Array);
	}
}