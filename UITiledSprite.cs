using NGUI.Meshing;
using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite (Tiled)")]
[ExecuteInEditMode]
public class UITiledSprite : UIGeometricSprite
{
	public UITiledSprite() : base(0)
	{
	}

	public override void MakePixelPerfect()
	{
		Vector3 num = base.cachedTransform.localPosition;
		num.x = (float)Mathf.RoundToInt(num.x);
		num.y = (float)Mathf.RoundToInt(num.y);
		num.z = (float)Mathf.RoundToInt(num.z);
		base.cachedTransform.localPosition = num;
		Vector3 vector3 = base.cachedTransform.localScale;
		vector3.x = (float)Mathf.RoundToInt(vector3.x);
		vector3.y = (float)Mathf.RoundToInt(vector3.y);
		vector3.z = 1f;
		base.cachedTransform.localScale = vector3;
	}

	public override void OnFill(MeshBuffer m)
	{
		Vertex vertex = new Vertex();
		Vertex vertex1 = new Vertex();
		Vertex vertex2 = new Vertex();
		Vertex vertex3 = new Vertex();
		Texture texture = base.material.mainTexture;
		if (texture == null)
		{
			return;
		}
		Rect pixels = this.mInner;
		if (base.atlas.coordinates == UIAtlas.Coordinates.TexCoords)
		{
			pixels = NGUIMath.ConvertToPixels(pixels, texture.width, texture.height, true);
		}
		Vector2 vector2 = base.cachedTransform.localScale;
		float single = base.atlas.pixelSize;
		float single1 = Mathf.Abs(pixels.width / vector2.x) * single;
		float single2 = Mathf.Abs(pixels.height / vector2.y) * single;
		if (single1 < 0.01f || single2 < 0.01f)
		{
			Debug.LogWarning(string.Concat("The tiled sprite (", NGUITools.GetHierarchy(base.gameObject), ") is too small.\nConsider using a bigger one."));
			single1 = 0.01f;
			single2 = 0.01f;
		}
		Vector2 vector21 = new Vector2(pixels.xMin / (float)texture.width, pixels.yMin / (float)texture.height);
		Vector2 vector22 = new Vector2(pixels.xMax / (float)texture.width, pixels.yMax / (float)texture.height);
		Vector2 vector23 = vector22;
		float single3 = 0f;
		Color color = base.color;
		float single4 = color.r;
		float single5 = single4;
		vertex3.r = single4;
		float single6 = single5;
		single5 = single6;
		vertex2.r = single6;
		float single7 = single5;
		single5 = single7;
		vertex1.r = single7;
		vertex.r = single5;
		float single8 = color.g;
		single5 = single8;
		vertex3.g = single8;
		float single9 = single5;
		single5 = single9;
		vertex2.g = single9;
		float single10 = single5;
		single5 = single10;
		vertex1.g = single10;
		vertex.g = single5;
		float single11 = color.b;
		single5 = single11;
		vertex3.b = single11;
		float single12 = single5;
		single5 = single12;
		vertex2.b = single12;
		float single13 = single5;
		single5 = single13;
		vertex1.b = single13;
		vertex.b = single5;
		float single14 = color.a;
		single5 = single14;
		vertex3.a = single14;
		float single15 = single5;
		single5 = single15;
		vertex2.a = single15;
		float single16 = single5;
		single5 = single16;
		vertex1.a = single16;
		vertex.a = single5;
		float single17 = 0f;
		single5 = single17;
		vertex3.z = single17;
		float single18 = single5;
		single5 = single18;
		vertex2.z = single18;
		float single19 = single5;
		single5 = single19;
		vertex1.z = single19;
		vertex.z = single5;
		while (single3 < 1f)
		{
			float single20 = 0f;
			vector23.x = vector22.x;
			float single21 = single3 + single2;
			if (single21 > 1f)
			{
				vector23.y = vector21.y + (vector22.y - vector21.y) * (1f - single3) / (single21 - single3);
				single21 = 1f;
			}
			while (single20 < 1f)
			{
				float single22 = single20 + single1;
				if (single22 > 1f)
				{
					vector23.x = vector21.x + (vector22.x - vector21.x) * (1f - single20) / (single22 - single20);
					single22 = 1f;
				}
				vertex.x = single22;
				vertex.y = -single3;
				vertex1.x = single22;
				vertex1.y = -single21;
				vertex2.x = single20;
				vertex2.y = -single21;
				vertex3.x = single20;
				vertex3.y = -single3;
				vertex.u = vector23.x;
				vertex.v = 1f - vector21.y;
				vertex1.u = vector23.x;
				vertex1.v = 1f - vector23.y;
				vertex2.u = vector21.x;
				vertex2.v = 1f - vector23.y;
				vertex3.u = vector21.x;
				vertex3.v = 1f - vector21.y;
				m.Quad(vertex, vertex1, vertex2, vertex3);
				single20 = single20 + single1;
			}
			single3 = single3 + single2;
		}
	}
}