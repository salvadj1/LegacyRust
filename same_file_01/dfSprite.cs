using System;
using UnityEngine;

[AddComponentMenu("Daikon Forge/User Interface/Sprite/Basic")]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
[Serializable]
public class dfSprite : dfControl
{
	private static int[] TRIANGLE_INDICES;

	[SerializeField]
	protected dfAtlas atlas;

	[SerializeField]
	protected string spriteName;

	[SerializeField]
	protected dfSpriteFlip flip;

	[SerializeField]
	protected dfFillDirection fillDirection;

	[SerializeField]
	protected float fillAmount = 1f;

	[SerializeField]
	protected bool invertFill;

	private PropertyChangedEventHandler<string> SpriteNameChanged;

	public dfAtlas Atlas
	{
		get
		{
			if (this.atlas == null)
			{
				dfGUIManager manager = base.GetManager();
				if (manager != null)
				{
					dfAtlas defaultAtlas = manager.DefaultAtlas;
					dfAtlas dfAtla = defaultAtlas;
					this.atlas = defaultAtlas;
					return dfAtla;
				}
			}
			return this.atlas;
		}
		set
		{
			if (!dfAtlas.Equals(value, this.atlas))
			{
				this.atlas = value;
				this.Invalidate();
			}
		}
	}

	public float FillAmount
	{
		get
		{
			return this.fillAmount;
		}
		set
		{
			if (!Mathf.Approximately(value, this.fillAmount))
			{
				this.fillAmount = Mathf.Max(0f, Mathf.Min(1f, value));
				this.Invalidate();
			}
		}
	}

	public dfFillDirection FillDirection
	{
		get
		{
			return this.fillDirection;
		}
		set
		{
			if (value != this.fillDirection)
			{
				this.fillDirection = value;
				this.Invalidate();
			}
		}
	}

	public dfSpriteFlip Flip
	{
		get
		{
			return this.flip;
		}
		set
		{
			if (value != this.flip)
			{
				this.flip = value;
				this.Invalidate();
			}
		}
	}

	public bool InvertFill
	{
		get
		{
			return this.invertFill;
		}
		set
		{
			if (value != this.invertFill)
			{
				this.invertFill = value;
				this.Invalidate();
			}
		}
	}

	public dfAtlas.ItemInfo SpriteInfo
	{
		get
		{
			if (this.Atlas == null)
			{
				return null;
			}
			return this.Atlas[this.spriteName];
		}
	}

	public string SpriteName
	{
		get
		{
			return this.spriteName;
		}
		set
		{
			value = base.getLocalizedValue(value);
			if (value != this.spriteName)
			{
				this.spriteName = value;
				if (!Application.isPlaying)
				{
					dfAtlas.ItemInfo spriteInfo = this.SpriteInfo;
					if (this.size == Vector2.zero && spriteInfo != null)
					{
						this.size = spriteInfo.sizeInPixels;
						this.updateCollider();
					}
				}
				this.Invalidate();
				this.OnSpriteNameChanged(value);
			}
		}
	}

	static dfSprite()
	{
		dfSprite.TRIANGLE_INDICES = new int[] { 0, 1, 3, 3, 1, 2 };
	}

	public dfSprite()
	{
	}

	public override Vector2 CalculateMinimumSize()
	{
		dfAtlas.ItemInfo spriteInfo = this.SpriteInfo;
		if (spriteInfo == null)
		{
			return Vector2.zero;
		}
		RectOffset rectOffset = spriteInfo.border;
		if (rectOffset == null || rectOffset.horizontal <= 0 || rectOffset.vertical <= 0)
		{
			return base.CalculateMinimumSize();
		}
		return Vector2.Max(base.CalculateMinimumSize(), new Vector2((float)rectOffset.horizontal, (float)rectOffset.vertical));
	}

	private static void doFill(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		int num = options.baseIndex;
		dfList<Vector3> vertices = renderData.Vertices;
		dfList<Vector2> uV = renderData.UV;
		int num1 = num;
		int num2 = num + 1;
		int num3 = num + 3;
		int num4 = num + 2;
		if (options.invertFill)
		{
			if (options.fillDirection != dfFillDirection.Horizontal)
			{
				num1 = num + 3;
				num2 = num + 2;
				num3 = num;
				num4 = num + 1;
			}
			else
			{
				num1 = num + 1;
				num2 = num;
				num3 = num + 2;
				num4 = num + 3;
			}
		}
		if (options.fillDirection != dfFillDirection.Horizontal)
		{
			vertices[num3] = Vector3.Lerp(vertices[num3], vertices[num1], 1f - options.fillAmount);
			vertices[num4] = Vector3.Lerp(vertices[num4], vertices[num2], 1f - options.fillAmount);
			uV[num3] = Vector2.Lerp(uV[num3], uV[num1], 1f - options.fillAmount);
			uV[num4] = Vector2.Lerp(uV[num4], uV[num2], 1f - options.fillAmount);
		}
		else
		{
			vertices[num2] = Vector3.Lerp(vertices[num2], vertices[num1], 1f - options.fillAmount);
			vertices[num4] = Vector3.Lerp(vertices[num4], vertices[num3], 1f - options.fillAmount);
			uV[num2] = Vector2.Lerp(uV[num2], uV[num1], 1f - options.fillAmount);
			uV[num4] = Vector2.Lerp(uV[num4], uV[num3], 1f - options.fillAmount);
		}
	}

