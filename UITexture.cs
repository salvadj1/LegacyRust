using NGUI.Meshing;
using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Texture")]
[ExecuteInEditMode]
public class UITexture : UIWidget
{
	[HideInInspector]
	[SerializeField]
	private bool _mirrorY;

	[HideInInspector]
	[SerializeField]
	private bool _mirrorX;

	public bool mirrorX
	{
		get
		{
			return this._mirrorX;
		}
		set
		{
			if (this._mirrorX != value)
			{
				this._mirrorX = value;
				base.ChangedAuto();
			}
		}
	}

	public bool mirrorY
	{
		get
		{
			return this._mirrorY;
		}
		set
		{
			if (this._mirrorY != value)
			{
				this._mirrorY = value;
				base.ChangedAuto();
			}
		}
	}

	public UITexture() : base(UIWidget.WidgetFlags.KeepsMaterial)
	{
	}

	public override void MakePixelPerfect()
	{
		Texture texture = base.mainTexture;
		if (texture != null)
		{
			Vector3 vector3 = base.cachedTransform.localScale;
			vector3.x = (float)texture.width;
			vector3.y = (float)texture.height;
			vector3.z = 1f;
			base.cachedTransform.localScale = vector3;
		}
		base.MakePixelPerfect();
	}

	public override void OnFill(MeshBuffer m)
	{
		Vertex vertex = new Vertex();
		Vertex vertex1 = new Vertex();
		Vertex vertex2 = new Vertex();
		Vertex vertex3 = new Vertex();
		vertex.z = 0f;
		vertex1.z = 0f;
		vertex2.z = 0f;
		vertex3.z = 0f;
		Color color = base.color;
		float single = color.r;
		float single1 = single;
		vertex3.r = single;
		float single2 = single1;
		single1 = single2;
		vertex2.r = single2;
		float single3 = single1;
		single1 = single3;
		vertex1.r = single3;
		vertex.r = single1;
		float single4 = color.g;
		single1 = single4;
		vertex3.g = single4;
		float single5 = single1;
		single1 = single5;
		vertex2.g = single5;
		float single6 = single1;
		single1 = single6;
		vertex1.g = single6;
		vertex.g = single1;
		float single7 = color.b;
		single1 = single7;
		vertex3.b = single7;
		float single8 = single1;
		single1 = single8;
		vertex2.b = single8;
		float single9 = single1;
		single1 = single9;
		vertex1.b = single9;
		vertex.b = single1;
		float single10 = color.a;
		single1 = single10;
		vertex3.a = single10;
		float single11 = single1;
		single1 = single11;
		vertex2.a = single11;
		float single12 = single1;
		single1 = single12;
		vertex1.a = single12;
		vertex.a = single1;
		if (this._mirrorX)
		{
			if (!this._mirrorY)
			{
				vertex.x = 0.5f;
				vertex.y = 0f;
				vertex1.x = 0.5f;
				vertex1.y = -1f;
				vertex2.x = 0f;
				vertex2.y = -1f;
				vertex3.x = 0f;
				vertex3.y = 0f;
				vertex.u = 1f;
				vertex.v = 1f;
				vertex1.u = 1f;
				vertex1.v = 0f;
				vertex2.u = 0f;
				vertex2.v = 0f;
				vertex3.u = 0f;
				vertex3.v = 1f;
				m.TextureQuad(vertex, vertex1, vertex2, vertex3);
				vertex.x = 1f;
				vertex.y = 0f;
				vertex1.x = 1f;
				vertex1.y = -1f;
				vertex2.x = 0.5f;
				vertex2.y = -1f;
				vertex3.x = 0.5f;
				vertex3.y = 0f;
				vertex.u = 0f;
				vertex.v = 1f;
				vertex1.u = 0f;
				vertex1.v = 0f;
				vertex2.u = 1f;
				vertex2.v = 0f;
				vertex3.u = 1f;
				vertex3.v = 1f;
				m.TextureQuad(vertex, vertex1, vertex2, vertex3);
			}
			else
			{
				vertex.x = 0.5f;
				vertex.y = -0.5f;
				vertex1.x = 0.5f;
				vertex1.y = -1f;
				vertex2.x = 0f;
				vertex2.y = -1f;
				vertex3.x = 0f;
				vertex3.y = -0.5f;
				vertex.u = 1f;
				vertex.v = 1f;
				vertex1.u = 1f;
				vertex1.v = 0f;
				vertex2.u = 0f;
				vertex2.v = 0f;
				vertex3.u = 0f;
				vertex3.v = 1f;
				m.TextureQuad(vertex, vertex1, vertex2, vertex3);
				vertex.x = 0.5f;
				vertex.y = 0f;
				vertex1.x = 0.5f;
				vertex1.y = -0.5f;
				vertex2.x = 0f;
				vertex2.y = -0.5f;
				vertex3.x = 0f;
				vertex3.y = 0f;
				vertex.u = 0f;
				vertex.v = 1f;
				vertex1.u = 0f;
				vertex1.v = 0f;
				vertex2.u = 1f;
				vertex2.v = 0f;
				vertex3.u = 1f;
				vertex3.v = 1f;
				m.TextureQuad(vertex, vertex1, vertex2, vertex3);
				vertex.x = 1f;
				vertex.y = -0.5f;
				vertex1.x = 1f;
				vertex1.y = -1f;
				vertex2.x = 0.5f;
				vertex2.y = -1f;
				vertex3.x = 0.5f;
				vertex3.y = -0.5f;
				vertex.u = 1f;
				vertex.v = 1f;
				vertex1.u = 1f;
				vertex1.v = 0f;
				vertex2.u = 0f;
				vertex2.v = 0f;
				vertex3.u = 0f;
				vertex3.v = 1f;
				m.TextureQuad(vertex, vertex1, vertex2, vertex3);
				vertex.x = 1f;
				vertex.y = 0f;
				vertex1.x = 1f;
				vertex1.y = -0.5f;
				vertex2.x = 0.5f;
				vertex2.y = -0.5f;
				vertex3.x = 0.5f;
				vertex3.y = 0f;
				vertex.u = 0f;
				vertex.v = 1f;
				vertex1.u = 0f;
				vertex1.v = 0f;
				vertex2.u = 1f;
				vertex2.v = 0f;
				vertex3.u = 1f;
				vertex3.v = 1f;
				m.TextureQuad(vertex, vertex1, vertex2, vertex3);
			}
		}
		else if (!this._mirrorY)
		{
			vertex.x = 1f;
			vertex.y = 0f;
			vertex1.x = 1f;
			vertex1.y = -1f;
			vertex2.x = 0f;
			vertex2.y = -1f;
			vertex3.x = 0f;
			vertex3.y = 0f;
			vertex.u = 1f;
			vertex.v = 1f;
			vertex1.u = 1f;
			vertex1.v = 0f;
			vertex2.u = 0f;
			vertex2.v = 0f;
			vertex3.u = 0f;
			vertex3.v = 1f;
			m.TextureQuad(vertex, vertex1, vertex2, vertex3);
		}
		else
		{
			vertex.x = 1f;
			vertex.y = -0.5f;
			vertex1.x = 1f;
			vertex1.y = -1f;
			vertex2.x = 0f;
			vertex2.y = -1f;
			vertex3.x = 0f;
			vertex3.y = -0.5f;
			vertex.u = 1f;
			vertex.v = 0f;
			vertex1.u = 1f;
			vertex1.v = 1f;
			vertex2.u = 0f;
			vertex2.v = 1f;
			vertex3.u = 0f;
			vertex3.v = 0f;
			m.TextureQuad(vertex, vertex1, vertex2, vertex3);
			vertex.x = 1f;
			vertex.y = 0f;
			vertex1.x = 1f;
			vertex1.y = -0.5f;
			vertex2.x = 0f;
			vertex2.y = -0.5f;
			vertex3.x = 0f;
			vertex3.y = 0f;
			vertex.u = 1f;
			vertex.v = 1f;
			vertex1.u = 1f;
			vertex1.v = 0f;
			vertex2.u = 0f;
			vertex2.v = 0f;
			vertex3.u = 0f;
			vertex3.v = 1f;
			m.TextureQuad(vertex, vertex1, vertex2, vertex3);
		}
	}
}