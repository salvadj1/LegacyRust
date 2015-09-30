using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class dfMarkupBoxTexture : dfMarkupBox
{
	private static int[] TRIANGLE_INDICES;

	private dfRenderData renderData = new dfRenderData(32);

	private Material material;

	public UnityEngine.Texture Texture
	{
		get;
		set;
	}

	static dfMarkupBoxTexture()
	{
		dfMarkupBoxTexture.TRIANGLE_INDICES = new int[] { 0, 1, 2, 0, 2, 3 };
	}

	public dfMarkupBoxTexture(dfMarkupElement element, dfMarkupDisplayType display, dfMarkupStyle style) : base(element, display, style)
	{
	}

	private static void addTriangleIndices(dfList<Vector3> verts, dfList<int> triangles)
	{
		int count = verts.Count;
		int[] tRIANGLEINDICES = dfMarkupBoxTexture.TRIANGLE_INDICES;
		for (int i = 0; i < (int)tRIANGLEINDICES.Length; i++)
		{
			triangles.Add(count + tRIANGLEINDICES[i]);
		}
	}

	private void ensureMaterial()
	{
		if (this.material != null || this.Texture == null)
		{
			return;
		}
		Shader shader = Shader.Find("Daikon Forge/Default UI Shader");
		if (shader == null)
		{
			Debug.LogError("Failed to find default shader");
			return;
		}
		Material material = new Material(shader)
		{
			name = "Default Texture Shader",
			hideFlags = HideFlags.DontSave,
			mainTexture = this.Texture
		};
		this.material = material;
	}

	internal void LoadTexture(UnityEngine.Texture texture)
	{
		if (texture == null)
		{
			throw new InvalidOperationException();
		}
		this.Texture = texture;
		this.Size = new Vector2((float)texture.width, (float)texture.height);
		this.Baseline = (int)this.Size.y;
	}

	protected override dfRenderData OnRebuildRenderData()
	{
		this.renderData.Clear();
		this.ensureMaterial();
		this.renderData.Material = this.material;
		this.renderData.Material.mainTexture = this.Texture;
		Vector3 vector3 = Vector3.zero;
		Vector3 size = vector3 + (Vector3.right * this.Size.x);
		Vector3 size1 = size + (Vector3.down * this.Size.y);
		Vector3 vector31 = vector3 + (Vector3.down * this.Size.y);
		this.renderData.Vertices.Add(vector3);
		this.renderData.Vertices.Add(size);
		this.renderData.Vertices.Add(size1);
		this.renderData.Vertices.Add(vector31);
		this.renderData.Triangles.AddRange(dfMarkupBoxTexture.TRIANGLE_INDICES);
		this.renderData.UV.Add(new Vector2(0f, 1f));
		this.renderData.UV.Add(new Vector2(1f, 1f));
		this.renderData.UV.Add(new Vector2(1f, 0f));
		this.renderData.UV.Add(new Vector2(0f, 0f));
		Color color = this.Style.Color;
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		return this.renderData;
	}
}