	protected internal override void OnLocalize()
	{
		base.OnLocalize();
		this.SpriteName = base.getLocalizedValue(this.spriteName);
	}

	protected override void OnRebuildRenderData()
	{
		if ((!(this.Atlas != null) || !(this.Atlas.Material != null) ? true : !base.IsVisible))
		{
			return;
		}
		if (this.SpriteInfo == null)
		{
			return;
		}
		this.renderData.Material = this.Atlas.Material;
		Color32 color32 = base.ApplyOpacity((!base.IsEnabled ? this.disabledColor : this.color));
		dfSprite.RenderOptions renderOption = new dfSprite.RenderOptions();
		dfSprite.RenderOptions atlas = renderOption;
		atlas.atlas = this.Atlas;
		atlas.color = color32;
		atlas.fillAmount = this.fillAmount;
		atlas.fillDirection = this.fillDirection;
		atlas.flip = this.flip;
		atlas.invertFill = this.invertFill;
		atlas.offset = this.pivot.TransformToUpperLeft(base.Size);
		atlas.pixelsToUnits = base.PixelsToUnits();
		atlas.size = base.Size;
		atlas.spriteInfo = this.SpriteInfo;
		renderOption = atlas;
		dfSprite.renderSprite(this.renderData, renderOption);
	}

	protected internal virtual void OnSpriteNameChanged(string value)
	{
		base.Signal("OnSpriteNameChanged", new object[] { value });
		if (this.SpriteNameChanged != null)
		{
			this.SpriteNameChanged(this, value);
		}
	}

	private static void rebuildColors(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		dfList<Color32> colors = renderData.Colors;
		colors.Add(options.color);
		colors.Add(options.color);
		colors.Add(options.color);
		colors.Add(options.color);
	}

	private static void rebuildTriangles(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		int num = options.baseIndex;
		dfList<int> triangles = renderData.Triangles;
		triangles.EnsureCapacity(triangles.Count + (int)dfSprite.TRIANGLE_INDICES.Length);
		for (int i = 0; i < (int)dfSprite.TRIANGLE_INDICES.Length; i++)
		{
			triangles.Add(num + dfSprite.TRIANGLE_INDICES[i]);
		}
	}

	private static void rebuildUV(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		Rect rect = options.spriteInfo.region;
		dfList<Vector2> uV = renderData.UV;
		uV.Add(new Vector2(rect.x, rect.yMax));
		uV.Add(new Vector2(rect.xMax, rect.yMax));
		uV.Add(new Vector2(rect.xMax, rect.y));
		uV.Add(new Vector2(rect.x, rect.y));
		Vector2 item = Vector2.zero;
		if (options.flip.IsSet(dfSpriteFlip.FlipHorizontal))
		{
			item = uV[1];
			uV[1] = uV[0];
			uV[0] = item;
			item = uV[3];
			uV[3] = uV[2];
			uV[2] = item;
		}
		if (options.flip.IsSet(dfSpriteFlip.FlipVertical))
		{
			item = uV[0];
			uV[0] = uV[3];
			uV[3] = item;
			item = uV[1];
			uV[1] = uV[2];
			uV[2] = item;
		}
	}

	private static void rebuildVertices(dfRenderData renderData, dfSprite.RenderOptions options)
	{
		dfList<Vector3> vertices = renderData.Vertices;
		int num = options.baseIndex;
		float single = 0f;
		float single1 = 0f;
		float single2 = Mathf.Ceil(options.size.x);
		float single3 = Mathf.Ceil(-options.size.y);
		vertices.Add(new Vector3(single, single1, 0f) * options.pixelsToUnits);
		vertices.Add(new Vector3(single2, single1, 0f) * options.pixelsToUnits);
		vertices.Add(new Vector3(single2, single3, 0f) * options.pixelsToUnits);
		vertices.Add(new Vector3(single, single3, 0f) * options.pixelsToUnits);
		Vector3 vector3 = options.offset.RoundToInt() * options.pixelsToUnits;
		for (int i = 0; i < 4; i++)
		{
			vertices[num + i] = (vertices[num + i] + vector3).Quantize(options.pixelsToUnits);
		}
	}

	internal static void renderSprite(dfRenderData data, dfSprite.RenderOptions options)
	{
		options.baseIndex = data.Vertices.Count;
		dfSprite.rebuildTriangles(data, options);
		dfSprite.rebuildVertices(data, options);
		dfSprite.rebuildUV(data, options);
		dfSprite.rebuildColors(data, options);
		if (options.fillAmount > -1f && options.fillAmount < 1f)
		{
			dfSprite.doFill(data, options);
		}
	}

	public override string ToString()
	{
		if (string.IsNullOrEmpty(this.spriteName))
		{
			return base.ToString();
		}
		return string.Format("{0} ({1})", base.name, this.spriteName);
	}

	public event PropertyChangedEventHandler<string> SpriteNameChanged
	{
		add
		{
			this.SpriteNameChanged += value;
		}
		remove
		{
			this.SpriteNameChanged -= value;
		}
	}

	internal struct RenderOptions
	{
		public dfAtlas atlas;

		public dfAtlas.ItemInfo spriteInfo;

		public Color32 color;

		public float pixelsToUnits;

		public Vector2 size;

		public dfSpriteFlip flip;

		public bool invertFill;

		public dfFillDirection fillDirection;

		public float fillAmount;

		public Vector3 offset;

		public int baseIndex;
	}
}